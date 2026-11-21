using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.AI;
public static class Hunt94Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int count;static AFAGFHJONFJ source;static MKMOJNAJKJE faction;static Wo_DeployTeamCtrl enemy;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static object Fixture(Type t,string n,params object[] a)=>t.GetMethod(n,F).Invoke(null,a);
 static IDictionary Orders=>(IDictionary)typeof(StrategicSupport).GetField("huntOrders",F).GetValue(null);
 static void Check(bool ok,string reason){count++;if(!ok)throw new Exception("Hunt94: "+reason);}
 static void Setup(int multiplier=50){
  Fixture(typeof(Relief86Tests),"Setup");Orders.Clear();ICGAEMLACJG.HFJIECIBFKB.Clear();MICGPAMNDKM.IJPKEMPIFIH.Clear();Call("ResetOffense");
  NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";NEINKOOBJIH.MIPFHCKDJOK=100;
  source=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","Runan",0f);faction=NEINKOOBJIH.COCMABJFOFH["F"];
  source.Generated=()=>source.Last.Combat=(int)source.Last.m_dicDeployArmy.Values.SelectMany(x=>x).Sum(a=>(long)a.GNCKPEPJDGF()*multiplier);
  enemy=new(){m_id=1407,m_belongPower="",m_bma=new(){OJPECMNINNA="BMA100109"},Position=new(20),Combat=165781,m_deployState=AKOPKFPKJPP.Camp};enemy.m_dicDeployArmy["raiders"]=new(){new(){FGHFOLKBHDL=34500}};ICGAEMLACJG.HFJIECIBFKB[enemy.m_id]=enemy;
  AOHIBKLOAEI.RegionAt=p=>source;NavMesh.Blocked=false;NavMesh.Detour=0;
 }
 static void RunHunt(){int n=0;foreach(var phase in (IEnumerable<string>)Call("HuntSteps",faction))if(++n>10000)throw new Exception("unbounded hunt");}
 public static void Run(){
  Setup();Check((int)Call("HuntPower",enemy)==223805,"reproduce observed 34500 raiders with required adjusted power223805");
  RunHunt();Check(Orders.Count==1&&source.Last.m_attackTarget==enemy&&!source.Last.Dismissed,"low raw surplus and high commander-adjusted real assembly can hunt");
  Check((long)Call("RawArmy",source.Last)<223805&&source.Last.GetCombatVal()>=223805,"raw retention and actual battle power are kept distinct");
  Check((long)Call("ReserveCityPower",source,ICGAEMLACJG.HFJIECIBFKB.Values.ToList())>=4000,"command bonus does not remove physical home reserve");
  Check((bool)Call("GuardHuntStart",source.Last),"same native power comparison approves departure");int calls=source.Calls;RunHunt();Check(source.Calls==calls&&Orders.Count==1,"existing hunt cannot create duplicate expedition");
  enemy.Combat=1000000;Check(!(bool)Call("GuardHuntStart",source.Last)&&source.Last.Dismissed&&Orders.Count==0,"enemy growth before departure cancels now-underpowered force");
  Setup(1);RunHunt();Check(source.Calls==1&&source.Last.Dismissed&&Orders.Count==0,"truly weak assembled force is dismissed before departure");RunHunt();Check(source.Calls==1,"unchanged failed roster does not create repeated armies");
  source.Roster[0].OJPECMNINNA="new-commander";RunHunt();Check(source.Calls==2,"new available commander immediately reopens comparison");
  NEINKOOBJIH.MIPFHCKDJOK+=3;RunHunt();Check(source.Calls==3,"unchanged failure has a bounded game-day retry");
  Setup();source.LMPMNLLGOEC.KJPPGIOCDID.RemoveRange(1,9);RunHunt();Check(source.Calls==0,"last garrison unit cannot be exported even with giant commander bonus");
  Setup();faction.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;RunHunt();Check(source.Calls==0,"StopWar remains authoritative");
  Setup();NavMesh.Blocked=true;RunHunt();Check(source.Calls==0,"blocked actual map path never assembles a hunter");NavMesh.Blocked=false;
  Setup();source.Generated=()=>{source.Last.Combat=500000;faction.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;};RunHunt();Check(source.Last.Dismissed&&Orders.Count==0,"policy change during native generation rolls back");
  Setup();source.Generated=()=>{source.Last.Combat=500000;ICGAEMLACJG.HFJIECIBFKB.Remove(1407);};RunHunt();Check(source.Last.Dismissed&&Orders.Count==0,"target removed during native creation cannot receive an order");
  Setup();enemy.m_isPrivate=true;RunHunt();Check(source.Calls==0,"private army is not treated as a raider");
  Setup();source.ThrowGeneration=true;try{RunHunt();}catch(InvalidOperationException){}Check(!(bool)typeof(StrategicSupport).GetField("offenseGenerating",F).GetValue(null)&&Orders.Count==0,"native exception restores generation scope");
  Fixture(typeof(OffensiveTests),"Setup");var t=typeof(OffensiveTests);source=(AFAGFHJONFJ)t.GetField("c",F).GetValue(null);var target=(AFAGFHJONFJ)t.GetField("target",F).GetValue(null);faction=NEINKOOBJIH.COCMABJFOFH["F"];
  target.Power=50000;source.Generated=()=>source.Last.Combat=500000;Fixture(t,"RunPlan");Check(source.Last!=null&&!source.Last.Dismissed&&(bool)Fixture(t,"Start"),"ordinary city attack no longer compares raw surplus against adjusted defenders");
  Call("ResetWork");Call("ResetOffense");Orders.Clear();AOHIBKLOAEI.RegionAt=null;Console.WriteLine($"PASS Hunt94 {count} observed scale-mismatch, native assembly, home reserve, failure retry and ordinary attack checks; native calls mocked, not gameplay.");
 }
}
