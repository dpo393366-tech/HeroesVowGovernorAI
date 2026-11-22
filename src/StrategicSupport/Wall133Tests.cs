using System;using System.Linq;using System.Reflection;using System.Collections.Generic;using UnityEngine;
public static class Wall133Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Wall133: "+why);}
 static (AFAGFHJONFJ,MKMOJNAJKJE,JNHOOCJJPJO,PABIPDOFBFG) Setup(){
  typeof(CityIntegrationTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");
  var c=(AFAGFHJONFJ)typeof(CityIntegrationTests).GetField("c",F).GetValue(null);
  var f=(MKMOJNAJKJE)typeof(CityIntegrationTests).GetField("f",F).GetValue(null);
  c.FCIJDABCHEN=new(-160,0,-210);c.FGBLEMGGOPO.Clear();
  var d=new JNHOOCJJPJO{OJPECMNINNA="wall",AJJCCPCJPFC=c,PLIFMOIBPME=true,FAFNFFJAFKH=new()};c.FGBLEMGGOPO.Add(d);
  var row=new Il2CppSystem.Collections.Generic.List<FEEDONDCELP>();d.FAFNFFJAFKH.Add(row);
  // Captured Sangdang positions: serial socket indices are not ground tiles.
  var positions=new[]{new Vector3(-70,8.7f,206),new Vector3(156,8.7f,140),new Vector3(-156,8.7f,-140),new Vector3(-90,8.7f,-206)};
  for(int i=0;i<positions.Length;i++)row.Add(new(){GMIKIMACDBC=d,HFBAJIGHOAF=1,PMGDGAAICCG=new[]{4,1,3,2}[i],PAHKDHJPFKD=i+1,BJHLIMBCBNN=positions[i]});
  var b=new PABIPDOFBFG{OJPECMNINNA="JZ40009",HFBAJIGHOAF=MHCEFEODENB.Wall,MLAJLJKFGBD=1,PNPLBNAFMCM=1,NPCHPDNFAME=500,DONIEGIDACP=20};
  AEHAGJAKNEK.ACIGFKMDOKE.Clear();AEHAGJAKNEK.ACIGFKMDOKE[b.OJPECMNINNA]=b;return(c,f,d,b);
 }
 static void Repair(AFAGFHJONFJ c,MKMOJNAJKJE f){foreach(var _ in (IEnumerable<bool>)Call("RepairWallRegistrations",c,f)){} }
 public static void Run(){
  var(c,f,d,b)=Setup();long cash=(long)c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO;
  for(int i=0;i<4;i++)Check((bool)Call("BuildBest",c,f),"four real-coordinate wall sockets construct");
  Check(d.GroundWallCalls==0&&d.WallCalls==4,"wall construction never uses ground parcel registration");
  Check(cash-c.LMPMNLLGOEC.DNOGPFHMOMO-f.DNOGPFHMOMO==2000,"normal native recipe charged once");
  foreach(var q in d.FAFNFFJAFKH[0]){
   var x=d.AIMLPNBOIIF.Single(t=>t.OJPECMNINNA==q.FCOJFBEHMAG);
   Check((x.BJHLIMBCBNN-(q.BJHLIMBCBNN-c.FCIJDABCHEN-d.BJHLIMBCBNN)).sqrMagnitude<.0001,"wall position includes height and city/district origins");
   Check(c.NBEJCGLJHJA.Contains(x)&&x.JOFCACIPJKK==q.PMGDGAAICCG,"native wall work list and socket side both linked");
   Check(x.EMHNNCLBADE==0&&x.BNMEOOJPOML==20,"native construction duration retained, not instant-completed");
  }
  for(int side=1;side<=4;side++)c.DAEIKODOFGJ(5,side);
  Check(d.AIMLPNBOIIF.All(x=>x.EMHNNCLBADE==5),"side-directed native work contract advances all four properly linked projects");
  Check(!(bool)Call("BuildBest",c,f),"all sockets occupied prevents repeated placement");
  var ids=d.AIMLPNBOIIF.Select(x=>x.OJPECMNINNA).ToArray();Repair(c,f);
  Check(ids.SequenceEqual(d.AIMLPNBOIIF.Select(x=>x.OJPECMNINNA)),"valid zero-percent projects are preserved");
  // A lying native registrar that adds a row but not the wall work is rejected.
  (c,f,d,b)=Setup();d.FailWallLink=true;cash=(long)c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO;
  try{Call("BuildBest",c,f);}catch(TargetInvocationException){}
  Check(d.AIMLPNBOIIF.Count==0&&c.NBEJCGLJHJA.Count==0,"failed socket linking rolls back registered row");
  Check(cash==(long)c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO,"failed wall registration refunds purchase");
  (c,f,d,b)=Setup();var bad=new OGABIJECHFG{OJPECMNINNA=24128,GEKKLMPLDHJ=b,GMIKIMACDBC=d,BJHLIMBCBNN=new(30,0,10),EMHNNCLBADE=0,BNMEOOJPOML=20};d.AIMLPNBOIIF.Add(bad);
  var duplicate=new OGABIJECHFG{OJPECMNINNA=24229,GEKKLMPLDHJ=b,GMIKIMACDBC=d,BJHLIMBCBNN=new(30,0,10),EMHNNCLBADE=0,BNMEOOJPOML=20};d.AIMLPNBOIIF.Add(duplicate);
  var completed=new OGABIJECHFG{OJPECMNINNA=21350,GEKKLMPLDHJ=b,GMIKIMACDBC=d,BJHLIMBCBNN=new(30,0,10),EMHNNCLBADE=20,BNMEOOJPOML=20};d.AIMLPNBOIIF.Add(completed);
  var groundProgress=new OGABIJECHFG{OJPECMNINNA=25000,GEKKLMPLDHJ=b,GMIKIMACDBC=d,BJHLIMBCBNN=new(50,0,10),EMHNNCLBADE=3,BNMEOOJPOML=20};d.AIMLPNBOIIF.Add(groundProgress);
  c.NBEJCGLJHJA.Add(bad);for(int side=1;side<=4;side++)c.DAEIKODOFGJ(5,side);
  Check(bad.EMHNNCLBADE==0,"legacy row remains at zero despite being listed because wall side was never assigned");
  int local=c.LMPMNLLGOEC.DNOGPFHMOMO;Repair(c,f);
  Check(!d.AIMLPNBOIIF.Contains(bad)&&!d.AIMLPNBOIIF.Contains(duplicate),"captured duplicate ground-grid zero-progress signature removed");
  Check(d.AIMLPNBOIIF.Contains(completed)&&d.AIMLPNBOIIF.Contains(groundProgress),"completed and progressing buildings preserved even at ground coordinates");
  Check(c.LMPMNLLGOEC.DNOGPFHMOMO==local+1000,"two malformed starts refund exactly two original gold-only recipes");Repair(c,f);
  Check(c.LMPMNLLGOEC.DNOGPFHMOMO==local+1000,"repeat repair cannot duplicate refund");
  Check((bool)Call("BuildBest",c,f),"normal wall construction resumes after malformed entries are removed");
  foreach(string protection in new[]{"private","material","points"}){
   (c,f,d,b)=Setup();if(protection=="private")b.OCJNCECECMH=FFDDNPOHLHN.Private;
   if(protection=="material")b.JADLIKBKOHG="mod-ore";if(protection=="points")b.ODKKMLDAPJL=20;
   d.AIMLPNBOIIF.Add(new(){OJPECMNINNA=24128,GEKKLMPLDHJ=b,GMIKIMACDBC=d,BJHLIMBCBNN=new(30,0,10),EMHNNCLBADE=0,BNMEOOJPOML=20});
   local=c.LMPMNLLGOEC.DNOGPFHMOMO;Repair(c,f);
   Check(d.AIMLPNBOIIF.Count==1&&c.LMPMNLLGOEC.DNOGPFHMOMO==local,"repair refuses unverified refund/ownership case: "+protection);
  }
  (c,f,d,b)=Setup();d.AIMLPNBOIIF.Add(new(){OJPECMNINNA=24128,GEKKLMPLDHJ=b,GMIKIMACDBC=d,BJHLIMBCBNN=new(30,0,10),EMHNNCLBADE=0,BNMEOOJPOML=20});
  using(var repair=((IEnumerable<bool>)Call("RepairWallRegistrations",c,f)).GetEnumerator()){
   Check(repair.MoveNext()&&!repair.Current,"repair yields before mutation");c.LMPMNLLGOEC.LMMNKIOAGLC="captured";while(repair.MoveNext()){}
  }Check(d.AIMLPNBOIIF.Count==1,"ownership change cancels pending repair");
  Console.WriteLine($"PASS Wall133 {checks} real-coordinate registration, native wall-list verification, rollback and legacy duplicate recovery checks; native execution mocked, not gameplay.");
 }
}
