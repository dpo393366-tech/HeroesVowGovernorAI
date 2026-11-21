using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ci_CityCtrl:Native {public AFAGFHJONFJ City;public static implicit operator bool(Ci_CityCtrl c)=>c!=null;}
public static partial class ICGAEMLACJG {public static Dictionary<string,Ci_CityCtrl> FKKCPDEFODO=new();}
public partial class Wo_DeployTeamCtrl {
 public Vector3 IAFGLIMLMGA,m_tmpTargetPos;
 public string m_belongGroup="gov",m_tmpTargetCity,m_targetCamp;
 public object m_targetVillage,m_targetPass;public Wo_DeployTeamCtrl m_attackTarget,m_mergeTarget;
 public int Wounds,CityMoves;public bool RejectCityMove;
 public bool MissingCampVisual;public void SetDeployState(AKOPKFPKJPP state){m_deployState=state;}
 public List<Vector3> GAANFLKOMKL=new();public bool NativeControl=true;public bool CanControl()=>NativeControl;
 public int GetWoundNum()=>Wounds;public void UpdateOneDay(){}public void EnterCity(AFAGFHJONFJ c){}public void StartCamp(){if(!MissingCampVisual)m_deployState=AKOPKFPKJPP.Camp;typeof(StrategicSupport).GetMethod("AfterCampaignCamp",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).Invoke(null,new object[]{this});}
 public void MoveToCity(Ci_CityCtrl c,bool enter,bool option){CityMoves++;if(RejectCityMove)return;m_targetCity=!enter&&c.City.LMPMNLLGOEC.LMMNKIOAGLC==m_belongPower?"":c.City.OJPECMNINNA;m_isEnterCity=enter;m_deployState=AKOPKFPKJPP.Expedition;m_listMovePos.Add(c.City.BMBMKPGEHBH);}
}
public static class Campaign81Tests {
 static int checks;static AFAGFHJONFJ home,target;static MKMOJNAJKJE f;static Wo_DeployTeamCtrl t;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static IDictionary Goals=>(IDictionary)typeof(StrategicSupport).GetField("campaignGoals",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
 static void Check(bool b,string s){checks++;if(!b)throw new Exception("R81: "+s);}
 static void Setup(){
  typeof(OffensiveTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);
  home=(AFAGFHJONFJ)typeof(OffensiveTests).GetField("c",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
  target=(AFAGFHJONFJ)typeof(OffensiveTests).GetField("target",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
  f=NEINKOOBJIH.COCMABJFOFH["F"];home.Capacity=30;home.BMBMKPGEHBH=new(0);target.BMBMKPGEHBH=new(100);
  ICGAEMLACJG.FKKCPDEFODO.Clear();foreach(var c in MICGPAMNDKM.IJPKEMPIFIH.Values)ICGAEMLACJG.FKKCPDEFODO[c.OJPECMNINNA]=new(){City=c};
  t=new(){m_id=318,m_assembleCity=home.OJPECMNINNA,m_targetCity=target.OJPECMNINNA,m_belongPower="F",m_deployState=AKOPKFPKJPP.Camp,Position=new(50),Food=20000};
  t.m_dicDeployArmy["troops"]=new(){new(){FGHFOLKBHDL=10000}};ICGAEMLACJG.HFJIECIBFKB[318]=t;Call("Ready");Goals.Clear();Call("RememberCampaign",t);
 }
 static void Run(){if(Goals.Contains(t.Pointer))foreach(var step in (IEnumerable<string>)Call("RecoverCampaign",t,Goals[t.Pointer])){} }
 public static void RunAll(){
  Setup();Run();Check(t.CityMoves==1&&t.m_targetCity==target.OJPECMNINNA&&!t.m_isEnterCity,"strong provisioned camp resumes actual goal through native movement");Check(t.Position.x==50&&!t.Dismissed,"no teleport or forced dismissal");
  Setup();f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Run();Check(t.m_isEnterCity&&t.m_targetCity==home.OJPECMNINNA,"StopWar returns instead of attacking");
  Setup();t.Combat=1;Run();Check(t.m_isEnterCity,"weak army returns");
  Setup();t.Morale=20;Run();Check(t.m_isEnterCity,"low morale returns");
  Setup();t.Wounds=6000;Run();Check(t.m_isEnterCity,"heavy wounded returns");
  Setup();t.Food=400;Run();Check(t.m_isEnterCity,"food sufficient only for return triggers retreat");
  Setup();target.LMPMNLLGOEC.LMMNKIOAGLC="neutral-power";Run();Check(t.m_isEnterCity,"peace or changed owner does not declare war");
  Setup();target.LMPMNLLGOEC.LMMNKIOAGLC="F";target.Capacity=20;Run();Check(t.m_isEnterCity,"captured target changes to friendly return");
  Setup();t.m_warInfo=new();Run();Check(t.CityMoves==0,"active battle untouched");
  Setup();t.m_targetWarInfo=new();Run();Check(t.CityMoves==0,"en route reinforcement order untouched");
  Setup();t.m_targetCamp="manual";Run();Check(t.CityMoves==0,"explicit camp order untouched");
  Setup();t.m_responseToPlayer=true;Run();Check(t.CityMoves==0,"player following army untouched");
  Setup();t.m_isPrivate=true;Run();Check(t.CityMoves==0,"private army untouched");
  Setup();t.m_targetCity="manual-new-goal";Run();Check(t.CityMoves==0,"user redirected army untouched");
  Setup();t.m_id++;Run();Check(t.CityMoves==0,"recycled native pointer untouched");
  Setup();NavMesh.Blocked=true;Run();Check(t.CityMoves==0&&!t.Dismissed,"unreachable route no teleport or destruction");
  Setup();t.Combat=1;home.Capacity=0;Run();Check(t.CityMoves==1&&!t.m_isEnterCity&&t.m_targetCity==""&&t.m_listMovePos.Count>0&&Vector3.Distance(t.m_listMovePos.Last(),home.BMBMKPGEHBH)<.1f,"native friendly staging clears city target but keeps path without overflowing garrison");
  Setup();t.Combat=1;home.Capacity=0;t.Position=home.BMBMKPGEHBH;Run();Check(t.CityMoves==0,"full staging point does not repeatedly issue zero-distance movement");
  Setup();var steps=((IEnumerable<string>)Call("RecoverCampaign",t,Goals[t.Pointer])).GetEnumerator();t.Combat=1;steps.MoveNext();t.m_warInfo=new();while(steps.MoveNext()){}Check(t.CityMoves==0,"battle starting between scheduler slices cancels order");
  Setup();t.Combat=1;home.Capacity=11;var inbound=new Wo_DeployTeamCtrl{m_id=999,m_belongPower="F",m_targetCity=home.OJPECMNINNA};inbound.m_dicDeployArmy["x"]=new(){new(),new()};ICGAEMLACJG.HFJIECIBFKB[999]=inbound;Run();Check(!t.m_isEnterCity,"incoming armies reserve destination capacity");
  Setup();Run();t.m_deployState=AKOPKFPKJPP.Camp;t.m_listMovePos.Clear();t.Combat=1;Run();Check(t.m_isEnterCity,"new battle losses cause reevaluation");
  Setup();t.Combat=1;Run();Check((bool)Call("GuardCampaignEntry",t,home),"valid arrival allowed");home.Capacity=0;Check(!(bool)Call("GuardCampaignEntry",t,home)&&t.m_deployState==AKOPKFPKJPP.Camp,"capacity shrinking during travel delays entry without desertion");
  Setup();t.Combat=1;Run();home.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!(bool)Call("GuardCampaignEntry",t,home),"city captured during return blocks forced entry");
  Setup();t.RejectCityMove=true;Run();Run();t.RejectCityMove=false;Run();Check(t.m_isEnterCity,"repeated rejected advance falls back to return");
  Setup();Call("AfterCampaignDay",t);var queue=(WorkQueue)typeof(StrategicSupport).GetField("work",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);Check(queue.Count==1,"native daily callback enqueues camp reassessment");Call("AfterCampaignDay",t);Check(queue.Count==1,"same day callback does not duplicate work");queue.Pump(10000,500,(s,ms)=>{},e=>throw e);Check(t.CityMoves==1,"daily scheduled work actually reaches native movement");
  Setup();t.Combat=1;home.Capacity=0;Run();t.Position=home.BMBMKPGEHBH;t.StartCamp();home.Capacity=30;NEINKOOBJIH.MIPFHCKDJOK++;Call("AfterCampaignDay",t);queue.Pump(10000,500,(s,ms)=>{},e=>throw e);Check(t.m_isEnterCity,"staged army retries and enters after capacity opens");
  Setup();Call("AfterCampaignCamp",t);queue.Pump(10000,500,(s,ms)=>{},e=>throw e);Check(t.CityMoves==1&&t.NativeControl,"delegated player-controllable army resumes despite vanilla CanControl exclusion");
  Setup();var second=new Wo_DeployTeamCtrl{m_id=352,m_assembleCity=t.m_assembleCity,m_targetCity=t.m_targetCity,m_belongPower="F",m_deployState=AKOPKFPKJPP.Camp,Position=t.Position,Food=20000};second.m_dicDeployArmy["troops"]=new(){new(){FGHFOLKBHDL=10000}};ICGAEMLACJG.HFJIECIBFKB[352]=second;Call("AfterCampaignDay",t);Call("AfterCampaignDay",second);queue.Pump(10000,500,(s,ms)=>{},e=>throw e);Check(t.CityMoves==1&&second.CityMoves==1,"two armies stalled at same point are both reassessed independently");
  Console.WriteLine("R81 campaign checks: "+checks);
 }
}
