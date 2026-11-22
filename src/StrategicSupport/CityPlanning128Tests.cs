using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;
public static class CityPlanning128Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static object Call(string name,params object[] a)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,a);
 static T Field<T>(Type type,string name)=>(T)type.GetField(name,F).GetValue(null);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("CityPlanning128: "+why);}
 static (AFAGFHJONFJ,MKMOJNAJKJE,JNHOOCJJPJO) Setup(){
  typeof(CityIntegrationTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");
  var c=Field<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");var f=Field<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");var d=c.FGBLEMGGOPO[0];
  d.HGNFEMJOPPB.Clear();for(int i=0;i<4;i++)d.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=d});
  AEHAGJAKNEK.ACIGFKMDOKE.Clear();AEHAGJAKNEK.ACIGFKMDOKE["market"]=new(){OJPECMNINNA="market",CNCPIILGDEF=100,NPCHPDNFAME=600};return(c,f,d);
 }
 static void Plan(AFAGFHJONFJ c,MKMOJNAJKJE f){foreach(var _ in (IEnumerable<bool>)Call("BuildScanSteps",c,f,true)){} }
 static int Execute(AFAGFHJONFJ c,MKMOJNAJKJE f,out bool refresh){object[] a={c,f,false};int result=(int)Call("ExecuteSavedLayout",a);refresh=(bool)a[2];return result;}
 static void Drain(IEnumerator<string> steps){using(steps)while(steps.MoveNext()){} }
 static IEnumerator<string> Busy(Action hit){while(true){hit();yield return "busy";}}
 public static void Run(){
  var (c,f,d)=Setup();long cash=(long)c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO;Plan(c,f);
  Check(d.AIMLPNBOIIF.Count==0&&cash==(long)c.LMPMNLLGOEC.DNOGPFHMOMO+f.DNOGPFHMOMO,"background planning cannot buy or reserve funds");
  Check(Execute(c,f,out bool refresh)==1&&!refresh&&d.AIMLPNBOIIF.Count==1,"cached plan buys without another layout scan");
  d.AIMLPNBOIIF[0].EMHNNCLBADE=100;
  Check(Execute(c,f,out refresh)==1&&!refresh&&d.AIMLPNBOIIF.Count==2,"completion keeps remaining geometry and does not replan whole city");
  Check(cash-(long)c.LMPMNLLGOEC.DNOGPFHMOMO-f.DNOGPFHMOMO==1200,"two cached purchases pay exactly twice");
  (c,f,d)=Setup();Plan(c,f);c.LMPMNLLGOEC.DNOGPFHMOMO=0;f.DNOGPFHMOMO=0;
  Check(Execute(c,f,out refresh)==2&&!refresh&&d.AIMLPNBOIIF.Count==0,"cash shortage retains existing plan without requesting layout work");
  c.LMPMNLLGOEC.DNOGPFHMOMO=100000;
  Check(Execute(c,f,out refresh)==1&&!refresh,"cash increase resumes same plan immediately");
  (c,f,d)=Setup();Plan(c,f);c.Scale++;
  Check(Execute(c,f,out refresh)==0&&refresh&&d.AIMLPNBOIIF.Count==0,"expansion requests new plan before stale purchase");
  (c,f,d)=Setup();Plan(c,f);AEHAGJAKNEK.ACIGFKMDOKE["market"].TechAllowed=false;
  Check(Execute(c,f,out refresh)==0&&refresh,"research/catalogue change invalidates cached execution");
  (c,f,d)=Setup();Plan(c,f);c.LMPMNLLGOEC.IPJHOOJBCKI="other";
  Check(Execute(c,f,out refresh)==0&&refresh,"governor handover never consumes previous command");
  (c,f,d)=Setup();Plan(c,f);foreach(var plot in d.HGNFEMJOPPB)foreach(var tile in plot.FAFNFFJAFKH)tile.FCOJFBEHMAG=9999;
  Check(Execute(c,f,out refresh)==0&&refresh&&d.AIMLPNBOIIF.Count==0,"occupied parcels rejected even if no registered building was added");
  (c,f,d)=Setup();d.HGNFEMJOPPB.Clear();Plan(c,f);
  Check(Execute(c,f,out refresh)==2&&!refresh,"no-vacancy result retained instead of scanning every day");
  d.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=d});Check(Execute(c,f,out refresh)==0&&refresh,"new vacant plot wakes cached empty plan");
  (c,f,d)=Setup();for(int i=0;i<6;i++)d.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=d});Plan(c,f);
  string governor=c.LMPMNLLGOEC.IPJHOOJBCKI;Call("OpenConstructionVisit",c,f,governor);
  Drain((IEnumerator<string>)Call("CityConstructionWork",c,f,governor));
  Check(d.AIMLPNBOIIF.Count==8,"one city visit starts at most eight cached buildings");
  Call("OpenConstructionVisit",c,f,governor);Drain((IEnumerator<string>)Call("CityConstructionWork",c,f,governor));
  Check(d.AIMLPNBOIIF.Count==8,"background callbacks or duplicate same-day admissions cannot refill construction allowance");
  NEINKOOBJIH.MIPFHCKDJOK+=3;Call("OpenConstructionVisit",c,f,governor);Drain((IEnumerator<string>)Call("CityConstructionWork",c,f,governor));
  Check(d.AIMLPNBOIIF.Count==10,"next NPC management period resumes remaining planned buildings");
  // Other districts may finish independent upgrades while this district's
  // reserved footprint is still safe to execute during background replanning.
  (c,f,d)=Setup();var other=new JNHOOCJJPJO{OJPECMNINNA="other",AJJCCPCJPFC=c};c.FGBLEMGGOPO.Add(other);Plan(c,f);
  other.AIMLPNBOIIF.Add(new(){OJPECMNINNA=128999,GMIKIMACDBC=other,GEKKLMPLDHJ=AEHAGJAKNEK.ACIGFKMDOKE["market"]});
  Check(Execute(c,f,out refresh)==1&&refresh&&d.AIMLPNBOIIF.Count==1,"unaffected district continues while changed district requests replanning");
  // Production dispatcher must enqueue independent lanes without draining an
  // existing long attack/planning coroutine before admitting research.
  (c,f,d)=Setup();var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");int attack=0;
  queue.Add("city-offense:"+c.OJPECMNINNA,()=>Busy(()=>attack++));Drain((IEnumerator<string>)Call("CityWork",c));
  Check(attack==0&&queue.Count>=6,"dispatcher schedules economy independently of an active attack job");
  int research=0;f.ANMMMOIKLEG["gov"].OnResearch=()=>research++;
  for(int i=0;i<200&&research==0;i++)queue.Pump(10000,1,null,e=>throw e);
  Check(research>0,"research executes while attack coroutine is still active");Call("ResetWork");
  var q=new WorkQueue();int urgent=0,normal=0,battle=0,recruit=0,recovery=0;var economy=new int[24];
  for(int i=0;i<180;i++){q.Add("defense:"+i,()=>Busy(()=>urgent++));q.Add("city:"+i,()=>Busy(()=>normal++));}
  q.Add("battle-support-plan:w",()=>Busy(()=>battle++));q.Add("recruit-queue:f",()=>Busy(()=>recruit++));q.Add("campaign:t",()=>Busy(()=>recovery++));
  for(int i=0;i<economy.Length;i++){int k=i;q.Add("economy:construction:"+i,()=>Busy(()=>economy[k]++));}
  for(int i=0;i<3000;i++)q.Pump(10000,1,null,e=>throw e);
  Check(economy.All(n=>n>0)&&urgent>0&&normal>0&&battle>0&&recruit>0&&recovery>0,"saturated mixed workload preserves every lane and every city's economy turn");
  Check(economy.Max()-economy.Min()<=1,"economy cities remain FIFO fair under permanent battle load");
  int before=economy.Sum();q.Pump(0,128,null,e=>throw e);Check(economy.Sum()-before<=1,"economy priority never bypasses global frame time budget");q.Clear();
  foreach(var rival in new[]{"battle-support-plan:only","recruit-queue:only","campaign:only"}){
   var pair=new WorkQueue();int rivalHits=0,economyHits=0;
   pair.Add(rival,()=>Busy(()=>rivalHits++));pair.Add("economy:construction:only",()=>Busy(()=>economyHits++));
   for(int i=0;i<128;i++)pair.Pump(10000,1,null,e=>throw e);
   Check(rivalHits>0&&economyHits>0,"economy remains runnable with only one other priority lane: "+rival);pair.Clear();
  }
  (c,f,d)=Setup();var oldBuilding=AEHAGJAKNEK.ACIGFKMDOKE["market"];
  var upgraded=new PABIPDOFBFG{OJPECMNINNA="upgraded",CNCPIILGDEF=300,NPCHPDNFAME=900,MDOIJIAONJM="market"};
  AEHAGJAKNEK.ACIGFKMDOKE["upgraded"]=upgraded;
  for(int i=0;i<40;i++)AEHAGJAKNEK.ACIGFKMDOKE["banned"+i]=new(){OJPECMNINNA="banned"+i,POLCGKHKLHI=true};
  var instance=new OGABIJECHFG{OJPECMNINNA=12901,GMIKIMACDBC=d,GEKKLMPLDHJ=oldBuilding};d.AIMLPNBOIIF.Add(instance);
  using(var scan=((IEnumerable<bool>)Call("BuildScanSteps",c,f,true)).GetEnumerator()){
   Check(scan.MoveNext(),"scan yields before planning");instance.GEKKLMPLDHJ=upgraded;
   while(scan.MoveNext()){}
  }
  Check(Execute(c,f,out refresh)==1,"upgrade during scan refreshes effects and starts a building instead of discarding geometry");
  (c,f,d)=Setup();using(var scan=((IEnumerable<bool>)Call("BuildScanSteps",c,f,true)).GetEnumerator()){
   Check(scan.MoveNext(),"expansion probe yields");c.Scale++;while(scan.MoveNext()){}
  }
  Check(Execute(c,f,out refresh)==0&&refresh,"expansion during scan still rejects obsolete geometry");
  var fair=new WorkQueue();int planning=0,regularEconomy=0,otherWork=0;
  for(int i=0;i<160;i++){fair.Add("city:pending"+i,()=>Busy(()=>otherWork++));fair.Add("economy:maintenance:pending"+i,()=>Busy(()=>regularEconomy++));}
  fair.Add("layout:waiting-city",()=>Busy(()=>planning++));
  fair.Add("battle-support-plan:busy",()=>Busy(()=>{}));fair.Add("recruit-queue:busy",()=>Busy(()=>{}));fair.Add("campaign:busy",()=>Busy(()=>{}));
  int quanta=0;while(planning<50&&quanta<2000){fair.Pump(10000,1,null,e=>throw e);quanta++;}
  Check(planning>=50&&regularEconomy>0&&otherWork>0,"layout advances without waiting behind 160 cities while preserving other lanes");
  Console.WriteLine($"CityPlanning130 reserved layout: 50 scan slices in {quanta} queue steps with 320 unrelated permanent jobs");fair.Clear();
  // A finished plan must not wait behind every city's maintenance again
  // between purchases. Run the real construction coroutine, not a counter.
  (c,f,d)=Setup();for(int i=0;i<4;i++)d.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=d});Plan(c,f);
  Call("OpenConstructionVisit",c,f,c.LMPMNLLGOEC.IPJHOOJBCKI);
  var commitQueue=new WorkQueue();int upkeep133=0,plan133=0,battle133=0,recruit133=0,recovery133=0,world133=0;
  for(int i=0;i<160;i++){
   commitQueue.Add("economy:maintenance:city"+i,()=>Busy(()=>upkeep133++));
   commitQueue.Add("city:world"+i,()=>Busy(()=>world133++));
  }
  commitQueue.Add("layout:other",()=>Busy(()=>plan133++));
  commitQueue.Add("battle-support-plan:busy",()=>Busy(()=>battle133++));
  commitQueue.Add("recruit-queue:busy",()=>Busy(()=>recruit133++));
  commitQueue.Add("campaign:busy",()=>Busy(()=>recovery133++));
  commitQueue.Add("economy:construction:ready",()=>(IEnumerator<string>)Call("CityConstructionWork",c,f,c.LMPMNLLGOEC.IPJHOOJBCKI));
  int commitQuanta=0;while(d.AIMLPNBOIIF.Count<8&&commitQuanta<600){commitQueue.Pump(10000,1,null,e=>throw e);commitQuanta++;}
  Check(d.AIMLPNBOIIF.Count==8,"ready eight-building plan executes within 600 slices despite 320 unrelated permanent jobs");
  Check(upkeep133>0&&plan133>0&&battle133>0&&recruit133>0&&recovery133>0&&world133>0,"ready construction preserves planning maintenance defense recruitment recovery and world work");
  Console.WriteLine($"CityPlanning133 real eight-building execution in {commitQuanta} queue steps with 320 unrelated permanent jobs");commitQueue.Clear();
  (c,f,d)=Setup();var market=AEHAGJAKNEK.ACIGFKMDOKE["market"];
  using(var scan=((IEnumerable<bool>)Call("BuildScanSteps",c,f,true)).GetEnumerator()){
   Check(scan.MoveNext(),"research invalidation fixture yields");market.TechAllowed=false;while(scan.MoveNext()){}
  }
  var plans=Field<System.Collections.IDictionary>(typeof(StrategicSupport),"layoutIntents");
  Check(!plans.Contains(c.OJPECMNINNA),"interrupted solver cannot publish a cached no-build result");
  (c,f,d)=Setup();for(int i=0;i<5;i++)c.HFLAGGEFBOO.Add(new(){DMNMNBGHNKM=i,Complete=true});Plan(c,f);
  Call("OpenConstructionVisit",c,f,c.LMPMNLLGOEC.IPJHOOJBCKI);Drain((IEnumerator<string>)Call("CityConstructionWork",c,f,c.LMPMNLLGOEC.IPJHOOJBCKI));
  Check(d.AIMLPNBOIIF.Count==4,"all five city goals completed and no upgrades still fills four safe vacant plots");
  // Existing full-city goal rebuild path must still run while ordinary vacant
  // construction has no possible choice.
  typeof(RenewalTests).GetMethod("FullGrid",F).Invoke(null,null);c=Field<AFAGFHJONFJ>(typeof(RenewalTests),"c");f=Field<MKMOJNAJKJE>(typeof(RenewalTests),"f");d=Field<JNHOOCJJPJO>(typeof(RenewalTests),"d");
  var prior=d.AIMLPNBOIIF.Select(x=>x.Pointer).ToHashSet();
  Drain((IEnumerator<string>)Call("CityRenewalWork",c,f,c.LMPMNLLGOEC.IPJHOOJBCKI));
  Check(d.AIMLPNBOIIF.Any(x=>!prior.Contains(x.Pointer))&&prior.Any(p=>!d.AIMLPNBOIIF.Any(x=>x.Pointer==p)),"goal redevelopment removes blocking low-value buildings and starts replacement");
  Call("ResetWork");Console.WriteLine($"PASS CityPlanning128 {checks} production cached execution, ownership/research/expansion/occupancy invalidation, independent research, queue fairness and goal demolition checks. Native APIs mocked; not gameplay.");
 }
}
