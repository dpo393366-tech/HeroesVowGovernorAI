using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class Camp117Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static Wo_DeployTeamCtrl t;static MKMOJNAJKJE f;static AFAGFHJONFJ home,near;static object goal;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static T Field<T>(string name)=>(T)typeof(StrategicSupport).GetField(name,F).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Camp117: "+why);}
 static AFAGFHJONFJ City(string id,float x){var c=new AFAGFHJONFJ{OJPECMNINNA=id,Capacity=30,BMBMKPGEHBH=new(x)};c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.LMPMNLLGOEC.OEGNGFBCHFL=10000000;MICGPAMNDKM.IJPKEMPIFIH[id]=c;ICGAEMLACJG.FKKCPDEFODO[id]=new(){City=c};return c;}
 static void Setup(int cities=80){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");Call("ResetDefense");Call("ResetBattleState");Call("ResetManualOrders");MLGOLGGHLMB.CBEGDDKANAO.Clear();
  t=(Wo_DeployTeamCtrl)typeof(Campaign81Tests).GetField("t",F).GetValue(null);home=(AFAGFHJONFJ)typeof(Campaign81Tests).GetField("home",F).GetValue(null);f=NEINKOOBJIH.COCMABJFOFH["F"];
  NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";NEINKOOBJIH.MIPFHCKDJOK=100;
  AOHIBKLOAEI.Region=null;AOHIBKLOAEI.RegionAt=null;NavMesh.Blocked=false;NavMesh.Detour=0;Field<IDictionary>("roadChoices").Clear();
  t.ClearTarget(false);t.m_listMovePos.Clear();t.m_isEnterCity=true;t.m_targetCity=home.OJPECMNINNA;Field<IDictionary>("campaignGoals").Clear();Call("RememberCampaign",t);goal=Field<IDictionary>("campaignGoals")[t.Pointer];t.m_targetCity="";
  near=City("near117",60);for(int i=0;i<cities;i++)City("far117-"+i,1000+i*20);
 }
 static int Partition(Action<string> action=null){int steps=0;foreach(var phase in (IEnumerable<string>)Call("ReturnPartitionSteps",t,goal,f)){steps++;action?.Invoke(phase);}return steps;}
 static IEnumerator<string> Busy(Action action){while(true){action();yield return "busy";}}
 public static void Run(){
  foreach(int size in new[]{10,46,80,140}){
   Setup(size);NavMesh.Calls105=0;int steps=Partition();Check(t.m_targetCity==near.OJPECMNINNA&&t.m_isEnterCity,"nearest legal road receives actual return order at world size "+size);
   Check(steps<12&&NavMesh.Calls105<12,"far-city count does not stretch nearby return to hundreds of slices at size "+size);
   Console.WriteLine($"Camp117 return size={size} slices={steps} nativePathCalls={NavMesh.Calls105}");
  }
  Setup();bool changed=false;Partition(p=>{if(!changed&&p=="return-partition-order"){changed=true;near.Capacity=0;}});Check(changed&&t.m_isEnterCity&&t.m_targetCity==home.OJPECMNINNA,"filled nearest city rechecks capacity and tries next real road");
  Setup();AOHIBKLOAEI.RegionAt=p=>p.x<55?home:near;home.MEKIDAONKMD[near.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){City("detour117",-500)}};Partition();Check(t.m_targetCity==home.OJPECMNINNA,"geometric neighbor loses to shorter real road when its road detours");
  Setup();changed=false;Partition(p=>{if(!changed&&p=="return-partition-nearest-return-city"){changed=true;near.LMPMNLLGOEC.LMMNKIOAGLC="enemy";}});Check(changed&&t.CityMoves==0,"ownership change during scan invalidates decision before issuing order");
  Setup();NavMesh.Blocked=true;Partition();Check(t.CityMoves==0&&!t.Dismissed,"blocked native paths do not fabricate return or delete army");
  Setup();changed=false;Partition(p=>{if(!changed){changed=true;t.m_warInfo=new();}});Check(t.CityMoves==0,"entering battle cancels in-flight recovery search");
  Setup();t.m_deployState=AKOPKFPKJPP.Expedition;t.m_targetCity=near.OJPECMNINNA;t.m_listMovePos.Add(near.BMBMKPGEHBH);
  Call("WatchCampaignMarch",t);NEINKOOBJIH.MIPFHCKDJOK+=.9;Call("WatchCampaignMarch",t);Check(t.m_deployState==AKOPKFPKJPP.Expedition,"new native march gets time to start");
  NEINKOOBJIH.MIPFHCKDJOK+=.2;Call("WatchCampaignMarch",t);Check((bool)Call("CampIdle",t)&&Field<WorkQueue>("work").Count>0,"motionless march with stale path enters recovery after one allied period");Field<WorkQueue>("work").Pump(10000,10000,(_,__)=>{},e=>throw e);Check(t.m_isEnterCity&&t.m_listMovePos.Count>0,"watchdog reaches a fresh native movement command");
  Setup();t.m_deployState=AKOPKFPKJPP.Expedition;t.m_targetCity=near.OJPECMNINNA;Call("WatchCampaignMarch",t);NEINKOOBJIH.MIPFHCKDJOK++;t.Position=new(51);Call("WatchCampaignMarch",t);Check(t.m_targetCity==near.OJPECMNINNA&&t.CityMoves==0,"real movement resets watchdog");
  Setup();t.m_deployState=AKOPKFPKJPP.Expedition;t.m_targetCity="missing117";Call("WatchCampaignMarch",t);NEINKOOBJIH.MIPFHCKDJOK++;Call("WatchCampaignMarch",t);Check((bool)Call("CampIdle",t),"stale explicit target with no path no longer waits forever");
  Setup();NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="other";t.m_deployState=AKOPKFPKJPP.Expedition;t.m_targetCity=near.OJPECMNINNA;Call("WatchCampaignMarch",t);NEINKOOBJIH.MIPFHCKDJOK+=2;Call("WatchCampaignMarch",t);Check(t.m_targetCity==near.OJPECMNINNA,"NPC keeps three-day period");NEINKOOBJIH.MIPFHCKDJOK++;Call("WatchCampaignMarch",t);Check((bool)Call("CampIdle",t),"NPC stalled march recovers on third day");
  foreach(string mode in new[]{"private","battle","reinforcement","manual"}){
   Setup();t.m_deployState=AKOPKFPKJPP.Expedition;t.m_targetCity=near.OJPECMNINNA;Call("WatchCampaignMarch",t);
   if(mode=="private")t.m_isPrivate=true;if(mode=="battle")t.m_warInfo=new();if(mode=="reinforcement")t.m_targetWarInfo=new();if(mode=="manual")MLGOLGGHLMB.CBEGDDKANAO["local.heroesvow.governor/manual/v1/F/team/"+t.m_id]=-1;
   NEINKOOBJIH.MIPFHCKDJOK+=10;Call("WatchCampaignMarch",t);Check(t.m_targetCity==near.OJPECMNINNA&&t.CityMoves==0,"watchdog respects "+mode);
  }
  for(int n=1;n<=12;n++){
   var q=new WorkQueue();int recovered=0,normal=0,urgent=0,battle=0,recruit=0;
   for(int i=0;i<300;i++)q.Add("defense:"+i,()=>Busy(()=>urgent++));for(int i=0;i<100;i++)q.Add("city:"+i,()=>Busy(()=>normal++));
   for(int i=0;i<n;i++)q.Add("campaign:"+i,()=>Busy(()=>recovered++));q.Add("battle-support-plan:F",()=>Busy(()=>battle++));q.Add("recruit-queue:F",()=>Busy(()=>recruit++));
   q.Pump(10000,400,(_,__)=>{},e=>throw e);Check(recovered>=60&&normal>0&&urgent>0&&battle==100&&recruit>0,"400 queued jobs preserve recovery, combat and ordinary progress, armies="+n);q.Clear();Check(q.Count==0,"recovery lane cleared on world switch");
  }
  Call("ResetWork");Call("ResetDefense");Call("ResetManualOrders");AOHIBKLOAEI.RegionAt=null;NavMesh.Blocked=false;
  Console.WriteLine("PASS Camp117 "+checks+" road-search scaling, stale marching, races and scheduling checks; native movement mocked, not live FPS.");
 }
}
