using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
namespace BepInEx {public class BepInPlugin:Attribute{public BepInPlugin(string a,string b,string c){}}}
namespace BepInEx {public static class Paths{public static string BepInExRootPath=>System.IO.Path.GetTempPath();}}
namespace BepInEx.Logging {public class ManualLogSource{public void LogInfo(object o){}public void LogWarning(object o){}}}
namespace BepInEx.Configuration {
 public class ConfigEntry<T>{public T Value;public ConfigEntry(T v){Value=v;}}
 public class AcceptableValueRange<T>{public AcceptableValueRange(T a,T b){}}
 public class ConfigDescription {public ConfigDescription(string s,object o){}}
 public class ConfigFile{public ConfigEntry<T> Bind<T>(string s,string n,T d,object x)=>new(d);}
}
namespace BepInEx.Unity.IL2CPP{public class BasePlugin{public BepInEx.Logging.ManualLogSource Log=new();public BepInEx.Configuration.ConfigFile Config=new();public T AddComponent<T>() where T:class=>null;public virtual void Load(){}}}
namespace HarmonyLib{
 public class HarmonyMethod{public HarmonyMethod(Type t,string s){}}
 public class Harmony{public Harmony(string s){}public void UnpatchSelf(){}public void Patch(object o,HarmonyMethod prefix=null,HarmonyMethod postfix=null,HarmonyMethod finalizer=null){}}
 public static class AccessTools{public static object Method(Type t,string s)=>null;}
}
namespace UnityEngine{
 public class MonoBehaviour{public MonoBehaviour(IntPtr p){}}
 public struct Vector3{public float x,y,z;public Vector3(float a,float b=0,float c=0){x=a;y=b;z=c;} public static Vector3 Lerp(Vector3 a,Vector3 b,float t)=>new(a.x+(b.x-a.x)*t,a.y+(b.y-a.y)*t,a.z+(b.z-a.z)*t);public float sqrMagnitude=>x*x+y*y+z*z;public static Vector3 operator -(Vector3 a,Vector3 b)=>new(a.x-b.x,a.y-b.y,a.z-b.z);public static float Distance(Vector3 a,Vector3 b)=>(float)Math.Sqrt((a-b).sqrMagnitude);}
 public class GameObject{public bool activeSelf;public bool activeInHierarchy=>activeSelf&&WorldVisible;public static bool WorldVisible;public void SetActive(bool b){activeSelf=b;}}
}
public class Native {static int serial;public IntPtr Pointer=(IntPtr)(++serial);}
public class NativeDict<K,V>:Dictionary<K,V>{static int n;public IntPtr Pointer=(IntPtr)(++n);}
public enum NEFBEOFPGJA{Auto,StopWar,AttackPower}
public enum BHBNKONCOJD{StopWar,ArmyPass,War,Ally}
public enum PMNFKDJNLDJ{None,Supplement,Train,Deploy,Transport}
public partial class FNFJODHIJFO:Native{public string LMMNKIOAGLC,LMAEGAJADKI;public NEFBEOFPGJA BNGFJOAJKDH;}
public enum GCPCLLMKIMM{None=100,Diplomacy=101,StartDiplomacy=102,EndDiplomacy=103,Scheme=111,StartScheme=112,EndScheme=113,CityMove=104,War=105,Follow=106,Technology=107,Recruit=108,StartRecruit=109,EndRecruit=110,PrivateMilitary=114}
public enum OEBGIDGEIIG{None,Manage,Military,Security,Patrol,Explore,WallEast,WallSouth,WallWest,WallNorth}
public partial class FEKJFDMPPCC{public List<CHBFLDOKEHC> ALGOEDPACKF=new();public DOHGPECAMJC JFKIANPNOJH;public GCPCLLMKIMM HFBAJIGHOAF=GCPCLLMKIMM.None;}
public class JLHIFLNDKPD{public OEBGIDGEIIG HFBAJIGHOAF;}
public partial class HeroRow{public string LMMNKIOAGLC="F",CNOMNBCPJDI;}
public partial class CLAGKDECFID:Native{
 public bool MGLFFEANDOI;public FEKJFDMPPCC OGPEJIEHDAN=new();public JLHIFLNDKPD BAPIJIJLLCA=new();
 public string OJPECMNINNA;public int HFBAJIGHOAF,CommandSlots=1;public int MJICLKFOAPI(bool official)=>CommandSlots;public bool GMILGFFJOIL,NativeEligible=true;public HeroRow LMPMNLLGOEC=new();
 public double[] Ability=Enumerable.Repeat(50d,14).ToArray();
 public int FKOIGKFHHKO(bool b)=>(int)Ability[0];public int FGCBICMIMOJ(bool b)=>(int)Ability[1];
 public int JNFEILANOGB(bool b,bool c)=>(int)Ability[2];public int IEGIBHMNKGO(bool b,bool c)=>(int)Ability[3];
 public int JIJBCAFMAKG(bool b,bool c)=>(int)Ability[4];public int KLNPHPCJPHP(bool b)=>(int)Ability[5];
 public int FDFDCEGNPGC(bool b)=>(int)Ability[6];public int IKIAIPPNKOF(bool b)=>(int)Ability[7];
 public int GGOIKBIJNCJ()=>(int)Ability[8];public int JLAMCIKNBON()=>(int)Ability[9];
 public int GNCHNFHKBKE()=>(int)Ability[10];public int DAEKNKAKHDA()=>(int)Ability[11];
 public int DHOMPHMNCJB()=>(int)Ability[12];public int IHMMCAALFKG()=>(int)Ability[13];
}
public class PPDKOMFGILG{public BHBNKONCOJD EBIOPDHEBOF;}
public partial class MKMOJNAJKJE:Native{public string IBGCLCAIBOL;public int DNOGPFHMOMO;public Dictionary<string,FNFJODHIJFO> ANMMMOIKLEG=new();public Dictionary<string,PPDKOMFGILG>AENJACOFBJK=new();}
public class HKBLIJKMPNJ{public float KCANKJJJNAC=500;public Dictionary<string,float> LGACDLFDIJG=new();public string LMMNKIOAGLC,IPJHOOJBCKI,BKENJPNKMGI;public int DNOGPFHMOMO,PNHICLAGHGD=10000,OEGNGFBCHFL=10000000;public Il2CppSystem.Collections.Generic.List<DHKINGPDKJJ>KJPPGIOCDID=new();}
public partial class DHKINGPDKJJ:Native{public string LMMNKIOAGLC;public string OLMAJLLIJDM="";public int OJPECMNINNA;public ANOPPJKAPIG AAKKDAPJMHC;public int CombatValue;public int GNCKPEPJDGF()=>CombatValue>0?CombatValue:FGHFOLKBHDL;public bool MGLFFEANDOI=true,MJNINDNBJHA=true;public int FGHFOLKBHDL=1000;public PMNFKDJNLDJ GDHPLIFJCBN;}
public static partial class EBNFFALGBPA{public static int[] AIEJCKPDMLE={1000,2000,3000,4000},LNBFFMONMCN={100000,200000,300000,400000};public static int LLLBAAAEIBB=2000;}
public static class NEINKOOBJIH{public static AFAGFHJONFJ JKGJNLHLJAJ;public static double MIPFHCKDJOK;public static CLAGKDECFID POMNMBGDDIN=new();public static string OPIKIHPNCIP="player";public static Dictionary<string,MKMOJNAJKJE>COCMABJFOFH=new();public static List<string> JMIADAFPGMC=new(),AOPJBNBGPLP=new();}
public static class MICGPAMNDKM{public static Dictionary<int,Ci_BuildingCtrl> DKDKOJNIGAJ=new();public static NativeDict<string,AFAGFHJONFJ> IJPKEMPIFIH=new();}
public static partial class ICGAEMLACJG{public static Dictionary<int,Wo_DeployTeamCtrl>HFJIECIBFKB=new();}
public partial class AFAGFHJONFJ:Native{public int HomeOfficerCount=5;public List<CLAGKDECFID> Roster;public List<CLAGKDECFID> ABJNKABKKJM=>Roster??Enumerable.Range(0,HomeOfficerCount).Select(i=>new CLAGKDECFID{OJPECMNINNA=i==0?"officer":"officer"+i,LMPMNLLGOEC=new HeroRow{CNOMNBCPJDI=OJPECMNINNA,LMMNKIOAGLC=LMPMNLLGOEC.LMMNKIOAGLC}}).ToList();
 public string OJPECMNINNA;public HKBLIJKMPNJ LMPMNLLGOEC=new();public bool OPNKGGKHAGJ=true,Player;public UnityEngine.Vector3 BMBMKPGEHBH;
 public List<AFAGFHJONFJ>FKMKGNPFLFP=new();public int Power=100,Calls;public bool Invalid,PathBlocked,ThrowGeneration,WrongGeneratedFaction,OwnershipChangesDuringGeneration;
 public bool AKIJOBLFLCJ(string id)=>Player||LMPMNLLGOEC.LMMNKIOAGLC==id||LMPMNLLGOEC.IPJHOOJBCKI==id||LMPMNLLGOEC.BKENJPNKMGI==id;public int Capacity=10;public int JNLIOIHLDHH()=>Capacity;public int LAGKJHBEPGF(bool p)=>1000;
 public int GNCKPEPJDGF(int a,bool b,float c,out int count){count=1;return Power;}
 public Wo_DeployTeamCtrl Last;public Action Generated;public Func<AFAGFHJONFJ,int,int,Il2CppSystem.Collections.Generic.List<CLAGKDECFID>,Wo_DeployTeamCtrl> AssemblyModel;
 public Wo_DeployTeamCtrl JLDJEOJJLDM(int leave,int power,int food,bool force,bool response){
  if(ThrowGeneration)throw new InvalidOperationException("injected native fault");
  var eligible=new Il2CppSystem.Collections.Generic.List<CLAGKDECFID>();foreach(var h in ABJNKABKKJM)if(h.NativeEligible)eligible.Add(h);
  object[] filter={this,eligible};typeof(StrategicSupport).GetMethod("FilterBalanceOfficer",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,filter);
  eligible=(Il2CppSystem.Collections.Generic.List<CLAGKDECFID>)filter[1];if(eligible.Count==0)return null;
  if(AssemblyModel!=null)return Last=AssemblyModel(this,leave,power,eligible);
  Calls++;var t=new Wo_DeployTeamCtrl{m_belongPower=LMPMNLLGOEC.LMMNKIOAGLC,m_assembleCity=OJPECMNINNA,Blocked=PathBlocked,Position=BMBMKPGEHBH};t.m_id=(int)t.Pointer;ICGAEMLACJG.HFJIECIBFKB[t.m_id]=t;
  t.m_listHero.Add(Invalid?new CLAGKDECFID{OJPECMNINNA="retainer",LMPMNLLGOEC=new HeroRow{CNOMNBCPJDI=OJPECMNINNA,LMMNKIOAGLC=LMPMNLLGOEC.LMMNKIOAGLC}}:eligible[0]);
  t.m_dicDeployArmy[t.m_listHero[0].OJPECMNINNA]=LMPMNLLGOEC.KJPPGIOCDID.Where(a=>a.FGHFOLKBHDL>=100&&a.FGHFOLKBHDL>EBNFFALGBPA.LLLBAAAEIBB*.4).Take(LMPMNLLGOEC.KJPPGIOCDID.Count-leave).ToList();Last=t;
  Generated?.Invoke();
  if(WrongGeneratedFaction)t.m_belongPower="other";
  if(OwnershipChangesDuringGeneration)LMPMNLLGOEC.LMMNKIOAGLC="other";
  return t;
 }
}
public partial class Wo_DeployTeamCtrl:Native{
 public static implicit operator bool(Wo_DeployTeamCtrl t)=>t!=null;
 public bool m_responseToPlayer;public bool m_isPrivate,m_isEnterCity,Dismissed,Blocked;public int m_id;public string m_targetCity,m_belongPower,m_assembleCity;
 public UnityEngine.GameObject gameObject=new();public List<CLAGKDECFID>m_listHero=new();public Dictionary<string,List<DHKINGPDKJJ>>m_dicDeployArmy=new();
 public int Combat=10000;public int GetCombatVal()=>Combat;public bool IncludePlayer()=>false;
 public int GetSoldierNum(bool b)=>m_dicDeployArmy.Values.SelectMany(x=>x).Sum(x=>x.FGHFOLKBHDL);
 public bool IsPassEnemyCity(UnityEngine.Vector3 v)=>Blocked;public bool ReturnUnitsOnDismiss;
 public void Dismiss(bool a,bool b){Dismissed=true;if(ReturnUnitsOnDismiss&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(m_assembleCity,out var city))foreach(var list in m_dicDeployArmy.Values)foreach(var unit in list)city.LMPMNLLGOEC.KJPPGIOCDID.Add(unit);ICGAEMLACJG.HFJIECIBFKB.Remove(m_id);}
}
public class IntegrationTests{
 static int tests;
 static void Check(bool b,string s){tests++;if(!b)throw new Exception(s);}
 static object Invoke(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,args);
 static AFAGFHJONFJ source,target;static MKMOJNAJKJE faction;static FNFJODHIJFO governor;
 static void Setup(){
  MICGPAMNDKM.IJPKEMPIFIH=new();ICGAEMLACJG.HFJIECIBFKB.Clear();NEINKOOBJIH.COCMABJFOFH.Clear();NEINKOOBJIH.JMIADAFPGMC.Clear();NEINKOOBJIH.AOPJBNBGPLP.Clear();
  faction=new(){IBGCLCAIBOL="F",DNOGPFHMOMO=1000000};governor=new(){LMMNKIOAGLC="F"};
 }
 public static void Main(){
  new StrategicSupport().Load();
  Prepare();Invoke("BeforeRecruit",source,true);Check(source.LMPMNLLGOEC.DNOGPFHMOMO==0,"private recruitment excluded");
  Invoke("BeforeRecruit",source,false);Check(source.LMPMNLLGOEC.DNOGPFHMOMO==3000&&faction.DNOGPFHMOMO==997000,"balanced native public funding");
  Prepare();source.Player=true;source.OPNKGGKHAGJ=false;source.LMPMNLLGOEC.BKENJPNKMGI="player";Invoke("BeforeRecruit",source,false);Check(source.LMPMNLLGOEC.DNOGPFHMOMO==0,"manual player city excluded");
  Prepare();source.Player=true;source.OPNKGGKHAGJ=false;Invoke("BeforeRecruit",source,false);Check(source.LMPMNLLGOEC.DNOGPFHMOMO==3000,"delegated player-faction city receives recruitment funding");
  Prepare();source.Player=true;source.OPNKGGKHAGJ=false;Invoke("AfterPlan",governor);Check(source.Calls==1,"delegated player-faction city can send guarded border support");
  Prepare();source.LMPMNLLGOEC.IPJHOOJBCKI="player";Invoke("BeforeRecruit",source,false);Check(source.LMPMNLLGOEC.DNOGPFHMOMO==0,"personal district excluded");
  Prepare();Invoke("BeforeStaff",source);Check(source.LMPMNLLGOEC.DNOGPFHMOMO==0,"manual staffing UI not funded");
  Prepare();Invoke("AfterPlan",governor);Check(source.Calls==1&&source.Last.m_targetCity=="target"&&source.Last.gameObject.activeSelf,"cross-district support queued");
  Invoke("AfterPlan",governor);Check(source.Calls==1,"repeat plan throttled");
  Prepare();source.Player=true;source.OPNKGGKHAGJ=false;source.LMPMNLLGOEC.BKENJPNKMGI="player";Invoke("AfterPlan",governor);Check(source.Calls==1,"manual city can send faction help");
  Prepare();source.Invalid=true;NEINKOOBJIH.AOPJBNBGPLP.Add("retainer");Invoke("AfterPlan",governor);Check(source.Last.Dismissed&&!source.Last.gameObject.activeSelf,"retainer reservation rejected");
  Prepare();source.PathBlocked=true;Invoke("AfterPlan",governor);Check(!source.Last.Dismissed,"obsolete native direct-line veto does not reject validated road");
  Prepare();source.Generated=()=>UnityEngine.AI.NavMesh.Blocked=true;Invoke("AfterPlan",governor);Check(source.Last.Dismissed,"physical route blocked during assembly rejects departure");UnityEngine.AI.NavMesh.Blocked=false;
  Prepare();source.LMPMNLLGOEC.KJPPGIOCDID[0].GDHPLIFJCBN=PMNFKDJNLDJ.Deploy;Invoke("AfterPlan",governor);Check(source.Calls==1&&!source.Last.m_dicDeployArmy.Values.SelectMany(x=>x).Contains(source.LMPMNLLGOEC.KJPPGIOCDID[0]),"busy unit excluded while idle units support");
  Prepare();ICGAEMLACJG.HFJIECIBFKB[2]=new(){m_belongPower="F",m_targetCity="target",Combat=20000};Invoke("AfterPlan",governor);Check(source.Calls==0,"incoming help prevents oversupply");
  Prepare();faction.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.Ally;Invoke("AfterPlan",governor);Check(source.Calls==0,"allied visitors not threats");
  Prepare();source.LMPMNLLGOEC.OEGNGFBCHFL=0;Invoke("AfterPlan",governor);Check(source.Calls==0,"no food no departure");
  Console.WriteLine($"PASS {tests} prior integration scenarios using native-call stubs; not gameplay");
  BattleIntegrationTests.Run(); CityIntegrationTests.Run();
 }
 static void Prepare(){
  MICGPAMNDKM.IJPKEMPIFIH=new();ICGAEMLACJG.HFJIECIBFKB.Clear();NEINKOOBJIH.COCMABJFOFH.Clear();NEINKOOBJIH.JMIADAFPGMC.Clear();NEINKOOBJIH.AOPJBNBGPLP.Clear();
  faction=new(){IBGCLCAIBOL="F",DNOGPFHMOMO=1000000};governor=new(){LMMNKIOAGLC="F"};
  faction.ANMMMOIKLEG["gov"]=governor;faction.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};NEINKOOBJIH.COCMABJFOFH["F"]=faction;
  source=new(){OJPECMNINNA="source"};target=new(){OJPECMNINNA="target"};
  source.LMPMNLLGOEC.LMMNKIOAGLC=target.LMPMNLLGOEC.LMMNKIOAGLC="F";
  source.LMPMNLLGOEC.IPJHOOJBCKI="gov";target.LMPMNLLGOEC.IPJHOOJBCKI="otherGov";
  for(int i=0;i<10;i++)source.LMPMNLLGOEC.KJPPGIOCDID.Add(new());
  target.FKMKGNPFLFP.Add(source);source.FKMKGNPFLFP.Add(target);
  MICGPAMNDKM.IJPKEMPIFIH["source"]=source;MICGPAMNDKM.IJPKEMPIFIH["target"]=target;
  ICGAEMLACJG.HFJIECIBFKB[1]=new(){m_targetCity="target",m_belongPower="enemy",Position=new UnityEngine.Vector3(100),m_deployState=AKOPKFPKJPP.Expedition};ICGAEMLACJG.HFJIECIBFKB[1].m_listMovePos.Add(target.BMBMKPGEHBH);
 }
}









public class FPIIMMKPEOM:Native {public string OJPECMNINNA;}
public partial class Wo_DeployTeamCtrl {public Wo_DeployTeamCtrl m_responseTeam,m_responseTeamAI;public bool m_isInstantTeam;public FPIIMMKPEOM m_bma;public void ClearTarget(bool b){m_attackTarget=null;m_mergeTarget=null;if(!b)m_targetCity="";m_isEnterCity=false;m_targetVillage=null;m_targetPass=null;m_targetWarInfo=null;}public void SetAttackTarget(Wo_DeployTeamCtrl t,bool b){ClearTarget(b);m_attackTarget=t;m_deployState=AKOPKFPKJPP.Expedition;}}

public static class SettingCtrl {public static bool m_isZhengShi;}
