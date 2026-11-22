using System;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;
public partial class MKMOJNAJKJE {
 public Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl> HMLLPOCACMJ=new();
 public Dictionary<Wo_DeployTeamCtrl,AFAGFHJONFJ> OEIKEALIMEP=new();
}
public static class Intercept138Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int n;static Wo_DeployTeamCtrl army,enemy;static MKMOJNAJKJE owner;static AFAGFHJONFJ home;
 static object Call(string name,params object[] a)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,a);
 static void Check(bool ok,string why){n++;if(!ok)throw new Exception("Intercept138: "+why);}
 static void Setup(){
  typeof(Intercept90Tests).GetMethod("Setup",F).Invoke(null,null);
  army=(Wo_DeployTeamCtrl)typeof(Intercept90Tests).GetField("army",F).GetValue(null);enemy=(Wo_DeployTeamCtrl)typeof(Intercept90Tests).GetField("enemy",F).GetValue(null);
  owner=NEINKOOBJIH.COCMABJFOFH[army.m_belongPower];home=MICGPAMNDKM.IJPKEMPIFIH[army.m_assembleCity];
  army.m_listHero.Add(new(){OJPECMNINNA="intercept138"});army.m_deployState=AKOPKFPKJPP.Expedition;army.m_attackTarget=enemy;army.m_listMovePos.Add(new(70));owner.HMLLPOCACMJ.Add(army);
 }
 static object Begin(params Wo_DeployTeamCtrl[] teams){var list=new Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl>();foreach(var t in teams)list.Add(t);object[] a={owner,list,null};Call("BeforeNativeInterception",a);return a;}
 static void End(object a)=>Call("AfterNativeInterception",((object[])a)[2]);
 static int Allowed(object a)=>((Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl>)((object[])a)[1]).Count;
 static bool Guard()=> (bool)Call("PreserveDeployedInterception",army);
 static bool Relevant()=> (bool)Call("InterceptRelevant",new object[]{army,enemy,null});
 public static void Run(){
  Setup();enemy.m_targetCity="";AOHIBKLOAEI.RegionAt=p=>home;Check(!Relevant(),"passing hostile in own region is not an attack");AOHIBKLOAEI.RegionAt=null;Check(!Relevant(),"unknown region is not permission");
  var s=Begin(enemy);Check(Allowed(s)==0,"candidate removed before native assembly creation");int soldiers=army.GetSoldierNum(false),food=army.GetFood();var pos=army.Position;
  Check(!(bool)Call("RecordDismissal",army),"no-candidate native Dismiss is suppressed before clearing order registries");End(s);
  Check(!army.Dismissed&&!army.m_responseToPlayer&&army.m_responseTeam==null,"invalid existing field pursuit transitions to physical return");
  Check(army.GetSoldierNum(false)==soldiers&&army.GetFood()==food&&army.Position.x==pos.x,"transition conserves troops food and position");Check(!Guard(),"dismissal exemption ends with native scope");
  Setup();s=Begin(enemy);Check(Allowed(s)==1&&Guard(),"actual attack on home preserved across no-candidate cleanup");End(s);Check(army.m_attackTarget==enemy&&army.m_listMovePos.Count>0&&!army.Dismissed,"valid moving pursuit is not reset");
  Setup();owner.AENJACOFBJK["ally"]=new(){EBIOPDHEBOF=BHBNKONCOJD.Ally};home.LMPMNLLGOEC.LMMNKIOAGLC="ally";Check(Relevant(),"direct allied city attack accepted even after home ownership changed");
  owner.AENJACOFBJK["ally"].EBIOPDHEBOF=BHBNKONCOJD.ArmyPass;Check(!Relevant(),"transit treaty cannot authorize interception");
  Setup();enemy.m_isEnterCity=true;Check(!Relevant(),"city admission is not siege intent");
  Setup();enemy.m_targetCity="";enemy.m_targetWarInfo=new(){EMMJBPMNFAK=new[]{"enemy",army.m_belongPower},IJGLCFJOOIK=home};Check(Relevant(),"enemy relief to own live battle accepted");enemy.m_targetWarInfo.FGHFOLKBHDL[0]=0;Check(!Relevant(),"finished battle ceases to authorize pursuit");
  Setup();s=Begin(enemy);enemy.m_targetCity="";End(s);Check(!army.m_responseToPlayer,"purpose changes during native evaluation are rechecked");
  Setup();s=Begin(enemy);ICGAEMLACJG.HFJIECIBFKB.Remove(enemy.m_id);End(s);Check(!army.m_responseToPlayer,"removed opponent does not retain stale pointer pursuit");
  Setup();s=Begin(enemy);army.m_id++;Check(!Guard(),"pooled interceptor identity never protected");End(s);
  foreach(int kind in new[]{0,1,2,3,4,5,6}){
   Setup();s=Begin(enemy);
   if(kind==0)army.m_isPrivate=true;if(kind==1)army.m_warInfo=new();if(kind==2)army.m_deployState=AKOPKFPKJPP.Assemble;
   if(kind==3)army.m_isEnterCity=true;if(kind==4)army.m_targetWarInfo=new();if(kind==5)army.m_targetCity="city";if(kind==6)army.m_listHero.Clear();
   Check(!Guard(),"private/combat/assembly/arrival/relief/explicit-city/empty-officer dismissal remains native "+kind);End(s);
  }
  Setup();s=Begin(enemy);var other=new Wo_DeployTeamCtrl{m_id=555};Check(!(bool)Call("PreserveDeployedInterception",other),"unrelated army not protected by another army's native scope");End(s);
  Setup();s=Begin(enemy);owner.OEIKEALIMEP[enemy]=home;Check(!Guard(),"non-cleanup native branch is not exempt from dismissal");End(s);
  Setup();s=Begin(enemy);foreach(var group in army.m_dicDeployArmy.Values)foreach(var u in group)u.FGHFOLKBHDL=0;Check(!Guard(),"zero surviving soldiers still allow native removal");End(s);
  Setup();s=Begin(enemy);var manualFaction=NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC;NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC=army.m_belongPower;
  var manualKey=Call("ManualKey",army.m_belongPower,"team",army.m_id.ToString());Call("QueueWrite",manualKey,-1);Check(!Guard(),"manual order acquired during native scope is respected");End(s);Call("QueueWrite",manualKey,0);Call("ResetManualOrders");NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC=manualFaction;
  Setup();var outer=Begin(enemy);var inner=Begin(enemy);End(inner);Check(Guard(),"nested finalizer restores outer scope");End(outer);Check(!Guard(),"outer finalizer restores no exemption, including exceptional native exit");
  Setup();enemy.m_targetCity="";s=Begin(enemy);End(s);
  var work=(WorkQueue)typeof(StrategicSupport).GetField("work",F).GetValue(null);work.Pump(100,200,(a,b)=>{},e=>throw e);
  Check(army.m_isEnterCity&&army.m_listMovePos.Count>0&&!army.Dismissed,"cancelled chase actually issues a native return movement instead of permanent camp");
  Check(army.GetSoldierNum(false)==10000,"physical return keeps original soldiers");
  for(int day=0;day<120;day++){
   Setup();bool threatens=day%3==0;if(!threatens)enemy.m_targetCity="";
   var original=new Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl>();original.Add(enemy);object[] args={owner,original,null};Call("BeforeNativeInterception",args);
   Check(original.Count==1&&Allowed(args)==(threatens?1:0),"daily candidate filtering does not mutate shared input "+day);
   // Model the exact native no-candidate branch: attempts Dismiss, then clears
   // the faction's transient response list before returning false.
   if(!threatens)Check(!(bool)Call("RecordDismissal",army),"daily cleanup must not teleport field troops "+day);
   owner.HMLLPOCACMJ.Clear();End(args);
   Check(army.m_responseToPlayer==threatens&&!army.Dismissed&&army.GetSoldierNum(false)==10000,"120 decisions preserve only valid pursuit and all soldiers "+day);
  }
  Setup();owner.AENJACOFBJK["ally"]=new(){EBIOPDHEBOF=BHBNKONCOJD.Ally};
  var defended=new AFAGFHJONFJ();defended.LMPMNLLGOEC.LMMNKIOAGLC="enemy";
  enemy.m_targetWarInfo=new(){EMMJBPMNFAK=new[]{"enemy","ally"},IJGLCFJOOIK=defended};
  Check(!Relevant(),"allied invader does not authorize interception of defending reinforcements");
  s=Begin(enemy);Check(Allowed(s)==0,"defending relief excluded BEFORE native sortie even with stale hostile city field");End(s);
  Check(!army.m_responseToPlayer,"existing interception of defending relief returns");
  enemy.m_warInfo=enemy.m_targetWarInfo;enemy.m_targetWarInfo=null;Check(!Relevant(),"already defending city does not become hostile allied-defense purpose");
  enemy.m_warInfo.IJGLCFJOOIK=null;Check(!Relevant(),"unclassified field battle cannot infer aggressor from opposing faction alone");
  enemy.m_warInfo.IJGLCFJOOIK=home;Check(!Relevant(),"city owner must actually belong to opposing battle side");
  enemy.m_warInfo.EMMJBPMNFAK=new[]{"enemy",army.m_belongPower};Check(Relevant(),"enemy attacking own defended city remains eligible");
  Call("ResetWork");AOHIBKLOAEI.RegionAt=null;Console.WriteLine($"PASS Intercept138 {n} target filtering, scoped cleanup, physical return and preservation checks; native callbacks simulated.");
 }
}
