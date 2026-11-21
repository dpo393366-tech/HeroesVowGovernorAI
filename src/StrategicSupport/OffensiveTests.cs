using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.AI;
public static class OffensiveTests
{
 static int tests;static AFAGFHJONFJ c,target;static MKMOJNAJKJE f;static FNFJODHIJFO g;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static void Set<T>(string n,T v)=>((ConfigEntry<T>)typeof(StrategicSupport).GetField(n,BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Value=v;
 static void Check(bool b,string why){tests++;if(!b)throw new Exception("offense: "+why);}
 static void Setup(){
  typeof(CityIntegrationTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);
  c=(AFAGFHJONFJ)typeof(CityIntegrationTests).GetField("c",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
  f=NEINKOOBJIH.COCMABJFOFH["F"];g=f.ANMMMOIKLEG["gov"];
  ICGAEMLACJG.HFJIECIBFKB.Clear();NEINKOOBJIH.MIPFHCKDJOK=100;Wa_Controller.m_warInfo=null;
  NavMesh.Blocked=false;NavMesh.Detour=0;AOHIBKLOAEI.Region=null;Time.timeScale=1;GameObject.WorldVisible=true;
  foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.FGHFOLKBHDL=EBNFFALGBPA.LLLBAAAEIBB;
  c.Roster=Enumerable.Range(0,8).Select(i=>new CLAGKDECFID{OJPECMNINNA="h"+i,LMPMNLLGOEC=new HeroRow{CNOMNBCPJDI=c.OJPECMNINNA,LMMNKIOAGLC="F"}}).ToList();
  f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};
  target=new(){OJPECMNINNA="enemy-city",Power=1000,BMBMKPGEHBH=new(20)};target.LMPMNLLGOEC.LMMNKIOAGLC="enemy";
  MICGPAMNDKM.IJPKEMPIFIH[target.OJPECMNINNA]=target;c.FKMKGNPFLFP.Add(target);target.FKMKGNPFLFP.Add(c);
  c.Generated=()=>c.Last.Combat=c.Last.m_dicDeployArmy.Values.SelectMany(x=>x).Sum(a=>a.GNCKPEPJDGF());
 }
 static void RunPlan(){foreach(var step in (IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")){} }
 static bool Start()=> (bool)Call("GuardAttackStart",c.Last);
 static void Rejected(string why){RunPlan();Check(c.Last==null||c.Last.Dismissed,why);}
 public static void Run(){
  Setup();target.LMPMNLLGOEC.LMMNKIOAGLC="";foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.MGLFFEANDOI=false;RunPlan();Check(c.Calls==1&&Start(),"neutral occupation and native-eligible troops are not excluded by unrelated flag");
  target.LMPMNLLGOEC.LMMNKIOAGLC="other";Check(!Start()&&c.Last.Dismissed,"neutral occupied by another faction before departure cancels");
  Setup();target.LMPMNLLGOEC.LMMNKIOAGLC="";g.BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Rejected("StopWar also forbids neutral occupation");
  Setup();target.LMPMNLLGOEC.LMMNKIOAGLC="";g.BNGFJOAJKDH=NEFBEOFPGJA.AttackPower;g.LMAEGAJADKI="enemy";Rejected("designated enemy policy does not authorize neutral occupation");
  Setup();var corridor=new AFAGFHJONFJ{OJPECMNINNA="corridor"};corridor.LMPMNLLGOEC.LMMNKIOAGLC="F";MICGPAMNDKM.IJPKEMPIFIH[corridor.OJPECMNINNA]=corridor;c.FKMKGNPFLFP.Clear();c.FKMKGNPFLFP.Add(corridor);corridor.FKMKGNPFLFP.Add(target);target.LMPMNLLGOEC.LMMNKIOAGLC="";RunPlan();Check(c.Calls==1&&Start(),"frontier neutral reachable through own city corridor");
  Setup();corridor=new AFAGFHJONFJ{OJPECMNINNA="blocked-corridor"};corridor.LMPMNLLGOEC.LMMNKIOAGLC="third-party";c.FKMKGNPFLFP.Clear();c.FKMKGNPFLFP.Add(corridor);corridor.FKMKGNPFLFP.Add(target);Check(!((List<AFAGFHJONFJ>)Call("FrontierTargets",c,f)).Contains(target),"search cannot cross another faction corridor");
  Setup();RunPlan();Check(c.Calls==1&&c.Last.gameObject.activeSelf&&!c.Last.m_isEnterCity&&c.Last.m_targetCity==target.OJPECMNINNA,"full wealthy city orders native attack");
  Check(c.Last.GetSoldierNum(false)<=8000,"60 percent soldier reserve retained");
  Check((int)Call("OffenseCashReserve",f)==3000&&(int)Call("InvestmentReserve",f)>=30000,"assembly funds protected from investment");
  Check((int)Call("ResearchFundsReserve",f)>=30000,"assembly funds protected from research");
  Check(Start(),"valid assembly allowed");Call("RecordAttackDeparture",c.Last,c);Check((int)Call("OffenseCashReserve",f)==0,"departure releases money hold");
  RunPlan();Check(c.Calls==1,"no repeated orders");
  Setup();g.BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Rejected("StopWar blocks initial decision");Check(c.Calls==0,"StopWar never calls native generator");
  Setup();using(var it=((IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")).GetEnumerator()){
   Check(it.MoveNext(),"target search yields");g.BNGFJOAJKDH=NEFBEOFPGJA.StopWar;while(it.MoveNext()){}
  }Check(c.Calls==0&&(int)Call("OffenseCashReserve",f)==0,"policy changed between frames aborts and releases hold");
  Setup();c.Generated=()=>g.BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Rejected("policy changed inside native generation cancels");
  Setup();RunPlan();g.BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Check(!Start()&&c.Last.Dismissed,"StopWar during assembly cancels departure");
  Check((int)Call("OffenseCashReserve",f)==0,"cancel releases treasury hold");
  Setup();g.BNGFJOAJKDH=NEFBEOFPGJA.AttackPower;g.LMAEGAJADKI="different-enemy";Rejected("specific target policy respected");
  Setup();g.BNGFJOAJKDH=NEFBEOFPGJA.AttackPower;g.LMAEGAJADKI="enemy";RunPlan();Check(c.Calls==1,"designated enemy allowed");
  Setup();f.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.Ally;Rejected("no war declaration against ally");
  Setup();RunPlan();f.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.StopWar;Check(!Start()&&c.Last.Dismissed,"peace before departure cancels attack");
  Setup();RunPlan();target.LMPMNLLGOEC.LMMNKIOAGLC="F";Check(!Start(),"captured target cancels departure");
  Setup();c.LMPMNLLGOEC.IPJHOOJBCKI="player";Rejected("personal player district protected");
  Setup();c.LMPMNLLGOEC.KJPPGIOCDID.RemoveRange(0,2);RunPlan();Check(c.Calls==1&&Start(),"80 percent occupied city can safely attack weaker target");
  foreach(int capacity in new[]{10,30,1000}){Setup();c.Capacity=capacity;RunPlan();Check(c.Calls==1&&Start()&&c.Last.GetSoldierNum(false)==8000,"capacity expansion does not alter safe expedition");}
  Setup();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.FGHFOLKBHDL=1000;RunPlan();Check(c.Calls==1&&Start(),"half filled but sufficient actual force can attack");
  Setup();for(int i=0;i<6;i++)c.LMPMNLLGOEC.KJPPGIOCDID[i].FGHFOLKBHDL=100;RunPlan();Check(c.Calls==1&&Start(),"unfinished recruits do not veto ready units");
  Setup();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.GDHPLIFJCBN=PMNFKDJNLDJ.Transport;Rejected("busy troops cannot serve as available attack force");
  Setup();c.LMPMNLLGOEC.KJPPGIOCDID.RemoveRange(0,8);Rejected("last two garrison units never depart");
  Setup();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.CombatValue=100;Rejected("full soldier headcount with weak combat power cannot attack");
  Setup();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.CombatValue=5000;target.Power=10000;RunPlan();Check(c.Calls==1&&Start(),"elite combat power can support stronger target while keeping reserves");
  Setup();RunPlan();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID.Where(a=>!c.Last.m_dicDeployArmy.Values.SelectMany(x=>x).Contains(a)))a.CombatValue=1;Check(!Start(),"home combat deterioration cancels departure");
  Setup();RunPlan();Check((bool)Call("GuardAttackStart",new Wo_DeployTeamCtrl()),"untracked native army is never gated");
  Setup();target.Power=10000;var weak=new AFAGFHJONFJ{OJPECMNINNA="weak",Power=500,BMBMKPGEHBH=new(25)};weak.LMPMNLLGOEC.LMMNKIOAGLC="enemy";MICGPAMNDKM.IJPKEMPIFIH[weak.OJPECMNINNA]=weak;c.FKMKGNPFLFP.Add(weak);
  // The stronger frontier remains a reserve constraint, so supply enough home strength.
  foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.CombatValue=4000;
  RunPlan();Check(c.Calls==1&&c.Last.m_targetCity=="weak"&&Start(),"weaker feasible neighbour preferred while retaining other frontier");
  Setup();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.FGHFOLKBHDL=200;Rejected("depleted units do not count as ready");
  Setup();f.DNOGPFHMOMO=2999;Rejected("insufficient actual mobilization reserve");
  Setup();NavMesh.Blocked=true;Rejected("impassable route");
  Setup();AOHIBKLOAEI.Region=new(){OJPECMNINNA="third-party"};AOHIBKLOAEI.Region.LMPMNLLGOEC.LMMNKIOAGLC="neutral";Rejected("no neutral trespass");
  Setup();target.Power=40000;Rejected("strong defenders and border reserve prevent suicide");
  Setup();var border=new AFAGFHJONFJ{OJPECMNINNA="other-front",Power=20000};border.LMPMNLLGOEC.LMMNKIOAGLC="enemy";border.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){CombatValue=border.Power});c.FKMKGNPFLFP.Add(border);Rejected("another frontier is not emptied");
  Setup();c.LMPMNLLGOEC.OEGNGFBCHFL=50000;Rejected("food stock must cover home and army");
  Setup();c.Generated=()=>{c.Last.Morale=20;};Rejected("low morale blocked");
  Setup();c.OwnershipChangesDuringGeneration=true;Rejected("source owner change rolls back generated team");
  Setup();RunPlan();c.LMPMNLLGOEC.IPJHOOJBCKI="replacement";Check(!Start(),"governor replaced before departure");
  Setup();ICGAEMLACJG.HFJIECIBFKB[123]=new(){m_id=123,m_belongPower="F",m_targetCity=target.OJPECMNINNA};Rejected("native expedition already targets city");
  Setup();ICGAEMLACJG.HFJIECIBFKB[123]=new(){m_id=123,m_belongPower="enemy",m_targetCity=c.OJPECMNINNA};Rejected("incoming attack keeps garrison home");
  Setup();for(int i=0;i<2;i++)ICGAEMLACJG.HFJIECIBFKB[200+i]=new(){m_id=200+i,m_belongPower="F",m_warInfo=new()};Rejected("two ongoing battles prevent additional expedition");
  Setup();RunPlan();NEINKOOBJIH.MIPFHCKDJOK+=61;Check(!Start()&&c.Last.Dismissed,"stale assembly cancelled, never sent without recheck");
  Setup();NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="unrelated-player";RunPlan();Check(c.Calls==1,"same offensive policy applies to non-player faction");
  Setup();f.DNOGPFHMOMO=80000;Set("attackTreasury",70000);RunPlan();Check(c.Calls==1,"current configurable treasury threshold used");
  Setup();RunPlan();c.Last.m_isEnterCity=true;g.BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Check(Start()&&!c.Last.Dismissed,"redirected friendly movement not cancelled as old attack");
  Setup();RunPlan();int original=c.Last.m_id;c.Last.m_id++;g.BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Check(Start()&&!c.Last.Dismissed,"recycled unrelated team never cancelled");
  Setup();using(var it=((IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")).GetEnumerator()){it.MoveNext();Check((int)Call("InvestmentReserve",f)>=30000,"scan holds funds before yielding");}
  Check((int)Call("OffenseCashReserve",f)==0,"aborted queue releases hold");
  var rng=new Random(62);
  for(int i=0;i<5000;i++){
   int units=rng.Next(0,40),ready=rng.Next(0,units+1),cash=rng.Next(0,1000000);
   var reason=OffensivePolicy.Readiness(units,ready,2,cash,100000,1000);
   if(reason==null)Check(units>2&&ready>0&&cash>=100000,"random readiness invariants");
   long home=rng.Next(0,1000000),available=rng.Next(0,1000000),borderPower=rng.Next(0,1000000);
   long powerBudget=OffensivePolicy.PowerBudget(home,available,borderPower,.6);
   Check(powerBudget>=0&&powerBudget<=available&&(powerBudget==0||home-powerBudget>=Math.Max(borderPower,Math.Ceiling(home*.6))),"random combat budget preserves every frontier and home fraction");
   int defense=rng.Next(0,int.MaxValue);Check(OffensivePolicy.RequiredPower(defense,1.35)>=defense,"no power overflow");
   long stock=rng.Next(0,10000000),reserved=rng.Next(0,1000000),floor=rng.Next(0,1000000);int daily=rng.Next(1,10000),days=rng.Next(1,120);
   Check(!OffensivePolicy.FoodSafe(stock,reserved,floor,daily,days)||stock-reserved-floor>=(long)daily*days,"random food conservation");
  }
  Set("offensive",false);Call("ResetOffense");
  Console.WriteLine("PASS offensive readiness, StopWar/target restrictions, assembly cancellation, border/food/cash safety checks="+tests+"; native calls mocked, not gameplay");
 }
}
