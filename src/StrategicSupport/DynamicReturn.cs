using System;using System.Collections.Generic;using System.Linq;using UnityEngine;
public sealed partial class StrategicSupport {
 static bool ReturnGroupFits(Wo_DeployTeamCtrl t,int room){
  if(room<=0)return false;if(TeamTroops(t).Count<=room)return true;
  if(t.m_listHero==null||t.m_listHero.Count<2||t.m_dicDeployArmy==null)return false;
  foreach(var g in t.m_dicDeployArmy)if(g.Value!=null){var units=new HashSet<IntPtr>();foreach(var a in g.Value)if(a!=null)units.Add(a.Pointer);if(units.Count>0&&units.Count<=room)return true;}return false;
 }
 static bool ReturningMarch(Wo_DeployTeamCtrl t,CampaignGoal s)=>t&&s.Returning&&s.Id==t.m_id&&s.Faction==t.m_belongPower&&t.m_deployState==AKOPKFPKJPP.Expedition&&((t.m_listMovePos?.Count??0)+(t.GAANFLKOMKL?.Count??0)>0)&&!OtherCampaignOrder(t)&&t.m_warInfo==null&&t.m_targetWarInfo==null&&CampaignControlled(t,out _)&&campaignGoals.TryGetValue(t.Pointer,out var current)&&ReferenceEquals(s,current);
 static void ScheduleReturnWatch(Wo_DeployTeamCtrl t){
  if(!t||t.m_deployState!=AKOPKFPKJPP.Expedition||!CampaignControlled(t,out _)||OtherCampaignOrder(t)||t.m_warInfo!=null||t.m_targetWarInfo!=null)return;
  RememberCampaign(t);if(!campaignGoals.TryGetValue(t.Pointer,out var s)||!ReturningMarch(t,s)||!AIGate("return-watch:"+t.m_id,t.m_belongPower,2))return;
  work.Add("campaign-return:"+t.m_id,()=>SafePhases(ReturnWatchSteps(t,s)).GetEnumerator());
 }
 static bool CampaignRouteDistance(Wo_DeployTeamCtrl t,AFAGFHJONFJ target,MKMOJNAJKJE f,out int days,out double length){
  days=0;length=double.PositiveInfinity;float speed=t.GetMoveSpeed(),seconds=AOHIBKLOAEI.NIOKFCDLGFK();
  if(speed<=0||seconds<=0||!ReliefPath(t.GetPosition(),target.BMBMKPGEHBH,f,target.OJPECMNINNA,true,out _,out length))return false;
  double duration=Math.Ceiling(length/speed/seconds);if(!double.IsFinite(duration)||duration>90)return false;days=Math.Max(1,(int)duration);return true;
 }
 static IEnumerable<string> ReturnWatchSteps(Wo_DeployTeamCtrl t,CampaignGoal s){
  if(!ReturningMarch(t,s)||!CampaignControlled(t,out var f))yield break;
  var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;string previous=t.m_targetCity;var start=t.GetPosition();
  string committed=s.Staging?s.OrderTarget:previous;
  bool Valid()=>Ready()&&ReturningMarch(t,s)&&CurrentFaction(f)&&world==MICGPAMNDKM.IJPKEMPIFIH?.Pointer&&t.m_targetCity==previous;
  var roads=new NearestReturnRoads(start,f);
  // Rank real roads, not population/officer quotas or an empty city's priority.
  // Capacity and the entire route are checked again immediately before commit.
  while(Valid()){
   foreach(var phase in roads.Next(Valid,c=>ReturnGroupFits(t,CapacityRoom(c,AttackTeams(),t))&&SafeLogisticsEndpoint(c,f)))yield return "return-live-"+phase;
   if(roads.City==null)break;
   yield return "return-live-choice";if(!Valid())yield break;
   var city=roads.City;var teams=AttackTeams();int room=CapacityRoom(city,teams,t),units=TeamTroops(t).Count;
   if(!ReturnGroupFits(t,room)||!CampaignRouteDistance(t,city,f,out int days,out double distance)||!ReturnSafe(t,city,f,teams,days,false))continue;
   if(room>=units&&!DestinationFoodSafe(city,teams,t))continue;
   // A moving origin can change while the yielded scan runs. Keep a still
   // valid current order when its fresh real road is already shorter.
   if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(committed??"",out var current)&&ReturnSafe(t,current,f,teams,0,true)&&DestinationFoodSafe(current,teams,t)&&CampaignRouteDistance(t,current,f,out int oldDays,out double oldDistance)&&ReturnSafe(t,current,f,teams,oldDays,true)){
    if(current.Pointer==city.Pointer||oldDistance-distance<=Math.Max(t.GetMoveSpeed(),oldDistance*.1))yield break;
   }
   if(room<units){
    // Keep a valid staging journey intact. The ordinary arrival/day recovery
    // retries actual partitioning; don't cancel it every two seconds merely
    // because it was intentionally too large to enter as a single army.
    if(s.Staging&&s.Ordered&&committed==city.OJPECMNINNA&&ReturnSafe(t,city,f,teams,days,false))yield break;
    t.ClearTarget(false);t.StartCamp();s.Ordered=false;s.Staging=false;
    Note($"return-live-replan team={t.m_id} previous={previous} nearest={city.OJPECMNINNA} room={room} units={units} road={distance:F2} action=split-nearest-compatible-groups");
    work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,s).GetEnumerator());yield break;
   }
   if(!MoveRelief(t,city,null,true,f,true))continue;
   s.Goal=city.OJPECMNINNA;s.OrderTarget=city.OJPECMNINNA;s.Ordered=true;s.Staging=false;s.Position=t.GetPosition();s.OrderedDay=NEINKOOBJIH.MIPFHCKDJOK;s.MovingLogged=false;
   Note($"return-live-replan team={t.m_id} previous={previous} target={city.OJPECMNINNA} room={room} units={units} road={distance:F2} etaDays={days} action=nearest-safe-city capacityIncludesInbound=true");yield break;
  }
  if(!Valid())yield break;
  var fresh=AttackTeams();
  if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(committed??"",out var old)&&CampaignRoute(t,old,f,out int remaining)&&ReturnSafe(t,old,f,fresh,remaining,!s.Staging)&&(s.Staging||DestinationFoodSafe(old,fresh,t)))yield break;
  t.ClearTarget(false);t.StartCamp();s.Ordered=false;s.Staging=false;
  Note($"return-live-replan team={t.m_id} previous={previous} action=reassess-split-or-safe-staging reason=no-longer-admissible");
  work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,s).GetEnumerator());
 }
}
