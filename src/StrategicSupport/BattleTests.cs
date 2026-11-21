using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace UnityEngine{public static class Time{public static float timeScale=1,deltaTime=1,unscaledDeltaTime=1;}}
public enum AKOPKFPKJPP{Assemble,Expedition,GoForCamp,Camp,Battle,VillageSuppress,PassSuppress}
public static partial class AOHIBKLOAEI{public static float NIOKFCDLGFK()=>2;}
public static class GLIJMELMEHI{public static int OHJCDAPNAOI=1,FKNPIBLCEDK=1;public static bool BEMGDNDJFMF,MIHOJGEPPID;}
public static partial class AEHAGJAKNEK{public static bool PAKENCLGOHK;public static float IAICEJLNKFJ=1;}
public partial class NJHDEHLHENN:Native{
 public string HIENOINGBCN="war";public string[] EMMJBPMNFAK={"F","enemy"};public List<Wo_DeployTeamCtrl>[] LGPMOKOLMLN={new(),new()};
 public List<DHKINGPDKJJ>[] KJPPGIOCDID={new(),new()},GPFFMNKOFCH={new(),new()};public int[] FGHFOLKBHDL={10000,20000};
 public Vector3 GBHGENNMJCK=new(100);public AFAGFHJONFJ IJGLCFJOOIK;public int MLFBHOOGBNA()=>0;
}
public static class Wa_Controller{
 public static NJHDEHLHENN m_warInfo;public static bool m_isQuiting,m_isBeforeWar,m_isByEvent,m_isRoundMode;
 public static object m_mockInfo,m_battleUI;public static int FoodRefreshes;public static void RefreshFoodUI(){FoodRefreshes++;}public static float m_battleSpeed=1;public static int m_myCurSoldierNum=10000,m_enemyCurSoldierNum=20000;
}
public partial class Wo_DeployTeamCtrl{
 public NJHDEHLHENN m_warInfo,m_targetWarInfo;public bool m_isStarting;public AKOPKFPKJPP m_deployState;
 public int m_targetFoodDay;
 public List<Vector3> m_listMovePos=new();public Vector3 Position;public float m_curSpeed=2;public int Starts,Moves;public Action Arrive;
 public Vector3 GetPosition()=>Position;public float GetMoveSpeed()=>Speed105;public static float Speed105=2;public static float GetMoveSpeed(string f,bool p)=>Speed105;public int GetLeftAssembleDay()=>0;
 public void StartDeploy(){Starts++;m_deployState=AKOPKFPKJPP.Expedition;m_listMovePos.Add(m_targetWarInfo.GBHGENNMJCK);
  if(MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(m_assembleCity,out var city))foreach(var list in m_dicDeployArmy.Values)foreach(var a in list)city.LMPMNLLGOEC.KJPPGIOCDID.Remove(a);
 }
 public void UpdateMove(){Moves++;Position=new(Position.x+m_curSpeed*Time.deltaTime);if(Position.x>=m_targetWarInfo.GBHGENNMJCK.x){m_warInfo=m_targetWarInfo;m_targetWarInfo=null;m_listMovePos.Clear();Arrive?.Invoke();}}
}
public static class BattleIntegrationTests{
 static int tests;static AFAGFHJONFJ source,enemySource;static NJHDEHLHENN war;
 static object Call(string n,params object[] args)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,args);
 static void Check(bool b,string s){tests++;if(!b)throw new Exception(s);}
 static void SetClock(double n)=>typeof(StrategicSupport).GetField("battleClock",BindingFlags.NonPublic|BindingFlags.Static).SetValue(null,n);
 static int MissionCount=>((System.Collections.IDictionary)typeof(StrategicSupport).GetField("missions",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Count;
 static AFAGFHJONFJ City(string id,string f){var city=new AFAGFHJONFJ{OJPECMNINNA=id};city.LMPMNLLGOEC.LMMNKIOAGLC=f;city.LMPMNLLGOEC.IPJHOOJBCKI="gov";for(int i=0;i<10;i++)city.LMPMNLLGOEC.KJPPGIOCDID.Add(new());MICGPAMNDKM.IJPKEMPIFIH[id]=city;return city;}
 static void Setup(){MICGPAMNDKM.IJPKEMPIFIH=new();ICGAEMLACJG.HFJIECIBFKB.Clear();NEINKOOBJIH.COCMABJFOFH.Clear();NEINKOOBJIH.JMIADAFPGMC.Clear();NEINKOOBJIH.AOPJBNBGPLP.Clear();
  foreach(var f in new[]{"F","enemy"}){var fac=new MKMOJNAJKJE{IBGCLCAIBOL=f};fac.ANMMMOIKLEG["gov"]=new(){LMMNKIOAGLC=f};fac.AENJACOFBJK[f=="F"?"enemy":"F"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};NEINKOOBJIH.COCMABJFOFH[f]=fac;}
  source=City("source","F");enemySource=City("enemyCity","enemy");war=new();Wa_Controller.m_warInfo=war;
  Wa_Controller.m_isQuiting=Wa_Controller.m_isBeforeWar=Wa_Controller.m_isByEvent=Wa_Controller.m_isRoundMode=false;Wa_Controller.m_mockInfo=null;
  Wa_Controller.m_battleSpeed=1;Wa_Controller.m_myCurSoldierNum=10000;Wa_Controller.m_enemyCurSoldierNum=20000;
  Time.timeScale=Time.deltaTime=Time.unscaledDeltaTime=1;AEHAGJAKNEK.IAICEJLNKFJ=1;AEHAGJAKNEK.PAKENCLGOHK=false;GameObject.WorldVisible=false;
  Call("Ready");
 }
 static void Send(bool enemy=false){if(enemy){Wa_Controller.m_myCurSoldierNum=20000;Wa_Controller.m_enemyCurSoldierNum=10000;}Call("Assess",war,0d,true);Call("Assess",war,10d,true);}
 public static void Run(){
  var loss=new AttritionWindow();loss.Sample(0,10000,20000);loss.Sample(10,9000,19500);Check(loss.FriendlyLoss==0,"single burst not extrapolated");loss.Sample(20,8000,19000);Check(loss.FriendlyLoss==100&&loss.EnemyLoss==50,"sustained loss rates");loss.Sample(30,12000,18500);Check(loss.Samples==4,"reinforcements preserve observation history");
  Check(BattlePolicy.Need(1000,2000,100,0,9,0,1.15)==0,"too late to rescue");Check(BattlePolicy.Need(10000,20000,0,0,10,13000,1.15)==0,"incoming sufficient");Check(BattlePolicy.Arrival(100,2,10)==60,"arrival includes travel");
  Setup();Send();Check(source.Calls==1&&enemySource.Calls==0,"support to losing ally during player direct command");var t=source.Last;
  Check(t.Starts==1&&t.m_warInfo==null&&t.m_targetWarInfo==war,"start once and no instant join");Check(source.LMPMNLLGOEC.KJPPGIOCDID.Count==4&&t.GetSoldierNum(false)==6000,"real garrison transfers and retention");
  SetClock(9);Call("AdvanceMissions",war,1f);Check(t.Moves==0,"mobilization wait");SetClock(10);Call("AdvanceMissions",war,1f);Check(t.Position.x==2&&t.m_curSpeed==2,"normal movement and speed restoration");Call("AdvanceMissions",war,2f);Check(t.Position.x==6,"2x movement applied once");
  Check(!(bool)Call("OwnedMovement",t),"native update not doubled");t.m_targetWarInfo=null;Check((bool)Call("OwnedMovement",t),"replacement order left untouched");Call("CleanMissions");Check(MissionCount==0,"cancelled mission removed");
  Setup();Send(true);Check(enemySource.Calls==1&&source.Calls==0,"same policy supports enemy side");
  Setup();war.KJPPGIOCDID[0].Add(new(){FGHFOLKBHDL=20000});Send();Check(source.Calls==1,"unknown reserve arrival not assumed immediate");var reserve=war.KJPPGIOCDID[0][0];war.GPFFMNKOFCH[0].Add(reserve);Check((int)Call("Soldiers",war,0,true)==10000,"transition reserve not double counted");
  Setup();source.Player=true;source.OPNKGGKHAGJ=false;source.LMPMNLLGOEC.BKENJPNKMGI="player";Send();Check(source.Calls==1,"manual city participates in faction defense");
  Setup();source.LMPMNLLGOEC.KJPPGIOCDID[0].GDHPLIFJCBN=PMNFKDJNLDJ.Deploy;Send();Check(source.Calls==1&&source.LMPMNLLGOEC.KJPPGIOCDID[0].GDHPLIFJCBN==PMNFKDJNLDJ.Deploy,"busy troop stays home while others support");
  Setup();UnityEngine.AI.NavMesh.Blocked=true;Send();Check(source.Calls==0&&MissionCount==0,"unreachable physical path rejected");UnityEngine.AI.NavMesh.Blocked=false;
  Setup();source.Invalid=true;NEINKOOBJIH.AOPJBNBGPLP.Add("retainer");Send();Check(source.Last.Dismissed,"retainer not taken");
  Setup();source.BMBMKPGEHBH=new(-10000);Send();Check(source.Calls==0,"distant city excluded");
  Setup();Send();t=source.Last;t.m_id++;Check((bool)Call("OwnedMovement",t),"recycled team not intercepted");Call("CleanMissions");Check(MissionCount==0,"recycled team cleanup");
  Setup();Send();war.HIENOINGBCN="replacement";Call("CleanMissions");Check(MissionCount==0,"recycled battle cleanup");
  Setup();Send();t=source.Last;SetClock(100);t.Position=new(99);t.Arrive=()=>Call("ForgetBattle",war);Call("AdvanceMissions",war,1f);Check(t.m_warInfo==war&&MissionCount==0,"arrival callback may mutate mission registry safely");
  Setup();Send();t=source.Last;SetClock(20);Time.timeScale=0;Call("DirectBattleTick");Check(t.Moves==0,"pause stops support movement");Time.timeScale=1;Wa_Controller.m_isBeforeWar=true;Call("DirectBattleTick");Check(t.Moves==0,"deployment screen stops travel");
  Setup();Send();int sent=source.Calls;Call("Assess",war,20d,true);Check(source.Calls==sent,"source cooldown and incoming reservations");
  Setup();Wa_Controller.m_warInfo=null;Call("WorldBattleTick",war);NEINKOOBJIH.MIPFHCKDJOK=5;Call("WorldBattleTick",war);Check(source.Calls==1,"world war support works without direct command");
  Setup();Wa_Controller.m_isByEvent=true;for(int i=0;i<30;i++)Call("DirectBattleTick");Check(source.Calls==0,"scripted event battle not modified");
  Console.WriteLine($"PASS {tests} battle policy/lifecycle stub assertions; actual native gameplay not validated");
 }
}


