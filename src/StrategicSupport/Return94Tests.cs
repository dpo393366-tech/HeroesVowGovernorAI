using System;using System.Reflection;using System.Collections;using System.Collections.Generic;using System.Linq;using UnityEngine;
public static class Return94Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int count;static Wo_DeployTeamCtrl army;static MKMOJNAJKJE faction;static AFAGFHJONFJ far,near,home;static object goal;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
 static void Check(bool b,string message){count++;if(!b)throw new Exception("Return94: "+message);}
 static void Setup(){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");ICGAEMLACJG.KBPAJECPNOH.Clear();
  army=Field<Wo_DeployTeamCtrl>(typeof(Campaign81Tests),"t");faction=Field<MKMOJNAJKJE>(typeof(Campaign81Tests),"f");far=Field<AFAGFHJONFJ>(typeof(Campaign81Tests),"target");home=Field<AFAGFHJONFJ>(typeof(Campaign81Tests),"home");
  far.LMPMNLLGOEC.LMMNKIOAGLC="F";far.LMPMNLLGOEC.IPJHOOJBCKI="gov";far.Capacity=30;far.LMPMNLLGOEC.OEGNGFBCHFL=10000000;
  near=new(){OJPECMNINNA="near-return",Capacity=30,BMBMKPGEHBH=new(60)};near.LMPMNLLGOEC.LMMNKIOAGLC="F";near.LMPMNLLGOEC.IPJHOOJBCKI="gov";near.LMPMNLLGOEC.OEGNGFBCHFL=10000000;MICGPAMNDKM.IJPKEMPIFIH[near.OJPECMNINNA]=near;ICGAEMLACJG.FKKCPDEFODO[near.OJPECMNINNA]=new(){City=near};
  ((IDictionary)typeof(StrategicSupport).GetField("campaignGoals",F).GetValue(null)).Clear();army.m_isEnterCity=true;army.m_targetCity=far.OJPECMNINNA;army.m_deployState=AKOPKFPKJPP.Expedition;army.m_listMovePos.Add(far.BMBMKPGEHBH);Call("RememberCampaign",army);goal=((IDictionary)typeof(StrategicSupport).GetField("campaignGoals",F).GetValue(null))[army.Pointer];
  AOHIBKLOAEI.Region=null;AOHIBKLOAEI.RegionAt=null;UnityEngine.AI.NavMesh.Detour=0;UnityEngine.AI.NavMesh.Blocked=false;
 }
 static void Evaluate(Action<string> phase=null){foreach(var s in (IEnumerable<string>)Call("ReturnWatchSteps",army,goal))phase?.Invoke(s);}
 public static void Run(){
  Setup();Evaluate();Check(army.m_targetCity==near.OJPECMNINNA&&army.m_isEnterCity,"moving army changes distant return to nearest actual road with capacity");Check(army.Position.x==50&&!army.Dismissed,"reroute neither teleports nor dismisses soldiers");
  Setup();near.Capacity=1;var inbound=new Wo_DeployTeamCtrl{m_id=9481,m_belongPower="F",m_targetCity=near.OJPECMNINNA,m_isEnterCity=true,m_deployState=AKOPKFPKJPP.Expedition};inbound.m_dicDeployArmy["other"]=new(){new(){FGHFOLKBHDL=1000}};ICGAEMLACJG.HFJIECIBFKB[9481]=inbound;Evaluate();Check(army.m_targetCity!=near.OJPECMNINNA,"other returning army reserves last city slot before arrival");
  Setup();bool filled=false;Evaluate(phase=>{if(!filled&&phase=="return-live-choice"){filled=true;near.Capacity=0;}});Check(filled&&army.m_targetCity!=near.OJPECMNINNA,"city filling during yielded scan cannot receive stale assignment");
  Setup();near.Capacity=0;Evaluate();near.Capacity=1;Call("RememberCampaign",army);goal=((IDictionary)typeof(StrategicSupport).GetField("campaignGoals",F).GetValue(null))[army.Pointer];Evaluate();Check(army.m_targetCity==near.OJPECMNINNA,"newly available closer city picked on next live assessment");
  Setup();near.MEKIDAONKMD[far.OJPECMNINNA]=new();AOHIBKLOAEI.RegionAt=p=>p.x<55?home:p.x<70?near:far;home.MEKIDAONKMD[near.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){new AFAGFHJONFJ{OJPECMNINNA="detour",BMBMKPGEHBH=new(-500),LMPMNLLGOEC=new(){LMMNKIOAGLC="F"}}}};Evaluate();Check(army.m_targetCity!=near.OJPECMNINNA,"geometrically close city with long actual road is not preferred");
  Setup();army.m_isPrivate=true;Evaluate();Check(army.m_targetCity==far.OJPECMNINNA,"private marching army not retargeted");
  Setup();army.m_targetWarInfo=new();Evaluate();Check(army.m_targetCity==far.OJPECMNINNA,"battle reinforcement destination not replaced by return optimizer");
  Setup();near.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Evaluate();Check(army.m_targetCity!=near.OJPECMNINNA,"captured closest city excluded");
  Setup();near.Capacity=0;far.Capacity=0;home.Capacity=0;Evaluate();Check(army.m_deployState==AKOPKFPKJPP.Camp&&army.m_listMovePos.Count==0,"invalid full return order stops and hands off to split/safe staging, no overflow entry");
  Setup();near.Capacity=1;army.m_listHero.Clear();army.m_listHero.Add(new(){OJPECMNINNA="single-commander"});army.m_dicDeployArmy.Values.Single().Add(new(){FGHFOLKBHDL=1000});Evaluate();Check(army.m_deployState==AKOPKFPKJPP.Expedition&&army.m_targetCity!=near.OJPECMNINNA,"single indivisible officer group does not repeatedly stop for incompatible one-slot city");
  Call("ResetWork");AOHIBKLOAEI.RegionAt=null;Console.WriteLine("PASS Return94 "+count+" live return rerouting, capacity race, road-distance and manual/battle safety checks; native calls mocked.");
 }
}
