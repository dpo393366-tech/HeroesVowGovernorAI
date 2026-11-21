using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class Extreme103Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;static readonly List<string> failures=new();
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static T Field<T>(Type type,string name)=>(T)type.GetField(name,F).GetValue(null);
 static object New(string name)=>Activator.CreateInstance(typeof(StrategicSupport).GetNestedType(name,BindingFlags.NonPublic),true);
 static void Put(object obj,string field,object value)=>obj.GetType().GetField(field).SetValue(obj,value);
 static T Get<T>(object obj,string field)=>(T)obj.GetType().GetField(field).GetValue(obj);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
 static void Case(string name,Action run){try{run();Console.WriteLine("EXTREME103 OK "+name);}catch(Exception e){while(e.InnerException!=null)e=e.InnerException;failures.Add(name+": "+e.Message);Console.WriteLine("EXTREME103 FAIL "+name+": "+e.Message);}}
 static void Setup(){typeof(IntegrationTests).GetMethod("Prepare",F).Invoke(null,null);Call("ResetWork");Call("ResetDefense");Call("ResetBattleState");Call("ResetManualOrders");Call("Ready");Field<IDictionary>(typeof(StrategicSupport),"roadChoices").Clear();AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;NavMesh.Blocked=false;NavMesh.Detour=0;Wa_Controller.m_warInfo=null;}
 static IEnumerable<string> Route(AFAGFHJONFJ a,AFAGFHJONFJ b,MKMOJNAJKJE f,object plan)=>(IEnumerable<string>)Call("ReliefPathSteps",a.BMBMKPGEHBH,b.BMBMKPGEHBH,f,null,false,plan);
 static void Routes(int seed){
  var random=new Random(seed);
  for(int trial=0;trial<12;trial++){
   Setup();var f=NEINKOOBJIH.COCMABJFOFH["F"];var a=MICGPAMNDKM.IJPKEMPIFIH["source"];var b=MICGPAMNDKM.IJPKEMPIFIH["target"];
   a.BMBMKPGEHBH=new(0);b.BMBMKPGEHBH=new(random.Next(20,200));AOHIBKLOAEI.RegionAt=p=>p.x<1?a:b;
   var first=New("RoutePlan");foreach(var p in Route(a,b,f,first)){}Check(Get<bool>(first,"Success"),"baseline legal route exists");
   var plan=New("RoutePlan");using var scan=Route(a,b,f,plan).GetEnumerator();Check(scan.MoveNext(),"cached path revalidation yields");
   b.LMPMNLLGOEC.LMMNKIOAGLC="enemy";int bound=0;while(scan.MoveNext())Check(++bound<1000,"changed route terminates");
   Check(!Get<bool>(plan,"Success"),"cached path must reject destination captured during yield");
  }
 }
 static void FieldBudget(int seed){
  Setup();var rng=new Random(seed);var war=new NJHDEHLHENN{HIENOINGBCN="field103",EMMJBPMNFAK=new[]{"F","enemy"},IJGLCFJOOIK=null,FGHFOLKBHDL=new[]{100000,20000}};
  var home=new Wo_DeployTeamCtrl{m_belongPower="F",m_warInfo=war,Combat=100000};var foe=new Wo_DeployTeamCtrl{m_belongPower="enemy",m_warInfo=war,Combat=20000};var teams=new List<Wo_DeployTeamCtrl>{home,foe};
  var baseline=(FrontAllocationPolicy.Budget)Call("FrontBudget",war,0,30d,teams,1.2);
  for(int i=0;i<80;i++){
   var irrelevant=new Wo_DeployTeamCtrl{m_id=100+i,m_belongPower="enemy",m_targetCity=null,m_deployState=AKOPKFPKJPP.Expedition,Position=war.GBHGENNMJCK,Combat=rng.Next(1000,200000)};irrelevant.m_dicDeployArmy["g"]=new(){new(){FGHFOLKBHDL=irrelevant.Combat}};teams.Add(irrelevant);
   var now=(FrontAllocationPolicy.Budget)Call("FrontBudget",war,0,30d,teams,1.2);
   Check(now.Men==baseline.Men&&now.Power==baseline.Power,"unrelated goal-less army cannot count as field reinforcements");
   irrelevant.m_targetWarInfo=war;now=(FrontAllocationPolicy.Budget)Call("FrontBudget",war,0,30d,teams,1.2);Check(now.Men<=baseline.Men&&now.Power<=baseline.Power,"real hostile wave reduces detachable surplus");teams.Remove(irrelevant);
  }
 }
 static void CoverAdmission(int seed){
  Setup();var rng=new Random(seed);var city=MICGPAMNDKM.IJPKEMPIFIH["target"];city.LMPMNLLGOEC.KJPPGIOCDID.Clear();city.LMPMNLLGOEC.OEGNGFBCHFL=100000000;city.Capacity=3;
  ICGAEMLACJG.HFJIECIBFKB.Clear();var covers=Field<IDictionary>(typeof(StrategicSupport),"coverOrders");
  var t=new Wo_DeployTeamCtrl{m_id=103,m_belongPower="F",m_assembleCity="source",m_targetCity="target",m_isEnterCity=true,m_deployState=AKOPKFPKJPP.Expedition,Food=1000000,Position=city.BMBMKPGEHBH};t.m_dicDeployArmy["a"]=new(){new(){FGHFOLKBHDL=1000},new(){FGHFOLKBHDL=1000}};
  var other=new Wo_DeployTeamCtrl{m_id=104,m_belongPower="F",m_targetCity="target",m_isEnterCity=true,m_deployState=AKOPKFPKJPP.Expedition,Food=1000000};other.m_dicDeployArmy["b"]=new(){new(){FGHFOLKBHDL=1000},new(){FGHFOLKBHDL=1000}};
  ICGAEMLACJG.HFJIECIBFKB[103]=t;ICGAEMLACJG.HFJIECIBFKB[104]=other;
  var order=New("CoverOrder");Put(order,"Team",t);Put(order,"Id",t.m_id);Put(order,"Faction","F");Put(order,"Target","target");Put(order,"Source","source");covers[t.Pointer]=order;
  Check((int)Call("CapacityRoom",city,new List<Wo_DeployTeamCtrl>{t,other},t)<2,"inbound reservations exceed room in fixture");
  Check((bool)Call("GuardCoverEntry",t,city),"arrived backfill uses physical capacity, not mutual future reservations");
  foreach(var unit in t.m_dicDeployArmy["a"])city.LMPMNLLGOEC.KJPPGIOCDID.Add(unit);
  Check(!(bool)Call("CanAdmitReturn",city,other),"later arrival cannot exceed capacity after first enters");
 }
 static void StaleLoan(){
  Setup();var loans=Field<IDictionary>(typeof(StrategicSupport),"reliefLoans");loans.Clear();var t=new Wo_DeployTeamCtrl{m_id=110,m_belongPower="F"};
  object Loan(int id){var x=New("ReliefLoan");Put(x,"Team",t);Put(x,"Id",id);Put(x,"Faction","F");Put(x,"City","source");return x;}
  var old=Loan(109);var current=Loan(110);loans[t.Pointer]=current;
  Check(!(bool)Call("CheckReliefLoan",old)&&ReferenceEquals(loans[t.Pointer],current),"old queued loan cannot delete replacement order on recycled army");
 }
 static void NullLoan(){Setup();var loans=Field<IDictionary>(typeof(StrategicSupport),"reliefLoans");loans.Clear();var stale=New("ReliefLoan");Put(stale,"Faction","F");loans[(IntPtr)99103]=stale;Check(!(bool)Call("CheckReliefLoan",stale)&&loans.Count==0,"missing army loan is removed without null dereference");}
 static void Numeric(int seed){
  var rng=new Random(seed);for(int i=0;i<150;i++){
   int treasury=rng.Next(int.MaxValue),city=rng.Next(int.MaxValue),price=rng.Next(int.MaxValue),tr=rng.Next(treasury+1),cr=rng.Next(city+1);
   int transfer=InvestmentPolicy.TransferForPurchase(treasury,city,price,tr,cr);Check(transfer>=0&&(long)treasury-transfer>=tr&&(long)city+transfer<=int.MaxValue,"random treasury transfer preserves reserve and avoids overflow");
   var waves=Enumerable.Range(0,rng.Next(1,15)).Select(_=>new TimedBorderDefense.Wave(rng.Next(0,300),rng.Next(1,100000))).ToArray();double own=rng.Next(1,1000000),period=rng.Next(10,1000);
   double actual=TimedBorderDefense.Collapse(own,waves,period);double consumed=waves.Sum(w=>Math.Max(0,actual-w.At)*w.Power/period);Check(Math.Abs(consumed-own)<Math.Max(1,own)*1e-8,"collapse matches independent integrated loss equation");
   double later=TimedBorderDefense.Collapse(own*1.5,waves.Reverse(),period);Check(later>=actual,"more retained troops cannot shorten survival");
   var a=new List<ReinforcementTimeline.Wave>{new(0,rng.Next(100,10000)),new(20,rng.Next(100,10000))};var e=new List<ReinforcementTimeline.Wave>{new(10,rng.Next(100,100000)),new(30,rng.Next(100,100000))};
   int need=ReinforcementTimeline.Need(own,1000,10,0,5,80,a,e,1.2,10,out _,out _,true);a.Reverse();e.Reverse();int perm=ReinforcementTimeline.Need(own,1000,10,0,5,80,a,e,1.2,10,out _,out _,true);Check(need==perm&&need>=0,"wave insertion order does not change support forecast");
   var arrivals=waves.Select(w=>new FrontAllocationPolicy.Arrival(w.At,w.Power,w.Power)).ToArray();var budget=FrontAllocationPolicy.Available(own,100,own,100,10,40,arrivals);Check(double.IsFinite(budget.Men)&&budget.Men>=0&&budget.Men<=own&&budget.Power>=0&&budget.Power<=own,"multi-front withdrawal cannot exceed actual force");
  }
 }
 static void QueueStress(int seed){
  var rng=new Random(seed);var q=new WorkQueue();var expected=new Dictionary<int,int>();var actual=new Dictionary<int,int>();int faultExpected=0,faultActual=0;
  IEnumerator<string> Steps(int id,int n,bool fault){for(int i=0;i<n;i++){actual[id]++;yield return "work";}if(fault)throw new Exception("injected103");}
  for(int i=0;i<300;i++){int id=i,count=rng.Next(1,20);bool fault=i%17==0;if(fault)faultExpected++;expected[id]=count;actual[id]=0;string key=(i%3==0?"campaign:":i%3==1?"recruit-queue:":"city:")+i;Check(q.Add(key,()=>Steps(id,count,fault)),"mixed queue accepts unique work");Check(!q.Add(key,()=>Steps(id,count,fault)),"duplicate queued command cannot spend twice");}
  int frames=0;while(q.Count>0&&frames++<15000)q.Pump(50,rng.Next(1,20),(_,_)=>{},_=>faultActual++);
  Check(q.Count==0&&faultActual==faultExpected&&expected.All(k=>actual[k.Key]==k.Value),"300 mixed jobs drain fairly with independent faults and no lost steps");
 }
 static void RecruitmentAuthority(){
  typeof(GovernorFeatureTests).GetMethod("Setup",F).Invoke(null,null);var f=Field<MKMOJNAJKJE>(typeof(GovernorFeatureTests),"faction");var city=Field<AFAGFHJONFJ>(typeof(GovernorFeatureTests),"city");
  typeof(GovernorFeatureTests).GetMethod("Candidate",F).Invoke(null,new object[]{"candidate103",KLMDJNIBCLD.Female,99,true});StrategicSupport.StartRecruitmentQueue(1,2);
  using var scan=((IEnumerable<string>)Call("RecruitmentQueueSteps",f)).GetEnumerator();Check(scan.MoveNext(),"recruitment yields before commit");NEINKOOBJAHeroFaction("changed-faction");
  int steps=0;while(scan.MoveNext())Check(++steps<20000,"recruitment authority change terminates");Check(!city.Roster.Any(h=>h.OGPEJIEHDAN.LKEPGHJDHGG=="candidate103"),"leaving faction cancels suspended user recruitment before dispatch");
  void NEINKOOBJAHeroFaction(string faction)=>NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC=faction;
 }
 static void SplitConservation(int seed){
  var rng=new Random(seed);Setup();for(int trial=0;trial<35;trial++){
   ICGAEMLACJG.HFJIECIBFKB.Clear();var t=new Wo_DeployTeamCtrl{m_id=103000+trial,m_belongPower="F",m_assembleCity="source",Food=rng.Next(10000,100000000),Combat=0,m_deployState=AKOPKFPKJPP.Camp};
   int groups=rng.Next(2,21);for(int i=0;i<groups;i++){var h=new CLAGKDECFID{OJPECMNINNA="split103-"+i};t.m_listHero.Add(h);var troops=new Il2CppSystem.Collections.Generic.List<DHKINGPDKJJ>();for(int j=0;j<rng.Next(1,6);j++){var a=new DHKINGPDKJJ{FGHFOLKBHDL=rng.Next(1,10000),CombatValue=rng.Next(1,50000)};troops.Add(a);t.Combat+=a.CombatValue;}t.m_dicDeployArmy[h.OJPECMNINNA]=troops;}
   ICGAEMLACJG.HFJIECIBFKB[t.m_id]=t;var units=(HashSet<IntPtr>)Call("TeamTroops",t);var heroes=t.m_listHero.Select(h=>h.Pointer).ToHashSet();long food=t.GetFood(),men=t.GetSoldierNum(false);int cap=rng.Next(2,Math.Max(3,t.Combat));
   Wo_DeployTeamCtrl.DirtySplit95=true;Wo_DeployTeamCtrl child;try{child=(Wo_DeployTeamCtrl)Call("SplitFieldArmy",t,(long)cap);}finally{Wo_DeployTeamCtrl.DirtySplit95=false;}
   if(child){var a=(HashSet<IntPtr>)Call("TeamTroops",t);var b=(HashSet<IntPtr>)Call("TeamTroops",child);Check(!a.Overlaps(b),"split troops never duplicated");a.UnionWith(b);Check(a.SetEquals(units)&&t.GetSoldierNum(false)+(long)child.GetSoldierNum(false)==men&&t.GetFood()+(long)child.GetFood()==food,"random mixed troop split preserves exact identity, men and food");Check(child.GetCombatVal()<=cap&&string.IsNullOrEmpty(child.m_targetCamp)&&!child.m_responseToPlayer,"pooled child respects power ceiling and has no obsolete camp/follow");child.MergeToTeam(t);}
   Check(((HashSet<IntPtr>)Call("TeamTroops",t)).SetEquals(units)&&t.GetSoldierNum(false)==men&&t.GetFood()==food&&t.m_listHero.Count==heroes.Count&&t.m_listHero.Select(h=>h.Pointer).ToHashSet().SetEquals(heroes),"split/rejected split/rollback conserves commanders and troops");
  }
 }
 static void ManualInterruption(){
  int visited=0;
  for(int interrupt=1;interrupt<=40;interrupt++){
   MLGOLGGHLMB.CBEGDDKANAO=new();Call("ResetManualOrders");typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
   var t=Field<Wo_DeployTeamCtrl>(typeof(Campaign81Tests),"t");t.Combat=1;var goals=Field<IDictionary>(typeof(StrategicSupport),"campaignGoals");var goal=goals[t.Pointer];
   using var scan=((IEnumerable<string>)Call("RecoverCampaign",t,goal)).GetEnumerator();int yielded=0;bool changed=false;int moves=0;
   while(scan.MoveNext()){
    Check(++yielded<2000,"campaign coroutine terminates");if(yielded!=interrupt)continue;
    changed=true;visited++;object[] input={0};Call("BeginManualInput",input);try{t.m_targetCity="player-new-order";t.m_listMovePos.Clear();t.m_listMovePos.Add(new(321));t.m_deployState=AKOPKFPKJPP.Expedition;Call("AfterManualTeamCommand",t);}finally{Call("EndManualInput",input[0]);}moves=t.CityMoves;
   }
   if(changed)Check(t.m_targetCity=="player-new-order"&&t.CityMoves==moves&&t.m_listMovePos.Count==1&&t.m_listMovePos[0].x==321,"manual order interrupts every tested suspended campaign phase");
  }
  Check(visited>=5,"manual interruption reached multiple real production yield points");Console.WriteLine("EXTREME103 manual interruptible phases="+visited);
 }
 static void LayoutAndOfficers(int seed){
  var rng=new Random(seed);for(int trial=0;trial<6;trial++){
   var actions=new List<DevelopmentPolicy.Action>();int loss=rng.Next(1,100),cells=rng.Next(3,7);
   for(int cell=0;cell<cells;cell++){
    actions.Add(new(){Id="goal",District="D",Tiles=new[]{(long)cell},Limit=2,Priority=10,SecurityFlow=-loss,Gains=new(){{"goal",1}}});
    actions.Add(new(){Id="police",District="D",Tiles=new[]{(long)cell},Limit=1,Priority=9,SecurityFlow=2*loss});
    actions.Add(new(){Id="farm",District="D",Tiles=new[]{(long)cell},Priority=1,Food=rng.Next(1,1000)});
   }
   actions=actions.OrderBy(_=>rng.Next()).ToList();List<int> chosen=null;int slices=0;foreach(var p in CityLayoutOptimizer.Solve(actions,new(){{"goal",new(){Missing=2,Target=2}}},x=>chosen=x)){Check(++slices<100000,"layout search bounded");}
   Check(chosen!=null&&chosen.Count==cells&&chosen.Count(i=>actions[i].Id=="goal")==2,"feasible two-building goal and full plot filling survive action permutation");double sec=1;var used=new HashSet<long>();foreach(int i in chosen){sec+=actions[i].SecurityFlow;Check(sec>=1&&actions[i].Tiles.All(used.Add),"every executable layout prefix preserves security and no plot overlap");}
   var cities=Enumerable.Range(0,rng.Next(2,30)).Select(i=>new OfficerDeploymentPolicy.City{Id="city"+i,Tier=rng.Next(1,4),Minimum=rng.Next(1,12),Vulnerable=rng.Next(4)==0}).ToList();int total=rng.Next(1,600);var quotas=OfficerDeploymentPolicy.Quotas(cities,total);Check(quotas.Values.Sum()==total&&quotas.All(x=>x.Value>=0),"random frontier officer quotas conserve scarce and abundant personnel");
  }
 }
 public static void Run(){
  string selected=Environment.GetEnvironmentVariable("EXTREME103_CASE");
  void RunCase(string name,Action body){if(string.IsNullOrEmpty(selected)||selected==name)Case(name,body);}
  RunCase("cached-territory",()=>Routes(103));RunCase("field-null-target",()=>FieldBudget(103));RunCase("cover-capacity",()=>CoverAdmission(103));RunCase("stale-loan",StaleLoan);RunCase("missing-loan",NullLoan);RunCase("recruit-authority",RecruitmentAuthority);RunCase("manual-interruption",ManualInterruption);
  int offset=int.TryParse(Environment.GetEnvironmentVariable("EXTREME103_SEED_OFFSET"),out var parsed)?parsed:0;
  for(int seed=0;seed<24;seed++){int s=10300+offset+seed;RunCase("numeric-"+seed,()=>Numeric(s));RunCase("queue-"+seed,()=>QueueStress(s));RunCase("split-"+seed,()=>SplitConservation(s));RunCase("layout-officers-"+seed,()=>LayoutAndOfficers(s));}
  Call("ResetWork");Call("ResetDefense");AOHIBKLOAEI.RegionAt=null;
  Console.WriteLine("EXTREME103 checks="+checks+" failures="+failures.Count+" seeds=24 native-actions-mocked");if(failures.Count>0)throw new Exception(string.Join(" | ",failures));
 }
}
