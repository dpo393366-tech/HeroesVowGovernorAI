using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using DeployConfirmation=UI_DeployUI.GMPKDAEEJMK;

public sealed partial class StrategicSupport {
 const string ManualPrefix="local.heroesvow.governor/manual/v1/";
 [ThreadStatic] static int manualInputDepth,manualGovernorDepth,manualDeployDepth;
 static bool ManualAIGeneration=>pumping||WorkQueue.InStep||manualGovernorDepth>0||governorStaffing>0||researchOwner!=null;
 static readonly Dictionary<string,CLAGKDECFID> manualHeroes=new();
 static readonly Dictionary<int,Wo_DeployTeamCtrl> manualTeams=new();
 static readonly Queue<string> manualHeroPoll=new();
 static readonly Queue<int> manualTeamPoll=new();
 static long manualPollAt;static bool manualRestorePending;
 static void TrackManual(CLAGKDECFID h){if(!manualHeroes.ContainsKey(h.OJPECMNINNA))manualHeroPoll.Enqueue(h.OJPECMNINNA);manualHeroes[h.OJPECMNINNA]=h;}
 static void TrackManual(Wo_DeployTeamCtrl t){if(!manualTeams.ContainsKey(t.m_id))manualTeamPoll.Enqueue(t.m_id);manualTeams[t.m_id]=t;}
 static void ResetManualOrders(){manualHeroes.Clear();manualTeams.Clear();manualHeroPoll.Clear();manualTeamPoll.Clear();manualPollAt=0;manualRestorePending=true;}

