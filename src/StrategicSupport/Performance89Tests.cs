using System;using System.Reflection;using System.Collections;using System.Collections.Generic;using System.Linq;using UnityEngine;
public static class Performance89Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Data<T>(object o,string n)=>(T)o.GetType().GetField(n).GetValue(o);
 static object Plan()=>Activator.CreateInstance(typeof(StrategicSupport).GetNestedType("RoutePlan",BindingFlags.NonPublic),true);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Performance89: "+why);}
 public static void Run(){
  typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);
  var f=NEINKOOBJIH.COCMABJFOFH["F"];var source=MICGPAMNDKM.IJPKEMPIFIH["source"];var target=MICGPAMNDKM.IJPKEMPIFIH["enemyCity"];
  source.BMBMKPGEHBH=new(0);target.BMBMKPGEHBH=new(1000);target.LMPMNLLGOEC.LMMNKIOAGLC="F";
  int probes=0;AOHIBKLOAEI.RegionAt=p=>{probes++;return p.x>=999?target:source;};UnityEngine.AI.NavMesh.Blocked=false;UnityEngine.AI.NavMesh.Detour=0;
  var plan=Plan();var it=((IEnumerable<string>)Call("ReliefPathSteps",source.BMBMKPGEHBH,target.BMBMKPGEHBH,f,null,false,plan)).GetEnumerator();
  int turns=0,peak=0;while(true){int before=probes;bool more=it.MoveNext();peak=Math.Max(peak,probes-before);if(!more)break;turns++;}it.Dispose();
  Check(Data<bool>(plan,"Success")&&Data<double>(plan,"Length")==1000,"sliced route retains complete physical route and length");
  Check(turns>=1&&peak<=2&&probes<=4,"native route topology removes two hundred misleading nearest-city probes");
  object[] sync={source.BMBMKPGEHBH,target.BMBMKPGEHBH,f,null,false,null,0d};
  Check((bool)Call("ReliefPath",sync)&&(double)sync[6]==Data<double>(plan,"Length"),"synchronous final guard uses identical route algorithm");
  plan=Plan();it=((IEnumerable<string>)Call("AttackRouteSteps",source,target,f,plan)).GetEnumerator();turns=peak=0;
  while(true){int before=probes;bool more=it.MoveNext();peak=Math.Max(peak,probes-before);if(!more)break;turns++;}it.Dispose();
  Check(Data<bool>(plan,"Success")&&Data<double>(plan,"Length")==1000&&turns>=1&&peak<=2,"offense uses the same bounded native road validation");
  var hostile=new AFAGFHJONFJ{OJPECMNINNA="block"};hostile.LMPMNLLGOEC.LMMNKIOAGLC="neutral";
  AOHIBKLOAEI.RegionAt=p=>p.x>490&&p.x<510?hostile:p.x>=999?target:source;
  plan=Plan();foreach(var phase in (IEnumerable<string>)Call("ReliefPathSteps",source.BMBMKPGEHBH,target.BMBMKPGEHBH,f,null,false,plan)){}
  Check(Data<bool>(plan,"Success"),"nearby third city is not falsely treated as a transit region");
  source.MEKIDAONKMD[target.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){hostile}};plan=Plan();foreach(var phase in (IEnumerable<string>)Call("ReliefPathSteps",source.BMBMKPGEHBH,target.BMBMKPGEHBH,f,null,false,plan)){}
  Check(!Data<bool>(plan,"Success"),"actual third-party intermediate remains blocked");
  source.MEKIDAONKMD.Clear();
  AOHIBKLOAEI.RegionAt=p=>source;plan=Plan();it=((IEnumerable<string>)Call("ReliefPathSteps",source.BMBMKPGEHBH,target.BMBMKPGEHBH,f,null,false,plan)).GetEnumerator();it.MoveNext();MICGPAMNDKM.IJPKEMPIFIH=new();while(it.MoveNext()){}it.Dispose();Check(!Data<bool>(plan,"Success"),"save switch cancels unfinished route rather than publishing stale success");
  AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;Call("ResetWork");
  typeof(City80Tests).GetMethod("Setup",F).Invoke(null,new object[]{new[]{30,0,0}});f=NEINKOOBJIH.COCMABJFOFH["F"];source=MICGPAMNDKM.IJPKEMPIFIH["city0"];
  var destinations=new List<AFAGFHJONFJ>();int edges=0;foreach(var phase in (IEnumerable<string>)Call("CapacityDestinationSteps",source,f,destinations))edges++;
  Check(edges>=6&&destinations.Count==2,"connectivity scan split without losing reachable cities");
  var costs=(IDictionary)typeof(StrategicSupport).GetField("phaseCosts",F).GetValue(null);costs.Clear();
  Call("RecordPhaseCost","city:one/route-territory",1d);Call("RecordPhaseCost","city:two/route-territory",8d);
  var cost=costs["city/route-territory"];Check(costs.Count==1&&Data<long>(cost,"Count")==2&&Data<double>(cost,"Total")==9&&Data<double>(cost,"Max")==8,"phase statistics combine city IDs without losing duration or calls");
  Check((double)cost.GetType().GetMethod("P95").Invoke(cost,null)==8,"histogram reports p95 upper bound correctly");costs.Clear();
  Call("ResetWork");Console.WriteLine($"PASS R89 {checks} sliced path/connectivity/profiling checks; no FPS claims from mocks.");
 }
}
