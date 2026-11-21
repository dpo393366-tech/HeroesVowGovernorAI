using System;using System.Linq;using System.Collections.Generic;using UnityEngine;
public sealed partial class StrategicSupport {
 static bool CampSupportPermission(Wo_DeployTeamCtrl t,MKMOJNAJKJE f)=>battleSupport.Value
  &&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_assembleCity??"",out var home)&&DistributionCity(home,out var owner)&&owner.Pointer==f.Pointer;
 static bool CampBattleNeed(Wo_DeployTeamCtrl t,NJHDEHLHENN war,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,out double eta,out int need,out int side){
  eta=double.PositiveInfinity;need=0;side=-1;
  if(war?.EMMJBPMNFAK==null||war.EMMJBPMNFAK.Length!=2||t.GetMorale()<40||t.GetWoundNum()>t.GetSoldierNum(false)/2||!CampSupportPermission(t,f))return false;
  side=war.EMMJBPMNFAK[0]==f.IBGCLCAIBOL?0:war.EMMJBPMNFAK[1]==f.IBGCLCAIBOL?1:-1;
  if(side<0||!War(f,war.EMMJBPMNFAK[1-side])||!teams.Any(x=>Same(x.m_warInfo,war)))return false;
  if(!ReliefPath(t.GetPosition(),war.GBHGENNMJCK,f,BattleTransitRegion(war,f),true,out _,out double length)||t.GetMoveSpeed()<=0)return false;
  eta=length/t.GetMoveSpeed();return CampBattleNeedAt(t,war,f,teams,eta,out need,out side);
 }
 static bool CampBattleNeedAt(Wo_DeployTeamCtrl t,NJHDEHLHENN war,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,double eta,out int need,out int side){
  need=0;side=WarSide(war,f.IBGCLCAIBOL);
  if(side<0||!War(f,war.EMMJBPMNFAK[1-side])||!CampSupportPermission(t,f)||!teams.Any(x=>Same(x.m_warInfo,war))||!double.IsFinite(eta)||eta>maxTravelSeconds.Value)return false;
  if(t.GetFood()<(long)Math.Max(1,t.GetOneDayFood())*(Math.Ceiling(eta/Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK()))+15))return false;
  var friends=new List<ReinforcementTimeline.Wave>();var enemies=new List<ReinforcementTimeline.Wave>();
  foreach(var x in teams)if(x.Pointer!=t.Pointer&&x.m_warInfo==null&&Same(x.m_targetWarInfo,war)){
   double arrival=TeamArrival(x,war.GBHGENNMJCK);if(!double.IsFinite(arrival))continue;
   if(x.m_belongPower==f.IBGCLCAIBOL)friends.Add(new(arrival,Math.Max(0,x.GetSoldierNum(false))));
   else if(x.m_belongPower==war.EMMJBPMNFAK[1-side])enemies.Add(new(arrival,Math.Max(0,x.GetSoldierNum(false))));
  }
  var loss=battles.TryGetValue(war.Pointer,out var state)&&state.Name==war.HIENOINGBCN?state.Loss[side]:null;
  need=ReinforcementTimeline.Need(Soldiers(war,side,IsDirect(war)),Soldiers(war,1-side,IsDirect(war)),loss?.FriendlyLoss??0,loss?.EnemyLoss??0,eta,eta+30,friends,enemies,battleCoverage.Value,0,out _,out _);
  return need>0;
 }
 static IEnumerable<string> CampBattleSteps(Wo_DeployTeamCtrl t,CampaignGoal goal,MKMOJNAJKJE f){
  if(!CampSupportPermission(t,f))yield break;
  var candidates=new List<(NJHDEHLHENN War,double Score)>();var teams=AttackTeams();var seen=new HashSet<IntPtr>();
  foreach(var member in teams){
   var war=member.m_warInfo;if(war==null||!seen.Add(war.Pointer))continue;
   if(!SameCamp(t,goal))yield break;
   if(CampBattleNeed(t,war,f,teams,out double eta,out int need,out _))candidates.Add((war,Math.Min(need,t.GetSoldierNum(false))/(1+eta)));
   yield return "campaign-battle-candidate";
  }
  foreach(var candidate in candidates.OrderByDescending(x=>x.Score)){
   if(!SameCamp(t,goal))yield break;
   var war=candidate.War;
   if(!CampBattleNeed(t,war,f,AttackTeams(),out double eta,out int need,out int side))continue;
   if(!MoveRelief(t,war.IJGLCFJOOIK,war,false,f,true)){Note($"campaign-support-block team={t.m_id} war={war.HIENOINGBCN} reason={reliefRouteFailure}");yield return "campaign-battle-retry";continue;}
   missions[t.Pointer]=new Mission{Team=t,Id=t.m_id,War=war,Name=war.HIENOINGBCN,Side=side,Faction=f.IBGCLCAIBOL,Source=t.m_assembleCity,Direct=IsDirect(war),ReadyAt=battleClock};
   campaignGoals.Remove(t.Pointer);
   Note($"campaign-decision team={t.m_id} faction={f.IBGCLCAIBOL} action=support-battle war={war.HIENOINGBCN} need={need} soldiers={t.GetSoldierNum(false)} etaSeconds={eta:F1} alreadyMobilized=true");
   yield return "campaign-battle-commit";yield break;
  }
 }
}
