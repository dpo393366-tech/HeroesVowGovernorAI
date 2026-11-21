using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;
public static class Return95Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static object Fixture(Type t,string n,params object[] a)=>t.GetMethod(n,F).Invoke(null,a);
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Return95: "+why);}
 static void Drain(string n,params object[] a){foreach(var p in (IEnumerable<string>)Call(n,a)){} }
 public static void Run(){
  // Match the actual 302007 soldiers/39 units/44 officers and destinations
  // smaller than the army. Model native pooled-child residue and missing model.
  Fixture(typeof(Relief86Tests),"Setup");ICGAEMLACJG.HFJIECIBFKB.Clear();MICGPAMNDKM.IJPKEMPIFIH.Clear();ICGAEMLACJG.FKKCPDEFODO.Clear();
  var f=NEINKOOBJIH.COCMABJFOFH["F"];var cities=new List<AFAGFHJONFJ>();
  foreach(int i in Enumerable.Range(0,6)){var c=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","split95-"+i,(float)(i*10));c.Capacity=8;c.LMPMNLLGOEC.KJPPGIOCDID.Clear();cities.Add(c);if(i>0)Fixture(typeof(Relief86Tests),"Link",cities[i-1],c);}
  var army=new Wo_DeployTeamCtrl{m_id=2064,m_belongPower="F",m_assembleCity=cities[0].OJPECMNINNA,Food=616839,Combat=3900000,m_deployState=AKOPKFPKJPP.Camp};
  for(int i=0;i<44;i++){var h=new CLAGKDECFID{OJPECMNINNA="large-officer"+i,LMPMNLLGOEC=new(){CNOMNBCPJDI=cities[0].OJPECMNINNA,LMMNKIOAGLC="F"}};army.m_listHero.Add(h);army.m_dicDeployArmy[h.OJPECMNINNA]=new();if(i<39)army.m_dicDeployArmy[h.OJPECMNINNA].Add(new(){FGHFOLKBHDL=i==38?5607:7800,CombatValue=100000});}
  ICGAEMLACJG.HFJIECIBFKB[2064]=army;AOHIBKLOAEI.RegionAt=v=>cities[Math.Clamp((int)Math.Round(v.x/10),0,5)];Call("RememberCampaign",army);
  var goals=(IDictionary)typeof(StrategicSupport).GetField("campaignGoals",F).GetValue(null);var goal=goals[army.Pointer];goal.GetType().GetField("Returning").SetValue(goal,true);
  Wo_DeployTeamCtrl.DirtySplit95=true;try{Drain("ReturnPartitionSteps",army,goal,f);}finally{Wo_DeployTeamCtrl.DirtySplit95=false;}
  var teams=ICGAEMLACJG.HFJIECIBFKB.Values.Where(t=>t.m_belongPower=="F").ToList();
  Check(teams.Sum(t=>t.GetSoldierNum(false))==302007&&teams.Sum(t=>t.GetFood())==616839&&teams.Sum(t=>t.m_listHero.Count)==44,"large-army partition conserves soldiers, food, officers exactly");
  Check(teams.Count(t=>t.m_isEnterCity)>=4,"pooled native children receive committed entry orders across several cities");
  foreach(var c in cities)Check(teams.Where(t=>t.m_isEnterCity&&t.m_targetCity==c.OJPECMNINNA).Sum(t=>((HashSet<IntPtr>)Call("TeamTroops",t)).Count)<=8,"all inbound reservations respect physical capacity");
  Check(teams.Where(t=>t!=army).All(t=>t.m_mergeTarget==null&&t.m_attackTarget==null&&!t.m_isStarting&&t.gameObject.activeSelf),"stale native pool commands and unfinished visual initialization cannot prevent departure");
  // A partial-capacity staging order is intentionally non-entry; do not turn
  // it into an invalid-full-entry event every return-watch interval.
  Fixture(typeof(Return94Tests),"Setup");army=Field<Wo_DeployTeamCtrl>(typeof(Return94Tests),"army");f=Field<MKMOJNAJKJE>(typeof(Return94Tests),"faction");var near=Field<AFAGFHJONFJ>(typeof(Return94Tests),"near");goal=Field<object>(typeof(Return94Tests),"goal");
  foreach(var c in MICGPAMNDKM.IJPKEMPIFIH.Values)c.Capacity=0;near.Capacity=1;
  army.m_listHero.Add(new(){OJPECMNINNA="extra95"});army.m_dicDeployArmy["extra95"]=new(){new(){FGHFOLKBHDL=1000}};
  army.m_targetCity="";army.m_isEnterCity=false;goal.GetType().GetField("Staging").SetValue(goal,true);goal.GetType().GetField("Ordered").SetValue(goal,true);goal.GetType().GetField("OrderTarget").SetValue(goal,near.OJPECMNINNA);
  int moves=army.CityMoves;for(int i=0;i<20;i++)Drain("ReturnWatchSteps",army,goal);
  Check(army.m_deployState==AKOPKFPKJPP.Expedition&&army.CityMoves==moves,"20 repeated live checks do not cancel or reverse an unchanged staging journey");
  // Reproduce Puyang: personal governor excludes outgoing automation but
  // must not exclude incoming officer relief for an empty captured city.
  Fixture(typeof(Relief86Tests),"Setup");ICGAEMLACJG.HFJIECIBFKB.Clear();f=NEINKOOBJIH.COCMABJFOFH["F"];
  var source=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","donor95",0f);var target=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","puyang95",10f);Fixture(typeof(Relief86Tests),"Link",source,target);
  target.Roster.Clear();target.LMPMNLLGOEC.KJPPGIOCDID.Clear();source.LMPMNLLGOEC.KJPPGIOCDID.Clear();target.LMPMNLLGOEC.IPJHOOJBCKI=NEINKOOBJIH.OPIKIHPNCIP;target.OPNKGGKHAGJ=false;
  NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";f.ANMMMOIKLEG[NEINKOOBJIH.OPIKIHPNCIP]=new(){LMMNKIOAGLC="F"};AEHAGJAKNEK.IEOHLJLICOA.Clear();foreach(var h in source.Roster)AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;
  Check((string)Call("Eligibility",target,null)=="player-personal-governor"&&(bool)Call("StaffingDestination",target,f),"personal management protection is independent of accepting help");
  Drain("AdministrativeStaffingSteps",f);Check(source.Roster.Any(h=>h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&h.OGPEJIEHDAN.NHKNHHIBOGK==target.OJPECMNINNA),"empty personal-governor capture receives actual independent officer move");
  Check(target.LMPMNLLGOEC.IPJHOOJBCKI==NEINKOOBJIH.OPIKIHPNCIP&&!target.OPNKGGKHAGJ,"incoming relief does not change personal governor or management toggle");
  Call("ResetWork");Call("ResetDefense");AOHIBKLOAEI.RegionAt=null;
  Fixture(typeof(OffensiveTests),"Setup");f=Field<MKMOJNAJKJE>(typeof(OffensiveTests),"f");source=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");target=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","governor-two",30f);
  target.LMPMNLLGOEC.IPJHOOJBCKI="gov2";f.ANMMMOIKLEG["gov2"]=new(){LMMNKIOAGLC="F",BNGFJOAJKDH=NEFBEOFPGJA.AttackPower,LMAEGAJADKI="enemy2"};f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;
  Check(!(bool)Call("AttackPermission",source,f,"gov","enemy2")&&(bool)Call("AttackPermission",target,f,"gov2","enemy2")&&!(bool)Call("AttackPermission",target,f,"gov2","enemy"),"two governors retain independent StopWar and designated-enemy policies");
  var budget=Call("Budget",f);Check(ReferenceEquals(budget,Call("Budget",f)),"two districts consume one shared faction budget");
  Call("ResetWork");Call("ScheduleFaction",f);var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");int jobs=queue.Count;Call("ScheduleFaction",f);Check(jobs>0&&queue.Count==jobs,"second governor cannot enqueue duplicate faction staffing, diplomacy or budget jobs");
  target.LMPMNLLGOEC.IPJHOOJBCKI="gov";Check(!(bool)Call("CurrentCity",target,f,"gov2"),"old governor's suspended city plan invalidated immediately after reassignment");
  target.Capacity=3;target.LMPMNLLGOEC.KJPPGIOCDID.Clear();var one=new Wo_DeployTeamCtrl{m_belongPower="F",m_assembleCity=source.OJPECMNINNA,m_targetCity=target.OJPECMNINNA};one.m_dicDeployArmy["a"]=new(){new(),new()};var two=new Wo_DeployTeamCtrl{m_belongPower="F",m_assembleCity=target.OJPECMNINNA,m_targetCity=target.OJPECMNINNA};two.m_dicDeployArmy["b"]=new(){new()};
  Check((int)Call("CapacityRoom",target,new List<Wo_DeployTeamCtrl>{one,two},null)==0,"different governor origins share the same inbound-capacity reservation");
  Call("ResetWork");Call("ResetDefense");
  Console.WriteLine($"PASS Return95 {checks} pooled-child, 302007-army partition, anti-oscillation and personal capture staffing checks; native actions mocked.");
 }
}
