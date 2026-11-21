using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public sealed partial class StrategicSupport {
 sealed class DefenseOrder {public Wo_DeployTeamCtrl Team;public int Id;public string Faction,Source,Target;public bool Routed,Moving;public Vector3 Position,ProgressPosition;public double Day,ProgressDay;}
 static readonly Dictionary<IntPtr,DefenseOrder> defenseOrders=new();
 static readonly Dictionary<string,(double Eta,long Until,string Stamp)> battleRoutes=new();
 static readonly Dictionary<int,string> defenseSignatures=new();
 static double defenseDay=-1;static long defensePollAt;
 static void ResetDefense(){fieldReliefOrders.Clear();coverPlanning.Clear();coverPlannedDay.Clear();reliefLoans.Clear();coverOrders.Clear();defenseOrders.Clear();defenseSignatures.Clear();battleRoutes.Clear();savedBattleRoads.Clear();defenseDay=-1;defensePollAt=0;}
 static void ScheduleRegularDefense(MKMOJNAJKJE f){
  string key="regular-defense:"+f.IBGCLCAIBOL;if(!DecisionDue(key,f.IBGCLCAIBOL,f.Pointer,"",false))return;
  if(ScheduleDefense(f))DecisionDue(key,f.IBGCLCAIBOL,f.Pointer,"",true);
 }
 static bool ScheduleDefense(MKMOJNAJKJE f){
  if(f==null||!support.Value)return false;
  // Threat events retain the fast allied response. NPC events use the same
  // short interval multiplied by three, rather than bypassing NPC cadence.
  if(!AIGate("defense-evaluation:"+f.IBGCLCAIBOL,f.IBGCLCAIBOL,2))return false;
  long queuedAt=Environment.TickCount64;double queuedDay=NEINKOOBJIH.MIPFHCKDJOK;
  return work.Add("defense:"+f.IBGCLCAIBOL,()=>LoggedDefenseSteps(f,queuedAt,queuedDay).GetEnumerator());
 }
 static IEnumerable<string> LoggedDefenseSteps(MKMOJNAJKJE f,long queuedAt,double queuedDay){
  bool report=Gate("defense-work:"+f.IBGCLCAIBOL,10);
  if(report)Note($"defense-work-start faction={f.IBGCLCAIBOL} queuedDay={queuedDay:F4} startDay={NEINKOOBJIH.MIPFHCKDJOK:F4} queueWaitMs={Environment.TickCount64-queuedAt}");
  foreach(var phase in DefenseSteps(f))yield return phase;
  if(report)Note($"defense-work-end faction={f.IBGCLCAIBOL} endDay={NEINKOOBJIH.MIPFHCKDJOK:F4} totalMs={Environment.TickCount64-queuedAt}");
 }
 static void NotifyDefense(Wo_DeployTeamCtrl t){
  if(t==null||!support.Value||!Ready())return;
  if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_targetCity??"",out var c)&&c.LMPMNLLGOEC!=null&&NEINKOOBJIH.COCMABJFOFH.TryGetValue(c.LMPMNLLGOEC.LMMNKIOAGLC,out var f))ScheduleDefense(f);
  defensePollAt=0;
 }
 static void PollDefense(){
  if(!support.Value||Environment.TickCount64<defensePollAt)return;
  defensePollAt=Environment.TickCount64+2000;work.Add("defense-pulse",()=>DefensePulse().GetEnumerator());
 }
 static IEnumerable<string> DefensePulse(){
  double day=Math.Floor((double)NEINKOOBJIH.MIPFHCKDJOK);bool daily=day!=defenseDay;defenseDay=day;
  var factions=new HashSet<string>();
  // Snapshot before yields: native city/team registries can change on arrival.
  if(daily){
  var cities=new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AFAGFHJONFJ>(MICGPAMNDKM.IJPKEMPIFIH.Count);MICGPAMNDKM.IJPKEMPIFIH.Values.CopyTo(cities,0);
  foreach(var c in cities){yield return "daily-city";if(!Ready())yield break;
   if(daily&&c?.LMPMNLLGOEC!=null&&DistributionCity(c,out var f)){
    if(factions.Add(f.IBGCLCAIBOL)){ScheduleRegularDefense(f);ScheduleStaffing(f);ScheduleBanditHunt(f);}
    // Run the existing sliced governor decisions daily, not the whole native
    // monolithic governor routine, which also carries unrelated side effects.
    ScheduleCity(c);
   }
  }
  }
  var teams=AttackTeams();var ids=new HashSet<int>();
  foreach(var faction in teams.Select(t=>t.m_belongPower).Distinct())if(NEINKOOBJIH.COCMABJFOFH.TryGetValue(faction??"",out var owner)&&owner!=null&&teams.Any(t=>t&&((t.m_belongPower==faction&&(t.m_warInfo!=null||t.m_targetWarInfo!=null))||FieldThreat(t,owner,out _)))&&AIGate("mobile-relief:"+faction,faction,2))work.Add("mobile-relief:"+faction,()=>SafePhases(MobileReliefSteps(owner)).GetEnumerator());
  foreach(var order in fieldReliefOrders.Values.ToArray()){yield return "field-relief-watch";CheckFieldRelief(order);}
  foreach(var t in teams){yield return "incoming-order";if(!t)continue;ids.Add(t.m_id);
   // Reuse the existing sliced scan; do not add a per-frame global army scan.
   // This also recovers orphaned Expedition survivors loaded from a save.
   AdoptCompletedCityOrder(t);AuditCampState(t);RecoverHostileStandoff(t);ScheduleReturnWatch(t);
   if(CampIdle(t))AfterCampaignDay(t);
   string signature=t.m_belongPower+"|"+t.m_targetCity+"|"+(int)t.m_deployState+"|attack="+t.m_attackTarget?.m_id+"|war="+t.m_targetWarInfo?.HIENOINGBCN;
   if(!defenseSignatures.TryGetValue(t.m_id,out var previous)||previous!=signature){
    defenseSignatures[t.m_id]=signature;
    Note($"defense-threat-seen team={t.m_id} faction={t.m_belongPower} target={t.m_targetCity} state={t.m_deployState} gameDay={NEINKOOBJIH.MIPFHCKDJOK:F4} daily={daily}");NotifyDefense(t);
    if(previous!=null){var parts=previous.Split('|');if(parts.Length>1&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(parts[1],out var old)&&old?.LMPMNLLGOEC!=null&&NEINKOOBJIH.COCMABJFOFH.TryGetValue(old.LMPMNLLGOEC.LMMNKIOAGLC,out var of))ScheduleDefense(of);}
   }
  }
  foreach(int id in defenseSignatures.Keys.ToArray())if(!ids.Contains(id))defenseSignatures.Remove(id);
  foreach(var hunt in huntOrders.Values.ToArray()){yield return "hunt-progress";try{CheckHunt(hunt);}catch(Exception e){Error(e);}}
  foreach(var cover in coverOrders.Values.ToArray()){yield return "regional-cover-progress";try{if(AIGate("cover-watch:"+cover.Id,cover.Faction,2))CheckRegionalCover(cover);}catch(Exception e){Error(e);}}
  foreach(var phase in RegionalLoanSteps())yield return phase;
  foreach(var o in defenseOrders.Values.ToArray()){yield return "relay-progress";try{if(AIGate("relay-watch:"+o.Id,o.Faction,2))CheckDefenseOrder(o);}catch(Exception e){Error(e);}}
 }
 static bool SameDefense(DefenseOrder o)=>o.Team&&!ManualProtected(o.Team)&&o.Team.m_id==o.Id&&o.Team.m_belongPower==o.Faction&&!o.Team.m_isPrivate&&!o.Team.m_responseToPlayer&&!o.Team.IncludePlayer();
 static bool DefenseBattleMatches(NJHDEHLHENN war,AFAGFHJONFJ target,MKMOJNAJKJE f)=>target!=null&&f!=null&&war!=null&&war.EMMJBPMNFAK!=null&&war.EMMJBPMNFAK.Contains(f.IBGCLCAIBOL)&&BattleTransitRegion(war,f)==target.OJPECMNINNA;
 static bool DefenseThreat(AFAGFHJONFJ target,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams)=>teams.Any(t=>t&&War(f,t.m_belongPower)&&(t.m_targetCity==target.OJPECMNINNA||DefenseBattleMatches(t.m_warInfo,target,f)));
 static void SampleDefenseProgress(DefenseOrder o){
  var t=o.Team;if(!SameDefense(o)||t.m_deployState!=AKOPKFPKJPP.Expedition)return;
  var position=t.GetPosition();
  if(Vector3.Distance(o.ProgressPosition,position)>.1f){o.ProgressPosition=position;o.ProgressDay=NEINKOOBJIH.MIPFHCKDJOK;}
 }
 static void ReleaseDefense(DefenseOrder o,string reason,bool returnHome){
  if(!SameDefense(o)){defenseOrders.Remove(o.Team.Pointer);return;}
  var t=o.Team;Note($"defense-release team={o.Id} target={o.Target} reason={reason} return={returnHome}");
  if(!returnHome||t.m_warInfo!=null){
   defenseOrders.Remove(t.Pointer);
   if(reason=="joined-battle")campaignGoals[t.Pointer]=new CampaignGoal{Id=t.m_id,Faction=o.Faction,Goal=o.Target,Returning=true};
   return;
  }
  if(t.m_deployState==AKOPKFPKJPP.Assemble){defenseOrders.Remove(t.Pointer);DismissSupport(t);return;}
  // Keep our ownership marker during StartCamp's callback so campaign recovery
  // cannot enqueue an obsolete offensive order re-entrantly.
  t.StartCamp();t.m_targetCity="";t.m_targetWarInfo=null;
  defenseOrders.Remove(t.Pointer);
  var goal=new CampaignGoal{Id=t.m_id,Faction=o.Faction,Goal=o.Target,Returning=true};campaignGoals[t.Pointer]=goal;
  work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());
 }
 static void CheckDefenseOrder(DefenseOrder o){
  if(!SameDefense(o)){defenseOrders.Remove(o.Team.Pointer);return;}var t=o.Team;
  if(OtherCampaignOrder(t)||(!string.IsNullOrEmpty(t.m_targetCity)&&t.m_targetCity!=o.Target)) {ReleaseDefense(o,"replacement-order",false);return;}
  if(t.m_warInfo!=null){
   MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Target,out var arrivalCity);NEINKOOBJIH.COCMABJFOFH.TryGetValue(o.Faction,out var arrivalFaction);
   bool matched=DefenseBattleMatches(t.m_warInfo,arrivalCity,arrivalFaction);
   Note($"defense-battle-arrival team={o.Id} intended={o.Target} actual={BattleTransitRegion(t.m_warInfo,arrivalFaction)} matched={matched}");ReleaseDefense(o,matched?"joined-battle":"unexpected-battle",false);return;
  }
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(o.Faction,out var f)||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Target,out var target)||target.LMPMNLLGOEC?.LMMNKIOAGLC!=o.Faction){ReleaseDefense(o,"destination-lost",true);return;}
  var teams=AttackTeams();bool threat=DefenseThreat(target,f,teams);
  if(!threat){ReleaseDefense(o,"threat-ended",true);return;}
  if(t.m_deployState==AKOPKFPKJPP.Assemble){if(NEINKOOBJIH.MIPFHCKDJOK-o.Day>5)Note($"defense-assembly-delayed team={o.Id} target={o.Target} days={NEINKOOBJIH.MIPFHCKDJOK-o.Day:F1}");return;}
  t.m_isEnterCity=false;
  float moved=Vector3.Distance(o.Position,t.GetPosition());if(!o.Moving&&moved>.1f){o.Moving=true;Note($"defense-moving team={o.Id} target={o.Target} distance={moved:F2}");}
  if(Vector3.Distance(o.ProgressPosition,t.GetPosition())>.1f){o.ProgressPosition=t.GetPosition();o.ProgressDay=NEINKOOBJIH.MIPFHCKDJOK;}
  else if(t.m_deployState==AKOPKFPKJPP.Expedition&&NEINKOOBJIH.MIPFHCKDJOK-o.ProgressDay>2){
   t.StartCamp();o.Routed=false;o.ProgressDay=NEINKOOBJIH.MIPFHCKDJOK;Note($"defense-stalled team={o.Id} target={o.Target} repath=true");
  }
  var war=teams.Select(x=>x.m_warInfo).FirstOrDefault(w=>DefenseBattleMatches(w,target,f));
  if(war!=null){if(!Same(t.m_targetWarInfo,war)){if(!MoveRelief(t,target,war,false,f)){ReleaseDefense(o,"battle-route-unavailable",true);return;}Note($"defense-join-order team={o.Id} target={o.Target} war={war.HIENOINGBCN}");}return;}
  if(t.m_targetWarInfo!=null){ReleaseDefense(o,"replacement-battle-order",false);return;}
  if(t.GetFood()<(long)Math.Max(1,t.GetOneDayFood())*5||t.GetMorale()<30){ReleaseDefense(o,"food-or-morale",true);return;}
  if(Vector3.Distance(t.GetPosition(),target.BMBMKPGEHBH)<2.2f){
   if(Gate("defense-hold:"+o.Id,30))Note($"defense-holding team={o.Id} target={o.Target} soldiers={t.GetSoldierNum(false)} outsideCity=true");return;
  }
  if(!o.Routed||(t.m_deployState==AKOPKFPKJPP.Camp&&Gate("defense-repath:"+o.Id,10))){
   if(!RouteSeconds(t.GetPosition(),target.BMBMKPGEHBH,f,t.GetMoveSpeed(),out double travel)||!ICGAEMLACJG.FKKCPDEFODO.TryGetValue(o.Target,out var ctrl)){ReleaseDefense(o,"route-unavailable",true);return;}
   if(!MoveRelief(t,target,null,false,f)){ReleaseDefense(o,"route-install-failed",true);return;}o.Routed=true;
   if(t.m_listMovePos==null||t.m_listMovePos.Count==0){ReleaseDefense(o,"native-route-empty",true);return;}
   Note($"defense-route team={o.Id} target={o.Target} travelSeconds={travel:F1} path={t.m_listMovePos.Count} outsideCity=true");
  }
 }
 static bool GuardDefenseEntry(Wo_DeployTeamCtrl t,AFAGFHJONFJ c){
  if(t==null||!defenseOrders.TryGetValue(t.Pointer,out var o)||!SameDefense(o))return true;
  // Relief armies stay deployed; entering a full city can cause desertion.
  if(c?.OJPECMNINNA==o.Target){t.StartCamp();t.m_targetCity="";t.m_isEnterCity=false;return false;}return true;
 }
 static bool RouteSeconds(Vector3 from,Vector3 to,MKMOJNAJKJE f,float speed,out double seconds,string allowedDestination=null){
  seconds=double.PositiveInfinity;
  if(speed<=0||!ReliefPath(from,to,f,allowedDestination,false,out var points,out double length))return false;
  seconds=length/speed;return double.IsFinite(seconds);
 }
 static double DefenseHomePower(AFAGFHJONFJ target,List<Wo_DeployTeamCtrl> teams){
  // GNCKPEPJDGF(city) pairs expedition-eligible commanders with ready units;
  // it can be zero for an occupied city that still has defending soldiers.
  var deployed=AttackReserved(teams);var seen=new HashSet<IntPtr>();long soldiers=0,raw=0;int units=0;
  if(target.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in target.LMPMNLLGOEC.KJPPGIOCDID){
   if(a==null||a.FGHFOLKBHDL<=0||deployed.Contains(a.Pointer)||!seen.Add(a.Pointer))continue;
   units++;soldiers+=a.FGHFOLKBHDL;raw+=Math.Max(0,a.GNCKPEPJDGF());
  }
  double expedition=Power(target),power=Math.Max(expedition,raw);
  if(Gate("defense-home:"+target.OJPECMNINNA,15))Note($"defense-home city={target.OJPECMNINNA} units={units} soldiers={soldiers} rawPower={raw} expeditionPower={expedition:F0} defensePower={power:F0} excludesDeployed=true");
  return power;
 }
 static double TeamArrival(Wo_DeployTeamCtrl t,Vector3 target){
  if(!t||t.GetMoveSpeed()<=0)return double.PositiveInfinity;
  if(t.m_deployState==AKOPKFPKJPP.Camp&&Vector3.Distance(t.GetPosition(),target)>2.2f)return double.PositiveInfinity;
  if(t.m_deployState==AKOPKFPKJPP.Expedition&&(t.m_listMovePos==null||t.m_listMovePos.Count==0)&&Vector3.Distance(t.GetPosition(),target)>2.2f)return double.PositiveInfinity;
  double length=ArmyRemainingRoad(t,target);return length/t.GetMoveSpeed()+Math.Max(0,t.GetLeftAssembleDay())*AOHIBKLOAEI.NIOKFCDLGFK();
 }
 static bool BattleRouteEta(AFAGFHJONFJ c,NJHDEHLHENN w,MKMOJNAJKJE f,out double eta){
  string key=w.Pointer+":"+w.HIENOINGBCN+":"+f.IBGCLCAIBOL+":"+c.OJPECMNINNA;
  if(SavedBattleQuote(c,w,f,key,RoadPolicyStamp(f),out eta))return double.IsFinite(eta);
  if(schedulerOnline){work.Add("battle-route:"+key,()=>BattleRouteQuote(c,w,f,key).GetEnumerator());eta=double.PositiveInfinity;return false;}
  foreach(var phase in BattleRouteQuote(c,w,f,key)){}eta=battleRoutes.TryGetValue(key,out var quote)?quote.Eta:double.PositiveInfinity;return double.IsFinite(eta);
 }
 static bool RelayFoodSafe(Wo_DeployTeamCtrl t,AFAGFHJONFJ source,double travel){
  double duration=Math.Ceiling(travel/Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK()))+15;if(!double.IsFinite(duration)||duration>180)return false;
  int days=(int)Math.Max(1,duration);
  long need=Math.Max(0,(long)Math.Max(1,t.GetOneDayFood())*days-t.GetFood());
  long home=0;foreach(var a in source.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null)home+=Math.Max(0,a.FGHFOLKBHDL);
  if(source.LMPMNLLGOEC.OEGNGFBCHFL-ReservedFood(source.OJPECMNINNA)-need<home*5)return false;
  t.m_targetFoodDay=Math.Max(t.m_targetFoodDay,days);return true;
 }
 static IEnumerable<string> BattleRouteQuote(AFAGFHJONFJ c,NJHDEHLHENN w,MKMOJNAJKJE f,string key){
  yield return "path-prepare";
  if(!CurrentFaction(f)||c.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||w.EMMJBPMNFAK==null||!w.EMMJBPMNFAK.Contains(f.IBGCLCAIBOL))yield break;
  string battlefield=BattleTransitRegion(w,f);
  var route=new RoutePlan();var destination=w.GBHGENNMJCK;var generation=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;string stamp=RoadPolicyStamp(f);
  foreach(var phase in ReliefPathSteps(c.BMBMKPGEHBH,destination,f,battlefield,false,route)){
   yield return phase;
   if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f)||c.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||Vector3.Distance(destination,w.GBHGENNMJCK)>.01f)yield break;
  }
  if(RoadPolicyStamp(f)!=stamp)yield break;
  SaveBattleRoad(c,w,f,route,stamp);
  float speed=Wo_DeployTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL,false);bool ok=route.Success&&speed>0;double eta=ok?route.Length/speed:double.PositiveInfinity;reliefRouteFailure=route.Failure;
  if(Gate("battle-route-result:"+key,15))Note($"battle-route-result source={c.OJPECMNINNA} faction={f.IBGCLCAIBOL} war={w.HIENOINGBCN} destination={w.IJGLCFJOOIK?.OJPECMNINNA} battlefield={battlefield} accepted={ok} eta={(ok?eta+mobilizeSeconds.Value:double.PositiveInfinity):F1} reason={(ok?"validated-complete-path":reliefRouteFailure)}");
  if(battleRoutes.Count>2048)battleRoutes.Clear();
  battleRoutes[key]=(ok?eta+mobilizeSeconds.Value:double.PositiveInfinity,Environment.TickCount64+15000,RoadPolicyStamp(f));
  // Do not wait another observation interval just because path work was queued.
  if(battles.TryGetValue(w.Pointer,out var state)&&state.Name==w.HIENOINGBCN){
   int side=w.EMMJBPMNFAK[0]==f.IBGCLCAIBOL?0:1;
   if(state.RouteAwaiting[side]&&ok){state.RouteAwaiting[side]=false;state.SideNext[side]=double.NegativeInfinity;state.Next=double.NegativeInfinity;}
  }
 }
 static IEnumerable<string> DefenseSteps(MKMOJNAJKJE f){
  if(!Ready()||!support.Value||!CurrentFaction(f))yield break;
  var teams=AttackTeams();var threatened=new HashSet<string>();var reserved=new HashSet<IntPtr>();
  foreach(var t in teams){yield return "threat-scan";Reserve(t,reserved);
   if(War(f,t.m_belongPower)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_targetCity??"",out var c)&&c.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL)threatened.Add(c.OJPECMNINNA);
  }
  var owned=f.PJMLPLDPEIM()?.ToArray();if(owned==null)yield break;
  foreach(string id in threatened){
   if(!CurrentFaction(f)||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(id,out var target)||target.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL)continue;
   if(target.GKLIEHCHDLD()){
    if(Gate("defense-battle-handoff:"+id,10))Note($"defense-handoff target={id} faction={f.IBGCLCAIBOL} gameDay={NEINKOOBJIH.MIPFHCKDJOK:F4} reason=already-in-battle evaluator=battle-support");continue;
   }
   var foes=new List<ReinforcementTimeline.Wave>();var friends=new List<ReinforcementTimeline.Wave>();
   foreach(var t in teams){yield return "arrival-scan";
    bool relay=defenseOrders.TryGetValue(t.Pointer,out var o)&&SameDefense(o)&&o.Target==id;
    if(t.m_warInfo!=null||(!relay&&t.m_targetCity!=id))continue;
    double at=TeamArrival(t,target.BMBMKPGEHBH);if(!double.IsFinite(at))continue;
    if(t.m_belongPower==f.IBGCLCAIBOL)friends.Add(new(at,Math.Max(0,t.GetCombatVal())));
    else if(War(f,t.m_belongPower))foes.Add(new(at,Math.Max(0,t.GetCombatVal())));
   }
   if(foes.Count==0)continue;
   double contact=foes.Min(x=>x.At),horizon=foes.Max(x=>x.At)+Math.Max(30,AOHIBKLOAEI.NIOKFCDLGFK()*10);
   var sources=new List<(AFAGFHJONFJ City,double Eta)>();
   foreach(var c in owned){yield return "source-route";
    if(c==null||c.Pointer==target.Pointer||!DistributionCity(c,out _))continue;
    string why=SupportSourceBlock(c,teams,reserved);if(why!=null){DefenseBlocked(f,id,c.OJPECMNINNA,why);continue;}
    var route=new RoutePlan();foreach(var phase in ReliefPathSteps(c.BMBMKPGEHBH,target.BMBMKPGEHBH,f,null,false,route)){yield return phase;if(!CurrentFaction(f)||target.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL)yield break;}
    float routeSpeed=Wo_DeployTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL,false);
    if(!route.Success||routeSpeed<=0){DefenseBlocked(f,id,c.OJPECMNINNA,"no-complete-permitted-route");continue;}double eta=route.Length/routeSpeed;
    eta+=mobilizeSeconds.Value;if(eta>maxTravelSeconds.Value){DefenseBlocked(f,id,c.OJPECMNINNA,"arrival-too-late");continue;}sources.Add((c,eta));
   }
   sources.Sort((x,y)=>x.Eta.CompareTo(y.Eta));int sent=0;
   foreach(var candidate in sources){yield return "relay-source";
    if(!CurrentFaction(f)||target.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||target.GKLIEHCHDLD())break;
    teams=AttackTeams();if(!DefenseThreat(target,f,teams))break;reserved.Clear();foreach(var t in teams)Reserve(t,reserved);
    RefreshDefenseWaves(f,target,teams,friends,foes);if(foes.Count==0)break;
    contact=foes.Min(x=>x.At);horizon=foes.Max(x=>x.At)+Math.Max(30,AOHIBKLOAEI.NIOKFCDLGFK()*10);
    var c=candidate.City;string why=SupportSourceBlock(c,teams,reserved);if(why!=null){DefenseBlocked(f,id,c.OJPECMNINNA,why);continue;}
    // Before contact, there is no observed casualty rate. Use a labelled
    // conservative planning assumption, not fabricated combat telemetry.
    double eta=candidate.Eta,own=DefenseHomePower(target,teams),loss=foes.Sum(x=>x.Strength)/Math.Max(60,AOHIBKLOAEI.NIOKFCDLGFK()*20);
    int need=ReinforcementTimeline.Need(own,0,loss,0,eta,Math.Max(eta,horizon),friends,foes,coverage.Value,contact,out string reason,out double hold,true);
    Note($"defense-forecast faction={f.IBGCLCAIBOL} target={id} source={c.OJPECMNINNA} ownPower={own} enemyPower={foes.Sum(x=>x.Strength):F0} enemyEta={contact:F1} supportEta={eta:F1} friendlyWaves={friends.Count} lossPowerPerSec={loss:F1} lossBasis=precontact-assumption-capped-25pct holdSeconds={hold:F1} needPower={need} reason={reason}");
    if(need<=0)continue;
    var tnew=GenerateMarchRelief(c,target,f,need,candidate.Eta,teams,reserved);if(!tnew){DefenseBlocked(f,id,c.OJPECMNINNA,"assembly-rejected-see-assembly-block");continue;}
    bool committed=false;
    try {
    var latest=AttackTeams();
    if(!CurrentFaction(f)||target.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||target.GKLIEHCHDLD()||!DefenseThreat(target,f,latest)||IncomingHostile(c,f)){
     DismissSupport(tnew);DefenseBlocked(f,id,c.OJPECMNINNA,"ownership-battle-or-source-threat-changed");continue;
    }
    if(!RouteSeconds(c.BMBMKPGEHBH,target.BMBMKPGEHBH,f,tnew.GetMoveSpeed(),out double actual)){DismissSupport(tnew);DefenseBlocked(f,id,c.OJPECMNINNA,"generated-route-rejected");continue;}
    actual+=Math.Max(mobilizeSeconds.Value,Math.Max(0,tnew.GetLeftAssembleDay())*AOHIBKLOAEI.NIOKFCDLGFK());
    if(actual>maxTravelSeconds.Value||!RelayFoodSafe(tnew,c,actual)||ReinforcementTimeline.Need(own,0,loss,0,actual,Math.Max(actual,horizon),friends,foes,coverage.Value,contact,out _,out _,true)<=0){DismissSupport(tnew);DefenseBlocked(f,id,c.OJPECMNINNA,"actual-arrival-food-or-demand-changed");continue;}
    // Native assembly uses a friendly city transfer destination. After native
    // departure we switch to non-entry rally routing; the entry hook protects
    // against a very short route arriving before that queued reassessment.
    tnew.m_targetCity=id;tnew.m_isEnterCity=true;
    defenseOrders[tnew.Pointer]=new DefenseOrder{Team=tnew,Id=tnew.m_id,Faction=f.IBGCLCAIBOL,Source=c.OJPECMNINNA,Target=id,Position=tnew.GetPosition(),ProgressPosition=tnew.GetPosition(),Day=NEINKOOBJIH.MIPFHCKDJOK,ProgressDay=NEINKOOBJIH.MIPFHCKDJOK};
    committed=true;if(!tnew.gameObject.activeSelf)tnew.gameObject.SetActive(true);
    friends.Add(new(actual,Math.Max(0,tnew.GetCombatVal())));sent++;BackfillAfterCommit(c,target,f);
    Note($"defense-relay-ordered sourceGovernor={c.LMPMNLLGOEC.IPJHOOJBCKI} targetGovernor={target.LMPMNLLGOEC.IPJHOOJBCKI} faction={f.IBGCLCAIBOL} target={id} source={c.OJPECMNINNA} team={tnew.m_id} eta={actual:F1} soldiers={tnew.GetSoldierNum(false)} power={tnew.GetCombatVal()} wave={friends.Count} nativeDeparturePending=true");
    } catch {if(!committed&&tnew){try{DismissSupport(tnew);}catch{}}throw;}
   }
   Note($"defense-summary faction={f.IBGCLCAIBOL} target={id} candidates={sources.Count} sent={sent} waves={friends.Count}");
  }
 }
 static void RefreshDefenseWaves(MKMOJNAJKJE f,AFAGFHJONFJ target,List<Wo_DeployTeamCtrl> teams,List<ReinforcementTimeline.Wave> friends,List<ReinforcementTimeline.Wave> foes){
  friends.Clear();foes.Clear();
  foreach(var t in teams){
   bool relay=defenseOrders.TryGetValue(t.Pointer,out var o)&&SameDefense(o)&&o.Target==target.OJPECMNINNA;
   if(t.m_warInfo!=null||(!relay&&t.m_targetCity!=target.OJPECMNINNA))continue;
   double at=TeamArrival(t,target.BMBMKPGEHBH);if(!double.IsFinite(at))continue;
   if(relay){
    // Queue latency is not lack of movement. Sample the live position before
    // applying the stall exclusion, including when the watch job has not run.
    SampleDefenseProgress(o);
    if(t.m_deployState==AKOPKFPKJPP.Expedition&&NEINKOOBJIH.MIPFHCKDJOK-o.ProgressDay>2){
     if(Gate("defense-wave-stalled:"+o.Id,10))Note($"defense-wave-excluded team={o.Id} target={o.Target} reason=position-unchanged days={NEINKOOBJIH.MIPFHCKDJOK-o.ProgressDay:F2}");continue;
    }
   }
   if(relay&&t.m_deployState==AKOPKFPKJPP.Assemble){
    double age=Math.Max(0,NEINKOOBJIH.MIPFHCKDJOK-o.Day)*AOHIBKLOAEI.NIOKFCDLGFK();if(age>Math.Max(30,5*AOHIBKLOAEI.NIOKFCDLGFK()))continue;
    at+=Math.Max(0,mobilizeSeconds.Value-age-Math.Max(0,t.GetLeftAssembleDay())*AOHIBKLOAEI.NIOKFCDLGFK());
   }
   if(t.m_belongPower==f.IBGCLCAIBOL)friends.Add(new(at,Math.Max(0,t.GetCombatVal())));
   else if(War(f,t.m_belongPower))foes.Add(new(at,Math.Max(0,t.GetCombatVal())));
  }
 }
 static void DefenseBlocked(MKMOJNAJKJE f,string target,string source,string why){if(Gate("defense-block:"+target+":"+source+":"+why,15))Note($"defense-blocked faction={f.IBGCLCAIBOL} target={target} source={source} reason={why}");}
}
