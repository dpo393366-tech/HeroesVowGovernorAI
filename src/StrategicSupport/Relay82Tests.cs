using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
public partial class Wo_DeployTeamCtrl {
 public void MoveToWarInfo(NJHDEHLHENN w,bool option){m_targetWarInfo=w;m_targetCity="";m_deployState=AKOPKFPKJPP.Expedition;m_listMovePos.Clear();m_listMovePos.Add(w.GBHGENNMJCK);}
}
public static class Relay82Tests {
 static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static void Check(bool b,string s){checks++;if(!b)throw new Exception("R82: "+s);}
 static int Need(double at,List<ReinforcementTimeline.Wave> waves,out string reason)=>ReinforcementTimeline.Need(10000,30000,100,0,at,at,waves,new List<ReinforcementTimeline.Wave>(),1.15,0,out reason,out _);
 public static void RunAll(){
  Check(Need(120,new List<ReinforcementTimeline.Wave>(),out string reason)==0&&reason=="collapse-before-wave","far city alone cannot rescue a collapsed front");
  Check(Need(40,new List<ReinforcementTimeline.Wave>(),out _)>0,"near city can send before collapse despite insufficient final strength");
  Check(Need(120,new List<ReinforcementTimeline.Wave>(){new(40,5000)},out _)>0,"near first wave extends survival for later distant wave");
  Check(Need(170,new List<ReinforcementTimeline.Wave>(){new(40,5000),new(120,6000)},out _)>0,"second wave extends survival for third wave");
  Check(Need(120,new List<ReinforcementTimeline.Wave>(){new(130,50000)},out reason)==0&&reason=="collapse-before-wave","late incoming soldiers not counted before arrival");
  Check(Need(120,new List<ReinforcementTimeline.Wave>(){new(100,5000)},out reason)==0,"arrival exactly at collapse is not assumed safe");
  Check(Need(120,new List<ReinforcementTimeline.Wave>(){new(40,50000)},out reason)==0&&reason=="covered-by-timed-waves","adequate timely wave prevents redundant armies");
  Check(Need(120,new List<ReinforcementTimeline.Wave>(){new(40,1000)},out reason)==0,"too-small first wave does not falsely enable late rescue");
  Check(ReinforcementTimeline.Need(10000,10000,0,0,20,20,new List<ReinforcementTimeline.Wave>(),new List<ReinforcementTimeline.Wave>(),1.15,0,out _,out _)==0,"equal stable front avoids reciprocal arms race");
  Check(ReinforcementTimeline.Need(10000,10000,0,0,20,60,new List<ReinforcementTimeline.Wave>(),new List<ReinforcementTimeline.Wave>(){new(40,20000)},1.15,0,out _,out _)>0,"known enemy follow-up triggers preventive reinforcement");
  Check(ReinforcementTimeline.Need(10000,0,100,0,20,80,new List<ReinforcementTimeline.Wave>(),new List<ReinforcementTimeline.Wave>(){new(60,30000)},1.15,60,out _,out _)>0,"precontact defender not depleted before enemy arrives");
  Check(ReinforcementTimeline.Need(10000,30000,10,10000,20,20,new List<ReinforcementTimeline.Wave>(),new List<ReinforcementTimeline.Wave>(),1.15,0,out _,out _)>0,"short enemy casualty burst never promises total victory");
  Check(ReinforcementTimeline.Need(10000,30000,100,0,double.PositiveInfinity,120,new List<ReinforcementTimeline.Wave>(),new List<ReinforcementTimeline.Wave>(),1.15,0,out _,out _)==0,"invalid ETA rejected");
  Check(ReinforcementTimeline.Need(10000,30000,100,0,20,20,new List<ReinforcementTimeline.Wave>(){new(double.NaN,5000)},new List<ReinforcementTimeline.Wave>(),1.15,0,out _,out _)==0,"invalid arrival rejected");
  // Reset/load and day-boundary scheduling use real iterator/queue integration.
  typeof(Campaign81Tests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);
  Call("ResetDefense");Call("ResetWork");
  foreach(var step in (IEnumerable<string>)Call("DefensePulse")){}
  var queue=(WorkQueue)typeof(StrategicSupport).GetField("work",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
  Check(queue.Count>=2,"daily pulse schedules defense and city decisions without native governor callback");
  int n=queue.Count;foreach(var step in (IEnumerable<string>)Call("DefensePulse")){}Check(queue.Count==n,"same-day duplicate jobs coalesced");
  Call("ResetWork");foreach(var step in (IEnumerable<string>)Call("DefensePulse")){}Check(queue.Count==0,"unchanged day does not requeue complete city analysis");
  NEINKOOBJIH.MIPFHCKDJOK++;foreach(var step in (IEnumerable<string>)Call("DefensePulse")){}Check(queue.Count>=2,"next day resumes governor decisions");
  Call("ResetWork");Call("ResetDefense");
  BattleRelay();DefenseLifecycle();
  var random=new Random(8219);
  for(int i=0;i<1000;i++){
   double own=random.Next(1000,50000),foe=random.Next(50000,100000),loss=random.Next(1,50),eta=random.Next(1,15);
   int baseline=ReinforcementTimeline.Need(own,foe,loss,0,eta,eta,new List<ReinforcementTimeline.Wave>(),new List<ReinforcementTimeline.Wave>(),1.15,0,out _,out _);
   int aided=ReinforcementTimeline.Need(own,foe,loss,0,eta,eta,new List<ReinforcementTimeline.Wave>{new(0,1000)},new List<ReinforcementTimeline.Wave>(),1.15,0,out _,out _);
   Check(aided<=baseline,"earlier real reinforcement cannot increase needed additional soldiers");
  }
  Console.WriteLine($"PASS {checks} R82 relay timeline and daily scheduler checks; native movement not verified");
 }
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
 static void BattleRelay(){
  typeof(BattleIntegrationTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);
  UnityEngine.AI.NavMesh.Blocked=false;UnityEngine.AI.NavMesh.Detour=0;AOHIBKLOAEI.Region=null;
  Vow.Waves.ReserveWaves.Set(null,90);
  var near=Field<AFAGFHJONFJ>(typeof(BattleIntegrationTests),"source");
  AFAGFHJONFJ City(string name,float x){var c=(AFAGFHJONFJ)typeof(BattleIntegrationTests).GetMethod("City",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,new object[]{name,"F"});c.BMBMKPGEHBH=new(x);return c;}
  var far=City("far",-160);var last=City("last",-190);var w=Field<NJHDEHLHENN>(typeof(BattleIntegrationTests),"war");var g=NEINKOOBJIH.COCMABJFOFH["F"].ANMMMOIKLEG["gov"];
  g.BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Wa_Controller.m_myCurSoldierNum=30000;Wa_Controller.m_enemyCurSoldierNum=60000;
  Call("Assess",w,0d,true);Wa_Controller.m_myCurSoldierNum=28000;Call("Assess",w,10d,true);
  Check(near.Calls+far.Calls+last.Calls>0,"StopWar permits defensive support");
  typeof(BattleIntegrationTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);near=Field<AFAGFHJONFJ>(typeof(BattleIntegrationTests),"source");far=City("far",-160);last=City("last",-190);w=Field<NJHDEHLHENN>(typeof(BattleIntegrationTests),"war");g=NEINKOOBJIH.COCMABJFOFH["F"].ANMMMOIKLEG["gov"];Wa_Controller.m_enemyCurSoldierNum=60000;
  Wa_Controller.m_myCurSoldierNum=26000;g.BNGFJOAJKDH=NEFBEOFPGJA.Auto;Call("Assess",w,20d,true);
  Check(near.Calls==1&&far.Calls==1&&last.Calls==1,"three cities dispatch in a connected survival chain despite old two-pending limit");
  Check(near.Last.Starts==1&&far.Last.Starts==1&&last.Last.Starts==1,"each real native assembly starts once");
  Check(near.Last.GetSoldierNum(false)==6000&&far.Last.GetSoldierNum(false)==6000&&last.Last.GetSoldierNum(false)==6000,"all three source garrisons retain four of ten units under urgent 35 percent floor");
  Check(near.Last.Position.x==0&&far.Last.Position.x==-160&&last.Last.Position.x==-190,"new relief orders never teleport to battlefield");
  Call("Assess",w,30d,true);Check(near.Calls==1&&far.Calls==1&&last.Calls==1,"ongoing waves and source cooldown prevent duplicate armies");
  typeof(BattleIntegrationTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);
  w=Field<NJHDEHLHENN>(typeof(BattleIntegrationTests),"war");near=Field<AFAGFHJONFJ>(typeof(BattleIntegrationTests),"source");near.HomeOfficerCount=0;
  for(int i=0;i<11;i++){var blocked=City("blocked"+i,i);blocked.HomeOfficerCount=0;}
  var fallback=City("fallback",-100);Call("Assess",w,0d,true);Call("Assess",w,10d,true);Call("Assess",w,20d,true);
  Check(fallback.Calls==1,"bounded scan resumes beyond twelve blocked sources instead of starving farther city");
 }
 static void DefenseLifecycle(){
  void Setup(){typeof(IntegrationTests).GetMethod("Prepare",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);Call("Ready");Call("ResetDefense");Call("ResetWork");UnityEngine.AI.NavMesh.Blocked=false;UnityEngine.AI.NavMesh.Detour=0;AOHIBKLOAEI.Region=null;}
  AFAGFHJONFJ Source()=>Field<AFAGFHJONFJ>(typeof(IntegrationTests),"source");
  MKMOJNAJKJE Faction()=>NEINKOOBJIH.COCMABJFOFH["F"];
  void Run(){foreach(var step in (IEnumerable<string>)Call("DefenseSteps",Faction())){}}
  IDictionary Orders()=>(IDictionary)typeof(StrategicSupport).GetField("defenseOrders",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
  Setup();Call("NotifyDefense",ICGAEMLACJG.HFJIECIBFKB[1]);var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");Check(queue.Count==1,"enemy departure schedules defense without governor invocation");
  Run();var t=Source().Last;Check(t!=null&&Orders().Count==1,"preventive reinforcement registered");
  Check(!(bool)Call("CampaignControlled",t,null),"campaign recovery cannot override defensive rally");
  var target=MICGPAMNDKM.IJPKEMPIFIH["target"];ICGAEMLACJG.FKKCPDEFODO["target"]=new(){City=target};
  t.m_deployState=AKOPKFPKJPP.Expedition;t.m_listMovePos.Add(target.BMBMKPGEHBH);t.Food=10000;
  Call("CheckDefenseOrder",Orders()[t.Pointer]);Check(!t.m_isEnterCity,"relief stays outside city rather than overfilling barracks");
  Check(!(bool)Call("GuardDefenseEntry",t,target),"native entry cannot exceed city capacity through relief order");
  var war=new NJHDEHLHENN{IJGLCFJOOIK=target,EMMJBPMNFAK=new[]{"F","enemy"}};ICGAEMLACJG.HFJIECIBFKB[1].m_warInfo=war;
  Call("CheckDefenseOrder",Orders()[t.Pointer]);Check(t.m_targetWarInfo==war,"rallied force gets a native battle destination when siege starts");
  t.m_warInfo=war;Call("CheckDefenseOrder",Orders()[t.Pointer]);Check(Orders().Count==0,"arrival releases campaign tracking to battle lifecycle");
  Setup();Run();t=Source().Last;ICGAEMLACJG.HFJIECIBFKB.Remove(1);Call("CheckDefenseOrder",Orders()[t.Pointer]);Check(t.Dismissed&&Orders().Count==0,"cancelled threat cancels only our unstarted assembly");
  Setup();Run();t=Source().Last;t.m_targetCity="manual-change";Call("CheckDefenseOrder",Orders()[t.Pointer]);Check(!t.Dismissed&&t.m_targetCity=="manual-change"&&Orders().Count==0,"manual redirection relinquishes automation");
  Setup();target=MICGPAMNDKM.IJPKEMPIFIH["target"];target.BMBMKPGEHBH=new(100);ICGAEMLACJG.FKKCPDEFODO["target"]=new(){City=target};
  var invader=ICGAEMLACJG.HFJIECIBFKB[1];invader.Position=new(300);invader.m_listMovePos.Clear();invader.m_listMovePos.Add(target.BMBMKPGEHBH);
  Run();t=Source().Last;t.m_deployState=AKOPKFPKJPP.Expedition;t.m_listMovePos.Add(target.BMBMKPGEHBH);t.Food=10000;
  Call("CheckDefenseOrder",Orders()[t.Pointer]);Check(t.CityMoves==1&&t.m_targetCity==""&&t.m_listMovePos.Count>0,"native friendly non-entry routing may clear goal while relay retains its destination");
  Call("CheckDefenseOrder",Orders()[t.Pointer]);Check(t.CityMoves==1&&Orders().Count==1,"valid ongoing rally is not repeatedly restarted");
  NEINKOOBJIH.MIPFHCKDJOK+=3;Call("CheckDefenseOrder",Orders()[t.Pointer]);Check(t.CityMoves==2,"stalled army receives a new native route after progress timeout");
  Setup();Run();t=Source().Last;t.m_deployState=AKOPKFPKJPP.Expedition;t.m_listMovePos.Clear();t.Position=new(100);
  Check(double.IsPositiveInfinity((double)Call("TeamArrival",t,new Vector3(0))),"pathless stalled relief cannot suppress further support");
  Setup();var c=Source();var f=Faction();var it=((IEnumerable<string>)Call("DefenseSteps",f)).GetEnumerator();it.MoveNext();c.LMPMNLLGOEC.LMMNKIOAGLC="enemy";while(it.MoveNext()){}Check(c.Calls==0,"source captured between slices cannot supply friendly relief");
  Setup();UnityEngine.AI.NavMesh.Blocked=true;Run();Check(Source().Calls==0,"blocked native route prevents imaginary support");UnityEngine.AI.NavMesh.Blocked=false;
  Setup();Source().Generated=()=>MICGPAMNDKM.IJPKEMPIFIH["target"].LMPMNLLGOEC.LMMNKIOAGLC="enemy";Run();Check(Source().Last.Dismissed&&Orders().Count==0,"target capture during native assembly is revalidated before activation");
  Setup();Source().Generated=()=>ICGAEMLACJG.HFJIECIBFKB[90]=new(){m_id=90,m_targetCity="source",m_belongPower="enemy"};Run();Check(Source().Last.Dismissed&&Orders().Count==0,"new source invasion during native assembly cancels lending");
  Setup();ICGAEMLACJG.HFJIECIBFKB[1].Combat=40000;
  var f2=Faction();
  foreach(string id in new[]{"remote1","remote2"}){
   var c2=new AFAGFHJONFJ{OJPECMNINNA=id};c2.LMPMNLLGOEC.LMMNKIOAGLC="F";c2.LMPMNLLGOEC.IPJHOOJBCKI="gov";for(int j=0;j<10;j++)c2.LMPMNLLGOEC.KJPPGIOCDID.Add(new());MICGPAMNDKM.IJPKEMPIFIH[id]=c2;
  }
  Run();Check(Orders().Count==3,"all owned cities considered for preventive relay without two-hop or two-army cap");
  Check(MICGPAMNDKM.IJPKEMPIFIH["remote1"].Last!=null&&MICGPAMNDKM.IJPKEMPIFIH["remote2"].Last!=null,"distant source candidates are actual native armies, not paper reservations");
  Call("ResetWork");Call("ResetDefense");
 }
}

