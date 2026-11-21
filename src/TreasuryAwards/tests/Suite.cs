using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
namespace Il2CppSystem.Collections.Generic { public class List<T> : System.Collections.Generic.List<T>{} }
public partial class CLAGKDECFID {
 public int Pointer,stat; public string OJPECMNINNA;public Story LMPMNLLGOEC=new();
 public int GGOIKBIJNCJ()=>stat;public int force=-1,wise=-1;public int JLAMCIKNBON()=>force<0?stat:force;public int GNCHNFHKBKE()=>wise<0?stat:wise;
 public int DAEKNKAKHDA()=>stat;public int DHOMPHMNCJB()=>stat;public int IHMMCAALFKG()=>stat;
 public string[] queue={"","",""};public bool instant=true;public List<string> given=new();
}
public class Story{public string LMMNKIOAGLC;public string[] ONOGINEKIJH=Array.Empty<string>();}
public class NEINKOOBJIH{public static CLAGKDECFID POMNMBGDDIN;}
public class AFAGFHJONFJ{ public Il2CppSystem.Collections.Generic.List<CLAGKDECFID> list=new();public Il2CppSystem.Collections.Generic.List<CLAGKDECFID> EOMOIINOEMB(int a,params bool[] b)=>list;}
public partial class MKMOJNAJKJE{public string IBGCLCAIBOL="mine";public List<AFAGFHJONFJ> cities=new();public List<AFAGFHJONFJ> PJMLPLDPEIM()=>cities;}
public class MLGOLGGHLMB{public static Dictionary<string,int> CBEGDDKANAO=new();}
public class UI_HeroListInfoUI{public static Il2CppSystem.Collections.Generic.List<CLAGKDECFID> PDPLMJHLKJF=new();}
public enum NGPAOHKHFPE{Weapon,Armor,Treasure,Horse,Helmet,Mission,Present,Shield}
public class Percent{public int Value=50;}
public class KKDFJCCMGHK{public NGPAOHKHFPE CIJIOJJFBEI;public int EPKCKLBHPGG;public int[] MEICJBFJGHD=new int[6];
 public string HDECJIENHCK,IGNLOFFCDEL,JJDNNGAOMBC;
 public float GBGEPLKBHJB,PDMFOJGPKDM;
 public int LDDKGCHHEKI,BGAENDAPBPM,KBABLAPAIKM,MKEIPLCMANH,BCAIBCEODLJ,EAEEGEBAEAB,FJDBEOJCFFM,EAEJLOCCOED,OGCFNICFNPG,AGPOCEIHAJL,JKFDDGBCJCK,NLPMMLAOPEC,JEDLCIKNNHM,FIGHLFPNIPF,CEAINOFHOBH,IPDLJPNILCE,JOKONPKJEKN,LIDKLOELDOG,PCCCMFNNAAF;}
public class Drug{public int IJGILOJIOAM,GDNLOKFFHDD;}

