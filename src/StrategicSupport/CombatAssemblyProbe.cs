using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 sealed class FailedCombatProbe {public string Signature;public double Day;}
 static readonly Dictionary<string,FailedCombatProbe> failedCombatProbes=new();
 // Raw unit power is ONLY a home-retention budget. It cannot be compared to
 // commander-adjusted enemy power. Admission needs some safe raw surplus;
 // the native assembled team's GetCombatVal is the authoritative fight test.
 static string CombatProbeSignature(AFAGFHJONFJ c,int required,List<Wo_DeployTeamCtrl> teams){
  var owner=c.LMPMNLLGOEC.LMMNKIOAGLC;var reserved=AttackReserved(teams);
  var units=new List<string>();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!reserved.Contains(a.Pointer))units.Add(a.Pointer+":"+a.FGHFOLKBHDL+":"+a.GNCKPEPJDGF()+":"+(int)a.GDHPLIFJCBN);
  units.Sort(StringComparer.Ordinal);
  return c.Pointer+"|"+owner+"|"+c.LMPMNLLGOEC.IPJHOOJBCKI+"|"+required+"|"+string.Join(",",units)+"|"+string.Join(",",HomeOfficers(c,teams).OrderBy(x=>x));
 }
 static bool CombatProbeReady(AFAGFHJONFJ c,string target,int required,List<Wo_DeployTeamCtrl> teams){
  string key=c.OJPECMNINNA+"/"+target;if(!failedCombatProbes.TryGetValue(key,out var old))return true;
  double day=NEINKOOBJIH.MIPFHCKDJOK;string signature=CombatProbeSignature(c,required,teams);
  if(day<old.Day||day-old.Day>=3*DecisionDays(c.LMPMNLLGOEC.LMMNKIOAGLC)||signature!=old.Signature){failedCombatProbes.Remove(key);return true;}
  if(Gate("combat-probe-wait:"+key,30))Note($"combat-probe-wait city={c.OJPECMNINNA} target={target} requiredAdjusted={required} reason=same-failed-roster retryDay={old.Day+3*DecisionDays(c.LMPMNLLGOEC.LMMNKIOAGLC)}");return false;
 }
 static void FailedProbe(AFAGFHJONFJ c,string target,int required){
  if(failedCombatProbes.Count>=1024)failedCombatProbes.Clear();
  failedCombatProbes[c.OJPECMNINNA+"/"+target]=new(){Signature=CombatProbeSignature(c,required,AttackTeams()),Day=NEINKOOBJIH.MIPFHCKDJOK};
 }
 static string HuntProbeKey(Wo_DeployTeamCtrl t)=>"raider:"+t.Pointer+":"+t.m_id;
}
