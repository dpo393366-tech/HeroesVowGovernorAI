using System;using System.Linq;using System.Collections;using System.Collections.Generic;using System.Reflection;using UnityEngine;
public static class Audit92Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int count;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool ok,string why){count++;if(!ok)throw new Exception("R92: "+why);}
 static object Fixture(Type t,string name,params object[] a)=>t.GetMethod(name,F).Invoke(null,a);
 static void Drain(string n,params object[] a){foreach(var step in (IEnumerable<string>)Call(n,a)){} }
 static IEnumerable<string> Busy(){while(true)yield return "unrelated-strategy-scan";}
 public static void Run(){
  Fixture(typeof(City80Tests),"Setup",new object[]{new[]{21,9,5}});
  var cities=MICGPAMNDKM.IJPKEMPIFIH.Values.ToList();var faction=NEINKOOBJIH.COCMABJFOFH["F"];var source=cities[0];
  foreach(var h in source.Roster.Take(11))h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.PrivateMilitary;
  foreach(var h in source.Roster.Skip(11))h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;
  Drain("AdministrativeStaffingSteps",faction);
  Check(source.Roster.Any(h=>h.MoveCalls>0),"21/9/5 imbalance can move private trainers even when native idle pool is empty");
  Check(source.Roster.Skip(11).All(h=>h.MoveCalls==0),"researchers counted locally but never exported");
  var before=source.Roster.Sum(h=>h.MoveCalls);Drain("AdministrativeStaffingSteps",faction);Check(source.Roster.Sum(h=>h.MoveCalls)==before,"native incoming orders prevent duplicate batch transfers");
  Fixture(typeof(StatecraftTests),"Setup");faction=NEINKOOBJIH.COCMABJFOFH["F"];cities=faction.PJMLPLDPEIM().ToArray().ToList();
  var lower=new JFLCKHMFGPJ{OJPECMNINNA="explore20",PMPLECHDMNM=23,IABANLBEFMG=0,EHPDLGLPOBD=0,CCBIHJLDGCG=10};lower.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)8,HLNCKLGGECA=20,LPIEHGBCPLP=1});
  var higher=new JFLCKHMFGPJ{OJPECMNINNA="explore30",PMPLECHDMNM=23,IABANLBEFMG=0,EHPDLGLPOBD=0,CCBIHJLDGCG=20};higher.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)8,HLNCKLGGECA=30,LPIEHGBCPLP=1});
  faction.MEAMPLFJPNH.Add(new(){BHIBPLKOFKH=lower});faction.Strategies.Add(higher);int points=faction.PEGLDBENJMF;
  Check((bool)Call("TryStateStrategy",faction,cities)&&faction.MEAMPLFJPNH.Count==1&&faction.MEAMPLFJPNH[0].BHIBPLKOFKH==higher&&faction.PEGLDBENJMF==points-higher.AFPGDLJKEDA,"stronger permanent same-family strategy replaces weaker one and pays exactly once");
  Check(!(bool)Call("StrategyDominates",lower,higher),"weaker option cannot replace stronger active strategy");higher.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)11,HLNCKLGGECA=-5});Check(!(bool)Call("StrategyDominates",higher,lower),"upgrade cannot silently introduce research penalty");
  var research=new JFLCKHMFGPJ{OJPECMNINNA="research35",PMPLECHDMNM=10,EHPDLGLPOBD=300};faction.ADJFGBAIACI[research.OJPECMNINNA]=79;
  Check((string)Call("StrategyBlock",faction,research)=="native-cooldown:79","real observed research cooldown explicitly reported and preserved");
  Fixture(typeof(GovernorFeatureTests),"Setup");faction=NEINKOOBJIH.COCMABJFOFH["F"];cities=faction.PJMLPLDPEIM().ToArray().ToList();
  var prisoner=(CLAGKDECFID)Fixture(typeof(GovernorFeatureTests),"Candidate","woman",KLMDJNIBCLD.Female,90,true);
  var queue=(WorkQueue)typeof(StrategicSupport).GetField("work",F).GetValue(null);queue.Clear();queue.Add("faction:F",()=>Busy().GetEnumerator());
  StrategicSupport.StartRecruitmentQueue(30,2);for(int i=0;i<1500&&!cities.SelectMany(c=>c.Roster).Any(h=>h.OGPEJIEHDAN.LKEPGHJDHGG==prisoner.OJPECMNINNA);i++)queue.Pump(10,128,(k,t)=>{},e=>throw e);
  Check(cities.SelectMany(c=>c.Roster).Any(h=>h.OGPEJIEHDAN.LKEPGHJDHGG==prisoner.OJPECMNINNA),"female30 queue starts while unrelated faction strategy coroutine never ends");queue.Clear();
  Fixture(typeof(Relief86Tests),"Setup");faction=NEINKOOBJIH.COCMABJFOFH["F"];var pathCities=new List<AFAGFHJONFJ>();
  for(int i=0;i<8;i++){var c=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","route"+i,(float)i*10);pathCities.Add(c);if(i>0)Fixture(typeof(Relief86Tests),"Link",pathCities[i-1],c);}
  var bad=new AFAGFHJONFJ{OJPECMNINNA="blocked-city"};bad.LMPMNLLGOEC.LMMNKIOAGLC="third";
  pathCities[0].MEKIDAONKMD[pathCities[7].OJPECMNINNA]=new(){EJMDIPFNGMG=new(){bad}};
  AOHIBKLOAEI.RegionAt=v=>pathCities[Math.Clamp((int)Math.Round(v.x/10),0,7)];
  object[] route={pathCities[0].BMBMKPGEHBH,pathCities[7].BMBMKPGEHBH,faction,2f,0d,null};
  Check((bool)Call("RouteSeconds",route)&&Math.Abs((double)route[4]-35)<.01,"seven-edge friendly alternative not truncated by old four-city limit");
  pathCities[4].LMPMNLLGOEC.LMMNKIOAGLC="third";Check(!(bool)Call("RouteSeconds",route),"unpermitted real intermediate remains blocked");AOHIBKLOAEI.RegionAt=null;
  Partition();CaptiveMutation();BuildGoal();Call("ResetWork");Call("ResetBattleState");Call("ResetDefense");
  Console.WriteLine($"PASS R92 {count} private-trainer transfers, independent recruitment/diplomacy, strategy upgrade, native topology and distributed return checks; native actions mocked.");
 }
 static void Partition(){
  Fixture(typeof(Relief86Tests),"Setup");ICGAEMLACJG.HFJIECIBFKB.Clear();MICGPAMNDKM.IJPKEMPIFIH.Clear();ICGAEMLACJG.FKKCPDEFODO.Clear();
  var f=NEINKOOBJIH.COCMABJFOFH["F"];var cities=new List<AFAGFHJONFJ>();
  for(int i=0;i<4;i++){var c=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","return"+i,(float)i*10);c.LMPMNLLGOEC.KJPPGIOCDID.Clear();c.Capacity=2;cities.Add(c);if(i>0)Fixture(typeof(Relief86Tests),"Link",cities[i-1],c);}
  var army=new Wo_DeployTeamCtrl{m_id=9200,m_belongPower="F",m_belongGroup="gov",m_assembleCity=cities[0].OJPECMNINNA,Food=90000,Combat=6000,m_deployState=AKOPKFPKJPP.Camp,Position=new(0)};
  for(int i=0;i<6;i++){var h=cities[0].Roster[i];army.m_listHero.Add(h);army.m_dicDeployArmy[h.OJPECMNINNA]=new(){new DHKINGPDKJJ{FGHFOLKBHDL=1000,CombatValue=1000}};}
  ICGAEMLACJG.HFJIECIBFKB[army.m_id]=army;AOHIBKLOAEI.RegionAt=v=>cities[Math.Clamp((int)Math.Round(v.x/10),0,3)];Call("RememberCampaign",army);
  var goals=(IDictionary)typeof(StrategicSupport).GetField("campaignGoals",F).GetValue(null);var goal=goals[army.Pointer];goal.GetType().GetField("Returning").SetValue(goal,true);
  Drain("ReturnPartitionSteps",army,goal,f);
  var teams=ICGAEMLACJG.HFJIECIBFKB.Values.Where(t=>t.m_belongPower=="F").ToList();
  Check(teams.Sum(t=>t.GetSoldierNum(false))==6000&&teams.Sum(t=>t.GetFood())==90000&&teams.Sum(t=>t.m_listHero.Count)==6,"partition preserves every officer, soldier and ration");
  Check(teams.Count(t=>t.m_isEnterCity)>=2&&teams.Where(t=>t.m_isEnterCity).Select(t=>t.m_targetCity).Distinct().Count()>=2,"army larger than every single city divides into multiple real native return orders");
  foreach(var c in cities)Check(teams.Where(t=>t.m_targetCity==c.OJPECMNINNA).Sum(t=>((HashSet<IntPtr>)Call("TeamTroops",t)).Count)<=c.Capacity,"incoming detachments cannot double-book destination capacity");
  AOHIBKLOAEI.RegionAt=null;
 }
 static void CaptiveMutation(){
  Fixture(typeof(StatecraftTests),"Setup");var f=NEINKOOBJIH.COCMABJFOFH["F"];var c=f.PJMLPLDPEIM()[0];var captor=new MKMOJNAJKJE{IBGCLCAIBOL="captor"};NEINKOOBJIH.COCMABJFOFH["captor"]=captor;
  AEHAGJAKNEK.IEOHLJLICOA["captor"]=new(){OJPECMNINNA="captor",LMPMNLLGOEC=new(){CNOMNBCPJDI="foreign",LMMNKIOAGLC="captor"}};
  captor.Captives90.Add(new(){OJPECMNINNA="prisoner",LMPMNLLGOEC=new(){LMMNKIOAGLC="F"}});
  var pool=c.Roster.Select(h=>(c,h)).ToList();var scan=((IEnumerable<string>)Call("CaptiveDiplomacySteps",f,pool,new List<CLAGKDECFID>())).GetEnumerator();
  while(scan.MoveNext())if(scan.Current=="ransom-captive")captor.Captives90.Clear();scan.Dispose();Check(c.Roster.All(h=>h.MissionCalls==0),"captive list changing during yielded assessment neither throws nor sends stale release request");
  captor.Captives90.Add(new(){OJPECMNINNA="prisoner2",LMPMNLLGOEC=new(){LMMNKIOAGLC="F"}});
  Call("ResetWork");var queue=(WorkQueue)typeof(StrategicSupport).GetField("work",F).GetValue(null);queue.Add("faction:F",()=>Busy().GetEnumerator());Call("ScheduleDiplomaticWork",f);
  for(int i=0;i<1000&&!c.Roster.Any(h=>h.Proposal==IPNKPODNAIN.ReturnCaptive);i++)queue.Pump(10,128,(k,t)=>{},e=>throw e);
  Check(c.Roster.Any(h=>h.Proposal==IPNKPODNAIN.ReturnCaptive&&h.AidCity=="prisoner2"),"release proposal runs independently of indefinitely busy strategy scan");queue.Clear();

 }
 static void BuildGoal(){
  var fixture=((OGABIJECHFG,OGABIJECHFG,PABIPDOFBFG))Fixture(typeof(RenewalTests),"FullGrid");
  var c=(AFAGFHJONFJ)typeof(RenewalTests).GetField("c",F).GetValue(null);var f=NEINKOOBJIH.COCMABJFOFH["F"];var d=(JNHOOCJJPJO)typeof(RenewalTests).GetField("d",F).GetValue(null);
  c.HFLAGGEFBOO.Add(new(){GoalCheck=city=>city.ELKFAJKBPKJ("small-a",false,true).Count>0});int before=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
  Fixture(typeof(RenewalTests),"Run");
  Check(d.AIMLPNBOIIF.Contains(fixture.Item1)&&d.AIMLPNBOIIF.Contains(fixture.Item2)&&before==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO,"native goal rejection restores originals and money without aborting city scheduler");
  Check(!(bool)Call("DayReady","rebuild-rejected:"+c.OJPECMNINNA+":"+fixture.Item3.OJPECMNINNA,3d,false),"rejected replacement is not repeated unchanged on the next day");
 }

}
