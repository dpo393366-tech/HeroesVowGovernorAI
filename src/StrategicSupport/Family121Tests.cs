using System;using System.Collections.Generic;using System.Reflection;using System.Linq;
public partial class AFAGFHJONFJ {public List<DHKINGPDKJJ> CHCFKBDFGGD=new();}
public partial class DHKINGPDKJJ {public bool LBMKIPABHME=true;public int IKNJJGAHFGL,EDIMNMBBCFM;}
public static partial class EBNFFALGBPA {public static int JPNKCMJHIAP=100,KMHPGABPIME=100;}
public class UI_HeroListInfoUI {public void RowPress(int a,int b,int c,int d){}}
public static class Family121Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;static AFAGFHJONFJ city,dest;static CLAGKDECFID h;static DHKINGPDKJJ troop;static MKMOJNAJKJE faction;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Family121: "+why);}
 static void Setup(){
  typeof(City80Tests).GetMethod("Setup",F).Invoke(null,new object[]{new[]{21,5}});
  var cities=(List<AFAGFHJONFJ>)typeof(City80Tests).GetField("cities",F).GetValue(null);city=cities[0];dest=cities[1];h=city.Roster[1];faction=NEINKOOBJIH.COCMABJFOFH["F"];
  NEINKOOBJIH.POMNMBGDDIN=new CLAGKDECFID{OJPECMNINNA="player",LMPMNLLGOEC=new(){LMMNKIOAGLC="F",CNOMNBCPJDI=city.OJPECMNINNA}};
  NEINKOOBJIH.JMIADAFPGMC.Add(h.OJPECMNINNA);h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.PrivateMilitary;
  troop=new(){OLMAJLLIJDM=h.OJPECMNINNA,FGHFOLKBHDL=EBNFFALGBPA.LLLBAAAEIBB,GDHPLIFJCBN=PMNFKDJNLDJ.Train};city.CHCFKBDFGGD.Add(troop);
 }
 static bool Protected()=>(bool)Call("FamilyRelocationProtected",h);
 static bool Guard()=>(bool)Call("GuardFamilyRelocation",h,dest.OJPECMNINNA);
 static bool Candidate()=>(bool)Call("AdministrativeCandidate",h,city,new List<AFAGFHJONFJ>{city,dest});
 static void Finish(){troop.FGHFOLKBHDL=EBNFFALGBPA.LLLBAAAEIBB;troop.IKNJJGAHFGL=EBNFFALGBPA.JPNKCMJHIAP;troop.EDIMNMBBCFM=EBNFFALGBPA.KMHPGABPIME;}
 public static void Run(){
  Setup();Check(StrategicSupport.ProtectFamilyPrivateTraining.Value&&!StrategicSupport.ExcludeFamilyAutoRelocation.Value,"independent default protection options");
  Check(Protected()&&!Candidate()&&!Guard(),"ordinary retainer household trainer excluded from planner and native city move");
  Check((bool)Call("RelocationHome",h,city)&&Call("AssignedOfficerCity",h,faction)?.ToString()==city.OJPECMNINNA,"protected family still counted in actual city staffing");
  Check(h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.PrivateMilitary&&troop.OLMAJLLIJDM==h.OJPECMNINNA,"blocked move never clears private training assignment");
  Finish();Check(!Protected()&&Candidate()&&Guard(),"completion immediately releases protection even if native PrivateMilitary job label has not cleared");
  troop.FGHFOLKBHDL--;Check(Protected(),"recruitment still unfinished after training completion remains protected");
  Finish();troop.EDIMNMBBCFM--;Check(Protected(),"second training stat still incomplete remains protected");
  Finish();StrategicSupport.ExcludeFamilyAutoRelocation.Value=true;Check(Protected()&&!Guard(),"whole household option retains protection after completion");
  StrategicSupport.ExcludeFamilyAutoRelocation.Value=false;Check(!Protected()&&Guard(),"whole household off immediately releases finished officer");
  Setup();StrategicSupport.ProtectFamilyPrivateTraining.Value=false;Check(!Protected()&&Candidate()&&Guard(),"trainer protection can be disabled independently");
  StrategicSupport.ExcludeFamilyAutoRelocation.Value=true;Check(Protected(),"whole household remains effective with training option off");
  Setup();NEINKOOBJIH.JMIADAFPGMC.Clear();Check(!Protected()&&Candidate()&&Guard(),"non-household NPC private trainer unaffected");
  Setup();h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.None;Check(Protected()&&!Guard(),"actual troop assignment protects transient None job during trainer refresh");
  h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.PrivateMilitary;troop.OLMAJLLIJDM="other";Check(!Protected(),"stale PrivateMilitary label without assigned troop is not indefinite protection");
  Setup();troop.GDHPLIFJCBN=PMNFKDJNLDJ.Deploy;Check(!Protected(),"deployed private army does not count as local training");
  Setup();troop.LBMKIPABHME=false;troop.MJNINDNBJHA=false;Check(!Protected(),"both private growth tasks disabled releases relocation");
  Setup();object[] scope={0};Call("BeginManualInput",scope);try{Check(Guard(),"direct player move allowed despite protection");Call("BeginManualGovernor");try{Check(!Guard(),"governor called inside UI still uses automatic protection");}finally{Call("EndManualGovernor");}}finally{Call("EndManualInput",scope[0]);}
  Setup();dest.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(Guard(),"capture or faction transfer not treated as same-faction relocation");
  Setup();h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.CityMove;Check(Guard(),"already travelling order not cancelled or teleported");
  Setup();foreach(var member in city.Roster.Take(5)){NEINKOOBJIH.JMIADAFPGMC.Add(member.OJPECMNINNA);member.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.PrivateMilitary;city.CHCFKBDFGGD.Add(new(){OLMAJLLIJDM=member.OJPECMNINNA,GDHPLIFJCBN=PMNFKDJNLDJ.Train});}
  foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",faction)){}
  Check(city.Roster.Take(5).All(x=>x.MoveCalls==0),"five household trainers survive real sliced staffing batch");Check(city.Roster.Skip(5).Any(x=>x.MoveCalls>0),"other officers still move to address imbalance");
  Setup();var iterator=((IEnumerable<string>)Call("AdministrativeStaffingSteps",faction)).GetEnumerator();bool changed=false;while(iterator.MoveNext()){if(iterator.Current=="relocation-validation"){StrategicSupport.ExcludeFamilyAutoRelocation.Value=true;NEINKOOBJIH.JMIADAFPGMC.Clear();foreach(var member in city.Roster)NEINKOOBJIH.JMIADAFPGMC.Add(member.OJPECMNINNA);changed=true;}}iterator.Dispose();Check(changed&&city.Roster.All(x=>x.MoveCalls==0),"option change during sliced planning revalidated before movement commit");
  StrategicSupport.ExcludeFamilyAutoRelocation.Value=false;StrategicSupport.ProtectFamilyPrivateTraining.Value=true;NEINKOOBJIH.JMIADAFPGMC.Clear();Call("ResetWork");
  Console.WriteLine("PASS Family121 "+checks+" household membership, actual unfinished training, automatic release, native transfer guard, manual commands, batch staffing and option-race checks; native actions mocked.");
 }
}
