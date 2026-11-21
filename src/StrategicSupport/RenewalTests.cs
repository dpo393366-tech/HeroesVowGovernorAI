using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
public static class RenewalTests {
 static int count;static AFAGFHJONFJ c;static MKMOJNAJKJE f;static JNHOOCJJPJO d;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,a);
 static void Check(bool b,string n){count++;if(!b)throw new Exception(n);}
 static void Setup(){typeof(MaintenanceTests).GetMethod("Setup",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,null);
  object Field(string n)=>typeof(MaintenanceTests).GetField(n,BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
  c=(AFAGFHJONFJ)Field("c");f=(MKMOJNAJKJE)Field("f");d=(JNHOOCJJPJO)Field("d");}
 static PABIPDOFBFG Def(string id,int income=100){var b=new PABIPDOFBFG{OJPECMNINNA=id,CNCPIILGDEF=income,FHKHHKBBKGM=true};AEHAGJAKNEK.ACIGFKMDOKE[id]=b;return b;}
 static OGABIJECHFG Instance(PABIPDOFBFG b){var x=new OGABIJECHFG{GEKKLMPLDHJ=b,GMIKIMACDBC=d};d.AIMLPNBOIIF.Add(x);return x;}
 static bool Run(){((IDictionary)typeof(StrategicSupport).GetField("next",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null)).Clear();bool result=false;foreach(bool r in (IEnumerable<bool>)Call("RenewalSteps",c,f))result|=r;return result;}
 static PABIPDOFBFG Goal(){var b=Def("JZ30004",0);b.HFBAJIGHOAF=MHCEFEODENB.Civil;b.DPFIFIOKHEI=2;c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE=b.OJPECMNINNA,KLCDGPDHENB=1}}});return b;}
 static (OGABIJECHFG,OGABIJECHFG,PABIPDOFBFG) FullGrid(){Setup();var a=Instance(Def("small-a",10));var b=Instance(Def("small-b",10));Instance(Def("protected-economy",200)).JOFCACIPJKK=1;
  a.GEKKLMPLDHJ.MLAJLJKFGBD=a.GEKKLMPLDHJ.PNPLBNAFMCM=b.GEKKLMPLDHJ.MLAJLJKFGBD=b.GEKKLMPLDHJ.PNPLBNAFMCM=1;a.OJPECMNINNA=1;b.OJPECMNINNA=2;
  d.FAFNFFJAFKH=new(){new(){new(){FCOJFBEHMAG=1,ICGFBPCAJCI=true,GMIKIMACDBC=d},new(){FCOJFBEHMAG=2,ICGFBPCAJCI=true,GMIKIMACDBC=d}}};a.Cells=new[]{d.FAFNFFJAFKH[0][0]};b.Cells=new[]{d.FAFNFFJAFKH[0][1]};d.GHECBGMGBBK=2;d.NDOFLDJGDOM=1;
  var goal=Goal();goal.PNPLBNAFMCM=1;return(a,b,goal);
 }
 public static void RunAll(){
  Setup();var low=Instance(Def("low",20));var high=Instance(Def("high",200));var goal=Goal();goal.TechAllowed=false;
  Check(!Run()&&low.GEKKLMPLDHJ.OJPECMNINNA=="low","locked goal must never remove a building");
  goal.TechAllowed=true;NEINKOOBJIH.JKGJNLHLJAJ=c;int cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
  Check(Run()&&low.GEKKLMPLDHJ==goal&&high.GEKKLMPLDHJ.OJPECMNINNA=="high","loaded full city replaces lowest-value compatible building");
  Check(low.EHABIIDOKGC()&&d.AIMLPNBOIIF.Count==2&&cash-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==goal.NPCHPDNFAME,"native construction time, occupied site and exact cost retained");
  Check(!Run(),"registered goal not duplicated while under construction");
  Setup();low=Instance(Def("low",20));high=Instance(Def("high",200));goal=Goal();d.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=d});Check(!Run(),"empty plot preferred over replacement");
  Setup();low=Instance(Def("low",20));high=Instance(Def("high",200));goal=Goal();goal.PNPLBNAFMCM=1;Check(!Run(),"different footprint cannot use same-parcel replacement");
  Setup();low=Instance(Def("low",20));high=Instance(Def("high",200));goal=Goal();low.JOFCACIPJKK=1;high.JOFCACIPJKK=1;Check(!Run(),"occupied facilities protected");
  Setup();low=Instance(Def("low",20));high=Instance(Def("high",200));goal=Goal();f.DNOGPFHMOMO=10000;c.LMPMNLLGOEC.DNOGPFHMOMO=10000;Check(!Run(),"reserve cannot fund destructive replacement");
  Setup();low=Instance(Def("low",20));high=Instance(Def("high",200));goal=Goal();low.ThrowBeforeUpgrade=true;cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;try{Run();}catch{}Check(low.GEKKLMPLDHJ.OJPECMNINNA=="low"&&cash==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO,"precommit failure retains original and refunds");
  low.ThrowBeforeUpgrade=false;low.ThrowAfterUpgrade=true;try{Run();}catch{}Check(low.GEKKLMPLDHJ==goal&&cash-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==goal.NPCHPDNFAME,"postcommit scene error never creates free building");
  Setup();low=Instance(Def("low",20));high=Instance(Def("high",200));goal=Goal();c.HFLAGGEFBOO.Add(new(){Complete=true,CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE="low",KLCDGPDHENB=1}}});Check(!(bool)Call("RenewalVictim",c,d,low,goal),"achieved count protected");
  Instance(low.GEKKLMPLDHJ);Check((bool)Call("RenewalVictim",c,d,low,goal),"excess duplicate above goal can be replaced");
  Setup();low=Instance(Def("low",20));var better=Def("better",250);Check(Run()&&low.GEKKLMPLDHJ==better,"unlocked more productive building replaces low-output predecessor");
  Setup();var tower=Def("JZ40009",0);tower.HFBAJIGHOAF=MHCEFEODENB.Wall;Check((bool)Call("CanBuild",c,f,tower,10000),"wall weapon admitted to legal placement candidates");Check((int)Call("DefenseShortfall",c,f,tower)==1,"peaceful interior still fills legal wall sockets");
  var border=new AFAGFHJONFJ();border.LMPMNLLGOEC.LMMNKIOAGLC="enemy";c.FKMKGNPFLFP.Add(border);f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};
  Check((int)Call("DefenseShortfall",c,f,tower)==2,"one hostile border requests two towers");
  var pending=Instance(tower);pending.EMHNNCLBADE=0;Check((int)Call("DefenseShortfall",c,f,tower)==2,"registered towers do not impose artificial quota");
  var stone=Def("JZ40008",0);stone.HFBAJIGHOAF=MHCEFEODENB.Wall;Check((int)Call("DefenseShortfall",c,f,stone)==2,"hostile border raises stone thrower urgency");
  f.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.StopWar;Check((int)Call("DefenseShortfall",c,f,tower)==1,"ceasefire removes urgency but permits wall completion");
  var fixture=FullGrid();cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
  var plans=((IEnumerable)Call("RebuildPlans",c,d,fixture.Item3,false)).Cast<object>().Where(x=>x!=null).ToList();Check(plans.Count>0&&d.FAFNFFJAFKH[0][0].FCOJFBEHMAG==1&&d.AIMLPNBOIIF.Count==3,"layout planning never changes live occupancy or buildings");
  Check(Run()&&d.AIMLPNBOIIF.Count==2&&d.AIMLPNBOIIF.Exists(x=>x.GEKKLMPLDHJ==fixture.Item3),"two low-value one-tile buildings become native two-tile goal construction");
  Check(cash-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==fixture.Item3.NPCHPDNFAME,"rebuild pays exact cost with no demolition refund");
  fixture=FullGrid();d.NoRegister=true;cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;try{Run();}catch{}Check(d.AIMLPNBOIIF.Contains(fixture.Item1)&&d.AIMLPNBOIIF.Contains(fixture.Item2)&&d.FAFNFFJAFKH[0][0].FCOJFBEHMAG==1&&d.FAFNFFJAFKH[0][1].FCOJFBEHMAG==2&&cash==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO,"failed native registration restores both originals, occupancy and money");
  fixture=FullGrid();d.ThrowAfterRegister=true;cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;Check(Run()&&d.AIMLPNBOIIF.Exists(x=>x.GEKKLMPLDHJ==fixture.Item3)&&cash-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==fixture.Item3.NPCHPDNFAME,"post-registration scene failure retains valid construction and payment");
  fixture=FullGrid();foreach(var q in d.FAFNFFJAFKH[0])q.ICGFBPCAJCI=false;Check(!Run()&&d.AIMLPNBOIIF.Count==3,"no legal street parcel means no demolition");
  fixture=FullGrid();fixture.Item2.JOFCACIPJKK=1;Check(!Run()&&d.AIMLPNBOIIF.Count==3,"one protected occupant prevents joined parcel demolition");
  fixture=FullGrid();plans=((IEnumerable)Call("RebuildPlans",c,d,fixture.Item3,false)).Cast<object>().Where(x=>x!=null).ToList();d.FAFNFFJAFKH[0][1].FCOJFBEHMAG=999;Check(!(bool)Call("RebuildParcelCurrent",plans[0]),"new parcel occupant invalidates plan before demolition");
  fixture=FullGrid();c.HFLAGGEFBOO.Add(new(){GoalCheck=city=>city.ELKFAJKBPKJ("small-a",false,true).Count>0});cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;try{Run();}catch{}Check(d.AIMLPNBOIIF.Contains(fixture.Item1)&&d.AIMLPNBOIIF.Contains(fixture.Item2)&&!d.AIMLPNBOIIF.Exists(x=>x.GEKKLMPLDHJ==fixture.Item3)&&cash==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO,"native goal invalidated after registration restores old buildings and payment");
  Setup();low=Instance(Def("low",20));Instance(Def("high",200));goal=Goal();c.MoneyIncome=10000;c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.CityMoneyIncome,KLCDGPDHENB=500}}});Check((bool)Call("RenewalVictim",c,d,low,goal),"large income surplus does not unconditionally protect weakest market");c.MoneyIncome=500;Check(!(bool)Call("RenewalVictim",c,d,low,goal),"income at goal threshold protects needed production");
  Console.WriteLine($"PASS {count} renewal checks; managed native-call stubs, not gameplay");
 }
}
