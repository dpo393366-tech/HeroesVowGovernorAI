using System;using System.Collections.Generic;using UnityEngine;
public sealed partial class StrategicSupport {
 static bool MarchAttackCandidate(Wo_DeployTeamCtrl t,out AFAGFHJONFJ target){
  target=null;
  return t&&t.m_deployState==AKOPKFPKJPP.Expedition&&!t.m_isEnterCity&&t.m_warInfo==null&&t.m_targetWarInfo==null&&!OtherCampaignOrder(t)
   &&!missions.ContainsKey(t.Pointer)&&!fieldReliefOrders.ContainsKey(t.Pointer)&&!huntOrders.ContainsKey(t.Pointer)&&!capacityOrders.ContainsKey(t.Pointer)
   &&CampaignControlled(t,out var f)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_targetCity??"",out target)&&target?.LMPMNLLGOEC!=null
   &&!string.IsNullOrEmpty(target.LMPMNLLGOEC.LMMNKIOAGLC)&&War(f,target.LMPMNLLGOEC.LMMNKIOAGLC)&&!target.GKLIEHCHDLD();
 }
 // Use existing complete movement corners only. This periodic strength check
 // never starts a path search and never credits a guessed straight shortcut.
 static double AttackMarchEta(Wo_DeployTeamCtrl t,AFAGFHJONFJ target){
  var path=t.m_listMovePos;if(path==null||path.Count==0||t.GetMoveSpeed()<=0||Vector3.Distance(path[path.Count-1],target.BMBMKPGEHBH)>1)return double.PositiveInfinity;
  double length=0;var p=t.GetPosition();foreach(var q in path){length+=Vector3.Distance(p,q);p=q;}
  return length/t.GetMoveSpeed();
 }
 static void ScheduleAttackMarch(Wo_DeployTeamCtrl t){
  if(!MarchAttackCandidate(t,out var target))return;
  string key="offense-march:"+t.m_id;
  if(!DecisionDue(key,t.m_belongPower,t.Pointer,target.OJPECMNINNA,false))return;
  int id=t.m_id;string goal=target.OJPECMNINNA;
  work.Add("attack-road-warm:"+t.m_belongPower+":"+goal,()=>WarmAttackRoads(t,id,goal).GetEnumerator());
  if(work.Add(key,()=>CheckAttackMarch(t,id,goal).GetEnumerator()))DecisionDue(key,t.m_belongPower,t.Pointer,goal,true);
 }
 static IEnumerable<string> CheckAttackMarch(Wo_DeployTeamCtrl t,int id,string goal){
  yield return "offense-march-strength";
  if(!LiveTeam(t)||t.m_id!=id||!MarchAttackCandidate(t,out var target)||target.OJPECMNINNA!=goal)yield break;
  var teams=AttackTeams();long available=Math.Max(0,t.GetCombatVal());long men=Math.Max(0,t.GetSoldierNum(false));double eta=AttackMarchEta(t,target);int companions=0;
  var seen=new HashSet<IntPtr>{t.Pointer};
  foreach(var friend in teams){
   if(!friend||!seen.Add(friend.Pointer)||!InterceptProtectedFaction(t.m_belongPower,friend.m_belongPower)||friend.m_isEnterCity||friend.m_deployState!=AKOPKFPKJPP.Expedition||friend.m_targetCity!=goal||friend.m_warInfo!=null||friend.m_targetWarInfo!=null||OtherCampaignOrder(friend))continue;
   double arrival=AttackMarchEta(friend,target);
   if(double.IsFinite(eta)&&double.IsFinite(arrival)&&arrival<=eta){available+=Math.Max(0,friend.GetCombatVal());men+=Math.Max(0,friend.GetSoldierNum(false));companions++;}
  }
  var resistance=AttackStrength(target,teams);int required=resistance.Required;
  if(Gate("offense-march-audit:"+id,15))Note($"offense-march-assess team={id} target={goal} ownAdjusted={t.GetCombatVal()} timelyCompanions={companions} combinedAdjusted={available} requiredAdjusted={required} ownSoldiers={men} defenderSoldiers={resistance.Soldiers} extraPathSearch=false");
  // Small shifts do not turn an otherwise adequate army around every day.
  // Withdraw only when it loses the underlying parity, not the safety margin.
  double parity=required/(Math.Round(attackAdvantage.Value,3)+NativeWarFrequencyMargin());
  if(available>=parity&&men>=Math.Ceiling(resistance.Soldiers*.5))yield break;
  RememberCampaign(t);if(!campaignGoals.TryGetValue(t.Pointer,out var s)||s.Id!=id)yield break;
  s.Returning=true;s.Ordered=false;s.Staging=false;s.CheckedDay=-1;
  t.ClearTarget(false);t.m_targetCity="";t.m_tmpTargetCity="";t.m_listMovePos?.Clear();t.GAANFLKOMKL?.Clear();t.StartCamp();
  work.Add("campaign:"+id,()=>RecoverCampaign(t,s).GetEnumerator());
  Note($"offense-march-withdraw team={id} target={goal} combinedAdjusted={available} requiredAdjusted={required} reason=defender-strength-changed action=physical-return noInstantDismiss=true");
 }
}
