using System;using System.Collections.Generic;using System.Reflection;using System.Linq;using UnityEngine;
public class FCMFBPDEADL {public float IILHJMIHJAI;public Il2CppSystem.Collections.Generic.List<AFAGFHJONFJ> EJMDIPFNGMG=new();}
public partial class AFAGFHJONFJ {public Dictionary<string,FCMFBPDEADL> MEKIDAONKMD=new();}
public static class Relief83Tests {
 static int checks;const BindingFlags Flags=BindingFlags.Static|BindingFlags.NonPublic;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,Flags).Invoke(null,a);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Relief83: "+why);}
 public static void Run(){
  var foes=new List<ReinforcementTimeline.Wave>{new(21.4,776667)};var friends=new List<ReinforcementTimeline.Wave>();
  int before=ReinforcementTimeline.Need(0,0,12944.5,0,24.9,81.4,friends,foes,1.15,21.4,out var reason,out _);
  Check(before==0&&reason=="collapse-before-wave","reproduces actual Wan rejection");
  int after=ReinforcementTimeline.Need(0,0,12944.5,0,24.9,81.4,friends,foes,1.15,21.4,out reason,out _,true);
  Check(after>0&&reason=="uncertain-precontact-relay","guessed precontact collapse no longer blocks safe surplus");
  Check(ReinforcementTimeline.Need(3763,99112,348.1,59,24.9,24.9,friends,new List<ReinforcementTimeline.Wave>(),1.15,0,out reason,out _)==0&&reason=="collapse-before-wave","actual late battle still rejects suicidal late arrivals");
  typeof(IntegrationTests).GetMethod("Prepare",Flags).Invoke(null,null);Call("Ready");Call("ResetDefense");Call("ResetWork");
  var f=NEINKOOBJIH.COCMABJFOFH["F"];var origin=MICGPAMNDKM.IJPKEMPIFIH["source"];var target=MICGPAMNDKM.IJPKEMPIFIH["target"];
  target.Power=0;target.LMPMNLLGOEC.KJPPGIOCDID.Clear();var garrison=new DHKINGPDKJJ{FGHFOLKBHDL=7898,CombatValue=2618};target.LMPMNLLGOEC.KJPPGIOCDID.Add(garrison);
  Check((double)Call("DefenseHomePower",target,new List<Wo_DeployTeamCtrl>())==2618,"real garrison power survives zero expedition commander pool");
  target.LMPMNLLGOEC.KJPPGIOCDID.Add(garrison);Check((double)Call("DefenseHomePower",target,new List<Wo_DeployTeamCtrl>())==2618,"duplicate unit reference counted once");
  var deployed=new Wo_DeployTeamCtrl();deployed.m_dicDeployArmy["x"]=new(){garrison};
  Check((double)Call("DefenseHomePower",target,new List<Wo_DeployTeamCtrl>{deployed})==0,"deployed unit not counted as additional home protection");
  origin.BMBMKPGEHBH=new(0);target.BMBMKPGEHBH=new(40);var via=new AFAGFHJONFJ{OJPECMNINNA="friendly-via",BMBMKPGEHBH=new(20)};via.LMPMNLLGOEC.LMMNKIOAGLC="F";
  var hostile=new AFAGFHJONFJ{OJPECMNINNA="hostile-shortcut"};hostile.LMPMNLLGOEC.LMMNKIOAGLC="enemy";
  AOHIBKLOAEI.RegionAt=p=>p.x<1?origin:p.x>39?target:hostile;
  origin.MEKIDAONKMD[target.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){via}};
  object[] args={origin.BMBMKPGEHBH,target.BMBMKPGEHBH,f,2f,0d,null};
  Check((bool)Call("RouteSeconds",args),"nearest city on a road sample is not native transit territory");
  AOHIBKLOAEI.RegionAt=p=>p.x<1?origin:p.x>39?target:via;
  Check((bool)Call("RouteSeconds",args)&&Math.Abs((double)args[4]-20)<.01,"validated friendly physical road and ETA agree");
  via.LMPMNLLGOEC.LMMNKIOAGLC="enemy";
  Check(!(bool)Call("RouteSeconds",args),"native road through enemy remains blocked");
  f.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.ArmyPass;
  Check((bool)Call("RouteSeconds",args),"native transit permission respected");
  UnityEngine.AI.NavMesh.Blocked=true;
  Check(!(bool)Call("RouteSeconds",args),"native road still needs physically complete NavMesh legs");UnityEngine.AI.NavMesh.Blocked=false;
  origin.MEKIDAONKMD[target.OJPECMNINNA].EJMDIPFNGMG.Clear();
  Check((bool)Call("RouteSeconds",args),"empty cached intermediate list is a valid direct route");
  AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;Call("ResetDefense");Call("ResetWork");
  Console.WriteLine($"PASS Relief83 {checks} live-log reproduction/native-road regression checks; native movement remains unverified");
 }
}
