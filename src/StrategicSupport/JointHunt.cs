using System;using System.Collections.Generic;using System.Linq;using UnityEngine;
public sealed partial class StrategicSupport {
 sealed class JointHunt {
  public string Key,Faction;public IntPtr World;public Wo_DeployTeamCtrl Target;public int TargetId;public AFAGFHJONFJ Staging;
  public readonly List<HuntOrder> Members=new();public bool Approved,Ending;public double Started,Until;
 }
 static readonly Dictionary<string,JointHunt> jointHunts=new();
 static bool JointHuntOwned(Wo_DeployTeamCtrl t)=>t&&huntOrders.TryGetValue(t.Pointer,out var o)&&o.Id==t.m_id&&o.Joint!=null;
 static bool JointMemberLive(HuntOrder o)=>LiveTeam(o.Team)&&o.Team.m_id==o.Id&&o.Team.m_belongPower==o.Faction;
 static string JointHuntBlock(JointHunt g){
  if(!CurrentHuntTarget(g))return "target-ended";
  if(Vector3.Distance(g.Staging.BMBMKPGEHBH,g.Target.GetPosition())<8)return "raider-entered-rendezvous";
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(g.Faction,out var f)||!CurrentFaction(f)||g.Staging?.LMPMNLLGOEC?.LMMNKIOAGLC!=g.Faction||!SafeLogisticsEndpoint(g.Staging,f)||IncomingHostile(g.Staging,f))return "staging-no-longer-safe";
  if(NEINKOOBJIH.MIPFHCKDJOK>g.Until)return "rendezvous-deadline";
  foreach(var o in g.Members){
   if(!JointMemberLive(o))return "member-lost";var t=o.Team;
   if(ManualProtected(t)||t.m_isPrivate||t.IncludePlayer()||t.m_responseToPlayer)return "member-player-order";
   if(t.m_warInfo!=null||t.m_targetWarInfo!=null)return "member-joined-battle";
   if(!huntOrders.TryGetValue(t.Pointer,out var held)||!ReferenceEquals(held,o))return "member-order-replaced";
   if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Source,out var city)||city.LMPMNLLGOEC?.LMMNKIOAGLC!=g.Faction||!AttackPermission(city,f,city.LMPMNLLGOEC.IPJHOOJBCKI)||IncomingHostile(city,f))return "source-policy-or-defense";
   if(t.GetMorale()<60)return "low-morale";
   if(t.m_deployState==AKOPKFPKJPP.Assemble&&!OffensivePolicy.FoodSafe(city.LMPMNLLGOEC.OEGNGFBCHFL,ReservedFood(city.OJPECMNINNA),CityFoodFloor(city,f.IBGCLCAIBOL),t.GetOneDayFood(),t.m_targetFoodDay,t.GetFood()))return "source-food-reserve";
   if(t.m_deployState!=AKOPKFPKJPP.Assemble&&t.GetFood()<(long)Math.Max(1,t.GetOneDayFood())*Math.Max(10,g.Until-NEINKOOBJIH.MIPFHCKDJOK))return "rendezvous-food";
  }
  return null;
 }
 static bool CurrentHuntTarget(JointHunt g)=>g.World==MICGPAMNDKM.IJPKEMPIFIH?.Pointer&&g.Target&&g.Target.m_id==g.TargetId&&Raider(g.Target)&&g.Target.m_warInfo==null;
 static long JointHuntPower(JointHunt g){
  long power=0;var units=new HashSet<IntPtr>();var officers=new HashSet<string>();var teams=new HashSet<IntPtr>();
  foreach(var o in g.Members){if(!JointMemberLive(o)||!teams.Add(o.Team.Pointer))return 0;var t=o.Team;
   if(t.m_listHero==null||t.m_dicDeployArmy==null)return 0;
   foreach(var h in t.m_listHero)if(h==null||!officers.Add(h.OJPECMNINNA))return 0;
   foreach(var row in t.m_dicDeployArmy)if(row.Value!=null)foreach(var a in row.Value)if(a==null||!units.Add(a.Pointer))return 0;
   power+=Math.Max(0,t.GetCombatVal());
  }return power;
 }
 static bool JointStagingRoad(List<Vector3> points,Wo_DeployTeamCtrl enemy,MKMOJNAJKJE f)=>
  FirstCorridorBlock(points,null,f)==null&&!double.IsFinite(CorridorContact(points,enemy.GetPosition()));
 static IEnumerable<string> JointHuntSteps(MKMOJNAJKJE f,Wo_DeployTeamCtrl enemy,List<(AFAGFHJONFJ City,int Days)> candidates){
  if(!CurrentFaction(f)||!Raider(enemy)||enemy.m_warInfo!=null||candidates.Count<2)yield break;
  string key=MusterKey(f.IBGCLCAIBOL,enemy);if(jointHunts.ContainsKey(key)||huntOrders.Values.Any(o=>o.Faction==f.IBGCLCAIBOL&&o.TargetId==enemy.m_id&&JointMemberLive(o)))yield break;
  var known=candidates.GroupBy(x=>x.City.Pointer).Select(x=>x.First()).OrderBy(x=>x.Days)
   .Where(x=>huntPowerSamples.TryGetValue(x.City.OJPECMNINNA+"/"+HuntProbeKey(enemy),out var s)&&s.Faction==f.IBGCLCAIBOL&&s.Adjusted>0).ToList();
  if(known.Count<2||known.Sum(x=>(long)huntPowerSamples[x.City.OJPECMNINNA+"/"+HuntProbeKey(enemy)].Adjusted)<HuntPower(enemy))yield break;
  // A friendly field rendezvous uses no city garrison slot. It must be outside
  // the raider's contact zone, with complete physical roads for each donor.
  foreach(var anchor in known){
   yield return "joint-hunt-anchor";if(!CurrentFaction(f)||!Raider(enemy)||enemy.m_warInfo!=null)yield break;
   var staging=anchor.City;if(!SafeLogisticsEndpoint(staging,f)||IncomingHostile(staging,f)||Vector3.Distance(staging.BMBMKPGEHBH,enemy.GetPosition())<8)continue;
   var selected=new List<(AFAGFHJONFJ City,int Days)>();long estimate=0;int longest=0;
   foreach(var choice in known){
    var path=new RoutePlan();foreach(var phase in ReliefPathSteps(choice.City.BMBMKPGEHBH,staging.BMBMKPGEHBH,f,null,false,path))yield return "joint-hunt-"+phase;
    if(!CurrentFaction(f)||!Raider(enemy)||enemy.m_warInfo!=null)yield break;
    if(!path.Success||!JointStagingRoad(path.Points,enemy,f))continue;
    int days=CapacityJourneyDays(path.Length,Wo_DeployTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL,false),AOHIBKLOAEI.NIOKFCDLGFK());
    if(days<0||days>20)continue;selected.Add(choice);longest=Math.Max(longest,days);estimate+=huntPowerSamples[choice.City.OJPECMNINNA+"/"+HuntProbeKey(enemy)].Adjusted;
   }
   if(selected.Count<2||estimate<HuntPower(enemy))continue;
   var g=new JointHunt{Key=key,World=MICGPAMNDKM.IJPKEMPIFIH.Pointer,Faction=f.IBGCLCAIBOL,Target=enemy,TargetId=enemy.m_id,Staging=staging,Started=NEINKOOBJIH.MIPFHCKDJOK,Until=NEINKOOBJIH.MIPFHCKDJOK+longest+35};jointHunts[key]=g;
   bool committed=false;
   try {
    foreach(var choice in selected){
     if(g.Members.Count>=2&&JointHuntPower(g)>=HuntPower(enemy))break;
     yield return "joint-hunt-assemble";
     var c=choice.City;if(JointHuntBlock(g)!=null||!RaiderNear(c,f,enemy)||!AttackPermission(c,f,c.LMPMNLLGOEC.IPJHOOJBCKI)||IncomingHostile(c,f))yield break;
     var teams=AttackTeams();if(!ViableSource(c,teams,AttackReserved(teams))||f.DNOGPFHMOMO<MobilizationCost(c)||teams.Any(t=>t.m_belongPower==f.IBGCLCAIBOL&&t.m_assembleCity==c.OJPECMNINNA&&t.m_deployState==AKOPKFPKJPP.Assemble))yield break;
     Wo_DeployTeamCtrl team;offenseGenerating=true;int previous=capacityTransferFoodDays;
     try{capacityTransferFoodDays=longest+35+anchor.Days*2+15;team=GenerateSupport(c,int.MaxValue,teams,AttackReserved(teams));}finally{capacityTransferFoodDays=previous;offenseGenerating=false;}
     if(!team)yield break;
     var o=new HuntOrder{Team=team,Target=enemy,Id=team.m_id,TargetId=enemy.m_id,Faction=f.IBGCLCAIBOL,Source=c.OJPECMNINNA,Governor=c.LMPMNLLGOEC.IPJHOOJBCKI,Started=g.Started,ProgressDay=NEINKOOBJIH.MIPFHCKDJOK,Position=team.GetPosition(),Joint=g};
     g.Members.Add(o);huntOrders[team.Pointer]=o;team.m_targetCity="";team.m_isEnterCity=false;team.m_attackTarget=null;team.m_tmpTargetPos=c.BMBMKPGEHBH;
     team.m_targetFoodDay=longest+35+anchor.Days*2+15;
    }
    if(JointHuntBlock(g)!=null||JointHuntPower(g)<HuntPower(enemy))yield break;
    g.Started=NEINKOOBJIH.MIPFHCKDJOK;g.Until=g.Started+longest+35;g.Approved=true;committed=true;
    foreach(var o in g.Members){o.Team.m_tmpTargetPos=staging.BMBMKPGEHBH;if(!o.Team.gameObject.activeSelf)o.Team.gameObject.SetActive(true);RouteJointHunt(o);}
    Note($"hunt-joint-approved target={g.TargetId} staging={staging.OJPECMNINNA} sources=[{string.Join(",",g.Members.Select(o=>o.Source))}] teams=[{string.Join(",",g.Members.Select(o=>o.Id))}] combinedAdjusted={JointHuntPower(g)} requiredAdjusted={HuntPower(enemy)} rendezvousDays={longest} fieldRendezvous=true citySlotsRequired=0");
    yield break;
   } finally {if(!committed)AbortJointHunt(g,"preparation-invalid-or-insufficient");}
  }
 }
 static bool RouteJointHunt(HuntOrder o){
  var g=o.Joint;var t=o.Team;if(g==null||g.Ending||!JointMemberLive(o)||ManualProtected(t)||t.m_isStarting||t.m_deployState==AKOPKFPKJPP.Assemble||t.m_warInfo!=null)return false;
  if(!g.Approved){t.StartCamp();return true;}
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(g.Faction,out var f))return false;
  if(Vector3.Distance(t.GetPosition(),g.Staging.BMBMKPGEHBH)<=1){t.ClearTarget(false);t.m_listMovePos?.Clear();t.GAANFLKOMKL?.Clear();t.StartCamp();o.JointRouted=true;return true;}
  if(!ReliefPath(t.GetPosition(),g.Staging.BMBMKPGEHBH,f,null,false,out var points,out double length)||!JointStagingRoad(points,g.Target,f))return false;
  int days=CapacityJourneyDays(length,t.GetMoveSpeed(),AOHIBKLOAEI.NIOKFCDLGFK());if(days<0||days>20)return false;
  t.MoveToDes(g.Staging.BMBMKPGEHBH,false,false);
  if(t.m_deployState!=AKOPKFPKJPP.Expedition||t.m_listMovePos==null)return false;
  t.GAANFLKOMKL?.Clear();t.m_listMovePos.Clear();foreach(var p in points)t.m_listMovePos.Add(p);
  t.IAFGLIMLMGA=g.Staging.BMBMKPGEHBH;t.m_tmpTargetPos=g.Staging.BMBMKPGEHBH;t.m_targetCity="";t.m_tmpTargetCity="";o.JointRouted=true;o.ProgressDay=NEINKOOBJIH.MIPFHCKDJOK;o.Position=t.GetPosition();
  Note($"hunt-joint-route team={o.Id} staging={g.Staging.OJPECMNINNA} roadLength={length:F1} etaDays={days} physicalTravel=true");return true;
 }
 static void AbortJointHunt(JointHunt g,string reason){
  if(g.Ending)return;g.Ending=true;jointHunts.Remove(g.Key);
  Note($"hunt-joint-cancel target={g.TargetId} reason={reason} members={g.Members.Count}");
  foreach(var o in g.Members.ToArray()){
   if(!huntOrders.TryGetValue(o.Team.Pointer,out var own)||!ReferenceEquals(own,o))continue;
   huntOrders.Remove(o.Team.Pointer);o.Joint=null;
   if(g.World!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer||!JointMemberLive(o)||ManualProtected(o.Team)||o.Team.m_isPrivate||o.Team.IncludePlayer()||o.Team.m_responseToPlayer||o.Team.m_warInfo!=null||o.Team.m_targetWarInfo!=null)continue;
   EndHunt(o,"joint-"+reason);
  }
 }
 static void CheckJointHunt(JointHunt g){
  if(g.Ending||!g.Approved||!DecisionDue("hunt-joint:"+g.Key,g.Faction,g.Target.Pointer,"",true))return;
  if(HandoffJointBattle(g))return;
  string block=JointHuntBlock(g);if(block!=null){AbortJointHunt(g,block);return;}
  if(JointHuntPower(g)<HuntPower(g.Target)){AbortJointHunt(g,"combined-power-fell");return;}
  bool arrived=true;double day=NEINKOOBJIH.MIPFHCKDJOK;
  foreach(var o in g.Members){var t=o.Team;
   if(t.m_isStarting||t.m_deployState==AKOPKFPKJPP.Assemble){arrived=false;continue;}
   if(Vector3.Distance(t.GetPosition(),g.Staging.BMBMKPGEHBH)<=1){if(t.m_deployState!=AKOPKFPKJPP.Camp)RouteJointHunt(o);continue;}
   arrived=false;if(!o.JointRouted){if(!RouteJointHunt(o)){AbortJointHunt(g,"rendezvous-route-blocked");return;}}
   if(t.m_listMovePos?.Count>0&&!JointStagingRoad(t.m_listMovePos.ToArray().ToList(),g.Target,NEINKOOBJIH.COCMABJFOFH[g.Faction])){AbortJointHunt(g,"rendezvous-corridor-changed");return;}
   if(Vector3.Distance(t.GetPosition(),o.Position)>.5){o.Position=t.GetPosition();o.ProgressDay=day;}
   else if(day-o.ProgressDay>=6){AbortJointHunt(g,"rendezvous-no-progress");return;}
  }
  if(!arrived)return;
  // Only physically co-located armies are merged using the game's own API.
  // Recheck the resulting army's actual commander-adjusted power afterward.
  var lead=g.Members.OrderByDescending(o=>o.Team.GetCombatVal()).First();long before=g.Members.Sum(o=>(long)o.Team.GetSoldierNum(false));
  try {
   foreach(var o in g.Members.Where(o=>!ReferenceEquals(o,lead)).ToArray()){
    if(JointHuntBlock(g)!=null||Vector3.Distance(o.Team.GetPosition(),lead.Team.GetPosition())>2){AbortJointHunt(g,"merge-precondition-changed");return;}
    o.Team.MergeToTeam(lead.Team);
    if(LiveTeam(o.Team)&&o.Team.m_id==o.Id&&o.Team.GetSoldierNum(false)>0){huntOrders[o.Team.Pointer]=o;AbortJointHunt(g,"native-merge-not-completed");return;}
    huntOrders.Remove(o.Team.Pointer);
    g.Members.Remove(o);
   }
   var team=lead.Team;int days;
   if(JointHuntBlock(g)!=null||team.GetSoldierNum(false)!=before||team.GetCombatVal()<HuntPower(g.Target)||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(g.Faction,out var f)||!HuntRoute(g.Staging,f,g.Target,out days)||team.GetFood()<(long)Math.Max(1,team.GetOneDayFood())*(days*2+15)){AbortJointHunt(g,"merged-power-route-or-food");return;}
   jointHunts.Remove(g.Key);g.Ending=true;lead.Joint=null;lead.ProgressDay=day;lead.Position=team.GetPosition();lead.Started=day;team.SetAttackTarget(g.Target,false);
   Note($"hunt-joint-attack team={team.m_id} target={g.TargetId} soldiersBefore={before} soldiersAfter={team.GetSoldierNum(false)} actualAdjusted={team.GetCombatVal()} requiredAdjusted={HuntPower(g.Target)} nativeMerge=true physicalArrivalVerified=true");
  } catch {AbortJointHunt(g,"native-merge-error");throw;}
 }
 static bool HandoffJointBattle(JointHunt g){
  var war=g.Target?.m_warInfo;if(g.Ending||g.World!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer||war==null||WarSide(war,g.Faction)<0||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(g.Faction,out var f))return false;
  // If the gathering is attacked, do not send its partners home while one
  // member fights. Transfer real troops to the existing battle-support flow.
  g.Ending=true;jointHunts.Remove(g.Key);
  foreach(var o in g.Members.ToArray()){
   if(!huntOrders.TryGetValue(o.Team.Pointer,out var held)||!ReferenceEquals(held,o))continue;
   huntOrders.Remove(o.Team.Pointer);o.Joint=null;var t=o.Team;
   if(!JointMemberLive(o)||ManualProtected(t)||t.m_isPrivate||t.IncludePlayer()||t.m_responseToPlayer)continue;
   if(t.m_warInfo!=null){EndHunt(o,"joined-battle");continue;}
   if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Source,out var city)||city.LMPMNLLGOEC?.LMMNKIOAGLC!=g.Faction||!AttackPermission(city,f,city.LMPMNLLGOEC.IPJHOOJBCKI)||IncomingHostile(city,f)||!PreparedBattlePath(t.GetPosition(),war,f,false,out var road,out double length)){EndHunt(o,"joint-battle-route-or-policy");continue;}
   int days=CapacityJourneyDays(length,t.GetMoveSpeed(),AOHIBKLOAEI.NIOKFCDLGFK());
   if(days<0||days>20||t.m_deployState!=AKOPKFPKJPP.Assemble&&t.GetFood()<(long)Math.Max(1,t.GetOneDayFood())*(days+10)){EndHunt(o,"joint-battle-food-or-travel");continue;}
   if(t.m_deployState==AKOPKFPKJPP.Assemble){t.m_targetCity="";t.m_attackTarget=null;t.m_targetWarInfo=war;}
   else if(!MoveRelief(t,war.IJGLCFJOOIK,war,false,f)){EndHunt(o,"joint-battle-movement-failed");continue;}
   missions[t.Pointer]=new(){Team=t,Id=t.m_id,War=war,Name=war.HIENOINGBCN,Side=WarSide(war,g.Faction),Faction=g.Faction,Source=o.Source,Direct=IsDirect(war),ReadyAt=battleClock,PlannedRoad=road};
   Note($"hunt-joint-battle-relief team={t.m_id} target={g.TargetId} war={war.HIENOINGBCN} etaDays={days} existingContingent=true");
  }return true;
 }
}
