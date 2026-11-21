using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;
public static class Audit94Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static object Fixture(Type t,string n,params object[] a)=>t.GetMethod(n,F).Invoke(null,a);
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
 static IDictionary Map(string n)=>Field<IDictionary>(typeof(StrategicSupport),n);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Audit94: "+why);}
 static IEnumerable<string> Endless(Action action){while(true){action();yield return "slice";}}
 static void Drain(string n,params object[] a){int count=0;foreach(var phase in (IEnumerable<string>)Call(n,a))if(++count>200000)throw new Exception("unbounded "+n);}
 public static void Run(){
  var q=new WorkQueue();int recruited=0,ordinary=0,urgent=0;
  for(int i=0;i<300;i++)q.Add("city:"+i,()=>Endless(()=>ordinary++).GetEnumerator());
  for(int i=0;i<100;i++)q.Add("defense:"+i,()=>Endless(()=>urgent++).GetEnumerator());
  q.Add("recruit-queue:F",()=>Endless(()=>recruited++).GetEnumerator());q.Pump(10000,400,(_,_)=>{},e=>throw e);
  Check(recruited==100&&ordinary==100&&urgent==200,"recruitment progresses under 400 simultaneous jobs, defense and cities keep fair service");
  q.Clear();Check(q.Count==0,"world change clears service queue too");
  q.Add("recruit-queue:F",()=>Endless(()=>recruited++).GetEnumerator());Check(q.Pump(0,100,(_,_)=>{},e=>throw e)==1,"service work obeys the existing frame budget");q.Clear();
  // Real 7-slot / 9-unit overflow: impossible border power may not prevent
  // saving the weakest two while retaining the strongest seven locally.
  Fixture(typeof(Logistics93Tests),"Setup");var s=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","Nanhai",0f);var d=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","receiver",10f);Fixture(typeof(Relief86Tests),"Link",s,d);Fixture(typeof(Relief86Tests),"Expose",s,1000f);
  s.Capacity=7;s.LMPMNLLGOEC.KJPPGIOCDID.Clear();d.Capacity=7;d.LMPMNLLGOEC.KJPPGIOCDID.Clear();
  for(int i=1;i<=9;i++)s.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){FGHFOLKBHDL=i*100,CombatValue=i*100});var all=s.LMPMNLLGOEC.KJPPGIOCDID.ToArray();var f=NEINKOOBJIH.COCMABJFOFH["F"];
  Check((long)Call("BorderThreat",s,f,ICGAEMLACJG.HFJIECIBFKB.Values.ToList())>all.Sum(a=>a.CombatValue),"fixture reproduces unreachable retention floor");
  Drain("CapacityTransferSteps",s,f);Check(s.LMPMNLLGOEC.KJPPGIOCDID.Count==7&&ICGAEMLACJG.KBPAJECPNOH.Count==1,"two excess units actually leave through native convoy");
  var cargo=ICGAEMLACJG.KBPAJECPNOH.Values.Single();Check(cargo.m_listArmy.Select(a=>a.Pointer).ToHashSet().SetEquals(all.Take(2).Select(a=>a.Pointer)),"weakest excess moves; strongest local defense stays");
  Check(cargo.m_listArmy.Sum(a=>a.FGHFOLKBHDL)+s.LMPMNLLGOEC.KJPPGIOCDID.Sum(a=>a.FGHFOLKBHDL)==all.Sum(a=>a.FGHFOLKBHDL),"overflow conserves every soldier");
  Drain("CapacityTransferSteps",s,f);Check(ICGAEMLACJG.KBPAJECPNOH.Count==1,"no transfer after capacity restored");
  // Accepted attack commands without any real progress must expire.
  Fixture(typeof(Intercept90Tests),"Setup");var army=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"army");Call("RecoverHostileStandoff",army);Check(army.m_deployState==AKOPKFPKJPP.Expedition,"first interception attempt starts");
  army.StartCamp();NEINKOOBJIH.MIPFHCKDJOK+=3;Call("RecoverHostileStandoff",army);army.StartCamp();NEINKOOBJIH.MIPFHCKDJOK+=3;Call("RecoverHostileStandoff",army);
  Check(!army.m_responseToPlayer&&Map("failedIntercepts").Count==1,"two accepted but motionless retries release rather than reset waiting clock forever");
  Check(Map("campaignGoals").Contains(army.Pointer),"released interceptor immediately enters support/return decision queue");
  // A full but restored receiver must release cover despite another active front.
  Fixture(typeof(Relief86Tests),"Setup");s=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","cover-A",0f);d=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","cover-B",10f);Fixture(typeof(Relief86Tests),"Link",s,d);Fixture(typeof(Relief86Tests),"Expose",s,1000f);Fixture(typeof(Relief86Tests),"Plan",s);
  var cover=Map("coverOrders").Values.Cast<object>().Single();army=d.Last;army.m_deployState=AKOPKFPKJPP.Camp;army.Position=s.BMBMKPGEHBH;s.FKMKGNPFLFP.RemoveAll(x=>x.LMPMNLLGOEC.LMMNKIOAGLC=="enemy");s.Capacity=s.LMPMNLLGOEC.KJPPGIOCDID.Count;
  Check(ICGAEMLACJG.HFJIECIBFKB.ContainsKey(1),"original front remains active");Call("CheckRegionalCover",cover);Check(!Map("coverOrders").Contains(army.Pointer),"restored full city releases backfill even while original front is active");
  Check(Map("campaignGoals").Contains(army.Pointer)&&!(bool)Map("campaignGoals")[army.Pointer].GetType().GetField("Returning").GetValue(Map("campaignGoals")[army.Pointer]),"released cover may support active front before returning");
  // StartCamp native does not clear paths; test actual postfix and loaded-state repair.
  Fixture(typeof(Campaign81Tests),"Setup");army=Field<Wo_DeployTeamCtrl>(typeof(Campaign81Tests),"t");army.m_listMovePos.Add(new(88));army.GAANFLKOMKL.Add(new(88));army.StartCamp();Check(army.m_listMovePos.Count==0&&army.GAANFLKOMKL.Count==0,"camp transition clears stale route and native waypoints");
  army.m_listMovePos.Add(new(80));Call("RepairCampMotion",army);Check(army.m_listMovePos.Count==0,"already loaded camp with residual movement is repaired");
  army.m_isPrivate=true;army.m_listMovePos.Add(new(90));Call("RepairCampMotion",army);Check(army.m_listMovePos.Count==1,"private control untouched");army.m_isPrivate=false;army.m_warInfo=new();Call("RepairCampMotion",army);Check(army.m_listMovePos.Count==1,"active combat untouched");
  army.m_warInfo=null;army.m_deployState=AKOPKFPKJPP.Expedition;Call("RepairCampMotion",army);Check(army.m_listMovePos.Count==1,"legitimate march untouched");
  // Whole-world mod hero census with a real queue, not direct synchronous Drain.
  Fixture(typeof(GovernorFeatureTests),"Setup");f=Field<MKMOJNAJKJE>(typeof(GovernorFeatureTests),"faction");var c=Field<AFAGFHJONFJ>(typeof(GovernorFeatureTests),"city");
  for(int i=0;i<6000;i++){var h=new CLAGKDECFID{OJPECMNINNA="excluded"+i,PMHBHENJPHA=KLMDJNIBCLD.Male,LMPMNLLGOEC=new(){LMMNKIOAGLC="enemy",CNOMNBCPJDI=c.OJPECMNINNA}};AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;}
  Fixture(typeof(GovernorFeatureTests),"Candidate","wanted-woman",KLMDJNIBCLD.Female,99,true);Call("ResetWork");CLAGKDECFID.PlayerAuthority94=false;StrategicSupport.StartRecruitmentQueue(30,2);
  var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");for(int i=0;i<300;i++)queue.Add("noise:"+i,()=>Endless(()=>{}).GetEnumerator());int slices=0;
  string key="local.heroesvow.governor/recruit/v1/"+f.IBGCLCAIBOL+"/pending";
  while((!MLGOLGGHLMB.CBEGDDKANAO.TryGetValue(key,out var pending)||pending==0)&&slices<16000){queue.Pump(10000,128,(_,_)=>{},e=>throw e);slices+=128;}
  Check(MLGOLGGHLMB.CBEGDDKANAO[key]==1&&slices<16000,"female native recruitment starts for ruler player under 6000 heroes and 300 concurrent city tasks");
  Check(!c.Roster.Any(h=>h.OGPEJIEHDAN.LKEPGHJDHGG?.StartsWith("excluded")==true),"female filter remains strict under load");Call("ResetWork");CLAGKDECFID.PlayerAuthority94=true;
  // A native officer arrival between pair ranking and commit used to replace
  // ah without replacing ar; the next city indexed the old ability list.
  Fixture(typeof(Relief86Tests),"Setup");ICGAEMLACJG.HFJIECIBFKB.Clear();MICGPAMNDKM.IJPKEMPIFIH.Clear();AEHAGJAKNEK.IEOHLJLICOA.Clear();
  s=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","quality-A",0f);d=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","quality-B",10f);var third=(AFAGFHJONFJ)Fixture(typeof(Relief86Tests),"City","quality-C",20f);Fixture(typeof(Relief86Tests),"Link",s,d);Fixture(typeof(Relief86Tests),"Link",s,third);f=NEINKOOBJIH.COCMABJFOFH["F"];
  foreach(var city in new[]{s,d,third})foreach(var h in city.Roster){h.Ability=Enumerable.Repeat(city==s?99d:10d,14).ToArray();AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;}
  bool changed=false;int turns=0;
  foreach(var phase in (IEnumerable<string>)Call("OfficerQualitySteps",f)){
   if(++turns>20000)throw new Exception("quality never completes");
   if(!changed&&phase=="friendly-corridor-edge"){
    changed=true;s.Roster[0].OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.CityMove;
    for(int i=0;i<5;i++){var h=new CLAGKDECFID{OJPECMNINNA="new-arrival"+i,Ability=Enumerable.Repeat(90d,14).ToArray(),LMPMNLLGOEC=new(){LMMNKIOAGLC="F",CNOMNBCPJDI=s.OJPECMNINNA}};s.Roster.Add(h);AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;}
   }
  }
  Check(changed,"quality fixture mutates roster after ranking and before native commit");Check(turns<20000,"roster arrival with invalidated chosen officer does not index mismatched lists");
  var aa=Enumerable.Range(0,12).Select(i=>new OfficerBalancePolicy.Officer("a"+i,Enumerable.Repeat(70d+i,14).ToArray())).ToList();var bb=Enumerable.Range(0,12).Select(i=>new OfficerBalancePolicy.Officer("b"+i,Enumerable.Repeat(10d+i,14).ToArray())).ToList();var ap=new OfficerBalancePolicy.Profile(aa,14);var bp=new OfficerBalancePolicy.Profile(bb,14);
  Check(aa.All(a=>bb.All(b=>OfficerBalancePolicy.ExchangeGain(aa,bb,a,b)==OfficerBalancePolicy.ExchangeGain(ap,bp,a,b))),"cached profiles preserve all pair decisions exactly");
  Call("ResetWork");
  Console.WriteLine("PASS Audit94 "+checks+" overflow conservation, queue load, camp motion and no-progress lifecycle checks; native calls mocked, not gameplay.");
 }
}