 static bool ManualInput=>manualInputDepth>0&&!ManualAIGeneration;
 static string ManualFaction=>NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC;
 static string ManualKey(string faction,string kind,string id)=>ManualPrefix+faction+"/"+kind+"/"+id;
 static int ManualNow=>Math.Max(1,(int)Math.Floor((double)NEINKOOBJIH.MIPFHCKDJOK*1000)+1);
 static bool ManualPending(int deadline,int now)=>deadline<0||deadline>now;
 static void BeginManualInput(out int __state){__state=manualInputDepth;manualInputDepth++;}
 static void EndManualInput(int __state){manualInputDepth=__state;}
 sealed class ManualDeployScope {public int Input,Deploy;}
 static void BeginManualDeploy(out ManualDeployScope __state){
  __state=new(){Input=manualInputDepth,Deploy=manualDeployDepth};manualInputDepth++;manualDeployDepth++;
 }
 static void EndManualDeploy(ManualDeployScope __state){manualInputDepth=__state.Input;manualDeployDepth=__state.Deploy;}
 static void AfterManualAssembly(Wo_DeployTeamCtrl __instance){
  // The native confirmation coroutine writes the army fields directly, then
  // RefreshLeader commits its roster. StartDeploy runs in a later coroutine,
  // after the input scope has ended. Capture ownership at this commit point.
  // Ordinary HUD/AI leader refreshes must never acquire player authority.
  if(manualDeployDepth<=0||!ManualInput||!__instance||(__instance.m_listHero?.Count??0)==0||(__instance.m_dicDeployArmy?.Count??0)==0)return;
  AfterManualTeamCommand(__instance);
 }
 static void BeginManualGovernor(){manualGovernorDepth++;}
 static void EndManualGovernor(){manualGovernorDepth=Math.Max(0,manualGovernorDepth-1);}
 void HookManualOrders(Harmony h){
  HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
  h.Patch(AccessTools.Method(typeof(FNFJODHIJFO),"GHNEGBJANJG"),prefix:H(nameof(BeginManualGovernor)),finalizer:H(nameof(EndManualGovernor)));
  h.Patch(AccessTools.Method(typeof(FNFJODHIJFO),"OBIOEOJAHMM"),prefix:H(nameof(BeginManualGovernor)),finalizer:H(nameof(EndManualGovernor)));
  h.Patch(AccessTools.Method(typeof(FNFJODHIJFO),"OHBNFHKFNPH"),prefix:H(nameof(BeginManualGovernor)),finalizer:H(nameof(EndManualGovernor)));
  h.Patch(AccessTools.Method(typeof(EventSystem),"Update"),prefix:H(nameof(BeginManualInput)),finalizer:H(nameof(EndManualInput)));
  h.Patch(AccessTools.Method(typeof(DeployConfirmation),"MoveNext"),prefix:H(nameof(BeginManualDeploy)),finalizer:H(nameof(EndManualDeploy)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"RefreshLeader"),postfix:H(nameof(AfterManualAssembly)));
  h.Patch(AccessTools.Method(typeof(Ci_CityCtrl),"MouseLeftPress"),prefix:H(nameof(BeginManualInput)),finalizer:H(nameof(EndManualInput)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"MouseLeftPress"),prefix:H(nameof(BeginManualInput)),finalizer:H(nameof(EndManualInput)));
  h.Patch(AccessTools.Method(typeof(Wo_PassCtrl),"MouseLeftPress"),prefix:H(nameof(BeginManualInput)),finalizer:H(nameof(EndManualInput)));
  h.Patch(AccessTools.Method(typeof(Wo_VillageCtrl),"MouseLeftPress"),prefix:H(nameof(BeginManualInput)),finalizer:H(nameof(EndManualInput)));
  h.Patch(AccessTools.Method(typeof(Wo_WorldPlaceCtrl),"MouseLeftPress"),prefix:H(nameof(BeginManualInput)),finalizer:H(nameof(EndManualInput)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"MoveToCity"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"MoveToDes"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"MoveToWarInfo"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"MoveToVillage"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"MoveToPass"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"MoveToCamp"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"MoveToMerge"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"SetAttackTarget"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"StartAssemble"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"StartDeploy"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"StartTeam"),postfix:H(nameof(AfterManualTeamCommand)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"StartCamp"),postfix:H(nameof(AfterManualTeamCamp)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"FinishMove"),postfix:H(nameof(ObserveManualTeam)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"EnterCity"),prefix:H(nameof(BeforeManualTeamRelease)),postfix:H(nameof(AfterManualTeamRelease)));
  h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"Dismiss"),prefix:H(nameof(BeforeManualTeamRelease)),postfix:H(nameof(AfterManualTeamRelease)));
  h.Patch(AccessTools.Method(typeof(CLAGKDECFID),"JKABOFDPCCD"),postfix:H(nameof(AfterManualHeroJob)));
  h.Patch(AccessTools.Method(typeof(CLAGKDECFID),"IFFJPNEJCIN"),postfix:H(nameof(AfterManualHeroJob)));
  h.Patch(AccessTools.Method(typeof(CLAGKDECFID),"KFNJDLCBLDN"),postfix:H(nameof(AfterManualHeroDuty)));
 }
 static bool ManualProtected(Wo_DeployTeamCtrl t){
  if(enabled?.Value!=true||!t||MLGOLGGHLMB.CBEGDDKANAO==null||t.m_belongPower!=ManualFaction)return false;
  string k=ManualKey(t.m_belongPower,"team",t.m_id.ToString());int until=QueueRead(k);
  if(until==0&&t.m_listHero!=null)foreach(var hero in t.m_listHero){
   if(hero==null)continue;string hk=ManualKey(t.m_belongPower,"hero",hero.OJPECMNINNA);
   // Survivors may be recreated or merged by the native battle lifecycle.
   // Carry authority with the actual participants, not only a pooled pointer.
   int inherited=QueueRead(hk);if(QueueRead(hk+"/army")!=0&&ManualPending(inherited,ManualNow)){until=inherited;QueueWrite(k,until);break;}
  }
  if(until!=0)TrackManual(t);
  return ManualPending(until,ManualNow);
 }
 static bool ManualProtected(CLAGKDECFID h){
  if(enabled?.Value!=true||h==null||MLGOLGGHLMB.CBEGDDKANAO==null||h.LMPMNLLGOEC?.LMMNKIOAGLC!=ManualFaction)return false;
  string key=ManualKey(ManualFaction,"hero",h.OJPECMNINNA);int until=QueueRead(key);
  if(until==0)return false;TrackManual(h);
  if(until<0&&QueueRead(key+"/army")==0&&!ManualHeroBusy(h,key)){CompleteManual(key,"officer="+h.OJPECMNINNA);until=QueueRead(key);}
  return ManualPending(until,ManualNow);
 }
 static bool ManualProtected(DHKINGPDKJJ a)=>enabled?.Value==true&&a!=null&&MLGOLGGHLMB.CBEGDDKANAO!=null&&a.LMMNKIOAGLC==ManualFaction&&ManualPending(QueueRead(ManualKey(a.LMMNKIOAGLC,"troop",a.OJPECMNINNA.ToString())),ManualNow);
 static bool ManualHeroBusy(CLAGKDECFID h,string key)=>h.OGPEJIEHDAN!=null&&h.OGPEJIEHDAN.HFBAJIGHOAF!=GCPCLLMKIMM.None||QueueRead(key+"/duty")==1&&h.BAPIJIJLLCA!=null&&h.BAPIJIJLLCA.HFBAJIGHOAF!=OEBGIDGEIIG.None;
 static void CompleteManual(string key,string description){if(QueueRead(key)>=0)return;int until=Math.Max(1,(int)Math.Ceiling((double)NEINKOOBJIH.MIPFHCKDJOK*1000)+1)+5000;QueueWrite(key,until);Note($"manual-order-complete {description} day={NEINKOOBJIH.MIPFHCKDJOK:F3} graceDays=5 releaseDay={(until-1)/1000d:F3}");}
 static void AfterManualHeroJob(CLAGKDECFID __instance){
  var h=__instance;if(enabled?.Value!=true||h?.LMPMNLLGOEC?.LMMNKIOAGLC!=ManualFaction||MLGOLGGHLMB.CBEGDDKANAO==null)return;
  string key=ManualKey(ManualFaction,"hero",h.OJPECMNINNA);
  if(ManualInput){QueueWrite(key,-1);QueueWrite(key+"/army",0);QueueWrite(key+"/duty",0);TrackManual(h);Note($"manual-order officer={h.OJPECMNINNA} job={h.OGPEJIEHDAN?.HFBAJIGHOAF} day={NEINKOOBJIH.MIPFHCKDJOK:F3}");}
  if(QueueRead(key)<0&&QueueRead(key+"/army")==0&&!ManualHeroBusy(h,key))CompleteManual(key,"officer="+h.OJPECMNINNA);
 }
 static void AfterManualHeroDuty(CLAGKDECFID __instance){
  if(!ManualInput){AfterManualHeroJob(__instance);return;}
  AfterManualHeroJob(__instance);if(__instance?.LMPMNLLGOEC?.LMMNKIOAGLC!=ManualFaction||MLGOLGGHLMB.CBEGDDKANAO==null)return;
  string key=ManualKey(ManualFaction,"hero",__instance.OJPECMNINNA);QueueWrite(key+"/duty",1);QueueWrite(key,-1);
  if(!ManualHeroBusy(__instance,key))CompleteManual(key,"officer="+__instance.OJPECMNINNA);
 }
 static void AfterManualTeamCommand(Wo_DeployTeamCtrl __instance){
  var t=__instance;if(enabled?.Value!=true||!ManualInput||!t||t.m_belongPower!=ManualFaction||MLGOLGGHLMB.CBEGDDKANAO==null)return;
  QueueWrite(ManualKey(t.m_belongPower,"team",t.m_id.ToString()),-1);TrackManual(t);
  if(t.m_listHero!=null)foreach(var hero in t.m_listHero){if(hero==null)continue;string k=ManualKey(t.m_belongPower,"hero",hero.OJPECMNINNA);QueueWrite(k,-1);QueueWrite(k+"/army",1);TrackManual(hero);}
  if(t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var a in group.Value)if(a!=null)QueueWrite(ManualKey(t.m_belongPower,"troop",a.OJPECMNINNA.ToString()),-1);
  // A player replaces any outstanding governor intent, including queued jobs.
  campaignGoals.Remove(t.Pointer);defenseOrders.Remove(t.Pointer);coverOrders.Remove(t.Pointer);capacityOrders.Remove(t.Pointer);huntOrders.Remove(t.Pointer);attackOrders.Remove(t.Pointer);fieldReliefOrders.Remove(t.Pointer);reliefLoans.Remove(t.Pointer);
  Note($"manual-order team={t.m_id} target={t.m_targetCity} state={t.m_deployState} day={NEINKOOBJIH.MIPFHCKDJOK:F3}");
 }
 static void AfterManualTeamCamp(Wo_DeployTeamCtrl __instance){AfterManualTeamCommand(__instance);if(ManualInput&&__instance&&ManualProtected(__instance))FinishManualTeam(__instance);else ObserveManualTeam(__instance);}
 static void ObserveManualTeam(Wo_DeployTeamCtrl __instance){
  var t=__instance;if(!t||!ManualProtected(t)||QueueRead(ManualKey(t.m_belongPower,"team",t.m_id.ToString()))>=0)return;
  if(t.m_isStarting||t.m_deployState==AKOPKFPKJPP.Assemble||t.m_warInfo!=null||t.m_targetWarInfo!=null||t.m_isEnterCity||t.m_responseToPlayer||OtherCampaignOrder(t)||(t.m_listMovePos?.Count??0)+(t.GAANFLKOMKL?.Count??0)>0)return;
  string goal=!string.IsNullOrEmpty(t.m_targetCity)?t.m_targetCity:t.m_tmpTargetCity;
  if(!string.IsNullOrEmpty(goal)){
   if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(goal,out var city)||city.LMPMNLLGOEC?.LMMNKIOAGLC!=t.m_belongPower||city.GKLIEHCHDLD()||!AtCompletedCity(t,city))return;
  }
  if(t.m_deployState==AKOPKFPKJPP.Camp||t.m_deployState==AKOPKFPKJPP.Expedition)FinishManualTeam(t);
 }
 sealed class ManualRelease {public Wo_DeployTeamCtrl Team;public string Faction;public int Id;public List<CLAGKDECFID> Heroes=new();public List<DHKINGPDKJJ> Troops=new();}
 static void BeforeManualTeamRelease(Wo_DeployTeamCtrl __instance,out ManualRelease __state){
  __state=null;var t=__instance;if(!t||!ManualProtected(t))return;
  __state=new(){Team=t,Id=t.m_id,Faction=t.m_belongPower};if(t.m_listHero!=null)foreach(var h in t.m_listHero)if(h!=null)__state.Heroes.Add(h);
  if(t.m_dicDeployArmy!=null)foreach(var p in t.m_dicDeployArmy)if(p.Value!=null)foreach(var a in p.Value)if(a!=null)__state.Troops.Add(a);
 }
 static void AfterManualTeamRelease(ManualRelease __state){if(__state==null)return;var t=__state.Team;if(t&&t.m_id==__state.Id&&t.m_belongPower==__state.Faction&&(t.m_listHero?.Count??0)>0)return;
   // Explicitly requested: a dissolved army and all released participants are
   // immediately available to AI. No post-disband grace period.
   ReleaseManualNow(__state);
 }
 static void FinishManualTeam(Wo_DeployTeamCtrl t){BeforeManualTeamRelease(t,out var state);if(state!=null)FinishManualRelease(state);}
 static void FinishManualRelease(ManualRelease s){
  string key=ManualKey(s.Faction,"team",s.Id.ToString());CompleteManual(key,"team="+s.Id);int until=QueueRead(key);
  foreach(var h in s.Heroes){string hk=ManualKey(s.Faction,"hero",h.OJPECMNINNA);QueueWrite(hk,until);QueueWrite(hk+"/army",0);}
  foreach(var a in s.Troops)QueueWrite(ManualKey(s.Faction,"troop",a.OJPECMNINNA.ToString()),until);
 }
 static void ReleaseManualNow(ManualRelease s){
  QueueWrite(ManualKey(s.Faction,"team",s.Id.ToString()),0);
  foreach(var h in s.Heroes){string hk=ManualKey(s.Faction,"hero",h.OJPECMNINNA);QueueWrite(hk,0);QueueWrite(hk+"/army",0);}
  foreach(var a in s.Troops)QueueWrite(ManualKey(s.Faction,"troop",a.OJPECMNINNA.ToString()),0);
  Note($"manual-order-released team={s.Id} reason=disband immediate=true");
 }
 static IEnumerable<string> RestoreManualOrders(){
  if(MLGOLGGHLMB.CBEGDDKANAO==null||string.IsNullOrEmpty(ManualFaction))yield break;
  // Native saved flags retain deadlines independently of runtime object pointers.
  foreach(var t in AttackTeams()){ManualProtected(t);yield return "manual-team-restore";}
  int n=0;foreach(var h in OfficerRegistrySnapshot()){if(++n%8==0)yield return "manual-officer-restore";ManualProtected(h);}
 }
 static void PollManualOrders(){
  if(manualRestorePending){manualRestorePending=false;work.Add("manual-restore",()=>RestoreManualOrders().GetEnumerator());}
  if(Environment.TickCount64<manualPollAt)return;manualPollAt=Environment.TickCount64+250;
  int count=Math.Min(8,manualTeamPoll.Count);
  for(int i=0;i<count;i++){int id=manualTeamPoll.Dequeue();if(!manualTeams.TryGetValue(id,out var t))continue;
   if(t&&t.m_id==id){ObserveManualTeam(t);if(ManualProtected(t)){manualTeamPoll.Enqueue(id);continue;}}
   manualTeams.Remove(id);
  }
  count=Math.Min(8,manualHeroPoll.Count);
  for(int i=0;i<count;i++){string id=manualHeroPoll.Dequeue();if(!manualHeroes.TryGetValue(id,out var h))continue;
   if(ManualProtected(h)){manualHeroPoll.Enqueue(id);continue;}manualHeroes.Remove(id);
  }
 }
}
