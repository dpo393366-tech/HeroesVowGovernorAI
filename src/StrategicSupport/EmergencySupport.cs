using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 sealed class EmergencyBudget {public IntPtr City;public string Faction;public long Soldiers,Power;public readonly Dictionary<IntPtr,string> Wars=new();public readonly HashSet<int> Sent=new();}
 sealed class EmergencyContext {public AFAGFHJONFJ City;public NJHDEHLHENN War;public double Eta;public EmergencyBudget Budget;}
 static readonly Dictionary<string,EmergencyBudget> emergencyBudgets=new();
 [ThreadStatic] static EmergencyContext emergencyContext;
 static bool EmergencyFor(AFAGFHJONFJ c)=>emergencyContext!=null&&emergencyContext.City?.Pointer==c?.Pointer;
 static EmergencyBudget EmergencyBaseline(AFAGFHJONFJ c,NJHDEHLHENN w,List<Wo_DeployTeamCtrl> teams,long men,long power){
  string key=c.OJPECMNINNA;string owner=c.LMPMNLLGOEC.LMMNKIOAGLC;
  if(!emergencyBudgets.TryGetValue(key,out var b)||b.City!=c.Pointer||b.Faction!=owner||
    (!b.Wars.Any(p=>battles.TryGetValue(p.Key,out var s)&&s.Name==p.Value)&&!teams.Any(t=>t&&b.Sent.Contains(t.m_id)&&t.m_belongPower==owner)))
   emergencyBudgets[key]=b=new(){City=c.Pointer,Faction=owner};
  b.Soldiers=Math.Max(b.Soldiers,men);b.Power=Math.Max(b.Power,power);if(w!=null)b.Wars[w.Pointer]=w.HIENOINGBCN;return b;
 }
 static double EmergencyFraction(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,double eta){
  // Potential border armies raise retention, but do not impose an impossible
  // sum-of-all-neighbours veto. Actual incoming attacks still prohibit lending.
  var waves=CounterattackWaves(c,f,teams,out _);double first=waves.Count>0?waves.Min(w=>w.At):double.PositiveInfinity;
  double day=Math.Max(1,AOHIBKLOAEI.NIOKFCDLGFK());
  return first<=2*day?.6:first<=eta?.5:.35;
 }
 static Wo_DeployTeamCtrl GenerateMarchRelief(AFAGFHJONFJ source,AFAGFHJONFJ target,MKMOJNAJKJE f,int requested,double eta,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved){
  if(!StaffingDestination(source,f)||!StaffingDestination(target,f)||!DefenseThreat(target,f,teams)||SupportSourceRules(source,teams,reserved,true)!=null)return null;
  var previous=emergencyContext;
  try{emergencyContext=new(){City=source,Eta=eta};return GenerateRelief(source,requested,target,teams,reserved);}finally{emergencyContext=previous;}
 }
 static Wo_DeployTeamCtrl GenerateEmergencyRelief(AFAGFHJONFJ c,NJHDEHLHENN w,int requested,double eta,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved){
  if(w==null||SupportSourceRules(c,teams,reserved,true)!=null)return null;
  var previous=emergencyContext;
  try {emergencyContext=new(){City=c,War=w,Eta=eta};return GenerateRelief(c,requested,w.IJGLCFJOOIK,teams,reserved);}
  finally{emergencyContext=previous;}
 }
}
