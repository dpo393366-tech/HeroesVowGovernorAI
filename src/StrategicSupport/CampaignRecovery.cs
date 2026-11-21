using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public sealed partial class StrategicSupport {
 sealed class CampaignGoal {
  public int Id; public string Faction,Goal,OrderTarget; public bool Returning,Ordered,MovingLogged,Staging,CompletedOrder;
  public Vector3 Position; public double OrderedDay,CheckedDay=-1; public int Failures,CheckedPeriod;
  public readonly Dictionary<string,double> RetryAfter=new();
 }
 static readonly Dictionary<IntPtr,CampaignGoal> campaignGoals=new();
 static bool CampaignControlled(Wo_DeployTeamCtrl t,out MKMOJNAJKJE f){
  f=null;
  if(!t||ManualProtected(t)||defenseOrders.ContainsKey(t.Pointer)||coverOrders.ContainsKey(t.Pointer)||t.m_isPrivate||t.m_responseToPlayer||t.IncludePlayer()||t.m_isStarting||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(t.m_belongPower??"",out f))return false;
  if(campaignGoals.TryGetValue(t.Pointer,out var completed)&&completed.Id==t.m_id&&completed.Faction==t.m_belongPower&&completed.CompletedOrder)return true;
  if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_assembleCity??"",out var home)&&home.LMPMNLLGOEC?.LMMNKIOAGLC==t.m_belongPower)return DistributionCity(home,out _);
  // An army whose origin was conquered can still return. Protect personal armies.
  return !string.IsNullOrEmpty(t.m_belongGroup)&&t.m_belongGroup!=NEINKOOBJIH.OPIKIHPNCIP&&f.ANMMMOIKLEG.TryGetValue(t.m_belongGroup,out var g)&&g.LMMNKIOAGLC==t.m_belongPower;
 }
 static bool OtherCampaignOrder(Wo_DeployTeamCtrl t)=>!string.IsNullOrEmpty(t.m_targetCamp)||t.m_targetVillage!=null||t.m_targetPass!=null||t.m_attackTarget||t.m_mergeTarget;
 // Native battle cleanup can leave Expedition with no goal/path and isStatic.
 // Only adopt truly orphaned marches; an explicit city order or pending native
 // waypoint still belongs to native movement and must not be replaced here.
 static bool CampIdle(Wo_DeployTeamCtrl t)=>t&&!t.m_isStarting&&t.m_warInfo==null&&t.m_targetWarInfo==null&&!OtherCampaignOrder(t)&&(t.m_listMovePos==null||t.m_listMovePos.Count==0)&&
  (t.m_deployState==AKOPKFPKJPP.Camp||(t.m_deployState==AKOPKFPKJPP.Expedition&&string.IsNullOrEmpty(t.m_targetCity)&&string.IsNullOrEmpty(t.m_tmpTargetCity)&&(t.GAANFLKOMKL==null||t.GAANFLKOMKL.Count==0)));
 static void RememberCampaign(Wo_DeployTeamCtrl t){
  if(!CampaignControlled(t,out _)||OtherCampaignOrder(t))return;
  string goal=!string.IsNullOrEmpty(t.m_targetCity)?t.m_targetCity:t.m_tmpTargetCity;
  if(string.IsNullOrEmpty(goal)){
   if(!CampIdle(t))return;
   if(campaignGoals.TryGetValue(t.Pointer,out var known)&&known.Id==t.m_id&&known.Faction==t.m_belongPower)return;
   // A surviving/merged battle army can be a NEW native object. Reconstruct
   // intent from current position, not from a lost pre-battle pointer.
   goal=AOHIBKLOAEI.PNKKPMCJIGA(t.GetPosition())?.OJPECMNINNA??t.m_assembleCity;
   campaignGoals[t.Pointer]=new CampaignGoal{Id=t.m_id,Faction=t.m_belongPower,Goal=goal};
   Note($"campaign-orphan-adopted team={t.m_id} faction={t.m_belongPower} state={t.m_deployState} source={t.m_assembleCity} location={goal} soldiers={t.GetSoldierNum(false)} reason=no-goal-no-path");return;
  }
  if(!campaignGoals.TryGetValue(t.Pointer,out var s)||s.Id!=t.m_id||s.Faction!=t.m_belongPower)
   campaignGoals[t.Pointer]=new CampaignGoal{Id=t.m_id,Faction=t.m_belongPower,Goal=goal,Returning=t.m_isEnterCity};
  else if(goal!=s.Goal&&goal!=s.OrderTarget){s.Goal=goal;s.Returning=t.m_isEnterCity;s.Ordered=false;s.Failures=0;}
 }
 static void BeforeCampaignDay(Wo_DeployTeamCtrl __instance){try{if(Ready())RememberCampaign(__instance);}catch(Exception e){Error(e);}}
 static void AfterCampaignCamp(Wo_DeployTeamCtrl __instance){
  try{
   var t=__instance;if(!Ready())return;RepairCampMotion(t);if(!CampaignControlled(t,out _)||OtherCampaignOrder(t))return;
   if(Gate("campaign-camp:"+t.m_id,5)){
    var pos=t.GetPosition();double gap=-1;
    if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_targetCity??"",out var city))gap=Vector3.Distance(pos,city.BMBMKPGEHBH);
    Note($"campaign-camp team={t.m_id} faction={t.m_belongPower} source={t.m_assembleCity} target={t.m_targetCity} nativeControl={t.CanControl()} state={t.m_deployState} war={t.m_warInfo!=null} targetWar={t.m_targetWarInfo!=null} path={t.m_listMovePos?.Count} waypoints={t.GAANFLKOMKL?.Count} position={pos} goalDistance={gap:F2}");
   }
   AfterCampaignDay(t);
  }catch(Exception e){Error(e);}
 }
 // Native StartCamp sets the label/state only; UpdateMove consumes a surviving
 // path even in Camp. Stop stale travel for delegated world armies, including
 // loaded saves, before their existing cover/interception/campaign owner acts.
 static void RepairCampMotion(Wo_DeployTeamCtrl t){
  if(!t||ManualProtected(t)||t.m_deployState!=AKOPKFPKJPP.Camp||t.m_isStarting||t.m_isPrivate||t.IncludePlayer()||t.m_warInfo!=null||t.m_targetWarInfo!=null)return;
  if((t.m_listMovePos?.Count??0)+(t.GAANFLKOMKL?.Count??0)==0)return;
  if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_assembleCity??"",out var origin)||!DistributionCity(origin,out var f)||f.IBGCLCAIBOL!=t.m_belongPower)return;
  if(t.m_responseToPlayer&&(!t.m_responseTeam||!War(f,t.m_responseTeam.m_belongPower)))return;
  RememberCampaign(t);
  int points=t.m_listMovePos?.Count??0;t.m_listMovePos?.Clear();t.GAANFLKOMKL?.Clear();
  Note($"camp-motion-repaired team={t.m_id} source={t.m_assembleCity} stalePoints={points} state=Camp action=stop-stale-route-and-reassess");
 }
 static bool GuardCampaignEntry(Wo_DeployTeamCtrl __instance,AFAGFHJONFJ __0){
  var t=__instance;
  if(!GuardCoverEntry(t,__0))return false;
  if(!GuardDefenseEntry(t,__0))return false;
  if(!t||!campaignGoals.TryGetValue(t.Pointer,out var s)||s.Id!=t.m_id||!s.Returning||!s.Ordered||s.OrderTarget!=__0?.OJPECMNINNA||!t.m_isEnterCity)return true;
  try {
   if(!Ready()||!CampaignControlled(t,out var f))return true;
   if(__0.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&CanAdmitReturn(__0,t)&&!__0.GKLIEHCHDLD())return true;
   // Admission uses actual occupants, not every future reservation. Otherwise
   // two arrivals can each reserve the last space and reject one another forever.
   s.Ordered=false;s.Staging=false;s.CheckedDay=-1;
   t.ClearTarget(false);t.m_tmpTargetCity="";t.m_listMovePos?.Clear();t.GAANFLKOMKL?.Clear();t.StartCamp();
   work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,s).GetEnumerator());
   Note($"campaign-entry-delayed team={t.m_id} city={__0.OJPECMNINNA} reason=ownership-battle-or-physical-capacity-changed action=release-reservation-and-replan");return false;
  }catch(Exception e){Error(e);return true;}
 }
 static bool CanAdmitReturn(AFAGFHJONFJ city,Wo_DeployTeamCtrl t){
  var units=TeamTroops(t);
  if(city.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in city.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null)units.Add(a.Pointer);
  return units.Count<=city.JNLIOIHLDHH();
 }
 static void AfterCampaignDay(Wo_DeployTeamCtrl __instance){
  try{
   if(!Ready())return;ObserveManualTeam(__instance);if(ManualProtected(__instance))return;AdoptCompletedCityOrder(__instance);if(!CampaignControlled(__instance,out _))return;
   var t=__instance;RememberCampaign(t);ScheduleReturnWatch(t);
   if(!campaignGoals.TryGetValue(t.Pointer,out var s))return;
   if(s.Ordered&&(s.OrderTarget==t.m_targetCity||(s.Staging&&string.IsNullOrEmpty(t.m_targetCity)))){
    float moved=Vector3.Distance(s.Position,t.GetPosition());
    if(moved>.1f&&!s.MovingLogged){s.MovingLogged=true;Note($"campaign-moving team={t.m_id} target={s.OrderTarget} distance={moved:F2} state={t.m_deployState}");}
    if(CampIdle(t)&&!s.MovingLogged&&NEINKOOBJIH.MIPFHCKDJOK>s.OrderedDay){s.Failures++;s.RetryAfter[s.OrderTarget]=NEINKOOBJIH.MIPFHCKDJOK+3;s.Ordered=false;Note($"campaign-no-progress team={t.m_id} target={s.OrderTarget} retryAfterDays=3");}
   }
   double day=Math.Floor((double)NEINKOOBJIH.MIPFHCKDJOK);
   int period=DecisionDays(t.m_belongPower);
   if(!CampIdle(t)||(s.CheckedPeriod==period&&s.CheckedDay>=0&&day>=s.CheckedDay&&day-s.CheckedDay<period))return;
   // Mark a decision day only after the queue actually accepts the work.
   if(work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,s).GetEnumerator())){s.CheckedDay=day;s.CheckedPeriod=period;}
  }catch(Exception e){Error(e);}
 }
 static bool SameCamp(Wo_DeployTeamCtrl t,CampaignGoal s)=>CampIdle(t)&&t.m_id==s.Id&&t.m_belongPower==s.Faction&&CampaignControlled(t,out _)&&campaignGoals.TryGetValue(t.Pointer,out var actual)&&ReferenceEquals(actual,s)&&
  (string.IsNullOrEmpty(t.m_targetCity)||t.m_targetCity==s.Goal||t.m_targetCity==s.OrderTarget);
 // Compute from the army's CURRENT position, never its assembly city. One
 // path per scheduler slice, with bounded ownership samples.
 static bool CampaignRoute(Wo_DeployTeamCtrl t,AFAGFHJONFJ target,MKMOJNAJKJE f,out int days)=>CampaignRouteDistance(t,target,f,out days,out _);

 static string CampaignAttackBlock(Wo_DeployTeamCtrl t,CampaignGoal s,AFAGFHJONFJ target,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,int days){
  if(NativeOffenseDisabled())return "native-no-war-option";
  if(s.Returning)return "return-already-selected";
  if(s.Failures>=2)return "repeated-no-movement";
  if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_assembleCity??"",out var home)||!AttackPermission(home,f,home.LMPMNLLGOEC.IPJHOOJBCKI,target.LMPMNLLGOEC.LMMNKIOAGLC??""))return "governor-policy-or-origin-changed";
  if(!AttackableOwner(f,target.LMPMNLLGOEC.LMMNKIOAGLC))return "target-no-longer-hostile";
  if(target.GKLIEHCHDLD())return "target-already-in-battle";
  if(t.GetMorale()<40)return "low-morale";
  if(t.GetWoundNum()>t.GetSoldierNum(false)/2)return "heavy-wounds";
  if(t.GetCombatVal()<AttackDefense(target,teams))return "insufficient-power";
  if(t.GetFood()<(long)Math.Max(1,t.GetOneDayFood())*(days*2+15))return "insufficient-roundtrip-food";
  return null;
 }
 static bool ReturnSafe(Wo_DeployTeamCtrl t,AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,int days,bool enter){
  if(c.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||c.GKLIEHCHDLD()||IncomingHostile(c,f))return false;
  if(t.GetFood()<(long)Math.Max(1,t.GetOneDayFood())*(days+2))return false;
  return !enter||CapacityRoom(c,teams,t)>=TeamTroops(t).Count;
 }
 static bool CampaignMove(Wo_DeployTeamCtrl t,CampaignGoal s,AFAGFHJONFJ target,bool enter,string reason,int days){
  if(!SameCamp(t,s)||!ICGAEMLACJG.FKKCPDEFODO.TryGetValue(target.OJPECMNINNA,out var ctrl)||!ctrl)return false;
  s.Position=t.GetPosition();s.OrderedDay=NEINKOOBJIH.MIPFHCKDJOK;s.OrderTarget=target.OJPECMNINNA;s.MovingLogged=false;s.Staging=s.Returning&&!enter;
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(s.Faction,out var owner)||!MoveRelief(t,target,null,enter,owner,true)){
   s.Ordered=false;s.Failures++;s.RetryAfter[target.OJPECMNINNA]=NEINKOOBJIH.MIPFHCKDJOK+3;
   Note($"campaign-order-rejected team={t.m_id} target={target.OJPECMNINNA} reason=route-or-native-movement");return false;
  }
  // Native MoveToCity deliberately clears targetCity for friendly non-entry
  // travel, while retaining its world-space path. Do not mistake that for failure.
  s.Ordered=(t.m_targetCity==target.OJPECMNINNA||(s.Staging&&string.IsNullOrEmpty(t.m_targetCity)))&&t.m_deployState==AKOPKFPKJPP.Expedition&&t.m_listMovePos?.Count>0;
  if(!s.Ordered){s.Failures++;s.RetryAfter[target.OJPECMNINNA]=NEINKOOBJIH.MIPFHCKDJOK+3;}
  double endpointGap=t.m_listMovePos?.Count>0?Vector3.Distance(t.m_listMovePos[t.m_listMovePos.Count-1],target.BMBMKPGEHBH):-1;
  Note($"campaign-route team={t.m_id} target={target.OJPECMNINNA} position={s.Position} cityPosition={target.BMBMKPGEHBH} pathEndGap={endpointGap:F2} waypoints={t.GAANFLKOMKL?.Count} nativeControl={t.CanControl()}");
  Note($"campaign-decision team={t.m_id} faction={s.Faction} goal={s.Goal} target={target.OJPECMNINNA} action={(enter?"return-enter":s.Returning?"return-staging":"advance")} reason={reason} power={t.GetCombatVal()} soldiers={t.GetSoldierNum(false)} morale={t.GetMorale()} food={t.GetFood()} travelDays={days} state={t.m_deployState} path={t.m_listMovePos?.Count} accepted={s.Ordered}");
  if(s.Ordered)AIGate("return-watch:"+t.m_id,t.m_belongPower,2);
  return s.Ordered;
 }
 static bool CampaignAdvance(Wo_DeployTeamCtrl t,CampaignGoal s,AFAGFHJONFJ target,MKMOJNAJKJE f,string reason,int days){
  long required=Math.Max(1000,(long)Math.Ceiling(AttackDefense(target,AttackTeams())*1.3));
  if(t.GetCombatVal()>required*2&&t.m_listHero?.Count>1){
   var child=SplitFieldArmy(t,required);
   if(child){
    var order=new CampaignGoal{Id=child.m_id,Faction=f.IBGCLCAIBOL,Goal=target.OJPECMNINNA,CompletedOrder=s.CompletedOrder};campaignGoals[child.Pointer]=order;
    if(CampaignAttackBlock(child,order,target,f,AttackTeams(),days)==null&&CampaignMove(child,order,target,false,reason+"-limited-detachment",days)){
     s.Returning=true;s.Ordered=false;work.Add("campaign-rest:"+t.m_id,()=>RecoverCampaign(t,s).GetEnumerator());
     Note($"aftermath-limited-advance original={t.m_id} attacking={child.m_id} attackMen={child.GetSoldierNum(false)} remainder={t.GetSoldierNum(false)} target={target.OJPECMNINNA} remainderRole=redistribute-or-return");return true;
    }
    campaignGoals.Remove(child.Pointer);child.MergeToTeam(t);
   }
  }
  return CampaignMove(t,s,target,false,reason,days);
 }
 static IEnumerable<string> RecoverCampaign(Wo_DeployTeamCtrl t,CampaignGoal s){
  // StartCamp synchronously invokes our postfix. A caller may then replace
  // its provisional orphan goal with an explicit return goal while the same
  // queue key is already pending. Resolve that handoff at execution time.
  // Never transfer a queued task to a recycled object or a new manual order.
  if(t&&t.m_id==s.Id&&t.m_belongPower==s.Faction&&CampaignControlled(t,out _)&&campaignGoals.TryGetValue(t.Pointer,out var latest)&&latest.Id==s.Id&&latest.Faction==s.Faction&&!ReferenceEquals(latest,s)){
   if(Gate("campaign-handoff:"+t.m_id,15))Note($"campaign-goal-handoff team={t.m_id} previous={s.Goal} current={latest.Goal} returning={latest.Returning} reason=new-intent-before-queued-work");
   s=latest;
  }
  int id=t?.m_id??-1;long started=Environment.TickCount64;double day=NEINKOOBJIH.MIPFHCKDJOK;
  bool report=Gate("campaign-recovery:"+id,15);
  if(report)Note($"campaign-recovery-start team={id} state={t?.m_deployState} goal={s.Goal} order={s.OrderTarget} returning={s.Returning} ordered={s.Ordered} day={day:F3}");
  try{foreach(var phase in RecoverCampaignCore(t,s))yield return phase;}
  finally{if(report&&t&&t.m_id==id)Note($"campaign-recovery-end team={id} elapsedMs={Environment.TickCount64-started} elapsedDays={NEINKOOBJIH.MIPFHCKDJOK-day:F3} state={t.m_deployState} target={t.m_targetCity} entry={t.m_isEnterCity} path={t.m_listMovePos?.Count} follow={t.m_responseToPlayer} war={t.m_warInfo!=null} targetWar={t.m_targetWarInfo!=null} stillIdle={CampIdle(t)} ordered={s.Ordered}");}
 }
 static IEnumerable<string> RecoverCampaignCore(Wo_DeployTeamCtrl t,CampaignGoal s){
  if(!SameCamp(t,s)||!CampaignControlled(t,out var f)){
   if(t&&Gate("campaign-skip:"+t.m_id,15))Note($"campaign-recovery-skipped team={t.m_id} idle={CampIdle(t)} controlled={CampaignControlled(t,out _)} sameIdentity={t.m_id==s.Id&&t.m_belongPower==s.Faction} currentGoal={campaignGoals.TryGetValue(t.Pointer,out var current)&&ReferenceEquals(current,s)} manual={ManualProtected(t)} state={t.m_deployState} target={t.m_targetCity} intended={s.Goal} path={t.m_listMovePos?.Count}");
   yield break;
  }
  foreach(var phase in AftermathHomeSteps(t,s,f))yield return phase;
  if(!SameCamp(t,s))yield break;
  if(!s.Returning)foreach(var phase in CampBattleSteps(t,s,f))yield return phase;
  if(!SameCamp(t,s))yield break;
  string reason="goal-missing";var teams=AttackTeams();
  if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(s.Goal??"",out var target)&&target.LMPMNLLGOEC!=null){
   if(CampaignRoute(t,target,f,out int days)){
    reason=CampaignAttackBlock(t,s,target,f,teams,days);
    if(reason==null){CampaignAdvance(t,s,target,f,"goal-still-feasible",days);yield break;}
   }else reason="goal-route-blocked";
  }
  yield return "campaign-goal";
  if(!SameCamp(t,s))yield break;
  // Reassess a completed/missing objective before falling back to return.
  // Search only the present region's frontier, with one route per slice.
  var anchor=AOHIBKLOAEI.PNKKPMCJIGA(t.GetPosition());
  if(anchor==null)MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(s.Goal??"",out anchor);
  if(anchor==null)MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_assembleCity??"",out anchor);
  var advances=new List<(AFAGFHJONFJ City,int Days,double Score)>();
  if(!s.Returning&&anchor?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!IncomingHostile(anchor,f)&&anchor.FKMKGNPFLFP!=null){
   foreach(var next in anchor.FKMKGNPFLFP.ToArray()){
    if(!SameCamp(t,s))yield break;
    teams=AttackTeams();string blocked="not-hostile";
    if(next?.LMPMNLLGOEC!=null&&AttackableOwner(f,next.LMPMNLLGOEC.LMMNKIOAGLC)){
     blocked="route-blocked";
     if(CampaignRoute(t,next,f,out int days)){
      blocked=CampaignAttackBlock(t,s,next,f,teams,days);
      if(blocked==null&&days>attackMaxDays.Value)blocked="too-far";
      if(blocked==null&&teams.Any(a=>a.Pointer!=t.Pointer&&a.m_belongPower==s.Faction&&a.m_targetCity==next.OJPECMNINNA))blocked="already-targeted";
      if(blocked==null)advances.Add((next,days,days+10.0*AttackDefense(next,teams)/Math.Max(1,t.GetCombatVal())));
     }
     Note($"campaign-next-candidate team={t.m_id} target={next.OJPECMNINNA} result={blocked??"feasible"}");
    }
    yield return "campaign-next-target";
   }
  }
  foreach(var next in advances.OrderBy(x=>x.Score)){
   if(!SameCamp(t,s))yield break;
   teams=AttackTeams();
   if(IncomingHostile(anchor,f)||anchor.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||!CampaignRoute(t,next.City,f,out int days)||CampaignAttackBlock(t,s,next.City,f,teams,days)!=null)continue;
   if(teams.Any(a=>a.Pointer!=t.Pointer&&a.m_belongPower==s.Faction&&a.m_targetCity==next.City.OJPECMNINNA))continue;
   if(CampaignAdvance(t,s,next.City,f,"completed-goal-next-frontier",days)){if(!s.Returning)s.Goal=next.City.OJPECMNINNA;yield break;}
   yield return "campaign-next-retry";
  }
  s.Returning=true;
  foreach(var phase in ReturnPartitionSteps(t,s,f))yield return phase;
  if(!SameCamp(t,s))yield break;
  var choices=new List<(AFAGFHJONFJ City,int Days,bool Enter,double Score)>();
  // Snapshot before yielding: ownership/capacity/routes are rechecked at commit.
  var cities=new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AFAGFHJONFJ>(MICGPAMNDKM.IJPKEMPIFIH.Count);MICGPAMNDKM.IJPKEMPIFIH.Values.CopyTo(cities,0);
  foreach(var c in cities){
   if(!SameCamp(t,s))yield break;
   if(c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&(!s.RetryAfter.TryGetValue(c.OJPECMNINNA,out var until)||until<=NEINKOOBJIH.MIPFHCKDJOK)&&CampaignRouteDistance(t,c,f,out int days,out double road)){
    teams=AttackTeams();bool enter=CapacityRoom(c,teams,t)>=TeamTroops(t).Count;
    if(ReturnSafe(t,c,f,teams,days,enter))choices.Add((c,days,enter,road+(enter?0:1000000000d)));
   }
   yield return "campaign-return-target";
  }
  foreach(var choice in choices.OrderBy(x=>x.Score)){
   if(!SameCamp(t,s))yield break;
   teams=AttackTeams();
   if(!CampaignRoute(t,choice.City,f,out int days)||!ReturnSafe(t,choice.City,f,teams,days,choice.Enter))continue;
   // Full cities are staging points only: never dismiss into an overflowing
   // garrison. Retry capacity each day instead of abandoning the army.
   if(!choice.Enter&&Vector3.Distance(t.GetPosition(),choice.City.BMBMKPGEHBH)<5){if(Gate("campaign-full:"+t.m_id,10))Note($"campaign-wait team={t.m_id} reason=no-compatible-officer-group-capacity city={choice.City.OJPECMNINNA} retry=next-day no-futile-staging-tour=true");yield break;}
   if(CampaignMove(t,s,choice.City,choice.Enter,reason,days))yield break;
   yield return "campaign-return-retry";
  }
  Note($"campaign-blocked team={t.m_id} goal={s.Goal} reason={reason};no-safe-reachable-return soldiers={t.GetSoldierNum(false)} food={t.GetFood()} retry=next-day");
 }
}
