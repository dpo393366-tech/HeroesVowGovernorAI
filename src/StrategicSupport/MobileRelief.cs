using System;using System.Linq;using System.Collections.Generic;using UnityEngine;
public sealed partial class StrategicSupport {
 sealed class FieldReliefOrder {public Wo_DeployTeamCtrl Team,Enemy,Protected;public int Id,EnemyId,ProtectedId;public string Faction;public double Day,Eta;public bool AwaitingDeparture;}
 static readonly Dictionary<IntPtr,FieldReliefOrder> fieldReliefOrders=new();
 static bool LiveArmy(Wo_DeployTeamCtrl t)=>t&&t.gameObject.activeSelf&&ICGAEMLACJG.HFJIECIBFKB.TryGetValue(t.m_id,out var current)&&current&&current.Pointer==t.Pointer;
 static bool FieldThreat(Wo_DeployTeamCtrl enemy,MKMOJNAJKJE f,out Wo_DeployTeamCtrl victim){
  victim=enemy?.m_attackTarget;
  return LiveArmy(enemy)&&LiveArmy(victim)&&enemy.m_warInfo==null&&victim.m_warInfo==null&&victim.m_belongPower==f.IBGCLCAIBOL&&War(f,enemy.m_belongPower);
 }
 static int FieldNeed(Wo_DeployTeamCtrl enemy,Wo_DeployTeamCtrl victim,MKMOJNAJKJE f,double eta,Wo_DeployTeamCtrl exclude=null){
  var teams=AttackTeams();double hostile=0,friendly=Math.Max(0,victim.GetCombatVal());
  foreach(var t in teams){if(!t||t.Pointer==exclude?.Pointer||t.Pointer==victim.Pointer||t.m_warInfo!=null)continue;
   if(War(f,t.m_belongPower)&&t.m_attackTarget?.Pointer==victim.Pointer)hostile+=Math.Max(0,t.GetCombatVal());
   else if(t.m_belongPower==f.IBGCLCAIBOL&&t.m_attackTarget?.Pointer==enemy.Pointer){
    double arrival=TeamArrival(t,enemy.GetPosition());
    if(fieldReliefOrders.TryGetValue(t.Pointer,out var order)&&order.Id==t.m_id&&order.EnemyId==enemy.m_id&&t.m_deployState==AKOPKFPKJPP.Assemble){double age=Math.Max(0,NEINKOOBJIH.MIPFHCKDJOK-order.Day)*Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK());if(age<=Math.Max(30,mobilizeSeconds.Value*3))arrival=Math.Max(0,order.Eta-age);}
    if(arrival<=eta+10)friendly+=Math.Max(0,t.GetCombatVal());
   }
  }
  return (int)Math.Min(int.MaxValue,Math.Max(0,Math.Ceiling(hostile*Math.Max(1.15,coverage.Value)-friendly)));
 }
 static bool MobileCandidate(Wo_DeployTeamCtrl t,MKMOJNAJKJE f)=>LiveArmy(t)&&!ManualProtected(t)&&t.m_belongPower==f.IBGCLCAIBOL&&!t.m_isPrivate&&!t.IncludePlayer()&&!t.m_responseToPlayer&&!t.m_isStarting&&t.m_deployState!=AKOPKFPKJPP.Assemble&&!t.m_attackTarget&&!t.m_mergeTarget&&t.m_targetVillage==null&&t.m_targetPass==null&&string.IsNullOrEmpty(t.m_targetCamp)&&!(defenseOrders.TryGetValue(t.Pointer,out var defense)&&SameDefense(defense))&&!(coverOrders.TryGetValue(t.Pointer,out var cover)&&ValidCover(cover))&&CampSupportPermission(t,f)&&t.GetMorale()>=40&&t.GetWoundNum()<=t.GetSoldierNum(false)/2;
 static bool SpareMarch(Wo_DeployTeamCtrl t,MKMOJNAJKJE f){
  var war=t.m_targetWarInfo;int side=WarSide(war,f.IBGCLCAIBOL);if(side<0||t.m_warInfo!=null)return false;
  var budget=FrontBudget(war,side,30,AttackTeams(),1.3);
  // Already present forces must suffice without this army or any other promised
  // allied arrival; actual enemy arrivals and own observed attrition count.
  return budget.Men>=1000&&budget.Power>0;
 }
 static bool InstallFieldRelief(Wo_DeployTeamCtrl t,Wo_DeployTeamCtrl enemy,Wo_DeployTeamCtrl victim,MKMOJNAJKJE f){
  if(!FieldThreat(enemy,f,out var current)||current.Pointer!=victim.Pointer||t.m_warInfo!=null)return false;
  string region=AOHIBKLOAEI.PNKKPMCJIGA(enemy.GetPosition())?.OJPECMNINNA;
  if(!ReliefPath(t.GetPosition(),enemy.GetPosition(),f,region,true,out var route,out double length)||route.Count==0||t.GetMoveSpeed()<=0)return false;
  double eta=length/t.GetMoveSpeed();if(eta>maxTravelSeconds.Value||t.GetFood()<Math.Max(1,t.GetOneDayFood())*(Math.Ceiling(eta/Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK()))+15))return false;
  var oldWar=t.m_targetWarInfo;
  t.SetAttackTarget(enemy,true);
  if(t.m_deployState!=AKOPKFPKJPP.Expedition||t.m_listMovePos==null){if(oldWar!=null)MoveRelief(t,oldWar.IJGLCFJOOIK,oldWar,false,f,true);return false;}
  t.GAANFLKOMKL?.Clear();t.m_listMovePos.Clear();foreach(var point in route)t.m_listMovePos.Add(point);t.m_isEnterCity=false;
  missions.Remove(t.Pointer);campaignGoals.Remove(t.Pointer);defenseOrders.Remove(t.Pointer);coverOrders.Remove(t.Pointer);reliefLoans.Remove(t.Pointer);
  fieldReliefOrders[t.Pointer]=new(){Team=t,Enemy=enemy,Protected=victim,Id=t.m_id,EnemyId=enemy.m_id,ProtectedId=victim.m_id,Faction=f.IBGCLCAIBOL,Day=NEINKOOBJIH.MIPFHCKDJOK,Eta=eta};
  Note($"field-relief-commit team={t.m_id} enemy={enemy.m_id} protected={victim.m_id} source={t.m_assembleCity} priorWar={oldWar?.HIENOINGBCN} soldiers={t.GetSoldierNum(false)} eta={eta:F1} reason=army-pursuit-threat actualRoad=true");return true;
 }
 static IEnumerable<string> MobileReliefSteps(MKMOJNAJKJE f){
  if(!Ready()||!CurrentFaction(f)||!battleSupport.Value)yield break;
  var threats=new List<Wo_DeployTeamCtrl>();foreach(var enemy in AttackTeams()){yield return "field-threat-scan";if(!CurrentFaction(f))yield break;if(FieldThreat(enemy,f,out _))threats.Add(enemy);}
  foreach(var enemy in threats){
   if(!FieldThreat(enemy,f,out var victim))continue;
   var candidates=new List<(Wo_DeployTeamCtrl Team,AFAGFHJONFJ City,double Eta)>();
   foreach(var t in AttackTeams()){
    yield return "field-mobile-route";if(!CurrentFaction(f)||!FieldThreat(enemy,f,out victim))break;
    if(t.Pointer==victim.Pointer||!MobileCandidate(t,f)||fieldReliefOrders.ContainsKey(t.Pointer))continue;
    if(t.m_warInfo!=null&&IsDirect(t.m_warInfo))continue;
    if(t.m_warInfo==null&&t.m_targetWarInfo!=null&&!SpareMarch(t,f))continue;
    if(t.m_warInfo==null&&t.m_targetWarInfo==null&&!CampIdle(t)&&!campaignGoals.ContainsKey(t.Pointer))continue;
    var route=new RoutePlan();foreach(var phase in ReliefPathSteps(t.GetPosition(),enemy.GetPosition(),f,AOHIBKLOAEI.PNKKPMCJIGA(enemy.GetPosition())?.OJPECMNINNA,true,route))yield return phase;
    if(route.Success&&t.GetMoveSpeed()>0)candidates.Add((t,null,route.Length/t.GetMoveSpeed()));
   }
   foreach(var c in ProtectedCities(f)){
    yield return "field-city-route";if(!CurrentFaction(f)||!FieldThreat(enemy,f,out victim))break;
    var teams=AttackTeams();if(SupportSourceRules(c,teams,AttackReserved(teams),true)!=null)continue;
    var route=new RoutePlan();foreach(var phase in ReliefPathSteps(c.BMBMKPGEHBH,enemy.GetPosition(),f,AOHIBKLOAEI.PNKKPMCJIGA(enemy.GetPosition())?.OJPECMNINNA,true,route))yield return phase;
    float speed=Wo_DeployTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL,false);if(route.Success&&speed>0)candidates.Add((null,c,route.Length/speed+mobilizeSeconds.Value));
   }
   foreach(var candidate in candidates.OrderBy(x=>x.Eta)){
    yield return "field-allocation";if(!CurrentFaction(f)||!FieldThreat(enemy,f,out victim))break;
    if(candidate.Eta>maxTravelSeconds.Value)continue;int need=FieldNeed(enemy,victim,f,candidate.Eta);if(need<=0)break;
    var t=candidate.Team;var origin=t?.m_warInfo;var child=(Wo_DeployTeamCtrl)null;
    if(t){
     if(!MobileCandidate(t,f)||t.m_targetWarInfo!=null&&!SpareMarch(t,f))continue;
     if(origin!=null){int side=WarSide(origin,f.IBGCLCAIBOL);if(side<0||IsDirect(origin))continue;
      var budget=FrontBudget(origin,side,candidate.Eta+30,AttackTeams());long power=(long)Math.Min(budget.Power,t.GetCombatVal()*Math.Min(budget.Men,Math.Max(1,t.GetSoldierNum(false)))/Math.Max(1,t.GetSoldierNum(false)));
      child=SplitArmyCore(t,Math.Min(power,(long)Math.Ceiling(need*1.2)),true);
      if(!child){if(Gate("field-split-block:"+t.m_id,10))Note($"field-relief-block team={t.m_id} enemy={enemy.m_id} reason=insufficient-splittable-surplus availableMen={budget.Men:F0} enemyIncludingArrivals={budget.EnemyMen:F0}");continue;}
      if(child.GetSoldierNum(false)>budget.Men||child.GetCombatVal()>budget.Power){child.MergeToTeam(t);continue;}
     }else if(t.GetCombatVal()>need*1.5&&t.m_listHero?.Count>1)child=SplitFieldArmy(t,(long)Math.Ceiling(need*1.2));
     var sent=child??t;if(!InstallFieldRelief(sent,enemy,victim,f)){if(child)child.MergeToTeam(t);continue;}
     if(origin!=null&&child)AccountFrontDetachment(origin,WarSide(origin,f.IBGCLCAIBOL),child);
    }else {
     var c=candidate.City;var teams=AttackTeams();if(!StaffingDestination(c,f)||SupportSourceRules(c,teams,AttackReserved(teams),true)!=null||IncomingHostile(c,f))continue;
     var previous=emergencyContext;try{emergencyContext=new(){City=c,Eta=candidate.Eta};t=GenerateSupport(c,need,teams,AttackReserved(teams));}finally{emergencyContext=previous;}
     if(!t){Note($"field-relief-block source={c.OJPECMNINNA} enemy={enemy.m_id} eta={candidate.Eta:F1} reason=assembly-or-retention-see-assembly-block");continue;}
     if(t.m_belongPower!=f.IBGCLCAIBOL||!StaffingDestination(c,f)||!RelayFoodSafe(t,c,candidate.Eta)||!FieldThreat(enemy,f,out victim)){DismissSupport(t);continue;}
     t.m_targetCity="";t.m_isEnterCity=false;t.m_attackTarget=enemy;
     fieldReliefOrders[t.Pointer]=new(){Team=t,Enemy=enemy,Protected=victim,Id=t.m_id,EnemyId=enemy.m_id,ProtectedId=victim.m_id,Faction=f.IBGCLCAIBOL,Day=NEINKOOBJIH.MIPFHCKDJOK,Eta=candidate.Eta,AwaitingDeparture=true};
     if(!t.gameObject.activeSelf)t.gameObject.SetActive(true);
     Note($"field-relief-assembly source={c.OJPECMNINNA} team={t.m_id} enemy={enemy.m_id} protected={victim.m_id} eta={candidate.Eta:F1} soldiers={t.GetSoldierNum(false)} needAdjustedPower={need}");
    }
   }
  }
  foreach(var phase in ReassignReserveSteps(f))yield return phase;
  foreach(var t in AttackTeams()){
   yield return "surplus-arrival-audit";if(!CurrentFaction(f))yield break;
   if(!MobileCandidate(t,f)||t.m_warInfo!=null||t.m_targetWarInfo==null||!SpareMarch(t,f)||fieldReliefOrders.ContainsKey(t.Pointer))continue;
   // Do not recall a useful nearest responder while an unresolved field threat
   // remains; the next sliced allocation can still select it.
   if(threats.Any(e=>FieldThreat(e,f,out var v)&&FieldNeed(e,v,f,0)>0))continue;
   NJHDEHLHENN best=null;double bestEta=double.PositiveInfinity;var teams=AttackTeams();var seen=new HashSet<IntPtr>();
   foreach(var member in teams){yield return "surplus-other-front";var w=member.m_warInfo;if(w==null||!seen.Add(w.Pointer)||Same(w,t.m_targetWarInfo))continue;
    if(CampBattleNeed(t,w,f,AttackTeams(),out double eta,out _,out _)&&eta<bestEta){best=w;bestEta=eta;}
   }
   if(!MobileCandidate(t,f)||!SpareMarch(t,f))continue;
   string old=t.m_targetWarInfo.HIENOINGBCN;
   if(best!=null&&CampBattleNeed(t,best,f,AttackTeams(),out _,out _,out int side)&&MoveRelief(t,best.IJGLCFJOOIK,best,false,f,true)){
    missions[t.Pointer]=new(){Team=t,Id=t.m_id,War=best,Name=best.HIENOINGBCN,Side=side,Faction=f.IBGCLCAIBOL,Source=t.m_assembleCity,Direct=IsDirect(best),ReadyAt=battleClock};
    Note($"surplus-redirect team={t.m_id} from={old} to={best.HIENOINGBCN} eta={bestEta:F1} enemyArrivalsIncluded=true");
   }else if(best==null){
    missions.Remove(t.Pointer);t.ClearTarget(false);var goal=new CampaignGoal{Id=t.m_id,Faction=f.IBGCLCAIBOL,Goal=t.m_assembleCity,Returning=true};campaignGoals[t.Pointer]=goal;t.StartCamp();
    work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());Note($"surplus-return team={t.m_id} from={old} reason=present-forces-cover-all-enemy-arrivals action=capacity-safe-distributed-return");
   }
  }
  foreach(var phase in WorldSurplusReturnSteps(f))yield return phase;
 }
 static IEnumerable<string> WorldSurplusReturnSteps(MKMOJNAJKJE f){
  var teams=AttackTeams();var wars=teams.Where(t=>t&&t.m_warInfo!=null&&WarSide(t.m_warInfo,f.IBGCLCAIBOL)>=0).Select(t=>t.m_warInfo).GroupBy(w=>w.Pointer).Select(g=>g.First()).ToList();
  foreach(var parent in teams){
   yield return "surplus-withdraw-scan";if(!CurrentFaction(f))yield break;
   if(!MobileCandidate(parent,f)||parent.m_warInfo==null||IsDirect(parent.m_warInfo)||parent.m_listHero?.Count<2)continue;
   var origin=parent.m_warInfo;int parentId=parent.m_id;string originName=origin.HIENOINGBCN;int side=WarSide(origin,f.IBGCLCAIBOL);if(side<0)continue;
   var budget=FrontBudget(origin,side,60,AttackTeams(),1.3);if(budget.Men<10000||budget.Power<=0)continue;
   // Unresolved fighting or pursuit elsewhere takes priority over dismissal.
   bool emergency=false;foreach(var w in wars){yield return "surplus-withdraw-other-front";if(!CurrentFaction(f))yield break;if(!Same(w,origin)&&FrontBudget(w,WarSide(w,f.IBGCLCAIBOL),60,AttackTeams(),1.15).Men<=0){emergency=true;break;}}
   if(emergency||AttackTeams().Any(e=>FieldThreat(e,f,out var v)&&FieldNeed(e,v,f,0)>0))continue;
   var destinations=new List<(AFAGFHJONFJ City,double Length)>();
   foreach(var city in ReturnCities(f)){yield return "surplus-withdraw-city";if(!CurrentFaction(f))yield break;if(!SafeLogisticsEndpoint(city,f)||CapacityRoom(city,AttackTeams())<=0)continue;
    var route=new RoutePlan();foreach(var p in ReliefPathSteps(parent.GetPosition(),city.BMBMKPGEHBH,f,null,true,route))yield return p;
    if(route.Success)destinations.Add((city,route.Length));
   }
   foreach(var item in destinations.OrderBy(x=>x.Length)){
    yield return "surplus-withdraw-commit";if(!CurrentFaction(f)||!MobileCandidate(parent,f)||parent.m_id!=parentId||origin.HIENOINGBCN!=originName||!Same(parent.m_warInfo,origin)||IsDirect(origin))break;
    var city=item.City;var fresh=AttackTeams();int room=CapacityRoom(city,fresh);if(room<=0||!SafeLogisticsEndpoint(city,f))continue;
    var otherWars=fresh.Where(t=>t&&t.m_warInfo!=null&&!Same(t.m_warInfo,origin)&&WarSide(t.m_warInfo,f.IBGCLCAIBOL)>=0).Select(t=>t.m_warInfo).GroupBy(w=>w.Pointer).Select(g=>g.First());
    if(otherWars.Any(w=>FrontBudget(w,WarSide(w,f.IBGCLCAIBOL),60,fresh,1.15).Men<=0)||fresh.Any(e=>FieldThreat(e,f,out var v)&&FieldNeed(e,v,f,0)>0))break;
    budget=FrontBudget(origin,side,60,fresh,1.3);int total=parent.GetSoldierNum(false),units=TeamTroops(parent).Count;if(total<=0||units<=0||budget.Men<10000)break;
    long cap=(long)Math.Min(budget.Power,parent.GetCombatVal()*Math.Min(.9,Math.Min(budget.Men/total,room/(double)units)));
    var child=SplitArmyCore(parent,cap,true);if(!child)continue;
    if(child.GetSoldierNum(false)>budget.Men||TeamTroops(child).Count>CapacityRoom(city,AttackTeams(),child)||!DestinationFoodSafe(city,AttackTeams(),child)||!CampaignRoute(child,city,f,out int days)||!ReturnSafe(child,city,f,AttackTeams(),days,true)){child.MergeToTeam(parent);continue;}
    var goal=new CampaignGoal{Id=child.m_id,Faction=f.IBGCLCAIBOL,Goal=city.OJPECMNINNA,Returning=true};campaignGoals[child.Pointer]=goal;
    if(!CampaignMove(child,goal,city,true,"winning-front-surplus-release",days)){campaignGoals.Remove(child.Pointer);child.MergeToTeam(parent);continue;}
    AccountFrontDetachment(origin,side,child);Note($"front-surplus-return war={origin.HIENOINGBCN} parent={parent.m_id} child={child.m_id} city={city.OJPECMNINNA} detached={child.GetSoldierNum(false)} remaining={Soldiers(origin,side,false)} enemyIncludingArrivals={budget.EnemyMen:F0} reassessableUntilEntry=true");break;
   }
  }
 }
 // Returning troops remain eligible responders until they actually enter a
 // city. Recompute all fronts, not just the war named by their former order.
 static IEnumerable<string> ReassignReserveSteps(MKMOJNAJKJE f){
  var teams=AttackTeams();var wars=teams.Where(t=>t&&t.m_warInfo!=null&&WarSide(t.m_warInfo,f.IBGCLCAIBOL)>=0).Select(t=>t.m_warInfo).GroupBy(w=>w.Pointer).Select(g=>g.First()).ToList();
  var options=new List<(Wo_DeployTeamCtrl Team,NJHDEHLHENN War,double Eta,double Score)>();
  foreach(var t in teams){
   yield return "reserve-front-scan";if(!CurrentFaction(f))yield break;
   if(!MobileCandidate(t,f)||t.m_warInfo!=null||fieldReliefOrders.ContainsKey(t.Pointer))continue;
   bool spare=t.m_targetWarInfo!=null&&SpareMarch(t,f);
   bool returning=t.m_targetWarInfo==null&&campaignGoals.TryGetValue(t.Pointer,out var goal)&&goal.Id==t.m_id&&goal.Returning;
   if(!spare&&!returning&&!CampIdle(t))continue;
   foreach(var w in wars){
    if(Same(w,t.m_targetWarInfo))continue;
    var route=new RoutePlan();foreach(var phase in ReliefPathSteps(t.GetPosition(),w.GBHGENNMJCK,f,BattleTransitRegion(w,f),true,route)){yield return "reserve-"+phase;if(!CurrentFaction(f))yield break;}
    double eta=route.Success&&t.GetMoveSpeed()>0?route.Length/t.GetMoveSpeed():double.PositiveInfinity;
    if(CampBattleNeedAt(t,w,f,AttackTeams(),eta,out int need,out int side))options.Add((t,w,eta,need/(1+eta)));
   }
  }
  var committed=new HashSet<int>();
  foreach(var option in options.OrderByDescending(x=>x.Score).ThenBy(x=>x.Eta)){
   yield return "reserve-front-commit";if(!CurrentFaction(f))yield break;
   var parent=option.Team;var war=option.War;
   if(committed.Contains(parent.m_id)||!MobileCandidate(parent,f)||parent.m_warInfo!=null||parent.m_targetWarInfo!=null&&!SpareMarch(parent,f))continue;
   if(!CampBattleNeed(parent,war,f,AttackTeams(),out double eta,out int need,out int side))continue;
   // Need is soldiers; convert once to this army's power for native splitting.
   int total=parent.GetSoldierNum(false);if(total<=0)continue;
   var child=total>need*1.5?SplitFieldArmy(parent,(long)Math.Ceiling(parent.GetCombatVal()*Math.Min(.9,need*1.2/total))):null;
   var sent=child??parent;
   if(!MoveRelief(sent,war.IJGLCFJOOIK,war,false,f,true)){if(child)child.MergeToTeam(parent);continue;}
   campaignGoals.Remove(sent.Pointer);missions[sent.Pointer]=new(){Team=sent,Id=sent.m_id,War=war,Name=war.HIENOINGBCN,Side=side,Faction=f.IBGCLCAIBOL,Source=sent.m_assembleCity,Direct=IsDirect(war),ReadyAt=battleClock};committed.Add(parent.m_id);
   Note($"reserve-front-reassigned team={sent.m_id} parent={parent.m_id} war={war.HIENOINGBCN} need={need} sent={sent.GetSoldierNum(false)} eta={eta:F1} reason=fresh-multifront-demand previous-return-not-final=true");
  }
 }
 static void RouteFieldDeparture(Wo_DeployTeamCtrl t){
  if(!t||!fieldReliefOrders.TryGetValue(t.Pointer,out var o)||!o.AwaitingDeparture||t.m_deployState==AKOPKFPKJPP.Assemble)return;
  o.AwaitingDeparture=false;
  if(!LiveArmy(t)||ManualProtected(t)||t.m_id!=o.Id||t.m_belongPower!=o.Faction||t.m_isPrivate||t.IncludePlayer()||t.m_responseToPlayer||t.m_warInfo!=null||t.m_mergeTarget||!string.IsNullOrEmpty(t.m_targetCity)||t.m_attackTarget&&t.m_attackTarget.Pointer!=o.Enemy.Pointer){fieldReliefOrders.Remove(t.Pointer);return;}
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(o.Faction,out var f)||!CampSupportPermission(t,f)){fieldReliefOrders.Remove(t.Pointer);return;}
  if(LiveArmy(o.Protected)&&o.Protected.m_id==o.ProtectedId&&o.Protected.m_warInfo!=null){t.m_attackTarget=o.Enemy;CheckFieldRelief(o);return;}
  if(o.Enemy.m_id==o.EnemyId&&InstallFieldRelief(t,o.Enemy,o.Protected,f))return;
  t.m_attackTarget=o.Enemy;CheckFieldRelief(o);
 }
 static void CheckFieldRelief(FieldReliefOrder order){
  var t=order.Team;if(!LiveArmy(t)||t.m_id!=order.Id||t.m_belongPower!=order.Faction){fieldReliefOrders.Remove(t.Pointer);return;}
  if(order.AwaitingDeparture&&t.m_deployState!=AKOPKFPKJPP.Assemble){RouteFieldDeparture(t);return;}
  if(t.m_warInfo!=null){fieldReliefOrders.Remove(t.Pointer);return;}
  if(ManualProtected(t)||t.m_isPrivate||t.IncludePlayer()||t.m_responseToPlayer||t.m_attackTarget?.Pointer!=order.Enemy.Pointer){fieldReliefOrders.Remove(t.Pointer);return;}
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(order.Faction,out var f))return;
  var engaged=order.Protected.m_id==order.ProtectedId?order.Protected.m_warInfo:null;
  if(engaged!=null&&WarSide(engaged,order.Faction)>=0&&t.m_deployState!=AKOPKFPKJPP.Assemble&&MoveRelief(t,engaged.IJGLCFJOOIK,engaged,false,f,true)){
   fieldReliefOrders.Remove(t.Pointer);missions[t.Pointer]=new(){Team=t,Id=t.m_id,War=engaged,Name=engaged.HIENOINGBCN,Side=WarSide(engaged,order.Faction),Faction=order.Faction,Source=t.m_assembleCity,Direct=IsDirect(engaged),ReadyAt=battleClock};Note($"field-relief-handoff team={t.m_id} protected={order.ProtectedId} war={engaged.HIENOINGBCN} action=join-started-battle");return;
  }
  if(engaged!=null&&t.m_deployState==AKOPKFPKJPP.Assemble)return;
  if(order.Enemy.m_id!=order.EnemyId||order.Protected.m_id!=order.ProtectedId||!FieldThreat(order.Enemy,f,out var victim)||victim.Pointer!=order.Protected.Pointer){
   fieldReliefOrders.Remove(t.Pointer);if(t.m_deployState==AKOPKFPKJPP.Assemble){DismissSupport(t);return;}
   t.ClearTarget(false);var goal=new CampaignGoal{Id=t.m_id,Faction=order.Faction,Goal=t.m_assembleCity,Returning=true};campaignGoals[t.Pointer]=goal;t.StartCamp();work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());return;
  }
  if(t.m_deployState==AKOPKFPKJPP.Assemble)return;
  if(AIGate("field-repath:"+t.m_id,order.Faction,2)&&((t.m_listMovePos?.Count??0)==0||Vector3.Distance(t.m_listMovePos[t.m_listMovePos.Count-1],order.Enemy.GetPosition())>Math.Max(2,t.GetMoveSpeed()*2)))InstallFieldRelief(t,order.Enemy,order.Protected,f);
 }
}
