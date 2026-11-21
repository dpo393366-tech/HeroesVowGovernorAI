using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
public static class City77Tests {
 static int checks;static AFAGFHJONFJ c;static MKMOJNAJKJE f;static JNHOOCJJPJO d;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
 static void Setup(){typeof(MaintenanceTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);c=Field<AFAGFHJONFJ>(typeof(MaintenanceTests),"c");f=Field<MKMOJNAJKJE>(typeof(MaintenanceTests),"f");d=Field<JNHOOCJJPJO>(typeof(MaintenanceTests),"d");}
 static void Check(bool ok,string label){if(!ok)throw new Exception("R77: "+label);checks++;}
 public static void Run(){
  Setup();var basic=new PABIPDOFBFG{OJPECMNINNA="required"};var up=new PABIPDOFBFG{OJPECMNINNA="high",MDOIJIAONJM="required",CNCPIILGDEF=200};
  AEHAGJAKNEK.ACIGFKMDOKE[basic.OJPECMNINNA]=basic;AEHAGJAKNEK.ACIGFKMDOKE[up.OJPECMNINNA]=up;
  var x=new OGABIJECHFG{GEKKLMPLDHJ=basic,GMIKIMACDBC=d};d.AIMLPNBOIIF.Add(x);c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE="required",KLCDGPDHENB=1}}});
  Check((bool)Call("CanUpgrade",c,f,x,up),"last goal building can become its direct upgrade");
  x.GMMDMPFFIGP("high",false);Check(c.ELKFAJKBPKJ("required",false,true).Count==0&&c.ELKFAJKBPKJ("required",true,true).Count==1,"native argument contract distinguishes construction and includes direct upgrade");
  var goals=Call("ReadGoals",c);var missing=(IDictionary)goals.GetType().GetField("Buildings").GetValue(goals);
  Check(!missing.Contains("required"),"pending direct upgrade is not duplicated as a missing base building");
  x.EMHNNCLBADE=x.BNMEOOJPOML;Check(c.ELKFAJKBPKJ("required",false,true).Count==1,"completed upgraded building counts toward native base goal");
  Check(!(bool)Call("UpgradePreservesRequiredCount",c,up,new PABIPDOFBFG{OJPECMNINNA="unrelated"}),"second-tier nonmatching replacement cannot erase original base goal");
  Setup();basic=new(){OJPECMNINNA="capacity-market",IKELHGAHBID=2};x=new(){GEKKLMPLDHJ=basic,GMIKIMACDBC=d};d.AIMLPNBOIIF.Add(x);
  var bigger=new PABIPDOFBFG{OJPECMNINNA="other-capacity",IKELHGAHBID=3,CNCPIILGDEF=200};
  Check(!(bool)Call("RenewalProtected",c,d,x,bigger),"equal or larger final capacity cannot justify zero-capacity reconstruction interval");
  bigger.MDOIJIAONJM=basic.OJPECMNINNA;AEHAGJAKNEK.ACIGFKMDOKE[basic.OJPECMNINNA]=basic;AEHAGJAKNEK.ACIGFKMDOKE[bigger.OJPECMNINNA]=bigger;
  Check((bool)Call("CanUpgrade",c,f,x,bigger),"direct native capacity upgrade remains available");
  bigger.IKELHGAHBID=1;Check(!(bool)Call("CanUpgrade",c,f,x,bigger),"automatic capacity downgrade blocked");bigger.IKELHGAHBID=3;
  for(int i=0;i<500;i++)AEHAGJAKNEK.ACIGFKMDOKE["noise"+i]=new(){OJPECMNINNA="noise"+i,MDOIJIAONJM="other"+i};
  int steps=0;foreach(var step in (IEnumerable<bool>)Call("MaintenanceSteps",c,f))steps++;
  Check(x.GEKKLMPLDHJ==bigger&&steps<50,"500 unrelated definitions do not impose 500 scheduler turns before real upgrade");
  var growth=new StatecraftPolicy.Needs{Growth=true};
  foreach(int kind in new[]{2,3,15,16,32,33,34,35})Check(StatecraftPolicy.Effect(kind,20,growth)>0,"wealthy city still values economic growth kind "+kind);
  Check(StatecraftPolicy.Effect(16,20,new(){Growth=true,Food=true})>StatecraftPolicy.Effect(16,20,growth),"food emergency outranks routine growth");
  Check(StatecraftPolicy.Effect(45,30,growth)>0,"recognized positive native effect can accompany economic benefits");
  Check(!StatecraftPolicy.Affordable(100,20,60,50,100),"growth never bypasses sustainable upkeep");
  int Frames(int max){var queue=new WorkQueue();IEnumerable<string> Job(){for(int k=0;k<100;k++)yield return "probe";}
   for(int k=0;k<136;k++)queue.Add("c"+k,()=>Job().GetEnumerator());int frames=0;while(queue.Count>0&&frames<2000){queue.Pump(10000,max,(_,__)=>{},e=>throw e);frames++;}return frames;}
  int oldFrames=Frames(8),newFrames=Frames(128);Check(newFrames*10<oldFrames,"tiny probes no longer stall behind eight-step cap");
  var q=new WorkQueue();IEnumerable<string> Tiny(){yield return "a";yield return "b";}q.Add("one",()=>Tiny().GetEnumerator());Check(q.Pump(0,128,(_,__)=>{},e=>throw e)==1,"time budget still terminates pump between steps");
  Console.WriteLine($"PASS R77 checks={checks}; tiny-probe fixture frames {oldFrames}->{newFrames}, not game FPS or measured live latency.");
 }
}
