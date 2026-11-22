using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class Roads143Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static AFAGFHJONFJ a,b,via;static MKMOJNAJKJE f;
 static object Call(string n,params object[] args)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,args);
 static T Field<T>(string n)=>(T)typeof(StrategicSupport).GetField(n,F).GetValue(null);
 static T Get<T>(object o,string n)=>(T)o.GetType().GetField(n).GetValue(o);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Roads143: "+why);}
 static object Plan()=>Activator.CreateInstance(typeof(StrategicSupport).GetNestedType("RoutePlan",BindingFlags.NonPublic),true);
 static object Route(Vector3 start,Vector3 end,MKMOJNAJKJE faction,string allowed,out int graph){var plan=Plan();graph=0;foreach(var step in (IEnumerable<string>)Call("ReliefPathSteps",start,end,faction,allowed,false,plan))if(step=="route-graph")graph++;return plan;}
 static AFAGFHJONFJ Add(string id,float x)=>(AFAGFHJONFJ)typeof(RouteOrder94Tests).GetMethod("Add",F).Invoke(null,new object[]{id,x});
 static void Setup(){
  typeof(RouteOrder94Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetFixedRoads");Field<IDictionary>("roadChoices").Clear();
  f=NEINKOOBJIH.COCMABJFOFH["F"];a=MICGPAMNDKM.IJPKEMPIFIH["source"];b=MICGPAMNDKM.IJPKEMPIFIH["enemyCity"];a.BMBMKPGEHBH=new(0);b.BMBMKPGEHBH=new(100);b.LMPMNLLGOEC.LMMNKIOAGLC="F";
  a.FKMKGNPFLFP.Add(b);AOHIBKLOAEI.RegionAt=p=>p.x>=99?b:a;NavMesh.Detour=0;NavMesh.Blocked=false;NavMesh.Calls105=0;
 }
 static void Geometry(){
  Setup();NavMesh.Detour=30;var road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);Check(Get<bool>(road,"Success")&&Get<double>(road,"Length")==160,"saved road uses actual detour corners, not direct distance");int cold=NavMesh.Calls105;
  foreach(var item in Field<IDictionary>("roadChoices").Values)item.GetType().GetField("Until").SetValue(item,0L);
  road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out int graphs);Check(graphs==0,"fixed road choice does not expire after former three-second TTL");
  for(int i=0;i<200;i++){road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);Check(Get<bool>(road,"Success")&&Get<double>(road,"Length")==160,"repeat preserves physical road "+i);}
  Check(NavMesh.Calls105==cold,"two hundred route plans add zero native path searches");
  var unrelated=Add("unrelated",500);road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);int warm=NavMesh.Calls105;unrelated.LMPMNLLGOEC.LMMNKIOAGLC="enemy";road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);
  Check(Get<bool>(road,"Success")&&NavMesh.Calls105==warm,"unrelated capture updates passage without recalculating geometry");
  var other=NEINKOOBJIH.COCMABJFOFH["enemy"];other.AENJACOFBJK["F"].EBIOPDHEBOF=BHBNKONCOJD.Ally;road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,other,null,out _);Check(Get<bool>(road,"Success")&&NavMesh.Calls105==warm,"different faction shares the same immutable road shape");
  other.AENJACOFBJK["F"].EBIOPDHEBOF=BHBNKONCOJD.War;road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,other,null,out _);Check(!Get<bool>(road,"Success"),"shared geometry never shares faction permission");
  Get<List<Vector3>>(road,"Points").Clear();road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);Check(Get<double>(road,"Length")==160&&Get<List<Vector3>>(road,"Points").Count>0,"callers cannot mutate stored corner chain");
  road=Route(a.BMBMKPGEHBH,new Vector3(110),f,null,out _);Check(Get<bool>(road,"Success")&&NavMesh.Calls105>warm,"different exact endpoint requires its own physical route");
  warm=NavMesh.Calls105;var previous=MICGPAMNDKM.IJPKEMPIFIH;MICGPAMNDKM.IJPKEMPIFIH=new();foreach(var kv in previous)MICGPAMNDKM.IJPKEMPIFIH[kv.Key]=kv.Value;
  road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);Check(NavMesh.Calls105>warm,"new world invalidates geometry rather than using another map");
  NavMesh.Blocked=true;object[] sync={a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,false,null,0d};Check(!(bool)Call("ReliefPath",sync),"selected command still rejects fresh physical failure");NavMesh.Blocked=false;
  warm=NavMesh.Calls105;road=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);Check(Get<bool>(road,"Success")&&NavMesh.Calls105>warm,"failed final leg is discarded and recovers after repair");
  Console.WriteLine($"Roads143 fixed route: cold native calls={cold};200 repeated plans native calls=0; mock navigation only.");
 }
 static void Passage(){
  Setup();via=Add("via",50);a.MEKIDAONKMD[b.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){via}};a.FKMKGNPFLFP.Clear();a.FKMKGNPFLFP.Add(via);via.FKMKGNPFLFP.Add(b);AOHIBKLOAEI.RegionAt=p=>p.x<25?a:p.x<75?via:b;
  var p=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);Check(Get<bool>(p,"Success"),"real intermediate route initially allowed");int warmed=NavMesh.Calls105;
  via.LMPMNLLGOEC.LMMNKIOAGLC="enemy";p=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);Check(!Get<bool>(p,"Success"),"captured intermediate blocks previously stored road");
  f.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.ArmyPass;p=Route(a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,out _);Check(Get<bool>(p,"Success")&&NavMesh.Calls105==warmed,"passage treaty restores route without geometry calculation");
  var pending=Plan();var it=((IEnumerable<string>)Call("ReliefPathSteps",a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,false,pending)).GetEnumerator();Check(it.MoveNext(),"planned route exposes policy boundary");f.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.War;while(it.MoveNext()){}it.Dispose();Check(!Get<bool>(pending,"Success"),"diplomacy change during yielded work cannot publish stale passage");
 }
 static void LargeMap(){
  Setup();AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;var cities=Enumerable.Range(0,80).Select(i=>Add("bulk"+i,i*10)).ToArray();var end=new Vector3(1000);
  foreach(var city in cities)Check(Get<bool>(Route(city.BMBMKPGEHBH,end,f,null,out _),"Success"),"80-source cold quote");int cold=NavMesh.Calls105;var watch=System.Diagnostics.Stopwatch.StartNew();
  for(int change=0;change<12;change++){
   b.LMPMNLLGOEC.LMMNKIOAGLC=change%2==0?"enemy":"F";
   foreach(var city in cities)Check(Get<bool>(Route(city.BMBMKPGEHBH,end,f,null,out _),"Success"),"ownership refresh does not remove physical route");
  }
  Check(NavMesh.Calls105==cold,"80 sources and12 ownership changes add zero native geometry searches");Console.WriteLine($"Roads143 large-map replay:80 sources,12 ownership changes,960 plans,new native calls={NavMesh.Calls105-cold},managedMs={watch.Elapsed.TotalMilliseconds:F2}; synthetic, not live FPS.");
 }
 static void NextRing(){
  var setup=typeof(Cold142Tests).GetMethod("Battle",F).Invoke(null,new object[]{100});
  var tuple=((AFAGFHJONFJ,NJHDEHLHENN,List<string>,WorkQueue))setup;var near=tuple.Item1;var war=tuple.Item2;var sent=tuple.Item3;var queue=tuple.Item4;
  near.LMPMNLLGOEC.OEGNGFBCHFL=0;var next=Add("second-ring",70);near.FKMKGNPFLFP.Add(next);int quotes=0;var generated=next.Generated;next.Generated=()=>{quotes=Field<IDictionary>("battleRoutes").Keys.Cast<string>().Count(k=>k.Contains(":F:"));generated();};
  Call("Assess",war,0d,true);for(int i=0;sent.Count==0&&queue.Count>0&&i<30000;i++)queue.Pump(1000,1,null,e=>throw e);
  Check(sent.Count>0&&sent[0]=="second-ring"&&quotes==2,"unavailable nearest city advances to second ring before hundred unrelated routes");
  for(int i=0;queue.Count>0&&i<30000;i++)queue.Pump(1000,1,null,e=>throw e);Check(queue.Count==0,"every remaining source still finishes after early wave");
  Check(sent.Count(x=>x=="second-ring")==1,"successive rings do not double-book native units");
  typeof(StrategicSupport).GetField("schedulerOnline",F).SetValue(null,false);Call("ResetWork");
 }
 public static void Run(){Geometry();Passage();LargeMap();NextRing();AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;NavMesh.Detour=0;Call("ResetWork");Console.WriteLine($"PASS Roads143 {checks} fixed geometry, live passage, world/endpoint invalidation, physical guard and progressive support checks; native calls mocked.");}
}
