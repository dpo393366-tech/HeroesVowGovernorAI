using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public sealed partial class StrategicSupport {
 // The native moving-army MoveToCity branch calls SetDestination directly.
 // Unlike the stationary branch it does not rebuild the intermediate city
 // queue. Install the very same complete corner chain that was validated.
 [ThreadStatic] static string reliefRouteFailure;
 // Field/village battles have no siege-city reference. Authorize the actual
 // battlefield region only when its owner is already a participant in this war.
 // This does not grant passage through unrelated hostile or neutral regions.
 static string BattleTransitRegion(NJHDEHLHENN war,MKMOJNAJKJE faction){
  if(war?.EMMJBPMNFAK==null||faction==null||!war.EMMJBPMNFAK.Contains(faction.IBGCLCAIBOL))return null;
  var city=war.IJGLCFJOOIK??AOHIBKLOAEI.PNKKPMCJIGA(war.GBHGENNMJCK);
  if(city?.LMPMNLLGOEC==null)return null;
  string owner=city.LMPMNLLGOEC.LMMNKIOAGLC;
  return string.IsNullOrEmpty(owner)||owner==faction.IBGCLCAIBOL||war.EMMJBPMNFAK.Contains(owner)?city.OJPECMNINNA:null;
 }
 sealed class RoutePlan {public NJHDEHLHENN CorridorBlocker;public bool CorridorDetour;public bool Success;public double Length;public List<Vector3> Points=new();public string Failure="native-route-or-territory";public List<Vector3> Legs=new();}
 // Planning callers consume one native territory sample per scheduler slice.
 // Final native command validation drains the exact same checks synchronously.
 static bool ReliefPath(Vector3 from,Vector3 to,MKMOJNAJKJE f,string allowed,bool escape,out List<Vector3> points,out double length){
  var result=new RoutePlan();foreach(var phase in ReliefPathAvoidSteps(from,to,f,allowed,escape,result,null,false)){}
  points=result.Points;length=result.Length;reliefRouteFailure=result.Failure;return result.Success;
 }
 static IEnumerable<string> ReliefPathSteps(Vector3 from,Vector3 to,MKMOJNAJKJE f,string allowed,bool escape,RoutePlan result){
  foreach(var phase in ReliefPathAvoidSteps(from,to,f,allowed,escape,result,null))yield return phase;
 }
 static IEnumerable<string> ReliefPathAvoidSteps(Vector3 from,Vector3 to,MKMOJNAJKJE f,string allowed,bool escape,RoutePlan result,Func<List<Vector3>,bool> passage,bool reuseGeometry=true){
  result.Success=false;result.Failure="native-route-or-territory";
  var generation=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;string key=RoadCacheKey(from,to,f,allowed,escape),stamp=RoadPolicyStamp(f);
  yield return "route-policy";if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation)yield break;
  if(passage==null&&roadChoices.TryGetValue(key,out var cached)&&cached.World==generation&&cached.Stamp==stamp){
   var at=from;var replay=new RoutePlan{Success=true};
   foreach(var end in cached.Legs){
    var leg=new RoutePlan();foreach(var phase in ReliefPathCoreSteps(at,end,f,allowed,escape&&at.Equals(from),null,leg,reuseGeometry)){yield return phase;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation)yield break;}
    if(!leg.Success){replay.Success=false;break;}AppendRoad(replay,leg);replay.Legs.Add(end);at=end;
   }
   if(replay.Success&&Math.Abs(replay.Length-cached.Length)<.01){
    // Replay can yield after checking passage. Ownership/diplomacy may change
    // before its native path returns, so a cached route needs the same final
    // policy validation as a newly calculated one.
    if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||RoadPolicyStamp(f)!=stamp){result.Success=false;result.Failure="route-policy-changed";yield break;}
    CopyRoad(result,replay);result.Failure="validated-shortest-road-cache";yield break;
   }
   roadChoices.Remove(key);
  }
  var direct=new RoutePlan();foreach(var phase in ReliefPathCoreSteps(from,to,f,allowed,escape,null,direct,reuseGeometry)){yield return phase;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation)yield break;}
  if(direct.Success&&passage!=null&&!passage(direct.Points))direct.Success=false;
  if(direct.Success){direct.Legs.Add(to);CopyRoad(result,direct);}
  var origin=AOHIBKLOAEI.PNKKPMCJIGA(from);var destination=AOHIBKLOAEI.PNKKPMCJIGA(to);
  if(origin!=null&&destination!=null&&origin.Pointer!=destination.Pointer){
   bool Friendly(AFAGFHJONFJ c){string owner=c?.LMPMNLLGOEC?.LMMNKIOAGLC;return c!=null&&(string.IsNullOrEmpty(owner)||owner==f.IBGCLCAIBOL||(f.AENJACOFBJK.TryGetValue(owner,out var rel)&&rel!=null&&(rel.EBIOPDHEBOF==BHBNKONCOJD.Ally||rel.EBIOPDHEBOF==BHBNKONCOJD.ArmyPass)));}
   var open=new List<AFAGFHJONFJ>{origin};var settled=new HashSet<IntPtr>();
   var plans=new Dictionary<IntPtr,RoutePlan>{{origin.Pointer,new RoutePlan{Success=true,Length=0,Points=new(){from}}}};
   while(open.Count>0){
    yield return "route-graph";if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation){result.Success=false;yield break;}
    var last=open.OrderBy(c=>plans[c.Pointer].Length).First();open.Remove(last);if(!settled.Add(last.Pointer))continue;
    var current=plans[last.Pointer];if(result.Success&&current.Length>=result.Length-.001)break;
    if(last.Pointer==destination.Pointer){CopyRoad(result,current);break;}
    if(last.FKMKGNPFLFP==null)continue;
    foreach(var neighbor in last.FKMKGNPFLFP.ToArray()){
     if(neighbor==null||settled.Contains(neighbor.Pointer)||(!Friendly(neighbor)&&neighbor.Pointer!=destination.Pointer))continue;
     var leg=new RoutePlan();var end=neighbor.Pointer==destination.Pointer?to:neighbor.BMBMKPGEHBH;
     var begin=last.Pointer==origin.Pointer?from:last.BMBMKPGEHBH;
     foreach(var phase in ReliefPathCoreSteps(begin,end,f,allowed,escape&&last.Pointer==origin.Pointer,null,leg,reuseGeometry)){yield return phase;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation){result.Success=false;yield break;}}
     if(!leg.Success||passage!=null&&!passage(leg.Points))continue;double distance=current.Length+leg.Length;
     if(result.Success&&distance>=result.Length-.001||plans.TryGetValue(neighbor.Pointer,out var old)&&old.Length<=distance)continue;
     var candidate=new RoutePlan{Success=true,Length=current.Length,Points=new(current.Points),Legs=new(current.Legs)};AppendRoad(candidate,leg);candidate.Legs.Add(end);
     plans[neighbor.Pointer]=candidate;if(!open.Any(c=>c.Pointer==neighbor.Pointer))open.Add(neighbor);
    }
   }
  }
  if(RoadPolicyStamp(f)!=stamp){result.Success=false;yield break;}
  if(result.Success){if(direct.Success&&result.Length+.01<direct.Length&&Gate("road-improved:"+key,15))Note($"road-shortest-improved faction={f.IBGCLCAIBOL} from={origin?.OJPECMNINNA} to={destination?.OJPECMNINNA} presetLength={direct.Length:F2} shortestLength={result.Length:F2} legs={result.Legs.Count} basis=native-road-graph");result.Failure="validated-shortest-native-road-graph";if(passage==null){if(roadChoices.Count>=8192&&!roadChoices.ContainsKey(key))roadChoices.Remove(roadChoices.Keys.First());roadChoices[key]=new RoadChoice{World=generation,Stamp=stamp,Until=long.MaxValue,Length=result.Length,Legs=new(result.Legs)};}}
 }
 static IEnumerable<string> ReliefPathCoreSteps(Vector3 from,Vector3 to,MKMOJNAJKJE f,string allowed,bool escape,List<Vector3> forcedLegs,RoutePlan result,bool reuseGeometry=false){
  result.Success=false;var points=new List<Vector3>();double length=0;var generation=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;var origin=AOHIBKLOAEI.PNKKPMCJIGA(from);var dest=AOHIBKLOAEI.PNKKPMCJIGA(to);
  var legs=new List<Vector3>();var seen=new HashSet<IntPtr>();
  bool Permitted(AFAGFHJONFJ c){var owner=c?.LMPMNLLGOEC?.LMMNKIOAGLC;return string.IsNullOrEmpty(owner)||owner==f.IBGCLCAIBOL||c.OJPECMNINNA==allowed||(f.AENJACOFBJK.TryGetValue(owner,out var r)&&r!=null&&(r.EBIOPDHEBOF==BHBNKONCOJD.Ally||r.EBIOPDHEBOF==BHBNKONCOJD.ArmyPass));}
  if(origin!=null&&!Permitted(origin)&&!escape){result.Failure="origin-territory:"+origin.OJPECMNINNA;yield break;}
  if(dest!=null&&!Permitted(dest)){result.Failure="destination-territory:"+dest.OJPECMNINNA;yield break;}
  if(forcedLegs==null&&origin!=null&&dest!=null&&origin.Pointer!=dest.Pointer&&origin.MEKIDAONKMD!=null&&origin.MEKIDAONKMD.TryGetValue(dest.OJPECMNINNA,out var road)&&road?.EJMDIPFNGMG!=null){
   if(road.EJMDIPFNGMG.Count>32)yield break;seen.Add(origin.Pointer);
   foreach(var via in road.EJMDIPFNGMG){if(via==null||!seen.Add(via.Pointer)||!Permitted(via)){result.Failure="native-intermediate-forbidden:"+via?.OJPECMNINNA;yield break;}if(via.Pointer!=dest.Pointer)legs.Add(via.BMBMKPGEHBH);}
  }
  if(forcedLegs!=null)legs.AddRange(forcedLegs);else legs.Add(to);var at=from;
  foreach(var leg in legs){
   FixedRoadLeg geometry=null;
   if(!reuseGeometry||!SavedFixedRoad(at,leg,out geometry)){
    yield return "route-native-path";if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation)yield break;
    if(!CalculateFixedRoad(at,leg,out geometry))yield break;
   }
   length+=geometry.Length;
   foreach(var p in geometry.Points)if(points.Count==0||Vector3.Distance(points[points.Count-1],p)>.01f){points.Add(p);if(points.Count>1024)yield break;}
   at=geometry.End;
  }
  result.Points=points;result.Length=length;result.Success=points.Count>0&&double.IsFinite(length);
 }
 static bool MoveRelief(Wo_DeployTeamCtrl t,AFAGFHJONFJ target,NJHDEHLHENN war,bool enter,MKMOJNAJKJE f,bool escape=false){
  if(!t||t.m_warInfo!=null||t.m_isPrivate||t.m_responseToPlayer||t.IncludePlayer())return false;
  Vector3 end=war!=null?war.GBHGENNMJCK:target.BMBMKPGEHBH;
  string allowed=war!=null?BattleTransitRegion(war,f):!enter&&target?.LMPMNLLGOEC!=null&&AttackableOwner(f,target.LMPMNLLGOEC.LMMNKIOAGLC)?target.OJPECMNINNA:null;
  List<Vector3> points;double length;
  if(war!=null){
   if(missions.TryGetValue(t.Pointer,out var planned)&&ValidMission(planned)&&Same(planned.War,war)&&TrimCorridorRoad(t.GetPosition(),planned.PlannedRoad,out points)&&FirstCorridorBlock(points,war,f)==null)length=CorridorLength(points);
   else if(!PreparedBattlePath(t.GetPosition(),war,f,escape,out points,out length))return false;
  }
  else if(!ReliefPath(t.GetPosition(),end,f,allowed,escape,out points,out length))return false;
  if(war!=null)t.MoveToWarInfo(war,false);
  else {if(!ICGAEMLACJG.FKKCPDEFODO.TryGetValue(target.OJPECMNINNA,out var ctrl)||!ctrl)return false;t.MoveToCity(ctrl,enter,false);}
  if(t.m_deployState!=AKOPKFPKJPP.Expedition||t.m_listMovePos==null)return false;
  t.GAANFLKOMKL?.Clear();t.m_listMovePos.Clear();foreach(var p in points)t.m_listMovePos.Add(p);
  t.IAFGLIMLMGA=end;t.m_tmpTargetPos=end;t.m_tmpTargetCity="";
  Note($"relief-route-installed team={t.m_id} destination={target?.OJPECMNINNA} war={war?.HIENOINGBCN} length={length:F1} corners={points.Count} nativeWaypoints={t.GAANFLKOMKL?.Count} start={t.GetPosition()} end={end} entry={enter} exactValidatedPath=true");return true;
 }
 static void RouteNewRelief(Wo_DeployTeamCtrl t){
  if(!t||ManualProtected(t)||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(t.m_belongPower,out var f))return;
  RestoreCampaignDeparture(t,f);
  if(t.m_deployState!=AKOPKFPKJPP.Expedition)return;
  if(fieldReliefOrders.ContainsKey(t.Pointer)){RouteFieldDeparture(t);return;}
  if(defenseOrders.TryGetValue(t.Pointer,out var o)&&SameDefense(o)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Target,out var city)){
   o.Routed=MoveRelief(t,city,null,false,f);if(!o.Routed){t.StartCamp();Note($"relief-route-deferred team={t.m_id} target={o.Target}");}
  }else if(coverOrders.TryGetValue(t.Pointer,out var cover)&&ValidCover(cover)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(cover.Target,out var target)){
   cover.Routed=MoveRelief(t,target,null,false,f);if(!cover.Routed)t.StartCamp();
  }else if(missions.TryGetValue(t.Pointer,out var m)&&ValidMission(m)){
   if(!MoveRelief(t,m.War.IJGLCFJOOIK,m.War,false,f)){t.StartCamp();work.Add("battle-route:reroute:"+t.m_id,()=>RepairCorridorSteps(t,m,f).GetEnumerator());Note($"battle-route-install-failed team={t.m_id} war={m.Name} repairQueued=true");}
  }
 }
 static void RestoreCampaignDeparture(Wo_DeployTeamCtrl t,MKMOJNAJKJE f){
  // GenDivideTeam builds its model asynchronously. Its eventual StartDeploy
  // calls MoveToCity(...,false,false), which erases an already committed entry
  // order. Restore that same validated order after native initialization.
  if(!campaignGoals.TryGetValue(t.Pointer,out var goal)||goal.Id!=t.m_id||goal.Faction!=t.m_belongPower||!goal.Ordered||!CampaignControlled(t,out _)||t.m_warInfo!=null||t.m_targetWarInfo!=null||OtherCampaignOrder(t))return;
  if(t.m_deployState!=AKOPKFPKJPP.Expedition&&t.m_deployState!=AKOPKFPKJPP.Camp)return;
  bool enter=goal.Returning&&!goal.Staging;
  if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(goal.OrderTarget??"",out var city))return;
  if(goal.Returning&&!ReturnSafe(t,city,f,AttackTeams(),0,enter)){
   goal.Ordered=false;t.ClearTarget(false);t.m_tmpTargetCity="";t.m_listMovePos?.Clear();t.GAANFLKOMKL?.Clear();t.StartCamp();
   work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());return;
  }
  if(MoveRelief(t,city,null,enter,f,true))Note($"campaign-departure-restored team={t.m_id} target={goal.OrderTarget} entry={enter} path={t.m_listMovePos?.Count} reason=native-model-startup-reissued-non-entry-order");
  else {goal.Ordered=false;t.StartCamp();work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());}
 }
}