public class AEHAGJAKNEK{public static Dictionary<string,KKDFJCCMGHK> GHNDPFCGKDJ=new();public static Dictionary<string,Drug> EGFJHINFDCA=new();}
public partial class CMHNADEDJIE{public int NGBNIPKFPOE,grade,price;public string OJPECMNINNA;public int ABLAHGCONGN(float f)=>(int)(price*f);public int MIOJGPKDBHL()=>grade;}
public partial class Go{public bool activeSelf;}
public partial class UI_RewardUI{ public static bool Virtualized106,BypassGuard106;public int Pointer=1;public int[] FBFDDKOLPEI={0,0,0,0};public List<object>[] NNIIMHLKFME={new(),new(),new(),new()};
 public static Il2CppSystem.Collections.Generic.List<CLAGKDECFID> ABJNKABKKJM=new();
 public static Il2CppSystem.Collections.Generic.List<CMHNADEDJIE>[] m_listBagItem={new()};public static MKMOJNAJKJE m_playerPower=new();
 public static CLAGKDECFID m_curHero;public static string[] m_rewards={"","","",""};public static int m_viewType=1,m_curPage=2;
 public Go m_instantReward=new();
 public void ButtonSwitchViewPress(int n){m_viewType=n;}public void PagePress(int n){m_curPage=n;}
 public void IBPPAOHIDKL(){}public void KNIEFDGIBHF(){}public void HAKFBKBCENB(bool b){}
 public void NLIIBFOLDBD(){Array.Copy(m_curHero.queue,0,m_rewards,1,3);m_instantReward.activeSelf=m_curHero.instant;}
 public void PIAEJBLCJGP(){if(!string.IsNullOrEmpty(m_rewards[0])){m_curHero.given.Add(m_rewards[0]);m_curHero.instant=false;}}
 public bool FinishDrag(int a,int row,int b,int slot){
 if(a!=0||b!=3||m_curPage!=0||m_viewType!=0||m_rewards[slot]!=""||(slot==0&&!m_instantReward.activeSelf))throw new Exception("invalid drag");
 var r=m_listBagItem[a][row];m_rewards[slot]=r.OJPECMNINNA;if(slot>0)m_curHero.queue[slot-1]=r.OJPECMNINNA;
 if(--r.NGBNIPKFPOE==0)m_listBagItem[a].RemoveAt(row);
 if(Virtualized106&&r.NGBNIPKFPOE>0){
  bool render=BypassGuard106||(bool)typeof(TreasuryAwards).GetMethod("BeforeBulkShowBox",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,new object[]{this,row,0,row});
  int local=row-FBFDDKOLPEI[0];if(render&&local<NNIIMHLKFME[0].Count){var box=NNIIMHLKFME[0][local];}
 }
 NLIIBFOLDBD();return true;
 }}
