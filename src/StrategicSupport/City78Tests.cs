using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
public static class City78Tests {
 static int checks;static AFAGFHJONFJ c;static MKMOJNAJKJE f;static JNHOOCJJPJO d;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static void Check(bool ok,string why){if(!ok)throw new Exception("R78: "+why);checks++;}
 static void Setup(){typeof(MaintenanceTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);object Field(string n)=>typeof(MaintenanceTests).GetField(n,BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);c=(AFAGFHJONFJ)Field("c");f=(MKMOJNAJKJE)Field("f");d=(JNHOOCJJPJO)Field("d");AEHAGJAKNEK.IEOHLJLICOA.Clear();}
 static PABIPDOFBFG Def(string id,int cap=0,string basis=null){var b=new PABIPDOFBFG{OJPECMNINNA=id,IKELHGAHBID=cap,MDOIJIAONJM=basis};AEHAGJAKNEK.ACIGFKMDOKE[id]=b;return b;}
 static OGABIJECHFG Add(PABIPDOFBFG b){var x=new OGABIJECHFG{GEKKLMPLDHJ=b,GMIKIMACDBC=d};d.AIMLPNBOIIF.Add(x);return x;}
 static void Overload(){c.Capacity=9;for(int i=0;i<16;i++)c.LMPMNLLGOEC.KJPPGIOCDID.Add(new());}
 public static void Run(){
  Setup();Overload();Check((int)Call("CapacityShortfall",c)==7,"actual 16 units vs 9 capacity gives shortage 7");
  var barrack=Def("barracks",2);var plan=Call("ReadRenewalGoals",c,f);var targets=(IDictionary)plan.GetType().GetField("Buildings").GetValue(plan);
  Check(targets.Contains("barracks")&&c.HFLAGGEFBOO.Count==0,"internal capacity goal without editing native city goals");
  var pending=Add(barrack);pending.EMHNNCLBADE=0;Check((int)Call("CapacityShortfall",c)==0,"wait for committed capacity construction rather than duplicate blindly");
  pending.EMHNNCLBADE=100;c.Capacity=11;Check((int)Call("CapacityShortfall",c)==5,"re-evaluate native capacity after completion");
  var up=Def("high-barracks",3,"barracks");pending.GEKKLMPLDHJ=up;pending.EMHNNCLBADE=0;Check((int)Call("CapacityShortfall",c)==0,"pending direct upgrade also waits");
  Setup();Overload();barrack=Def("barracks",2);barrack.TechAllowed=false;plan=Call("ReadRenewalGoals",c,f);targets=(IDictionary)plan.GetType().GetField("Buildings").GetValue(plan);Check(!targets.Contains("barracks"),"no illegal locked construction");
  Setup();Overload();var damaged=Add(Def("damaged-market"));damaged.OGFAHJHBCNG=400;var basic=Def("barracks",2);var x=Add(basic);up=Def("high-barracks",3,"barracks");
  var rich=Def("rich-market",0,"damaged-market");rich.CNCPIILGDEF=10000;
  var cash=(long)f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
  foreach(var step in (IEnumerable<bool>)Call("MaintenanceSteps",c,f)){}
  Check(damaged.OGFAHJHBCNG==650&&x.GEKKLMPLDHJ==up,"repair and urgent capacity upgrade both happen in same visit");
  Check(damaged.GEKKLMPLDHJ.OJPECMNINNA=="damaged-market","capacity upgrade beats lucrative optional economy upgrade");
  Check(cash-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==638,"repair and upgrade each paid once");
  Setup();Overload();basic=Def("barracks",2);x=Add(basic);up=Def("high-barracks",3,"barracks");pending=Add(basic);pending.EMHNNCLBADE=0;
  foreach(var step in (IEnumerable<bool>)Call("MaintenanceSteps",c,f)){}
  Check(x.GEKKLMPLDHJ==basic,"unfinished capacity expansion prevents another optional upgrade");
  Setup();for(int i=0;i<1024;i++)AEHAGJAKNEK.IEOHLJLICOA["h"+i]=new(){LMPMNLLGOEC=new(){LMMNKIOAGLC="other"}};
  var missions=new List<CLAGKDECFID>();var pool=new List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>();
  var iterator=((IEnumerable<string>)Call("StateOfficerSteps",new List<AFAGFHJONFJ>{c},f,pool,missions)).GetEnumerator();
  Check(iterator.MoveNext()&&iterator.Current=="mission-registry"&&pool.Count==0,"global mission scan yields before processing all officers");
  int steps=1;while(iterator.MoveNext())steps++;iterator.Dispose();Check(steps>=128&&missions.Count==0,"global scan sliced and irrelevant officers excluded");
  Setup();var destination=new AFAGFHJONFJ{OJPECMNINNA="new-city",Roster=new()};destination.LMPMNLLGOEC.LMMNKIOAGLC=f.IBGCLCAIBOL;destination.LMPMNLLGOEC.IPJHOOJBCKI="gov";
  MICGPAMNDKM.IJPKEMPIFIH[destination.OJPECMNINNA]=destination;c.FKMKGNPFLFP.Add(destination);destination.FKMKGNPFLFP.Add(c);c.Roster=new();
  for(int i=0;i<10;i++){var h=new CLAGKDECFID{OJPECMNINNA="staff"+i,LMPMNLLGOEC=new(){LMMNKIOAGLC=f.IBGCLCAIBOL,CNOMNBCPJDI=c.OJPECMNINNA}};for(int k=0;k<h.Ability.Length;k++)h.Ability[k]=i<5?80:30;c.Roster.Add(h);AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;}
  c.Roster[1].OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;
  foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",f)){}
  var moved=c.Roster.Where(h=>h.MoveCalls>0).ToList();
  Check(moved.Count>=2&&c.LMPMNLLGOEC.KJPPGIOCDID.Count==0&&destination.LMPMNLLGOEC.KJPPGIOCDID.Count==0,"officer relocates with zero source and destination troops");
  Check(moved[0].OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&moved[0].LMPMNLLGOEC.CNOMNBCPJDI==c.OJPECMNINNA,"native travel begins without teleporting city membership");
  Check(string.IsNullOrEmpty(destination.LMPMNLLGOEC.BKENJPNKMGI)&&!string.IsNullOrEmpty(c.LMPMNLLGOEC.BKENJPNKMGI),"mayor is appointed locally, never before arriving");
  Check(moved[0].Ability[0]>=50&&c.Roster[1].MoveCalls==0,"capacity-independent move retains researchers and chooses capable officer");
  var arrived=moved[0];arrived.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.None;arrived.LMPMNLLGOEC.CNOMNBCPJDI=destination.OJPECMNINNA;c.Roster.Remove(arrived);destination.Roster.Add(arrived);
  foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",f)){}
  Check(destination.LMPMNLLGOEC.BKENJPNKMGI==arrived.OJPECMNINNA&&arrived.MoveCalls==1,"arrival allows mayor appointment and preserves it without moving back");
  var civilians=c.Roster.Where(h=>h.OJPECMNINNA!=c.LMPMNLLGOEC.BKENJPNKMGI&&h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.None).Take(2).ToArray();
  civilians[0].BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Manage;
  Check(!(bool)Call("AdministrativeCandidate",civilians[0],c,new List<AFAGFHJONFJ>{c,destination}),"sole civil worker retained");
  civilians[1].BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Manage;
  Check((bool)Call("AdministrativeCandidate",civilians[0],c,new List<AFAGFHJONFJ>{c,destination}),"surplus assigned civil worker can move instead of staffing deadlock");
  void BattleSetup(){typeof(BattleIntegrationTests).GetMethod("Setup",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,null);}
  object BattleField(string n)=>typeof(BattleIntegrationTests).GetField(n,BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
  BattleSetup();var supportCity=(AFAGFHJONFJ)BattleField("source");var war=(NJHDEHLHENN)BattleField("war");
  var originalArmy=new Wo_DeployTeamCtrl{m_id=777,m_belongPower="F",m_assembleCity=supportCity.OJPECMNINNA,m_warInfo=war};
  originalArmy.m_dicDeployArmy["original"]=Enumerable.Range(0,20).Select(_=>new DHKINGPDKJJ()).ToList();ICGAEMLACJG.HFJIECIBFKB[777]=originalArmy;
  Call("Assess",war,0d,true);Call("Assess",war,10d,true);
  Check(supportCity.Calls==1&&supportCity.Last.GetSoldierNum(false)==6000&&supportCity.LMPMNLLGOEC.KJPPGIOCDID.Count==4,"original engaged 20k does not require impossible 18k home reserve from a 10k garrison");
  Check(typeof(StrategicSupport).GetField("battleOriginalReserveExclusions",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null)==null,"battle reserve context restored after support generation");
  BattleSetup();supportCity=(AFAGFHJONFJ)BattleField("source");war=(NJHDEHLHENN)BattleField("war");
  var frontier=new AFAGFHJONFJ{OJPECMNINNA="other-front",Power=20000};frontier.LMPMNLLGOEC.LMMNKIOAGLC="enemy";frontier.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){CombatValue=frontier.Power});supportCity.FKMKGNPFLFP.Add(frontier);
  Call("Assess",war,0d,true);Call("Assess",war,10d,true);Check(supportCity.Calls==1&&supportCity.Last.GetSoldierNum(false)==4000,"potential nearby border army raises emergency retention to sixty percent without absolute all-neighbours veto");
  Console.WriteLine($"PASS R78 checks={checks}; capacity/repair/sliced mission scan native calls mocked, no gameplay/FPS claim.");
 }
}

public partial class CLAGKDECFID {
 public int MoveCalls;public void IFFJPNEJCIN(string target,int mode,bool immediate){MoveCalls++;OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.CityMove;OGPEJIEHDAN.MIGIFJPINLB=LMPMNLLGOEC.CNOMNBCPJDI;OGPEJIEHDAN.NHKNHHIBOGK=target;}
}

