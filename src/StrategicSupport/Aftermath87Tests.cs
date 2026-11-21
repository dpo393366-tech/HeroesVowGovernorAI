using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;
public static class Aftermath87Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static Wo_DeployTeamCtrl t;static AFAGFHJONFJ home,target;static MKMOJNAJKJE f;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static IDictionary Goals=>(IDictionary)typeof(StrategicSupport).GetField("campaignGoals",F).GetValue(null);
 static WorkQueue Queue=>(WorkQueue)typeof(StrategicSupport).GetField("work",F).GetValue(null);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Aftermath87: "+why);}
 static void Setup(){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);
  NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
  t=(Wo_DeployTeamCtrl)typeof(Campaign81Tests).GetField("t",F).GetValue(null);
  home=(AFAGFHJONFJ)typeof(Campaign81Tests).GetField("home",F).GetValue(null);
  target=(AFAGFHJONFJ)typeof(Campaign81Tests).GetField("target",F).GetValue(null);
  f=NEINKOOBJIH.COCMABJFOFH["F"];Call("ResetWork");Call("ResetDefense");Goals.Clear();
  t.m_id=402;t.m_deployState=AKOPKFPKJPP.Expedition;t.m_targetCity="";t.m_tmpTargetCity="";t.m_listMovePos.Clear();t.GAANFLKOMKL.Clear();t.Position=home.BMBMKPGEHBH;AOHIBKLOAEI.Region=home;
 }
 static void Pump(){Queue.Pump(10000,10000,(s,ms)=>{},e=>throw e);}
 static void Decide(){Call("AfterCampaignDay",t);Pump();}
 public static void Run(){
  Setup();Check((bool)Call("CampIdle",t),"live 402 shape: Expedition with empty goals/path is recoverable");Decide();Check(t.CityMoves==1&&t.m_targetCity==target.OJPECMNINNA&&!t.m_isEnterCity,"surviving untracked army evaluates adjacent attack");Check(t.m_listMovePos.Count>0&&!t.Dismissed,"native path installed without teleport or field dismissal");
  Setup();f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Decide();Check(t.m_isEnterCity&&t.m_targetCity==home.OJPECMNINNA,"StopWar returns for native city demobilization");
  Setup();t.Combat=1;Decide();Check(t.m_isEnterCity,"weak survivor returns");
  Setup();t.Morale=20;Decide();Check(t.m_isEnterCity,"poor morale prevents follow-on attack");
  Setup();t.Wounds=6000;Decide();Check(t.m_isEnterCity,"heavy casualties return");
  Setup();target.LMPMNLLGOEC.LMMNKIOAGLC="neutral";Decide();Check(t.m_isEnterCity,"no attack on a neutral faction");
  Setup();t.m_targetWarInfo=new();Decide();Check(t.CityMoves==0&&Goals.Count==0,"unfinished combat destination protected");
  Setup();t.m_warInfo=new();Decide();Check(t.CityMoves==0&&Goals.Count==0,"battle itself never interrupted");
  Setup();t.m_targetCity=target.OJPECMNINNA;Decide();Check(t.CityMoves==0,"native pending explicit destination left intact");
  Setup();t.m_tmpTargetCity=target.OJPECMNINNA;Decide();Check(t.CityMoves==0,"native deferred destination left intact");
  Setup();t.GAANFLKOMKL.Add(new(1));Decide();Check(t.CityMoves==0,"native intermediate waypoint left intact");
  Setup();t.m_listMovePos.Add(new(1));Decide();Check(t.CityMoves==0,"healthy movement left intact");
  Setup();t.m_isPrivate=true;Decide();Check(t.CityMoves==0,"private army left intact");
  Setup();t.m_responseToPlayer=true;Decide();Check(t.CityMoves==0,"player-following army left intact");
  Setup();t.m_targetCamp="manual";Decide();Check(t.CityMoves==0,"explicit camp command left intact");
  Setup();t.m_isStarting=true;Decide();Check(t.CityMoves==0,"native departure transition left intact");
  Setup();t.m_deployState=AKOPKFPKJPP.Assemble;Decide();Check(t.CityMoves==0,"assembly never mistaken for aftermath");
  Setup();t.Combat=1;home.Capacity=0;Decide();Check(!t.m_isEnterCity&&!t.Dismissed,"full destination is not force-fed returning troops");home.Capacity=30;NEINKOOBJIH.MIPFHCKDJOK++;Decide();Check(t.m_isEnterCity,"capacity opening resumes return on next day");
  Setup();Call("RememberCampaign",t);var it=((IEnumerable<string>)Call("RecoverCampaign",t,Goals[t.Pointer])).GetEnumerator();Check(it.MoveNext(),"aftermath work yields");f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;while(it.MoveNext()){}Check(t.m_isEnterCity,"policy change during sliced planning honored");
  Setup();NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="other";t.Combat=1;home.Capacity=0;Decide();home.Capacity=30;NEINKOOBJIH.MIPFHCKDJOK++;Decide();Check(!t.m_isEnterCity,"NPC camp does not reevaluate at allied one-day interval");NEINKOOBJIH.MIPFHCKDJOK+=2;Decide();Check(t.m_isEnterCity,"NPC camp resumes on third day, not ninth day");
  Setup();Call("RememberCampaign",t);it=((IEnumerable<string>)Call("RecoverCampaign",t,Goals[t.Pointer])).GetEnumerator();it.MoveNext();t.m_listMovePos.Add(new(4));while(it.MoveNext()){}Check(t.CityMoves==0,"new native movement during planning cancels stale plan");
  Setup();Call("AfterCampaignDay",t);NEINKOOBJIH.MIPFHCKDJOK+=.25;Call("AfterCampaignDay",t);Check(Queue.Count==1,"fractional day pulse does not enqueue repeat scans");Pump();
  Setup();foreach(var phase in (IEnumerable<string>)Call("DefensePulse")){}Pump();Check(t.CityMoves==1,"existing sliced world scan recovers save-loaded orphan without a StartCamp event");
  Call("ResetWork");Call("ResetDefense");Goals.Clear();Console.WriteLine($"PASS Aftermath87 {checks} survivor recovery and policy checks; native calls mocked, not live gameplay.");
 }
}
