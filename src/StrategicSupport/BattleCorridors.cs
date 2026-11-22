using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed partial class StrategicSupport {
 // Native KDNMJMEGCJM at RVA 0x36f2469 loads squared distance 20.0;
 // 0x36f2528 compares all three coordinates, then calls LHJOHJKDCJA.
 // Check the road polyline against that sphere, NOT the nearest city or
 // a straight line between endpoints. No collision rule is patched.
 const double CorridorRadiusSquared=20;
 sealed class CorridorWar {public NJHDEHLHENN War;public string Name,A,B;public Vector3 Position;}
 static readonly Dictionary<IntPtr,CorridorWar> corridorWars=new();
 static long corridorRevision;
 static bool CorridorWarAlive(NJHDEHLHENN w)=>w?.EMMJBPMNFAK?.Length==2&&w.FGHFOLKBHDL?.Length==2&&w.FGHFOLKBHDL[0]>0&&w.FGHFOLKBHDL[1]>0;
 static void ResetBattleCorridors(){corridorWars.Clear();corridorRevision++;}
 static void ForgetCorridor(NJHDEHLHENN w){if(corridorWars.Remove(w.Pointer))corridorRevision++;}
 static void RefreshBattleCorridors(List<Wo_DeployTeamCtrl> teams){
  var seen=new HashSet<IntPtr>();
  void Add(NJHDEHLHENN w){if(!CorridorWarAlive(w)||!seen.Add(w.Pointer))return;
   if(corridorWars.TryGetValue(w.Pointer,out var old)&&old.Name==w.HIENOINGBCN&&old.A==w.EMMJBPMNFAK[0]&&old.B==w.EMMJBPMNFAK[1]&&old.Position.Equals(w.GBHGENNMJCK))return;
   corridorWars[w.Pointer]=new(){War=w,Name=w.HIENOINGBCN,A=w.EMMJBPMNFAK[0],B=w.EMMJBPMNFAK[1],Position=w.GBHGENNMJCK};corridorRevision++;
  }
  foreach(var t in teams)if(t)Add(t.m_warInfo);
  if(!Wa_Controller.m_isQuiting)Add(Wa_Controller.m_warInfo);
  foreach(var key in corridorWars.Keys.ToArray())if(!seen.Contains(key)){corridorWars.Remove(key);corridorRevision++;}
 }
 // Return first sphere-entry distance along a real road. Infinity means no
 // entry (including a tangent: the native comparison is strictly < 20).
 readonly struct CorridorSegment {
  public readonly Vector3 A,B;public readonly double Length,Start;
  public CorridorSegment(Vector3 a,Vector3 b,double start){A=a;B=b;Start=start;Length=Vector3.Distance(a,b);}
 }
 sealed class CorridorTrace {
  readonly CorridorSegment[] segments;readonly Vector3 first;readonly bool empty;
  readonly double minX,maxX,minY,maxY,minZ,maxZ;const double radius=4.472135955;
  public CorridorTrace(IReadOnlyList<Vector3> points){
   empty=points==null||points.Count==0;segments=new CorridorSegment[empty?0:Math.Max(0,points.Count-1)];if(empty)return;first=points[0];
   double lx=first.x,hx=first.x,ly=first.y,hy=first.y,lz=first.z,hz=first.z,walked=0;
   for(int i=0;i<points.Count;i++){var p=points[i];lx=Math.Min(lx,p.x);hx=Math.Max(hx,p.x);ly=Math.Min(ly,p.y);hy=Math.Max(hy,p.y);lz=Math.Min(lz,p.z);hz=Math.Max(hz,p.z);if(i>0){segments[i-1]=new(points[i-1],p,walked);walked+=segments[i-1].Length;}}
   minX=lx-radius;maxX=hx+radius;minY=ly-radius;maxY=hy+radius;minZ=lz-radius;maxZ=hz+radius;
  }
  public double Contact(Vector3 center){
   if(empty||center.x<minX||center.x>maxX||center.y<minY||center.y>maxY||center.z<minZ||center.z>maxZ)return double.PositiveInfinity;
   double fx=first.x-center.x,fy=first.y-center.y,fz=first.z-center.z;if(fx*fx+fy*fy+fz*fz<CorridorRadiusSquared)return 0;
   foreach(var s in segments){var a=s.A;var b=s.B;
    if(center.x<Math.Min(a.x,b.x)-radius||center.x>Math.Max(a.x,b.x)+radius||center.y<Math.Min(a.y,b.y)-radius||center.y>Math.Max(a.y,b.y)+radius||center.z<Math.Min(a.z,b.z)-radius||center.z>Math.Max(a.z,b.z)+radius)continue;
    double x=a.x-center.x,y=a.y-center.y,z=a.z-center.z,c=x*x+y*y+z*z-CorridorRadiusSquared;if(c<0)return s.Start;
    double dx=b.x-a.x,dy=b.y-a.y,dz=b.z-a.z,aa=dx*dx+dy*dy+dz*dz;if(aa<=1e-12)continue;
    double bb=x*dx+y*dy+z*dz,disc=bb*bb-aa*c;
    if(disc>0){double at=(-bb-Math.Sqrt(disc))/aa;if(at>=0&&at<1)return s.Start+at*s.Length;}
   }
   return double.PositiveInfinity;
  }
 }
 static double CorridorContact(IReadOnlyList<Vector3> points,Vector3 center)=>new CorridorTrace(points).Contact(center);
 static NJHDEHLHENN FirstCorridorBlock(IReadOnlyList<Vector3> points,NJHDEHLHENN target,MKMOJNAJKJE f){
  if(points==null||points.Count==0||f==null)return null;
  // An overlap at exactly the same entry distance is ambiguous: do not
  // advertise it as guaranteed arrival at the desired battle.
  var trace=new CorridorTrace(points);double first=target==null?double.PositiveInfinity:trace.Contact(target.GBHGENNMJCK);NJHDEHLHENN found=null;
  foreach(var b in corridorWars.Values){
   if(Same(b.War,target)||!CorridorWarAlive(b.War)||b.War.HIENOINGBCN!=b.Name)continue;
   if(b.A!=f.IBGCLCAIBOL&&b.B!=f.IBGCLCAIBOL&&!War(f,b.A)&&!War(f,b.B))continue;
   double at=trace.Contact(b.Position);if(double.IsFinite(at)&&at<=first){first=at;found=b.War;}
  }
  return found;
 }
 static IEnumerable<string> AvoidBattleCorridors(Vector3 from,NJHDEHLHENN target,MKMOJNAJKJE f,RoutePlan result){
  if(!result.Success)yield break;
  var blocker=FirstCorridorBlock(result.Points,target,f);if(blocker==null)yield break;
  long version=corridorRevision;var alternative=new RoutePlan();
  foreach(var phase in ReliefPathAvoidSteps(from,target.GBHGENNMJCK,f,BattleTransitRegion(target,f),false,alternative,p=>FirstCorridorBlock(p,target,f)==null)){
   yield return phase;if(version!=corridorRevision){result.Success=false;result.Failure="corridor-changed-during-plan";yield break;}
  }
  if(alternative.Success){CopyRoad(result,alternative);result.CorridorDetour=true;result.Failure="battle-corridor-detour";}
  else {result.Success=false;result.CorridorBlocker=blocker;result.Failure="battle-corridor-blocked:"+blocker.HIENOINGBCN;}
 }
 static bool ValidateBattleDeparture(AFAGFHJONFJ source,NJHDEHLHENN war,MKMOJNAJKJE f,out List<Vector3> points,out double length){
  points=null;length=0;
  if(savedBattleRoads.TryGetValue(BattleRoadKey(source,war,f),out var cached)&&cached.Detour&&cached.Success&&cached.Stamp==RoadPolicyStamp(f)&&cached.Legs?.Count>0){
   var result=new RoutePlan{Success=true};var at=source.BMBMKPGEHBH;
   foreach(var end in cached.Legs){var leg=new RoutePlan();foreach(var phase in ReliefPathCoreSteps(at,end,f,BattleTransitRegion(war,f),false,null,leg)){}
    if(!leg.Success||FirstCorridorBlock(leg.Points,war,f)!=null)return false;AppendRoad(result,leg);at=end;
   }
   points=result.Points;length=result.Length;return true;
  }
  return ReliefPath(source.BMBMKPGEHBH,war.GBHGENNMJCK,f,BattleTransitRegion(war,f),false,out points,out length)&&FirstCorridorBlock(points,war,f)==null;
 }
 static bool PreparedBattlePath(Vector3 from,NJHDEHLHENN war,MKMOJNAJKJE f,bool escape,out List<Vector3> points,out double length){
  points=null;length=0;string key=RoadCacheKey(from,war.GBHGENNMJCK,f,BattleTransitRegion(war,f),false);
  if(savedBattleRoads.TryGetValue(key,out var cached)&&cached.Success&&cached.Points?.Count>0&&cached.World==MICGPAMNDKM.IJPKEMPIFIH?.Pointer&&cached.Stamp==RoadPolicyStamp(f)&&FirstCorridorBlock(cached.Points,war,f)==null){points=cached.Points;length=cached.Length;return true;}
  if(!ReliefPath(from,war.GBHGENNMJCK,f,BattleTransitRegion(war,f),escape,out points,out length))return false;
  var blocked=FirstCorridorBlock(points,war,f);if(blocked==null)return true;
  reliefRouteFailure="battle-corridor-blocked:"+blocked.HIENOINGBCN;return false;
 }
 static void RequestCorridorBreakthrough(AFAGFHJONFJ source,NJHDEHLHENN target,MKMOJNAJKJE f){
  if(!savedBattleRoads.TryGetValue(BattleRoadKey(source,target,f),out var road)||road.Corridors!=corridorRevision||road.Blocker==null)return;
  var blocker=road.Blocker;int side=WarSide(blocker,f.IBGCLCAIBOL);if(side<0||!CorridorWarAlive(blocker))return;
  // One deduplicated request per blocking front, not per source/original
  // target. Its normal forecast accounts for all already-committed waves.
  work.Add("battle-route:breakthrough:"+blocker.Pointer+":"+f.IBGCLCAIBOL,()=>CorridorBreakthroughSteps(blocker,f,target.HIENOINGBCN).GetEnumerator());
  if(Gate("corridor-block:"+target.Pointer+":"+source.OJPECMNINNA,30))Note($"battle-corridor-blocked source={source.OJPECMNINNA} original={target.HIENOINGBCN} blocker={blocker.HIENOINGBCN} action=assess-breakthrough originalArrivalCredit=0");
 }
 static IEnumerable<string> CorridorBreakthroughSteps(NJHDEHLHENN blocker,MKMOJNAJKJE f,string original){
  yield return "corridor-breakthrough";
  if(!enabled.Value||!battleSupport.Value||!CurrentFaction(f)||!CorridorWarAlive(blocker)||!corridorWars.TryGetValue(blocker.Pointer,out var live)||live.Name!=blocker.HIENOINGBCN)yield break;
  // Retain normal player/NPC decision gates; don't bypass the demand or
  // source reserve rules merely because another battle depends on this one.
  Assess(blocker,IsDirect(blocker)?battleClock:NEINKOOBJIH.MIPFHCKDJOK*(double)AOHIBKLOAEI.NIOKFCDLGFK(),IsDirect(blocker));
 }
 static bool CorridorArrivalClear(Wo_DeployTeamCtrl t,NJHDEHLHENN target,MKMOJNAJKJE f){
  if(corridorWars.Count==0)return true;
  var points=new List<Vector3>{t.GetPosition()};if(t.m_listMovePos!=null)foreach(var p in t.m_listMovePos)points.Add(p);
  if(points.Count==1&&t.m_deployState==AKOPKFPKJPP.Assemble)return true; // source plan checks departure
  var blocked=FirstCorridorBlock(points,target,f);if(blocked==null&&points.Count>1)return true;
  if(ManualProtected(t)||t.m_isPrivate||t.IncludePlayer()||t.m_responseToPlayer||!CampSupportPermission(t,f))return false;
  if(!missions.TryGetValue(t.Pointer,out var mission)){
   int side=WarSide(target,f.IBGCLCAIBOL);if(side<0)return false;
   missions[t.Pointer]=mission=new Mission{Team=t,Id=t.m_id,War=target,Name=target.HIENOINGBCN,Side=side,Faction=f.IBGCLCAIBOL,Source=t.m_assembleCity,Direct=IsDirect(target),ReadyAt=battleClock};
   Note($"battle-corridor-adopt team={t.m_id} source={t.m_assembleCity} original={target.HIENOINGBCN} manualProtected=false");
  }
  if(ValidMission(mission))work.Add("battle-route:reroute:"+t.m_id,()=>RepairCorridorSteps(t,mission,f).GetEnumerator());
  return false;
 }
 static IEnumerable<string> RepairCorridorSteps(Wo_DeployTeamCtrl t,Mission mission,MKMOJNAJKJE f){
  bool Valid()=>enabled.Value&&battleSupport.Value&&ValidMission(mission)&&missions.TryGetValue(t.Pointer,out var current)&&ReferenceEquals(current,mission)&&!ManualProtected(t)&&!t.m_responseToPlayer&&!t.m_isPrivate&&!t.IncludePlayer()&&CurrentFaction(f);
  if(!Valid())yield break;var target=mission.War;var start=t.GetPosition();long revision=corridorRevision;var route=new RoutePlan();
  string policy=RoadPolicyStamp(f);
  if(mission.FailedCorridor?.CorridorBlocker!=null&&mission.FailedCorridorRevision==revision&&mission.FailedCorridorPolicy==policy&&mission.FailedCorridorStart.Equals(start))route=mission.FailedCorridor;
  else {
   foreach(var phase in ReliefPathSteps(start,target.GBHGENNMJCK,f,BattleTransitRegion(target,f),false,route)){yield return phase;if(!Valid()||revision!=corridorRevision)yield break;}
   foreach(var phase in AvoidBattleCorridors(start,target,f,route)){yield return phase;if(!Valid()||revision!=corridorRevision)yield break;}
  }
  if(policy!=RoadPolicyStamp(f))yield break;
  if(!Valid())yield break;
  if(route.Success){
   // The route may have moved less than a unit while planning: include the
   // connecting segment in the final combat check before installing it.
   if(!TrimCorridorRoad(t.GetPosition(),route.Points,out var install))yield break;
   if(FirstCorridorBlock(install,target,f)!=null)yield break;
   InstallCorridorRoad(t,target,install);mission.PlannedRoad=install;Note($"battle-corridor-reroute team={t.m_id} original={mission.Name} action=detour corners={install.Count}");yield break;
  }
  var blocker=route.CorridorBlocker;if(blocker==null||WarSide(blocker,f.IBGCLCAIBOL)<0)yield break;
  mission.FailedCorridor=route;mission.FailedCorridorRevision=revision;mission.FailedCorridorPolicy=policy;mission.FailedCorridorStart=start;
  var teams=AttackTeams();
  // A cached hold still re-evaluates combat demand. The geometric search is
  // only repeated when the road/war changes, not once per waiting army/day.
  double approach=CorridorContact(route.Points,blocker.GBHGENNMJCK)/Math.Max(.01,t.GetMoveSpeed());
  if(!double.IsFinite(approach))approach=maxTravelSeconds.Value;
  if(!CampBattleNeedAt(t,blocker,f,teams,approach,out int need,out int side)||!CampBattleNeed(t,blocker,f,teams,out double eta,out need,out side)){
   // No more donors are sent into this corridor. Already mobilized relief
   // waits only for this live obstruction; the next assessment reopens it
   // after a topology change. Do not dismantle it and recruit it again.
   if(t.GetFood()<(long)Math.Max(1,t.GetOneDayFood())*5){missions.Remove(t.Pointer);t.ClearTarget(false);t.StartCamp();Note($"battle-corridor-release team={t.m_id} original={mission.Name} reason=low-food action=campaign-reassessment");yield break;}
   if(Vector3.Distance(t.GetPosition(),blocker.GBHGENNMJCK)>Math.Sqrt(CorridorRadiusSquared)&&t.m_deployState!=AKOPKFPKJPP.Camp){t.StartCamp();t.m_targetWarInfo=target;t.m_listMovePos?.Clear();t.GAANFLKOMKL?.Clear();}
   if(Gate("corridor-hold:"+t.m_id,30))Note($"battle-corridor-hold team={t.m_id} original={mission.Name} blocker={blocker.HIENOINGBCN} reason=blocker-covered-or-arrival-not-useful originalArrivalCredit=0");
   yield break;
  }
  if(!MoveRelief(t,blocker.IJGLCFJOOIK,blocker,false,f))yield break;
  string original=mission.Name;mission.War=blocker;mission.Name=blocker.HIENOINGBCN;mission.Side=side;mission.Direct=IsDirect(blocker);mission.ReadyAt=battleClock;
  Note($"battle-corridor-reroute team={t.m_id} original={original} blocker={mission.Name} action=breakthrough soldiers={t.GetSoldierNum(false)} need={need} originalArrivalCredit=0");
 }
 static double CorridorLength(IReadOnlyList<Vector3> points){double length=0;for(int i=1;i<points.Count;i++)length+=Vector3.Distance(points[i-1],points[i]);return length;}
 static bool TrimCorridorRoad(Vector3 current,List<Vector3> original,out List<Vector3> remaining){
  remaining=null;if(original==null||original.Count<2)return false;
  int best=-1;double distance=double.PositiveInfinity;
  for(int i=1;i<original.Count;i++){
   var a=original[i-1];var b=original[i];double dx=b.x-a.x,dy=b.y-a.y,dz=b.z-a.z,len=dx*dx+dy*dy+dz*dz;
   double at=len<1e-12?0:Math.Clamp(((current.x-a.x)*dx+(current.y-a.y)*dy+(current.z-a.z)*dz)/len,0,1);
   double x=current.x-a.x-at*dx,y=current.y-a.y-at*dy,z=current.z-a.z-at*dz,d=x*x+y*y+z*z;
   if(d<distance){best=i;distance=d;}
  }
  if(best<0||distance>1)return false;
  remaining=new(){current};for(int i=best;i<original.Count;i++)remaining.Add(original[i]);return true;
 }
 static void InstallCorridorRoad(Wo_DeployTeamCtrl t,NJHDEHLHENN war,List<Vector3> points){
  t.MoveToWarInfo(war,false);t.GAANFLKOMKL?.Clear();t.m_listMovePos.Clear();foreach(var p in points)t.m_listMovePos.Add(p);t.m_tmpTargetPos=war.GBHGENNMJCK;t.IAFGLIMLMGA=war.GBHGENNMJCK;t.m_tmpTargetCity="";
 }
 static void ObserveCorridorOutcome(Mission m){
  if(!m.Team||m.Team.m_id!=m.Id||m.Team.m_belongPower!=m.Faction)return;
  var actual=m.Team.m_warInfo;
  if(actual!=null)Note($"battle-support-arrival team={m.Id} source={m.Source} original={m.Name} actual={actual.HIENOINGBCN} intercepted={!Same(actual,m.War)} soldiers={m.Team.GetSoldierNum(false)}");
 }
}
