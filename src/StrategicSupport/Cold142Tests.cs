using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class Cold142Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int count;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static object Fixture(Type t,string n,params object[] a)=>t.GetMethod(n,F).Invoke(null,a);
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
 static void Set(string n,object v)=>typeof(StrategicSupport).GetField(n,F).SetValue(null,v);
 static void Check(bool b,string n){count++;if(!b)throw new Exception("Cold142: "+n);}
 static void Drain(WorkQueue q){for(int i=0;q.Count>0&&i<30000;i++)q.Pump(1000,1,null,e=>throw e);Check(q.Count==0,"bounded queue completes");}
 static IEnumerable<string> Job(Action action,int steps=100){for(int i=0;i<steps;i++){action();yield return "slice";}}
 static (AFAGFHJONFJ,NJHDEHLHENN,List<string>,WorkQueue) Battle(int remote=100){
  Fixture(typeof(RouteOrder94Tests),"Setup");Call("ResetManualOrders");
  var near=Field<AFAGFHJONFJ>(typeof(RouteOrder94Tests),"near");var war=Field<NJHDEHLHENN>(typeof(RouteOrder94Tests),"war");
  war.IJGLCFJOOIK=MICGPAMNDKM.IJPKEMPIFIH["enemyCity"];war.IJGLCFJOOIK.BMBMKPGEHBH=war.GBHGENNMJCK;war.IJGLCFJOOIK.FKMKGNPFLFP.Add(near);
  for(int i=0;i<remote;i++)Fixture(typeof(RouteOrder94Tests),"Add","remote"+i,-200f-i*10);
  Set("schedulerOnline",true);return(near,war,Field<List<string>>(typeof(RouteOrder94Tests),"sent"),Field<WorkQueue>(typeof(StrategicSupport),"work"));
 }
 static void BattleChecks(){
  var (near,war,sent,q)=Battle();int decisions=0,steps=0;var generated=near.Generated;near.Generated=()=>{decisions=Field<IDictionary>(typeof(StrategicSupport),"battleRoutes").Keys.Cast<string>().Count(k=>k.Contains(":F:"));generated();};Call("Assess",war,0d,true);
  while(sent.Count==0&&q.Count>0&&steps++<30000)q.Pump(1000,1,null,e=>throw e);
  Check(sent.Count>0&&sent[0]=="near","cold loaded battle sends local help before remote scan");
  Check(decisions==1,"one connected source evaluated before hundred remote quotes count="+decisions);
  Drain(q);var routes=Field<IDictionary>(typeof(StrategicSupport),"battleRoutes");Check(routes.Count>=102,"remote support still evaluated after first wave");
  Check(sent.Count(x=>x=="near")==1,"local donor never duplicated in remaining batch");
  (near,war,sent,q)=Battle(12);near.LMPMNLLGOEC.OEGNGFBCHFL=0;Call("Assess",war,0d,true);Drain(q);
  Check(!sent.Contains("near")&&sent.Contains("source"),"local shortage cannot suppress remote relief");
  (near,war,sent,q)=Battle(12);Call("Assess",war,0d,true);Call("ForgetBattle",war);Drain(q);Check(sent.Count==0,"ended loaded battle cannot launch either tier");
  (near,war,sent,q)=Battle(12);Call("Assess",war,0d,true);near.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Drain(q);Check(!sent.Contains("near"),"captured source cannot ship troops through queued tier");
  // With a complete cache, even an indirect source with a shorter actual
  // road must be first. No straight-line distance is used by production.
  (near,war,sent,q)=Battle(0);var source=MICGPAMNDKM.IJPKEMPIFIH["source"];source.BMBMKPGEHBH=new(99);var faction=NEINKOOBJIH.COCMABJFOFH["F"];
  foreach(var c in new[]{source,near})foreach(var p in (IEnumerable<string>)Call("BattleRouteQuote",c,war,faction,war.Pointer+":"+war.HIENOINGBCN+":F:"+c.OJPECMNINNA)){}
  Call("Assess",war,0d,true);Drain(q);Check(sent.Count>0&&sent[0]=="source","warm plan retains global real-road ranking");
  Set("schedulerOnline",false);Call("ResetWork");
 }
 static void HuntChecks(){
  Fixture(typeof(Hunt94Tests),"Setup",50);var source=MICGPAMNDKM.IJPKEMPIFIH["Runan"];var faction=NEINKOOBJIH.COCMABJFOFH["F"];var enemy=ICGAEMLACJG.HFJIECIBFKB[1407];
  for(int i=0;i<200;i++)Fixture(typeof(Relief86Tests),"City","unrelated"+i,500f+i);
  int scanned=0;foreach(var p in (IEnumerable<string>)Call("HuntSteps",faction))if(p=="hunt-source")scanned++;
  Check(scanned==1,"hunt skips unrelated cities before yielding");Check(source.Last!=null&&!source.Last.Dismissed&&source.Last.m_attackTarget==enemy,"filtered hunt still uses real native assembly and target");
  int calls=source.Calls;foreach(var p in (IEnumerable<string>)Call("HuntSteps",faction)){}Check(source.Calls==calls,"active hunt prevents duplicate launch");
  Fixture(typeof(Hunt94Tests),"Setup",50);source=MICGPAMNDKM.IJPKEMPIFIH["Runan"];faction=NEINKOOBJIH.COCMABJFOFH["F"];enemy=ICGAEMLACJG.HFJIECIBFKB[1407];
  var it=((IEnumerable<string>)Call("HuntSteps",faction)).GetEnumerator();Check(it.MoveNext(),"hunt yields before source validation");faction.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;while(it.MoveNext()){}it.Dispose();Check(source.Calls==0,"changed governor policy rechecked after yield");
  Fixture(typeof(Hunt94Tests),"Setup",50);source=MICGPAMNDKM.IJPKEMPIFIH["Runan"];faction=NEINKOOBJIH.COCMABJFOFH["F"];enemy=ICGAEMLACJG.HFJIECIBFKB[1407];
  enemy.Position=new(80);object[] args={source,faction,enemy,0};Check((bool)Call("HuntRoute",args)&&(int)args[3]==20,"existing twenty day hunt limit retained");enemy.Position=new(84);args[3]=0;Check(!(bool)Call("HuntRoute",args)&&(int)args[3]==21,"travel limit distinguishable from unreachable route");
  NavMesh.Blocked=true;args[3]=0;Check(!(bool)Call("HuntRoute",args)&&(int)args[3]==0,"physical path failure distinguished without relaxing reachability");NavMesh.Blocked=false;
  Call("ResetWork");Call("ResetOffense");AOHIBKLOAEI.RegionAt=null;
 }
 static void QueueChecks(){
  var q=new WorkQueue();int plans=0,watches=0,normal=0,hunt=0,economy=0,service=0,recovery=0;
  for(int i=0;i<300;i++)q.Add("defense-watch:"+i,()=>Job(()=>watches++).GetEnumerator());
  q.Add("battle-support-plan:loaded",()=>Job(()=>plans++).GetEnumerator());
  for(int i=0;i<200;i++)q.Add("ordinary:"+i,()=>Job(()=>normal++).GetEnumerator());
  q.Add("hunt:F",()=>Job(()=>hunt++).GetEnumerator());q.Add("economy:construction:A",()=>Job(()=>economy++).GetEnumerator());q.Add("recruit-queue:F",()=>Job(()=>service++).GetEnumerator());q.Add("campaign:idle",()=>Job(()=>recovery++).GetEnumerator());
  q.Pump(1000,1,null,e=>throw e);Check(plans==1,"loaded battle plan not queued behind hundreds of watchers");
  q.Pump(10000,127,null,e=>throw e);Check(plans>1&&watches>0,"plan and watcher slices both progress");Check(normal>0&&hunt>0&&economy>0&&service>0&&recovery>0,"same frame budget preserves every work class");
  q.Clear();Check(q.Count==0,"clear removes newly separated battle lane");q.Add("battle-support-plan:loaded",()=>Job(()=>plans++).GetEnumerator());int before=plans;q.Pump(1000,1,null,e=>throw e);Check(plans==before+1,"cleared battle key can be rescheduled");q.Clear();
  q.Add("battle-support-plan:blocked",()=>Job(()=>plans++).GetEnumerator());q.Add("ordinary:allowed",()=>Job(()=>normal++).GetEnumerator());int ordinary=normal;q.Pump(1000,1,null,e=>throw e,key=>key.StartsWith("ordinary:"));Check(normal==ordinary+1,"ineligible plan does not block eligible work");q.Clear();
 }
 public static void Run(){BattleChecks();HuntChecks();QueueChecks();Console.WriteLine($"PASS Cold142 {count} cold battle dispatch, hunt scan latency, invalidation and queue fairness checks; native actions mocked.");}
}
