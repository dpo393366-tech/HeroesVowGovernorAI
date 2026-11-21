using System;using System.Collections.Generic;using System.Linq;using System.Reflection;
public class GlobalUpdateCtrl {public void Update(){}}
public static class Batch98Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] args)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,args);
 static void Check(bool value,string why){checks++;if(!value)throw new Exception("Batch98 "+why);}
 static void Setup()=>typeof(StatecraftTests).GetMethod("Setup",F).Invoke(null,null);
 static JFLCKHMFGPJ Add(string id,int kind,int amount=35)=>(JFLCKHMFGPJ)typeof(StatecraftTests).GetMethod("Strategy",F).Invoke(null,new object[]{id,kind,amount,0});
 static MKMOJNAJKJE Owner=>(MKMOJNAJKJE)typeof(StatecraftTests).GetField("f",F).GetValue(null);
 static List<AFAGFHJONFJ> Cities=>(List<AFAGFHJONFJ>)typeof(StatecraftTests).GetField("cities",F).GetValue(null);
 public static void Run(){
  Setup();var f=Owner;f.PEGLDBENJMF=200000;
  for(int i=0;i<20;i++)Add("benefit"+i,i%2==0?11:8);
  int committed=0,slices=0;foreach(bool done in (IEnumerable<bool>)Call("StateStrategyBatchSteps",f,Cities,true)){if(done)committed++;slices++;}
  Check(committed==20&&f.MEAMPLFJPNH.Count==20,"twenty pure research/exploration buffs committed in one census batch");Check(f.PEGLDBENJMF==198000,"all native start costs paid");
  Check(slices<140,"bounded batch does not repeat whole-census twenty times");
  foreach(bool done in (IEnumerable<bool>)Call("StateStrategyBatchSteps",f,Cities,true))Check(!done,"active buffs cannot duplicate");
  Setup();f=Owner;var first=Add("a",11);var second=Add("b",8);f.PEGLDBENJMF=200000;
  var iter=((IEnumerable<bool>)Call("StateStrategyBatchSteps",f,Cities,true)).GetEnumerator();bool changed=false;int steps=0;
  while(iter.MoveNext()){steps++;if(steps==4){first.LGGBONFIIJN=true;changed=true;}}
  Check(changed&&f.MEAMPLFJPNH.Any(x=>x.BHIBPLKOFKH==second)&&!f.MEAMPLFJPNH.Any(x=>x.BHIBPLKOFKH==first),"native ban changed during scan cannot starve later valid choice");iter.Dispose();
  Setup();var city=Cities[0];WorkQueue.InStep=true;WorkQueue.StepToken++;
  var flow=Call("ReadCivicFlow",city);Check(ReferenceEquals(flow,Call("ReadCivicFlow",city)),"civic snapshot shared inside one queue step");
  WorkQueue.StepToken++;Check(!ReferenceEquals(flow,Call("ReadCivicFlow",city)),"snapshot expires at next step, never across frames");
  var population=Call("PopulationSnapshot",city);Check(ReferenceEquals(population,Call("PopulationSnapshot",city)),"population computation reused in current step");
  Call("InvalidatePlanningReads");Check(!ReferenceEquals(population,Call("PopulationSnapshot",city)),"mutation invalidates same-step population snapshot");WorkQueue.InStep=false;
  var q=new WorkQueue();IEnumerable<string> Fault(){if(Environment.TickCount64>=0)throw new Exception("injected");
   yield return "unreachable";
  }
  q.Add("test",()=>Fault().GetEnumerator());q.Pump(20,1,(_,__)=>{},_=>{});Check(!WorkQueue.InStep,"exception releases snapshot scope");
  Setup();var building=new PABIPDOFBFG{FHKHHKBBKGM=true,MLAJLJKFGBD=2,PNPLBNAFMCM=2};
  object[] before={building,false};typeof(StrategicSupport).GetMethod("BeginPlacementAdjacency",F).Invoke(null,before);
  Check(!building.FHKHHKBBKGM,"road requirement disabled inside original predicate");
  Check(building.MLAJLJKFGBD==2&&building.PNPLBNAFMCM==2,"footprint remains unchanged");
  Call("EndPlacementAdjacency",building,before[1]);Check(building.FHKHHKBBKGM,"original definition restored even after failed placement");
  var setting=(BepInEx.Configuration.ConfigEntry<bool>)typeof(StrategicSupport).GetField("relaxBuildingAdjacency",F).GetValue(null);setting.Value=false;
  before=new object[]{building,false};typeof(StrategicSupport).GetMethod("BeginPlacementAdjacency",F).Invoke(null,before);Check(building.FHKHHKBBKGM,"disabled option retains native restriction");Call("EndPlacementAdjacency",building,before[1]);setting.Value=true;
  Console.WriteLine($"PASS Batch98 {checks} pure-benefit batching, option changes, cache lifetime and failure checks; native actions mocked.");
 }
}

namespace UnityEngine.EventSystems {public class EventSystem{public void Update(){}}}
public class UI_DeployUI {public class GMPKDAEEJMK{public bool MoveNext()=>false;}}
public class Wo_WorldPlaceCtrl {public void MouseLeftPress(){}}
public class Wo_PassCtrl {public void MouseLeftPress(){}}
