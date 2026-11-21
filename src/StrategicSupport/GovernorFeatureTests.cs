using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
public static class GovernorFeatureTests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Governor feature: "+why);}
 static MKMOJNAJKJE faction;static AFAGFHJONFJ city;
 static void Setup(){typeof(StatecraftTests).GetMethod("Setup",F).Invoke(null,null);faction=(MKMOJNAJKJE)typeof(StatecraftTests).GetField("f",F).GetValue(null);city=(AFAGFHJONFJ)typeof(StatecraftTests).GetField("c",F).GetValue(null);MLGOLGGHLMB.CBEGDDKANAO=new();UI_NotifyUI.Calls=0;NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC=faction.IBGCLCAIBOL;}
 static string Key=>"local.heroesvow.governor/recruit/v1/"+faction.IBGCLCAIBOL+"/";
 static int Q(string key)=>MLGOLGGHLMB.CBEGDDKANAO.TryGetValue(Key+key,out int v)?v:0;
 static void Recruit(){foreach(var phase in (IEnumerable<string>)Call("RecruitmentQueueSteps",faction)){} }
 static CLAGKDECFID Candidate(string id,KLMDJNIBCLD sex,int talent,bool captive=false){var h=new CLAGKDECFID{OJPECMNINNA=id,PMHBHENJPHA=sex,GMILGFFJOIL=captive,LMPMNLLGOEC=new(){LMMNKIOAGLC=captive?"enemy":"",CNOMNBCPJDI=city.OJPECMNINNA},Ability=Enumerable.Repeat((double)talent,14).ToArray()};AEHAGJAKNEK.IEOHLJLICOA[id]=h;if(captive)city.JHNBHLPJKMO.Add(h);return h;}
 static void Join(CLAGKDECFID h){h.LMPMNLLGOEC.LMMNKIOAGLC=faction.IBGCLCAIBOL;h.GMILGFFJOIL=false;foreach(var a in city.Roster)if(a.OGPEJIEHDAN.LKEPGHJDHGG==h.OJPECMNINNA)a.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.None;}
 static List<int> Solve(List<DevelopmentPolicy.Action> actions,Dictionary<string,DevelopmentPolicy.Need> needs,double p=1,double s=1){List<int> result=null;foreach(var step in CityLayoutOptimizer.Solve(actions,needs,x=>result=x,p,s)){}return result;}
 public static void Run(){
  var cities=new List<OfficerDeploymentPolicy.City>{new(){Id="front",Tier=3,Minimum=4},new(){Id="middle",Tier=2,Minimum=3},new(){Id="rear",Tier=1,Minimum=2}};
  for(int total=0;total<300;total++){var quota=OfficerDeploymentPolicy.Quotas(cities,total);Check(quota.Values.Sum()==total,"officer quota conserves every officer under scarcity and abundance");if(total>=30)Check(quota["front"]>=quota["middle"]&&quota["middle"]>=quota["rear"],"border/middle/rear demand weighting");}
  Setup();var male=Candidate("male",KLMDJNIBCLD.Male,99);var female=Candidate("female",KLMDJNIBCLD.Female,70);var prisoner=Candidate("captive/with/slash",KLMDJNIBCLD.Female,90,true);
  Check((bool)Call("RecruitEligible",faction,prisoner),"own prisoner bypasses native free-officer-only eligibility");
  StrategicSupport.StartRecruitmentQueue(1,2);Recruit();Check(Q("pending")==1&&Q("done")==0,"one pending mission reserves one slot, no premature success");
  Check(city.Roster.Any(h=>h.OGPEJIEHDAN.LKEPGHJDHGG==prisoner.OJPECMNINNA)&&!city.Roster.Any(h=>h.OGPEJIEHDAN.LKEPGHJDHGG==male.OJPECMNINNA),"talented female prisoner ranked ahead of weaker free woman; male excluded");
  Recruit();Check(Q("pending")==1,"repeat scan cannot overshoot target");Join(prisoner);Recruit();Check(Q("done")==1&&Q("pending")==0&&Q("enabled")==0&&UI_NotifyUI.Calls==1,"actual joining completes queue and shows one native notification");Recruit();Check(Q("done")==1&&UI_NotifyUI.Calls==1,"completed queue never recounts or repeats notification");
  Setup();var free=Candidate("free",KLMDJNIBCLD.Male,80);StrategicSupport.StartRecruitmentQueue(2,1);Recruit();Check(Q("pending")==1,"free officer native mission scheduled");foreach(var h in city.Roster)h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.None;Recruit();Check(Q("done")==0&&Q("pending")==0&&Q("retry/free")>NEINKOOBJIH.MIPFHCKDJOK,"failed mission does not count and has finite retry delay");
  NEINKOOBJIH.MIPFHCKDJOK+=8;StrategicSupport.PauseRecruitmentQueue(true);Recruit();Check(Q("pending")==0,"paused queue does not launch new missions");StrategicSupport.PauseRecruitmentQueue(false);Recruit();Check(Q("pending")==1,"resume launches eligible retry");
  var saved=MLGOLGGHLMB.CBEGDDKANAO;Call("ResetWork");Recruit();Check(Q("pending")==1,"pending work reconstructed from saved flags and native mission");MLGOLGGHLMB.CBEGDDKANAO=new();Recruit();Check(Q("pending")==0&&Q("goal")==0,"another save does not inherit queue");MLGOLGGHLMB.CBEGDDKANAO=saved;
  Setup();Check(StrategicSupport.StartRecruitmentQueue(0,0).Contains("1~1000")&&Q("goal")==0,"invalid requested count rejected without modifying save");
  var needs=new Dictionary<string,DevelopmentPolicy.Need>{{"casino",new(){Missing=2,Target=2}}};var actions=new List<DevelopmentPolicy.Action>();
  for(int tile=0;tile<4;tile++){
   actions.Add(new(){Id="casino",Key="c"+tile,District="D",Tiles=new[]{(long)tile},Limit=2,Priority=10,SecurityFlow=-30,Gains=new(){{"casino",1}}});
   actions.Add(new(){Id="police",Key="p"+tile,District="D",Tiles=new[]{(long)tile},Limit=1,Priority=9,SecurityFlow=60});
   actions.Add(new(){Id="farm",Key="f"+tile,District="D",Tiles=new[]{(long)tile},Limit=5,Priority=1,Agriculture=10,Food=100});
  }
  var plan=Solve(actions,needs);Check(plan.Count==4&&plan.Count(i=>actions[i].Id=="casino")==2&&plan.SelectMany(i=>actions[i].Tiles).Distinct().Count()==4,"two casino goal plus compensation fills all legal cells");double sec=1;foreach(int i in plan){sec+=actions[i].SecurityFlow;Check(sec>=1,"compensation is built before harmful goal, every committed prefix remains sustainable");}
  var remaining=actions.Where(a=>a.Id!="casino").ToList();Check(Solve(remaining,needs).Count==4,"locked building never leaves all usable land vacant");
  Setup();city.PopularFlow90=20;var tax=new JFLCKHMFGPJ();tax.MDPGDMKHDAG.Add(new(){HFBAJIGHOAF=(COGDLKNKJGA)29,HLNCKLGGECA=-1});Check(!(bool)Call("StrategyCivicSafe",tax,new List<AFAGFHJONFJ>{city}),"negative normal monthly popular flow rejects harmful tax package");city.PopularFlow90=100;Check((bool)Call("StrategyCivicSafe",tax,new List<AFAGFHJONFJ>{city}),"same package allowed when infrastructure absorbs cost");
  Setup();ICGAEMLACJG.HFJIECIBFKB.Clear();var army=new Wo_DeployTeamCtrl{m_id=900,m_belongPower=faction.IBGCLCAIBOL,Food=123457,Combat=6000};for(int i=0;i<3;i++){var h=city.Roster[i];army.m_listHero.Add(h);army.m_dicDeployArmy[h.OJPECMNINNA]=new(){new DHKINGPDKJJ{FGHFOLKBHDL=2000}};}ICGAEMLACJG.HFJIECIBFKB[army.m_id]=army;
  var refs=(HashSet<IntPtr>)Call("TeamTroops",army);var child=(Wo_DeployTeamCtrl)Call("SplitFieldArmy",army,2500L);Check(child!=null&&army.GetSoldierNum(false)+child.GetSoldierNum(false)==6000&&army.GetFood()+child.GetFood()==123457,"native split preserves soldiers and provisions");var union=(HashSet<IntPtr>)Call("TeamTroops",army);union.UnionWith((HashSet<IntPtr>)Call("TeamTroops",child));Check(union.SetEquals(refs)&&army.m_listHero.Count+child.m_listHero.Count==3,"split preserves troop identities and officers");child.MergeToTeam(army);Check(army.GetSoldierNum(false)==6000&&army.GetFood()==123457&&army.m_listHero.Count==3,"cancelled route can merge child without losses");army.m_warInfo=new();Check(Call("SplitFieldArmy",army,2500L)==null,"active battle cannot be mutated through ordinary aftermath split");
  Call("ResetBattleState");Call("ResetWork");UnityEngine.AI.NavMesh.Blocked=false;AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;Wa_Controller.m_warInfo=null;
  army.m_assembleCity=city.OJPECMNINNA;army.m_deployState=AKOPKFPKJPP.Camp;army.Position=new(0);army.Combat=6000;
  var origin=new NJHDEHLHENN{HIENOINGBCN="winning-front",EMMJBPMNFAK=new[]{faction.IBGCLCAIBOL,"enemy"},FGHFOLKBHDL=new[]{6000,1000},AOKBHBMECBD=new[]{6000,1000}};
  var destination=new NJHDEHLHENN{HIENOINGBCN="losing-front",EMMJBPMNFAK=new[]{faction.IBGCLCAIBOL,"enemy"},FGHFOLKBHDL=new[]{1000,6000},GBHGENNMJCK=new(20)};army.m_warInfo=origin;
  faction.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};
  foreach(var list in army.m_dicDeployArmy.Values)origin.KJPPGIOCDID[0].AddRange(list);
  ICGAEMLACJG.HFJIECIBFKB[901]=new(){m_id=901,m_belongPower="enemy",m_warInfo=origin,Combat=1000};
  ICGAEMLACJG.HFJIECIBFKB[902]=new(){m_id=902,m_belongPower=faction.IBGCLCAIBOL,m_warInfo=destination,Combat=1000};
  foreach(var phase in (IEnumerable<string>)Call("CrossFrontSteps",destination,faction.IBGCLCAIBOL)){}
  var reinforcements=ICGAEMLACJG.HFJIECIBFKB.Values.Where(t=>t.m_targetWarInfo==destination&&t.m_warInfo==null).ToList();
  Check(reinforcements.Count==1,"winning world battle dispatches one partial detachment to losing battle");
  Check(army.GetSoldierNum(false)+reinforcements.Sum(t=>t.GetSoldierNum(false))==6000&&army.GetFood()+reinforcements.Sum(t=>t.GetFood())==123457,"cross-front transaction conserves soldiers and food");
  Check(origin.FGHFOLKBHDL[0]>=origin.FGHFOLKBHDL[1]*1.15&&origin.KJPPGIOCDID[0].Sum(t=>t.FGHFOLKBHDL)==army.GetSoldierNum(false),"donor retains advantage and removes detached units from battle reserves");
  Check(origin.AOKBHBMECBD[0]==origin.FGHFOLKBHDL[0],"withdrawal is not reported as casualty");
  double surplus=(double)Call("FrontSurplus",origin,0,100d);Check(surplus>=0&&surplus<=origin.FGHFOLKBHDL[0]-origin.FGHFOLKBHDL[1]*1.15+1,"repeated reassessment cannot spend the original surplus twice");
  var enemyWave=new Wo_DeployTeamCtrl{m_id=909,m_belongPower="enemy",m_targetWarInfo=origin,m_deployState=AKOPKFPKJPP.Expedition,Position=new(5)};enemyWave.m_dicDeployArmy["wave"]=new(){new(){FGHFOLKBHDL=5000}};enemyWave.m_listMovePos.Add(origin.GBHGENNMJCK);ICGAEMLACJG.HFJIECIBFKB[909]=enemyWave;
  Check((double)Call("FrontSurplus",origin,0,100d)==0,"incoming enemy reinforcement prevents stripping a currently winning front");
  typeof(City79Tests).GetMethod("Setup",F).Invoke(null,null);var source=(AFAGFHJONFJ)typeof(City79Tests).GetField("c",F).GetValue(null);typeof(StrategicSupport).GetField("supportDestinationSlots",F).SetValue(null,1);
  try{var limited=(Wo_DeployTeamCtrl)Call("GenerateSupport",source,100000,new List<Wo_DeployTeamCtrl>(),new HashSet<IntPtr>());Check(limited!=null&&((HashSet<IntPtr>)Call("TeamTroops",limited)).Count==1,"one-slot occupation destination receives one unit without disabling source reserves");}finally{typeof(StrategicSupport).GetField("supportDestinationSlots",F).SetValue(null,0);}
  Console.WriteLine($"PASS Governor features {checks} checks: recruitment, officer tiers, layout, civic and split conservation; native calls mocked, not gameplay.");
 }
}
