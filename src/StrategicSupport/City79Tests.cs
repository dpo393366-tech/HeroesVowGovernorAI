using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
public static class City79Tests {
 static int checks;static AFAGFHJONFJ c,target;static MKMOJNAJKJE f;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("R79: "+why);}
 static void Setup(){typeof(StrategicSafetyTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);c=(AFAGFHJONFJ)typeof(StrategicSafetyTests).GetField("c",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);f=NEINKOOBJIH.COCMABJFOFH["F"];AEHAGJAKNEK.IEOHLJLICOA.Clear();c.Capacity=6;ICGAEMLACJG.KBPAJECPNOH.Clear();Wo_TransportTeamCtrl.FailMove=Wo_TransportTeamCtrl.FailDebit=Wo_TransportTeamCtrl.FailRegister=false;
  target=new(){OJPECMNINNA="recipient",Roster=new()};target.LMPMNLLGOEC.LMMNKIOAGLC="F";target.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.FKMKGNPFLFP.Add(target);target.FKMKGNPFLFP.Add(c);MICGPAMNDKM.IJPKEMPIFIH[target.OJPECMNINNA]=target;ICGAEMLACJG.FKKCPDEFODO[target.OJPECMNINNA]=new(){City=target};
 }
 static void RunTransfer(){foreach(var phase in (IEnumerable<string>)Call("CapacityTransferSteps",c,f)){} }
 static List<Wo_DeployTeamCtrl> Teams()=>ICGAEMLACJG.HFJIECIBFKB.Values.ToList();
 static int Room()=>(int)Call("CapacityRoom",target,Teams(),null);
 static CLAGKDECFID Hero(string id,string city,string faction="F"){var h=new CLAGKDECFID{OJPECMNINNA=id,LMPMNLLGOEC=new(){LMMNKIOAGLC=faction,CNOMNBCPJDI=city}};AEHAGJAKNEK.IEOHLJLICOA[id]=h;return h;}
 static void ClearTimers()=>((IDictionary)typeof(StrategicSupport).GetField("next",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Clear();
 public static void Run(){
  Setup();c.Roster=new();c.Roster.Add(Hero(NEINKOOBJIH.OPIKIHPNCIP,c.OJPECMNINNA));
  for(int i=0;i<82;i++){var h=Hero("owned"+i,c.OJPECMNINNA);c.Roster.Add(h);NEINKOOBJIH.JMIADAFPGMC.Add(h.OJPECMNINNA);}
  for(int i=0;i<7;i++)c.Roster.Add(Hero("neutral"+i,c.OJPECMNINNA,""));
  var home=(HashSet<string>)Call("HomeOfficers",c,new List<Wo_DeployTeamCtrl>());
  Check(home.Count==82&&!home.Contains("neutral0"),"actual 83 owned/82 JMI pattern retains 82 faction officers, not seven neutral residents");
  NEINKOOBJIH.AOPJBNBGPLP.Add("owned0");c.Roster[2].OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;c.Roster[3].OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.CityMove;
  home=(HashSet<string>)Call("HomeOfficers",c,new List<Wo_DeployTeamCtrl>());
  Check(home.Count==79&&!home.Contains("owned0")&&!home.Contains("owned1")&&!home.Contains("owned2"),"actual party, research and travelling officers excluded from command reserve");
  foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",f)){}
  var moved=c.Roster.Where(h=>h.MoveCalls>0).ToList();
  Check(moved.Count>12&&moved[0].LMPMNLLGOEC.LMMNKIOAGLC=="F"&&moved[0].OJPECMNINNA!="owned0","independent Xinye relocation works with 82-entry nonparty list");
  var arriving=moved[0];arriving.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.None;arriving.LMPMNLLGOEC.CNOMNBCPJDI=target.OJPECMNINNA;c.Roster.Remove(arriving);target.Roster.Add(arriving);
  foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",f)){}
  Check(target.LMPMNLLGOEC.BKENJPNKMGI==arriving.OJPECMNINNA,"actual-party correction allows local appointment after mocked native arrival");
  Setup();var before=c.LMPMNLLGOEC.KJPPGIOCDID.ToArray();c.LMPMNLLGOEC.KJPPGIOCDID.Add(before[0]);Call("NormalizeTroopReferences",c);
  Check(before.SequenceEqual(c.LMPMNLLGOEC.KJPPGIOCDID)&&before.Sum(a=>a.FGHFOLKBHDL)==10000,"only duplicate pointer removed; equal IDs/types in distinct native units preserved");
  var generated=(Wo_DeployTeamCtrl)Call("GenerateSupport",c,500,Teams(),new HashSet<IntPtr>());generated.ReturnUnitsOnDismiss=true;Call("DismissSupport",generated);
  Check(before.SequenceEqual(c.LMPMNLLGOEC.KJPPGIOCDID),"native cancellation returning already-home troops cannot duplicate roster");
  Check(Call("GenerateSupport",c,500,Teams(),new HashSet<IntPtr>())!=null,"another support attempt succeeds after cancellation");
  Setup();RunTransfer();var convoy=ICGAEMLACJG.KBPAJECPNOH.Values.Single();
  Check(c.Last==null&&convoy.m_toCity==target.OJPECMNINNA&&convoy.m_listArmy.Sum(a=>a.FGHFOLKBHDL)==4000,"overflow uses four-unit native convoy without army assembly");
  Check(target.LMPMNLLGOEC.KJPPGIOCDID.Count==0&&c.LMPMNLLGOEC.KJPPGIOCDID.Count==6,"cargo debited at departure but never credited early");
  Check(Room()==6,"inbound convoy reserves destination slots");
  ClearTimers();RunTransfer();Check(ICGAEMLACJG.KBPAJECPNOH.Count==1,"departed cargo cannot be dispatched twice");
  Setup();target.Capacity=2;RunTransfer();Check(ICGAEMLACJG.KBPAJECPNOH.Values.Single().m_listArmy.Count==2,"destination room limits convoy");
  Setup();target.Capacity=0;RunTransfer();Check(ICGAEMLACJG.KBPAJECPNOH.Count==0,"full destination blocked");
  Setup();target.LMPMNLLGOEC.OEGNGFBCHFL=0;RunTransfer();Check(ICGAEMLACJG.KBPAJECPNOH.Values.Single().m_food>0,"food shortage supplied with troop cargo");
  Setup();ICGAEMLACJG.HFJIECIBFKB[901]=new(){m_id=901,m_belongPower="enemy",m_targetCity=c.OJPECMNINNA};RunTransfer();Check(ICGAEMLACJG.KBPAJECPNOH.Count==0,"invaded source never emptied into a civilian convoy");
  Setup();c.HomeOfficerCount=2;RunTransfer();Check(ICGAEMLACJG.KBPAJECPNOH.Count==1&&c.Last==null,"troop cargo does not consume the last commanders");
  Setup();target.Capacity=8;var shared=new DHKINGPDKJJ();target.LMPMNLLGOEC.KJPPGIOCDID.Add(shared);var inbound=new Wo_DeployTeamCtrl{m_id=901,m_belongPower="F",m_targetCity=target.OJPECMNINNA};inbound.m_dicDeployArmy["x"]=new(){shared};ICGAEMLACJG.HFJIECIBFKB[901]=inbound;Check(Room()==7,"home/incoming overlap counted once");
  Check(BattlePolicy.AssessNeed(1000,2000,100,0,9,0,1.15,out var reason)==0&&reason=="collapse-before-arrival","late rescue logged separately from covered battle");
  Check(BattlePolicy.AssessNeed(10000,5000,0,0,10,0,1.15,out reason)==0&&reason=="covered-at-arrival","covered forecast labelled accurately");
  Check(BattlePolicy.AssessNeed(1000,2000,0,300,10,0,1.15,out reason)==0&&reason=="enemy-projected-defeated","projected enemy defeat distinguished");
  Setup();target.Capacity=0;var middle=new AFAGFHJONFJ{OJPECMNINNA="middle",Capacity=0};middle.LMPMNLLGOEC.LMMNKIOAGLC="F";middle.LMPMNLLGOEC.IPJHOOJBCKI="gov";MICGPAMNDKM.IJPKEMPIFIH["middle"]=middle;target.FKMKGNPFLFP.Add(middle);middle.FKMKGNPFLFP.Add(target);
  var far=new AFAGFHJONFJ{OJPECMNINNA="far",Capacity=4};far.LMPMNLLGOEC.LMMNKIOAGLC="F";far.LMPMNLLGOEC.IPJHOOJBCKI="gov";MICGPAMNDKM.IJPKEMPIFIH["far"]=far;middle.FKMKGNPFLFP.Add(far);far.FKMKGNPFLFP.Add(middle);ICGAEMLACJG.FKKCPDEFODO["far"]=new(){City=far};RunTransfer();Check(ICGAEMLACJG.KBPAJECPNOH.Values.Any(t=>t.m_toCity=="far"),"three-hop friendly recipient found when nearer cities are full");
  Setup();var iterator=((IEnumerable<string>)Call("CapacityTransferSteps",c,f)).GetEnumerator();Check(iterator.MoveNext(),"capacity scan yields before native commit");c.Capacity=20;while(iterator.MoveNext()){}iterator.Dispose();Check(c.Last==null,"new capacity completed during scan cancels obsolete redistribution");
  var rng=new Random(7918);for(int i=0;i<200;i++){Setup();int capacity=rng.Next(2,11),space=rng.Next(0,11);c.Capacity=capacity;target.Capacity=space;RunTransfer();int sent=ICGAEMLACJG.KBPAJECPNOH.Values.Sum(t=>t.m_listArmy.Count);Check(sent<=10-capacity&&sent<=space&&sent<=8&&Room()>=0,"random transfers honor source surplus, destination slots and minimum garrison");}
  Console.WriteLine($"PASS R79 {checks} checks; native roster/relocation/capacity lifecycle mocked, not live-game proof.");
 }
}

