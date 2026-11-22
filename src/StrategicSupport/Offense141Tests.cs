using System;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.AI;
public static class Offense141Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static Wo_DeployTeamCtrl army;static AFAGFHJONFJ home,target;static MKMOJNAJKJE faction;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Field<T>(Type type,string n)=>(T)type.GetField(n,F).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Offense141: "+why);}
 static void Drain(string n,params object[] a){int steps=0;foreach(var phase in (IEnumerable<string>)Call(n,a))if(++steps>30000)throw new Exception("unbounded iterator "+n);}
 static void Setup(){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);army=Field<Wo_DeployTeamCtrl>(typeof(Campaign81Tests),"t");home=Field<AFAGFHJONFJ>(typeof(Campaign81Tests),"home");target=Field<AFAGFHJONFJ>(typeof(Campaign81Tests),"target");faction=NEINKOOBJIH.COCMABJFOFH["F"];
  Call("ResetWork");Call("ResetDefense");Call("ResetManualOrders");Call("ResetOffense");NavMesh.Blocked=false;AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=target;
  army.m_deployState=AKOPKFPKJPP.Expedition;army.m_listMovePos.Clear();army.m_listMovePos.Add(target.BMBMKPGEHBH);army.Combat=100000;target.Power=0;target.LMPMNLLGOEC.KJPPGIOCDID.Clear();target.FKMKGNPFLFP.Clear();
 }
 static Wo_DeployTeamCtrl Enemy(int id,int combat,int men,string owner="enemy"){
  var t=new Wo_DeployTeamCtrl{m_id=id,m_belongPower=owner,m_deployState=AKOPKFPKJPP.Camp,Combat=combat,Position=target.BMBMKPGEHBH};t.m_dicDeployArmy["commander"]=new(){new(){FGHFOLKBHDL=men}};ICGAEMLACJG.HFJIECIBFKB[id]=t;return t;
 }
 static (int,long) Strength()=>(ValueTuple<int,long>)Call("AttackStrength",target,new List<Wo_DeployTeamCtrl>(ICGAEMLACJG.HFJIECIBFKB.Values));
 public static void Run(){
  Setup();Enemy(14100,1000,200000);ICGAEMLACJG.HFJIECIBFKB.Remove(army.m_id);home.Generated=()=>home.Last.Combat=10000000;
  Check(!(bool)Call("TryAttack",home,target,faction,"gov")&&home.Last!=null&&home.Last.Dismissed,"final native assembly rejects massive manpower deficit despite high commander score");
  Setup();var defender=Enemy(14101,200000,140000);var s=Strength();Check(s.Item1==270000&&s.Item2==140000,"empty native city power still counts local camp defender without targetCity");
  ICGAEMLACJG.HFJIECIBFKB[14102]=defender;s=Strength();Check(s.Item1==270000&&s.Item2==140000,"same field pointer cannot be counted twice");
  target.LMPMNLLGOEC.KJPPGIOCDID.Add(defender.m_dicDeployArmy["commander"][0]);s=Strength();Check(s.Item2==140000,"roster references to deployed soldiers excluded from home census");
  Setup();target.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){FGHFOLKBHDL=15000});s=Strength();Check(s.Item2==15000&&s.Item1>1350,"no commander expedition power does not erase defending garrison");
  Setup();defender=Enemy(14103,200000,140000);AOHIBKLOAEI.RegionAt=p=>home;s=Strength();Check(s.Item1==1350&&s.Item2==0,"remote unrelated army excluded");defender.m_targetCity=target.OJPECMNINNA;s=Strength();Check(s.Item2==140000,"explicit incoming enemy support counted outside target region");
  Setup();defender=Enemy(14104,100000,50000);defender.m_deployState=AKOPKFPKJPP.Assemble;s=Strength();Check(s.Item2==0,"assembly references do not duplicate city troops");
  Setup();defender=Enemy(14105,200000,140000);Drain("CheckAttackMarch",army,army.m_id,target.OJPECMNINNA);Check(army.m_deployState==AKOPKFPKJPP.Camp&&!army.Dismissed,"weaker autonomous march stops for physical recovery, not dismissal");
  var goal=Field<IDictionary>(typeof(StrategicSupport),"campaignGoals")[army.Pointer];Check((bool)goal.GetType().GetField("Returning").GetValue(goal),"return commitment prevents next-day attack oscillation");
  int men=army.GetSoldierNum(false),food=army.Food;var position=army.Position;
  var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");queue.Pump(10000,500,(p,ms)=>{},e=>throw e);Check(army.GetSoldierNum(false)==men&&army.Food==food&&army.Position.x==position.x&&!army.Dismissed,"return preserves soldiers food and physical position");
  Setup();Enemy(14106,1000,200000);army.Combat=1000000;Drain("CheckAttackMarch",army,army.m_id,target.OJPECMNINNA);Check(army.m_deployState==AKOPKFPKJPP.Camp,"extreme officer multiplier cannot send ten thousand into two hundred thousand");
  foreach(int protectedKind in new[]{0,1,2,3,4}){
   Setup();Enemy(14107,200000,140000);if(protectedKind==0)army.m_isPrivate=true;if(protectedKind==1)army.m_targetWarInfo=new();if(protectedKind==2)army.m_warInfo=new();if(protectedKind==3)army.m_isEnterCity=true;if(protectedKind==4)army.m_attackTarget=new();
   Drain("CheckAttackMarch",army,army.m_id,target.OJPECMNINNA);Check(army.m_listMovePos.Count==1&&!army.Dismissed,"private/support/combat/return/explicit army order untouched "+protectedKind);
  }
  Setup();Enemy(14108,150000,20000);var friend=Enemy(14109,100000,15000,"F");friend.m_deployState=AKOPKFPKJPP.Expedition;friend.m_targetCity=target.OJPECMNINNA;friend.Position=new(75);friend.m_listMovePos.Add(target.BMBMKPGEHBH);
  Drain("CheckAttackMarch",army,army.m_id,target.OJPECMNINNA);Check(army.m_deployState==AKOPKFPKJPP.Expedition,"timely converging army prevents mistaken retreat of joint attack");
  Setup();Drain("WarmAttackRoads",army,army.m_id,target.OJPECMNINNA);var roads=Field<IDictionary>(typeof(StrategicSupport),"savedBattleRoads");Check(roads.Count>0,"march prepares real road quotes before battle");
  var war=new NJHDEHLHENN{IJGLCFJOOIK=target,GBHGENNMJCK=target.BMBMKPGEHBH};object[] args={home,war,faction,"prewarm-test",Call("RoadPolicyStamp",faction),0d};Check((bool)Call("SavedBattleQuote",args)&&double.IsFinite((double)args[5]),"battle at prepared endpoint reuses distance and validates corridors");
  target.LMPMNLLGOEC.LMMNKIOAGLC="changed";args[4]=Call("RoadPolicyStamp",faction);Check(!(bool)Call("SavedBattleQuote",args),"ownership change invalidates prewar quote");
  Setup();NavMesh.Blocked=true;Drain("WarmAttackRoads",army,army.m_id,target.OJPECMNINNA);Check(Field<IDictionary>(typeof(StrategicSupport),"savedBattleRoads").Count==0,"unreachable prewar road is never published as reachable");NavMesh.Blocked=false;
  Setup();Enemy(14110,100000,140000);Call("FailedAttackProbe",home,target,200000,500000,"insufficient-attack-manpower");
  for(int day=1;day<=73;day++){NEINKOOBJIH.MIPFHCKDJOK+=1;Check(!(bool)Call("CombatProbeReady",home,"city:"+target.Pointer,200000,new List<Wo_DeployTeamCtrl>(ICGAEMLACJG.HFJIECIBFKB.Values)),"unchanged manpower rejection does not rebuild armies day="+day);}
  ICGAEMLACJG.HFJIECIBFKB.Remove(14110);Check((bool)Call("CombatProbeReady",home,"city:"+target.Pointer,1350,new List<Wo_DeployTeamCtrl>(ICGAEMLACJG.HFJIECIBFKB.Values)),"real loss of defenders reopens manpower probe");
  Muster();AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;Call("ResetWork");Call("ResetOffense");Call("ResetDefense");Console.WriteLine($"PASS Offense141 {checks} defender census, march protection, staged hunt and prewar road regressions; native actions mocked.");
 }
 static void Muster(){
  typeof(Muster123Tests).GetMethod("Setup",F).Invoke(null,null);var city=MICGPAMNDKM.IJPKEMPIFIH["Runan"];var enemy=ICGAEMLACJG.HFJIECIBFKB[1407];var f=NEINKOOBJIH.COCMABJFOFH["F"];
  var samples=Field<IDictionary>(typeof(StrategicSupport),"huntPowerSamples");foreach(var x in samples.Values){x.GetType().GetField("Raw").SetValue(x,1000L);x.GetType().GetField("Budget").SetValue(x,1000000L);x.GetType().GetField("Adjusted").SetValue(x,10000);}
  Check((long)Call("HuntMusterMissing",city,f,enemy)>0,"failed actual assembly cannot be hidden by larger unused roster budget");
  Drain("HuntMusterSteps",f,enemy);Check(ICGAEMLACJG.KBPAJECPNOH.Count>0,"measured shortfall triggers joint native troop shipments");
  int before=ICGAEMLACJG.KBPAJECPNOH.Count;Drain("HuntMusterSteps",f,enemy);Check(ICGAEMLACJG.KBPAJECPNOH.Count==before,"inbound reservation prevents repeated shipments");
  typeof(Hunt94Tests).GetMethod("Setup",F).Invoke(null,new object[]{2});city=MICGPAMNDKM.IJPKEMPIFIH["Runan"];f=NEINKOOBJIH.COCMABJFOFH["F"];bool requestedAll=false;
  city.AssemblyModel=(c,leave,power,eligible)=>{requestedAll=power==int.MaxValue;return null;};Drain("HuntSteps",f);Check(requestedAll,"hunt asks native allocator for safe pool instead of underfilled threshold army");
 }
}
