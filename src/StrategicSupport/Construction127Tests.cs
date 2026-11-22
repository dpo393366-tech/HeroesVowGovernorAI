using System;using System.Linq;using System.Collections.Generic;using System.Reflection;using BepInEx.Configuration;
public static class Construction127Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Construction127: "+why);}
 static (AFAGFHJONFJ,MKMOJNAJKJE,JNHOOCJJPJO) Setup(){
  typeof(MaintenanceTests).GetMethod("Setup",F).Invoke(null,null);
  var c=(AFAGFHJONFJ)typeof(MaintenanceTests).GetField("c",F).GetValue(null);
  var f=(MKMOJNAJKJE)typeof(MaintenanceTests).GetField("f",F).GetValue(null);var d=c.FGBLEMGGOPO[0];
  var b=new PABIPDOFBFG{OJPECMNINNA="max-building",CNCPIILGDEF=100};AEHAGJAKNEK.ACIGFKMDOKE[b.OJPECMNINNA]=b;
  for(int i=0;i<240;i++)d.AIMLPNBOIIF.Add(new(){OJPECMNINNA=127000+i,GMIKIMACDBC=d,GEKKLMPLDHJ=b,OGFAHJHBCNG=1000});return(c,f,d);
 }
 static (AFAGFHJONFJ,MKMOJNAJKJE,JNHOOCJJPJO) UpgradeSetup(){
  var (c,f,d)=Setup();d.AIMLPNBOIIF.Clear();
  var b=AEHAGJAKNEK.ACIGFKMDOKE["max-building"];
  AEHAGJAKNEK.ACIGFKMDOKE["advanced"]=new(){OJPECMNINNA="advanced",MDOIJIAONJM=b.OJPECMNINNA,CNCPIILGDEF=200,NPCHPDNFAME=5000};
  for(int i=0;i<9;i++)d.AIMLPNBOIIF.Add(new(){OJPECMNINNA=128000+i,GMIKIMACDBC=d,GEKKLMPLDHJ=b,OGFAHJHBCNG=1000});return(c,f,d);
 }
 static IEnumerable<bool> Maintenance(AFAGFHJONFJ c,MKMOJNAJKJE f)=>(IEnumerable<bool>)Call("ConstructionSlices",(IEnumerable<bool>)Call("MaintenanceSteps",c,f),10000d,256);
 public static void Run(){
  var (c,f,d)=Setup();long cash=(long)c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO;
  var original=((IEnumerable<bool>)Call("MaintenanceSteps",c,f)).ToList();
  Check(!original.Any(x=>x)&&original.Count>=480,"dense city scans all complete buildings without unnecessary purchases");
  (c,f,d)=Setup();var packed=((IEnumerable<bool>)Call("ConstructionSlices",(IEnumerable<bool>)Call("MaintenanceSteps",c,f),10000d,256)).ToList();
  Check(!packed.Any(x=>x)&&packed.Count<original.Count/20,"same complete maintenance inspection uses fewer queue turns");
  Check(d.AIMLPNBOIIF.Count==240&&cash==(long)c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO,"batching neither removes buildings nor changes cash");
  (c,f,d)=Setup();d.AIMLPNBOIIF[239].OGFAHJHBCNG=400;
  using(var steps=((IEnumerable<bool>)Call("ConstructionSlices",(IEnumerable<bool>)Call("MaintenanceSteps",c,f),10000d,256)).GetEnumerator()){
   Check(steps.MoveNext()&&steps.Current,"commit ends batch immediately even after many cheap probes");
   Check(d.AIMLPNBOIIF[239].OGFAHJHBCNG==650,"native repair amount retained");
  }
  (c,f,d)=Setup();
  using(var steps=((IEnumerable<bool>)Call("ConstructionSlices",(IEnumerable<bool>)Call("MaintenanceSteps",c,f),10000d,16)).GetEnumerator()){
   Check(steps.MoveNext()&&!steps.Current,"read-only batch yields at hard step limit");
   d.AIMLPNBOIIF[239].OGFAHJHBCNG=400;c.Siege=true;while(steps.MoveNext())Check(!steps.Current,"siege during queue wait prevents spending");
  }
  Check(d.AIMLPNBOIIF[239].OGFAHJHBCNG==400,"ownership/siege recheck retained after batched wait");
  new StrategicSupport().Load();Check(((ConfigEntry<int>)typeof(StrategicSupport).GetField("buildsPerCity",F).GetValue(null)).Value==8,"fresh configuration permits eight separately budgeted purchases");
  (c,f,d)=UpgradeSetup();cash=(long)c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO;
  Check(Maintenance(c,f).Count(x=>x)==8,"nine affordable buildings start exactly eight upgrades per visit");
  Check(d.AIMLPNBOIIF.Count(x=>x.GEKKLMPLDHJ.OJPECMNINNA=="advanced")==8&&d.AIMLPNBOIIF.Count(x=>x.EHABIIDOKGC())==8,"eight distinct upgrades preserve native construction time");
  Check(cash-(long)c.LMPMNLLGOEC.DNOGPFHMOMO-f.DNOGPFHMOMO==40000,"eight upgrades paid exactly once each");
  Check(!Maintenance(c,f).Any(),"same maintenance gate prevents duplicate batch");
  (c,f,d)=UpgradeSetup();c.LMPMNLLGOEC.DNOGPFHMOMO=20000;f.DNOGPFHMOMO=0;
  Check(Maintenance(c,f).Count(x=>x)==2&&c.LMPMNLLGOEC.DNOGPFHMOMO==10000,"batch stops before spending the local reserve");
  (c,f,d)=UpgradeSetup();
  using(var steps=Maintenance(c,f).GetEnumerator()){
   while(steps.MoveNext()&&!steps.Current){}
   Check(d.AIMLPNBOIIF.Count(x=>x.EHABIIDOKGC())==1,"first purchase yields before second upgrade");
   c.Siege=true;while(steps.MoveNext())Check(!steps.Current,"new siege cancels remaining upgrades");
  }
  Check(d.AIMLPNBOIIF.Count(x=>x.EHABIIDOKGC())==1,"siege after first upgrade preserves all remaining buildings");
  Console.WriteLine($"PASS Construction127 {checks} production maintenance batching checks; dense city queue yields {original.Count}->{packed.Count}, no game-time acceleration. Native actions mocked.");
 }
}
