using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AI;

public sealed partial class StrategicSupport
{
 static ConfigEntry<bool> offensive;
 static ConfigEntry<int> attackTreasury,attackMaxDays;
 static ConfigEntry<float> attackAdvantage;
 sealed class AttackOrder {public Wo_DeployTeamCtrl Team;public int Id;public string City,Target,Faction,Governor,Enemy;public double Day;}
 static readonly Dictionary<IntPtr,AttackOrder> attackOrders=new();
 static readonly Dictionary<string,int> attackPlanning=new();
 static readonly Dictionary<string,(string Faction,int Cost)> attackPlanningCosts=new();
 void ConfigureOffense(){
  offensive=Config.Bind("Offensive","Enabled",true,"Extra attacks based on available expedition power after home defense reserves. Empty city capacity does not block attacks. Never overrides StopWar or declares war.");
  attackTreasury=Config.Bind("Offensive","MinimumTreasury",100000,new ConfigDescription("Upper baseline for adaptive mobilization funds. Actual floor uses three upkeep periods, minimum 3000; independent of the research reserve.",new AcceptableValueRange<int>(30000,10000000)));
  // Old CapacityFill/TroopStrength settings are no longer read. They measured
  // the whole city's empty capacity rather than the army we can safely send.
  attackAdvantage=Config.Bind("Offensive","PowerAdvantage",1.35f,new ConfigDescription("Required expedition combat power relative to target and nearby relief estimate.",new AcceptableValueRange<float>(1.1f,2f)));
  attackMaxDays=Config.Bind("Offensive","MaximumTravelDays",45,new ConfigDescription("Actual army road travel cap; reserve an additional 30 days of expedition food.",new AcceptableValueRange<int>(10,90)));
 }
 void HookOffense(Harmony h){
  HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"UpdateOneDay"),prefix:H(nameof(BeforeCampaignDay)),postfix:H(nameof(AfterCampaignDay)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"EnterCity"),prefix:H(nameof(GuardCampaignEntry)));
  h.Patch(AccessTools.Method(typeof(MKMOJNAJKJE),"BLOFJHJPIOH"),prefix:H(nameof(BeforeNativeTeamRefresh)),finalizer:H(nameof(AfterNativeTeamRefresh)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"StartCamp"),postfix:H(nameof(AfterCampaignCamp)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"StartDeploy"),prefix:H(nameof(GuardAttackStart)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"StartDeploy"),postfix:H(nameof(RecordDeployResult)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"Dismiss"),prefix:H(nameof(RecordDismissal)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"LeaveCity"),postfix:H(nameof(RecordAttackDeparture)));
  h.Patch(AccessTools.Method(typeof(Wa_ArmyCtrl),"DrawNavigationPath"),prefix:H(nameof(BeforeArmyNavigationDrawing)));
 }
 static void ResetOffense(){failedCombatProbes.Clear();attackOrders.Clear();attackPlanning.Clear();attackPlanningCosts.Clear();}
 static bool SameAttack(AttackOrder o)=>o.Team&&o.Team.m_id==o.Id&&o.Team.m_belongPower==o.Faction&&o.Team.m_assembleCity==o.City&&o.Team.m_targetCity==o.Target&&!o.Team.m_isEnterCity;
 static int OffenseCashReserve(MKMOJNAJKJE f){
  if(f==null||offensive?.Value!=true)return 0;
  int floor=0;foreach(var p in attackPlanningCosts.Values)if(p.Faction==f.IBGCLCAIBOL)floor=Math.Max(floor,p.Cost);
  foreach(var o in attackOrders.Values)if(o.Faction==f.IBGCLCAIBOL&&SameAttack(o)&&o.Team.m_deployState==AKOPKFPKJPP.Assemble&&NEINKOOBJIH.MIPFHCKDJOK-o.Day<60&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.City,out var city))floor=Math.Max(floor,MobilizationCost(city));
  return floor;
 }
 static int ResearchFundsReserve(MKMOJNAJKJE f)=>SharedReserve(f);
 static bool AttackPermission(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor,string enemy=null){
  if(offensive?.Value!=true||!CurrentCity(c,f,governor)||!f.ANMMMOIKLEG.TryGetValue(governor,out var g)||g==null||g.BNGFJOAJKDH==NEFBEOFPGJA.StopWar)return false;
  return enemy==null||g.BNGFJOAJKDH!=NEFBEOFPGJA.AttackPower||g.LMAEGAJADKI==enemy;
 }
 static List<Wo_DeployTeamCtrl> AttackTeams(){var a=new List<Wo_DeployTeamCtrl>();if(ICGAEMLACJG.HFJIECIBFKB!=null)foreach(var kv in ICGAEMLACJG.HFJIECIBFKB)if(kv.Value)a.Add(kv.Value);return a;}
 static HashSet<IntPtr> AttackReserved(List<Wo_DeployTeamCtrl> teams){var r=new HashSet<IntPtr>();foreach(var t in teams)Reserve(t,r);return r;}
 static bool AttackableOwner(MKMOJNAJKJE f,string owner)=>string.IsNullOrEmpty(owner)||War(f,owner);
 // An owned corridor can lead to an unoccupied frontier beyond the capital's
 // immediate neighbours. Never use an enemy/neutral city as a transit frontier.
 static List<AFAGFHJONFJ> FrontierTargets(AFAGFHJONFJ source,MKMOJNAJKJE f){
  var result=new List<AFAGFHJONFJ>();var queue=new Queue<AFAGFHJONFJ>();var seen=new HashSet<string>{source.OJPECMNINNA};queue.Enqueue(source);
  while(queue.Count>0&&seen.Count<=256){var city=queue.Dequeue();if(city.FKMKGNPFLFP==null)continue;
   foreach(var n in city.FKMKGNPFLFP){if(n?.LMPMNLLGOEC==null||!seen.Add(n.OJPECMNINNA))continue;
    if(n.LMPMNLLGOEC.LMMNKIOAGLC==f.IBGCLCAIBOL)queue.Enqueue(n);else result.Add(n);
   }
  }return result;
 }
 static string AttackReadiness(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor,List<Wo_DeployTeamCtrl> teams){
  if(NativeOffenseDisabled())return "native-no-war-option";
  if(!AttackPermission(c,f,governor))return "governor-or-control";
  if(IncomingHostile(c,f))return "source-under-attack";
  var stock=AttackStock(c,f,teams);
  string reason=OffensivePolicy.Readiness(stock.Home,stock.Ready,minUnits.Value,f.DNOGPFHMOMO,MobilizationCost(c),Math.Max(0,c.LAGKJHBEPGF(false)));
  if(reason!=null)return reason;
  if(!DayReady("attack-source:"+c.OJPECMNINNA,7))return "offensive-planning-interval";
  int active=0;foreach(var t in teams)if(t.m_belongPower==f.IBGCLCAIBOL&&!t.m_isPrivate){
   if(t.m_assembleCity==c.OJPECMNINNA)return "source-already-deployed";
   if(t.m_warInfo!=null||(!t.m_isEnterCity&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_targetCity??"",out var target)&&target.LMPMNLLGOEC!=null&&AttackableOwner(f,target.LMPMNLLGOEC.LMMNKIOAGLC)))active++;
  }
  return active>=ExpeditionLimit(f)?"faction-expedition-limit":null;
 }
 static (int Home,int Ready,long Power,long Budget) AttackStock(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams){
  var reserved=AttackReserved(teams);var seen=new HashSet<IntPtr>();int home=0,ready=0;long power=0,availablePower=0;
  if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID){
   if(a==null||reserved.Contains(a.Pointer)||!seen.Add(a.Pointer)||a.FGHFOLKBHDL<=0)continue;
   home++;long p=Math.Max(0,a.GNCKPEPJDGF());power+=p;
   if(Available(a)){ready++;availablePower+=p;}
  }
  return (home,ready,power,OffensivePolicy.PowerBudget(power,availablePower,AttackBorderThreat(c,f,teams),Retention(c,f)));
 }
 static void AttackSkip(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,string reason,AFAGFHJONFJ target=null){
  var s=AttackStock(c,f,teams);
  Note($"offense-skip city={c.OJPECMNINNA} faction={f.IBGCLCAIBOL} target={target?.OJPECMNINNA} reason={reason} homeUnits={s.Home} readyUnits={s.Ready} homePower={s.Power} surplusPower={s.Budget} required={(target==null?0:AttackDefense(target,teams))} treasury={f.DNOGPFHMOMO} mobilization={MobilizationCost(c)} upkeep={c.LAGKJHBEPGF(false)} cityCash={c.LMPMNLLGOEC.DNOGPFHMOMO} food={c.LMPMNLLGOEC.OEGNGFBCHFL}");
  if(reason=="no-ready-troops"&&c.LMPMNLLGOEC.KJPPGIOCDID!=null){var rows=new List<string>();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID){if(a!=null&&rows.Count<16)rows.Add($"{a.OJPECMNINNA}:soldiers={a.FGHFOLKBHDL},flag={a.MGLFFEANDOI},state={a.GDHPLIFJCBN}");}Note($"offense-troops city={c.OJPECMNINNA} nativeSizeThreshold={EBNFFALGBPA.LLLBAAAEIBB*.4} [{string.Join(";",rows)}]");}
 }
 static bool EnemyTarget(AFAGFHJONFJ c,AFAGFHJONFJ target,MKMOJNAJKJE f,string governor,List<Wo_DeployTeamCtrl> teams){
  return EnemyTargetChecked(c,target,f,governor,teams,false);
 }
 static bool EnemyTargetChecked(AFAGFHJONFJ c,AFAGFHJONFJ target,MKMOJNAJKJE f,string governor,List<Wo_DeployTeamCtrl> teams,bool knownFrontier){
  if(NativeOffenseDisabled())return false;
  if(target?.LMPMNLLGOEC==null||!AttackableOwner(f,target.LMPMNLLGOEC.LMMNKIOAGLC)||!AttackPermission(c,f,governor,target.LMPMNLLGOEC.LMMNKIOAGLC??"")||target.GKLIEHCHDLD())return false;
  if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(target.OJPECMNINNA,out var actual)||actual?.Pointer!=target.Pointer)return false;
  if(!knownFrontier&&!FrontierTargets(c,f).Any(n=>n.Pointer==target.Pointer))return false;
  foreach(var t in teams)if(t.m_belongPower==f.IBGCLCAIBOL&&(t.m_targetCity==target.OJPECMNINNA||t.m_warInfo?.IJGLCFJOOIK?.Pointer==target.Pointer))return false;
  return true;
 }
 static int AttackDefense(AFAGFHJONFJ target,List<Wo_DeployTeamCtrl> teams){
  long defense=Power(target);var seen=new HashSet<string>{target.OJPECMNINNA};
  string owner=target.LMPMNLLGOEC.LMMNKIOAGLC;
  if(!string.IsNullOrEmpty(owner)){
   if(target.FKMKGNPFLFP!=null)foreach(var n in target.FKMKGNPFLFP)if(n?.LMPMNLLGOEC?.LMMNKIOAGLC==owner&&seen.Add(n.OJPECMNINNA))defense+=Power(n)/3;
   foreach(var t in teams)if(t.m_belongPower==owner&&t.m_targetCity==target.OJPECMNINNA)defense+=Math.Max(0,t.GetCombatVal());
  }
  return OffensivePolicy.RequiredPower(defense,Math.Round(attackAdvantage.Value,3)+NativeWarFrequencyMargin());
 }
 // UI WarFrequency -> static +0x34. Native governor offense adds .8 in
 // mode 0 vs .4 otherwise to its variable strength margin. Preserve that
 // .4 difference in our deterministic extra-attack threshold as well.
 // Defense/support/return timing is deliberately unaffected.
 static double NativeWarFrequencyMargin()=>GLIJMELMEHI.FKNPIBLCEDK==0?.4:0;
 // Native MKMOJNAJKJE.GHNEGBJANJG skips governor attack planning for mode 2.
 // Do not put this in AttackPermission: that also serves defensive bandit
 // interception and hostile schemes, which are not city invasions.
 static bool NativeOffenseDisabled()=>GLIJMELMEHI.FKNPIBLCEDK==2;
 static bool AttackRoute(AFAGFHJONFJ c,AFAGFHJONFJ target,MKMOJNAJKJE f,out double length){
  var result=new RoutePlan();foreach(var phase in AttackRouteSteps(c,target,f,result)){}length=result.Length;return result.Success;
 }
 static IEnumerable<string> AttackRouteSteps(AFAGFHJONFJ c,AFAGFHJONFJ target,MKMOJNAJKJE f,RoutePlan result){
  foreach(var phase in ReliefPathCoreSteps(c.BMBMKPGEHBH,target.BMBMKPGEHBH,f,target.OJPECMNINNA,false,null,result))yield return phase;

 }
 static IEnumerable<string> OffensiveSteps(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor){
  if(offensive?.Value!=true||!AIGate("attack-check:"+c.OJPECMNINNA,f.IBGCLCAIBOL,30))yield break;
  // Dropped/recycled/redirected teams must not retain resources indefinitely.
  foreach(var pair in attackOrders.ToArray()){
   var o=pair.Value;
   if(!SameAttack(o)||o.Team.m_deployState!=AKOPKFPKJPP.Assemble){attackOrders.Remove(pair.Key);continue;}
   if(NEINKOOBJIH.MIPFHCKDJOK-o.Day>=60){attackOrders.Remove(pair.Key);o.Team.Dismiss(true,false);Note($"offense-cancel team={o.Id} reason=assembly-timeout");}
  }
  var teams=AttackTeams();string reason=AttackReadiness(c,f,governor,teams);
  if(reason!=null){AttackSkip(c,f,teams,reason);yield break;}
  attackPlanning.TryGetValue(f.IBGCLCAIBOL,out int holders);attackPlanning[f.IBGCLCAIBOL]=holders+1;
  attackPlanningCosts[c.OJPECMNINNA]=(f.IBGCLCAIBOL,MobilizationCost(c));
  try{
   var targets=FrontierTargets(c,f);
   var candidates=new List<(AFAGFHJONFJ City,double Score)>();
   long powerBudget=AttackStock(c,f,teams).Budget;
   // A zero budget already prevents every candidate. Do not repeat the
   // whole source census and native target power query for each enemy city.
   if(powerBudget<=0){AttackSkip(c,f,teams,"no-safe-raw-surplus");Note($"offense-scan-blocked city={c.OJPECMNINNA} targets=[{string.Join(",",targets.Select(t=>t.OJPECMNINNA))}] reason=no-safe-raw-surplus scope=single-source allTargetPathsScanned=false");yield break;}
   foreach(var target in targets){
    if(!AttackPermission(c,f,governor))yield break;
    // Reuse this scan's frontier walk; dispatch rechecks current ownership
    // and the frontier from scratch after all cooperative yields.
    if(EnemyTargetChecked(c,target,f,governor,teams,true)){
     int required=AttackDefense(target,teams);
     if(!CombatProbeReady(c,"city:"+target.Pointer,required,teams))continue;
     else {
      var plannedRoute=new RoutePlan();foreach(var phase in AttackRouteSteps(c,target,f,plannedRoute)){yield return phase;if(!CurrentCity(c,f,governor))yield break;}
      bool route=plannedRoute.Success;double distance=plannedRoute.Length;
      if(!route)AttackSkip(c,f,teams,"route-blocked",target);
      else {double score=1d/(required*(1+distance/100));candidates.Add((target,score));if(Gate("offense-rank:"+c.OJPECMNINNA+":"+target.OJPECMNINNA,120))Note($"offense-target-rank source={c.OJPECMNINNA} target={target.OJPECMNINNA} requiredAdjusted={required} roadLength={distance:F2} score={score:R} basis=defense-times-road-cost");}
     }
    }
    yield return "offense-target";
   }
   if(candidates.Count==0){AttackSkip(c,f,teams,"no-feasible-frontier-target");yield break;}
   // If native assembly cannot supply the first target, try the next feasible
   // one, yielding between attempts. Keep work bounded in crowded mod maps.
   foreach(var candidate in candidates.OrderByDescending(x=>x.Score).Take(4)){
    if(!AttackPermission(c,f,governor))yield break;
    bool sent=TryAttack(c,candidate.City,f,governor);yield return "offense-dispatch";
    if(sent)yield break;
   }
  }finally{attackPlanningCosts.Remove(c.OJPECMNINNA);if(attackPlanning.TryGetValue(f.IBGCLCAIBOL,out int n)){if(n<=1)attackPlanning.Remove(f.IBGCLCAIBOL);else attackPlanning[f.IBGCLCAIBOL]=n-1;}}
 }
 static bool TryAttack(AFAGFHJONFJ c,AFAGFHJONFJ target,MKMOJNAJKJE f,string governor){
  var teams=AttackTeams();string reason=AttackReadiness(c,f,governor,teams);
  if(reason==null&&!EnemyTarget(c,target,f,governor,teams))reason="target-changed";
  if(reason==null&&!AttackRoute(c,target,f,out _))reason="route-blocked";
  if(reason==null&&AttackStock(c,f,teams).Budget<=0)reason="no-safe-raw-surplus";
  if(reason==null&&!CombatProbeReady(c,"city:"+target.Pointer,AttackDefense(target,teams),teams))reason="unchanged-failed-assembly";
  if(reason!=null){AttackSkip(c,f,teams,reason,target);return false;}
  var reserved=AttackReserved(teams);if(!ViableSource(c,teams,reserved)){AttackSkip(c,f,teams,"source-food-officer-or-troop-readiness",target);return false;}
  Wo_DeployTeamCtrl team=null;
  try{
   int required=AttackDefense(target,teams);
   offenseGenerating=true;try{team=GenerateSupport(c,required,teams,reserved);}finally{offenseGenerating=false;}if(!team){FailedProbe(c,"city:"+target.Pointer,required);AttackSkip(c,f,AttackTeams(),"native-assembly-or-garrison-reserve",target);return false;}
   // The same native generator and assembly/food lifecycle used by support.
   // Only the destination is hostile; no forced StartDeploy or teleport.
   team.m_targetCity=target.OJPECMNINNA;team.m_isEnterCity=false;
   team.m_targetFoodDay=Math.Max(foodDays.Value,attackMaxDays.Value+30);
   var order=new AttackOrder{Team=team,Id=team.m_id,City=c.OJPECMNINNA,Target=target.OJPECMNINNA,Faction=f.IBGCLCAIBOL,Enemy=target.LMPMNLLGOEC.LMMNKIOAGLC,Governor=governor,Day=NEINKOOBJIH.MIPFHCKDJOK};
   reason=ValidateAttack(order,c,target,f);
   if(reason!=null){int actual=team.GetCombatVal();Note($"offense-power-probe city={c.OJPECMNINNA} target={target.OJPECMNINNA} team={team.m_id} actualAdjusted={actual} requiredAdjusted={required} soldiers={team.GetSoldierNum(false)} rejected={reason} phase=before-dismiss");DismissSupport(team);FailedAttackProbe(c,target,required,actual,reason);AttackSkip(c,f,AttackTeams(),reason,target);return false;}
   attackOrders[team.Pointer]=order;DayReady("attack-source:"+c.OJPECMNINNA,0,true);
   if(!team.gameObject.activeSelf)team.gameObject.SetActive(true);
   Note($"offense-ordered faction={f.IBGCLCAIBOL} source={c.OJPECMNINNA} target={target.OJPECMNINNA} governor={governor} team={team.m_id} power={team.GetCombatVal()} required={required} soldiers={team.GetSoldierNum(false)} treasury={f.DNOGPFHMOMO} assembly=true");return true;
  }catch{if(team){attackOrders.Remove(team.Pointer);try{DismissSupport(team);}catch{}}throw;}
 }
 static string ValidateAttack(AttackOrder o,AFAGFHJONFJ c,AFAGFHJONFJ target,MKMOJNAJKJE f){
  if(NativeOffenseDisabled())return "native-no-war-option";
  var t=o.Team;
  if(NEINKOOBJIH.MIPFHCKDJOK-o.Day>=60)return "assembly-timeout";
  if(!SameAttack(o)||!AttackPermission(c,f,o.Governor,o.Enemy??""))return "policy-or-ownership-changed";
  if(target?.LMPMNLLGOEC==null||(target.LMPMNLLGOEC.LMMNKIOAGLC??"")!=(o.Enemy??"")||!AttackableOwner(f,o.Enemy)||target.GKLIEHCHDLD())return "target-or-war-changed";
  if(f.DNOGPFHMOMO<MobilizationCost(c))return "treasury-changed";
  if(!AttackRoute(c,target,f,out double attackLength))return "route-blocked";
  var teams=AttackTeams();var others=teams.Where(x=>x.Pointer!=t.Pointer).ToList();
  if(!EnemyTarget(c,target,f,o.Governor,others))return "duplicate-or-invalid-target";
  int required=AttackDefense(target,others);if(t.GetCombatVal()<required)return "insufficient-attack-power";
  var sent=new HashSet<IntPtr>();long sentPower=0,sentSoldiers=0;
  if(t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var a in group.Value){if(a==null||!sent.Add(a.Pointer))return "invalid-army";sentPower+=Math.Max(0,a.GNCKPEPJDGF());sentSoldiers+=Math.Max(0,a.FGHFOLKBHDL);}
  var reserved=AttackReserved(others);long homePower=0,homeSoldiers=0;int homeUnits=0;var present=new HashSet<IntPtr>();
  if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!reserved.Contains(a.Pointer)&&a.FGHFOLKBHDL>0&&present.Add(a.Pointer)){homePower+=Math.Max(0,a.GNCKPEPJDGF());homeSoldiers+=a.FGHFOLKBHDL;homeUnits++;}
  long keepPower=Math.Max(AttackBorderThreat(c,f,others),(long)Math.Ceiling(homePower*Retention(c,f)));
  long keepSoldiers=(long)Math.Ceiling(homeSoldiers*Retention(c,f));
  if(sent.Count==0||!sent.IsSubsetOf(present)||homeUnits-sent.Count<minUnits.Value||homePower-sentPower<keepPower||homeSoldiers-sentSoldiers<keepSoldiers){
   Note($"attack-reserve-block team={o.Id} faction={o.Faction} source={o.City} target={o.Target} homeUnits={homeUnits} sentUnits={sent.Count} sentInRoster={sent.Count(p=>present.Contains(p))} keptUnits={homeUnits-sent.Count} minUnits={minUnits.Value} homePower={homePower} sentPower={sentPower} keepPower={keepPower} homeSoldiers={homeSoldiers} sentSoldiers={sentSoldiers} keepSoldiers={keepSoldiers}");
   return "defense-reserve-changed";
  }
  int days=0;if(t.m_listHero==null||t.m_listHero.Count==0)return "no-officer";
  var assigned=new HashSet<string>();foreach(var h in t.m_listHero)if(h!=null)assigned.Add(h.OJPECMNINNA);
  int availableHome=0;var homeIds=HomeOfficers(c,others);if(c.ABJNKABKKJM!=null)foreach(var h in c.ABJNKABKKJM)if(h!=null&&homeIds.Contains(h.OJPECMNINNA)&&(h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None||assigned.Contains(h.OJPECMNINNA)))availableHome++;
  if(!assigned.IsSubsetOf(homeIds)||availableHome-assigned.Count<2)return "home-command-reserve";
  foreach(var h in t.m_listHero)if(h==null)return "invalid-officer";
  days=CapacityJourneyDays(attackLength,t.GetMoveSpeed(),AOHIBKLOAEI.NIOKFCDLGFK());if(days<=0||days>attackMaxDays.Value)return "travel-too-long";
  int foodDaysNeeded=Math.Max(days+30,t.m_targetFoodDay);
  if(!OffensivePolicy.FoodSafe(c.LMPMNLLGOEC.OEGNGFBCHFL,ReservedFood(c.OJPECMNINNA),CityFoodFloor(c,f.IBGCLCAIBOL),t.GetOneDayFood(),foodDaysNeeded,t.GetFood()))return "food-reserve";
  if(t.GetMorale()<70)return "low-morale";
  return null;
 }
 static bool GuardAttackStart(Wo_DeployTeamCtrl __instance){
  if(__instance==null)return true;
  if(!GuardFailedIntercept(__instance))return false;
  if(!GuardHuntStart(__instance))return false;
  if(!GuardCapacityTransfer(__instance))return false;
  if(Gate("deploy-observe:"+__instance.m_id,30))Note($"native-deploy-attempt faction={__instance.m_belongPower} source={__instance.m_assembleCity} target={__instance.m_targetCity} team={__instance.m_id} state={__instance.m_deployState} private={__instance.m_isPrivate} enterCity={__instance.m_isEnterCity} tracked={attackOrders.ContainsKey(__instance.Pointer)}");
  if(!attackOrders.TryGetValue(__instance.Pointer,out var o)){
   // Native plans (including assemblies restored from a save) were never
   // approved by our extra-offense planner. Do not retroactively adopt them
   // and apply our cash floor, retention ratio, target competition or NavMesh
   // heuristic. StartDeploy can start assembly; it is not proof of departure.
   // Preserve explicit governor policy, then let native lifecycle checks run.
   try {
   if(!Ready()||ManualProtected(__instance)||offensive?.Value!=true||__instance.m_isPrivate||__instance.IncludePlayer()||__instance.m_responseToPlayer||__instance.m_isEnterCity||__instance.m_deployState!=AKOPKFPKJPP.Assemble)return true;
   if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(__instance.m_assembleCity??"",out var source)||!Eligible(source,out var owner)||owner.IBGCLCAIBOL!=__instance.m_belongPower
    ||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(__instance.m_targetCity??"",out var destination)||destination?.LMPMNLLGOEC==null||!AttackableOwner(owner,destination.LMPMNLLGOEC.LMMNKIOAGLC))return true;
   string blocked=NativeOffenseDisabled()&&__instance.m_targetWarInfo==null&&__instance.m_warInfo==null?"native-no-war-option":null;
   if(owner.ANMMMOIKLEG.TryGetValue(source.LMPMNLLGOEC.IPJHOOJBCKI??"",out var governor)&&governor!=null){
    if(governor.BNGFJOAJKDH==NEFBEOFPGJA.StopWar)blocked="native-governor-StopWar";
    else if(governor.BNGFJOAJKDH==NEFBEOFPGJA.AttackPower&&governor.LMAEGAJADKI!=(destination.LMPMNLLGOEC.LMMNKIOAGLC??""))blocked="native-designated-enemy";
   }
   if(blocked!=null){__instance.Dismiss(true,false);Note($"offense-cancel team={__instance.m_id} faction={owner.IBGCLCAIBOL} source={source.OJPECMNINNA} target={destination.OJPECMNINNA} origin=native reason={blocked}");return false;}
   if(Gate("native-policy-pass:"+__instance.m_id,30))Note($"native-deploy-policy-pass team={__instance.m_id} faction={owner.IBGCLCAIBOL} source={source.OJPECMNINNA} target={destination.OJPECMNINNA} treasury={owner.DNOGPFHMOMO} cityCash={source.LMPMNLLGOEC.DNOGPFHMOMO} extraOffenseRules=false nativeLifecycle=true");
   return true;
   } catch(Exception e){Error(e);Note($"native-deploy-observation-error team={__instance.m_id} nativeLifecycle=true");return true;}
  }
  if(!SameAttack(o)||__instance.m_deployState!=AKOPKFPKJPP.Assemble){attackOrders.Remove(__instance.Pointer);return true;}
  string reason="world-or-record-changed";
  try{
   if(Ready()&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.City,out var c)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Target,out var target)&&NEINKOOBJIH.COCMABJFOFH.TryGetValue(o.Faction,out var f))reason=ValidateAttack(o,c,target,f);
   if(reason==null)return true;
  }catch(Exception e){reason="validation-error";Error(e);}
  int actualPower=__instance.GetCombatVal();
  Note($"offense-cancel team={o.Id} faction={o.Faction} source={o.City} target={o.Target} reason={reason} phase=before-dismiss");
  attackOrders.Remove(__instance.Pointer);DismissSupport(__instance);
  if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.City,out var failedSource)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Target,out var failedTarget)&&failedSource.LMPMNLLGOEC?.LMMNKIOAGLC==o.Faction)
   FailedAttackProbe(failedSource,failedTarget,AttackDefense(failedTarget,AttackTeams()),actualPower,reason);
  return false;
 }
 static void RecordDeployResult(Wo_DeployTeamCtrl __instance){
  try {RouteNewRelief(__instance);AfterInterceptDeploy(__instance);NotifyDefense(__instance);if(__instance!=null&&Gate("deploy-result:"+__instance.m_id,30))Note($"native-deploy-result team={__instance.m_id} faction={__instance.m_belongPower} source={__instance.m_assembleCity} target={__instance.m_targetCity} state={__instance.m_deployState} active={__instance.gameObject.activeSelf} tracked={attackOrders.ContainsKey(__instance.Pointer)} departureRequiresLeaveCity=true");}catch(Exception e){Error(e);}
 }
 static void RecordDismissal(Wo_DeployTeamCtrl __instance){
  try{ObserveBattleDispatchDismissal(__instance);}catch(Exception e){Error(e);}
  try{LogDismissCallsite(__instance);}catch(Exception e){Error(e);}
  if(__instance!=null)fieldReliefOrders.Remove(__instance.Pointer);
  if(__instance!=null){reliefLoans.Remove(__instance.Pointer);coverOrders.Remove(__instance.Pointer);defenseOrders.Remove(__instance.Pointer);NotifyDefense(__instance);capacityOrders.Remove(__instance.Pointer);campaignGoals.Remove(__instance.Pointer);}
  try {if(__instance!=null&&Gate("dismiss-observe:"+__instance.m_id,30))Note($"army-dismiss-observed team={__instance.m_id} faction={__instance.m_belongPower} source={__instance.m_assembleCity} target={__instance.m_targetCity} state={__instance.m_deployState} tracked={attackOrders.ContainsKey(__instance.Pointer)} private={__instance.m_isPrivate} enterCity={__instance.m_isEnterCity}");}catch(Exception e){Error(e);}
 }
 static void RecordAttackDeparture(Wo_DeployTeamCtrl __instance,AFAGFHJONFJ __0){
  try{
   if(__instance==null||__0==null)return;
   if(missions.TryGetValue(__instance.Pointer,out var mission)&&mission.Id==__instance.m_id&&mission.Faction==__instance.m_belongPower&&!mission.Departed){mission.Departed=true;mission.DepartureTick=Environment.TickCount64;mission.DepartureDay=NEINKOOBJIH.MIPFHCKDJOK;mission.DeparturePosition=__instance.GetPosition();}
   NotifyDefense(__instance);
   RememberCampaign(__instance);
   if(capacityOrders.TryGetValue(__instance.Pointer,out var transfer)&&transfer.Id==__instance.m_id){capacityOrders.Remove(__instance.Pointer);Note($"capacity-transfer-departed team={transfer.Id} source={transfer.Source} target={transfer.Target} nativeLeaveCity=true");}
   if(attackOrders.TryGetValue(__instance.Pointer,out var o)&&SameAttack(o)&&__0.OJPECMNINNA==o.City){
    attackOrders.Remove(__instance.Pointer);Note($"offense-departed faction={o.Faction} source={o.City} target={o.Target} team={o.Id} nativeLeaveCity=true food={__instance.GetFood()}");
   }else Note($"native-departed faction={__instance.m_belongPower} source={__0.OJPECMNINNA} target={__instance.m_targetCity} team={__instance.m_id} enterCity={__instance.m_isEnterCity} private={__instance.m_isPrivate} power={__instance.GetCombatVal()}");
  }catch(Exception e){Error(e);}
 }
}
