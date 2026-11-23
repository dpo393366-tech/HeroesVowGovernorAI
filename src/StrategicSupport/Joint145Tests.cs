using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.AI;
public partial class Wo_DeployTeamCtrl {public void MoveToDes(Vector3 pos,bool a,bool b){ClearTarget(false);m_deployState=AKOPKFPKJPP.Expedition;m_listMovePos.Clear();m_listMovePos.Add(pos);}}
public static class Joint145Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int count;static AFAGFHJONFJ a,b,c;static Wo_DeployTeamCtrl enemy;static MKMOJNAJKJE faction;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static IDictionary Groups=>(IDictionary)typeof(StrategicSupport).GetField("jointHunts",F).GetValue(null);
 static IDictionary Orders=>(IDictionary)typeof(StrategicSupport).GetField("huntOrders",F).GetValue(null);
 static void Check(bool ok,string why){count++;if(!ok)throw new Exception("Joint145: "+why);}
 static void Drain(string n,params object[] args){int steps=0;foreach(var phase in (IEnumerable<string>)Call(n,args))if(++steps>20000)throw new Exception("unbounded "+n);}
 static void Setup(){
  typeof(Hunt94Tests).GetMethod("Setup",F).Invoke(null,new object[]{2});Call("ResetManualOrders");Call("ResetBattleCorridors");MLGOLGGHLMB.CBEGDDKANAO=new();
  a=MICGPAMNDKM.IJPKEMPIFIH["Runan"];a.Capacity=10;
  b=(AFAGFHJONFJ)typeof(Relief86Tests).GetMethod("City",F).Invoke(null,new object[]{"Xiangping",-10f});c=(AFAGFHJONFJ)typeof(Relief86Tests).GetMethod("City",F).Invoke(null,new object[]{"Youbeiping",-20f});
  a.FKMKGNPFLFP.Add(b);a.FKMKGNPFLFP.Add(c);b.FKMKGNPFLFP.Add(a);c.FKMKGNPFLFP.Add(a);
  faction=NEINKOOBJIH.COCMABJFOFH["F"];enemy=ICGAEMLACJG.HFJIECIBFKB[1407];enemy.Combat=359514;enemy.m_dicDeployArmy["raiders"][0].FGHFOLKBHDL=100000;
  a.Generated=()=>{a.Last.Combat=321120;a.Last.Food=10000000;a.Last.Position=a.BMBMKPGEHBH;};b.Generated=()=>{b.Last.Combat=250057;b.Last.Food=10000000;b.Last.Position=b.BMBMKPGEHBH;};c.Generated=()=>{c.Last.Combat=297442;c.Last.Food=10000000;c.Last.Position=c.BMBMKPGEHBH;};
 }
 static List<Wo_DeployTeamCtrl> Hunters()=>ICGAEMLACJG.HFJIECIBFKB.Values.Where(t=>Orders.Contains(t.Pointer)).ToList();
 static object Group()=>Groups.Values.Cast<object>().Single();
 static void Tick(){NEINKOOBJIH.MIPFHCKDJOK+=1;foreach(var g in Groups.Values.Cast<object>().ToArray())Call("CheckJointHunt",g);}
 static void Arrive(){foreach(var t in Hunters()){t.m_deployState=AKOPKFPKJPP.Camp;t.Position=a.BMBMKPGEHBH;t.m_listMovePos.Clear();t.GAANFLKOMKL.Clear();}}
 public static void Run(){
  Setup();Drain("HuntSteps",faction);Check(Groups.Count==1,"full cities can form a joint field expedition");var hunters=Hunters();Check(hunters.Count==2,"nearest sufficient two-city force instead of draining all three");Check(hunters.All(t=>t.m_attackTarget==null),"no weak contingent attacks before rendezvous");
  Check((long)Call("JointHuntPower",Group())>=485344,"actual safe commander-adjusted power pooled");Check(hunters.All(t=>t.GetCombatVal()<485344),"neither individual army could previously pass");Check(hunters.All(t=>(bool)Call("GuardHuntStart",t)),"departure guard accepts jointly sufficient staged contingents");
  int calls=a.Calls+b.Calls+c.Calls;Drain("HuntSteps",faction);Check(calls==a.Calls+b.Calls+c.Calls,"active joint target prevents duplicate recruitment");
  int paths=NavMesh.Calls105;for(int i=0;i<1000;i++)Drain("HuntSteps",faction);Check(paths==NavMesh.Calls105&&calls==a.Calls+b.Calls+c.Calls,"1000 active-plan decisions perform no new path search or army generation");
  foreach(var t in hunters){t.m_deployState=AKOPKFPKJPP.Expedition;Call("RouteNewRelief",t);}
  Check(hunters.All(t=>!t.m_isEnterCity),"field gathering consumes no city slots");Check(b.Last.Position.x==-10,"route installation never teleports donor");
  Tick();Check(Hunters().All(t=>t.m_attackTarget==null)&&Groups.Count==1,"early arrival waits for distant real arrival");
  var refs=hunters.SelectMany(t=>t.m_dicDeployArmy.Values.SelectMany(x=>x)).Select(u=>u.Pointer).ToHashSet();long soldiers=hunters.Sum(t=>(long)t.GetSoldierNum(false)),food=hunters.Sum(t=>(long)t.Food);int heroes=hunters.Sum(t=>t.m_listHero.Count);
  Arrive();Tick();Check(Groups.Count==0&&Hunters().Count==1,"physically arrived contingents consolidated through native merge");var leader=Hunters().Single();Check(leader.m_attackTarget==enemy&&leader.GetCombatVal()>=485344,"merged real power rechecked before attack");Check(leader.GetSoldierNum(false)==soldiers&&leader.Food==food&&leader.m_listHero.Count==heroes&&refs.SetEquals(leader.m_dicDeployArmy.Values.SelectMany(x=>x).Select(u=>u.Pointer)),"native merge conserves exact troops officers and food");
  Setup();Drain("HuntSteps",faction);enemy.Combat=2000000;Tick();Check(Groups.Count==0&&Hunters().Count==0,"enemy growth cancels insufficient total before solo attack");
  Setup();Drain("HuntSteps",faction);var manual=Hunters().First();object[] input={0};Call("BeginManualInput",input);Call("AfterManualTeamCommand",manual);Call("EndManualInput",input[0]);Tick();Check(Groups.Count==0&&!manual.Dismissed,"manual override excluded and never cancelled by joint cleanup");
  Setup();Drain("HuntSteps",faction);b.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Tick();Check(Groups.Count==0,"captured donor cancels shared commitment");
  Setup();Drain("HuntSteps",faction);faction.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Tick();Check(Groups.Count==0,"StopWar respected during rendezvous");
  Setup();Drain("HuntSteps",faction);foreach(var t in Hunters())t.m_deployState=AKOPKFPKJPP.Expedition;Hunters()[0].Food=0;Tick();Check(Groups.Count==0,"food loss cannot strand waiting army");
  Setup();Drain("HuntSteps",faction);ICGAEMLACJG.HFJIECIBFKB.Remove(enemy.m_id);Tick();Check(Groups.Count==0,"removed target releases all commitments");
  Setup();Drain("HuntSteps",faction);foreach(var t in Hunters()){t.m_deployState=AKOPKFPKJPP.Expedition;Call("RouteNewRelief",t);}for(int i=0;i<8;i++)Tick();Check(Groups.Count==0,"immobile donor cannot cause infinite camp");
  Setup();Drain("HuntSteps",faction);Arrive();var list=Hunters();list[1].m_dicDeployArmy["duplicate"]=list[0].m_dicDeployArmy.Values.First();Tick();Check(Groups.Count==0,"same troop cannot provide double power credit");
  Setup();Drain("HuntSteps",faction);foreach(var t in Hunters())t.m_deployState=AKOPKFPKJPP.Expedition;NavMesh.Blocked=true;Tick();Check(Groups.Count==0,"physical route failure aborts field move");NavMesh.Blocked=false;
  Setup();a.Generated=()=>{a.Last.Combat=10000;a.Last.Food=10000000;};b.Generated=()=>{b.Last.Combat=10000;b.Last.Food=10000000;};c.Generated=()=>{c.Last.Combat=10000;c.Last.Food=10000000;};Drain("HuntSteps",faction);Check(Groups.Count==0,"truly insufficient surrounding cities do not launch suicide hunt");
  Setup();Drain("HuntSteps",faction);enemy.Position=a.BMBMKPGEHBH;Tick();Check(Groups.Count==0,"moving raider cannot enter a supposedly safe staging point unchecked");
  Setup();Drain("HuntSteps",faction);faction.HMLLPOCACMJ=new();foreach(var t in Hunters())faction.HMLLPOCACMJ.Add(t);var original=faction.HMLLPOCACMJ;object[] interception={faction,new Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl>(),null};Call("BeforeNativeInterception",interception);Check(faction.HMLLPOCACMJ.Count==0,"native interception cannot steal gathering armies");Call("AfterNativeInterception",interception[2]);Check(ReferenceEquals(faction.HMLLPOCACMJ,original),"native registry restored after scoped interception filter");
  Setup();Drain("HuntSteps",faction);a.LMPMNLLGOEC.OEGNGFBCHFL=0;Tick();Check(Groups.Count==0,"source food reserve enforced before actual departure");
  Setup();Drain("HuntSteps",faction);var together=Hunters();foreach(var t in together)t.m_deployState=AKOPKFPKJPP.Expedition;
  var war=new NJHDEHLHENN{HIENOINGBCN="Wuhuan",EMMJBPMNFAK=new[]{"F",""},FGHFOLKBHDL=new[]{100000,100000},GBHGENNMJCK=enemy.Position};enemy.m_warInfo=war;together[0].m_warInfo=war;Tick();
  Check(Groups.Count==0&&!together[0].Dismissed&&together[1].m_targetWarInfo==war,"ambushed rendezvous hands partners to battle support instead of abandoning battle");
  Setup();Drain("HuntSteps",faction);var stale=Hunters();MICGPAMNDKM.IJPKEMPIFIH=new();Tick();Check(Groups.Count==0&&stale.All(t=>!t.Dismissed),"world replacement discards old plans without mutating old or restored armies");
  for(int seed=0;seed<60;seed++){
   Setup();var random=new Random(seed);int pa=random.Next(250000,340000),pb=random.Next(250000,340000);
   a.Generated=()=>{a.Last.Combat=pa;a.Last.Food=10000000;a.Last.Position=a.BMBMKPGEHBH;};b.Generated=()=>{b.Last.Combat=pb;b.Last.Food=10000000;b.Last.Position=b.BMBMKPGEHBH;};Drain("HuntSteps",faction);
   Check(Groups.Count==1,"seeded joint plan "+seed);var wave=Hunters();long men=wave.Sum(t=>(long)t.GetSoldierNum(false));long supply=wave.Sum(t=>(long)t.GetFood());
   Check(wave.All(t=>t.m_attackTarget==null)&&wave.Count==2,"seeded no premature or excess dispatch "+seed);
   Check(a.LMPMNLLGOEC.KJPPGIOCDID.Count>=4&&b.LMPMNLLGOEC.KJPPGIOCDID.Count>=4,"seeded local garrison preserved "+seed);
   if(seed%3==0){enemy.Combat=1000000;Tick();Check(Groups.Count==0&&wave.All(t=>t.m_attackTarget==null),"seeded enemy growth abort "+seed);}
   else if(seed%3==1){Arrive();Tick();Check(Groups.Count==0&&Hunters().Single().GetSoldierNum(false)==men&&Hunters().Single().GetFood()==supply,"seeded physical merge conservation "+seed);}
   else {foreach(var t in wave)t.m_deployState=AKOPKFPKJPP.Expedition;NavMesh.Blocked=true;Tick();Check(Groups.Count==0&&wave.All(t=>t.m_attackTarget==null),"seeded route failure no attack "+seed);NavMesh.Blocked=false;}
  }
  Call("ResetWork");Call("ResetOffense");Orders.Clear();AOHIBKLOAEI.RegionAt=null;Console.WriteLine($"PASS Joint145 {count} full-city coalition, real arrival, conservation, manual orders, policy, food, target changes and stalled routes; native actions mocked.");
 }
}
