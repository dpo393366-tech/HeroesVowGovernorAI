using System;using System.Collections.Generic;using System.Linq;using System.Reflection;using System.Diagnostics;
public static class Layout95Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string reason){checks++;if(!ok)throw new Exception("Layout95: "+reason);}
 static BuildingEffectsPolicy.Building B(string id,int x,int y=0)=>new(){Id=id,District="D",Geometry=true,X0=x,X1=x,Y0=y,Y1=y};
 public static void Run(){
  var market=B("market",0);market.Money=100;market.Kind=6;var a=B("pub",1);a.AdjMoney=.25;var b=B("pub",-1);b.AdjMoney=.25;
  Check(BuildingEffectsPolicy.Evaluate(new[]{market,a,b}).Money==125,"same noncumulative definition applies once per recipient");b.Id="other-pub";Check(BuildingEffectsPolicy.Evaluate(new[]{market,a,b}).Money==150,"distinct IDs stack under actual native dedup rule");b.Id="pub";a.StackMoney=b.StackMoney=true;Check(BuildingEffectsPolicy.Evaluate(new[]{market,a,b}).Money==150,"cumulative copies all apply");
  market.Kind=7;Check(BuildingEffectsPolicy.Evaluate(new[]{market,a,b}).Money==100,"money adjacency does not boost an ineligible recipient type");market.Kind=61;Check(BuildingEffectsPolicy.Evaluate(new[]{market,a,b}).Money==150,"BigMarket is eligible");
  a=B("block",10);b=B("block",20);a.BlockMoney=b.BlockMoney=.5;Check(BuildingEffectsPolicy.Evaluate(new[]{market,a,b}).Money==225,"district multipliers compound even without adjacency");market.Money=3;Check(BuildingEffectsPolicy.Evaluate(new[]{market,a,b}).Money==6,"native truncation occurs after each increment");market.Money=100;
  Check(!BuildingEffectsPolicy.Adjacent(market,B("diagonal",1,1))&&BuildingEffectsPolicy.Adjacent(market,B("cardinal",1)),"diagonal contact excluded; edge contact included");
  var farm=B("farm",0);farm.Food=100;farm.Kind=99;a=B("water",1);a.AdjFood=.5;Check(BuildingEffectsPolicy.Evaluate(new[]{farm,a}).Food==150,"food adjacency is not incorrectly restricted to market types");a.AdjMoney=.5;Check(BuildingEffectsPolicy.Evaluate(new[]{farm,a}).Food==100,"dual-channel source follows native nonmarket skip");market.Food=100;market.Kind=6;Check(BuildingEffectsPolicy.Evaluate(new[]{market,a}).Food==100,"shared native dedup set prevents fictional second channel");a.StackFood=true;Check(BuildingEffectsPolicy.Evaluate(new[]{market,a}).Food==150,"explicit food cumulation bypasses shared dedup");
  var actions=new List<DevelopmentPolicy.Action>();for(int x=0;x<3;x++){
   var m=B("market",x);m.Kind=6;m.Money=100;var bonus=B("bazaar",x);bonus.AdjMoney=1;
   actions.Add(new(){Id="market",District="D",Tiles=new[]{(long)x},Effects=m,Limit=2});actions.Add(new(){Id="bazaar",District="D",Tiles=new[]{(long)x},Effects=bonus,Limit=1});
  }
  List<int> plan=null;foreach(var step in CityLayoutOptimizer.Solve(actions,new(),r=>plan=r)){}
  var selected=plan.Select(i=>actions[i].Effects).ToList();Check(plan.Count==3&&BuildingEffectsPolicy.Evaluate(selected).Money==400,"future-to-future adjacency places effect building between two planned markets");
  double exact=0;for(int i=0;i<actions.Count;i++)for(int j=i+1;j<actions.Count;j++)for(int k=j+1;k<actions.Count;k++){var pack=new[]{actions[i],actions[j],actions[k]};if(pack.SelectMany(x=>x.Tiles).Distinct().Count()!=3||pack.Count(x=>x.Id=="market")>2||pack.Count(x=>x.Id=="bazaar")>1)continue;exact=Math.Max(exact,BuildingEffectsPolicy.Evaluate(pack.Select(x=>x.Effects).ToList()).Money);}Check(BuildingEffectsPolicy.Evaluate(selected).Money==exact,"small-map result matches independent exhaustive enumeration");
  typeof(CityIntegrationTests).GetMethod("Setup",F).Invoke(null,null);var city=(AFAGFHJONFJ)typeof(CityIntegrationTests).GetField("c",F).GetValue(null);var f=NEINKOOBJIH.COCMABJFOFH["F"];AEHAGJAKNEK.ACIGFKMDOKE.Clear();
  var school=new PABIPDOFBFG{OJPECMNINNA="school95",CNCPIILGDEF=0,IIBKLLKPNEH=5};AEHAGJAKNEK.ACIGFKMDOKE[school.OJPECMNINNA]=school;
  Check((bool)Call("BuildBest",city,f)&&city.FGBLEMGGOPO.Any(d=>d.AIMLPNBOIIF.Any(x=>x.GEKKLMPLDHJ==school)),"zero-cash tech building reaches real production construction path");
  var upgrade=new PABIPDOFBFG{OJPECMNINNA="school95-up",MDOIJIAONJM="school95",CNCPIILGDEF=0,IIBKLLKPNEH=8};var district=city.FGBLEMGGOPO.First(d=>d.AIMLPNBOIIF.Any(x=>x.GEKKLMPLDHJ==school));var instance=district.AIMLPNBOIIF.First(x=>x.GEKKLMPLDHJ==school);
  Check((double)Call("LocationValue",district,upgrade,instance,null)>(double)Call("LocationValue",district,school,instance,null),"tech-only upgrade has positive marginal utility");
  upgrade.IIBKLLKPNEH=0;Check(!(bool)Call("PreservesOutputs",school,upgrade),"upgrade cannot silently erase research income");
  var old=Call("ReadLayoutRevision",city,f);school.IIBKLLKPNEH=7;var fresh=Call("ReadLayoutRevision",city,f);Check((string)old.GetType().GetMethod("Change").Invoke(old,new[]{fresh})=="research-or-building-caps","modified effect definition invalidates saved layout");
  city.PopulationFlow90=1;city.LMPMNLLGOEC.PNHICLAGHGD=10000;
  var harmful=new PABIPDOFBFG{OJPECMNINNA="harmful95",CNCPIILGDEF=50000,DHLNIKKLNKJ=-100000};Check((string)Call("CivicBuildBlock",city,harmful)=="population-net-decrease","population loss is guarded even when cash benefit is huge");
  SettingCtrl.m_isZhengShi=true;Check((double)Call("PopulationGain",city,0d,1000d)==0,"classic mode ignores populationAdd exactly as native formula");SettingCtrl.m_isZhengShi=false;
  city.LMPMNLLGOEC.PNHICLAGHGD=EBNFFALGBPA.LNBFFMONMCN[city.MPGFDBPEHBO(true)];city.PopulationFlow90=0;Check((double)Call("PopulationGain",city,1000d,1000d)==0,"population cap zero is not mistaken for building failure");
  var tech=B("tech",0);tech.Tech=5;var strategy=B("strategy",0);strategy.Strategy=5;var recruit=B("recruit",0);recruit.Recruit=10;
  Check(BuildingEffectsPolicy.Evaluate(new[]{tech}).Utility>0&&BuildingEffectsPolicy.Evaluate(new[]{strategy}).Utility>0&&BuildingEffectsPolicy.Evaluate(new[]{recruit}).Utility>0,"noncash research, strategy and recruitment contribute explicit utility");
  var special=B("special",0);special.Special=5;var duplicate=B("special2",1);duplicate.Special=5;Check(BuildingEffectsPolicy.Evaluate(new[]{special,duplicate}).Utility==100,"special facility unlock not counted twice");
  // Larger deterministic candidate catalogue measures managed solver only.
  var load=new List<DevelopmentPolicy.Action>();for(int x=0;x<12;x++)for(int kind=0;kind<6;kind++){var e=B("def"+kind,x);e.Money=kind*20;e.Tech=kind==0?3:0;e.Kind=6;load.Add(new(){Id=e.Id,District="D",Effects=e,Tiles=new[]{(long)x},Limit=6,Cost=100});}
  double maximum=0;int yields=0;var timer=Stopwatch.StartNew();var iterator=CityLayoutOptimizer.Solve(load,new(),r=>plan=r).GetEnumerator();while(true){var tick=Stopwatch.StartNew();bool more=iterator.MoveNext();maximum=Math.Max(maximum,tick.Elapsed.TotalMilliseconds);if(!more)break;yields++;}
  Check(plan.Count==12&&yields>1,"larger all-candidate layout fills legal cells and yields cooperatively");Console.WriteLine($"LAYOUT95 managed benchmark candidates={load.Count} steps={yields} totalMs={timer.Elapsed.TotalMilliseconds:F2} maxStepMs={maximum:F2}; not gameplay FPS");
  Console.WriteLine($"PASS Layout95 {checks} native-derived effects, utility candidates, population, upgrade, revision and exact small-map checks; native calls mocked.");
 }
}