public class Suite{
 static string Run(UI_RewardUI u)=>(string)typeof(TreasuryAwards).GetMethod("Run",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{u});
 static void Ok(bool x,string s){if(!x)throw new Exception(s);}
 static CLAGKDECFID Hero(int n,string power="mine")=>new(){Pointer=n,stat=n,OJPECMNINNA=n.ToString(),LMPMNLLGOEC=new(){LMMNKIOAGLC=power}};
 static void Item(string id,int rank,int price,int count,NGPAOHKHFPE? type=null){
 if(type.HasValue)AEHAGJAKNEK.GHNDPFCGKDJ[id]=new(){CIJIOJJFBEI=type.Value};
 UI_RewardUI.m_listBagItem[0].Add(new(){NGBNIPKFPOE=count,OJPECMNINNA=id,grade=rank,price=price});}
 static int Main(){try{RunSuite();return 0;}catch(Exception e){Console.WriteLine(e);return 1;}}
 static void RunSuite(){
 var u=new UI_RewardUI();var p=Hero(100);var a=Hero(60);var b=Hero(50);var c=Hero(40);var d=Hero(30);var e=Hero(20);var enemy=Hero(150,"other");
 NEINKOOBJIH.POMNMBGDDIN=p; UI_RewardUI.m_playerPower.cities.Add(new());UI_RewardUI.m_playerPower.cities[0].list.AddRange(new[]{p,a,b,c,d,e,enemy});
 UI_RewardUI.ABJNKABKKJM.Add(d);UI_RewardUI.m_curHero=d;UI_RewardUI.m_rewards[0]="manual";
 Item("rareWeapon",3,10,99);Item("consumable",0,900,1);Item("horse",1,800,1);Item("book",2,800,1);Item("title",0,1000,1);
 a.queue[0]="booked";b.instant=false;
 string first=Run(u);
 Ok(a.given.SequenceEqual(new[]{"title"}) && a.queue.SequenceEqual(new[]{"booked","horse","rareWeapon"}),"unit price first, rarity tiebreak, occupied slot");
 Ok(b.queue[0]=="consumable" && b.queue[1]=="rareWeapon","horse and lower-value equipment");
 Ok(d.given.Count+e.given.Count+p.given.Count+enemy.given.Count==0,"exclusion");Ok(UI_RewardUI.m_curHero==d&&UI_RewardUI.m_rewards[0]=="manual"&&UI_RewardUI.m_curPage==2&&UI_RewardUI.m_viewType==1,"restore");
 // Only usable stock runs dry. Start a clean cycle with one eligible stack to verify save-backed resume.
 b.queue=new[]{"","",""};c.queue=new[]{"","",""};c.instant=true;c.given.Clear();UI_RewardUI.m_listBagItem[0].Clear();MLGOLGGHLMB.CBEGDDKANAO.Clear();a.instant=true; a.queue=new[]{"","",""};Item("supply",1,5,1);
 Run(u);Ok(a.given.Last()=="supply"&& !MLGOLGGHLMB.CBEGDDKANAO.ContainsKey("local.heroesvow.treasuryawards/v1/mine/cycle"),"pause");
 Item("next",1,5,14);Run(u);Ok(b.queue.All(i=>i=="next") && c.given.SequenceEqual(new[]{"next"})&&c.queue.All(i=>i=="next"),"resume and odd upper half"); Ok(a.queue.All(i=>i=="next"),"later rounds may return to highest after others get a turn");
 Ok(d.queue.All(i=>i=="")&&e.queue.All(i=>i==""),"bottom half");
 var flags=MLGOLGGHLMB.CBEGDDKANAO;const string k="local.heroesvow.treasuryawards/v1/mine/";
 flags[k+"epoch/60"]=2;flags[k+"count/60"]=19;a.instant=true;a.queue=new[]{"","",""};Run(u);Ok(flags[k+"count/60"]==20&&a.queue.All(i=>i==""),"cap");
 // Different IDs and stacks with the same wearing slot share a limit per officer.
 flags.Clear();UI_RewardUI.m_listBagItem[0].Clear();
 foreach(var hero in new[]{a,b,c}){hero.queue=new[]{"","",""};hero.instant=true;hero.given.Clear();}
 Item("goldHat",4,900,10,NGPAOHKHFPE.Helmet);Item("silverHat",3,800,10,NGPAOHKHFPE.Helmet);
 Item("sword",3,700,10,NGPAOHKHFPE.Weapon);Item("spear",2,600,10,NGPAOHKHFPE.Weapon);
 Item("medicine",0,100,30);a.queue[1]="silverHat";
 Run(u);
 Ok(a.given.SequenceEqual(new[]{"sword"}) && a.queue.SequenceEqual(new[]{"medicine","silverHat","medicine"}),"manual helmet protected, weapon variants excluded, consumables repeat");
 foreach(var hero in new[]{b,c})Ok(hero.given.SequenceEqual(new[]{"goldHat"}) && hero.queue.SequenceEqual(new[]{"sword","medicine","medicine"}),"one hat and one weapon per recipient");
 // Resume after a saved grant, even after its scheduled slot has cleared.
 flags[k+"epoch/60"]=2;flags[k+"slots/60"]=1<<(int)NGPAOHKHFPE.Helmet;
 a.instant=true;a.queue=new[]{"","",""};Run(u);
 Ok(a.given.Last()=="sword" && a.queue.All(x=>x=="medicine"),"saved equipment mask");
 a.instant=true;a.queue=new[]{"","",""};Run(u);
 Ok(a.given.Last()=="goldHat" && a.queue.SequenceEqual(new[]{"sword","medicine","medicine"}),"new cycle resets mask");
 NewTests.Run(); AutomaticTests.Run(); RoundRobinTests.Run();TreasuryPerfTests.Run();Button106Tests.Run();
 Console.WriteLine("PASS: updated round-robin order, six-stat ranking, upper half, own faction only, all item categories, unit price before rarity, occupied/four slots, instant disabled, saved turn, 20 cap, manual pending and screen restored.");
 }}

