using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
public static class IntegratedTests {
 static int count;static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static void Check(bool b,string s){count++;if(!b)throw new Exception("Integrated: "+s);}
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
 static void Setup(Type t)=>t.GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);
 static void Clear()=>Field<IDictionary>(typeof(StrategicSupport),"next").Clear();
 public static void Run(){
  Setup(typeof(MaintenanceTests));var c=Field<AFAGFHJONFJ>(typeof(MaintenanceTests),"c");var f=Field<MKMOJNAJKJE>(typeof(MaintenanceTests),"f");var d=Field<JNHOOCJJPJO>(typeof(MaintenanceTests),"d");
  var old=new PABIPDOFBFG{OJPECMNINNA="base"};var up=new PABIPDOFBFG{OJPECMNINNA="material-upgrade",MDOIJIAONJM="base",JADLIKBKOHG="ore",GJPOJINPCHP=3,CNCPIILGDEF=200,NPCHPDNFAME=3000};AEHAGJAKNEK.ACIGFKMDOKE[old.OJPECMNINNA]=old;AEHAGJAKNEK.ACIGFKMDOKE[up.OJPECMNINNA]=up;
  var x=new OGABIJECHFG{GEKKLMPLDHJ=old,GMIKIMACDBC=d};d.AIMLPNBOIIF.Add(x);
  Check(!(bool)Call("CanUpgrade",c,f,x,up),"material upgrade requires real stock");
  var ore1=new CMHNADEDJIE{OJPECMNINNA="ore",NGBNIPKFPOE=2};var ore2=new CMHNADEDJIE{OJPECMNINNA="ore",NGBNIPKFPOE=2};f.CNBLOJCGNFD.Add(ore1);f.EHGOOJLIHGH.Add(ore2);
  Check((bool)Call("CanUpgrade",c,f,x,up),"stock across stacks permits native upgrade");
  int cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
  try{Call("SpendBuilding",c,f,up,(Action)(()=>throw new Exception("before mutation")),(Func<bool>)(()=>false));}catch(TargetInvocationException){}
  Check(ore1.NGBNIPKFPOE==2&&ore2.NGBNIPKFPOE==2&&f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO==cash,"failed purchase restores material stacks and money");
  Check((bool)Call("SpendBuilding",c,f,up,(Action)(()=>x.GMMDMPFFIGP(up.OJPECMNINNA,false)),(Func<bool>)(()=>x.GEKKLMPLDHJ==up)),"native material upgrade registers");
  Check(!f.CNBLOJCGNFD.Contains(ore1)&&ore2.NGBNIPKFPOE==1&&cash-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==3000&&x.EHABIIDOKGC(),"exact material/cash cost, zero stack removed, no free completion");
  Setup(typeof(MaintenanceTests));c=Field<AFAGFHJONFJ>(typeof(MaintenanceTests),"c");f=Field<MKMOJNAJKJE>(typeof(MaintenanceTests),"f");d=Field<JNHOOCJJPJO>(typeof(MaintenanceTests),"d");
  var repair=new PABIPDOFBFG{OJPECMNINNA="repair"};x=new(){GEKKLMPLDHJ=repair,GMIKIMACDBC=d,OGFAHJHBCNG=1};d.AIMLPNBOIIF.Add(x);cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
  using(var it=((IEnumerable<bool>)Call("MaintenanceSteps",c,f)).GetEnumerator()){Check(it.MoveNext()&&!it.Current&&x.OGFAHJHBCNG==1,"maintenance yields before native mutation");c.LMPMNLLGOEC.LMMNKIOAGLC="captured";while(it.MoveNext()){} }
  Check(x.OGFAHJHBCNG==1&&cash==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO,"ownership change while yielding cannot spend old owner's funds");
  Setup(typeof(CityIntegrationTests));c=Field<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");f=Field<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");d=Field<JNHOOCJJPJO>(typeof(CityIntegrationTests),"d");
  var goal=new PABIPDOFBFG{OJPECMNINNA="future-relief",TechAllowed=false,HFBAJIGHOAF=MHCEFEODENB.Civil,CNCPIILGDEF=0};AEHAGJAKNEK.ACIGFKMDOKE[goal.OJPECMNINNA]=goal;c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE=goal.OJPECMNINNA,KLCDGPDHENB=1}}});
  var sites=Call("FutureGoalSites",c,f,Call("ReadGoals",c),false);Check(!(bool)Call("ConsumesFutureSite",d.HGNFEMJOPPB[0],new PABIPDOFBFG{OJPECMNINNA="market"},sites),"locked future goal does not permanently leave usable land empty");
  Check(!(bool)Call("ConsumesFutureSite",d.HGNFEMJOPPB[0],goal,sites),"goal itself can use its reservation");
  Setup(typeof(OffensiveTests));c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");f=Field<MKMOJNAJKJE>(typeof(OffensiveTests),"f");var target=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"target");
  target.LMPMNLLGOEC.LMMNKIOAGLC="";f.DNOGPFHMOMO=50000;foreach(var step in (IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")){}
  Check(c.Last!=null&&!c.Last.Dismissed,"50k treasury can mobilize a cheap safe neutral occupation");
  Check(c.Last.GetSoldierNum(false)<=12000,"safe interior keeps at least 40 percent soldiers");
  Field<IDictionary>(typeof(StrategicSupport),"attackOrders").Clear();f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;
  Check(!(bool)Call("GuardAttackStart",c.Last)&&c.Last.Dismissed,"untracked vanilla or loaded-save army respects StopWar");
  Setup(typeof(OffensiveTests));c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");f=Field<MKMOJNAJKJE>(typeof(OffensiveTests),"f");foreach(var step in (IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")){}Field<IDictionary>(typeof(StrategicSupport),"attackOrders").Clear();c.Last.m_responseToPlayer=true;f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Check((bool)Call("GuardAttackStart",c.Last),"explicit player response remains under player control");
  Setup(typeof(OfficerBalanceTests));var make=typeof(OfficerBalanceTests).GetMethod("City",BindingFlags.NonPublic|BindingFlags.Static);var a=(AFAGFHJONFJ)make.Invoke(null,new object[]{"exchange-a",8,90d});var b=(AFAGFHJONFJ)make.Invoke(null,new object[]{"exchange-b",8,20d});
  Check((bool)Call("TryOfficerExchange",a),"equal counts allow paired native exchange");
  Check(a.Last.m_targetCity==b.OJPECMNINNA&&b.Last.m_targetCity==a.OJPECMNINNA&&a.Roster.Count==8&&b.Roster.Count==8,"both armies target reciprocal cities without teleporting rosters");
  Setup(typeof(OfficerBalanceTests));a=(AFAGFHJONFJ)make.Invoke(null,new object[]{"exchange-a",8,90d});b=(AFAGFHJONFJ)make.Invoke(null,new object[]{"exchange-b",8,20d});b.ThrowGeneration=true;
  try{Call("TryOfficerExchange",a);}catch(TargetInvocationException){}Check(a.Last!=null&&a.Last.Dismissed&&a.Roster.Count==8&&b.Roster.Count==8,"second native generation failure cancels first exchange army");
  var high=Enumerable.Range(0,8).Select(i=>new OfficerBalancePolicy.Officer("h"+i,90d,90d)).ToList();var low=Enumerable.Range(0,8).Select(i=>new OfficerBalancePolicy.Officer("l"+i,20d,20d)).ToList();
  for(int i=0;i<4;i++){var h=high.First(v=>v.Ability[0]==90);var l=low.First(v=>v.Ability[0]==20);Check(OfficerBalancePolicy.ExchangeGain(high,low,h,l)>0,"successive equal-count swaps improve means");high.Remove(h);low.Remove(l);high.Add(l);low.Add(h);}
  Check(high.Average(v=>v.Ability[0])==55&&low.Average(v=>v.Ability[0])==55,"four paired exchanges equalize 90/20 means without changing counts");
  var future=Enumerable.Range(0,6).Select(i=>new DevelopmentPolicy.Action{Id="p"+i,Key="p"+i,District="block",Cost=100,Priority=1,Limit=1,BaseValue=100,Money=100,MoneyBonus=.1,Tiles=new[]{(long)i}}).ToList();
  List<int> planned=null;int slices=0;foreach(var step in DevelopmentPolicy.PlanSteps(future,new(),1000,1000,1000,result=>planned=result))slices++;
  Check(planned.Count==6&&slices>1,"six-building synergy planned across multiple bounded slices");
  Check(planned.SequenceEqual(DevelopmentPolicy.Plan(future,new(),1000,1000,1000)),"incremental plan matches synchronous reference for same inputs");
  Setup(typeof(PlanningTests));c=Field<AFAGFHJONFJ>(typeof(PlanningTests),"c");var heroes=new List<CLAGKDECFID>();for(int i=0;i<20;i++)heroes.Add(new(){OJPECMNINNA="specialist"+i,LMPMNLLGOEC=new(){CNOMNBCPJDI=c.OJPECMNINNA,LMMNKIOAGLC="F"}});heroes[0].Ability[8]=100;heroes[0].Ability[9]=100;heroes[1].Ability[0]=200;heroes[1].Ability[1]=200;c.Roster=heroes;
  var keep=(HashSet<string>)Call("ProtectedCivilOfficers",c);Check(keep.Contains("specialist0")&&keep.Contains("specialist1"),"research retains actual command and civil specialists, not arbitrary remaining names");
  Setup(typeof(CityIntegrationTests));c=Field<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");d=Field<JNHOOCJJPJO>(typeof(CityIntegrationTests),"d");var plot=d.HGNFEMJOPPB[0];var footprint=(long[])Call("Footprint",plot);Check((bool)Call("CurrentPlot",c,d,plot,footprint),"original vacant parcel accepted");plot.FAFNFFJAFKH[0].FCOJFBEHMAG=99;Check(!(bool)Call("CurrentPlot",c,d,plot,footprint),"another building occupying planned site prevents overwrite");
  Setup(typeof(OffensiveTests));c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");f=Field<MKMOJNAJKJE>(typeof(OffensiveTests),"f");foreach(var step in (IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")){}foreach(var hero in c.Roster.Where(h=>!c.Last.m_listHero.Contains(h)))hero.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;Check(!(bool)Call("GuardAttackStart",c.Last)&&c.Last.Dismissed,"research workers cannot masquerade as idle home commanders");
  Console.WriteLine($"PASS {count} integrated regressions; native calls mocked, no gameplay/FPS claim");
 }
}
