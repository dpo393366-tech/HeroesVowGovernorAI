using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 sealed class FailedCombatProbe {public string Signature,Reason,TargetOwner,TargetCity,SourceOwner;public double Day;public int Actual,Required;public long RequiredMen;public AttackProbeCapacity Capacity;}
 static readonly Dictionary<string,FailedCombatProbe> failedCombatProbes=new();
 // Raw unit power is ONLY a home-retention budget. It cannot be compared to
 // commander-adjusted enemy power. Admission needs some safe raw surplus;
 // the native assembled team's GetCombatVal is the authoritative fight test.
 static string CombatProbeSignature(AFAGFHJONFJ c,int required,List<Wo_DeployTeamCtrl> teams){
  var owner=c.LMPMNLLGOEC.LMMNKIOAGLC;var reserved=AttackReserved(teams);
  var units=new List<string>();var seen=new HashSet<IntPtr>();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!reserved.Contains(a.Pointer)&&seen.Add(a.Pointer))units.Add(a.Pointer+":"+a.FGHFOLKBHDL+":"+a.GNCKPEPJDGF()+":"+(int)a.GDHPLIFJCBN);
  units.Sort(StringComparer.Ordinal);
  return c.Pointer+"|"+owner+"|"+c.LMPMNLLGOEC.IPJHOOJBCKI+"|"+required+"|"+string.Join(",",units)+"|"+string.Join(",",HomeOfficers(c,teams).OrderBy(x=>x));
 }
 static bool CombatProbeReady(AFAGFHJONFJ c,string target,int required,List<Wo_DeployTeamCtrl> teams){
  string key=c.OJPECMNINNA+"/"+target;if(!failedCombatProbes.TryGetValue(key,out var old))return true;
  bool cityTarget=target.StartsWith("city:",StringComparison.Ordinal);
  double day=NEINKOOBJIH.MIPFHCKDJOK;
  // Enemy estimates fluctuate daily. A still-insufficient roster must not be
  // regenerated merely because the required power changed by a few points.
  bool nowStrongEnough=old.Reason=="insufficient-attack-power"&&old.Actual>=required;
  bool ownerChanged=cityTarget&&old.TargetCity!=null&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(old.TargetCity,out var destination)&&(destination.LMPMNLLGOEC?.LMMNKIOAGLC??"")!=old.TargetOwner;
  if(cityTarget&&old.Reason=="insufficient-attack-manpower"&&old.Capacity!=null&&!ownerChanged&&day>=old.Day&&old.SourceOwner==c.LMPMNLLGOEC.LMMNKIOAGLC){
   var current=ReadAttackProbeCapacity(c,teams);
   long needed=MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(old.TargetCity,out var defender)?(long)Math.Ceiling(AttackStrength(defender,teams).Soldiers*.75):old.RequiredMen;
   if(needed>old.RequiredMen*.9&&current.Slots<=old.Capacity.Slots&&current.Raw<Math.Max(1,old.Capacity.Raw*1.1)){
    if(Gate("attack-manpower-wait:"+key,30))Note($"attack-manpower-wait city={c.OJPECMNINNA} target={old.TargetCity} requiredSoldiers={needed} reason=unchanged-insufficient-assembly noArmyCreated=true");return false;
   }
   failedCombatProbes.Remove(key);return true;
  }
  if(cityTarget&&old.Reason=="insufficient-attack-power"&&old.Capacity!=null&&!nowStrongEnough&&!ownerChanged&&day>=old.Day&&old.SourceOwner==c.LMPMNLLGOEC.LMMNKIOAGLC){
   var capacity=ReadAttackProbeCapacity(c,teams);double estimate=old.Actual*(old.Capacity.Raw>0?(double)capacity.Raw/old.Capacity.Raw:0);
   bool commanderImproved=capacity.Slots>old.Capacity.Slots||capacity.Quality>=old.Capacity.Quality+Math.Max(5,old.Capacity.Quality*.1);
   bool improved=capacity.Raw>0&&(old.Capacity.Raw<=0||commanderImproved||estimate>=required||old.Actual<=0&&capacity.Raw>old.Capacity.Raw*1.1);
   if(!improved){
    if(Gate("attack-power-wait:"+key,30))Note($"attack-power-wait city={c.OJPECMNINNA} target={old.TargetCity} lastActualAdjusted={old.Actual} requiredAdjusted={required} estimatedAdjusted={estimate:F0} commandSlots={capacity.Slots} usableRaw={capacity.Raw} reason=insufficient-capability-unchanged elapsedDays={day-old.Day:F1} noArmyCreated=true");
    return false;
   }
   failedCombatProbes.Remove(key);Note($"attack-power-recheck city={c.OJPECMNINNA} target={old.TargetCity} estimatedAdjusted={estimate:F0} requiredAdjusted={required} reason={(commanderImproved?"commander-improved":"usable-power-improved")} actualValidationRequired=true");return true;
  }
  string signature=CombatProbeSignature(c,cityTarget?0:required,teams);
  if(day<old.Day||day-old.Day>=3*DecisionDays(c.LMPMNLLGOEC.LMMNKIOAGLC)||signature!=old.Signature||nowStrongEnough||ownerChanged){failedCombatProbes.Remove(key);return true;}
  if(Gate("combat-probe-wait:"+key,30))Note($"combat-probe-wait city={c.OJPECMNINNA} target={target} requiredAdjusted={required} reason=same-failed-roster retryDay={old.Day+3*DecisionDays(c.LMPMNLLGOEC.LMMNKIOAGLC)}");return false;
 }
 static void FailedProbe(AFAGFHJONFJ c,string target,int required){
  if(failedCombatProbes.Count>=1024)failedCombatProbes.Clear();
  failedCombatProbes[c.OJPECMNINNA+"/"+target]=new(){Signature=CombatProbeSignature(c,target.StartsWith("city:",StringComparison.Ordinal)?0:required,AttackTeams()),Day=NEINKOOBJIH.MIPFHCKDJOK,Required=required};
 }
 static void FailedAttackProbe(AFAGFHJONFJ c,AFAGFHJONFJ target,int required,int actual,string reason){
  string key="city:"+target.Pointer;FailedProbe(c,key,required);
  var failed=failedCombatProbes[c.OJPECMNINNA+"/"+key];failed.Actual=actual;failed.Reason=reason;failed.TargetOwner=target.LMPMNLLGOEC?.LMMNKIOAGLC??"";failed.TargetCity=target.OJPECMNINNA;failed.SourceOwner=c.LMPMNLLGOEC.LMMNKIOAGLC;
  if(reason=="insufficient-attack-power"||reason=="insufficient-attack-manpower")failed.Capacity=ReadAttackProbeCapacity(c,AttackTeams());
  if(reason=="insufficient-attack-manpower")failed.RequiredMen=(long)Math.Ceiling(AttackStrength(target,AttackTeams()).Soldiers*.75);
  Note($"attack-dispatch-rejected source={c.OJPECMNINNA} target={target.OJPECMNINNA} reason={reason} actualAdjusted={actual} requiredAdjusted={required} retry={(failed.Capacity!=null?"capability-change":(failed.Day+3*DecisionDays(c.LMPMNLLGOEC.LMMNKIOAGLC)).ToString())} scope=this-city-this-target");
 }
 static string HuntProbeKey(Wo_DeployTeamCtrl t)=>"raider:"+t.Pointer+":"+t.m_id;
}
