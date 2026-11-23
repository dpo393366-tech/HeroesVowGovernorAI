using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using System.Diagnostics;using UnityEngine;using UnityEngine.AI;
public static class Perf146Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static AFAGFHJONFJ[] cities;static MKMOJNAJKJE faction;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static T Get<T>(object o,string name)=>(T)o.GetType().GetField(name).GetValue(o);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Perf146: "+why);}
 static AFAGFHJONFJ Add(string id,float x)=>(AFAGFHJONFJ)typeof(RouteOrder94Tests).GetMethod("Add",F).Invoke(null,new object[]{id,x});
 static void Setup(int seed){
  typeof(RouteOrder94Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetFixedRoads");((IDictionary)typeof(StrategicSupport).GetField("roadChoices",F).GetValue(null)).Clear();
  faction=NEINKOOBJIH.COCMABJFOFH["F"];cities=Enumerable.Range(0,80).Select(i=>Add("perf-city"+i,i*10)).ToArray();
  var far=Add("detour",2000);cities[0].MEKIDAONKMD[cities[79].OJPECMNINNA]=new(){EJMDIPFNGMG=new(){far}};
  var rand=new Random(seed);for(int i=0;i<79;i++){cities[i].FKMKGNPFLFP.Add(cities[i+1]);for(int j=i+2;j<Math.Min(80,i+12);j++)if(rand.Next(3)==0)cities[i].FKMKGNPFLFP.Add(cities[j]);}
  AOHIBKLOAEI.RegionAt=p=>p.x==2000?far:cities[Math.Clamp((int)Math.Round(p.x/10),0,79)];NavMesh.Blocked=false;NavMesh.Detour=2;NavMesh.Calls105=0;
 }
 static object Plan()=>Activator.CreateInstance(typeof(StrategicSupport).GetNestedType("RoutePlan",BindingFlags.NonPublic),true);
 static object Route(out int yields){var p=Plan();yields=0;foreach(var phase in (IEnumerable<string>)Call("ReliefPathSteps",cities[0].BMBMKPGEHBH,cities[79].BMBMKPGEHBH,faction,null,false,p))yields++;return p;}
 static bool Sync(out double distance,float offset=0){object[] args={new Vector3(offset),cities[79].BMBMKPGEHBH,faction,null,false,null,0d};bool ok=(bool)Call("ReliefPath",args);distance=(double)args[6];return ok;}
 static void Renewal(){
  typeof(MaintenanceTests).GetMethod("Setup",F).Invoke(null,null);
  var c=(AFAGFHJONFJ)typeof(MaintenanceTests).GetField("c",F).GetValue(null);var f=(MKMOJNAJKJE)typeof(MaintenanceTests).GetField("f",F).GetValue(null);var d=(JNHOOCJJPJO)typeof(MaintenanceTests).GetField("d",F).GetValue(null);
  var low=new PABIPDOFBFG{OJPECMNINNA="perf-low",CNCPIILGDEF=100,FHKHHKBBKGM=true};var high=new PABIPDOFBFG{OJPECMNINNA="perf-high",CNCPIILGDEF=300,FHKHHKBBKGM=true};AEHAGJAKNEK.ACIGFKMDOKE.Clear();AEHAGJAKNEK.ACIGFKMDOKE[low.OJPECMNINNA]=low;AEHAGJAKNEK.ACIGFKMDOKE[high.OJPECMNINNA]=high;
  var x=new OGABIJECHFG{GEKKLMPLDHJ=low,GMIKIMACDBC=d};d.AIMLPNBOIIF.Clear();d.AIMLPNBOIIF.Add(x);d.AIMLPNBOIIF.Add(new(){GEKKLMPLDHJ=new(){OJPECMNINNA="protected-output",CNCPIILGDEF=10000},GMIKIMACDBC=d,JOFCACIPJKK=1});c.MoneyIncome=100;
  c.HFLAGGEFBOO.Clear();c.HFLAGGEFBOO.Add(new(){GoalCheck=city=>city.MoneyIncome-(x.EHABIIDOKGC()?100:0)>=100,CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.CityMoneyIncome,KLCDGPDHENB=100}}});
  int calls=OGABIJECHFG.NativeRenewalCalls146,cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
  for(int i=0;i<100;i++){
   ((IDictionary)typeof(StrategicSupport).GetField("next",F).GetValue(null)).Clear();foreach(bool changed in (IEnumerable<bool>)Call("RenewalSteps",c,f))Check(!changed,"no transient completed-goal loss");
  }
  Check(calls==OGABIJECHFG.NativeRenewalCalls146&&x.GEKKLMPLDHJ==low&&cash==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO,"100 rejected renewals cause zero native model rebuilds and no resource change");
  c.MoneyIncome=10000;((IDictionary)typeof(StrategicSupport).GetField("next",F).GetValue(null)).Clear();bool done=false;foreach(bool changed in (IEnumerable<bool>)Call("RenewalSteps",c,f))done|=changed;
  Check(done&&x.GEKKLMPLDHJ==high&&OGABIJECHFG.NativeRenewalCalls146==calls+1,"surplus city still replaces inferior building once at native construction speed");
 }
 static IEnumerator<string> SlowFixture(Action effect){effect();System.Threading.Thread.Sleep(12);yield return "native-call";}
 static void SlowDiagnostic(){
  var queue=new WorkQueue();int effects=0,reports=0;queue.Add("campaign:test",()=>SlowFixture(()=>effects++));
  queue.Pump(2,128,null,e=>throw e,null,(stage,ms)=>{Check(stage=="campaign:test/native-call"&&ms>=8,"only long active step is reported");reports++;});
  Check(effects==1&&reports==1&&queue.Count==1,"slow diagnostic never cancels or reruns game action");
  queue.Pump(2,128,null,e=>throw e);Check(queue.Count==0&&effects==1,"same continuation completes with diagnostics disabled");
 }
 public static void Run(){
  for(int seed=0;seed<20;seed++){
   Setup(seed);var p=Route(out _);Check(Get<bool>(p,"Success"),"connected route "+seed);
   var expected=Enumerable.Repeat(double.PositiveInfinity,80).ToArray();expected[0]=0;
   for(int i=0;i<80;i++)foreach(var n in cities[i].FKMKGNPFLFP){int j=Array.IndexOf(cities,n);expected[j]=Math.Min(expected[j],expected[i]+n.BMBMKPGEHBH.x-cities[i].BMBMKPGEHBH.x+4);}
   Check(Math.Abs(Get<double>(p,"Length")-expected[79])<.001,"independent shortest-path oracle "+seed);
   Check(Sync(out double length)&&Math.Abs(length-expected[79])<.001,"fresh selected road equals planner "+seed);
  }
  Setup(7);Route(out _);((IDictionary)typeof(StrategicSupport).GetField("roadChoices",F).GetValue(null)).Clear();
  long bytes=GC.GetAllocatedBytesForCurrentThread();var timer=Stopwatch.StartNew();var warm=Route(out int slices);double ms=timer.Elapsed.TotalMilliseconds;bytes=GC.GetAllocatedBytesForCurrentThread()-bytes;
  int calls=NavMesh.Calls105;timer.Restart();for(int i=0;i<100;i++)Check(Sync(out _),"repeat command "+i);int selectedCalls=NavMesh.Calls105-calls;
  Console.WriteLine($"BENCH146 seed=7 cities=80 warmGraphSlices={slices} warmGraphBytes={bytes} warmGraphMs={ms:F3} repeatedCommands=100 nativeCalls={selectedCalls} selectedLegs={Get<List<Vector3>>(warm,"Legs").Count} commandMs={timer.Elapsed.TotalMilliseconds:F3}; managed fixture, not game FPS");
  calls=NavMesh.Calls105;timer.Restart();for(int i=1;i<=100;i++)Check(Sync(out _,i*.001f),"moving origin command "+i);
  Check(NavMesh.Calls105-calls<=2000,"moving origin cannot repeat all alternative native road searches");
  Console.WriteLine($"BENCH146 movingCommands=100 nativeCalls={NavMesh.Calls105-calls} elapsedMs={timer.Elapsed.TotalMilliseconds:F3}; exact moving origin, immutable city roads; fixture only");
  NavMesh.Blocked=true;Check(!Sync(out _),"fresh physical failure still rejects cached route");NavMesh.Blocked=false;Check(Sync(out _),"navigation recovers after rejected leg");
  cities[79].LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!Sync(out _),"captured destination blocks at command time");
  faction.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.ArmyPass;Check(Sync(out _),"new passage treaty takes effect immediately");
  var pending=Plan();var it=((IEnumerable<string>)Call("ReliefPathSteps",cities[0].BMBMKPGEHBH,cities[79].BMBMKPGEHBH,faction,null,false,pending)).GetEnumerator();Check(it.MoveNext(),"policy yield retained");faction.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.War;while(it.MoveNext()){}it.Dispose();Check(!Get<bool>(pending,"Success"),"policy change while suspended cannot publish route");
  AOHIBKLOAEI.RegionAt=null;NavMesh.Detour=0;NavMesh.Blocked=false;Renewal();SlowDiagnostic();Call("ResetWork");Console.WriteLine($"PASS Perf146 {checks} shortest road oracle, physical failure, ownership and yielded-policy regressions.");
 }
 public static int Main(){try{Run();return 0;}catch(Exception e){Console.WriteLine(e);return 1;}}
}
