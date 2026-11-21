using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
public static class Perf83Tests {
 static int checks;
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("R83: "+why);}
 static object Call(string name,params object[] a)=>typeof(StrategicSupport).GetMethod(name,BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,a);
 static T Field<T>(Type t,string name)=>(T)t.GetField(name,BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
 static IEnumerator<string> Steps(List<string> events,string name,int count){for(int i=0;i<count;i++){events.Add(name);yield return "step";}}
 static void Setup(int n=80){typeof(City80Tests).GetMethod("Setup",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{new[]{n,0}});Call("ResetWork");}
 public static void Run(){
  var q=new WorkQueue();var events=new List<string>();
  for(int i=0;i<130;i++)q.Add("city:"+i,()=>Steps(events,"normal",4));
  q.Add("defense:F",()=>Steps(events,"defense",600));
  for(int i=0;i<30;i++)q.Pump(100,1,(_,_)=>{},e=>throw e);
  Check(events.Take(3).SequenceEqual(new[]{"defense","defense","normal"}),"defense starts ahead of daily backlog and yields a normal turn");
  Check(events.Count(x=>x=="normal")==10&&events.Count(x=>x=="defense")==20,"bounded priority does not starve economy");
  int guard=0;while(q.Count>0&&guard++<10000)q.Pump(100,32,(_,_)=>{},e=>throw e);
  Check(q.Count==0&&events.Count==1120,"priority drains all jobs with no lost work");
  Setup();var f=Field<MKMOJNAJKJE>(typeof(City80Tests),"f");var cities=Field<List<AFAGFHJONFJ>>(typeof(City80Tests),"cities");
  var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");
  NEINKOOBJIH.MIPFHCKDJOK=100;Call("ScheduleCity",cities[0]);Check(queue.Count==1,"first daily admission");queue.Clear();
  Call("ScheduleCity",cities[0]);Check(queue.Count==0,"native callback cannot repeat completed same-day city scan");
  NEINKOOBJIH.MIPFHCKDJOK=100+(int)Call("DecisionDays",cities[0].LMPMNLLGOEC.LMMNKIOAGLC);Call("ScheduleCity",cities[0]);Check(queue.Count==1,"next configured decision day still evaluates");queue.Clear();
  cities[0].LMPMNLLGOEC.IPJHOOJBCKI="new-governor";Call("ScheduleCity",cities[0]);Check(queue.Count==1,"governor change bypasses admission stamp");Call("ResetWork");
  Setup();f=Field<MKMOJNAJKJE>(typeof(City80Tests),"f");cities=Field<List<AFAGFHJONFJ>>(typeof(City80Tests),"cities");
  var phases=new Dictionary<string,int>();int steps=0;
  foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",f)){phases.TryGetValue(phase,out var n);phases[phase]=n+1;if(++steps>=20000)throw new Exception("staffing did not terminate");}
  Check(steps<20000,"bounded staffing completes");
  Check(phases.GetValueOrDefault("relocation-revalidate")>=400,"each transfer refreshes faction arrivals in eight-officer slices");
  Check(AEHAGJAKNEK.IEOHLJLICOA.Values.Sum(h=>h.MoveCalls)==40,"slicing preserves bulk forty-officer redistribution");
  Setup(40);f=Field<MKMOJNAJKJE>(typeof(City80Tests),"f");cities=Field<List<AFAGFHJONFJ>>(typeof(City80Tests),"cities");
  bool attacked=false;foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",f))if(!attacked&&phase=="relocation-validation"){
   attacked=true;f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};ICGAEMLACJG.HFJIECIBFKB[17]=new(){m_id=17,m_belongPower="enemy",m_targetCity=cities[0].OJPECMNINNA};
  }
  Check(attacked&&AEHAGJAKNEK.IEOHLJLICOA.Values.All(h=>h.MoveCalls==0),"attack between scan and command prevents stale transfer");
  Setup(80);f=Field<MKMOJNAJKJE>(typeof(City80Tests),"f");cities=Field<List<AFAGFHJONFJ>>(typeof(City80Tests),"cities");
  var needs=new StatecraftPolicy.Needs();int yields=0;foreach(var phase in (IEnumerable<bool>)Call("StateNeedsSteps",cities,f,needs))yields++;
  Check(needs.Officers==80&&yields>=14,"needs census yields without losing officers");
  Call("ResetDefense");Call("ResetWork");foreach(var _ in (IEnumerable<string>)Call("DefensePulse")){};
  var again=((IEnumerable<string>)Call("DefensePulse")).ToArray();Check(!again.Contains("daily-city"),"two-second watch skips unchanged daily city census");Call("ResetWork");
  foreach(bool changeRelation in new[]{false,true}){
   typeof(StatecraftTests).GetMethod("Setup",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,null);
   f=Field<MKMOJNAJKJE>(typeof(StatecraftTests),"f");cities=Field<List<AFAGFHJONFJ>>(typeof(StatecraftTests),"cities");
   var enemy=new MKMOJNAJKJE{IBGCLCAIBOL="enemy"};NEINKOOBJIH.COCMABJFOFH["enemy"]=enemy;f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};
   var ec=new AFAGFHJONFJ{OJPECMNINNA="enemyCity",Power=500};ec.LMPMNLLGOEC.LMMNKIOAGLC="enemy";MICGPAMNDKM.IJPKEMPIFIH[ec.OJPECMNINNA]=ec;
   var pool=(List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>)Call("StateOfficers",cities,f);int envoySteps=0;bool commit=false;
   foreach(var phase in (IEnumerable<string>)Call("DiplomacySteps",f,cities,pool,new List<CLAGKDECFID>())){
    if(phase=="diplomacy-envoy")envoySteps++;
    if(changeRelation&&phase=="diplomacy-validate")f.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.Ally;
    commit|=phase=="diplomacy-commit";
   }
   Check(envoySteps==24,"diplomacy candidate routes processed separately");
   Check(commit!=changeRelation&&cities[0].Roster.Sum(h=>h.MissionCalls)==(changeRelation?0:1),"diplomacy revalidates relationship before native command");
  }
  Setup(40);f=Field<MKMOJNAJKJE>(typeof(City80Tests),"f");cities=Field<List<AFAGFHJONFJ>>(typeof(City80Tests),"cities");
  bool removed=false;foreach(var phase in (IEnumerable<string>)Call("AdministrativeStaffingSteps",f))if(!removed&&phase=="relocation-validation"){
   removed=true;foreach(var h in cities[0].Roster.Skip(3))h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;
   foreach(var h in cities[0].Roster.Skip(3))h.LMPMNLLGOEC.CNOMNBCPJDI="elsewhere";
  }
  Check(removed&&AEHAGJAKNEK.IEOHLJLICOA.Values.All(h=>h.MoveCalls==0),"source roster changed after sliced refresh cannot empty garrison staff");
  Call("ResetWork");
  Console.WriteLine($"PASS R83 performance/scheduling checks={checks}; native calls stubbed, not an FPS measurement");
 }
}
