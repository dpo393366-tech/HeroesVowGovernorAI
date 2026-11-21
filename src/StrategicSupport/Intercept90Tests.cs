using System;using System.Collections;using System.Collections.Generic;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class Intercept90Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;static Wo_DeployTeamCtrl army,enemy;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static IDictionary Map(string n)=>(IDictionary)typeof(StrategicSupport).GetField(n,F).GetValue(null);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Intercept90: "+why);}
 static void Setup(){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");Map("standoffs").Clear();Map("failedIntercepts").Clear();Map("decisionStamps").Clear();
  army=(Wo_DeployTeamCtrl)typeof(Campaign81Tests).GetField("t",F).GetValue(null);army.m_targetCity="";army.gameObject.activeSelf=true;army.m_responseToPlayer=true;
  enemy=new(){m_id=401,m_belongPower="enemy",m_deployState=AKOPKFPKJPP.Expedition,Position=new(70),Combat=9000};enemy.gameObject.activeSelf=true;enemy.m_listMovePos.Add(new(90));army.m_responseTeam=enemy;ICGAEMLACJG.HFJIECIBFKB[401]=enemy;AOHIBKLOAEI.RegionAt=null;
 }
 public static void Run(){
  Setup();Call("RecoverHostileStandoff",army);Check(army.m_attackTarget==enemy&&army.m_deployState==AKOPKFPKJPP.Expedition&&army.m_listMovePos.Count>0,"native empty-target Camp immediately obtains a real attack and validated path against moving opponent");Check(!army.Dismissed&&army.Position.x==50,"existing force is neither teleported nor dismissed");
  Setup();Call("AfterInterceptDeploy",army);var work=(WorkQueue)typeof(StrategicSupport).GetField("work",F).GetValue(null);work.Pump(100,20,(s,d)=>{},e=>throw e);Check(army.m_attackTarget==enemy,"departure event schedules recovery even before the next daily pulse");
  Setup();Call("ObserveIntercept",army);enemy.m_id=434;enemy.m_belongPower="F";ICGAEMLACJG.HFJIECIBFKB.Remove(401);ICGAEMLACJG.HFJIECIBFKB[434]=enemy;Call("RecoverHostileStandoff",army);Check(!army.m_responseToPlayer&&army.m_responseTeam==null&&Map("campaignGoals").Contains(army.Pointer),"pooled former enemy now friendly cannot become a perpetual follow target");
  Setup();Call("ObserveIntercept",army);enemy.m_warInfo=new();Call("RecoverHostileStandoff",army);Check(!army.m_responseToPlayer&&Map("campaignGoals").Contains(army.Pointer),"opponent joining battle releases stale interception for support or return assessment");
  Setup();Call("ObserveIntercept",army);ICGAEMLACJG.HFJIECIBFKB.Remove(401);Call("RecoverHostileStandoff",army);Check(!army.m_responseToPlayer,"removed target is abandoned even if its Unity object remains alive");
  Setup();NavMesh.Blocked=true;Call("RecoverHostileStandoff",army);Check(army.m_responseToPlayer&&!army.Dismissed,"transient blocked route gets bounded reassessment");NEINKOOBJIH.MIPFHCKDJOK+=3;Call("RecoverHostileStandoff",army);Check(!army.m_responseToPlayer&&Map("failedIntercepts").Count==1,"unchanged blocked path returns instead of waiting forever");
  var retry=new Wo_DeployTeamCtrl{m_id=409,m_assembleCity=army.m_assembleCity,m_belongPower="F",m_deployState=AKOPKFPKJPP.Assemble,m_responseToPlayer=true,m_responseTeam=enemy};Check(!(bool)Call("GuardFailedIntercept",retry)&&retry.Dismissed,"same failed target cannot repeatedly consume departures");enemy.Position=new(85);var moved=new Wo_DeployTeamCtrl{m_id=410,m_assembleCity=army.m_assembleCity,m_belongPower="F",m_deployState=AKOPKFPKJPP.Assemble,m_responseToPlayer=true,m_responseTeam=enemy};Check((bool)Call("GuardFailedIntercept",moved)&&!moved.Dismissed,"target movement immediately invalidates suppression");
  Setup();army.m_isPrivate=true;Call("RecoverHostileStandoff",army);Check(army.m_attackTarget==null&&army.m_responseToPlayer,"private army retains manual follow");
  Setup();enemy.m_belongPower="F";Call("RecoverHostileStandoff",army);Check(army.m_responseToPlayer&&army.m_attackTarget==null,"unobserved friendly follow is not guessed to be hostile");
  Setup();army.m_targetCity="enemy-city";Call("RecoverHostileStandoff",army);Check(army.m_targetCity=="enemy-city"&&army.m_attackTarget==null,"explicit city orders remain untouched");
  Setup();army.m_warInfo=new();Call("RecoverHostileStandoff",army);Check(army.m_warInfo!=null&&army.m_responseToPlayer,"active battle is never interrupted");
  Setup();Call("RecoverHostileStandoff",army);enemy.Position=new(95);NEINKOOBJIH.MIPFHCKDJOK+=3;Call("RecoverHostileStandoff",army);Check(army.m_listMovePos.Count>0&&army.m_responseToPlayer,"healthy march is not needlessly reissued");
  NavMesh.Blocked=false;Call("ResetWork");Map("standoffs").Clear();Map("failedIntercepts").Clear();Console.WriteLine($"PASS Intercept90 {checks} moving-target, pooled-identity, bounded retry and manual-order checks; native calls mocked.");
 }
}
