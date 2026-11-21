using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.AI;
public static class Decision93Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int count;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static object Fixture(Type t,string n,params object[] a)=>t.GetMethod(n,F).Invoke(null,a);
 static void Check(bool ok,string why){count++;if(!ok)throw new Exception("Decision93: "+why);}
 static void Drain(string n,params object[] a){int steps=0;foreach(var phase in (IEnumerable<string>)Call(n,a))if(++steps>20000)throw new Exception("unbounded "+n);}
 static void Setup(){Fixture(typeof(Relief86Tests),"Setup");((IDictionary)typeof(StrategicSupport).GetField("threatRoads",F).GetValue(null)).Clear();}
 static AFAGFHJONFJ City(string id,float x)=>(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City",id,x);
 static List<Wo_DeployTeamCtrl> Teams()=>ICGAEMLACJG.HFJIECIBFKB.Values.ToList();
 static Wo_DeployTeamCtrl Send(AFAGFHJONFJ s,AFAGFHJONFJ d,MKMOJNAJKJE f)=>(Wo_DeployTeamCtrl)Call("GenerateMarchRelief",s,d,f,999999,30d,Teams(),Call("AttackReserved",Teams()));
 public static void Run(){
  Setup();var f=NEINKOOBJIH.COCMABJFOFH["F"];var city=City("Lujiang",0);var shield=City("friendly-screen",10);var enemy=City("enemy-road",20);enemy.LMPMNLLGOEC.LMMNKIOAGLC="enemy";city.FKMKGNPFLFP.Add(enemy);enemy.MEKIDAONKMD[city.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){shield}};
  Check((long)Call("BorderThreat",city,f,Teams())==0,"enemy behind a native friendly intermediate is not direct home threat");
  object[] args={city,f,Teams(),null};var waves=(List<TimedBorderDefense.Wave>)Call("CounterattackWaves",args);Check(waves.Count==0&&((string)args[3]).Contains("screened-by:friendly-screen"),"screening reason retained in timing diagnostic");
  shield.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check((long)Call("BorderThreat",city,f,Teams())>0,"capture of screening city immediately restores actual threat");
  NavMesh.Detour=50;((IDictionary)typeof(StrategicSupport).GetField("threatRoads",F).GetValue(null)).Clear();args=new object[]{city,f,Teams(),null};waves=(List<TimedBorderDefense.Wave>)Call("CounterattackWaves",args);Check(waves.Count==1&&waves[0].At>50,"counterattack ETA uses physical road detour instead of straight distance");
  Setup();f=NEINKOOBJIH.COCMABJFOFH["F"];city=City("Lujiang",0);var front=MICGPAMNDKM.IJPKEMPIFIH["front"];for(int i=0;i<3;i++)city.LMPMNLLGOEC.KJPPGIOCDID.Add(new());Fixture(typeof(Relief86Tests),"Expose",city,1000f);
  Check(Call("GenerateSupport",city,999999,Teams(),Call("AttackReserved",Teams()))==null,"reproduce thirteen-unit source blocked by absolute border sum");
  var sent=Send(city,front,f);Check(sent!=null&&sent.GetSoldierNum(false)>0,"confirmed incoming attack permits close first wave before battle begins");
  Check(Send(city,front,f)==null,"repeated urgent wave cannot recursively drain baseline reserve");
  Call("BackfillAfterCommit",city,front,f);Check(((WorkQueue)typeof(StrategicSupport).GetField("work",F).GetValue(null)).Count>0,"successful first wave immediately schedules rear replenishment assessment");
  ICGAEMLACJG.HFJIECIBFKB[901]=new(){m_id=901,m_belongPower="enemy",m_targetCity=city.OJPECMNINNA};Check(Send(city,front,f)==null,"actual redirected attack at donor blocks further export");
  Setup();f=NEINKOOBJIH.COCMABJFOFH["F"];city=City("A",0);front=MICGPAMNDKM.IJPKEMPIFIH["front"];var rear=City("rear",10);Fixture(typeof(Relief86Tests),"Link",city,rear);Fixture(typeof(Relief86Tests),"Expose",city,1000f);
  Drain("RegionalCoverSteps",city,front,new HashSet<string>());Check(((IDictionary)typeof(StrategicSupport).GetField("coverOrders",F).GetValue(null)).Count==1,"real rear order exists before any loan credit");
  sent=Send(city,front,f);Check(sent!=null&&sent.GetSoldierNum(false)==1000,"urgent branch now consumes real timely rear cover with no phantom troops");
  rear.Last.m_targetCity="manual";Check(Send(city,front,f)==null|| (long)Call("HomeSoldiers",city,Teams())>=3500,"cancelled cover cannot lend below persistent physical floor");
  Setup();f=NEINKOOBJIH.COCMABJFOFH["F"];var target=City("target",0);var near=City("looks-near",5);var far=City("looks-far",20);var via=City("long-road",100);near.MEKIDAONKMD[target.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){via}};AOHIBKLOAEI.RegionAt=p=>p.x>90?via:p.x>10?far:p.x>2?near:target;
  var ranked=new List<(AFAGFHJONFJ City,double Length)>();Drain("RankCityRoutes",new[]{near,far},target,f,true,ranked);Check(ranked.Count==2&&ranked[0].City==far&&ranked[0].Length==20&&ranked[1].Length==195,"all city priorities rank real road length, not map proximity");
  var army=new Wo_DeployTeamCtrl{m_belongPower="F",Position=new(0),m_deployState=AKOPKFPKJPP.Expedition};army.m_listMovePos.Add(new(5));NavMesh.Detour=30;Check((double)Call("ArmyRemainingRoad",army,new Vector3(10))==70,"partial army route resolves actual remaining detour");NavMesh.Blocked=true;Check(double.IsPositiveInfinity((double)Call("ArmyRemainingRoad",army,new Vector3(10))),"unreachable tail cannot create fictional straight-line arrival");NavMesh.Blocked=false;NavMesh.Detour=0;AOHIBKLOAEI.RegionAt=null;
  Missions();Call("ResetWork");Call("ResetDefense");Console.WriteLine($"PASS Decision93 {count} real-road ranking, screened threat, urgent relay and native rescue/raid checks; native actions mocked, not gameplay.");
 }
 static void Missions(){
  Fixture(typeof(StatecraftTests),"Setup");var f=NEINKOOBJIH.COCMABJFOFH["F"];var cities=f.PJMLPLDPEIM().ToArray().ToList();var home=cities[0];var prison=new AFAGFHJONFJ{OJPECMNINNA="prison"};prison.LMPMNLLGOEC.LMMNKIOAGLC="enemy";MICGPAMNDKM.IJPKEMPIFIH[prison.OJPECMNINNA]=prison;f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};home.FKMKGNPFLFP.Add(prison);
  var captive=new CLAGKDECFID{OJPECMNINNA="captured-ally",GMILGFFJOIL=true,LMPMNLLGOEC=new(){LMMNKIOAGLC="F",CNOMNBCPJDI=home.OJPECMNINNA}};prison.JHNBHLPJKMO.Add(captive);
  var pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Call("StateOfficers",cities,f);
  Drain("ActiveSchemeSteps",f,cities,pool,new List<CLAGKDECFID>(),true);var rescue=home.Roster.SingleOrDefault(h=>h.OGPEJIEHDAN.JFKIANPNOJH==DOHGPECAMJC.Rescue&&h.MissionCalls==1);Check(rescue!=null&&rescue.Target==prison.OJPECMNINNA&&rescue.OGPEJIEHDAN.LKEPGHJDHGG==captive.OJPECMNINNA,"native Rescue uses actual prison city, not captive former home");
  Check((bool)Call("CaptiveMissionPending",new List<CLAGKDECFID>{rescue},captive.OJPECMNINNA),"rescue prevents duplicate ransom");
  var ransom=new CLAGKDECFID();ransom.FHEMHGIGNDE("enemy",new(){HFBAJIGHOAF=IPNKPODNAIN.ReturnCaptive,BOEPOCFEHKG=captive.OJPECMNINNA});Check((bool)Call("CaptiveMissionPending",new List<CLAGKDECFID>{ransom},captive.OJPECMNINNA),"native ransom proposal prevents duplicate rescue after reload");
  var victim=new CLAGKDECFID{OJPECMNINNA="enemy-officer",LMPMNLLGOEC=new(){LMMNKIOAGLC="enemy",CNOMNBCPJDI=prison.OJPECMNINNA}};prison.Roster=new(){victim};pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Call("StateOfficers",cities,f);
  Drain("ActiveSchemeSteps",f,cities,pool,(List<CLAGKDECFID>)Call("ActiveMissions",f),false);Check(home.Roster.Any(h=>h.MissionCalls==1&&h.OGPEJIEHDAN.JFKIANPNOJH==DOHGPECAMJC.Murder&&h.OGPEJIEHDAN.LKEPGHJDHGG==victim.OJPECMNINNA),"native raid travels to eligible hostile officer without forcing outcome");
  victim.GMILGFFJOIL=true;Check(!(bool)Call("SchemeTargetPresent",prison,victim,f,DOHGPECAMJC.Murder),"already captured enemy is not a raid target");
  Check(StatecraftPolicy.MissionSlots(3,0,0,false)==0&&StatecraftPolicy.MissionSlots(6,0,0,true)==0,"rear and border retain minimum local staff");
  Check(StatecraftPolicy.MissionSlots(24,0,0,false)==4&&StatecraftPolicy.MissionSlots(20,0,4,false)==0,"more active missions still enforce shared cap");
 }
}
