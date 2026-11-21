using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
public enum DOHGPECAMJC{Spy,Rescue,Murder}
public enum IPNKPODNAIN{Improve=0,ArmyPass=10,Ally1=11,Ally2=12,ReturnCaptive=13,AskForHelp=14,StopWar=15,Trade=16,BreakContract=17,Wait=18}
public class CHBFLDOKEHC{public IPNKPODNAIN HFBAJIGHOAF;public string BOEPOCFEHKG;}
public class CNLMAMDALFC{public COGDLKNKJGA HFBAJIGHOAF;public int HLNCKLGGECA,ODEKPKAIGPD,LPIEHGBCPLP;}
public class JFLCKHMFGPJ:Native{
 public Localized90 HIENOINGBCN;
 public string OJPECMNINNA;public bool LGGBONFIIJN,ThrowEffect;public int PMPLECHDMNM,IABANLBEFMG=1,NDAPJMMIMKH,AFPGDLJKEDA=100,CCBIHJLDGCG,EHPDLGLPOBD=30,Executed;
 public Il2CppSystem.Collections.Generic.List<CNLMAMDALFC> MDPGDMKHDAG=new();public List<int> OBCCCHHMNOO=new();
 public void ALHDMCHMJNL(string faction,string target,string actor){Executed++;if(ThrowEffect)throw new Exception("native effect failed after partial execution");}
}
public class DLAFJLCLCBG{public string ACPKBHHPKBH,OLMAJLLIJDM;public JFLCKHMFGPJ BHIBPLKOFKH;public static DLAFJLCLCBG OCELGGJBGEN(JFLCKHMFGPJ s,bool b)=>new(){BHIBPLKOFKH=s};}
public partial class MKMOJNAJKJE{
 public bool AidAllowed=true;public bool MHMHBNOMNIB(string ally,string city)=>AidAllowed&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(city,out var c)&&c.LMPMNLLGOEC.LMMNKIOAGLC==ally;
 public Dictionary<string,int> ADJFGBAIACI=new();public List<DLAFJLCLCBG> MEAMPLFJPNH=new();public Il2CppSystem.Collections.Generic.List<JFLCKHMFGPJ> Strategies=new();
 public int Production=100;public int GFHFKMKLGFB(bool x)=>Production;public Il2CppSystem.Collections.Generic.List<JFLCKHMFGPJ> NIDCNBBLPGB(int mode)=>Strategies;
 public Il2CppSystem.Collections.Generic.List<AFAGFHJONFJ> PJMLPLDPEIM(){var list=new Il2CppSystem.Collections.Generic.List<AFAGFHJONFJ>();list.AddRange(MICGPAMNDKM.IJPKEMPIFIH.Values.Where(c=>c.LMPMNLLGOEC.LMMNKIOAGLC==IBGCLCAIBOL));return list;}
}
public partial class CLAGKDECFID{
 public int Travel=10,Skill=75,MissionCalls;public string Target,AidCity;public IPNKPODNAIN Proposal;
 public int LHEKNHNFPIE()=>Skill;public int NBAPOJABAAO(string target,bool b)=>Travel;public int MLGPOHGNLOL(DOHGPECAMJC action)=>Skill;
 public void FHEMHGIGNDE(string target,CHBFLDOKEHC proposal){MissionCalls++;Target=target;Proposal=proposal.HFBAJIGHOAF;AidCity=proposal.BOEPOCFEHKG;OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.StartDiplomacy;OGPEJIEHDAN.MIGIFJPINLB=LMPMNLLGOEC.CNOMNBCPJDI;OGPEJIEHDAN.LKEPGHJDHGG=target;OGPEJIEHDAN.ALGOEDPACKF.Add(proposal);}
 public void IGIMIIDMGAH(string target,string hero,DOHGPECAMJC action,bool b){MissionCalls++;Target=target;OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.StartScheme;OGPEJIEHDAN.MIGIFJPINLB=LMPMNLLGOEC.CNOMNBCPJDI;OGPEJIEHDAN.NHKNHHIBOGK=target;OGPEJIEHDAN.LKEPGHJDHGG=hero;OGPEJIEHDAN.JFKIANPNOJH=action;}
}
public partial class FEKJFDMPPCC{public string MIGIFJPINLB,LKEPGHJDHGG,NHKNHHIBOGK;}
public static partial class AEHAGJAKNEK{public static Dictionary<string,CLAGKDECFID> IEOHLJLICOA=new();public static CLAGKDECFID KDHLLDHILCO(string id)=>new(){OJPECMNINNA=id,LMPMNLLGOEC=new(){LMMNKIOAGLC=id,CNOMNBCPJDI="capital"}};}
public static class StatecraftTests{
 const BindingFlags Flags=BindingFlags.Static|BindingFlags.NonPublic;static int tests;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,Flags).Invoke(null,args);
 static void Set(string name,object x)=>typeof(StrategicSupport).GetField(name,Flags).SetValue(null,x);
 static void Check(bool b,string message){tests++;if(!b)throw new Exception("Statecraft: "+message);}
 static AFAGFHJONFJ c;static MKMOJNAJKJE f;static List<AFAGFHJONFJ> cities;
 static void Setup(){
  typeof(CityIntegrationTests).GetMethod("Setup",Flags).Invoke(null,null);c=(AFAGFHJONFJ)typeof(CityIntegrationTests).GetField("c",Flags).GetValue(null);f=(MKMOJNAJKJE)typeof(CityIntegrationTests).GetField("f",Flags).GetValue(null);
  Call("Ready");((Dictionary<string,double>)typeof(StrategicSupport).GetField("statecraftDays",Flags).GetValue(null)).Clear();
  NEINKOOBJIH.MIPFHCKDJOK=100;Set("researchOwner",null);Set("researchGovernor",null);Set("researchTeam",null);cities=new(){c};
  c.Roster=Enumerable.Range(0,24).Select(i=>new CLAGKDECFID{OJPECMNINNA="s"+i,LMPMNLLGOEC=new(){LMMNKIOAGLC=f.IBGCLCAIBOL,CNOMNBCPJDI=c.OJPECMNINNA},MGLFFEANDOI=true,BAPIJIJLLCA=new(){HFBAJIGHOAF=OEBGIDGEIIG.Explore}}).ToList();
  AEHAGJAKNEK.IEOHLJLICOA.Clear();foreach(var h in c.Roster)AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;
  f.DNOGPFHMOMO=100000;c.Power=100;f.PEGLDBENJMF=2000;
 }
 static JFLCKHMFGPJ Strategy(string id,int effect,int amount=20,int instant=1){var s=new JFLCKHMFGPJ{OJPECMNINNA=id,PMPLECHDMNM=f.Strategies.Count+1,IABANLBEFMG=instant};s.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)effect,HLNCKLGGECA=amount});f.Strategies.Add(s);return s;}
 static object Pool()=>Call("StateOfficers",cities,f);
 static MKMOJNAJKJE Enemy(string id,BHBNKONCOJD relation){var other=new MKMOJNAJKJE{IBGCLCAIBOL=id};NEINKOOBJIH.COCMABJFOFH[id]=other;f.AENJACOFBJK[id]=new(){EBIOPDHEBOF=relation};return other;}
 public static void Run(){
  Setup();f.DNOGPFHMOMO=0;var money=Strategy("cash",32,5000);Check((bool)Call("TryStateStrategy",f,cities)&&money.Executed==1&&f.PEGLDBENJMF==1900,"native instant effect paid once");Check(!(bool)Call("TryStateStrategy",f,cities)&&money.Executed==1,"native cooldown prevents duplicate");
  Setup();c.LMPMNLLGOEC.OEGNGFBCHFL=0;var food=Strategy("food",16,20,0);food.CCBIHJLDGCG=40;var existing=new DLAFJLCLCBG{BHIBPLKOFKH=new(){OJPECMNINNA="manual",PMPLECHDMNM=99,CCBIHJLDGCG=30}};f.MEAMPLFJPNH.Add(existing);
  Check((bool)Call("TryStateStrategy",f,cities)&&f.MEAMPLFJPNH.Count==2&&f.MEAMPLFJPNH.Contains(existing),"ongoing benefit preserves existing manual strategy");Check(!(bool)Call("TryStateStrategy",f,cities)&&f.MEAMPLFJPNH.Count==2,"active type does not stack");
  Setup();c.LMPMNLLGOEC.OEGNGFBCHFL=0;food=Strategy("food",16,20,0);food.CCBIHJLDGCG=101;Check(!(bool)Call("TryStateStrategy",f,cities)&&f.PEGLDBENJMF==2000,"upkeep cannot exceed production");
  food.CCBIHJLDGCG=100;f.PEGLDBENJMF=299;Check(!(bool)Call("TryStateStrategy",f,cities),"two upkeep periods remain after start cost");
  Setup();c.LMPMNLLGOEC.OEGNGFBCHFL=0;food=Strategy("mixed",16);food.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)999,HLNCKLGGECA=-20});Check(!(bool)Call("TryStateStrategy",f,cities),"unmodeled tradeoff is not silently ignored");
  Setup();f.DNOGPFHMOMO=0;money=Strategy("fault",32);money.ThrowEffect=true;try{Call("TryStateStrategy",f,cities);}catch(TargetInvocationException){}Check(f.PEGLDBENJMF==1900&&money.Executed==1&&!(bool)Call("TryStateStrategy",f,cities),"partial native exception does not refund into repeated free effects");
  Setup();var pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Pool();Check(pool.Count==24,"occupied exploration officers available without stopping their duties");c.Roster[0].BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Military;c.Roster[1].OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;
  pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Pool();Check(pool.Count==22&&!pool.Any(p=>p.Hero==c.Roster[0]||p.Hero==c.Roster[1]),"military and research protected");c.Roster[2].OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;Check(((List<(AFAGFHJONFJ,CLAGKDECFID)>)Pool()).Count==21,"researchers remain protected without blocking every idle envoy");
  Setup();var other=Enemy("enemy",BHBNKONCOJD.War);var ec=new AFAGFHJONFJ{OJPECMNINNA="enemyCity",Power=500};ec.LMPMNLLGOEC.LMMNKIOAGLC="enemy";MICGPAMNDKM.IJPKEMPIFIH[ec.OJPECMNINNA]=ec;c.FKMKGNPFLFP.Add(ec);
  pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Pool();Check((bool)Call("TryStateDiplomacy",f,cities,pool)&&c.Roster.Any(h=>h.Proposal==IPNKPODNAIN.StopWar&&h.MissionCalls==1),"overmatched faction sends native ceasefire proposal");Check(!(bool)Call("TryStateDiplomacy",f,cities,pool),"target diplomacy cooldown");
  Setup();other=Enemy("enemy",BHBNKONCOJD.War);ec=new(){OJPECMNINNA="enemyCity",Power=50};ec.LMPMNLLGOEC.LMMNKIOAGLC="enemy";c.FKMKGNPFLFP.Add(ec);pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Pool();Check(!(bool)Call("TryStateDiplomacy",f,cities,pool),"healthy front does not automatically surrender initiative");Check((bool)Call("TryStateSpy",f,cities,pool)&&c.Roster.Count(h=>h.MissionCalls==1)==1,"hostile adjacent city receives one native spy");Check(!(bool)Call("TryStateSpy",f,cities,pool),"spy target cooldown");
  Setup();c.Siege=true;Check(((List<(AFAGFHJONFJ,CLAGKDECFID)>)Pool()).Count==0,"siege never exports envoy or spy");
  Setup();var traveler=c.Roster[0];traveler.IGIMIIDMGAH("farCity","",DOHGPECAMJC.Spy,false);c.Roster.Remove(traveler);var envoy=c.Roster[0];envoy.FHEMHGIGNDE("farFaction",new(){HFBAJIGHOAF=IPNKPODNAIN.Ally1});c.Roster.Remove(envoy);
  Check(((List<(AFAGFHJONFJ,CLAGKDECFID)>)Pool()).Count==0,"travellers missing from city roster still count toward faction cap after load");
  Setup();other=Enemy("ally",BHBNKONCOJD.Ally);var aidCity=new AFAGFHJONFJ{OJPECMNINNA="allyRear",Power=500};aidCity.LMPMNLLGOEC.LMMNKIOAGLC="ally";for(int i=0;i<3;i++)aidCity.LMPMNLLGOEC.KJPPGIOCDID.Add(new());MICGPAMNDKM.IJPKEMPIFIH[aidCity.OJPECMNINNA]=aidCity;
  Check(Call("AidSource",f,other)==aidCity,"native aid names ally source city");aidCity.Siege=true;Check(Call("AidSource",f,other)==null,"ally under siege cannot donate troops");aidCity.Siege=false;f.AidAllowed=false;Check(Call("AidSource",f,other)==null,"native aid eligibility remains required");
  f.AidAllowed=true;Enemy("enemy",BHBNKONCOJD.War);ICGAEMLACJG.HFJIECIBFKB[999]=new(){m_belongPower="enemy",m_targetCity=c.OJPECMNINNA};pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Pool();
  Check(pool.Count==0,"incoming invasion protects home officers before battle arrives");
  // Use a separate rear delegation for an envoy; threatened cities keep staff.
  var rear=new AFAGFHJONFJ{OJPECMNINNA="rear",Roster=c.Roster};rear.LMPMNLLGOEC.LMMNKIOAGLC=f.IBGCLCAIBOL;rear.LMPMNLLGOEC.IPJHOOJBCKI=c.LMPMNLLGOEC.IPJHOOJBCKI;foreach(var h in rear.Roster)h.LMPMNLLGOEC.CNOMNBCPJDI=rear.OJPECMNINNA;c.Roster=new();cities.Add(rear);MICGPAMNDKM.IJPKEMPIFIH[rear.OJPECMNINNA]=rear;
  pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Pool();Check((bool)Call("TryStateDiplomacy",f,cities,pool)&&rear.Roster.Any(h=>h.Proposal==IPNKPODNAIN.StopWar||h.AidCity==aidCity.OJPECMNINNA),"rear envoy responds to invaded city without taking its staff");
  // The enemy is on cooldown after the ceasefire envoy: explicitly exercise
  // the allied aid branch and its source-city payload, not just the rank.
  pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Pool();Check((bool)Call("TryStateDiplomacy",f,cities,pool)&&rear.Roster.Any(h=>h.Proposal==IPNKPODNAIN.AskForHelp&&h.AidCity==aidCity.OJPECMNINNA),"ally request carries validated rear source city");
  Setup();c.LMPMNLLGOEC.OEGNGFBCHFL=0;food=Strategy("banned",16);food.LGGBONFIIJN=true;Check(!(bool)Call("TryStateStrategy",f,cities),"mod AI ban remains effective");food.LGGBONFIIJN=false;food.NDAPJMMIMKH=10;Check((bool)Call("TryStateStrategy",f,cities)&&food.Executed==0&&f.MEAMPLFJPNH.Count==1&&f.PEGLDBENJMF==1900,"delayed benefit queues native execution and pays only once");Check(!(bool)Call("TryStateStrategy",f,cities)&&f.PEGLDBENJMF==1900,"pending delayed strategy does not duplicate");
  Setup();Check((bool)Call("DayReady","clock",1d,true)&&!(bool)Call("DayReady","clock",1d,false),"paused time cannot repeat planning");NEINKOOBJIH.MIPFHCKDJOK+=1;Check((bool)Call("DayReady","clock",1d,false),"next game day can reconsider");
  Check(StatecraftPolicy.Diplomacy(3,1,false,100,100,true)==250,"ally aid when invaded");Check(StatecraftPolicy.Diplomacy(0,1,true,100,100,false)==150,"alliance needs shared hostile faction");Check(StatecraftPolicy.Diplomacy(0,0,false,100,100,false)==0,"peaceful no-purpose diplomacy suppressed");
  Setup();c.LMPMNLLGOEC.IPJHOOJBCKI=NEINKOOBJIH.OPIKIHPNCIP;c.LMPMNLLGOEC.BKENJPNKMGI=NEINKOOBJIH.OPIKIHPNCIP;c.OPNKGGKHAGJ=false;
  f.DNOGPFHMOMO=0;money=Strategy("independent-national-cash",32,5000);
  Check(((List<AFAGFHJONFJ>)Call("StateCities",f)).Count==0,"fixture has NO delegated cities");
  Call("TryFactionStatecraft",f);Check(money.Executed==1&&f.PEGLDBENJMF==1900,"national strategy executes without governor or mayor delegation");
  Check(c.Roster.All(h=>h.MissionCalls==0),"manual city exports no envoy or spy");
  Call("TryFactionStatecraft",f);Check(money.Executed==1,"same day callbacks do not double spend");
  var random=new Random(156);for(int i=0;i<500;i++){int points=random.Next(5000),start=random.Next(6000),upkeep=random.Next(300),active=random.Next(500),production=random.Next(500);if(StatecraftPolicy.Affordable(points,start,upkeep,active,production))Check(points>=start&&(upkeep==0||(active+upkeep<=production&&points-start>=2*(active+upkeep))),"random strategy resource conservation");}
  Setup();var saving=Strategy("food-cost-saving",19,-30,0);Check((bool)Call("TryStateStrategy",f,cities)&&f.MEAMPLFJPNH.Any(x=>x.BHIBPLKOFKH==saving),"negative food expense is beneficial");
  Setup();saving=Strategy("salary-saving",27,-30,0);Check((bool)Call("TryStateStrategy",f,cities),"salary reduction with officers supported");
  Setup();saving=Strategy("bad-cost-increase",19,30,0);Check(!(bool)Call("TryStateStrategy",f,cities),"positive expense increase still rejected");
  Setup();for(int i=0;i<7;i++)f.MEAMPLFJPNH.Add(new(){BHIBPLKOFKH=new(){OJPECMNINNA="existing"+i,PMPLECHDMNM=100+i}});
  var mixed=Strategy("fgo-mixed-benefits",12,100,0);mixed.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)19,HLNCKLGGECA=-50});mixed.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)27,HLNCKLGGECA=-50});
  Check((bool)Call("TryStateStrategy",f,cities)&&f.MEAMPLFJPNH.Count==8,"eighth mixed national strategy permitted");
  Check(!(bool)Call("TryStateStrategy",f,cities)&&f.MEAMPLFJPNH.Count==8,"new supported active type still deduplicated");
  Setup();var explore=Strategy("exploration",8,30,0);Check((bool)Call("TryStateStrategy",f,cities),"actual exploration workers enable strategy");
  Setup();var unknown=Strategy("private",45,100,0);Check((bool)Call("TryStateStrategy",f,cities),"recognized beneficial private interaction modifier is not rejected as unknown");
  Console.WriteLine($"PASS Statecraft {tests} assertions (native calls mocked; not gameplay).");
 }
}
