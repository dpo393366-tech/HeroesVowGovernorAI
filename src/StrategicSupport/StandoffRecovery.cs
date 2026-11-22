using System;using System.Linq;using System.Collections.Generic;using UnityEngine;
public sealed partial class StrategicSupport {
 sealed class Standoff {public int Id,Target;public string Faction,TargetFaction;public IntPtr TargetPointer;public Vector3 Position,TargetPosition;public double Day;public int Attempts;}
 static readonly Dictionary<IntPtr,Standoff> standoffs=new();
 sealed class FailedIntercept {public int Enemy;public string Faction;public Vector3 Position;public double Day;public int Power;}
 static readonly Dictionary<string,FailedIntercept> failedIntercepts=new();
 // Native interception may protect an ally or cut reinforcements to an allied
 // battle. Only checking the opponent's current region cancels those orders.
 static bool InterceptProtectedFaction(string own,string other){
  if(string.IsNullOrEmpty(other))return false;
  return own==other||(NEINKOOBJIH.COCMABJFOFH.TryGetValue(own??"",out var f)&&f!=null&&f.AENJACOFBJK!=null&&f.AENJACOFBJK.TryGetValue(other,out var relation)&&relation?.EBIOPDHEBOF==BHBNKONCOJD.Ally);
 }
 static bool InterceptBattlePurpose(string own,Wo_DeployTeamCtrl enemy,NJHDEHLHENN battle){
  if(!CorridorWarAlive(battle))return false;
  int side=battle.EMMJBPMNFAK[0]==enemy.m_belongPower?0:battle.EMMJBPMNFAK[1]==enemy.m_belongPower?1:-1;
  return side>=0&&InterceptProtectedFaction(own,battle.EMMJBPMNFAK[1-side]);
 }
 static bool InterceptRelevant(Wo_DeployTeamCtrl t,Wo_DeployTeamCtrl enemy,out string reason){
  reason=null;
  if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_assembleCity??"",out var home)||home.LMPMNLLGOEC?.LMMNKIOAGLC!=t.m_belongPower)return true;
  bool Own(AFAGFHJONFJ c)=>c?.LMPMNLLGOEC?.LMMNKIOAGLC==t.m_belongPower;
  if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(enemy.m_targetCity??"",out var attacked)&&!enemy.m_isEnterCity&&InterceptProtectedFaction(t.m_belongPower,attacked?.LMPMNLLGOEC?.LMMNKIOAGLC))return true;
  if(enemy.m_attackTarget&&InterceptProtectedFaction(t.m_belongPower,enemy.m_attackTarget.m_belongPower))return true;
  if(InterceptBattlePurpose(t.m_belongPower,enemy,enemy.m_targetWarInfo)||InterceptBattlePurpose(t.m_belongPower,enemy,enemy.m_warInfo))return true;
  var region=AOHIBKLOAEI.PNKKPMCJIGA(enemy.GetPosition());
  // Unknown geometry is not enough evidence to cancel a native order.
  if(region==null||Own(region)||region.Pointer==home.Pointer)return true;
  if(home.FKMKGNPFLFP!=null)foreach(var c in home.FKMKGNPFLFP)if(c!=null&&c.Pointer==region.Pointer)return true;
  reason="opponent-outside-source-frontier-without-friendly-target";return false;
 }
 static bool GuardFailedIntercept(Wo_DeployTeamCtrl t){
  if(!Ready()||!t||t.m_deployState!=AKOPKFPKJPP.Assemble||t.m_isPrivate||t.IncludePlayer()||!t.m_responseToPlayer||!t.m_responseTeam)return true;
  string key=t.m_belongPower+"/"+t.m_assembleCity;var enemy=t.m_responseTeam;
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(t.m_belongPower??"",out var owner)||!War(owner,enemy.m_belongPower))return true;
  if(ManualProtected(t))return true;
  if(!InterceptRelevant(t,enemy,out string irrelevant)){
   int rejectedId=t.m_id;DismissSupport(t);
   if(Gate("intercept-reject:"+key+":"+enemy.m_id,10))Note($"intercept-preflight-rejected team={rejectedId} source={key} opponent={enemy.m_id} reason={irrelevant}");return false;
  }
  ObserveIntercept(t);
  if(!failedIntercepts.TryGetValue(key,out var old))return true;
  double day=NEINKOOBJIH.MIPFHCKDJOK;
  if(old.Enemy!=enemy.m_id||old.Faction!=enemy.m_belongPower||day<old.Day||day-old.Day>=7||enemy.m_warInfo!=null||UnityEngine.Vector3.Distance(old.Position,enemy.GetPosition())>.5f||t.GetCombatVal()>old.Power*1.2){failedIntercepts.Remove(key);return true;}
  // Retry when the enemy moves, strength improves, or the finite retry window
  // expires. Repeated assemblies must not march into the identical stalemate.
  int id=t.m_id;DismissSupport(t);Note($"standoff-repeat-suppressed team={id} source={key} opponent={old.Enemy} reason=unchanged-failed-interception retryAfterDays={7-(day-old.Day):F1}");return false;
 }
 static void AuditCampState(Wo_DeployTeamCtrl t){
  if(Ready())RepairCampMotion(t);
  if(!t||t.m_deployState!=AKOPKFPKJPP.Camp||!Gate("camp-audit:"+t.m_id,30))return;
  bool controlled=CampaignControlled(t,out _);
  Note($"camp-audit team={t.m_id} faction={t.m_belongPower} heroes={string.Join(",",t.m_listHero==null?Array.Empty<string>():t.m_listHero.ToArray().Select(h=>h?.OJPECMNINNA))} day={NEINKOOBJIH.MIPFHCKDJOK:F3} controlled={controlled} idle={CampIdle(t)} private={t.m_isPrivate} player={t.IncludePlayer()} follow={t.m_responseToPlayer} response={t.m_responseTeam?.m_id} responseAI={t.m_responseTeamAI?.m_id} attack={t.m_attackTarget?.m_id} merge={t.m_mergeTarget?.m_id} village={t.m_targetVillage!=null} pass={t.m_targetPass!=null} target={t.m_targetCity} camp={t.m_targetCamp} war={t.m_warInfo!=null} targetWar={t.m_targetWarInfo!=null} path={t.m_listMovePos?.Count} cover={coverOrders.ContainsKey(t.Pointer)} defense={defenseOrders.ContainsKey(t.Pointer)} hunt={huntOrders.ContainsKey(t.Pointer)} position={t.GetPosition()}");
 }
 static void ObserveIntercept(Wo_DeployTeamCtrl t){
  if(!t||!t.m_responseToPlayer||!t.m_responseTeam||t.m_isPrivate||t.IncludePlayer()||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(t.m_belongPower??"",out var f)||!War(f,t.m_responseTeam.m_belongPower))return;
  var target=t.m_responseTeam;
  if(standoffs.TryGetValue(t.Pointer,out var old)&&old.Id==t.m_id&&old.Faction==t.m_belongPower&&old.TargetPointer==target.Pointer)return;
  standoffs[t.Pointer]=new(){Id=t.m_id,Faction=t.m_belongPower,Target=target.m_id,TargetFaction=target.m_belongPower,TargetPointer=target.Pointer,Position=t.GetPosition(),TargetPosition=target.GetPosition(),Day=NEINKOOBJIH.MIPFHCKDJOK};
 }
 static void AfterInterceptDeploy(Wo_DeployTeamCtrl t){
  if(!Ready()||!t||!t.gameObject.activeSelf||!t.m_responseToPlayer||!t.m_responseTeam)return;
  ObserveIntercept(t);
  if(InterceptIdle(t))work.Add("intercept:"+t.m_id,()=>InterceptStartSteps(t,t.m_id).GetEnumerator());
 }
 static IEnumerable<string> InterceptStartSteps(Wo_DeployTeamCtrl t,int id){
  yield return "validate-native-camp";
  if(Ready()&&t&&t.m_id==id)RecoverHostileStandoff(t);
 }
 static bool InterceptIdle(Wo_DeployTeamCtrl t)=>t&&!t.m_isStarting&&t.m_warInfo==null&&t.m_targetWarInfo==null&&!t.m_mergeTarget&&t.m_targetVillage==null&&t.m_targetPass==null&&string.IsNullOrEmpty(t.m_targetCamp)&&string.IsNullOrEmpty(t.m_targetCity)&&
  (t.m_deployState==AKOPKFPKJPP.Camp||t.m_deployState==AKOPKFPKJPP.Expedition)&&(t.m_listMovePos==null||t.m_listMovePos.Count==0)&&(t.GAANFLKOMKL==null||t.GAANFLKOMKL.Count==0)&&(!t.m_attackTarget||t.m_attackTarget==t.m_responseTeam);
 static void ReleaseIntercept(Wo_DeployTeamCtrl t,Standoff s,string why,bool returning){
  t.m_responseToPlayer=false;t.m_responseTeam=null;t.m_responseTeamAI=null;t.m_attackTarget=null;
  t.m_listMovePos?.Clear();t.GAANFLKOMKL?.Clear();
  if(t.m_deployState==AKOPKFPKJPP.Expedition)t.StartCamp();
  standoffs.Remove(t.Pointer);
  var goal=new CampaignGoal{Id=t.m_id,Faction=t.m_belongPower,Goal=t.m_assembleCity,Returning=returning};campaignGoals[t.Pointer]=goal;
  Note($"standoff-release team={t.m_id} opponent={s.Target} idleDays={NEINKOOBJIH.MIPFHCKDJOK-s.Day:F2} action={(returning?"safe-return":"reassess-support-or-return")} reason={why}");
  work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());
 }
 static void RecoverHostileStandoff(Wo_DeployTeamCtrl t){
  if(!t||ManualProtected(t)||t.m_isPrivate||t.IncludePlayer()||t.m_warInfo!=null)return;
  RepairOrphanResponse(t);
  if(t.m_targetWarInfo!=null||t.m_isEnterCity||!string.IsNullOrEmpty(t.m_targetCity)||!string.IsNullOrEmpty(t.m_targetCamp)||t.m_mergeTarget||t.m_targetVillage!=null||t.m_targetPass!=null||missions.ContainsKey(t.Pointer)||defenseOrders.ContainsKey(t.Pointer)||coverOrders.ContainsKey(t.Pointer)||fieldReliefOrders.ContainsKey(t.Pointer))return;
  if(!DecisionDue("standoff:"+t.m_id,t.m_belongPower,t.Pointer,"",true))return;
  ObserveIntercept(t);
  if(!standoffs.TryGetValue(t.Pointer,out var s)||s.Id!=t.m_id||s.Faction!=t.m_belongPower)return;
  var target=t.m_responseTeam;
  if(!t.m_responseToPlayer){standoffs.Remove(t.Pointer);return;}
  if(!target||target.m_id!=s.Target||target.m_belongPower!=s.TargetFaction||!ICGAEMLACJG.HFJIECIBFKB.TryGetValue(s.Target,out var registered)||!registered||registered.Pointer!=s.TargetPointer||!target.gameObject.activeSelf){
   // A pooled Unity object can survive dismissal and now represent an entirely
   // different army. Never follow it based on pointer/non-null checks alone.
   if(t.m_targetWarInfo==null&&(!t.m_attackTarget||t.m_attackTarget.Pointer==s.TargetPointer)){ReleaseIntercept(t,s,"opponent-removed-or-pooled-identity-changed",true);}return;
  }
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(t.m_belongPower??"",out var owner)||!War(owner,target.m_belongPower)){ReleaseIntercept(t,s,"opponent-no-longer-hostile",true);return;}
  if(!InterceptRelevant(t,target,out string irrelevant)){ReleaseIntercept(t,s,irrelevant,true);return;}
  double day=NEINKOOBJIH.MIPFHCKDJOK;
  if(day<s.Day||Vector3.Distance(s.Position,t.GetPosition())>.5f){s.Day=day;s.Position=t.GetPosition();s.Attempts=0;}
  if(!InterceptIdle(t))return;
  if(target.m_warInfo!=null){ReleaseIntercept(t,s,"opponent-entered-battle",false);return;}
  // Issuing the same order is not movement. Preserve the last actual progress
  // clock across retries so an accepted-but-stalled native attack can expire.
  if(s.Attempts>=2&&day-s.Day>=2*DecisionDays(t.m_belongPower)){
   failedIntercepts[t.m_belongPower+"/"+t.m_assembleCity]=new(){Enemy=target.m_id,Faction=target.m_belongPower,Position=target.GetPosition(),Day=day,Power=t.GetCombatVal()};
   ReleaseIntercept(t,s,"repeated-order-without-movement",false);return;
  }
  // The native faction refresh only restarts Expedition interceptors. A Camp
  // interceptor never reaches that branch, even when its route is now legal.
  // Reissue a real attack order after checking the current path and policy.
  if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_assembleCity??"",out var origin)&&Eligible(origin,out var faction)
   &&AttackPermission(origin,faction,origin.LMPMNLLGOEC.IPJHOOJBCKI,target.m_belongPower)
   &&t.GetCombatVal()>=target.GetCombatVal()&&t.GetMorale()>=40&&t.GetFood()>Math.Max(1,t.GetOneDayFood())*10L){
   string region=AOHIBKLOAEI.PNKKPMCJIGA(target.GetPosition())?.OJPECMNINNA;
   if(ReliefPath(t.GetPosition(),target.GetPosition(),faction,region,true,out var route,out double length)&&route.Count>0){
    t.SetAttackTarget(target,true);
    if(t.m_deployState==AKOPKFPKJPP.Expedition&&t.m_listMovePos!=null){
     t.GAANFLKOMKL?.Clear();t.m_listMovePos.Clear();foreach(var point in route)t.m_listMovePos.Add(point);
     s.TargetPosition=target.GetPosition();s.Attempts++;Note($"standoff-resumed team={t.m_id} opponent={target.m_id} opponentState={target.m_deployState} routePoints={route.Count} length={length:F1} nativeAttack=true opponentTargetWar={target.m_targetWarInfo?.HIENOINGBCN} alliedBattlePurpose={InterceptBattlePurpose(t.m_belongPower,target,target.m_targetWarInfo)}");return;
    }
   }
  }
  // Only hostile interception is released. Friendly follow/player/private and
  // combat orders never enter this path. A stalemate must not consume all food.
  if(day-s.Day<3){Note($"standoff-wait team={t.m_id} opponent={target.m_id} power={t.GetCombatVal()}/{target.GetCombatVal()} morale={t.GetMorale()} food={t.GetFood()} retryDay={s.Day+3:F1}");return;}
  failedIntercepts[t.m_belongPower+"/"+t.m_assembleCity]=new(){Enemy=target.m_id,Faction=target.m_belongPower,Position=target.GetPosition(),Day=day,Power=t.GetCombatVal()};
  ReleaseIntercept(t,s,"interception-route-strength-food-or-policy-unusable",true);
 }
 // Saved or cancelled automatic interceptors can retain only the response flag.
 // Clearing that stale flag restores eligibility for the ordinary recovery
 // owner; preserve a valid explicit battle/city/army order if one exists.
 static void RepairOrphanResponse(Wo_DeployTeamCtrl t){
  if(!t||!t.m_responseToPlayer||ManualProtected(t)||t.m_isPrivate||t.IncludePlayer()||t.m_warInfo!=null)return;
  var target=t.m_responseTeam;
  if(target&&ICGAEMLACJG.HFJIECIBFKB.TryGetValue(target.m_id,out var registered)&&registered&&registered.Pointer==target.Pointer&&target.gameObject.activeSelf)return;
  t.m_responseToPlayer=false;t.m_responseTeam=null;t.m_responseTeamAI=null;
  if(target&&t.m_attackTarget&&t.m_attackTarget.Pointer==target.Pointer)t.m_attackTarget=null;
  standoffs.Remove(t.Pointer);
  Note($"intercept-orphan-repaired team={t.m_id} target={target?.m_id} city={t.m_targetCity} targetWar={t.m_targetWarInfo!=null} reason=missing-registered-opponent");
  if(CampIdle(t))AfterCampaignDay(t);
 }
}
