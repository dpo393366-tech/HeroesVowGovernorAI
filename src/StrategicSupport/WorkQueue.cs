using System;
using System.Collections.Generic;
using System.Diagnostics;

// Managed queue only. The caller pumps on Unity's main thread; no game objects
// or native APIs are ever read on the log writer thread.
public sealed class WorkQueue
{
 [ThreadStatic] public static bool InStep;
 [ThreadStatic] public static long StepToken;
 sealed class Job { public string Key;public IEnumerator<string> Steps;public int Generation;public bool Urgent,Service,Critical,BattlePlan,Recovery,Economy,Planning,Construction; }
 readonly Queue<Job> jobs=new(),urgent=new(),service=new(),critical=new(),battlePlans=new(),recovery=new(),economy=new(),planning=new(),construction=new();readonly HashSet<string> keys=new();int generation,urgentStreak,serviceGap,criticalGap=3,recoveryGap=3,economyGap=7;bool planningTurn=true,constructionTurn=true,battleTurn=true;
 void Enqueue(Job job){(job.BattlePlan?battlePlans:job.Critical?critical:job.Planning?planning:job.Construction?construction:job.Economy?economy:job.Service?service:job.Recovery?recovery:job.Urgent?urgent:jobs).Enqueue(job);}
 public int Count=>keys.Count;
 public long LastStepAllocatedBytes {get;private set;}
 public bool Add(string key,Func<IEnumerator<string>> create){
  if(keys.Contains(key)||keys.Count>=2048)return false;
  var job=new Job{Key=key,Steps=create(),Generation=generation,Economy=key.StartsWith("economy:")||key.StartsWith("layout:"),Planning=key.StartsWith("layout:"),Construction=key.StartsWith("economy:construction:"),BattlePlan=key.StartsWith("battle-support-plan:"),Critical=key.StartsWith("defense-watch:")||key.StartsWith("battle-support-plan:")||key.StartsWith("battle-route:"),Service=key.StartsWith("recruit-queue:"),Recovery=key.StartsWith("campaign:")||key.StartsWith("campaign-rest:"),Urgent=key.StartsWith("hunt:")||key.StartsWith("threat-route:")||key.StartsWith("staffing-occupation:")||key.StartsWith("staffing-urgent:")||key.StartsWith("capacity-emergency:")||key=="defense-pulse"||key.StartsWith("defense:")||key.StartsWith("defense-cover:")||key.StartsWith("campaign-return:")||key.StartsWith("intercept:")||key.StartsWith("crossfront:")||key.StartsWith("mobile-relief:")};keys.Add(key);Enqueue(job);return true;
 }
 public void Clear(){generation++;keys.Clear();urgentStreak=serviceGap=0;criticalGap=recoveryGap=3;economyGap=7;planningTurn=constructionTurn=battleTurn=true;foreach(var queue in new[]{jobs,urgent,service,critical,battlePlans,recovery,economy,planning,construction})while(queue.Count>0){var j=queue.Dequeue();try{j.Steps.Dispose();}catch{}}}
 bool Take(Queue<Job> queue,Predicate<string> eligible,out Job job){
  int count=queue.Count;while(count-->0){var next=queue.Dequeue();if(eligible==null||eligible(next.Key)){job=next;return true;}queue.Enqueue(next);}
  job=null;return false;
 }
 public int Pump(double budgetMs,int maxSteps,Action<string,double> measured,Action<Exception> error,Predicate<string> eligible=null){
  long start=Stopwatch.GetTimestamp();int steps=0;
  while((jobs.Count>0||urgent.Count>0||service.Count>0||critical.Count+battlePlans.Count>0||recovery.Count>0||economy.Count>0||planning.Count>0||construction.Count>0)&&steps<maxSteps){
   // Two emergency slices then one ordinary slice: prompt defense without
   // starving construction, research, awards or officer balancing.
   int economyCount=economy.Count+planning.Count+construction.Count;
   bool useUrgent=urgent.Count>0&&(urgentStreak<2||jobs.Count==0);
   // Explicit user recruitment gets one slice per four, within the same
   // frame budget. Hundreds of city tasks cannot stretch one roster scan
   // across months; defense and normal work still receive their slices.
   bool useService=service.Count>0&&(serviceGap>=3||jobs.Count+urgent.Count+recovery.Count+economyCount==0);
   bool useCritical=critical.Count+battlePlans.Count>0&&(criticalGap>=3||jobs.Count+urgent.Count+service.Count+recovery.Count+economyCount==0);
   // Idle armies get a reserved share instead of one turn behind every city's
   // urgent work. Preserve battle/recruitment shares and the same frame budget.
   bool useRecovery=recovery.Count>0&&(recoveryGap>=3||jobs.Count+urgent.Count+economyCount==0);
   bool useEconomy=economyCount>0&&(economyGap>=7||jobs.Count+urgent.Count+service.Count+critical.Count+battlePlans.Count+recovery.Count==0);
   // Use the existing economy share, alternating plan work with execution.
   // A city's long plan no longer sits behind every ordinary world job, and
   // hundreds of daily maintenance admissions cannot drown out plan progress.
   // Keep the planning half intact. Alternate the execution half between
   // ready purchases and maintenance/research, so each purchase does not
   // traverse the entire maintenance backlog. Still one bounded job step.
   var executionLane=construction.Count>0&&(constructionTurn||economy.Count==0)?construction:economy;
   var economyLane=planning.Count>0&&(planningTurn||economy.Count+construction.Count==0)?planning:executionLane;
   var criticalLane=battlePlans.Count>0&&(battleTurn||critical.Count==0)?battlePlans:critical;
   var selected=useCritical?criticalLane:useService?service:useRecovery?recovery:useEconomy?economyLane:useUrgent?urgent:jobs;
   if(!Take(selected,eligible,out var j)&&!Take(urgent,eligible,out j)&&!Take(jobs,eligible,out j)&&!Take(service,eligible,out j)&&!Take(critical,eligible,out j)&&!Take(battlePlans,eligible,out j)&&!Take(recovery,eligible,out j)&&!Take(economy,eligible,out j)&&!Take(planning,eligible,out j)&&!Take(construction,eligible,out j))break;
   if(j.Economy){planningTurn=!j.Planning;if(!j.Planning)constructionTurn=!j.Construction;}
   useService=j.Service;useUrgent=j.Urgent;economyGap=j.Economy?0:Math.Min(7,economyGap+1);
   if(j.Critical){criticalGap=0;battleTurn=!j.BattlePlan;}else criticalGap=Math.Min(3,criticalGap+1);
   if(j.Recovery)recoveryGap=0;else recoveryGap=Math.Min(3,recoveryGap+1);
   if(j.Critical){}else if(useService)serviceGap=0;else {serviceGap=Math.Min(3,serviceGap+1);if(!j.Recovery)urgentStreak=useUrgent?urgentStreak+1:0;}
   long allocated=measured!=null?GC.GetAllocatedBytesForCurrentThread():0;
   bool more=false;long at=measured!=null?Stopwatch.GetTimestamp():0;string stage="end";
   bool previousStep=InStep;StepToken++;InStep=true;
   try{more=j.Steps.MoveNext();if(more)stage=j.Steps.Current;}catch(Exception e){error(e);}finally{InStep=previousStep;}
   LastStepAllocatedBytes=0;
   if(measured!=null){
    double elapsed=(Stopwatch.GetTimestamp()-at)*1000d/Stopwatch.Frequency;
    LastStepAllocatedBytes=Math.Max(0,GC.GetAllocatedBytesForCurrentThread()-allocated);
    measured(j.Key+"/"+stage,elapsed);
   }
   steps++;
   if(more&&j.Generation==generation)Enqueue(j);
   else {if(j.Generation==generation)keys.Remove(j.Key);try{j.Steps.Dispose();}catch(Exception e){error(e);}}
   if((Stopwatch.GetTimestamp()-start)*1000d/Stopwatch.Frequency>=budgetMs)break;
  }
  return steps;
 }
}
