using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

public sealed class AIWorkDriver:MonoBehaviour
{
 public AIWorkDriver(IntPtr p):base(p){}
 public void Update(){StrategicSupport.PumpEconomy();}
 public void OnApplicationQuit(){for(int i=0;i<8;i++)AsyncJournal.Flush(true);}
}
public sealed partial class StrategicSupport
{
 static readonly WorkQueue work=new();static bool schedulerOnline,pumping;
 static ConfigEntry<float> workBudget;
 void ConfigurePerformance(){
  workBudget=Config.Bind("Performance","BudgetMilliseconds",2f,new ConfigDescription("Soft main-thread AI work budget per frame. Native calls cannot be interrupted; unfinished work continues next frame.",new AcceptableValueRange<float>(0.5f,8f)));
  AsyncJournal.Start(Path.Combine(Paths.BepInExRootPath,"StrategicSupport-investment.log"));
 }
 void StartWorkScheduler(){
  schedulerOnline=AddComponent<AIWorkDriver>()!=null;
  Note("ai-scheduler online="+schedulerOnline+" budgetMs="+workBudget.Value+" performanceRecording=false actionLogging=background-batched");
 }
 static readonly Dictionary<string,(double Day,IntPtr City,string Owner,string Governor)> cityAdmissions=new();
 sealed class ConstructionVisit {public IntPtr City;public string Owner,Governor;public double Day;public int Remaining;}
 static readonly Dictionary<string,ConstructionVisit> constructionVisits=new();
 static void OpenConstructionVisit(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor){
  double day=Math.Floor((double)NEINKOOBJIH.MIPFHCKDJOK);
  if(constructionVisits.TryGetValue(c.OJPECMNINNA,out var old)&&old.City==c.Pointer&&old.Owner==f.IBGCLCAIBOL&&old.Governor==governor){
   if(day>=old.Day&&day-old.Day<DecisionDays(f.IBGCLCAIBOL))return;
   old.Day=day;old.Remaining=buildsPerCity.Value;return;
  }
  constructionVisits[c.OJPECMNINNA]=new(){City=c.Pointer,Owner=f.IBGCLCAIBOL,Governor=governor,Day=day,Remaining=buildsPerCity.Value};
 }
 static void ResetWork(){work.Clear();ResetFixedRoads();constructionVisits.Clear();coverPlanning.Clear();decisionStamps.Clear();foreach(var key in aiGatePeriods.Keys)next.Remove(key);aiGatePeriods.Clear();cityAdmissions.Clear();}
 static bool CurrentCity(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor)=>c!=null&&f!=null&&c.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL
  &&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(c.OJPECMNINNA,out var registered)&&registered?.Pointer==c.Pointer
  &&c.LMPMNLLGOEC.IPJHOOJBCKI==governor&&Eligible(c,out var current)&&current.Pointer==f.Pointer&&!c.GKLIEHCHDLD();
 static bool CurrentFaction(MKMOJNAJKJE f)=>f!=null&&NEINKOOBJIH.COCMABJFOFH.TryGetValue(f.IBGCLCAIBOL,out var current)&&current?.Pointer==f.Pointer;
 static void ScheduleCity(AFAGFHJONFJ c){
  try{
   if(c?.LMPMNLLGOEC==null||!Ready())return;
   var stamp=(Math.Floor((double)NEINKOOBJIH.MIPFHCKDJOK),c.Pointer,c.LMPMNLLGOEC.LMMNKIOAGLC,c.LMPMNLLGOEC.IPJHOOJBCKI);
   string id=c.OJPECMNINNA;
   ObserveOccupation(c);
   if(!DecisionDue("city:"+id,stamp.Item3,stamp.Item2,stamp.Item4,false))return;
   if(work.Add("city:"+id,()=>CityWork(c))){cityAdmissions[id]=stamp;DecisionDue("city:"+id,stamp.Item3,stamp.Item2,stamp.Item4,true);}
  }catch(Exception e){Error(e);}
 }
 static void ScheduleFaction(MKMOJNAJKJE f){if(f!=null)ScheduleStaffing(f);if(f!=null&&DecisionDue("faction:"+f.IBGCLCAIBOL,f.IBGCLCAIBOL,f.Pointer,"",false)&&work.Add("faction:"+f.IBGCLCAIBOL,()=>FactionWork(f)))DecisionDue("faction:"+f.IBGCLCAIBOL,f.IBGCLCAIBOL,f.Pointer,"",true);}
 static IEnumerator<string> CityWork(AFAGFHJONFJ c){
  if(!Ready()||c?.LMPMNLLGOEC==null||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(c.LMPMNLLGOEC.LMMNKIOAGLC,out var f)||f==null)yield break;
  ScheduleFaction(f);string governor=c.LMPMNLLGOEC.IPJHOOJBCKI;
  work.Add("capacity-emergency:"+c.OJPECMNINNA,()=>SafePhases(CapacityTransferSteps(c,f)).GetEnumerator());
  if(!CurrentCity(c,f,governor)){InvestmentAudit(c,"excluded:"+Eligibility(c,out _));yield break;}
  work.Add("city-offense:"+c.OJPECMNINNA,()=>SafePhases(OffensiveSteps(c,f,governor)).GetEnumerator());
  if(investment.Value&&AIGate("invest:"+c.OJPECMNINNA,f.IBGCLCAIBOL,2)){
   OpenConstructionVisit(c,f,governor);
   work.Add("income:"+c.OJPECMNINNA,()=>SafePhases(AuditCityIncomeSteps(c,f)).GetEnumerator());
   work.Add("economy:research:"+c.OJPECMNINNA,()=>CityResearchWork(c,f,governor));
   work.Add("economy:maintenance:"+c.OJPECMNINNA,()=>CityMaintenanceWork(c,f,governor));
   ScheduleConstruction(c,f,governor);
   // Goal-driven demolition has its own slot; a long vacant-plot search or
   // an existing cached construction plan cannot suppress redevelopment.
   work.Add("economy:renewal:"+c.OJPECMNINNA,()=>CityRenewalWork(c,f,governor));
   AuditDevelopmentAgenda(c,f);
   var row=c.LMPMNLLGOEC;
   if(budget.Value&&row.KJPPGIOCDID!=null&&row.KJPPGIOCDID.Count<c.JNLIOIHLDHH()&&row.PNHICLAGHGD>0){
    Fund(c,true);if(row.DNOGPFHMOMO>=50001&&AIGate("staff-invest:"+c.OJPECMNINNA,f.IBGCLCAIBOL,10))c.OBIOEOJAHMM();
   }
  }
  ScheduleStaffing(f);yield return "city-admitted";
  if(CurrentCity(c,f,governor)&&domestic.Value&&AIGate("domestic:"+c.OJPECMNINNA,f.IBGCLCAIBOL,30)&&DayReady("domestic:"+c.OJPECMNINNA,DecisionDays(f.IBGCLCAIBOL))){
   DayReady("domestic:"+c.OJPECMNINNA,0,true);AssignDomestic(c,f);
  }yield return "domestic";
 }
 static IEnumerator<string> CityResearchWork(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor){
  if(!Ready()||!investment.Value||!CurrentCity(c,f,governor))yield break;
   for(int pass=0;pass<4;pass++){
    if(!CurrentCity(c,f,governor))yield break;
    if(pass>0)next.Remove("research-invest:"+f.IBGCLCAIBOL+":"+governor);
    int beforeResearch=f.DNOGPFHMOMO;
    try{TryResearch(c,f);}catch(Exception e){Error(e);}finally{researchOwner=null;researchGovernor=null;researchTeam=null;researchHomes=null;}
    yield return "research";
    if(f.DNOGPFHMOMO>=beforeResearch)break;
   }
   if(!CurrentCity(c,f,governor))yield break;
 }
 static IEnumerator<string> CityMaintenanceWork(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor){
  if(!Ready()||!investment.Value||!CurrentCity(c,f,governor))yield break;
   if(maintenance.Value){using(var steps=ConstructionSlices(MaintenanceSteps(c,f)).GetEnumerator())while(true){
    bool more;try{more=steps.MoveNext();}catch(Exception e){Error(e);break;}if(!more)break;
    yield return steps.Current?"maintenance-commit":"maintenance-scan";if(!CurrentCity(c,f,governor))yield break;
   }}yield return "maintenance";
 }
 static void ScheduleConstruction(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor)=>work.Add("economy:construction:"+c.OJPECMNINNA,()=>CityConstructionWork(c,f,governor));
 static void ScheduleLayout(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor)=>work.Add("layout:"+c.OJPECMNINNA,()=>CityLayoutWork(c,f,governor));
 static IEnumerator<string> CityConstructionWork(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor){
  if(!Ready()||!investment.Value||!construction.Value||!CurrentCity(c,f,governor))yield break;
  if(!constructionVisits.TryGetValue(c.OJPECMNINNA,out var visit)||visit.City!=c.Pointer||visit.Owner!=f.IBGCLCAIBOL||visit.Governor!=governor)yield break;
  int built=0;
  for(int n=0;n<buildsPerCity.Value&&visit.Remaining>0;n++){
   if(!investment.Value||!construction.Value||!CurrentCity(c,f,governor))yield break;
   int result=ExecuteSavedLayout(c,f,out bool refresh);
   if(refresh)ScheduleLayout(c,f,governor);
   if(result!=1){
    if(Gate("construction-wait:"+c.OJPECMNINNA,30))Note($"construction-wait city={c.OJPECMNINNA} state={(result==2?"plan-waiting-conditions":"awaiting-plan")} refresh={refresh} remainingAllowance={visit.Remaining} planned={(layoutIntents.TryGetValue(c.OJPECMNINNA,out var pending)?pending.Keys.Count:0)} treasury={f.DNOGPFHMOMO} cityCash={c.LMPMNLLGOEC.DNOGPFHMOMO}");
    break;
   }
   visit.Remaining--;built++;yield return "build-commit";
  }
  if(built>0)InvestmentAudit(c,$"evaluated built={built} treasury={f.DNOGPFHMOMO} cityCash={c.LMPMNLLGOEC.DNOGPFHMOMO} cachedPlan=true scheduled=true");
 }
 static IEnumerator<string> CityLayoutWork(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor){
  if(!Ready()||!investment.Value||!construction.Value||!CurrentCity(c,f,governor))yield break;
  if(Gate("layout-start:"+c.OJPECMNINNA,30))Note($"layout-job-start city={c.OJPECMNINNA} scheduling=reserved-economy-share");
  using(var scan=ConstructionSlices(BuildScanSteps(c,f,true)).GetEnumerator())while(scan.MoveNext()){
   yield return "layout-scan";if(!investment.Value||!construction.Value||!CurrentCity(c,f,governor))yield break;
  }
  if(layoutIntents.TryGetValue(c.OJPECMNINNA,out var plan)&&plan.Keys.Count>0)ScheduleConstruction(c,f,governor);
 }
 static IEnumerator<string> CityRenewalWork(AFAGFHJONFJ c,MKMOJNAJKJE f,string governor){
  if(!Ready()||!investment.Value||!CurrentCity(c,f,governor))yield break;
  using(var scan=ConstructionSlices(RenewalSteps(c,f)).GetEnumerator())while(scan.MoveNext()){
   yield return scan.Current?"renewal-commit":"renewal-scan";if(!investment.Value||!CurrentCity(c,f,governor))yield break;
  }
 }
 // Geometry searches yield for individual cells and the optimizer yields for
 // tiny candidate batches. Sending every such yield behind all world cities
 // stretches a small CPU job across game months. Coalesce read-only scan steps
 // within a small CPU slice, still inside the unchanged global frame budget.
 // A mutation always returns immediately so the caller rechecks ownership.
 static IEnumerable<bool> ConstructionSlices(IEnumerable<bool> source,double sliceMs=.5,int maxSteps=256){
  using var scan=source.GetEnumerator();var timer=Stopwatch.StartNew();int count=0;
  while(scan.MoveNext()){
   bool committed=scan.Current;
   if(committed||++count>=maxSteps||timer.Elapsed.TotalMilliseconds>=sliceMs){
    yield return committed;count=0;timer.Restart();
   }
  }
 }
 static IEnumerable<string> SafePhases(IEnumerable<string> phases){using(var it=phases.GetEnumerator())while(true){bool more;try{more=it.MoveNext();}catch(Exception e){Error(e);yield break;}if(!more)yield break;yield return it.Current;}}
 static System.Reflection.MethodInfo awardsApi;
 static IEnumerator<string> FactionWork(MKMOJNAJKJE f){
  if(!Ready()||!CurrentFaction(f))yield break;
  foreach(var phase in SafePhases(FactionStatecraftSteps(f)))yield return phase;
  if(!CurrentFaction(f)||!AIGate("administration:"+f.IBGCLCAIBOL,f.IBGCLCAIBOL,30)||!DayReady("administration:"+f.IBGCLCAIBOL,DecisionDays(f.IBGCLCAIBOL)))yield break;
  DayReady("administration:"+f.IBGCLCAIBOL,0,true);
  // Officer movement has its own daily queue, independent of strategies and awards.
  if(!CurrentFaction(f))yield break;
  if(titles.Value)foreach(var phase in SafePhases(TitleSteps(f)))yield return phase;yield return "titles";
  if(CurrentFaction(f)&&awards.Value&&f.IBGCLCAIBOL==NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC){
   if(awardsApi==null){var type=AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType("TreasuryAwards")).FirstOrDefault(t=>t!=null);awardsApi=type?.GetMethod("RunAutomaticSteps");}
   // Do not silently run the legacy monolithic API if the companion DLL is
   // older. The manual award button remains available in that case.
   if(awardsApi==null){if(Gate("treasury-api-missing",120))Note("treasury-auto skipped: incremental API requires TreasuryAwards 1.4.2");}
   else if(awardsApi.Invoke(null,new object[]{f}) is IEnumerable<string> steps){
    foreach(var phase in SafePhases(steps)){
     if(phase.StartsWith("result:"))Note("treasury-auto "+phase.Substring(7));
     yield return phase.StartsWith("result:")?"treasury-result":phase;
     if(!CurrentFaction(f))yield break;
    }
   }
  }yield return "treasury";
 }
 static bool CurrentBattleJob(string key){
  var w=Wa_Controller.m_warInfo;if(w==null)return false;
  // Only battle transport planning and its route/cover transport dependencies.
  // Pausing campaign days does not unlock construction, recruitment or tactical unit commands.
  return key.StartsWith("battle-support-plan:"+w.Pointer+":")||key.StartsWith("battle-route:"+w.Pointer+":")
   ||(key.StartsWith("supply-route:")&&key.Contains(":"+w.Pointer+":"+w.HIENOINGBCN+":"))
   ||key.StartsWith("threat-route:")||key.StartsWith("defense-cover:");
 }
 // Sparse managed-scheduler diagnostics only: no hooks on native game Update,
 // no allocation census, and no per-step text/file writes. Existing async log.
 static void SlowWorkSlice(string phase,double elapsed){
  if(Gate("work-slice-overrun",5))Note($"work-slice-overrun stage={phase} cpuMs={elapsed:F2} queued={work.Count} frameBudgetMs={workBudget.Value:F2} nativeCallMayBeIndivisible=true");
 }
 internal static void PumpEconomy(){
  if(!schedulerOnline||pumping)return;
  try{
   pumping=true;
   SyncWorldVisuals();
   bool campaignRunning=!AEHAGJAKNEK.PAKENCLGOHK;
   bool directRunning=Wa_Controller.m_warInfo!=null&&!Wa_Controller.m_isBeforeWar&&!Wa_Controller.m_isQuiting&&!Wa_Controller.m_isByEvent&&Wa_Controller.m_mockInfo==null&&Wa_Controller.m_battleSpeed>0&&(Wa_Controller.m_isRoundMode||campaignRunning);
   if(Ready()&&Time.timeScale>0&&(campaignRunning||directRunning)){
    if(campaignRunning){PollManualOrders();PollDefense();}
    // Cheap yielded probes previously hit the eight-step cap far below the
    // time budget, stretching a city scan into minutes on a populated map.
    // Retain the frame budget and priority lanes without profiling game Update.
    work.Pump(workBudget.Value,128,null,Error,campaignRunning?null:CurrentBattleJob,SlowWorkSlice);
   }
  }catch(Exception e){Error(e);}finally{pumping=false;}
 }
}
