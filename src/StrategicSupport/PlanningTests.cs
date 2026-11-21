using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using A=DevelopmentPolicy.Action;
public static class PlanningTests
{
 static int tests;const BindingFlags Flags=BindingFlags.Static|BindingFlags.NonPublic;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,Flags).Invoke(null,a);
 static void Set(string n,object v)=>typeof(StrategicSupport).GetField(n,Flags).SetValue(null,v);
 static object Get(string n)=>typeof(StrategicSupport).GetField(n,Flags).GetValue(null);
 static void Check(bool v,string why){tests++;if(!v)throw new Exception("Planning: "+why);}
 static AFAGFHJONFJ c;static MKMOJNAJKJE f;static FNFJODHIJFO g;
 static void Setup(){
  typeof(CityIntegrationTests).GetMethod("Setup",Flags).Invoke(null,null);
  c=(AFAGFHJONFJ)typeof(CityIntegrationTests).GetField("c",Flags).GetValue(null);
  f=(MKMOJNAJKJE)typeof(CityIntegrationTests).GetField("f",Flags).GetValue(null);g=f.ANMMMOIKLEG["gov"];
  AEHAGJAKNEK.HMHMMONMBAP.Clear();Set("researchOwner",f);Set("researchGovernor",g);Set("researchTeam",null);c.HomeOfficerCount=14;f.DNOGPFHMOMO=60000;
 }
 static FBLFDEEGJMH Tech(string id,int cost=10000,int people=1){var t=new FBLFDEEGJMH{GPEOLOAMMHP=id,Cost=cost};f.OINKOPJJGJH[id]=t;AEHAGJAKNEK.HMHMMONMBAP[id]=new(){DAJIHBICKBE=new[]{people}};return t;}
 static FBLFDEEGJMH Pick(params FBLFDEEGJMH[] list){var items=new Il2CppSystem.Collections.Generic.List<FBLFDEEGJMH>();items.AddRange(list);object[] a={f,items,null};Check(!(bool)Call("SelectResearch",a),"automatic selection replaces vanilla ranking");return (FBLFDEEGJMH)a[2];}
 static Dictionary<string,DevelopmentPolicy.Need> Needs(object plan)=>(Dictionary<string,DevelopmentPolicy.Need>)plan.GetType().GetField("Needs").GetValue(plan);
 static object N(params (string,object)[] values){var t=typeof(StrategicSupport).GetNestedType("ResearchNeeds",BindingFlags.NonPublic);var n=Activator.CreateInstance(t,true);foreach(var (key,v) in values)t.GetField(key).SetValue(n,v);return n;}
 static double Urgency(string id,object needs)=>(double)Call("ResearchUrgency",id,needs);
 static A Action(string id,long tile,double value=100)=>new(){Id=id,District="d",Tiles=new[]{tile},Cost=100,BaseValue=value,Priority=1,Zone=1};
 public static void Run(){
  var empty=new Dictionary<string,DevelopmentPolicy.Need>();
  var actions=Enumerable.Range(0,20).Select(i=>Action("m"+i,i+1)).ToList();
  actions.Add(new(){Id="bonus",District="d",Tiles=new long[]{99},Cost=100,MoneyBonus=10,Priority=1,Zone=1});
  foreach(var a in actions.Take(20))a.Money=100;
  var steps=DevelopmentPolicy.Plan(actions,empty,300,0,0);
  Check(steps.Count>=2&&steps.Any(i=>actions[i].Id=="bonus"),"effect building survives crowded shortlist and pays back with producers");
  actions=new(){Action("a",1),Action("b",1,10000)};steps=DevelopmentPolicy.Plan(actions,empty,300,0,0);Check(steps.Count==1,"overlapping sites cannot both be reserved");
  actions=new(){Action("market",1),Action("farm",2)};actions[1].Zone=2;actions[1].FoodBonus=100;actions[0].Food=100;
  steps=DevelopmentPolicy.Plan(actions,empty,300,0,0);Check(steps.Count==1,"future construction cannot mix district specializations");
  var need=new Dictionary<string,DevelopmentPolicy.Need>{{"security",new(){Missing=5,Target=100}}};var a1=Action("a",1);a1.Gains["security"]=100;
  Check(DevelopmentPolicy.GoalGain(need,a1,new[]{a1})==0,"completed projected goal gives no additional incentive");
  Check(Math.Abs(DevelopmentPolicy.GoalGain(need,a1,Array.Empty<A>())-52.5)<.001,"goal credit capped at remaining five points");
  var rng=new Random(154);
  for(int trial=0;trial<1000;trial++){
   actions=new();for(int i=0;i<15;i++){var a=Action("b"+rng.Next(4),rng.Next(8),rng.Next(1000));a.District="d"+rng.Next(2);a.Cost=rng.Next(200);a.Tech=rng.Next(4);a.Strategy=rng.Next(3);a.Limit=2;a.Zone=rng.Next(3);a.Priority=rng.Next(1,4);actions.Add(a);}
   int cash=rng.Next(400),tech=rng.Next(8),strategy=rng.Next(6);steps=DevelopmentPolicy.Plan(actions,empty,cash,tech,strategy);var chosen=steps.Select(i=>actions[i]).ToList();
   Check(chosen.Count<=6&&chosen.Sum(a=>a.Cost)<=cash&&chosen.Sum(a=>a.Tech)<=tech&&chosen.Sum(a=>a.Strategy)<=strategy,"random plan stays within all budgets");
   Check(chosen.Count==0||chosen[0].Priority==actions.Max(a=>a.Priority),"urgent first step preserved");
   Check(chosen.GroupBy(a=>a.Id).All(group=>group.Count()<=2),"building cap preserved");
   for(int i=0;i<chosen.Count;i++)for(int j=i+1;j<chosen.Count;j++)Check(!DevelopmentPolicy.Overlap(chosen[i],chosen[j])&&(chosen[i].District!=chosen[j].District||chosen[i].Zone==0||chosen[j].Zone==0||chosen[i].Zone==chosen[j].Zone),"random placements do not overlap/mix");
  }
  Setup();c.Security=100;c.Popular=100;c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.CitySecurity,KLCDGPDHENB=100},new(){HFBAJIGHOAF=DDBHJKOIIPI.CityPopularFeel,KLCDGPDHENB=100},new(){HFBAJIGHOAF=DDBHJKOIIPI.CityMoneyIncome,KLCDGPDHENB=1000}}});
  var needs=Needs(Call("ReadGoals",c));Check(!needs.ContainsKey("security")&&!needs.ContainsKey("popular")&&needs.ContainsKey("money"),"satisfied subconditions removed while incomplete goal remains");
  c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE="pending",KLCDGPDHENB=1}}});
  c.FGBLEMGGOPO[0].AIMLPNBOIIF.Add(new(){GEKKLMPLDHJ=new(){OJPECMNINNA="pending"},EMHNNCLBADE=0});Check(!Needs(Call("ReadGoals",c)).ContainsKey("building:pending"),"registered unfinished building prevents duplicate goal purchases");
  Setup();FBLFDEEGJMH cheap=Tech("17",30000),expensive=Tech("21",30001);c.LMPMNLLGOEC.OEGNGFBCHFL=0;
  Check(Pick(expensive,cheap)==cheap,"60000 treasury permits exactly 30000, rejects 30001 even in food crisis");
  f.DNOGPFHMOMO-=cheap.Cost;Check(Pick(cheap,expensive)==null&&f.DNOGPFHMOMO==30000,"sequential decision cannot spend reserve");
  Setup();FBLFDEEGJMH food=Tech("21"),money=Tech("17"),granary=Tech("28");c.LMPMNLLGOEC.OEGNGFBCHFL=0;
  Check(Pick(money,granary,food)==food,"food crisis chooses actual food production, not granary morale");
  c.LMPMNLLGOEC.OEGNGFBCHFL=10000000;Check(Pick(food,money)==money,"priority changes after food crisis ends");
  Setup();food=Tech("21");money=Tech("17");c.LMPMNLLGOEC.OEGNGFBCHFL=0;AEHAGJAKNEK.HMHMMONMBAP["21"].MinAbility=90;
  Check(Pick(food,money)==money,"unqualified urgent research does not stall affordable staffed work");
  c.HomeOfficerCount=1;Check(Pick(money)==null,"single idle officer city excluded just as native routine");
  c.HomeOfficerCount=2;AEHAGJAKNEK.HMHMMONMBAP["17"].DAJIHBICKBE[0]=3;Check(Pick(money)==null,"cannot start with fewer qualified officers than required");
  c.HomeOfficerCount=5;c.LMPMNLLGOEC.BKENJPNKMGI="player";Check(Pick(money)==null,"native false-automation researcher gate respected");
  Setup();food=Tech("21");money=Tech("17");food.ABJNKABKKJM.Add(new());Check(Pick(food,money)==money,"does not reassign running research");food.ABJNKABKKJM.Clear();food.FDNECLDGCHN=10000;Check(Pick(food,money)==money,"does not repeat completed research");
  Setup();FBLFDEEGJMH root=Tech("mod-prereq"),target=Tech("21"),other=Tech("17");AEHAGJAKNEK.HMHMMONMBAP["21"].ANNMPIPJGNC.Add("mod-prereq");c.LMPMNLLGOEC.OEGNGFBCHFL=0;
  Check(Pick(other,root)==root,"necessary prerequisite inherits emergency need without injecting locked descendant");
  target.FDNECLDGCHN=10000;Check(Pick(root,other)==other,"completed descendant no longer inflates prerequisite");
  target.FDNECLDGCHN=0;AEHAGJAKNEK.HMHMMONMBAP["mod-prereq"].ANNMPIPJGNC.Add("21");Check(Pick(other,root)==root,"cyclic mod prerequisites terminate safely");
  Setup();FBLFDEEGJMH one=Tech("17",10000),two=Tech("19",20000);Check(Pick(two,one)==one,"equal need prefers cheaper research");
  var n=N(("SecurityGoal",1d));Check(Urgency("32",n)>Urgency("27",n)&&Urgency("32",n)>Urgency("30",n),"security goals do not misrank tower count or population");
  n=N(("Food",true),("FoodBase",10000d));Check(Urgency("21",n)>Urgency("36",n)&&Urgency("21",n)>Urgency("28",n),"existing farms favor direct yield before indirect unlocks");
  n=N(("Front",true),("Troops",new double[]{0,0,10000,0,0}));Check(Urgency("7",n)>Urgency("1",n)&&Urgency("51",n)>Urgency("41",n),"military need follows existing troop roles including elite cavalry");
  Setup();var unlock=Tech("35");money=Tech("17");var locked=new PABIPDOFBFG{OJPECMNINNA="JZ10007",TechAllowed=false};AEHAGJAKNEK.ACIGFKMDOKE[locked.OJPECMNINNA]=locked;
  c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE=locked.OJPECMNINNA,KLCDGPDHENB=1}}});Check(Pick(money,unlock)==unlock,"research unlocks building required by actual unmet city goal");
  Setup();Set("researchOwner",null);Set("researchGovernor",null);object[] scope={g,null};Call("BeginResearch",scope);Check(Get("researchOwner")==f&&Get("researchGovernor")==g,"ordinary native governor research enters priority/reserve scope");
  object[] nested={g,null};Call("BeginResearch",nested);Call("EndResearch",nested[1]);Check(Get("researchOwner")==f,"nested native scope restores outer faction");
  Call("EndResearch",scope[1]);Check(Get("researchOwner")==null&&Get("researchGovernor")==null,"research context restored after native call");
  // Actual live failure: 75 faction officers all on civil duties; 62 Explore.
  Setup();c.Roster=new();for(int i=0;i<75;i++)c.Roster.Add(new(){OJPECMNINNA="worker"+i,LMPMNLLGOEC=new(){LMMNKIOAGLC="F",CNOMNBCPJDI=c.OJPECMNINNA},MGLFFEANDOI=true,BAPIJIJLLCA=new(){HFBAJIGHOAF=i<62?OEBGIDGEIIG.Explore:OEBGIDGEIIG.Military}});
  var original=c.Roster.ToArray();var nativeCandidates=new[]{Tech("17"),Tech("21"),Tech("34")};
  Set("researchOwner",null);Check(c.EOMOIINOEMB(1,false,false,false,false,false,false).Count==0,"recorded all-employed fixture reproduces native empty list");Set("researchOwner",f);
  var selected=Pick(nativeCandidates);Check(selected!=null,"all-employed governor can staff research from noncritical civil jobs");
  var nativePool=c.EOMOIINOEMB(1,false,false,false,false,false,false);Check(nativePool.Count>1&&nativePool.All(h=>h.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Explore),"native start receives a valid multi-entry pool with no military workers");
  Check(c.Roster.All(h=>h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.None),"selection alone does not mutate jobs or pay research");
  var worker=nativePool.First(h=>AEHAGJAKNEK.HMHMMONMBAP[selected.GPEOLOAMMHP].OIFAMCDJHFP(h,0));worker.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;selected.ABJNKABKKJM.Add(worker);f.DNOGPFHMOMO-=selected.Cost;
  Check(f.DNOGPFHMOMO==50000&&c.Roster.Count(h=>h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.Technology)==1,"simulated native payment/job transition consumes one researcher and exact cost");
  Check(Pick(nativeCandidates)!=selected,"subsequent research never steals an already assigned researcher");
  foreach(var h in c.Roster.Take(3))h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;Check((int)Call("ResearchCitySlots",c)==47,"75 home officers reserve 25 and subtract 3 existing researchers without fixed cap");
  foreach(var h in c.Roster){h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.None;h.BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Military;}
  Check(Pick(nativeCandidates)!=null,"surplus military assignments no longer blacklist every qualified researcher");
  c.Roster=c.Roster.Take((int)Call("ResearchMilitaryKeep",c)).ToList();Set("researchTeam",null);
  Check(c.EOMOIINOEMB(1,false,true,false,false,false,false).Count==0,"actual minimum military staff remains protected");
  Setup();var forbidden=new PABIPDOFBFG{OJPECMNINNA="JZ100258kn0",OCJNCECECMH=FFDDNPOHLHN.Private,CNCPIILGDEF=999999};AEHAGJAKNEK.ACIGFKMDOKE[forbidden.OJPECMNINNA]=forbidden;
  Check((string)Call("BuildBlock",c,f,forbidden,10000,false)=="private-estate-only"&&!(bool)Call("BuildBest",c,f),"private-only mod buildings cannot be purchased for public districts regardless of huge benefit");
  Setup();f.DNOGPFHMOMO=415349;c.Roster=new();for(int i=0;i<68;i++)c.Roster.Add(new(){OJPECMNINNA="rich-worker"+i,LMPMNLLGOEC=new(){LMMNKIOAGLC="F",CNOMNBCPJDI=c.OJPECMNINNA}});
  foreach(var h in c.Roster.Take(2))h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;
  Check((int)Call("ResearchCitySlots",c)==43,"recorded 66 idle plus 2 researchers leaves 23 local reserve and 43 additional slots");
  var richTech=Tech("17",10000);AEHAGJAKNEK.HMHMMONMBAP["17"].DAJIHBICKBE[0]=3;
  Check(Pick(richTech)==richTech,"wealthy recorded fixture can staff a three-officer project while two already researching");
  c.Roster=c.Roster.Take(5).ToList();Check((int)Call("ResearchCitySlots",c)==0,"small city with two active researchers retains local reserve");
  Console.WriteLine($"PASS {tests} new planning/research checks (1000 randomized layouts); native-call stubs, not gameplay");
 }
}

