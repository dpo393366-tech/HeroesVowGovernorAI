using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 static int WarSide(NJHDEHLHENN w,string faction)=>w?.EMMJBPMNFAK?.Length==2?(w.EMMJBPMNFAK[0]==faction?0:w.EMMJBPMNFAK[1]==faction?1:-1):-1;
 static FrontAllocationPolicy.Budget FrontBudget(NJHDEHLHENN war,int side,double horizon,List<Wo_DeployTeamCtrl> teams,double margin=1.2){
  if(war==null||side<0||side>1)return new();
  double own=Soldiers(war,side,IsDirect(war)),enemy=Soldiers(war,1-side,IsDirect(war));
  double op=0,ep=0;var arrivals=new List<FrontAllocationPolicy.Arrival>();
  foreach(var t in teams){if(!t)continue;
   if(Same(t.m_warInfo,war)){if(t.m_belongPower==war.EMMJBPMNFAK[side])op+=Math.Max(0,t.GetCombatVal());else if(t.m_belongPower==war.EMMJBPMNFAK[1-side])ep+=Math.Max(0,t.GetCombatVal());}
   else if(t.m_warInfo==null&&t.m_belongPower==war.EMMJBPMNFAK[1-side]&&(t.m_targetWarInfo!=null?Same(t.m_targetWarInfo,war):!string.IsNullOrEmpty(t.m_targetCity)&&t.m_targetCity==war.IJGLCFJOOIK?.OJPECMNINNA)){
    double at=TeamArrival(t,war.GBHGENNMJCK);if(double.IsFinite(at))arrivals.Add(new(at,Math.Max(0,t.GetSoldierNum(false)),Math.Max(0,t.GetCombatVal())));
   }
  }
  double loss=battles.TryGetValue(war.Pointer,out var state)&&state.Name==war.HIENOINGBCN?state.Loss[side].FriendlyLoss:0;
  return FrontAllocationPolicy.Available(own,enemy,op,ep,loss,horizon,arrivals,margin);
 }
 static double FrontSurplus(NJHDEHLHENN war,int side,double horizon)=>FrontBudget(war,side,horizon,AttackTeams()).Men;
 static void AccountFrontDetachment(NJHDEHLHENN origin,int side,Wo_DeployTeamCtrl child){
  int sent=child.GetSoldierNum(false);var detached=TeamTroops(child);
  if(origin.KJPPGIOCDID!=null&&origin.KJPPGIOCDID.Length>side&&origin.KJPPGIOCDID[side]!=null)foreach(var troop in origin.KJPPGIOCDID[side].ToArray())if(troop!=null&&detached.Contains(troop.Pointer))origin.KJPPGIOCDID[side].Remove(troop);
  if(origin.GPFFMNKOFCH!=null&&origin.GPFFMNKOFCH.Length>side&&origin.GPFFMNKOFCH[side]!=null)foreach(var troop in origin.GPFFMNKOFCH[side].ToArray())if(troop!=null&&detached.Contains(troop.Pointer))origin.GPFFMNKOFCH[side].Remove(troop);
  origin.FGHFOLKBHDL[side]=Math.Max(0,origin.FGHFOLKBHDL[side]-sent);
  if(origin.AOKBHBMECBD!=null&&origin.AOKBHBMECBD.Length>side)origin.AOKBHBMECBD[side]=Math.Max(origin.FGHFOLKBHDL[side],origin.AOKBHBMECBD[side]-sent);
  battles.Remove(origin.Pointer);
 }
 static IEnumerable<string> CrossFrontSteps(NJHDEHLHENN target,string faction){
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(faction,out var f)||f==null)yield break;
  var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;string name=target.HIENOINGBCN;
  bool Valid()=>CurrentFaction(f)&&world==MICGPAMNDKM.IJPKEMPIFIH?.Pointer&&target.HIENOINGBCN==name&&WarSide(target,faction)>=0&&AttackTeams().Any(t=>Same(t.m_warInfo,target));
  var ranked=new List<(Wo_DeployTeamCtrl Team,double Eta)>();
  foreach(var t in AttackTeams()){yield return "crossfront-route-rank";if(!Valid())yield break;
   if(!t||t.m_belongPower!=faction||WarSide(t.m_warInfo,faction)<0||Same(t.m_warInfo,target)||IsDirect(t.m_warInfo))continue;
   var route=new RoutePlan();foreach(var phase in ReliefPathSteps(t.GetPosition(),target.GBHGENNMJCK,f,BattleTransitRegion(target,f),true,route)){yield return phase;if(!Valid())yield break;}
   if(route.Success&&t.GetMoveSpeed()>0&&CampBattleNeedAt(t,target,f,AttackTeams(),route.Length/t.GetMoveSpeed(),out _,out _))ranked.Add((t,route.Length/t.GetMoveSpeed()));}
  foreach(var candidate in ranked.OrderBy(x=>x.Eta)){var parent=candidate.Team;
   yield return "crossfront-candidate";if(!Valid())yield break;
   var origin=parent.m_warInfo;int side=WarSide(origin,faction);
   if(ManualProtected(parent)||side<0||Same(origin,target)||IsDirect(origin)||parent.m_belongPower!=faction||parent.m_isPrivate||parent.IncludePlayer()||parent.m_listHero?.Count<2||!CampSupportPermission(parent,f))continue;
   // Real-time battlefield objects cannot be removed through the world-map
   // roster transaction. Existing camp/arrival redirection still applies there.
   var teams=AttackTeams();if(!CampBattleNeed(parent,target,f,teams,out double eta,out int need,out int targetSide))continue;
   var allocation=FrontBudget(origin,side,eta+30,teams);double available=allocation.Men;if(available<1000)continue;
   double powerLimit=allocation.Power;
   int soldiers=parent.GetSoldierNum(false);if(soldiers<=0)continue;
   long budget=(long)Math.Min(powerLimit,parent.GetCombatVal()*Math.Min(available,need)/soldiers);if(budget<1)continue;
   // No yield from the final forecasts through split, route registration and
   // battle accounting: no other AI can spend this surplus in between.
   if(!Valid()||!Same(parent.m_warInfo,origin)||IsDirect(origin))continue;
   Wo_DeployTeamCtrl child=null;
   for(int attempt=0;attempt<3&&budget>1;attempt++){
    child=SplitArmyCore(parent,budget,true);if(!child)break;
    if(child.GetSoldierNum(false)<=available&&child.GetCombatVal()<=powerLimit&&CampBattleNeed(child,target,f,AttackTeams(),out eta,out need,out targetSide)&&child.GetFood()>=Math.Max(1,child.GetOneDayFood())*(Math.Ceiling(eta/Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK()))+15))break;
    child.MergeToTeam(parent);child=null;budget/=2;
   }
   if(!child)continue;
   if(!MoveRelief(child,target.IJGLCFJOOIK,target,false,f,true)){child.MergeToTeam(parent);continue;}
   int sent=child.GetSoldierNum(false);AccountFrontDetachment(origin,side,child);
   missions[child.Pointer]=new Mission{Team=child,Id=child.m_id,War=target,Name=name,Side=targetSide,Faction=faction,Source=parent.m_assembleCity,Direct=IsDirect(target),ReadyAt=battleClock};
   Note($"crossfront-relief sourceWar={origin.HIENOINGBCN} targetWar={name} parent={parent.m_id} child={child.m_id} sent={sent} etaSeconds={eta:F1} retained={origin.FGHFOLKBHDL[side]} enemy={Soldiers(origin,1-side,false)} soldierConservation=true");
   yield return "crossfront-commit";
  }
 }
}
