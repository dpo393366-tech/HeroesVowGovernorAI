using System;using System.Collections.Generic;using System.Linq;using System.Reflection;using System.Diagnostics;using UnityEngine;
public class Wa_ArmyCtrl:Native {
 public List<Vector3> m_listDestination=new(),NJEMJKJHHBE=new();public object m_attribute=new();public DrawComponent m_transform=new(),m_navAgent=new();public LineRenderer GFJGCKCEIHC=new(),KIJLPJKGJMJ=new();public static implicit operator bool(Wa_ArmyCtrl a)=>a!=null;
}
public class DrawComponent {public static implicit operator bool(DrawComponent c)=>c!=null;}
public static class Perf102Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int n;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string why){n++;if(!ok)throw new Exception("Perf102: "+why);}
 public static void Run(){
  typeof(IntegrationTests).GetMethod("Prepare",F).Invoke(null,null);var f=NEINKOOBJIH.COCMABJFOFH["F"];
  var cities=MICGPAMNDKM.IJPKEMPIFIH.Values.ToList();
  for(int i=0;i<80;i++){var c=new AFAGFHJONFJ{OJPECMNINNA="road-test"+i};c.LMPMNLLGOEC.LMMNKIOAGLC="F";foreach(var near in cities.TakeLast(4))c.FKMKGNPFLFP.Add(near);cities.Add(c);MICGPAMNDKM.IJPKEMPIFIH[c.OJPECMNINNA]=c;}
  for(int i=0;i<100;i++){
   var c=cities[i%cities.Count];c.LMPMNLLGOEC.LMMNKIOAGLC=i%2==0?"F":"enemy";
   if(i%7==0)c.FKMKGNPFLFP.Add(cities[(i+1)%cities.Count]);
   f.AENJACOFBJK["enemy"].EBIOPDHEBOF=i%3==0?BHBNKONCOJD.Ally:BHBNKONCOJD.War;
   Check((string)Call("RoadPolicyStamp",f)==(string)Call("BuildRoadPolicyStamp",f),"live owner/edge/diplomacy changes match exact original stamp");
  }
  var first=(string)Call("RoadPolicyStamp",f);Check(ReferenceEquals(first,Call("RoadPolicyStamp",f)),"unchanged roads reuse exact stamp object without TTL");
  var args=new object[]{f};foreach(var name in new[]{"BuildRoadPolicyStamp","RoadPolicyStamp"}){
   var method=typeof(StrategicSupport).GetMethod(name,F);method.Invoke(null,args);long bytes=GC.GetAllocatedBytesForCurrentThread();var timer=Stopwatch.StartNew();
   for(int i=0;i<1000;i++)method.Invoke(null,args);
   Console.WriteLine("BENCH102 "+name+" calls=1000 bytes="+(GC.GetAllocatedBytesForCurrentThread()-bytes)+" ms="+timer.Elapsed.TotalMilliseconds.ToString("F2")+" managed-fixture-not-game-FPS");
  }
  var army=new Wa_ArmyCtrl();Check((bool)Call("BeforeArmyNavigationDrawing",army),"valid empty path drawing remains native");
  army.m_listDestination.Add(new(5));Check((bool)Call("BeforeArmyNavigationDrawing",army),"valid moving path drawing remains native");
  army.GFJGCKCEIHC=null;Check(!(bool)Call("BeforeArmyNavigationDrawing",army)&&army.m_listDestination.Count==1,"missing path renderer defers drawing without erasing movement");
  army.GFJGCKCEIHC=new();army.m_attribute=null;Check(!(bool)Call("BeforeArmyNavigationDrawing",army),"uninitialized attributes cannot crash path draw");
  army.m_attribute=new();army.m_navAgent=null;Check(!(bool)Call("BeforeArmyNavigationDrawing",army),"missing navigation component defers draw");
  army.m_navAgent=new();army.NJEMJKJHHBE=null;Check(!(bool)Call("BeforeArmyNavigationDrawing",army),"missing draw scratch list defers draw");
  army.NJEMJKJHHBE=new();Check((bool)Call("BeforeArmyNavigationDrawing",army),"drawing resumes as soon as resources are ready");
  army.m_listDestination=null;Check(!(bool)Call("BeforeArmyNavigationDrawing",army),"disposed destination list cannot crash empty-path branch");
  Console.WriteLine("PASS Perf102 "+n+" exact route stamp and navigation lifecycle regressions; native resources mocked.");
 }
}
