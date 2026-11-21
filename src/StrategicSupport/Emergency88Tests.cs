using System;using System.Linq;using System.Reflection;using System.Collections.Generic;using UnityEngine;using UnityEngine.AI;
public static class Emergency88Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int count;static AFAGFHJONFJ source;static NJHDEHLHENN war;static MKMOJNAJKJE f;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool b,string s){count++;if(!b)throw new Exception("Emergency88: "+s);}
 static void Setup(){typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetBattleState");Call("ResetDefense");Call("ResetWork");NavMesh.Blocked=false;NavMesh.Detour=0;AOHIBKLOAEI.Region=null;AOHIBKLOAEI.RegionAt=null;source=(AFAGFHJONFJ)typeof(BattleIntegrationTests).GetField("source",F).GetValue(null);war=(NJHDEHLHENN)typeof(BattleIntegrationTests).GetField("war",F).GetValue(null);f=NEINKOOBJIH.COCMABJFOFH["F"];}
 static Wo_DeployTeamCtrl Send(){var teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();var used=(HashSet<IntPtr>)Call("AttackReserved",teams);return (Wo_DeployTeamCtrl)Call("GenerateEmergencyRelief",source,war,99999,30d,teams,used);}
 public static void Run(){
  Setup();var first=Send();Check(first!=null&&first.GetSoldierNum(false)==6000,"urgent request sends six of ten rather than ordinary four");Check(Send()==null,"same-battle repeated request keeps initial 35-percent floor instead of recursively emptying city");
  var secondWar=new NJHDEHLHENN{HIENOINGBCN="other battle"};war=secondWar;Check(Send()==null,"same source cannot reset its home floor by switching to a different battle");
  Setup();var abroad=new Wo_DeployTeamCtrl{m_id=900,m_belongPower="F",m_assembleCity=source.OJPECMNINNA};abroad.m_dicDeployArmy["away"]=Enumerable.Range(0,40).Select(_=>new DHKINGPDKJJ()).ToList();ICGAEMLACJG.HFJIECIBFKB[900]=abroad;Check(Send()?.GetSoldierNum(false)==6000,"already unavailable army does not create an impossible home reserve");
  Setup();source.LMPMNLLGOEC.KJPPGIOCDID.RemoveRange(2,8);source.HomeOfficerCount=2;Check(Send()?.GetSoldierNum(false)==1000,"two-unit two-officer city sends one and preserves one commander and unit");Check(Send()==null,"last physical unit and commander never sent");
  Setup();f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Check(Send()!=null,"StopWar permits urgent defense support");
  Setup();ICGAEMLACJG.HFJIECIBFKB[900]=new(){m_id=900,m_belongPower="enemy",m_targetCity=source.OJPECMNINNA};Check(Send()==null,"real enemy marching at home blocks urgent export");
  Setup();var border=new AFAGFHJONFJ{OJPECMNINNA="near-threat",BMBMKPGEHBH=new(2)};border.LMPMNLLGOEC.LMMNKIOAGLC="enemy";border.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){CombatValue=100000});source.FKMKGNPFLFP.Add(border);Check(Send()?.GetSoldierNum(false)==4000,"potential close enemy raises retained share to60 without demanding impossible100k home power");
  Setup();source.Generated=()=>ICGAEMLACJG.HFJIECIBFKB[901]=new(){m_id=901,m_belongPower="enemy",m_targetCity=source.OJPECMNINNA};Check(Send()==null&&source.Last.Dismissed,"attack appears during native creation: transaction cancelled");
  Setup();source.Generated=()=>f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Check(Send()!=null&&!source.Last.Dismissed,"StopWar selected during assembly does not cancel defense");
  Setup();source.ThrowGeneration=true;try{Send();}catch(TargetInvocationException){}Check(typeof(StrategicSupport).GetField("emergencyContext",F).GetValue(null)==null,"native exception restores urgent scope");
  Setup();source.LMPMNLLGOEC.OEGNGFBCHFL=0;Check(Send()==null,"no-food city not emptied");
  Setup();source.PathBlocked=true;Call("Assess",war,0d,true);Check(source.Last!=null&&!source.Last.Dismissed,"obsolete native direct-route veto cannot reject a fully validated physical relief path");
  Routes();Call("ResetBattleState");Call("ResetWork");Call("ResetDefense");Console.WriteLine($"PASS Emergency88 {count} urgent budgets, rollback, multi-front ledger and friendly-route fallback checks; native calls mocked.");
 }
 static void Routes(){
  Setup();source.OJPECMNINNA="Runan";source.BMBMKPGEHBH=new(0);
  var via=new AFAGFHJONFJ{OJPECMNINNA="Xinye",BMBMKPGEHBH=new(0,0,20)};via.LMPMNLLGOEC.LMMNKIOAGLC="F";
  var dest=new AFAGFHJONFJ{OJPECMNINNA="Xuchang",BMBMKPGEHBH=new(40)};dest.LMPMNLLGOEC.LMMNKIOAGLC="enemy";
  var blocked=new AFAGFHJONFJ{OJPECMNINNA="other-hostile"};blocked.LMPMNLLGOEC.LMMNKIOAGLC="enemy";
  var rear=new AFAGFHJONFJ{OJPECMNINNA="Shouchun",BMBMKPGEHBH=new(-20)};rear.LMPMNLLGOEC.LMMNKIOAGLC="F";
  source.FKMKGNPFLFP.Add(dest);source.FKMKGNPFLFP.Add(via);via.FKMKGNPFLFP.Add(dest);rear.FKMKGNPFLFP.Add(source);
  source.MEKIDAONKMD[dest.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){blocked}};
  rear.MEKIDAONKMD[dest.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){source,blocked}};
  AOHIBKLOAEI.RegionAt=p=>p.x< -15?rear:p.x>38?dest:p.x>10&&p.x<30&&p.z<2?blocked:p.z>1?via:source;
  object[] args={source.BMBMKPGEHBH,dest.BMBMKPGEHBH,f,dest.OJPECMNINNA,false,null,0d};
  Check((bool)Call("ReliefPath",args)&&((List<Vector3>)args[5]).Any(p=>p.z==20),"Runan direct line crosses foreign territory: find actual Xinye corridor");
  args[0]=rear.BMBMKPGEHBH;Check((bool)Call("ReliefPath",args)&&((List<Vector3>)args[5]).Any(p=>p.z==20),"Shouchun through Runan can use the same extra friendly detour");
  war.IJGLCFJOOIK=dest;war.GBHGENNMJCK=dest.BMBMKPGEHBH;var moving=new Wo_DeployTeamCtrl{m_belongPower="F",Position=rear.BMBMKPGEHBH,m_deployState=AKOPKFPKJPP.Expedition};
  Check((bool)Call("MoveRelief",moving,dest,war,false,f,false)&&moving.m_listMovePos.Any(p=>p.z==20),"validated alternate waypoints reach the actual native movement list");
  via.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!(bool)Call("ReliefPath",args),"captured detour cannot be treated as a friendly corridor");
  AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;
 }
}
