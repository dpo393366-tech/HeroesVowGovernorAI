using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 const string RecruitPrefix="local.heroesvow.governor/recruit/v1/";
 static string PlayerRecruitKey()=>RecruitPrefix+(NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC??"")+"/";
 static int QueueRead(string key)=>MLGOLGGHLMB.CBEGDDKANAO!=null&&MLGOLGGHLMB.CBEGDDKANAO.TryGetValue(key,out int value)?value:0;
 static void QueueWrite(string key,int value)=>MLGOLGGHLMB.CBEGDDKANAO[key]=value;
 // Native saved integer flags bind this queue to the loaded save, not a global
 // config file. Targets and envoy IDs are encoded in keys without pointer IDs.
 static void ScheduleRecruitment(MKMOJNAJKJE f){
  if(f==null||f.IBGCLCAIBOL!=NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC||MLGOLGGHLMB.CBEGDDKANAO==null)return;
  string k=RecruitPrefix+f.IBGCLCAIBOL+"/",task="recruit-queue:"+f.IBGCLCAIBOL;
  EnsureRecruitList(k);if(!ActivateRecruitHead(k))return;
  if(QueueRead(k+"generation")<=0||(QueueRead(k+"enabled")!=1&&QueueRead(k+"pending")==0&&QueueRead(RecruitItem(k,QueueRead(k+"queue/active"))+"status")!=RecruitClosing))return;
  if(DecisionDue(task,f.IBGCLCAIBOL,f.Pointer,"",false)&&work.Add(task,()=>SafeMissionPhases(RecruitmentQueueSteps(f)).GetEnumerator())){
   DecisionDue(task,f.IBGCLCAIBOL,f.Pointer,"",true);Note($"recruit-queue-scheduled faction={f.IBGCLCAIBOL} goal={QueueRead(k+"goal")} gender={QueueRead(k+"gender")} day={NEINKOOBJIH.MIPFHCKDJOK}");
  }
 }
 static bool RecruitJob(GCPCLLMKIMM job)=>job==GCPCLLMKIMM.Recruit||job==GCPCLLMKIMM.StartRecruit||job==GCPCLLMKIMM.EndRecruit;
 static bool RecruitGender(CLAGKDECFID h,int filter)=>filter==0||filter==1&&h.PMHBHENJPHA==KLMDJNIBCLD.Male||filter==2&&h.PMHBHENJPHA==KLMDJNIBCLD.Female;
 static AFAGFHJONFJ CaptiveLocation(MKMOJNAJKJE f,CLAGKDECFID h){
  var cities=f.PJMLPLDPEIM();if(cities!=null)foreach(var c in cities)if(c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&c.JHNBHLPJKMO!=null)foreach(var p in c.JHNBHLPJKMO)if(p?.OJPECMNINNA==h.OJPECMNINNA)return c;return null;
 }
 static bool RecruitEligible(MKMOJNAJKJE f,CLAGKDECFID h)=>h?.LMPMNLLGOEC!=null&&h.LMPMNLLGOEC.LMMNKIOAGLC!=f.IBGCLCAIBOL&&(int)h.HFBAJIGHOAF<=1
  &&(h.GMILGFFJOIL?CaptiveLocation(f,h)!=null:f.OJFOLKOFMCE(h,false));
 static IEnumerable<string> RecruitmentActiveSteps(MKMOJNAJKJE f){
  if(f.IBGCLCAIBOL!=NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC||MLGOLGGHLMB.CBEGDDKANAO==null)yield break;
  string k=RecruitPrefix+f.IBGCLCAIBOL+"/";int gen=QueueRead(k+"generation"),goal=QueueRead(k+"goal");if(gen<=0||goal<=0)yield break;
  var save=MLGOLGGHLMB.CBEGDDKANAO;bool Valid()=>CurrentFaction(f)&&f.IBGCLCAIBOL==NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC&&save.Pointer==MLGOLGGHLMB.CBEGDDKANAO?.Pointer&&QueueRead(k+"generation")==gen;
  long startedAt=Environment.TickCount64;
  Note($"recruit-queue-start faction={f.IBGCLCAIBOL} generation={gen} goal={goal} gender={QueueRead(k+"gender")} day={NEINKOOBJIH.MIPFHCKDJOK}");
  var roster=new Dictionary<string,CLAGKDECFID>();int probes=0;
  foreach(var h in OfficerRegistrySnapshot()){if(++probes%8==0){yield return "recruit-roster";if(!Valid())yield break;}if(h!=null&&!string.IsNullOrEmpty(h.OJPECMNINNA))roster[h.OJPECMNINNA]=h;}
  string prefix=k+"mission/";var records=new List<string>();foreach(var item in save)if(item.Key.StartsWith(prefix,StringComparison.Ordinal)&&Math.Abs((long)item.Value)==gen)records.Add(item.Key);
  int done=0,pending=0;var claimed=new HashSet<string>();
  foreach(string record in records){
   yield return "recruit-result";if(!Valid())yield break;var parts=record.Substring(prefix.Length).Split('/');if(parts.Length!=2)continue;
   string target=Uri.UnescapeDataString(parts[0]),actor=Uri.UnescapeDataString(parts[1]);int state=QueueRead(record);if(state== -gen){done++;claimed.Add(target);continue;}
   roster.TryGetValue(target,out var h);roster.TryGetValue(actor,out var envoy);
   if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!h.GMILGFFJOIL){QueueWrite(record,-gen);done++;claimed.Add(target);Note($"recruit-queue-joined faction={f.IBGCLCAIBOL} officer={target} completed={done} goal={goal}");}
   else if(envoy?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&envoy.OGPEJIEHDAN!=null&&RecruitJob(envoy.OGPEJIEHDAN.HFBAJIGHOAF)&&envoy.OGPEJIEHDAN.LKEPGHJDHGG==target){pending++;claimed.Add(target);}
   else {QueueWrite(record,0);QueueWrite(k+"retry/"+target,(int)NEINKOOBJIH.MIPFHCKDJOK+7);Note($"recruit-queue-failed faction={f.IBGCLCAIBOL} officer={target} retryDays=7");}
  }
  QueueWrite(k+"done",done);QueueWrite(k+"pending",pending);
  if(done>=goal){
   QueueWrite(k+"enabled",0);
   if(QueueRead(k+"notified")!=gen&&QueueRead(RecruitItem(k,gen)+"status")!=RecruitClosing){QueueWrite(k+"notified",gen);UI_NotifyUI.OpenUI($"자동 등용 완료: 요청한 {goal}명의 장수가 합류했습니다.",true,NEINKOOBJIH.POMNMBGDDIN,false,false,null);}
   yield break;
  }
  if(QueueRead(k+"enabled")!=1||done+pending>=goal)yield break;
  var cities=StateCities(f);var prisoners=new HashSet<string>();foreach(var c in cities)if(c.JHNBHLPJKMO!=null)foreach(var h in c.JHNBHLPJKMO)if(h!=null)prisoners.Add(h.OJPECMNINNA);
  var targeted=new HashSet<string>();foreach(var h in roster.Values)if(h.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&h.OGPEJIEHDAN!=null&&RecruitJob(h.OGPEJIEHDAN.HFBAJIGHOAF))targeted.Add(h.OGPEJIEHDAN.LKEPGHJDHGG??"");
  int gender=QueueRead(k+"gender");var choices=new List<(CLAGKDECFID Hero,double Score)>();
  foreach(var h in roster.Values){
   if(++probes%8==0){yield return "recruit-candidate";if(!Valid())yield break;}
   if(h.LMPMNLLGOEC==null||(int)h.HFBAJIGHOAF>1||h.OJPECMNINNA==NEINKOOBJIH.OPIKIHPNCIP||claimed.Contains(h.OJPECMNINNA)||targeted.Contains(h.OJPECMNINNA)||!RecruitGender(h,gender))continue;
   if(h.LMPMNLLGOEC.LMMNKIOAGLC==f.IBGCLCAIBOL||!string.IsNullOrEmpty(h.LMPMNLLGOEC.LMMNKIOAGLC)&&!prisoners.Contains(h.OJPECMNINNA))continue;
   if(QueueRead(k+"retry/"+h.OJPECMNINNA)>NEINKOOBJIH.MIPFHCKDJOK||!RecruitEligible(f,h))continue;
   choices.Add((h,OfficerDeploymentPolicy.Talent(ReadOfficer(h),3)));
  }
  Note($"recruit-queue-candidates faction={f.IBGCLCAIBOL} roster={roster.Count} candidates={choices.Count} captives={choices.Count(x=>x.Hero.GMILGFFJOIL)} elapsedMs={Environment.TickCount64-startedAt}");
  var used=new HashSet<string>();var homes=new Dictionary<string,List<CLAGKDECFID>>();var byCity=new Dictionary<string,AFAGFHJONFJ>();int blocked=0;
  foreach(var c in cities){homes[c.OJPECMNINNA]=new();byCity[c.OJPECMNINNA]=c;}
  // Previously cities * entire officer registry ran in one MoveNext (217ms
  // observed). Index once, with bounded slices and one eligibility read.
  foreach(var h in roster.Values){
   if(++probes%8==0){yield return "recruit-home-index";if(!Valid())yield break;}
   string city=h.LMPMNLLGOEC?.CNOMNBCPJDI;if(city!=null&&byCity.TryGetValue(city,out var c)&&RelocationHome(h,c))homes[city].Add(h);
  }
  // BBGAPBMGDPL is a player/current-city authority predicate, not envoy
  // availability (verified native reads of player city/governor/ruler).
  // Use the actual owning faction, job, protected duties and road instead.
  var envoys=new List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>();
  foreach(var c in cities){
   yield return "recruit-envoy-pool";if(!Valid())yield break;
   var home=homes[c.OJPECMNINNA];if(home.Count<4||IncomingHostile(c,f))continue;
   foreach(var h in home){
    if(++probes%8==0){yield return "recruit-envoy-pool";if(!Valid())yield break;}
    if(h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None&&AdministrativeCandidateIndexed(h,c,cities,home))envoys.Add((c,h));
   }
  }
  Note($"recruit-queue-envoys faction={f.IBGCLCAIBOL} eligible={envoys.Count} cities={cities.Count}");
  foreach(var choice in choices.OrderByDescending(x=>x.Score).ThenBy(x=>x.Hero.OJPECMNINNA,StringComparer.Ordinal)){
   if(done+pending>=goal||envoys.All(e=>used.Contains(e.Hero.OJPECMNINNA)||e.Hero.OGPEJIEHDAN?.HFBAJIGHOAF!=GCPCLLMKIMM.None))break;
   (AFAGFHJONFJ City,CLAGKDECFID Hero) best=default;double score=double.NegativeInfinity;
   foreach(var entry in envoys){
    yield return "recruit-envoy";if(!Valid())yield break;
    var c=entry.City;var h=entry.Hero;
    if(used.Contains(h.OJPECMNINNA)||h.OGPEJIEHDAN?.HFBAJIGHOAF!=GCPCLLMKIMM.None||!RelocationHome(h,c)||IncomingHostile(c,f))continue;
    var home=homes[c.OJPECMNINNA].Where(x=>RelocationHome(x,c)).ToList();
    if(home.Count<4||!AdministrativeCandidateIndexed(h,c,cities,home))continue;
    string destination=choice.Hero.GMILGFFJOIL?CaptiveLocation(f,choice.Hero)?.OJPECMNINNA:choice.Hero.LMPMNLLGOEC.CNOMNBCPJDI;
    if(string.IsNullOrEmpty(destination))continue;
    int days=destination==c.OJPECMNINNA?0:h.NBAPOJABAAO(destination,false);if(days<0||days==0&&destination!=c.OJPECMNINNA)continue;
    double value=h.DHOMPHMNCJB()/(1+days/30d);if(value>score){score=value;best=(c,h);}
   }
   if(best.Hero==null){blocked++;if(blocked<=3)Note($"recruit-queue-block target={choice.Hero.OJPECMNINNA} reason=no-idle-eligible-envoy-or-road cities={cities.Count}");continue;}
   if(!Valid()||QueueRead(k+"enabled")!=1||RecruitTargetBusy(f,choice.Hero.OJPECMNINNA)||!RecruitEligible(f,choice.Hero)||!RecruitGender(choice.Hero,gender)||best.Hero.OGPEJIEHDAN?.HFBAJIGHOAF!=GCPCLLMKIMM.None||!AdministrativeCandidateIndexed(best.Hero,best.City,cities,homes[best.City.OJPECMNINNA].Where(h=>RelocationHome(h,best.City)).ToList()))continue;
   string record=prefix+Uri.EscapeDataString(choice.Hero.OJPECMNINNA)+"/"+Uri.EscapeDataString(best.Hero.OJPECMNINNA);QueueWrite(record,gen);QueueWrite(k+"pending",pending+1); // reserve before native callbacks
   best.Hero.BBJBFPDCBJG(choice.Hero.OJPECMNINNA,false,false,false);
   bool started=RecruitJob(best.Hero.OGPEJIEHDAN.HFBAJIGHOAF)&&best.Hero.OGPEJIEHDAN.LKEPGHJDHGG==choice.Hero.OJPECMNINNA;
   if(choice.Hero.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!choice.Hero.GMILGFFJOIL){QueueWrite(record,-gen);done++;QueueWrite(k+"done",done);QueueWrite(k+"pending",pending);used.Add(best.Hero.OJPECMNINNA);}
   else if(started){pending++;QueueWrite(k+"pending",pending);used.Add(best.Hero.OJPECMNINNA);}else{QueueWrite(record,0);QueueWrite(k+"pending",pending);QueueWrite(k+"retry/"+choice.Hero.OJPECMNINNA,(int)NEINKOOBJIH.MIPFHCKDJOK+3);}
   Note($"recruit-queue-order target={choice.Hero.OJPECMNINNA} envoy={best.Hero.OJPECMNINNA} talent={choice.Score:F1} captive={prisoners.Contains(choice.Hero.OJPECMNINNA)} started={started} completed={done} pending={pending} goal={goal}");
  }
  Note($"recruit-queue-assess faction={f.IBGCLCAIBOL} candidates={choices.Count} noEnvoyOrRoute={blocked} completed={done} pending={pending} goal={goal} elapsedMs={Environment.TickCount64-startedAt}");
 }
}
