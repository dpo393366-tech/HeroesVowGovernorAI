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
 static double windowMaxFrame,windowMaxWork,windowWorkTotal,windowNativeCity,windowNativePlan;
 static int windowFrames,windowSteps,windowPlanCalls;static long reportAt,lastPump;
 static string slowestWork="none";
 static long windowStarted;static double windowPumpTotal,windowPumpMax,previousPumpMs,gapPrecedingPumpMs;
 static int windowLongGaps,windowOverBudget;
 void ConfigurePerformance(){
  workBudget=Config.Bind("Performance","BudgetMilliseconds",2f,new ConfigDescription("Soft main-thread AI work budget per frame. Native calls cannot be interrupted; unfinished work continues next frame.",new AcceptableValueRange<float>(0.5f,8f)));
  AsyncJournal.Start(Path.Combine(Paths.BepInExRootPath,"StrategicSupport-investment.log"));
 }
 void StartPerformance(Harmony h){
  schedulerOnline=AddComponent<AIWorkDriver>()!=null;
  HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
  h.Patch(AccessTools.Method(typeof(AFAGFHJONFJ),"JFOOAMEIMDO"),prefix:H(nameof(BeginNativeCityTiming)),finalizer:H(nameof(EndNativeCityTiming)));
  h.Patch(AccessTools.Method(typeof(FNFJODHIJFO),"GHNEGBJANJG"),prefix:H(nameof(BeginNativePlanTiming)),finalizer:H(nameof(EndNativePlanTiming)));
  Note("ai-scheduler online="+schedulerOnline+" budgetMs="+workBudget.Value+" logging=background-batched");
 }
 static void BeginNativeCityTiming(out long __state){__state=Stopwatch.GetTimestamp();}
 static void EndNativeCityTiming(long __state){windowNativeCity=Math.Max(windowNativeCity,(Stopwatch.GetTimestamp()-__state)*1000d/Stopwatch.Frequency);}
 static void BeginNativePlanTiming(out long __state){windowPlanCalls++;__state=Stopwatch.GetTimestamp();}
 static void EndNativePlanTiming(FNFJODHIJFO __instance,long __state){if(__instance!=null&&Gate("native-plan-audit:"+__instance.Pointer,60))Note($"native-plan faction={__instance.LMMNKIOAGLC} policy={__instance.BNGFJOAJKDH} ms={(Stopwatch.GetTimestamp()-__state)*1000d/Stopwatch.Frequency:F2}");windowNativePlan=Math.Max(windowNativePlan,(Stopwatch.GetTimestamp()-__state)*1000d/Stopwatch.Frequency);}
 static readonly Dictionary<string,(double Day,IntPtr City,string Owner,string Governor)> cityAdmissions=new();
 static void ResetWork(){work.Clear();coverPlanning.Clear();decisionStamps.Clear();foreach(var key in aiGatePeriods.Keys)next.Remove(key);aiGatePeriods.Clear();cityAdmissions.Clear();}
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
  // National strategy is independent of city delegation, even for manual cities.
  ScheduleFaction(f);string governor=c.LMPMNLLGOEC.IPJHOOJBCKI;
  foreach(var phase in SafePhases(CapacityTransferSteps(c,f)))yield return phase;
  if(!CurrentCity(c,f,governor)){InvestmentAudit(c,"excluded:"+Eligibility(c,out _));yield break;}
  if(!CurrentCity(c,f,governor))yield break;
  using(var attack=OffensiveSteps(c,f,governor).GetEnumerator())while(true){
   bool more;try{more=attack.MoveNext();}catch(Exception e){Error(e);break;}
   if(!more)break;yield return attack.Current;
   if(!CurrentCity(c,f,governor))yield break;
  }
  bool invest=investment.Value&&AIGate("invest:"+c.OJPECMNINNA,f.IBGCLCAIBOL,2);
  if(invest){
   AuditCapacity(c,"work-start");
   if(CurrentCity(c,f,governor)){
    // Diagnostics are independent yielded work, never a prerequisite for spending.
    work.Add("income:"+c.OJPECMNINNA,()=>SafePhases(AuditCityIncomeSteps(c,f)).GetEnumerator());
    AuditDevelopmentAgenda(c,f);
   }yield return "development-audit";
   if(!CurrentCity(c,f,governor))yield break;
   var row=c.LMPMNLLGOEC;
   if(budget.Value&&row.KJPPGIOCDID!=null&&row.KJPPGIOCDID.Count<c.JNLIOIHLDHH()&&row.PNHICLAGHGD>0){
    Fund(c,true);if(row.DNOGPFHMOMO>=50001&&AIGate("staff-invest:"+c.OJPECMNINNA,f.IBGCLCAIBOL,10))c.OBIOEOJAHMM();
   }yield return "staffing";
   if(!CurrentCity(c,f,governor))yield break;
   for(int pass=0;pass<4;pass++){
    if(!CurrentCity(c,f,governor))yield break;
    if(pass>0)next.Remove("research-invest:"+f.IBGCLCAIBOL+":"+governor);
    int beforeResearch=f.DNOGPFHMOMO;
    try{TryResearch(c,f);}catch(Exception e){Error(e);}finally{researchOwner=null;researchGovernor=null;researchTeam=null;researchHomes=null;}
    yield return "research";
    if(f.DNOGPFHMOMO>=beforeResearch)break;
   }
   if(!CurrentCity(c,f,governor))yield break;
   if(maintenance.Value){using(var steps=MaintenanceSteps(c,f).GetEnumerator())while(true){
    bool more;try{more=steps.MoveNext();}catch(Exception e){Error(e);break;}if(!more)break;
    yield return steps.Current?"maintenance-commit":"maintenance-scan";if(!CurrentCity(c,f,governor))yield break;
   }}yield return "maintenance";
   int built=0;
   if(construction.Value)for(int n=0;n<buildsPerCity.Value;n++){
    if(!CurrentCity(c,f,governor))yield break;
    bool purchased=false;
    using(var scan=BuildSteps(c,f).GetEnumerator())while(scan.MoveNext()){purchased|=scan.Current;yield return scan.Current?"build-commit":"build-scan";if(!CurrentCity(c,f,governor))yield break;}
    if(!purchased)break;built++;
   }
   InvestmentAudit(c,$"evaluated built={built} treasury={f.DNOGPFHMOMO} cityCash={c.LMPMNLLGOEC.DNOGPFHMOMO} scheduled=true");
   // Legal empty plots get serviced before the much larger rebuild search.
   if(CurrentCity(c,f,governor)){
    using(var renewal=RenewalSteps(c,f).GetEnumerator())while(renewal.MoveNext()){
     yield return renewal.Current?"renewal-commit":"renewal-scan";if(!CurrentCity(c,f,governor))yield break;
    }
   }
   AuditCapacity(c,"work-end");
   if(!CurrentCity(c,f,governor))yield break;
   ScheduleStaffing(f);yield return "garrison";
  }
  if(CurrentCity(c,f,governor)&&domestic.Value&&AIGate("domestic:"+c.OJPECMNINNA,f.IBGCLCAIBOL,30)&&DayReady("domestic:"+c.OJPECMNINNA,DecisionDays(f.IBGCLCAIBOL))){
   DayReady("domestic:"+c.OJPECMNINNA,0,true);AssignDomestic(c,f);
  }yield return "domestic";
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
 internal static void PumpEconomy(){
  if(!schedulerOnline||pumping)return;
  long now=Environment.TickCount64;
  if(windowStarted==0)windowStarted=now;
  if(lastPump>0){double gap=now-lastPump;if(gap>windowMaxFrame){windowMaxFrame=gap;gapPrecedingPumpMs=previousPumpMs;}if(gap>=50)windowLongGaps++;}lastPump=now;windowFrames++;
  long pumpStart=Stopwatch.GetTimestamp();
  try{
   pumping=true;
   SyncWorldVisuals();
   bool campaignRunning=!AEHAGJAKNEK.PAKENCLGOHK;
   bool directRunning=Wa_Controller.m_warInfo!=null&&!Wa_Controller.m_isBeforeWar&&!Wa_Controller.m_isQuiting&&!Wa_Controller.m_isByEvent&&Wa_Controller.m_mockInfo==null&&Wa_Controller.m_battleSpeed>0&&(Wa_Controller.m_isRoundMode||campaignRunning);
   if(Ready()&&Time.timeScale>0&&(campaignRunning||directRunning)){
    if(campaignRunning){PollManualOrders();PollDefense();}
    // Cheap yielded probes previously hit the eight-step cap far below the
    // time budget, stretching a city scan into minutes on a populated map.
    windowSteps+=work.Pump(workBudget.Value,128,(label,ms)=>{
     RecordPhaseCost(label,ms);RecordPhaseAllocation(label,work.LastStepAllocatedBytes);
     windowWorkTotal+=ms;if(ms>windowMaxWork){windowMaxWork=ms;slowestWork=label;}
     if(ms>16&&Gate("slow-work:"+label,30))Note($"ai-slow work={label} ms={ms:F2} queue={work.Count}");
    },Error,campaignRunning?null:CurrentBattleJob);
   }
  }catch(Exception e){Error(e);}finally{
   previousPumpMs=(Stopwatch.GetTimestamp()-pumpStart)*1000d/Stopwatch.Frequency;
   windowPumpTotal+=previousPumpMs;windowPumpMax=Math.Max(windowPumpMax,previousPumpMs);
   if(previousPumpMs>workBudget.Value)windowOverBudget++;
   pumping=false;
  }
  if(now>=reportAt){
   reportAt=now+30000;
   ReportPhaseCosts();
   Note($"ai-perf frames={windowFrames} frameGapMaxMs={windowMaxFrame:F2} workSteps={windowSteps} workTotalMs={windowWorkTotal:F2} workMaxMs={windowMaxWork:F2} slowest={slowestWork} nativeCityMaxMs={windowNativeCity:F2} nativePlanMaxMs={windowNativePlan:F2} nativePlanCalls={windowPlanCalls} queue={work.Count} budgetMs={workBudget.Value:F2}");
   Note($"ai-frame-cost elapsedMs={now-windowStarted} pumpTotalMs={windowPumpTotal:F2} pumpMaxMs={windowPumpMax:F2} overBudgetFrames={windowOverBudget} gaps50ms={windowLongGaps} longestGapPrecedingPumpMs={gapPrecedingPumpMs:F2} timeScale={Time.timeScale:F2} campaignPaused={AEHAGJAKNEK.PAKENCLGOHK} gameDay={NEINKOOBJIH.MIPFHCKDJOK:F4} note=frame-gaps-include-non-AI-work");
   windowFrames=windowSteps=windowPlanCalls=0;windowMaxFrame=windowMaxWork=windowWorkTotal=windowNativeCity=windowNativePlan=0;slowestWork="none";
   windowStarted=now;windowPumpTotal=windowPumpMax=gapPrecedingPumpMs=0;windowLongGaps=windowOverBudget=0;
  }
 }
}
