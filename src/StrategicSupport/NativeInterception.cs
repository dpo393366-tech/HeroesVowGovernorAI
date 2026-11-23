using System;using System.Collections.Generic;
public sealed partial class StrategicSupport {
 sealed class InterceptionSnapshot {
  public Wo_DeployTeamCtrl Team;public int Id;public string Faction;public bool DismissAttempt;
 }
 sealed class InterceptionScope {
  public InterceptionScope Parent;public MKMOJNAJKJE Faction;
  public Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl> OriginalField,FilteredField;
  public readonly Dictionary<IntPtr,InterceptionSnapshot> Existing=new();
 }
 [ThreadStatic]static InterceptionScope interceptionScope;
 static bool InterceptorHasSoldiers(Wo_DeployTeamCtrl t){
  if(t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var unit in group.Value)if(unit!=null&&unit.FGHFOLKBHDL>0)return true;
  return false;
 }
 static bool AutomaticFieldInterceptor(Wo_DeployTeamCtrl t)=>t&&t.gameObject.activeSelf&&t.m_responseToPlayer&&!t.m_isPrivate&&!t.IncludePlayer()&&!ManualProtected(t)
  &&t.m_warInfo==null&&!t.m_isEnterCity&&t.m_targetWarInfo==null&&string.IsNullOrEmpty(t.m_targetCity)&&string.IsNullOrEmpty(t.m_targetCamp)
  &&!t.m_mergeTarget&&t.m_targetVillage==null&&t.m_targetPass==null
  &&(t.m_deployState==AKOPKFPKJPP.Expedition||t.m_deployState==AKOPKFPKJPP.Camp)
  &&t.m_listHero!=null&&t.m_listHero.Count>0&&InterceptorHasSoldiers(t)
  &&!missions.ContainsKey(t.Pointer)&&!defenseOrders.ContainsKey(t.Pointer)&&!coverOrders.ContainsKey(t.Pointer)&&!fieldReliefOrders.ContainsKey(t.Pointer);
 static void BeforeNativeInterception(MKMOJNAJKJE __instance,ref Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl> __0,out InterceptionScope __state){
  __state=null;if(!Ready()||__instance==null||__0==null)return;
  var state=new InterceptionScope{Parent=interceptionScope,Faction=__instance};__state=state;interceptionScope=state;
  // Filter before native creates an army: canceling after StartDeploy is too
  // late, since the native response target is assigned after departure.
  var allowed=new Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl>();int rejected=0;
  foreach(var enemy in __0)if(InterceptThreatPurpose(__instance.IBGCLCAIBOL,enemy,out _))allowed.Add(enemy);else rejected++;
  __0=allowed;
  if(__instance.HMLLPOCACMJ!=null)foreach(var t in __instance.HMLLPOCACMJ)
   if(AutomaticFieldInterceptor(t)&&t.m_belongPower==__instance.IBGCLCAIBOL)state.Existing[t.Pointer]=new(){Team=t,Id=t.m_id,Faction=t.m_belongPower};
  // A rendezvous is a committed movement order, not a free camp available
  // for the native opportunistic interception selector.
  if(__instance.HMLLPOCACMJ!=null){
   foreach(var t in __instance.HMLLPOCACMJ)if(JointHuntOwned(t)){
    state.OriginalField=__instance.HMLLPOCACMJ;state.FilteredField=new();
    foreach(var member in state.OriginalField)if(!JointHuntOwned(member))state.FilteredField.Add(member);
    __instance.HMLLPOCACMJ=state.FilteredField;break;
   }
  }
  if(rejected>0&&Gate("intercept-filter:"+__instance.IBGCLCAIBOL,30))Note($"intercept-candidates-filtered faction={__instance.IBGCLCAIBOL} allowed={allowed.Count} rejected={rejected} policy=actual-own-or-allied-target noRouteSearch=true");
 }
 static bool SameInterception(InterceptionSnapshot s)=>s.Team&&s.Team.m_id==s.Id&&s.Team.m_belongPower==s.Faction&&AutomaticFieldInterceptor(s.Team);
 static bool PreserveDeployedInterception(Wo_DeployTeamCtrl t){
  var scope=interceptionScope;
  if(scope==null||scope.Faction.OEIKEALIMEP==null||scope.Faction.OEIKEALIMEP.Count!=0||!t||!scope.Existing.TryGetValue(t.Pointer,out var s)||!SameInterception(s))return false;
  // Only the synchronous native interception routine can enter this guard.
  // Its no-candidates cleanup calls Dismiss on existing field interceptors
  // when the opponent is not in battle. Assembly rejection, city admission,
  // death, user dismissal and all unrelated Dismiss callers remain native.
  s.DismissAttempt=true;
  bool purpose=InterceptThreatPurpose(t.m_belongPower,t.m_responseTeam,out string reason);
  if(Gate("intercept-native-cleanup:"+t.m_id,30))Note($"intercept-native-dismiss-prevented team={t.m_id} source={t.m_assembleCity} opponent={t.m_responseTeam?.m_id} scope=MKMOJNAJKJE.IPDNICOBPDF nativeCandidates={scope.Faction.OEIKEALIMEP?.Count} opponentInBattle={t.m_responseTeam?.m_warInfo!=null} state={t.m_deployState} purpose={reason} action={(purpose?"preserve-and-reassess":"physical-return")} path={t.m_listMovePos?.Count}");
  return true;
 }
 static void AfterNativeInterception(InterceptionScope __state){
  if(__state==null)return;
  // Restore before any recovery callbacks. A recovery failure or nested native
  // call must never leak a global dismissal exemption into unrelated gameplay.
  if(ReferenceEquals(interceptionScope,__state))interceptionScope=__state.Parent;
  if(__state.FilteredField!=null&&__state.Faction.HMLLPOCACMJ?.Pointer==__state.FilteredField.Pointer)__state.Faction.HMLLPOCACMJ=__state.OriginalField;
  if(!Ready())return;
  foreach(var s in __state.Existing.Values)try{
   if(!SameInterception(s))continue;var t=s.Team;
   bool registered=t.m_responseTeam&&ICGAEMLACJG.HFJIECIBFKB.TryGetValue(t.m_responseTeam.m_id,out var enemy)&&enemy&&enemy.Pointer==t.m_responseTeam.Pointer;
   bool valid=registered&&InterceptThreatPurpose(t.m_belongPower,t.m_responseTeam,out _);
   if(!valid){
    var old=new Standoff{Id=t.m_id,Faction=t.m_belongPower,Target=t.m_responseTeam?.m_id??0,Day=NEINKOOBJIH.MIPFHCKDJOK};
    ReleaseIntercept(t,old,"no-current-own-or-allied-attack-purpose",true);
   }else if(s.DismissAttempt){
    // Queue existing recovery; never execute pathfinding inside native cleanup.
    work.Add("intercept:"+t.m_id,()=>InterceptStartSteps(t,s.Id).GetEnumerator());
   }
  }catch(Exception e){Error(e);}
 }
}
