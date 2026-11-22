using System;using System.Collections.Generic;using System.Linq;using System.Reflection;
public static class Editor125Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static void Check(bool value,string why){checks++;if(!value)throw new Exception("Editor125: "+why);}
 static IEnumerable<string> Job(List<string> trace,string name,int count){for(int i=0;i<count;i++){trace.Add(name+":"+i);yield return "step";}trace.Add(name+":done");}
 static List<string> Trace(bool observe){
  var q=new WorkQueue();var trace=new List<string>();int samples=0,turns=0;
  foreach(var name in new[]{"city:A","city:B","battle-support-plan:war","recruit-queue:F","campaign:army","defense:A"})q.Add(name,()=>Job(trace,name,8).GetEnumerator());
  while(q.Count>0&&turns++<1000)q.Pump(10000,1,observe?(name,ms)=>{samples++;}:null,e=>throw e);
  Check(q.Count==0,"all priority lanes complete with observation="+observe);
  Check(observe?samples==54:samples==0,"observer never controls work completion");
  if(!observe)Check(q.LastStepAllocatedBytes==0,"production path does not collect allocation statistics");
  return trace;
 }
 public static void Run(){
  var old=Trace(true);var current=Trace(false);
  Check(old.SequenceEqual(current),"profiling removal preserves complete action order across battle, recruitment, recovery, defense and city work");
  var q=new WorkQueue();var trace=new List<string>();q.Add("city",()=>Job(trace,"city",4).GetEnumerator());
  Check(q.Pump(0,128,null,e=>throw e)==1&&q.Count==1&&trace.Count==1,"zero frame budget still stops after one indivisible call without an observer");
  q.Clear();trace.Clear();q.Add("city",()=>Job(trace,"city",3).GetEnumerator());q.Add("battle",()=>Job(trace,"battle",2).GetEnumerator());
  q.Pump(10000,100,null,e=>throw e,key=>key=="battle");
  Check(q.Count==1&&trace.All(x=>x.StartsWith("battle:")),"paused campaign filtering retained without performance recording");
  q.Pump(10000,100,null,e=>throw e);Check(q.Count==0&&trace.Count(x=>x.StartsWith("city:"))==4,"filtered work resumes without loss");
  q.Clear();int failures=0,disposed=0;
  IEnumerable<string> Fault(){try{yield return "ready";throw new InvalidOperationException("expected");}finally{disposed++;}}
  q.Add("bad",()=>Fault().GetEnumerator());q.Add("good",()=>Job(trace,"good",1).GetEnumerator());
  while(q.Count>0)q.Pump(10000,100,null,e=>failures++);
  Check(failures==1&&disposed==1&&trace.Contains("good:done")&&!WorkQueue.InStep,"exceptions clean up scopes and do not stop unrelated jobs");
  var action=typeof(StrategicSupport).GetMethod("TimedAI",F).MakeGenericMethod(typeof(int));int calls=0;
  Check((int)action.Invoke(null,new object[]{null,"editor-test",(Func<int>)(()=>{calls++;return 42;})})==42&&calls==1,"unprofiled native wrapper executes action exactly once and preserves result");
  var expected=new InvalidOperationException("native failure");bool propagated=false;
  try{action.Invoke(null,new object[]{null,"editor-test",(Func<int>)(()=>throw expected)});}catch(TargetInvocationException e){propagated=ReferenceEquals(e.InnerException,expected);}
  Check(propagated,"native exception identity retained");
  Check(new[]{"HookWorldTiming","BeginWorldTiming","EndWorldTiming","BeginNativeCityTiming","BeginNativePlanTiming","RecordPhaseCost","ReportPhaseCosts"}.All(n=>typeof(StrategicSupport).GetMethod(n,F)==null),"profiling hooks and aggregation absent from compiled candidate");
  Check(typeof(StrategicSupport).GetMethod("GuardFamilyRelocation",F)!=null&&typeof(StrategicSupport).GetMethod("AfterManualHeroJob",F)!=null,"family protection and manual authority retained");
  Console.WriteLine("PASS Editor125 "+checks+" observer/no-observer differential action traces, budget, pause, exception and compiled hook-removal checks; editor/game execution not simulated.");
 }
}
