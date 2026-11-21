using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;
public static class Governor96Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static object Fixture(Type t,string n,params object[] a)=>t.GetMethod(n,F).Invoke(null,a);
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Governor96: "+why);}
 static void Drain(string n,params object[] a){foreach(var phase in (IEnumerable<string>)Call(n,a)){} }
 static string Governor(string mode)=>mode=="personal"?NEINKOOBJIH.OPIKIHPNCIP:mode=="unassigned"?"":"gov2";
 static void Policy(AFAGFHJONFJ c,MKMOJNAJKJE f,string mode){c.LMPMNLLGOEC.IPJHOOJBCKI=Governor(mode);c.OPNKGGKHAGJ=false;if(mode=="other")f.ANMMMOIKLEG["gov2"]=new(){LMMNKIOAGLC=f.IBGCLCAIBOL,BNGFJOAJKDH=NEFBEOFPGJA.StopWar};}
 public static void Run(){
  new StrategicSupport().Load();
  foreach(string mode in new[]{"personal","unassigned","other"}){
   Fixture(typeof(IntegrationTests),"Prepare");Call("Ready");Call("ResetDefense");Call("ResetWork");UnityEngine.AI.NavMesh.Blocked=false;UnityEngine.AI.NavMesh.Detour=0;AOHIBKLOAEI.Region=null;AOHIBKLOAEI.RegionAt=null;
   var f=NEINKOOBJIH.COCMABJFOFH["F"];var target=MICGPAMNDKM.IJPKEMPIFIH["target"];var source=MICGPAMNDKM.IJPKEMPIFIH["source"];Policy(target,f,mode);
   Drain("DefenseSteps",f);
   var orders=(IDictionary)typeof(StrategicSupport).GetField("defenseOrders",F).GetValue(null);
   Check(source.Last!=null&&source.Last.m_targetCity==target.OJPECMNINNA&&orders.Contains(source.Last.Pointer),mode+" city receives native preventive assembly from other governor");
   Check(target.LMPMNLLGOEC.IPJHOOJBCKI==Governor(mode)&&!target.OPNKGGKHAGJ,mode+" support does not alter destination governor or management toggle");
   Fixture(typeof(BattleIntegrationTests),"Setup");Call("ResetDefense");Call("ResetWork");Call("ResetBattleState");typeof(StrategicSupport).GetField("schedulerOnline",F).SetValue(null,false);
   f=NEINKOOBJIH.COCMABJFOFH["F"];source=Field<AFAGFHJONFJ>(typeof(BattleIntegrationTests),"source");var war=Field<NJHDEHLHENN>(typeof(BattleIntegrationTests),"war");target=new(){OJPECMNINNA="held-"+mode};target.LMPMNLLGOEC.LMMNKIOAGLC="F";Policy(target,f,mode);MICGPAMNDKM.IJPKEMPIFIH[target.OJPECMNINNA]=target;war.IJGLCFJOOIK=target;
   Call("Assess",war,0d,true);Call("Assess",war,10d,true);
   Check(source.Last!=null&&source.Last.m_targetWarInfo==war&&source.Last.Starts==1,mode+" ongoing battle receives actual support order and native departure start");
   // Interior, established city with three officers and mayor: r95 excluded
   // this recipient because it was neither frontier nor a vulnerable capture.
   Fixture(typeof(Relief86Tests),"Setup");ICGAEMLACJG.HFJIECIBFKB.Clear();MICGPAMNDKM.IJPKEMPIFIH.Clear();ICGAEMLACJG.FKKCPDEFODO.Clear();AEHAGJAKNEK.IEOHLJLICOA.Clear();
   f=NEINKOOBJIH.COCMABJFOFH["F"];source=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","donor96-"+mode,0f);target=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","recipient96-"+mode,10f);Fixture(typeof(Relief86Tests),"Link",source,target);
   source.LMPMNLLGOEC.KJPPGIOCDID.Clear();target.LMPMNLLGOEC.KJPPGIOCDID.Clear();Policy(target,f,mode);target.Roster=target.Roster.Take(3).ToList();target.LMPMNLLGOEC.BKENJPNKMGI=target.Roster[0].OJPECMNINNA;
   foreach(var h in source.Roster.Concat(target.Roster))AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;
   Check(!(bool)Call("OccupationVulnerable",target)&&(int)Call("BorderTier",target,f)==1,"fixture is established safe interior, not previous exception");
   Check((bool)Call("StaffingDestination",target,f),mode+" same-faction recipient allowed without governor or native automation");
   Drain("AdministrativeStaffingSteps",f);
   Check(source.Roster.Any(h=>h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&h.OGPEJIEHDAN.NHKNHHIBOGK==target.OJPECMNINNA),mode+" receives independent officer move without army or troop slot");
   Check(target.LMPMNLLGOEC.IPJHOOJBCKI==Governor(mode)&&!target.OPNKGGKHAGJ&&target.Roster.All(h=>h.OGPEJIEHDAN.HFBAJIGHOAF!=GCPCLLMKIMM.CityMove),mode+" incoming staffing preserves manual city officers and settings");
   target.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!(bool)Call("StaffingDestination",target,f),"captured or foreign recipient excluded immediately");
   // The user's expanded requirement includes outgoing resources from personal
   // or unassigned sources, independently of domestic automation permissions.
   Fixture(typeof(IntegrationTests),"Prepare");Call("Ready");Call("ResetDefense");Call("ResetWork");
   f=NEINKOOBJIH.COCMABJFOFH["F"];source=MICGPAMNDKM.IJPKEMPIFIH["source"];target=MICGPAMNDKM.IJPKEMPIFIH["target"];Policy(source,f,mode);Drain("DefenseSteps",f);
   Check(source.Last!=null&&source.Last.m_targetCity==target.OJPECMNINNA,mode+" source can send defensive support irrespective of governor policy");
   Check(source.LMPMNLLGOEC.IPJHOOJBCKI==Governor(mode)&&!source.OPNKGGKHAGJ,mode+" outgoing resource permission does not enable domestic management");
   Fixture(typeof(Logistics93Tests),"Setup");ICGAEMLACJG.HFJIECIBFKB.Clear();MICGPAMNDKM.IJPKEMPIFIH.Clear();ICGAEMLACJG.FKKCPDEFODO.Clear();AEHAGJAKNEK.IEOHLJLICOA.Clear();
   f=NEINKOOBJIH.COCMABJFOFH["F"];source=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","source96-"+mode,0f);target=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","destination96-"+mode,10f);Fixture(typeof(Relief86Tests),"Link",source,target);Policy(source,f,mode);Policy(target,f,"unassigned");
   source.Capacity=8;target.Capacity=30;target.LMPMNLLGOEC.KJPPGIOCDID.Clear();int totalSoldiers=source.LMPMNLLGOEC.KJPPGIOCDID.Sum(a=>a.FGHFOLKBHDL);
   Drain("CapacityTransferSteps",source,f);
   var cargo=ICGAEMLACJG.KBPAJECPNOH.Values.ToList();Check(cargo.Count==1&&cargo[0].m_fromCity==source.OJPECMNINNA&&cargo[0].m_toCity==target.OJPECMNINNA&&cargo[0].m_listArmy.Count==2,mode+" overflow exports native convoy into unassigned city");
   Check(source.LMPMNLLGOEC.KJPPGIOCDID.Sum(a=>a.FGHFOLKBHDL)+cargo.Sum(t=>t.m_listArmy.Sum(a=>a.FGHFOLKBHDL))==totalSoldiers&&source.Last==null,"cross-governor transfer conserves soldiers and does not assemble a marching army");
   source.LMPMNLLGOEC.KJPPGIOCDID.Clear();target.LMPMNLLGOEC.KJPPGIOCDID.Clear();target.Roster=target.Roster.Take(3).ToList();target.LMPMNLLGOEC.BKENJPNKMGI=target.Roster[0].OJPECMNINNA;
   foreach(var h in source.Roster.Concat(target.Roster))AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;
   Drain("AdministrativeStaffingSteps",f);
   Check(source.Roster.Any(h=>h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&h.OGPEJIEHDAN.NHKNHHIBOGK==target.OJPECMNINNA),mode+" source sends independent officers to unassigned city");
   ICGAEMLACJG.KBPAJECPNOH.Clear();source.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){FGHFOLKBHDL=1000});target.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){FGHFOLKBHDL=1000});source.LMPMNLLGOEC.OEGNGFBCHFL=100000;target.LMPMNLLGOEC.OEGNGFBCHFL=0;
   Drain("CityFoodBalanceSteps",f);cargo=ICGAEMLACJG.KBPAJECPNOH.Values.ToList();
   Check(cargo.Count==1&&cargo[0].m_food>=30000&&cargo[0].m_listArmy.Count==0,mode+" pure-food native convoy fills recipient deficit without moving soldiers");
   Check(source.LMPMNLLGOEC.OEGNGFBCHFL+cargo[0].m_food==100000&&source.LMPMNLLGOEC.OEGNGFBCHFL>=30000&&target.LMPMNLLGOEC.OEGNGFBCHFL==0,"food debited once, donor reserve protected, destination not credited before arrival");
   Drain("CityFoodBalanceSteps",f);Check(ICGAEMLACJG.KBPAJECPNOH.Count==1,"in-transit food prevents duplicate balancing orders");
   Fixture(typeof(SupplyTests),"Setup");source=Field<AFAGFHJONFJ>(typeof(SupplyTests),"city");var battle=Field<NJHDEHLHENN>(typeof(SupplyTests),"war");var army=Field<Wo_DeployTeamCtrl>(typeof(SupplyTests),"army");f=NEINKOOBJIH.COCMABJFOFH["F"];Policy(source,f,mode);
   Call("SupplyTick",battle,0d,true);army.Food=0;Call("SupplyTick",battle,24d,true);Check(army.Food>0&&source.LMPMNLLGOEC.OEGNGFBCHFL<100000,mode+" source supplies a field army after real estimated delivery time");
  }
  for(int failure=1;failure<=3;failure++){
   Fixture(typeof(Logistics93Tests),"Setup");ICGAEMLACJG.HFJIECIBFKB.Clear();MICGPAMNDKM.IJPKEMPIFIH.Clear();ICGAEMLACJG.FKKCPDEFODO.Clear();
   var f=NEINKOOBJIH.COCMABJFOFH["F"];var source=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","food-fault-source",0f);var target=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","food-fault-target",10f);Fixture(typeof(Relief86Tests),"Link",source,target);Policy(source,f,"personal");Policy(target,f,"unassigned");source.LMPMNLLGOEC.OEGNGFBCHFL=1000000;target.LMPMNLLGOEC.OEGNGFBCHFL=0;
   Wo_TransportTeamCtrl.FailMove=failure==1;Wo_TransportTeamCtrl.FailDebit=failure==2;Wo_TransportTeamCtrl.FailRegister=failure==3;Drain("CityFoodBalanceSteps",f);
   Check(source.LMPMNLLGOEC.OEGNGFBCHFL==1000000&&target.LMPMNLLGOEC.OEGNGFBCHFL==0&&ICGAEMLACJG.KBPAJECPNOH.Count==0,"food-only native failure rolls back cargo and source debit: "+failure);
  }
  Wo_TransportTeamCtrl.FailMove=Wo_TransportTeamCtrl.FailDebit=Wo_TransportTeamCtrl.FailRegister=false;
  Call("ResetDefense");Call("ResetWork");Call("ResetBattleState");AOHIBKLOAEI.Region=null;AOHIBKLOAEI.RegionAt=null;
  Drain("DefensePulse");var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");Check(queue.Count>0,"daily distribution is scheduled even with only personal and unassigned cities");
  Call("ResetDefense");Call("ResetWork");
  Console.WriteLine($"PASS Governor96 {checks} cross-governor, personal and unassigned recipient dispatch/staffing checks; native movement mocked, not gameplay.");
 }
}
