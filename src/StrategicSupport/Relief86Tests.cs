using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.AI;
public static class Relief86Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int count;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool b,string n){count++;if(!b)throw new Exception("R86: "+n);}
 static IDictionary Covers=>(IDictionary)typeof(StrategicSupport).GetField("coverOrders",F).GetValue(null);
 static MKMOJNAJKJE faction;static AFAGFHJONFJ front;
 static void Setup(){typeof(StrategicSafetyTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetDefense");Call("ResetWork");MICGPAMNDKM.IJPKEMPIFIH.Clear();ICGAEMLACJG.FKKCPDEFODO.Clear();AOHIBKLOAEI.Region=null;AOHIBKLOAEI.RegionAt=null;NavMesh.Detour=0;NavMesh.Blocked=false;faction=NEINKOOBJIH.COCMABJFOFH["F"];front=City("front",-20);var foe=new Wo_DeployTeamCtrl{m_belongPower="enemy",m_targetCity="front",Position=new(10000),m_deployState=AKOPKFPKJPP.Expedition};ICGAEMLACJG.HFJIECIBFKB[1]=foe;}
 static AFAGFHJONFJ City(string id,float x){var c=new AFAGFHJONFJ{OJPECMNINNA=id,BMBMKPGEHBH=new(x),HomeOfficerCount=10};c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.LMPMNLLGOEC.OEGNGFBCHFL=10000000;c.Roster=Enumerable.Range(0,10).Select(i=>new CLAGKDECFID{OJPECMNINNA=id+"off"+i,LMPMNLLGOEC=new HeroRow{CNOMNBCPJDI=id,LMMNKIOAGLC="F"}}).ToList();for(int j=0;j<10;j++)c.LMPMNLLGOEC.KJPPGIOCDID.Add(new());MICGPAMNDKM.IJPKEMPIFIH[id]=c;ICGAEMLACJG.FKKCPDEFODO[id]=new(){City=c};return c;}
 static void Link(AFAGFHJONFJ a,AFAGFHJONFJ b){a.FKMKGNPFLFP.Add(b);b.FKMKGNPFLFP.Add(a);}
 static void Expose(AFAGFHJONFJ c,float distance=1000){var e=new AFAGFHJONFJ{OJPECMNINNA="enemy-"+c.OJPECMNINNA,BMBMKPGEHBH=new(c.BMBMKPGEHBH.x+distance)};e.LMPMNLLGOEC.LMMNKIOAGLC="enemy";e.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){CombatValue=50000});c.FKMKGNPFLFP.Add(e);}
 static List<Wo_DeployTeamCtrl> Teams()=>ICGAEMLACJG.HFJIECIBFKB.Values.ToList();
 static HashSet<IntPtr> Reserved()=>(HashSet<IntPtr>)Call("AttackReserved",Teams());
 static Wo_DeployTeamCtrl Generate(AFAGFHJONFJ c)=>(Wo_DeployTeamCtrl)Call("GenerateSupport",c,5000,Teams(),Reserved());
 static void Plan(AFAGFHJONFJ c){int steps=0;foreach(var phase in (IEnumerable<string>)Call("RegionalCoverSteps",c,front,new HashSet<string>())){steps++;if(steps>500)throw new Exception("unbounded cover graph");}Check(steps<=500,"graph work remains bounded and yielded");}
 public static void Run(){
  Setup();var a=City("A",0);var b=City("B",10);Link(a,b);Expose(a);Check(Generate(a)==null,"reproduce exposed Xinye rejection before backfill");Plan(a);
  Check(Covers.Count==1&&b.Last.m_targetCity=="A","real donor order committed toward the exposed source");var outgoing=Generate(a);Check(outgoing!=null,"confirmed timely backfill unlocks first wave despite pre-existing border deficit");
  Check((long)Call("ReserveCityPower",a,Teams())>=6000,"physical local sixty percent remains after first wave");
  Check(Generate(a)==null,"same incoming army cannot unlock an additional unsupported wave");
  var cover=Covers.Values.Cast<object>().Single();b.Last.m_targetCity="manual";Check(Generate(a)==null,"redirected backfill immediately revokes credit");Call("CheckRegionalCover",cover);Check(Covers.Count==0,"manual replacement relinquishes ownership");
  Setup();a=City("A",0);b=City("B",10);Link(a,b);Expose(a,2);Plan(a);Check(Covers.Count==0,"enemy can arrive before backfill: no unsafe loan");
  Setup();a=City("A",0);b=City("B",10);Link(a,b);Expose(a);faction.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Plan(a);Check(Covers.Count==1,"StopWar permits defensive regional backfill");
  Setup();a=City("A",0);b=City("B",10);Link(a,b);Expose(a);Expose(b);Plan(a);Check(Covers.Count==0,"A promises B and B promises A cannot create phantom surplus");
  Setup();var chain=new List<AFAGFHJONFJ>();for(int i=0;i<7;i++){var c=City("N"+i,i*10);chain.Add(c);if(i>0)Link(chain[i-1],c);if(i<6)Expose(c);}Plan(chain[0]);
  Check(Covers.Count==6,"seven-city domino reaches an independently funded rear donor");
  Check(Generate(chain[0])!=null,"far-back commitment enables actual leading city's first wave");
  Check((bool)Call("CoverCycle","N0","N6"),"cross-link completing an existing transfer cycle rejected");
  var used=new HashSet<IntPtr>();foreach(var t in Teams().Where(t=>t.m_belongPower=="F"))foreach(var unit in t.m_dicDeployArmy.Values.SelectMany(g=>g))Check(used.Add(unit.Pointer),"each native unit assigned to one city/front only");
  foreach(var c in chain)Check((long)Call("ReserveCityPower",c,Teams())>=6000,"each intermediate retains physical soldiers, not just expected arrivals");
  var rearOrder=Covers.Values.Cast<object>().First(x=>(string)x.GetType().GetField("Source").GetValue(x)=="N6");chain[6].Last.m_targetCity="manual";Call("CheckRegionalCover",rearOrder);
  foreach(var phase in (IEnumerable<string>)Call("RegionalLoanSteps")){}
  Check(Covers.Count==0,"cancelled rear commitment propagates through all dependent unstarted backfills");
  Setup();a=City("A",0);b=City("B",10);var d=City("D",20);Link(a,b);Link(a,d);Expose(a);Plan(a);int first=Covers.Count;Plan(a);Check(Covers.Count==first,"alternate directions do not double-book an already covered request");
  Setup();a=City("A",0);b=City("B",10);Link(a,b);Expose(a);Plan(a);b.Last.Food=0;Check(Generate(a)==null,"starved incoming force cannot provide cover credit");
  Setup();a=City("A",0);b=City("B",10);Link(a,b);Expose(a);Plan(a);b.Last.m_deployState=AKOPKFPKJPP.Camp;Check(Generate(a)==null,"stalled distant camp cannot provide cover credit");
  Setup();a=City("A",0);b=City("B",10);Link(a,b);Expose(a);Plan(a);cover=Covers.Values.Cast<object>().Single();var arrived=b.Last;arrived.m_deployState=AKOPKFPKJPP.Camp;arrived.Position=a.BMBMKPGEHBH;a.Capacity=30;
  Call("CheckRegionalCover",cover);Check(arrived.m_isEnterCity,"cover army with safe capacity enters instead of permanent camp");
  Check((bool)Call("GuardCoverEntry",arrived,a)&&Covers.Count==0,"successful native entry releases temporary cover ownership");
  Setup();a=City("A",0);b=City("B",10);Link(a,b);Expose(a);Plan(a);cover=Covers.Values.Cast<object>().Single();arrived=b.Last;arrived.m_deployState=AKOPKFPKJPP.Camp;arrived.Position=a.BMBMKPGEHBH;
  Check(!(bool)Call("GuardCoverEntry",arrived,a)&&Covers.Count==1,"cover entry still rejects full city rather than causing attrition");
  a.Capacity=30;a.LMPMNLLGOEC.OEGNGFBCHFL=0;Check(!(bool)Call("GuardCoverEntry",arrived,a),"empty food reserve prevents garrison starvation");
  Setup();a=City("A",0);b=City("B",10);Link(a,b);Expose(a);Plan(a);cover=Covers.Values.Cast<object>().Single();arrived=b.Last;arrived.m_deployState=AKOPKFPKJPP.Camp;arrived.Position=a.BMBMKPGEHBH;
  ICGAEMLACJG.HFJIECIBFKB.Remove(1);a.FKMKGNPFLFP.RemoveAll(x=>x.LMPMNLLGOEC.LMMNKIOAGLC=="enemy");a.LMPMNLLGOEC.KJPPGIOCDID.Clear();a.Capacity=0;
  Call("CheckRegionalCover",cover);Check(Covers.Count==0,"ended current threat releases cover even below historical strength and without entry capacity");
  // Native-style moving-command shortcut is represented by the old MoveToCity
  // stub; the installed path must replace its direct destination and stale queue.
  Setup();a=City("Runan",0);b=City("Xinye",-20);var dest=City("Wan",-40);a.MEKIDAONKMD[dest.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){b}};AOHIBKLOAEI.RegionAt=p=>p.x>-10?a:p.x>-30?b:dest;
  var army=new Wo_DeployTeamCtrl{m_id=999,m_belongPower="F",m_assembleCity="Runan",Position=a.BMBMKPGEHBH,m_deployState=AKOPKFPKJPP.Expedition};army.m_listMovePos.Add(new(100));army.GAANFLKOMKL.Add(new(100));
  Check((bool)Call("MoveRelief",army,dest,null,false,faction,false),"moving army accepts a complete validated native movement path");
  Check(army.m_listMovePos.Any(p=>p.x==-20)&&army.m_listMovePos.Last().x==-40&&!army.m_listMovePos.Any(p=>p.x>0)&&army.GAANFLKOMKL.Count==0,"Xinye waypoint retained and hostile stale path/queue removed");
  object[] quote={a.BMBMKPGEHBH,dest.BMBMKPGEHBH,faction,2f,0d,null};Check((bool)Call("RouteSeconds",quote)&&(double)quote[4]==20,"ETA measured from same complete corridor used by movement");
  b.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!(bool)Call("MoveRelief",army,dest,null,false,faction,false),"captured intermediate cannot be traversed by relief");
  AOHIBKLOAEI.RegionAt=null;Call("ResetDefense");Call("ResetWork");Cadence();Console.WriteLine($"PASS R86 {count} regional-cover network, exact-route and cadence checks; native actions mocked, gameplay not yet verified.");
 }
 static void Cadence(){
  var original=NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC;
  try{
   NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
   Check((int)Call("DecisionDays","F")==1&&(int)Call("DecisionDays","enemy")==3,"current player faction daily and NPC every three days");
   bool Due(string key,string owner,bool commit=true)=>(bool)Call("DecisionDue",key,owner,new IntPtr(86),"gov",commit);
   NEINKOOBJIH.MIPFHCKDJOK=100;Check(Due("test-player","F")&&Due("test-enemy","enemy"),"first observation allowed on both sides");
   NEINKOOBJIH.MIPFHCKDJOK=101;Check(Due("test-player","F")&&!Due("test-enemy","enemy"),"next day only player regular decisions admitted");
   NEINKOOBJIH.MIPFHCKDJOK=102;Check(!Due("test-enemy","enemy"),"NPC day two not admitted");NEINKOOBJIH.MIPFHCKDJOK=103;Check(Due("test-enemy","enemy"),"NPC resumes exactly three game days later");
   NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="";Check((int)Call("DecisionDays","F")==3,"retired unaffiliated protagonist's former faction becomes NPC");
   Check(Due("test-player","F"),"player leaving faction invalidates old cadence stamp");
   NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check((int)Call("DecisionDays","enemy")==1&&Due("test-enemy","enemy"),"joining another faction switches daily classification immediately");
   NEINKOOBJIH.MIPFHCKDJOK=90;Check(Due("test-enemy","enemy"),"clock rollback does not freeze decisions until former future date");
   Call("ResetWork");NEINKOOBJIH.MIPFHCKDJOK=100;Call("ScheduleRegularDefense",faction);var queue=(WorkQueue)typeof(StrategicSupport).GetField("work",F).GetValue(null);queue.Clear();NEINKOOBJIH.MIPFHCKDJOK=101;Call("ScheduleRegularDefense",faction);Check(queue.Count==0,"NPC periodic defense uses three-day admission");var gates=(IDictionary)typeof(StrategicSupport).GetField("next",F).GetValue(null);gates["defense-evaluation:"+faction.IBGCLCAIBOL]=0L;Call("ScheduleDefense",faction);Check(queue.Count==1,"emergency evaluation remains available inside regular three-day cooldown after scaled event interval");
  }finally{NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC=original;Call("ResetWork");}
 }
}
