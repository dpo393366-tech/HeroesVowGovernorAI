using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;
public static class Muster123Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static AFAGFHJONFJ staging,a,b;static MKMOJNAJKJE faction;static Wo_DeployTeamCtrl enemy;
 static object Call(string n,params object[] args)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,args);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Muster123: "+why);}
 static AFAGFHJONFJ City(string name,float x)=>(AFAGFHJONFJ)typeof(Relief86Tests).GetMethod("City",F).Invoke(null,new object[]{name,x});
 static void Drain(string name,params object[] args){int n=0;foreach(var phase in (IEnumerable<string>)Call(name,args))if(++n>10000)throw new Exception("unbounded muster");}
 static void Setup(){
  typeof(Hunt94Tests).GetMethod("Setup",F).Invoke(null,new object[]{2});ICGAEMLACJG.KBPAJECPNOH.Clear();Wo_TransportTeamCtrl.FailMove=Wo_TransportTeamCtrl.FailDebit=Wo_TransportTeamCtrl.FailRegister=false;
  staging=MICGPAMNDKM.IJPKEMPIFIH["Runan"];staging.Capacity=50;
  for(int i=10;i<40;i++)staging.Roster.Add(new(){OJPECMNINNA="reserve"+i,LMPMNLLGOEC=new HeroRow{CNOMNBCPJDI="Runan",LMMNKIOAGLC="F"}});staging.HomeOfficerCount=40;
  a=City("Henei",-10);b=City("Ye",-20);faction=NEINKOOBJIH.COCMABJFOFH["F"];enemy=ICGAEMLACJG.HFJIECIBFKB[1407];enemy.Combat=19000;
  // Production sizing is calibrated from a real failed native assembly result.
  var sample=new Wo_DeployTeamCtrl{Combat=12000};sample.m_dicDeployArmy["officer"]=new(){new(){FGHFOLKBHDL=6000}};Call("RememberHuntPower",staging,enemy,sample);
 }
 static List<Wo_TransportTeamCtrl> Convoys()=>ICGAEMLACJG.KBPAJECPNOH.Values.Where(t=>t&&!t.Recycled).ToList();
 public static void Run(){
  Setup();int before=staging.LMPMNLLGOEC.KJPPGIOCDID.Count;Drain("HuntMusterSteps",faction,enemy);var convoys=Convoys();
  Check(convoys.Count==2&&convoys.Select(t=>t.m_fromCity).Distinct().Count()==2,"two cities jointly supply one nearby staging city");
  Check(staging.LMPMNLLGOEC.KJPPGIOCDID.Count==before&&staging.Last==null,"no phantom early credit and no understrength expedition");
  Check(a.LMPMNLLGOEC.KJPPGIOCDID.Count>=4&&b.LMPMNLLGOEC.KJPPGIOCDID.Count>=4,"both donors retain physical defense");
  Check(convoys.All(t=>t.m_toCity=="Runan")&&(long)Call("HuntMusterMissing",staging,faction,enemy)==0,"incoming troops fully reserve the remaining estimated need");
  Drain("HuntMusterSteps",faction,enemy);Check(Convoys().Count==2,"repeat decision cannot double book incoming cargo");
  Check((bool)Call("HuntMusterCity",staging),"staging is protected from extra offensive planning while assembling");
  foreach(var t in convoys){foreach(var u in t.m_listArmy){u.GDHPLIFJCBN=PMNFKDJNLDJ.None;staging.LMPMNLLGOEC.KJPPGIOCDID.Add(u);}staging.LMPMNLLGOEC.OEGNGFBCHFL+=t.m_food;ICGAEMLACJG.KBPAJECPNOH.Remove(t.m_id);t.Recycle();}
  Drain("HuntSteps",faction);Check(staging.Last!=null&&!staging.Last.Dismissed&&staging.Last.m_attackTarget==enemy&&staging.Last.GetCombatVal()>=(int)Call("HuntPower",enemy),"after mocked physical delivery production native power recheck launches sufficient force");
  Setup();staging.Capacity=10;Drain("HuntMusterSteps",faction,enemy);Check(Convoys().Count==0,"full staging capacity blocks extra shipments");
  Setup();faction.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Drain("HuntMusterSteps",faction,enemy);Check(Convoys().Count==0,"StopWar forbids joint suppression");
  Setup();a.LMPMNLLGOEC.KJPPGIOCDID.ForEach(u=>u.OLMAJLLIJDM="working");b.LMPMNLLGOEC.KJPPGIOCDID.ForEach(u=>u.OLMAJLLIJDM="working");Drain("HuntMusterSteps",faction,enemy);Check(Convoys().Count==0,"occupied native workers are not cancelled for troop shipments");
  Setup();UnityEngine.AI.NavMesh.Blocked=true;Drain("HuntMusterSteps",faction,enemy);Check(Convoys().Count==0,"broken actual map road cannot create cargo");UnityEngine.AI.NavMesh.Blocked=false;
  Setup();Drain("HuntMusterSteps",faction,enemy);enemy.m_belongPower="new-faction";Check(!(bool)Call("HuntMusterCity",staging),"target no longer a raider releases staging claim without deleting cargo");
  Check(Convoys().Count==2,"already dispatched native shipments survive target changes");Call("ResetOffense");Check(!(bool)Call("HuntMusterCity",staging),"world reset drops temporary claims");
  Console.WriteLine($"PASS Muster123 {checks} joint-city, conservation, defense, policy and actual-power recheck scenarios; delivery mocked, not gameplay.");
 }
}
