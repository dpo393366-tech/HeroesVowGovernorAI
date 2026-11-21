using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
public static class Revision74Tests {
 static int checks;const BindingFlags Flags=BindingFlags.Static|BindingFlags.NonPublic;
 static object Call(string n,params object[] args)=>typeof(StrategicSupport).GetMethod(n,Flags).Invoke(null,args);
 static void Setup(Type type)=>type.GetMethod("Setup",Flags).Invoke(null,null);
 static T Get<T>(Type type,string n)=>(T)type.GetField(n,Flags).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("R74: "+why);}
 public static void Run(){
  Setup(typeof(StatecraftTests));var c=Get<AFAGFHJONFJ>(typeof(StatecraftTests),"c");var f=Get<MKMOJNAJKJE>(typeof(StatecraftTests),"f");
  var enemy=new AFAGFHJONFJ{OJPECMNINNA="border",LMPMNLLGOEC=new(){LMMNKIOAGLC="enemy"}};c.FKMKGNPFLFP.Add(enemy);f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};
  c.Security=c.Popular=100;CLAGKDECFID.MilitaryWorking=false;Call("AssignDomestic",c,f);
  Check(c.Roster.Count(x=>x.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Military)==1,"native accepted military task is counted even without working flag");
  var soldier=c.Roster.First(x=>x.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Military);
  Check(!(bool)Call("SafeStateOfficer",c,soldier)&&!(bool)Call("CivilResearchCandidate",c,soldier),"non-working assigned defender cannot be stolen for missions/research");
  Call("AssignDomestic",c,f);Check(c.Roster.Count(x=>x.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Military)==1,"next cycle does not assign another phantom vacancy");
  Check(c.Roster.Any(x=>x.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Manage),"military acceptance leaves room for management");
  soldier.BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Explore;CLAGKDECFID.RejectMilitary=true;
  int managed=c.Roster.Count(x=>x.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Manage);Call("AssignDomestic",c,f);
  Check(c.Roster.Count(x=>x.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Manage)>managed,"real rejected duty falls through to another useful duty");
  CLAGKDECFID.RejectMilitary=false;CLAGKDECFID.MilitaryWorking=true;
  Setup(typeof(StatecraftTests));c=Get<AFAGFHJONFJ>(typeof(StatecraftTests),"c");f=Get<MKMOJNAJKJE>(typeof(StatecraftTests),"f");f.PEGLDBENJMF=5000;f.Production=1000;
  for(int i=0;i<6;i++)f.Strategies.Add(new(){OJPECMNINNA="burst"+i,PMPLECHDMNM=i+20,IABANLBEFMG=0,AFPGDLJKEDA=10,CCBIHJLDGCG=10,MDPGDMKHDAG=new(){new(){HFBAJIGHOAF=(COGDLKNKJGA)12,HLNCKLGGECA=20}}});
  ((IDictionary)Get<object>(typeof(StrategicSupport),"next")).Clear();int yields=0;foreach(var stage in (IEnumerable<string>)Call("FactionStatecraftSteps",f))yields++;
  Check(f.MEAMPLFJPNH.Count==6&&f.PEGLDBENJMF==4940,"all six affordable distinct strategies with exact cost");Check(yields>=8,"strategy burst yields before and after each transaction");
  foreach(var stage in (IEnumerable<string>)Call("FactionStatecraftSteps",f)){}Check(f.MEAMPLFJPNH.Count==6,"burst never repeats already active strategies");
  var needs=new StatecraftPolicy.Needs{Front=true,Troops=10,Security=1};var mixed=new JFLCKHMFGPJ{MDPGDMKHDAG=new(){new(){HFBAJIGHOAF=(COGDLKNKJGA)5,HLNCKLGGECA=20},new(){HFBAJIGHOAF=(COGDLKNKJGA)11,HLNCKLGGECA=-5}}};
  object[] args={mixed,needs,null};Check((double)Call("ScoreStrategy",args)>0,"small research penalty can be outweighed by urgent training");
  mixed.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)999,HLNCKLGGECA=-1});args=new object[]{mixed,needs,null};Check((double)Call("ScoreStrategy",args)==0,"unknown effects never guessed");
  Setup(typeof(CityIntegrationTests));c=Get<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");f=Get<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");var d=Get<JNHOOCJJPJO>(typeof(CityIntegrationTests),"d");
  AEHAGJAKNEK.ACIGFKMDOKE["market"]=new(){OJPECMNINNA="market"};
  var stale=d.HGNFEMJOPPB[0];stale.FAFNFFJAFKH[0].FCOJFBEHMAG=42;
  d.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=d});Check((bool)Call("BuildBest",c,f),"occupied native-compatible parcel cannot starve alternative free plot");Check(stale.FAFNFFJAFKH[0].FCOJFBEHMAG==42,"never overwrite existing tile occupant");
  var shortPlot=new PEGMABENGBB{GMIKIMACDBC=d,PABEMCAGKJJ=1,HAPLIFADGBA=1,FAFNFFJAFKH=new FEEDONDCELP[]{new(),new(){FCOJFBEHMAG=99},null,null}};
  var footprint=(long[])Call("Footprint",shortPlot);Check(footprint.Length==1&&(bool)Call("CurrentPlot",c,d,shortPlot,footprint),"live pooled 1x1 array ignores occupied/null unused slots");
  shortPlot.HAPLIFADGBA=2;shortPlot.FAFNFFJAFKH[1]=new();footprint=(long[])Call("Footprint",shortPlot);Check(footprint.Length==2&&(bool)Call("CurrentPlot",c,d,shortPlot,footprint),"live pooled 1x2 array ignores trailing nulls");
  shortPlot.FAFNFFJAFKH[1].FCOJFBEHMAG=3;Check(!(bool)Call("CurrentPlot",c,d,shortPlot,footprint),"occupied active slot still blocks placement");
  var b=new PABIPDOFBFG{OJPECMNINNA="required-base"};var up=new PABIPDOFBFG{OJPECMNINNA="upgrade",MDOIJIAONJM=b.OJPECMNINNA,CNCPIILGDEF=200};AEHAGJAKNEK.ACIGFKMDOKE[b.OJPECMNINNA]=b;
  var building=new OGABIJECHFG{GEKKLMPLDHJ=b,GMIKIMACDBC=d};d.AIMLPNBOIIF.Add(building);c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE=b.OJPECMNINNA,KLCDGPDHENB=1}}});
  Check((bool)Call("CanUpgrade",c,f,building,up),"native direct upgrade preserves base goal after completion");
  d.AIMLPNBOIIF.Add(new(){GEKKLMPLDHJ=b,GMIKIMACDBC=d});Check((bool)Call("CanUpgrade",c,f,building,up),"surplus base building still upgrades");
  int threshold=(int)Call("MobilizationCost",c);Check(threshold>=3000&&threshold<30000,"cheap mobilization independent of 30k research floor");
  Check(OffensivePolicy.Readiness(8,6,2,10000,threshold,0)==null,"new-game treasury permits otherwise-safe attack");
  Check(OffensivePolicy.Readiness(8,6,2,2999,threshold,0)=="treasury","genuine low funds still blocked");
  Setup(typeof(OffensiveTests));c=Get<AFAGFHJONFJ>(typeof(OffensiveTests),"c");f=Get<MKMOJNAJKJE>(typeof(OffensiveTests),"f");f.DNOGPFHMOMO=10000;
  foreach(var phase in (IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")){}Check(c.Last!=null&&!c.Last.Dismissed,"10k NPC treasury can create actual mocked hostile expedition after all guards");
  Setup(typeof(StatecraftTests));f=Get<MKMOJNAJKJE>(typeof(StatecraftTests),"f");
  AEHAGJAKNEK.NHBPCOGKDML=new(){new(),new()};
  using(var titles=((IEnumerable<string>)Call("TitleSteps",f)).GetEnumerator()){
   Check(titles.MoveNext()&&AEHAGJAKNEK.IEOHLJLICOA.Values.All(h=>h.LMPMNLLGOEC.JAAMIHKIMKP<0),"title scan yields before assigning across registry");
   NEINKOOBJIH.COCMABJFOFH.Remove(f.IBGCLCAIBOL);while(titles.MoveNext()){}
   Check(AEHAGJAKNEK.IEOHLJLICOA.Values.All(h=>h.LMPMNLLGOEC.JAAMIHKIMKP<0),"faction removal while yielding prevents stale title changes");
  }
  Console.WriteLine($"PASS {checks} R74 diagnosis regressions; native calls stubbed, not live gameplay");
 }
}

