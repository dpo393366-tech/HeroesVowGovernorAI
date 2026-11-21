using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Il2CppInterop.Runtime.InteropTypes.Arrays {
 public class Il2CppReferenceArray<T>:IEnumerable<T> {
  readonly T[] items;public Il2CppReferenceArray(int n){items=new T[n];}public int Length=>items.Length;
  public T this[int i]{get=>items[i];set=>items[i]=value;}
  public static implicit operator T[](Il2CppReferenceArray<T> a)=>a.items;
  public IEnumerator<T> GetEnumerator()=>((IEnumerable<T>)items).GetEnumerator();IEnumerator IEnumerable.GetEnumerator()=>GetEnumerator();
 }
}
public static class City80Tests {
 static int checks;static MKMOJNAJKJE f;static List<AFAGFHJONFJ> cities;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("R80: "+why);}
 static void Setup(params int[] counts){
  new StrategicSupport().Load();MICGPAMNDKM.IJPKEMPIFIH=new();NEINKOOBJIH.COCMABJFOFH.Clear();ICGAEMLACJG.HFJIECIBFKB.Clear();AEHAGJAKNEK.IEOHLJLICOA.Clear();NEINKOOBJIH.AOPJBNBGPLP.Clear();NEINKOOBJIH.JMIADAFPGMC.Clear();
  f=new(){IBGCLCAIBOL="F"};f.ANMMMOIKLEG["gov"]=new(){LMMNKIOAGLC="F"};NEINKOOBJIH.COCMABJFOFH["F"]=f;cities=new();
  for(int i=0;i<counts.Length;i++){var c=new AFAGFHJONFJ{OJPECMNINNA="city"+i,Roster=new()};c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="gov";cities.Add(c);MICGPAMNDKM.IJPKEMPIFIH[c.OJPECMNINNA]=c;
   for(int j=0;j<counts[i];j++){var h=new CLAGKDECFID{OJPECMNINNA="h"+i+"_"+j,LMPMNLLGOEC=new(){CNOMNBCPJDI=c.OJPECMNINNA,LMMNKIOAGLC="F"}};c.Roster.Add(h);AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;}
  }
  foreach(var c in cities)foreach(var other in cities)if(other!=c)c.FKMKGNPFLFP.Add(other);Call("Ready");
 }
 static IEnumerable<CLAGKDECFID> Heroes=>AEHAGJAKNEK.IEOHLJLICOA.Values;
 static IEnumerable<CLAGKDECFID> At(AFAGFHJONFJ c)=>Heroes.Where(h=>h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.CityMove?h.OGPEJIEHDAN.NHKNHHIBOGK==c.OJPECMNINNA:h.LMPMNLLGOEC.CNOMNBCPJDI==c.OJPECMNINNA);
 static void RunStaff(){foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",f)){} }
 public static void Run(){
  Setup(80,0);RunStaff();Check(At(cities[0]).Count()==40&&At(cities[1]).Count()==40,"80/0 converges to projected40/40 in one assessment, exceeding old12 cap");
  Check(Heroes.Sum(h=>h.MoveCalls)==40&&cities.All(c=>c.LMPMNLLGOEC.KJPPGIOCDID.Count==0),"forty independent native commands require no soldiers");RunStaff();Check(Heroes.Sum(h=>h.MoveCalls)==40,"incoming40 blocks duplicate movement before arrival");
  Setup(60,0,0);RunStaff();Check(cities.All(c=>At(c).Count()==20),"multiple recipients each filled20 without concentrating on first city");
  Setup(80,0);for(int i=0;i<80;i++)cities[0].Roster[i].Ability=Enumerable.Repeat(i<40?90d:30d,14).ToArray();RunStaff();
  for(int d=0;d<14;d++)Check(Math.Abs(At(cities[0]).Average(h=>h.Ability[d])-At(cities[1]).Average(h=>h.Ability[d]))<=3,"batch balances each ability dimension, not just counts");
  Setup(40,0);var onlyExpert=cities[0].Roster[0];onlyExpert.Ability[6]=1000;RunStaff();Check(onlyExpert.MoveCalls==0,"unique outstanding specialist retained across the entire batch");
  Setup(30,0);var research=cities[0].Roster[1];research.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;var companion=cities[0].Roster[2];NEINKOOBJIH.AOPJBNBGPLP.Add(companion.OJPECMNINNA);var worker=cities[0].Roster[3];worker.BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Manage;
  RunStaff();Check(research.MoveCalls==0&&companion.MoveCalls==0&&worker.MoveCalls==0,"research, actual party and sole civil worker preserved during bulk relocation");
  Setup(1,0);var solo=cities[0].Roster[0];solo.BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Military;RunStaff();Check(cities[0].LMPMNLLGOEC.BKENJPNKMGI==solo.OJPECMNINNA&&solo.MoveCalls==0,"sole local military worker can become mayor without being moved away");
  Setup(40,0);f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};ICGAEMLACJG.HFJIECIBFKB[1]=new(){m_id=1,m_belongPower="enemy",m_targetCity=cities[0].OJPECMNINNA};RunStaff();Check(Heroes.All(h=>h.MoveCalls==0),"attacked source retains officers despite large imbalance");
  Setup(20,0);var scan=((IEnumerable<string>)Call("AdministrativeStaffingSteps",f)).GetEnumerator();bool registryMutated=false,poolMutated=false;
  while(scan.MoveNext()){
   if(scan.Current=="staffing-roster"&&!registryMutated){AEHAGJAKNEK.IEOHLJLICOA["new-neutral"]=new(){OJPECMNINNA="new-neutral",LMPMNLLGOEC=new(){LMMNKIOAGLC="other"}};registryMutated=true;}
   if(scan.Current=="relocation-candidate"&&!poolMutated){cities[0].LastNativePool.Clear();poolMutated=true;}
  }scan.Dispose();Check(registryMutated&&poolMutated&&Heroes.Any(h=>h.MoveCalls>0),"registry and shared native candidate list may mutate between yielded frames without aborting staffing");
  Setup(30,0);var missions=new List<CLAGKDECFID>();var pool=new List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>();scan=((IEnumerable<string>)Call("StateOfficerSteps",cities,f,pool,missions)).GetEnumerator();bool homeMutated=false;registryMutated=false;
  while(scan.MoveNext()){
   if(scan.Current=="mission-registry"&&!registryMutated){AEHAGJAKNEK.IEOHLJLICOA["new-neutral"]=new(){OJPECMNINNA="new-neutral",LMPMNLLGOEC=new(){LMMNKIOAGLC="other"}};registryMutated=true;}
   if(scan.Current=="mission-home"&&!homeMutated){cities[0].Roster.RemoveAt(cities[0].Roster.Count-1);homeMutated=true;}
  }scan.Dispose();Check(homeMutated&&registryMutated,"mission scan tolerates registry and local roster changes across frames");
  Setup(30,0);scan=((IEnumerable<string>)Call("AdministrativeStaffingSteps",f)).GetEnumerator();while(scan.MoveNext()){if(scan.Current=="relocation-commit"){f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};ICGAEMLACJG.HFJIECIBFKB[1]=new(){m_id=1,m_belongPower="enemy",m_targetCity=cities[0].OJPECMNINNA};}}
  scan.Dispose();Check(Heroes.Sum(h=>h.MoveCalls)==1,"new attack halfway through batch stops further transfers");
  Console.WriteLine($"PASS R80 {checks} checks; bulk staffing/mutable roster replay native calls mocked, not gameplay or FPS proof.");
 }
}
