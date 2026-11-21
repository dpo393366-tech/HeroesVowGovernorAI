using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public sealed partial class StrategicSupport {
 // Native GenDivideTeam's argument is combat power, NOT soldier count. Its
 // internal 1.1 multiplier is bounded here; whole officer/troop groups move.
 static Wo_DeployTeamCtrl SplitFieldArmy(Wo_DeployTeamCtrl source,long maximumPower)=>SplitArmyCore(source,maximumPower,false);
 static Wo_DeployTeamCtrl SplitArmyCore(Wo_DeployTeamCtrl source,long maximumPower,bool worldBattle){
  if(!source||ManualProtected(source)||source.m_warInfo!=null&&(!worldBattle||IsDirect(source.m_warInfo))||source.m_isStarting||source.m_isPrivate||source.IncludePlayer()||source.m_listHero==null||source.m_listHero.Count<2||maximumPower<2)return null;
  var before=TeamTroops(source);long soldiers=source.GetSoldierNum(false),food=source.GetFood();
  int budget=(int)Math.Min(int.MaxValue,Math.Floor(maximumPower/1.100001));if(budget<1)return null;
  var child=source.GenDivideTeam(budget);if(!child)return null;
  var left=TeamTroops(source);var moved=TeamTroops(child);bool intact=left.Count>0&&moved.Count>0&&!left.Overlaps(moved);
  left.UnionWith(moved);intact&=left.SetEquals(before)&&source.GetSoldierNum(false)+(long)child.GetSoldierNum(false)==soldiers&&source.GetFood()+(long)child.GetFood()==food;
  if(!intact||child.GetCombatVal()>maximumPower){child.MergeToTeam(source);Note($"army-split-reverted source={source.m_id} reason={(intact?"native-budget":"conservation-check")}");return null;}
  // GetOne reuses native objects: GenDivideTeam copies ownership/troops but
  // does not clear old attack/merge/temporary orders. StartCamp also skips its
  // state change while the new child's visual model has not been created.
  // Normalize only this freshly split object, before any routing/eligibility.
  missions.Remove(child.Pointer);campaignGoals.Remove(child.Pointer);defenseOrders.Remove(child.Pointer);coverOrders.Remove(child.Pointer);capacityOrders.Remove(child.Pointer);reliefLoans.Remove(child.Pointer);fieldReliefOrders.Remove(child.Pointer);
  child.ClearTarget(false);child.m_attackTarget=null;child.m_mergeTarget=null;child.m_responseTeam=null;child.m_responseTeamAI=null;child.m_responseToPlayer=false;
  child.m_warInfo=null;child.m_targetWarInfo=null;child.m_targetCity="";child.m_tmpTargetCity="";child.m_targetCamp="";child.m_isEnterCity=false;child.m_isStarting=false;
  child.m_listMovePos?.Clear();child.GAANFLKOMKL?.Clear();child.SetDeployState(AKOPKFPKJPP.Camp);
  if(!child.gameObject.activeSelf)child.gameObject.SetActive(true);
  Note($"army-split source={source.m_id} child={child.m_id} retained={source.GetSoldierNum(false)} detached={child.GetSoldierNum(false)} unitsConserved=true foodConserved=true");return child;
 }
 static long HomeProtectionNeed(AFAGFHJONFJ city,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams){
  long danger=0;int units=0;var reserved=AttackReserved(teams);var counted=new HashSet<IntPtr>();
  foreach(var a in city.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!reserved.Contains(a.Pointer)&&a.FGHFOLKBHDL>0&&counted.Add(a.Pointer))units++;
  if(city.FKMKGNPFLFP!=null)foreach(var n in city.FKMKGNPFLFP)if(n?.LMPMNLLGOEC!=null&&War(f,n.LMPMNLLGOEC.LMMNKIOAGLC))danger+=ReserveCityPower(n,teams);
  double deadline=3*Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK());
  foreach(var t in teams)if(t&&War(f,t.m_belongPower)&&(t.m_targetCity==city.OJPECMNINNA||t.m_warInfo?.IJGLCFJOOIK?.OJPECMNINNA==city.OJPECMNINNA)){danger+=RawArmy(t);deadline=Math.Min(deadline,t.m_warInfo!=null?0:TeamArrival(t,city.BMBMKPGEHBH));}
  long home=ReserveCityPower(city,teams);int minimum=OccupationVulnerable(city)?Math.Max(2,minUnits.Value):minUnits.Value;
  // Credit only committed, timely arrivals, never hypothetical donor capacity.
  foreach(var t in teams)if(t&&t.m_belongPower==f.IBGCLCAIBOL&&t.m_warInfo==null&&t.m_isEnterCity&&t.m_targetCity==city.OJPECMNINNA&&t.m_deployState==AKOPKFPKJPP.Expedition&&!t.m_isStarting&&TeamArrival(t,city.BMBMKPGEHBH)<=deadline){
   var arriving=TeamTroops(t);arriving.ExceptWith(counted);if(arriving.Count==0)continue;home+=RawArmy(t);units+=arriving.Count;counted.UnionWith(arriving);
  }
  home+=TimelyConvoyDefense(city,f,deadline,counted,out int cargoUnits);units+=cargoUnits;
  long deficit=Math.Max(0,(long)Math.Ceiling(danger*Math.Round(coverage.Value,3))-home);
  return Math.Max(deficit,Math.Max(0,minimum-units)*(long)Math.Max(1000,EBNFFALGBPA.LLLBAAAEIBB));
 }
 static IEnumerable<string> AftermathHomeSteps(Wo_DeployTeamCtrl t,CampaignGoal goal,MKMOJNAJKJE f){
  if(goal.Returning)yield break;
  var ids=new HashSet<string>();if(!string.IsNullOrEmpty(t.m_assembleCity))ids.Add(t.m_assembleCity);
  if(t.m_listHero!=null)foreach(var h in t.m_listHero)if(!string.IsNullOrEmpty(h?.LMPMNLLGOEC?.CNOMNBCPJDI))ids.Add(h.LMPMNLLGOEC.CNOMNBCPJDI);
  var local=AOHIBKLOAEI.PNKKPMCJIGA(t.GetPosition());if(local?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&OccupationVulnerable(local))ids.Add(local.OJPECMNINNA);
  if(goal.CompletedOrder&&!string.IsNullOrEmpty(goal.Goal))ids.Add(goal.Goal);
  var needs=new List<(AFAGFHJONFJ City,long Power)>();
  foreach(var id in ids){
   yield return "aftermath-home-audit";if(!SameCamp(t,goal))yield break;
   if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(id,out var c)||c.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL)continue;
   var teams=AttackTeams();long need=HomeProtectionNeed(c,f,teams);if(need>0)needs.Add((c,need));
  }
  int dispatched=0;foreach(var item in needs.OrderByDescending(n=>n.Power)){
   yield return "aftermath-home-allocation";if(!SameCamp(t,goal))yield break;
   var c=item.City;var teams=AttackTeams();long need=HomeProtectionNeed(c,f,teams);if(need<=0)continue;
   int room=CapacityRoom(c,teams,t);if(room<=0||c.GKLIEHCHDLD()||!CampaignRoute(t,c,f,out int days)){goal.Returning=true;continue;}
   // Reserving home defense takes precedence over optional new conquest. If
   // a single commander cannot be divided, send that army home as a whole.
   int totalPower=t.GetCombatVal();long raw=Math.Max(1,RawArmy(t));long desired=Math.Min(totalPower,Math.Max(1,(long)Math.Ceiling(totalPower*Math.Min(1d,need/(double)raw))));
   Wo_DeployTeamCtrl child=null;
   for(int attempt=0;attempt<3&&desired>1;attempt++){
    child=SplitFieldArmy(t,desired);if(!child)break;
    if(TeamTroops(child).Count<=room&&DestinationFoodSafe(c,teams,child)&&CampaignRoute(child,c,f,out days)&&child.GetFood()>=child.GetOneDayFood()*(long)(days+5))break;
    child.MergeToTeam(t);child=null;desired/=2;
   }
   if(child){
    var order=new CampaignGoal{Id=child.m_id,Faction=f.IBGCLCAIBOL,Goal=c.OJPECMNINNA,Returning=true,CompletedOrder=goal.CompletedOrder};campaignGoals[child.Pointer]=order;
    if(CampaignMove(child,order,c,true,"restore-contributing-city-defense",days)){dispatched++;Note($"aftermath-allocation original={t.m_id} detachment={child.m_id} city={c.OJPECMNINNA} role={(local?.Pointer==c.Pointer?"occupation-garrison":"home-return")} soldiers={child.GetSoldierNum(false)}");}
    else {campaignGoals.Remove(child.Pointer);child.MergeToTeam(t);goal.Returning=true;}
   }else if(TeamTroops(t).Count<=room&&DestinationFoodSafe(c,teams,t)){
    goal.Returning=true;CampaignMove(t,goal,c,true,"home-defense-requires-undivided-army",days);yield break;
   }else goal.Returning=true;
   if(dispatched>=4){goal.Returning=true;break;}
  }
  // A small detachment may be all the destination can receive. Do not let
  // the main army invade again while any contributing home remains exposed.
  if(SameCamp(t,goal))foreach(var item in needs){yield return "aftermath-residual-defense";if(!SameCamp(t,goal))yield break;
   if(item.City.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&HomeProtectionNeed(item.City,f,AttackTeams())>0){goal.Returning=true;Note($"aftermath-hold-offense team={t.m_id} city={item.City.OJPECMNINNA} reason=residual-home-defense");break;}
  }
 }
 static IEnumerable<string> ReturnPartitionSteps(Wo_DeployTeamCtrl t,CampaignGoal goal,MKMOJNAJKJE f){
  var destinations=new List<(AFAGFHJONFJ City,double Score)>();
  var snapshot=AttackTeams();
  foreach(var city in ReturnCities(f)){
   yield return "return-partition-city";if(!SameCamp(t,goal))yield break;
   int room=CapacityRoom(city,snapshot,t);if(room<=0||city.GKLIEHCHDLD()||IncomingHostile(city,f))continue;
   var route=new RoutePlan();foreach(var phase in ReliefPathSteps(t.GetPosition(),city.BMBMKPGEHBH,f,null,true,route)){yield return "return-partition-"+phase;if(!SameCamp(t,goal))yield break;}
   if(route.Success)destinations.Add((city,route.Length)); // commit rechecks room and route

  }
  foreach(var item in destinations.OrderBy(x=>x.Score)){
   yield return "return-partition-order";if(!SameCamp(t,goal))yield break;
   var city=item.City;var teams=AttackTeams();int room=CapacityRoom(city,teams,t),count=TeamTroops(t).Count;
   if(room<=0||!CampaignRoute(t,city,f,out int days)||!ReturnSafe(t,city,f,teams,days,false))continue;
   if(count<=room){if(DestinationFoodSafe(city,teams,t)&&CampaignMove(t,goal,city,true,"distributed-return-remainder",days))yield break;continue;}
   long limit=Math.Max(2,(long)Math.Floor(t.GetCombatVal()*Math.Min(.9,room/(double)Math.Max(1,count))));
   Wo_DeployTeamCtrl child=null;
   for(int attempt=0;attempt<8&&limit>=2;attempt++){
    child=SplitFieldArmy(t,limit);if(!child)break;
    var fresh=AttackTeams();int free=CapacityRoom(city,fresh,child),selected=TeamTroops(child).Count;
    string failure=selected>free?"unit-capacity":!DestinationFoodSafe(city,fresh,child)?"destination-food":!CampaignRoute(child,city,f,out days)?"child-route:"+reliefRouteFailure:child.GetFood()<(long)child.GetOneDayFood()*(days+2)?"journey-food":null;
    if(failure==null)break;
    Note($"return-split-rejected original={t.m_id} child={child.m_id} city={city.OJPECMNINNA} reason={failure} units={selected} room={free} position={child.GetPosition()} state={child.m_deployState} food={child.GetFood()} days={days}");
    child.MergeToTeam(t);child=null;limit/=2;
    yield return "return-partition-retry";if(!SameCamp(t,goal))yield break;
   }
   if(!child)continue;
   var order=new CampaignGoal{Id=child.m_id,Faction=f.IBGCLCAIBOL,Goal=city.OJPECMNINNA,Returning=true,CompletedOrder=goal.CompletedOrder};campaignGoals[child.Pointer]=order;
   if(CampaignMove(child,order,city,true,"distributed-return-capacity-and-officers",days))Note($"return-partition original={t.m_id} child={child.m_id} city={city.OJPECMNINNA} units={TeamTroops(child).Count} officers={child.m_listHero?.Count} soldiers={child.GetSoldierNum(false)} tier={BorderTier(city,f)} reservedRoom={room}");
   else {Note($"return-split-rejected original={t.m_id} child={child.m_id} city={city.OJPECMNINNA} reason=commit-state-or-native-order state={child.m_deployState} camp={CampIdle(child)} controlled={CampaignControlled(child,out _)}");campaignGoals.Remove(child.Pointer);child.MergeToTeam(t);}
  }
 }

}

