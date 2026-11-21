using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class RouteOrder94Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int n;static AFAGFHJONFJ source,near;static NJHDEHLHENN war;static MKMOJNAJKJE faction;static List<string> sent;
 static object Call(string m,params object[] a)=>typeof(StrategicSupport).GetMethod(m,F).Invoke(null,a);
 static T Field<T>(Type t,string name)=>(T)t.GetField(name,F).GetValue(null);
 static void Set(string name,object x)=>typeof(StrategicSupport).GetField(name,F).SetValue(null,x);
 static void Check(bool b,string why){n++;if(!b)throw new Exception("RouteOrder94: "+why);}
 static void Setup(){new StrategicSupport().Load();
  typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetBattleState");Call("ResetDefense");Call("ResetWork");Set("schedulerOnline",false);
  source=Field<AFAGFHJONFJ>(typeof(BattleIntegrationTests),"source");war=Field<NJHDEHLHENN>(typeof(BattleIntegrationTests),"war");faction=NEINKOOBJIH.COCMABJFOFH["F"];
  AOHIBKLOAEI.Region=null;AOHIBKLOAEI.RegionAt=null;NavMesh.Blocked=false;NavMesh.Detour=0;sent=new();source.Generated=()=>sent.Add(source.OJPECMNINNA);source.BMBMKPGEHBH=new(0);Wa_Controller.m_enemyCurSoldierNum=200000;
  near=Add("near",80);Call("Ready");
 }
 static AFAGFHJONFJ Add(string id,float pos){
  var c=new AFAGFHJONFJ{OJPECMNINNA=id,BMBMKPGEHBH=new(pos)};c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="gov";for(int i=0;i<10;i++)c.LMPMNLLGOEC.KJPPGIOCDID.Add(new());
  c.Roster=Enumerable.Range(0,5).Select(i=>new CLAGKDECFID{OJPECMNINNA=id+i,LMPMNLLGOEC=new(){LMMNKIOAGLC="F",CNOMNBCPJDI=id}}).ToList();c.Generated=()=>sent.Add(id);MICGPAMNDKM.IJPKEMPIFIH[id]=c;return c;
 }
 static void Assess()=>Call("Assess",war,0d,true);
 static void Drain(){var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");for(int i=0;queue.Count>0&&i<10000;i++)queue.Pump(1000,1,(s,ms)=>{},e=>throw e);Check(queue.Count==0,"bounded coroutine finishes all candidates");}
 static (Wo_DeployTeamCtrl,AFAGFHJONFJ,AFAGFHJONFJ) Completed(){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");var t=Field<Wo_DeployTeamCtrl>(typeof(Campaign81Tests),"t");var c=Field<AFAGFHJONFJ>(typeof(Campaign81Tests),"target");var home=Field<AFAGFHJONFJ>(typeof(Campaign81Tests),"home");
  c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="player";c.Capacity=30;t.Position=c.BMBMKPGEHBH;t.m_deployState=AKOPKFPKJPP.Expedition;t.m_tmpTargetCity=c.OJPECMNINNA;t.m_listMovePos.Clear();t.GAANFLKOMKL.Clear();AOHIBKLOAEI.RegionAt=p=>p.x>90?c:home;return(t,c,home);
 }
 public static void Run(){
  Setup();Assess();Check(sent.Count>0&&sent[0]=="near","nearer city departs first despite distant source insertion order");
  Setup();near.LMPMNLLGOEC.OEGNGFBCHFL=0;Assess();Check(sent.SequenceEqual(new[]{"source"}),"farther source used when nearest has no food");
  Setup();source.HomeOfficerCount=1;near.HomeOfficerCount=1;near.Roster=null;for(int i=0;i<13;i++){var blocked=Add("blocked"+i,90-i);blocked.Roster=null;blocked.HomeOfficerCount=1;}var last=Add("last",-20);Set("schedulerOnline",true);Assess();Drain();Check(sent.Contains("last"),"more than twelve blocked nearer sources cannot starve farther relief");Set("schedulerOnline",false);
  Setup();string key=war.Pointer+":"+war.HIENOINGBCN+":F:"+source.OJPECMNINNA;foreach(var p in (IEnumerable<string>)Call("BattleRouteQuote",source,war,faction,key)){}Set("schedulerOnline",true);Assess();Check(sent.Count==0,"ready distant quote does not launch while nearer route is pending");Drain();Check(sent[0]=="near","sliced route batch preserves true distance priority");Set("schedulerOnline",false);
  Setup();Set("schedulerOnline",true);Assess();Call("ForgetBattle",war);Drain();Check(sent.Count==0,"ended battle invalidates pending support plan");Set("schedulerOnline",false);
  Setup();var dest=Add("destination",100);var detour=Add("detour",-500);source.MEKIDAONKMD[dest.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){detour}};source.FKMKGNPFLFP.Add(near);near.FKMKGNPFLFP.Add(dest);AOHIBKLOAEI.RegionAt=p=>p.x<1?source:p.x<90?near:dest;
  object[] route={source.BMBMKPGEHBH,dest.BMBMKPGEHBH,faction,null,false,null,0d};Check((bool)Call("ReliefPath",route)&&Math.Abs((double)route[6]-100)<.01,"valid but long native preset replaced by shorter legal road graph");
  near.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check((bool)Call("ReliefPath",route)&&(double)route[6]>1000,"captured intermediate city invalidates shortest route cache immediately");NavMesh.Blocked=true;Check(!(bool)Call("ReliefPath",route),"cached route cannot bypass fresh physical path failure");NavMesh.Blocked=false;AOHIBKLOAEI.RegionAt=null;
  var (t,c,home)=Completed();Check((bool)Call("AdoptCompletedCityOrder",t),"captured target retained on manual expedition is adopted");Check(t.m_deployState==AKOPKFPKJPP.Camp&&t.m_targetCity==""&&t.m_tmpTargetCity=="","completed primary and backup order removed before reassessment");Check(((IEnumerable<AFAGFHJONFJ>)Call("ReturnCities",NEINKOOBJIH.COCMABJFOFH["F"])).Contains(c),"captured personal city remains eligible as safe return destination");
  (t,c,home)=Completed();home.LMPMNLLGOEC.IPJHOOJBCKI="player";Check((bool)Call("AdoptCompletedCityOrder",t),"completed manually sourced expedition enters postbattle recovery");object[] controlled={t,null};Check((bool)Call("CampaignControlled",controlled),"completed order marker permits recovery without enabling personal city management");
  (t,c,home)=Completed();t.m_listMovePos.Add(new(101));Check(!(bool)Call("AdoptCompletedCityOrder",t),"live native path preserved");
  (t,c,home)=Completed();t.GAANFLKOMKL.Add(new(101));Check(!(bool)Call("AdoptCompletedCityOrder",t),"pending native waypoint preserved");
  (t,c,home)=Completed();t.Position=new(50);Check(!(bool)Call("AdoptCompletedCityOrder",t),"distant friendly city command not assumed completed");
  (t,c,home)=Completed();c.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!(bool)Call("AdoptCompletedCityOrder",t),"unfinished hostile target preserved");
  (t,c,home)=Completed();t.m_warInfo=new();Check(!(bool)Call("AdoptCompletedCityOrder",t),"active battle preserved");
  (t,c,home)=Completed();t.m_isPrivate=true;Check(!(bool)Call("AdoptCompletedCityOrder",t),"private army preserved");
  (t,c,home)=Completed();t.m_responseToPlayer=true;Check(!(bool)Call("AdoptCompletedCityOrder",t),"follow-player army preserved");
  (t,c,home)=Completed();t.m_tmpTargetCity="new-order";Check(!(bool)Call("AdoptCompletedCityOrder",t),"new conflicting target preserved");
  AOHIBKLOAEI.RegionAt=null;Set("schedulerOnline",false);Call("ResetWork");Console.WriteLine($"PASS RouteOrder94 {n} ordered support, shortest legal roads, cache invalidation and completed manual orders; native movement mocked, not gameplay.");
 }
}
