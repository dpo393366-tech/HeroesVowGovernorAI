using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class Corridor132Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static AFAGFHJONFJ source,dest,via;static NJHDEHLHENN target,blocker;static MKMOJNAJKJE faction;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Field<T>(string n)=>(T)typeof(StrategicSupport).GetField(n,F).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Corridor132: "+why);}
 static void RunSteps(string n,params object[] a){foreach(var _ in (IEnumerable<string>)Call(n,a)){} }
 static List<Wo_DeployTeamCtrl> Teams()=>ICGAEMLACJG.HFJIECIBFKB.Values.ToList();
 static object Plan()=>Activator.CreateInstance(typeof(StrategicSupport).GetNestedType("RoutePlan",BindingFlags.NonPublic));
 static T Get<T>(object o,string n)=>(T)o.GetType().GetField(n).GetValue(o);
 static void Setup(bool bypass=false){
  typeof(RouteOrder94Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetManualOrders");MLGOLGGHLMB.CBEGDDKANAO.Clear();
  source=(AFAGFHJONFJ)typeof(RouteOrder94Tests).GetField("source",F).GetValue(null);target=(NJHDEHLHENN)typeof(RouteOrder94Tests).GetField("war",F).GetValue(null);faction=NEINKOOBJIH.COCMABJFOFH["F"];
  source.BMBMKPGEHBH=new(0);target.GBHGENNMJCK=new(100);target.HIENOINGBCN="original";Wa_Controller.m_warInfo=null;
  blocker=new(){HIENOINGBCN="blocking",GBHGENNMJCK=new(50),FGHFOLKBHDL=new[]{10000,40000}};
  ICGAEMLACJG.HFJIECIBFKB[99001]=new(){m_id=99001,m_belongPower="F",m_warInfo=blocker,Position=new(50)};
  ICGAEMLACJG.HFJIECIBFKB[99002]=new(){m_id=99002,m_belongPower="enemy",m_warInfo=target,Position=new(100)};
  dest=new(){OJPECMNINNA="goal",BMBMKPGEHBH=new(100)};dest.LMPMNLLGOEC.LMMNKIOAGLC="enemy";target.IJGLCFJOOIK=dest;
  via=new(){OJPECMNINNA="via",BMBMKPGEHBH=new(50,0,20)};via.LMPMNLLGOEC.LMMNKIOAGLC="F";
  MICGPAMNDKM.IJPKEMPIFIH[dest.OJPECMNINNA]=dest;MICGPAMNDKM.IJPKEMPIFIH[via.OJPECMNINNA]=via;
  source.FKMKGNPFLFP.Clear();source.FKMKGNPFLFP.Add(dest);if(bypass){source.FKMKGNPFLFP.Add(via);via.FKMKGNPFLFP.Add(dest);}
  AOHIBKLOAEI.RegionAt=p=>p.x>95?dest:p.z>10?via:source;
  Call("RefreshBattleCorridors",Teams());
 }
 static void Quote(){RunSteps("BattleRouteQuote",source,target,faction,"quote");}
 static bool Saved(out double eta){object[] args={source,target,faction,"quote",Call("RoadPolicyStamp",faction),0d};bool ok=(bool)Call("SavedBattleQuote",args);eta=(double)args[5];return ok;}
 static Wo_DeployTeamCtrl Send()=> (Wo_DeployTeamCtrl)Call("DispatchBattle",source,target,0,10000,60d,false,Teams(),Call("AttackReserved",Teams()),new Func<double,bool>(_=>true));
 public static void Run(){
  Setup();var road=new List<Vector3>{new(0),new(100)};
  Check(ReferenceEquals(Call("FirstCorridorBlock",road,target,faction),blocker),"actual road intersects intervening combat");
  Check(Call("FirstCorridorBlock",new List<Vector3>{new(0,0,6),new(100,0,6)},target,faction)==null,"nearby parallel road outside native radius is not blocked");
  Check(Call("FirstCorridorBlock",new List<Vector3>{new(0,6,0),new(100,6,0)},target,faction)==null,"vertical separation follows native three-coordinate distance");
  blocker.GBHGENNMJCK=new(110);Call("RefreshBattleCorridors",Teams());Check(Call("FirstCorridorBlock",road,target,faction)==null,"combat beyond target's native join radius cannot block arrival");
  Setup();Quote();Check(Saved(out var eta)&&double.IsPositiveInfinity(eta),"blocked path is not promised as on-time reinforcement");Check(Send()==null&&source.Calls==0,"blocked original front does not recruit then disband");
  int cold=NavMesh.Calls105;for(int i=0;i<500;i++)Check(Saved(out eta)&&double.IsPositiveInfinity(eta),"unchanged blocked quote retained");Check(NavMesh.Calls105==cold,"500 blocked assessments add zero native path calls");
  for(int i=0;i<30;i++)Call("RequestCorridorBreakthrough",source,target,faction);Check(Field<WorkQueue>("work").Count==1,"30 blocked donors request one breakthrough assessment");Call("ResetWork");
  ICGAEMLACJG.HFJIECIBFKB.Remove(99001);Call("RefreshBattleCorridors",Teams());Check(!Saved(out eta),"battle ending invalidates negative quote without timeout");Quote();Check(Saved(out eta)&&double.IsFinite(eta)&&Send()!=null,"cleared corridor resumes normal support");
  Setup(true);Quote();Check(Saved(out eta)&&eta>60&&double.IsFinite(eta),"safe alternate road is selected with its real longer ETA");
  var team=Send();Check(team!=null,"safe detour can dispatch");team.StartDeploy();Call("RouteNewRelief",team);Check(team.m_listMovePos.Any(p=>p.z>10),"native movement receives actual alternate road corners");
  Check(Call("FirstCorridorBlock",team.m_listMovePos,target,faction)==null,"installed route cannot silently fall back into blocked direct road");
  cold=NavMesh.Calls105;for(int i=0;i<500;i++)Check(Saved(out eta)&&double.IsFinite(eta),"cached detour retained");Check(NavMesh.Calls105==cold,"500 detour quotes add zero native path calls");
  via.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!Saved(out eta),"captured bypass cannot reuse obsolete permissions");Quote();Check(Saved(out eta)&&double.IsPositiveInfinity(eta),"lost bypass switches back to breakthrough planning");
  Setup(true);Quote();blocker.GBHGENNMJCK=new(50,0,20);Call("RefreshBattleCorridors",Teams());Check(!Saved(out eta),"moving combat invalidates detour");Quote();Check(Saved(out eta)&&Math.Abs(eta-60)<.01,"newly clear direct road replaces old detour");
  Setup(true);Quote();source.Generated=()=>NavMesh.Blocked=true;Check(Send()==null&&source.Last.Dismissed,"physical path change during assembly still rolls back");NavMesh.Blocked=false;
  Setup(true);Quote();source.Generated=()=>{via.LMPMNLLGOEC.LMMNKIOAGLC="enemy";};Check(Send()==null&&source.Last.Dismissed,"permission change during assembly still rolls back");
  Setup();blocker.EMMJBPMNFAK=new[]{"neutral-a","neutral-b"};Call("RefreshBattleCorridors",Teams());Check(Call("FirstCorridorBlock",road,target,faction)==null,"unrelated neutral battle does not block passage");
  // A fresh war appears during sliced planning: never publish the old route.
  Setup(true);var e=((IEnumerable<string>)Call("BattleRouteQuote",source,target,faction,"quote")).GetEnumerator();int slices=0;
  while(e.MoveNext()){slices++;if(slices==5){blocker.GBHGENNMJCK=new(50,0,20);Call("RefreshBattleCorridors",Teams());}}e.Dispose();
  if(Saved(out eta)&&double.IsFinite(eta)){var cached=Field<IDictionary>("savedBattleRoads");foreach(var item in cached.Values)if(Get<bool>(item,"Success"))Check(Call("FirstCorridorBlock",Get<List<Vector3>>(item,"Points"),target,faction)==null,"changed combat geometry cannot publish unsafe successful path");}
  // Multi-city blockade: the original front must not keep recruiting while
  // its corridor is blocked, but the blocking front still receives real aid.
  Setup();MICGPAMNDKM.IJPKEMPIFIH.Remove("near");
  var donors=new List<AFAGFHJONFJ>{source};
  for(int k=0;k<12;k++)donors.Add((AFAGFHJONFJ)typeof(RouteOrder94Tests).GetMethod("Add",F).Invoke(null,new object[]{"donor"+k,-5f*(k+1)}));
  blocker.FGHFOLKBHDL=new[]{10000,15000};
  for(int day=0;day<30;day++){
   NEINKOOBJIH.MIPFHCKDJOK=100+day;
   Call("Assess",target,200d+day*10,false);
   Call("Assess",blocker,200d+day*10,false);
  }
  var deployed=Teams().Where(x=>x.m_belongPower=="F"&&x.m_warInfo==null&&x.m_targetWarInfo!=null).ToList();
  Check(deployed.Count>0&&deployed.All(x=>ReferenceEquals(x.m_targetWarInfo,blocker)),"30 repeated daily decisions recruit for breakthrough, never the blocked destination");
  int committed=deployed.Sum(x=>x.GetSoldierNum(false));
  Check(committed<=18000,"all cities share the blocking front demand; no 30-day exponential drafting");
  Check(donors.All(c=>c.LMPMNLLGOEC.KJPPGIOCDID.Count>0),"multi-city breakthrough preserves source garrisons");
  Console.WriteLine($"Corridor132 multi-city: {donors.Count} donors,30 repeated decisions,breakthrough committed={committed},original-front dispatched=0");Call("ResetWork");
  // Independent segment/sphere oracle across varied road shapes, both sides.
  var random=new Random(132);int evaluated=0;
  for(int seed=0;seed<64;seed++){
   Setup();float z=(float)(random.NextDouble()*20-10);blocker.GBHGENNMJCK=new(40+(float)random.NextDouble()*20,0,z);Call("RefreshBattleCorridors",Teams());
   bool expected=z*z<20;bool actual=Call("FirstCorridorBlock",road,target,faction)!=null;Check(actual==expected,"random road/radius oracle "+seed);evaluated++;
   blocker.EMMJBPMNFAK=new[]{"enemy","F"};Call("RefreshBattleCorridors",Teams());Check((Call("FirstCorridorBlock",road,target,faction)!=null)==expected,"reversed battle side "+seed);
  }
  Setup();var waiting=new Wo_DeployTeamCtrl{m_id=99003,m_belongPower="F",m_assembleCity=source.OJPECMNINNA,m_targetWarInfo=target,Position=new(0),m_deployState=AKOPKFPKJPP.Expedition};waiting.m_listMovePos.Add(new(100));
  NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";MLGOLGGHLMB.CBEGDDKANAO["local.heroesvow.governor/manual/v1/F/team/99003"]=-1;
  Check(!(bool)Call("CorridorArrivalClear",waiting,target,faction),"blocked existing wave excluded even when not owned by this mod");Check(Field<WorkQueue>("work").Count==0,"unowned/manual order not retasked");
  MLGOLGGHLMB.CBEGDDKANAO.Clear();waiting.Food=100000;waiting.m_dicDeployArmy["u"]=new(){new DHKINGPDKJJ()};ICGAEMLACJG.HFJIECIBFKB[waiting.m_id]=waiting;
  blocker.FGHFOLKBHDL=new[]{50000,1000};Call("RefreshBattleCorridors",Teams());
  Check(!(bool)Call("CorridorArrivalClear",waiting,target,faction),"loaded automatic support adopted for blocked-route repair");
  var mission=Field<IDictionary>("missions")[waiting.Pointer];Check(mission!=null,"automatic existing order keeps explicit repair owner");
  RunSteps("RepairCorridorSteps",waiting,mission,faction);
  Check(waiting.m_deployState==AKOPKFPKJPP.Camp&&waiting.m_listMovePos.Count==0&&waiting.m_targetWarInfo==target,"covered blocker causes a safe explicit wait rather than continued blind advance");
  int beforeHold=NavMesh.Calls105;for(int i=0;i<50;i++)RunSteps("RepairCorridorSteps",waiting,mission,faction);
  Check(NavMesh.Calls105==beforeHold,"50 held-army decisions reuse blocked geometry without native navigation");
  blocker.FGHFOLKBHDL=new[]{10000,20000};RunSteps("RepairCorridorSteps",waiting,mission,faction);
  Check(waiting.m_targetWarInfo==blocker&&waiting.m_deployState==AKOPKFPKJPP.Expedition,"reversal at blocking front reuses held army immediately for breakthrough");
  Setup();waiting=new(){m_id=99003,m_belongPower="F",m_assembleCity=source.OJPECMNINNA,m_targetWarInfo=target,Position=new(0),m_deployState=AKOPKFPKJPP.Expedition,Food=100000};waiting.m_listMovePos.Add(new(100));waiting.m_dicDeployArmy["u"]=new(){new DHKINGPDKJJ()};ICGAEMLACJG.HFJIECIBFKB[waiting.m_id]=waiting;
  blocker.FGHFOLKBHDL=new[]{50000,1000};Call("CorridorArrivalClear",waiting,target,faction);mission=Field<IDictionary>("missions")[waiting.Pointer];RunSteps("RepairCorridorSteps",waiting,mission,faction);
  ICGAEMLACJG.HFJIECIBFKB.Remove(99001);Call("RefreshBattleCorridors",Teams());RunSteps("RepairCorridorSteps",waiting,mission,faction);
  Check(waiting.m_targetWarInfo==target&&waiting.m_deployState==AKOPKFPKJPP.Expedition&&waiting.m_listMovePos.Count>0,"blocker ending releases held army toward original target without another recruitment");
  Setup();var longRoad=Enumerable.Range(0,1024).Select(i=>new Vector3(i,0,(float)Math.Sin(i*.01)*5)).ToList();
  for(int k=0;k<128;k++){var distant=new NJHDEHLHENN{HIENOINGBCN="far"+k,GBHGENNMJCK=new(k*8,0,100+k),FGHFOLKBHDL=new[]{100,100}};ICGAEMLACJG.HFJIECIBFKB[100000+k]=new(){m_id=100000+k,m_belongPower="F",m_warInfo=distant};}
  ICGAEMLACJG.HFJIECIBFKB.Remove(99001);Call("RefreshBattleCorridors",Teams());
  for(int i=0;i<10;i++)Call("FirstCorridorBlock",longRoad,target,faction);
  var timer=System.Diagnostics.Stopwatch.StartNew();for(int i=0;i<500;i++)Check(Call("FirstCorridorBlock",longRoad,target,faction)==null,"large off-route war set stays clear");timer.Stop();
  Console.WriteLine($"Corridor132 managed geometry: 500 queries,1024 road corners,128 off-route battles,totalMs={timer.Elapsed.TotalMilliseconds:F2}; synthetic CPU benchmark only");
  Call("ResetBattleState");Check(Field<IDictionary>("corridorWars").Count==0,"save reset removes combat topology");Call("ResetDefense");Call("ResetWork");AOHIBKLOAEI.RegionAt=null;
  Console.WriteLine($"PASS Corridor132 {checks} assertions; {evaluated} seeded geometry cases, blocked/detour cache, native-departure rollback and lifecycle. Native movement mocked; not live FPS.");
 }
}
