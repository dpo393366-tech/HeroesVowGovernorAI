using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
namespace Il2CppSystem.Collections.Generic{public class List<T>:System.Collections.Generic.List<T>{static int serial;public IntPtr Pointer=(IntPtr)(++serial);}}
public enum MHCEFEODENB{None,Military,Civil,Agriculture,Commerce,Resources,Special,Wall}
public enum FFDDNPOHLHN{None,Public,Private}
public enum COGDLKNKJGA{None}
public enum HLIMKFFGEPO{None}
public enum HNDFFIEJNHI{Government,Public,Private,Forest}
public enum GHIHNCIFMBB{Up,Down,Left,Right}
public enum LDPAFPHFBCP{Commerce,Agriculture,Function}
public enum DDBHJKOIIPI{BuildingMax,BuildingNum,CityCommerce,CityAgriculture,CityMoneyIncome,CityFoodIncome,CityPopularFeel,CitySecurity,CityPopulation,TwoBlockCommerce,TwoBlockAgriculture}
public class OKDBGMLHBAP{}
public class PABIPDOFBFG:Native{
 public FFDDNPOHLHN OCJNCECECMH;
 public string OJPECMNINNA,MDOIJIAONJM,EIMGMPDMAAJ,JADLIKBKOHG;public bool POLCGKHKLHI,FHKHHKBBKGM;
 public int NPCHPDNFAME=600,MLAJLJKFGBD=2,PNPLBNAFMCM=2,IKELHGAHBID,ODKKMLDAPJL,ADGOBJJMEAO,GJPOJINPCHP;
 public int OGFAHJHBCNG=1000,OFHFGEBGMBD,DHLNIKKLNKJ,KGIMAFCNEKE,LCKEPHGCAPC,IIBKLLKPNEH,PEGLDBENJMF,HIKFEJKBPIK,DJNPCHEJGNM,EOJNDEDNEKE,LAIEKKPFOKF,EMHGLEDIOHK;public bool KFCEGGHJGGB=true;public float DONIEGIDACP=20;public string KAPKDBGFHEG(int language)=>OJPECMNINNA;
 public MHCEFEODENB HFBAJIGHOAF=MHCEFEODENB.Commerce;public int[]LJCICFHPMLM={-1,-1,-1,-1};
 public int CNCPIILGDEF=100,AJJJJNDNNME,CGBGIDCEBGD,EGBEEDFCIGH,HKBPGJLMFIG,DPFIFIOKHEI;
 public float FIPONBEGNJO,NIKFGJLNNCA,LGCANBOEGGH,AIFPACNFKDB;
 public int JCMOAFOEPDP;public bool OEKNLKCBAEH,ACNIHJHNIEP;
 public bool TechAllowed=true,Prerequisite=true;
 public bool PLEHAHBBLGL(OKDBGMLHBAP f)=>TechAllowed;
 public bool MCGEOICKKEE(AFAGFHJONFJ c,bool p)=>Prerequisite;
 public bool LOJCCNNJKHF(AFAGFHJONFJ c,bool p)=>LJCICFHPMLM[c.MPGFDBPEHBO(true)]>=0&&c.ELKFAJKBPKJ(OJPECMNINNA,true,true).Count>=LJCICFHPMLM[c.MPGFDBPEHBO(true)];
}
public class OGABIJECHFG:Native{
 public UnityEngine.Vector3 BJHLIMBCBNN;public GHIHNCIFMBB EOMMBIBIEJN;public bool ABEAICBDKLD;public string BHPDJIDJHPF;
 public FEEDONDCELP[] Cells=Array.Empty<FEEDONDCELP>();public FEEDONDCELP[] JNMLGGLLANN()=>Cells;
 public PABIPDOFBFG GEKKLMPLDHJ;public JNHOOCJJPJO GMIKIMACDBC;public int OJPECMNINNA,OGFAHJHBCNG=1000,JOFCACIPJKK;public string LHCJPKGANCK;
 public float EMHNNCLBADE=100,BNMEOOJPOML=100;public bool MGLFFEANDOI,ThrowBeforeUpgrade,ThrowAfterUpgrade,ThrowRepair;
 public List<string>JMIADAFPGMC=new(),HNEPDNGMAIL=new();public List<OGABIJECHFG>GJLBKJOONCF=new();
 public bool EHABIIDOKGC()=>EMHNNCLBADE<BNMEOOJPOML;
 public void OHILNFOEADD(int n){OGFAHJHBCNG=(int)Math.Clamp((long)OGFAHJHBCNG+n,0,GEKKLMPLDHJ.OGFAHJHBCNG);if(ThrowRepair)throw new Exception("repair post-change failure");}
 public void GMMDMPFFIGP(string id,bool complete){if(ThrowBeforeUpgrade)throw new Exception("upgrade pre-change failure");GEKKLMPLDHJ=AEHAGJAKNEK.ACIGFKMDOKE[id];EMHNNCLBADE=complete?100:0;if(ThrowAfterUpgrade)throw new Exception("upgrade post-change failure");}
}
public class FEEDONDCELP:Native{
 public UnityEngine.Vector3 BJHLIMBCBNN;public int HFBAJIGHOAF,PAHKDHJPFKD,GHMMBOIKHNC,FCOJFBEHMAG=-9999,PMGDGAAICCG,FPOIMBFMEDH,FGHAPGBJINB;public bool ICGFBPCAJCI;public JNHOOCJJPJO GMIKIMACDBC;
}
public class Ci_BuildingCtrl {public static implicit operator bool(Ci_BuildingCtrl x)=>x!=null;public void RemoveBuilding(bool x){}public static Ci_BuildingCtrl GetOne(OGABIJECHFG x)=>new();}
public class PEGMABENGBB:Native{
 public static PEGMABENGBB OCELGGJBGEN(JNHOOCJJPJO d)=>new(){GMIKIMACDBC=d};
 public bool NEDMGJPJNKA=true;public GHIHNCIFMBB ILFHMILCBLA;
 public FEEDONDCELP[] FAFNFFJAFKH={new(),new(),new(),new()};public JNHOOCJJPJO GMIKIMACDBC;public int PABEMCAGKJJ=2,HAPLIFADGBA=2;public bool Occupied,Street=true;
 public List<OGABIJECHFG> GJLBKJOONCF=new();
 public bool BFPBNAKIKEO(PABIPDOFBFG b)=>!Occupied&&Street&&PABEMCAGKJJ*HAPLIFADGBA==b.MLAJLJKFGBD*b.PNPLBNAFMCM;
}
public class JNHOOCJJPJO:Native{
 public bool PLIFMOIBPME;public UnityEngine.Vector3 BJHLIMBCBNN;public int FMANEHEEDKP,GHECBGMGBBK,NDOFLDJGDOM;
 public Il2CppSystem.Collections.Generic.List<Il2CppSystem.Collections.Generic.List<FEEDONDCELP>> FAFNFFJAFKH;
 public void BCJALLCJCNL(){} public void BBJBFAFMCCB(bool unused){if(FAFNFFJAFKH==null)return;HGNFEMJOPPB.Clear();if(PLIFMOIBPME)return;for(int y=0;y<FAFNFFJAFKH.Count;y++)for(int x=0;x<FAFNFFJAFKH[y].Count;x++)for(int h=1;h<=2;h++)for(int w=1;w<=2;w++){
  if(y+h>FAFNFFJAFKH.Count||x+w>FAFNFFJAFKH[y].Count)continue;var cells=new List<FEEDONDCELP>();for(int yy=y;yy<y+h;yy++)for(int xx=x;xx<x+w;xx++)cells.Add(FAFNFFJAFKH[yy][xx]);if(cells.Any(q=>q.FCOJFBEHMAG!=-9999))continue;
  HGNFEMJOPPB.Add(new(){GMIKIMACDBC=this,PABEMCAGKJJ=w,HAPLIFADGBA=h,FAFNFFJAFKH=cells.ToArray(),Street=cells.Any(q=>q.ICGFBPCAJCI),NEDMGJPJNKA=cells.Any(q=>q.ICGFBPCAJCI)});
 }}
 public void FPFEFLILLFN(OGABIJECHFG x,bool unused){x.GMIKIMACDBC=this;AIMLPNBOIIF.Add(x);foreach(var q in x.Cells)q.FCOJFBEHMAG=x.OJPECMNINNA;if(PLIFMOIBPME&&x.Cells.Length>0){x.JOFCACIPJKK=x.Cells[0].PMGDGAAICCG;AJJCCPCJPFC.NBEJCGLJHJA.Add(x);}}
 static int wallSerial=30000;public int GroundWallCalls,WallCalls;public bool FailWallLink;
 public void MAOKGGOJNPM(OGABIJECHFG x,bool skipRefresh,bool register){
  WallCalls++;if(NoRegister)return;x.OJPECMNINNA=++wallSerial;
  x.Cells=FailWallLink?Array.Empty<FEEDONDCELP>():FAFNFFJAFKH.SelectMany(r=>r).Where(q=>(q.BJHLIMBCBNN-AJJCCPCJPFC.FCIJDABCHEN-BJHLIMBCBNN-x.BJHLIMBCBNN).sqrMagnitude<1).Take(1).ToArray();
  FPFEFLILLFN(x,true);if(ThrowAfterRegister)throw new InvalidOperationException("scene refresh failed");
 }
 public string HLGFIMOKLAK;public int EMPGDOLPPFI(OKDBGMLHBAP faction,bool privateTax)=>AIMLPNBOIIF.Sum(x=>x.GEKKLMPLDHJ.CNCPIILGDEF);
 public string OJPECMNINNA;public AFAGFHJONFJ AJJCCPCJPFC;public HNDFFIEJNHI KFCOLLDEJNF=HNDFFIEJNHI.Public;
 public LDPAFPHFBCP HFBAJIGHOAF=LDPAFPHFBCP.Function;public List<OGABIJECHFG>AIMLPNBOIIF=new();public List<PEGMABENGBB>HGNFEMJOPPB=new();
 public int IMMLKOLPPHF()=>AIMLPNBOIIF.Sum(x=>x.GEKKLMPLDHJ.CGBGIDCEBGD);
 public int KMDCHCBIAIG()=>AIMLPNBOIIF.Sum(x=>x.GEKKLMPLDHJ.EGBEEDFCIGH);
 public bool NoRegister,ThrowAfterRegister;
 public void PJNMLNMLAKA(PABIPDOFBFG b,PEGMABENGBB p){if(PLIFMOIBPME)GroundWallCalls++;if(NoRegister)return;p.Occupied=true;var created=new OGABIJECHFG{OJPECMNINNA=10000+AIMLPNBOIIF.Count,GEKKLMPLDHJ=b,GMIKIMACDBC=this,EMHNNCLBADE=0,OGFAHJHBCNG=b.OGFAHJHBCNG,Cells=PLIFMOIBPME?Array.Empty<FEEDONDCELP>():p.FAFNFFJAFKH.Take(p.PABEMCAGKJJ*p.HAPLIFADGBA).ToArray()};FPFEFLILLFN(created,false);if(ThrowAfterRegister)throw new InvalidOperationException("scene refresh failed");}
 public void HNIEPCENKGC(OGABIJECHFG x){AIMLPNBOIIF.Remove(x);AJJCCPCJPFC?.NBEJCGLJHJA.Remove(x);foreach(var q in x.Cells)if(q.FCOJFBEHMAG==x.OJPECMNINNA)q.FCOJFBEHMAG=-9999;}
}
public class PGLFLKOHDFO{public DDBHJKOIIPI HFBAJIGHOAF;public string IJNEIHPHMEE;public int KLCDGPDHENB;}
public class HIADFOLCFII{
 public Func<AFAGFHJONFJ,bool> GoalCheck;public bool PIHDGCBCMCP,Complete;public int DMNMNBGHNKM;public List<PGLFLKOHDFO>CKIJPJFFFOA=new();public bool NHIPAJDALBA(AFAGFHJONFJ c)=>GoalCheck?.Invoke(c)??Complete;
}
public class FBLFDEEGJMH{public int Cost,CNLBJJNJLNG,FDNECLDGCHN,LBGMKEHHPAA;public string GPEOLOAMMHP;public Il2CppSystem.Collections.Generic.List<CLAGKDECFID> ABJNKABKKJM=new();public int PKPLGNACAPG()=>Cost;}
public class GGOGLBGDEJJ {public int MinAbility;public int[] DDGCODBDINJ={500},DONIEGIDACP={10000},DAJIHBICKBE={1};public Il2CppSystem.Collections.Generic.List<string> ANNMPIPJGNC=new();public bool OIFAMCDJHFP(CLAGKDECFID h,int level)=>h.NativeEligible&&h.Ability[2]>=MinAbility;}
public partial class MKMOJNAJKJE{public float IJJDBPGOJJA(COGDLKNKJGA kind,AFAGFHJONFJ c)=>0;public int IIBKLLKPNEH=1000,PEGLDBENJMF=1000;public OKDBGMLHBAP KCKGONGIMDA=new();public Dictionary<string,FBLFDEEGJMH> OINKOPJJGJH=new();}
public partial class FNFJODHIJFO{public int ResearchCalls;public Action OnResearch;public bool ThrowResearch;public void OHBNFHKFNPH(){ResearchCalls++;OnResearch?.Invoke();if(ThrowResearch)throw new Exception("injected research failure");}public Il2CppSystem.Collections.Generic.List<AFAGFHJONFJ> PJMLPLDPEIM(){var result=new Il2CppSystem.Collections.Generic.List<AFAGFHJONFJ>();foreach(var c in MICGPAMNDKM.IJPKEMPIFIH.Values)if(c.LMPMNLLGOEC.LMMNKIOAGLC==LMMNKIOAGLC && NEINKOOBJIH.COCMABJFOFH[LMMNKIOAGLC].ANMMMOIKLEG.TryGetValue(c.LMPMNLLGOEC.IPJHOOJBCKI,out var g) && g.Pointer==Pointer)result.Add(c);return result;}}
public static partial class AEHAGJAKNEK{public static bool KDHLLDHILCO(string id,out CLAGKDECFID h){h=null;return false;}public static Dictionary<string,PABIPDOFBFG> ACIGFKMDOKE=new();public static Dictionary<string,GGOGLBGDEJJ> HMHMMONMBAP=new();}
public partial class AFAGFHJONFJ{
 public UnityEngine.Vector3 FCIJDABCHEN;public Il2CppSystem.Collections.Generic.List<OGABIJECHFG> NBEJCGLJHJA=new();
 // Native DAEIKODOFGJ visits only the requested wall side, then advances
 // the first unfinished project on it. A registered row with side zero stalls.
 public float DAEIKODOFGJ(float effort,int side){foreach(var x in NBEJCGLJHJA)if(x.JOFCACIPJKK==side&&x.EHABIIDOKGC()){float used=Math.Min(effort,x.BNMEOOJPOML-x.EMHNNCLBADE);x.EMHNNCLBADE+=used;return effort-used;}return effort;}
 public float EHCCLNDFJGL(HLIMKFFGEPO kind)=>0;
 public List<JNHOOCJJPJO>FGBLEMGGOPO=new();public List<HIADFOLCFII>HFLAGGEFBOO=new();public bool Siege,CanPlan=true;
 public int StaffingCalls;public void OBIOEOJAHMM(){StaffingCalls++;}
 // Matches installed native NBKEECPNIJH: only the actual mayor/governor
 // being the player makes the personal automation flag relevant.
 public bool NBKEECPNIJH(bool x)=>CanPlan&&((LMPMNLLGOEC.IPJHOOJBCKI!=NEINKOOBJIH.OPIKIHPNCIP&&LMPMNLLGOEC.BKENJPNKMGI!=NEINKOOBJIH.OPIKIHPNCIP)||(x&&OPNKGGKHAGJ));public bool GKLIEHCHDLD()=>Siege;
 public Il2CppSystem.Collections.Generic.List<CLAGKDECFID> EOMOIINOEMB(int mode,bool a,bool b,bool cc,bool d,bool e,bool f){object[] prefix={this,mode,b};typeof(StrategicSupport).GetMethod("IncludeResearchWorkers",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,prefix);b=(bool)prefix[2];var result=new Il2CppSystem.Collections.Generic.List<CLAGKDECFID>();foreach(var h in ABJNKABKKJM)if(h.NativeEligible&&(!h.MGLFFEANDOI||b)&&h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.None)result.Add(h);object[] postfix={this,result};typeof(StrategicSupport).GetMethod("FilterResearchWorkers",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,postfix);LastNativePool=(Il2CppSystem.Collections.Generic.List<CLAGKDECFID>)postfix[1];return LastNativePool;}
 public Il2CppSystem.Collections.Generic.List<CLAGKDECFID> LastNativePool;
 public int MoneyIncome,FoodIncome;public int Security,Popular,Scale;public int CBDMPGOEMMK(string faction)=>Security;public int PCNOLBELFMF()=>Popular;public int MPGFDBPEHBO(bool x)=>Scale;public int NMDMENNPKDM()=>0;public int NMKKHJEMMFH()=>0;public int EMPGDOLPPFI()=>MoneyIncome;public int NBAKOCHGMBM(bool x)=>FoodIncome;
 public List<OGABIJECHFG> ELKFAJKBPKJ(string id,bool a,bool b)=>FGBLEMGGOPO.SelectMany(x=>x.AIMLPNBOIIF).Where(x=>(a||!x.EHABIIDOKGC())&&(x.GEKKLMPLDHJ.OJPECMNINNA==id||(b&&x.GEKKLMPLDHJ.MDOIJIAONJM==id))).ToList();
}
public static class CityIntegrationTests
{
 static int tests;static AFAGFHJONFJ c;static MKMOJNAJKJE f;static JNHOOCJJPJO d;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,args);
 static void Check(bool x,string reason){tests++;if(!x)throw new Exception(reason);}
 static void Set(string name,object value)=>typeof(StrategicSupport).GetField(name,BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,value);
 static void Setup(){
  new StrategicSupport().Load();MICGPAMNDKM.IJPKEMPIFIH=new();NEINKOOBJIH.COCMABJFOFH.Clear();AEHAGJAKNEK.ACIGFKMDOKE.Clear();
  f=new(){IBGCLCAIBOL="F",DNOGPFHMOMO=1000000};f.ANMMMOIKLEG["gov"]=new(){LMMNKIOAGLC="F"};NEINKOOBJIH.COCMABJFOFH["F"]=f;
  c=new(){OJPECMNINNA="city"};c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.LMPMNLLGOEC.DNOGPFHMOMO=70000;MICGPAMNDKM.IJPKEMPIFIH["city"]=c;
  for(int i=0;i<10;i++)c.LMPMNLLGOEC.KJPPGIOCDID.Add(new());
  d=new(){OJPECMNINNA="block",AJJCCPCJPFC=c};c.FGBLEMGGOPO.Add(d);
  for(int i=0;i<8;i++)d.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=d});Call("Ready");
 }
 static PABIPDOFBFG Add(string id,MHCEFEODENB type=MHCEFEODENB.Commerce,int cost=600){var b=new PABIPDOFBFG{OJPECMNINNA=id,HFBAJIGHOAF=type,NPCHPDNFAME=cost};AEHAGJAKNEK.ACIGFKMDOKE[id]=b;return b;}
 static bool Build()=>(bool)Call("BuildBest",c,f);
 public static void Run(){
  // Recorded live failure: Xuchang, Wu Zetian as both governor and mayor,
  // player faction, personal automation false. This is delegated, not manual.
  Setup();Add("market");c.OJPECMNINNA="CS1026";MICGPAMNDKM.IJPKEMPIFIH.Clear();MICGPAMNDKM.IJPKEMPIFIH[c.OJPECMNINNA]=c;c.Player=true;c.OPNKGGKHAGJ=false;c.LMPMNLLGOEC.BKENJPNKMGI="player";
  f.ANMMMOIKLEG["WJ1000612bed"]=f.ANMMMOIKLEG["gov"];
  c.LMPMNLLGOEC.IPJHOOJBCKI=c.LMPMNLLGOEC.BKENJPNKMGI="WJ1000612bed";
  f.DNOGPFHMOMO=137500;c.LMPMNLLGOEC.DNOGPFHMOMO=0;
  Check(c.AKIJOBLFLCJ("player")&&c.NBKEECPNIJH(true),"Live native distinction: player affiliation does not mean personal management");
  Call("AfterGovernorEconomy",f.ANMMMOIKLEG["gov"]);
  Check(d.AIMLPNBOIIF.Count>0,"LIVE REGRESSION: delegated Xuchang must build despite personal automation false");
  Check(f.ANMMMOIKLEG["gov"].ResearchCalls==1,"Delegated Xuchang research path reached");
  Check(f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO==137500-d.AIMLPNBOIIF.Count*600,"Delegated investment charges exact construction cost");
  for(int affiliation=0;affiliation<2;affiliation++)for(int playerGovernor=0;playerGovernor<2;playerGovernor++)for(int playerMayor=0;playerMayor<2;playerMayor++)for(int automation=0;automation<2;automation++){
   Setup();c.Player=affiliation==1;c.OPNKGGKHAGJ=automation==1;
   f.ANMMMOIKLEG["player"]=f.ANMMMOIKLEG["gov"];
   c.LMPMNLLGOEC.IPJHOOJBCKI=playerGovernor==1?"player":"gov";
   c.LMPMNLLGOEC.BKENJPNKMGI=playerMayor==1?"player":"mayor";
   object[] eligibilityArgs={c,null};
   Check((bool)Call("Eligible",eligibilityArgs)==(playerGovernor==0&&(playerMayor==0||automation==1)),"Affiliation/governor/mayor/automation eligibility matrix");
  }
  Setup();Add("market");var barracks=Add("barracks",MHCEFEODENB.Military);barracks.IKELHGAHBID=1;barracks.LJCICFHPMLM[0]=1;
  Check(Build()&&d.AIMLPNBOIIF[0].GEKKLMPLDHJ==barracks,"barracks first");Check(Build()&&d.AIMLPNBOIIF[1].GEKKLMPLDHJ.OJPECMNINNA=="market","native barracks limit");
  Check(c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO==1070000-1200,"real costs, no money creation");
  Setup();Add("market");var needed=Add("temple",MHCEFEODENB.Civil);needed.CNCPIILGDEF=0;c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE="temple",KLCDGPDHENB=1}}});
  Check(Build()&&d.AIMLPNBOIIF[0].GEKKLMPLDHJ==needed,"missing development building before economy");Check(Build()&&d.AIMLPNBOIIF[1].GEKKLMPLDHJ.OJPECMNINNA=="market","goal count not overbuilt");
  Setup();Add("market");d.HFBAJIGHOAF=LDPAFPHFBCP.Agriculture;Check(Build(),"agricultural district permits beneficial commerce when no farm is available");
  Setup();Add("market");d.HFBAJIGHOAF=LDPAFPHFBCP.Agriculture;
  var farm=Add("farm",MHCEFEODENB.Agriculture);farm.CNCPIILGDEF=0;farm.AJJJJNDNNME=400;c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE="farm",KLCDGPDHENB=1}}});Check(Build()&&d.AIMLPNBOIIF[0].GEKKLMPLDHJ==farm,"city farm goal takes priority over a permitted market");
  Setup();var m=Add("market");d.AIMLPNBOIIF.Add(new(){GEKKLMPLDHJ=m});var a=Add("farm",MHCEFEODENB.Agriculture);a.CNCPIILGDEF=999999;Check(Build()&&d.AIMLPNBOIIF.Last().GEKKLMPLDHJ==a,"existing commerce permits a higher-value mixed build");
  Setup();Add("market");d.KFCOLLDEJNF=HNDFFIEJNHI.Private;Check(!Build(),"private property untouched");
  Setup();var b=Add("market");b.TechAllowed=false;Check(!Build(),"technology lock preserved");b.TechAllowed=true;b.Prerequisite=false;Check(!Build(),"prerequisite preserved");
  b.Prerequisite=true;b.POLCGKHKLHI=true;Check(!Build(),"banAI preserved");b.POLCGKHKLHI=false;b.MDOIJIAONJM="base";Check(!Build(),"upgrades not created as free new building");
  b.MDOIJIAONJM=null;b.JADLIKBKOHG="item";b.GJPOJINPCHP=1;Check(!Build(),"item payment not bypassed");
  Setup();b=Add("building");b.ODKKMLDAPJL=100;b.ADGOBJJMEAO=80;Check(Build()&&f.IIBKLLKPNEH==900&&f.PEGLDBENJMF==920,"points charged once");
  Setup();Add("market");f.DNOGPFHMOMO=10000;c.LMPMNLLGOEC.DNOGPFHMOMO=10000;Check(!Build(),"reserves cannot be spent");
  f.DNOGPFHMOMO=104000;((System.Collections.IDictionary)typeof(StrategicSupport).GetField("budgets",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null)).Clear();Check(Build()&&f.DNOGPFHMOMO==103400&&c.LMPMNLLGOEC.DNOGPFHMOMO==10000,"exact required treasury transfer");
  Setup();Add("market");d.NoRegister=true;try{Build();throw new Exception("missing failure");}catch(TargetInvocationException){}Check(c.LMPMNLLGOEC.DNOGPFHMOMO==70000&&f.DNOGPFHMOMO==1000000,"failed registration refunds");
  Setup();Add("market");d.ThrowAfterRegister=true;try{Build();throw new Exception("missing failure");}catch(TargetInvocationException){}Check(c.LMPMNLLGOEC.DNOGPFHMOMO==69400&&d.AIMLPNBOIIF.Count==1,"partial native scene failure never creates free building");
  Setup();Add("market");foreach(var p in d.HGNFEMJOPPB)p.Occupied=true;Check(!Build(),"occupied tiles excluded");
  Setup();Add("market");c.Siege=true;Call("AfterRecruitInvestment",c,false);Check(d.AIMLPNBOIIF.Count==0&&f.ANMMMOIKLEG["gov"].ResearchCalls==0,"besieged city not disturbed");
  Setup();Add("market");c.LMPMNLLGOEC.IPJHOOJBCKI="player";Call("AfterRecruitInvestment",c,false);Check(d.AIMLPNBOIIF.Count==0,"player personal district excluded");
  Setup();Add("market");c.Player=true;c.OPNKGGKHAGJ=false;c.LMPMNLLGOEC.BKENJPNKMGI="player";Call("AfterRecruitInvestment",c,false);Check(d.AIMLPNBOIIF.Count==0,"manually managed city excluded");
  Setup();Add("market");Call("AfterRecruitInvestment",c,true);Check(d.AIMLPNBOIIF.Count==0,"private recruitment update excluded");
  ((BepInEx.Configuration.ConfigEntry<int>)typeof(StrategicSupport).GetField("buildsPerCity",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Value=2;
  Call("AfterRecruitInvestment",c,false);Check(d.AIMLPNBOIIF.Count>0&&d.AIMLPNBOIIF.Count<=2,"explicit two-building user limit retained");Check(f.ANMMMOIKLEG["gov"].ResearchCalls==1,"native research reconsidered");
  Call("AfterRecruitInvestment",c,false);Check(f.ANMMMOIKLEG["gov"].ResearchCalls==1,"no repeated research on same tick");
  Setup();Set("researchOwner",f);f.DNOGPFHMOMO=30500;
  var list=new Il2CppSystem.Collections.Generic.List<FBLFDEEGJMH>{new(){Cost=600},new(){Cost=400}};object[] args={f,list,null};
  Check((bool)Call("FilterResearchBudget",args)&&((Il2CppSystem.Collections.Generic.List<FBLFDEEGJMH>)args[1]).Count==1,"research candidates honor reserve");
  f.DNOGPFHMOMO=30000;args=new object[]{f,list,null};Check(!(bool)Call("FilterResearchBudget",args)&&args[2]==null,"empty research list safely stops selection");Set("researchOwner",null);
  // Regression: the reported 137,500 treasury could not fund even a cheap
  // building after the old 100k + 25% + 50,001 city-reserve combination.
  Check(!InvestmentPolicy.CanPurchase(109375,0,600,100000,50001),"Old usable 9375 cannot bridge old local reserve");
  Setup();Add("market");f.DNOGPFHMOMO=137500;c.LMPMNLLGOEC.DNOGPFHMOMO=0;
  c.LMPMNLLGOEC.KJPPGIOCDID.Clear();
  Call("AfterGovernorEconomy",f.ANMMMOIKLEG["gov"]);
  Check(d.AIMLPNBOIIF.Count>0,"137500 treasury and empty garrison still fund construction without recruitment callback");
  Check(f.ANMMMOIKLEG["gov"].ResearchCalls==1,"Independent governor visit evaluates research");
  Check(f.DNOGPFHMOMO>=10000&&c.LMPMNLLGOEC.DNOGPFHMOMO>=10000,"Keep emergency and operating reserves");
  Check(f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO==137500-d.AIMLPNBOIIF.Count*600,"Construction transfers conserve money and charge actual cost");
  Setup();Add("market");c.Player=true;c.OPNKGGKHAGJ=true;c.LMPMNLLGOEC.BKENJPNKMGI="player";
  Call("AfterRecruitInvestment",c,false);Check(d.AIMLPNBOIIF.Count>0,"Player mayor under delegated governor and explicit city automation can invest");
  Setup();Add("market");f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;
  Call("AfterPlan",f.ANMMMOIKLEG["gov"]);Check(d.AIMLPNBOIIF.Count>0,"Peace policy does not suppress economic work");
  Setup();Add("market");f.ANMMMOIKLEG["gov"].ThrowResearch=true;
  Call("AfterRecruitInvestment",c,false);Check(d.AIMLPNBOIIF.Count>0,"Research failure does not cancel construction");
  var rng=new Random(234);
  for(int i=0;i<100000;i++){int treasury=rng.Next(int.MaxValue),cash=rng.Next(int.MaxValue),price=rng.Next(1000000),r=rng.Next(1000000),cr=rng.Next(1000000);int transfer=InvestmentPolicy.TransferForPurchase(treasury,cash,price,r,cr);
    if(transfer<0||(long)cash+transfer>int.MaxValue||transfer>Math.Max(0L,(long)treasury-r))throw new Exception("transfer invariant");
    if(InvestmentPolicy.CanPurchase(treasury,cash,price,r,cr)&&(long)cash+transfer-price<cr)throw new Exception("purchase invariant");
  }
  Console.WriteLine($"PASS {tests} investment integration checks + 100000 budget invariants; native-call stubs, not gameplay");
 }
}




public class CMHNADEDJIE:Native {public string OJPECMNINNA;public int NGBNIPKFPOE;}
public partial class MKMOJNAJKJE {public Il2CppSystem.Collections.Generic.List<CMHNADEDJIE> CNBLOJCGNFD=new(),EHGOOJLIHGH=new(),OILNHBFCJII=new(),FCJHJPANFJN=new(),OHDEBNFBGIE=new(),PCGHDEOMPBO=new(),OOACPLDPNHM=new(),ONEHAJPKPMP=new(),CJDLOOOEGPI=new(),EFHPAEBIGJK=new(),HFCLLCIPNBB=new();}


