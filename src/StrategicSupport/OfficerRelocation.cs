using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 static void ScheduleStaffing(MKMOJNAJKJE f){
  ScheduleRecruitment(f);ScheduleDiplomaticWork(f);
  foreach(string stage in new[]{"occupation","quality","food"}){
   string task="staffing-"+stage+":"+f.IBGCLCAIBOL;
   if(DecisionDue(task,f.IBGCLCAIBOL,f.Pointer,"",false)&&work.Add(task,()=>SafePhases(stage=="occupation"?OccupationReliefSteps(f):stage=="food"?CityFoodBalanceSteps(f):OfficerQualitySteps(f)).GetEnumerator()))DecisionDue(task,f.IBGCLCAIBOL,f.Pointer,"",true);
  }
  string key=(DecisionDays(f.IBGCLCAIBOL)==1?"staffing-urgent:":"staffing:")+f.IBGCLCAIBOL;
  if(!DecisionDue(key,f.IBGCLCAIBOL,f.Pointer,"",false))return;
  double day=NEINKOOBJIH.MIPFHCKDJOK;long at=Environment.TickCount64;
  if(work.Add(key,()=>StaffingWork(f,day,at).GetEnumerator()))DecisionDue(key,f.IBGCLCAIBOL,f.Pointer,"",true);
 }
 static IEnumerable<string> StaffingWork(MKMOJNAJKJE f,double queuedDay,long at){
  if(!Ready()||!CurrentFaction(f))yield break;
  Note($"officer-work-start faction={f.IBGCLCAIBOL} queuedDay={queuedDay:F4} startDay={NEINKOOBJIH.MIPFHCKDJOK:F4} waitMs={Environment.TickCount64-at} periodDays={DecisionDays(f.IBGCLCAIBOL)} paused={AEHAGJAKNEK.PAKENCLGOHK}");
  foreach(var phase in SafePhases(AdministrativeStaffingSteps(f)))yield return phase;

  Note($"officer-work-end faction={f.IBGCLCAIBOL} gameDay={NEINKOOBJIH.MIPFHCKDJOK:F4} totalMs={Environment.TickCount64-at}");
 }

 static bool RelocationHome(CLAGKDECFID h,AFAGFHJONFJ c)=>h?.LMPMNLLGOEC!=null&&!ManualProtected(h)&&!h.GMILGFFJOIL&&(int)h.HFBAJIGHOAF<=1
  &&h.LMPMNLLGOEC.LMMNKIOAGLC==c.LMPMNLLGOEC.LMMNKIOAGLC&&h.LMPMNLLGOEC.CNOMNBCPJDI==c.OJPECMNINNA
  &&h.OGPEJIEHDAN!=null&&(h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.None||h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.Technology||h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.PrivateMilitary)
  &&h.OJPECMNINNA!=NEINKOOBJIH.OPIKIHPNCIP&&NEINKOOBJIH.AOPJBNBGPLP?.Contains(h.OJPECMNINNA)!=true;
 static string AssignedOfficerCity(CLAGKDECFID h,MKMOJNAJKJE f){
  if(h?.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||h.GMILGFFJOIL||(int)h.HFBAJIGHOAF>1||h.OJPECMNINNA==NEINKOOBJIH.OPIKIHPNCIP)return null;
  string id=h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove?h.OGPEJIEHDAN.NHKNHHIBOGK:h.LMPMNLLGOEC.CNOMNBCPJDI;
  return !string.IsNullOrEmpty(id)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(id,out var c)&&c.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL?id:null;
 }
 static bool MayorCandidate(CLAGKDECFID h,AFAGFHJONFJ c,List<AFAGFHJONFJ> owned)=>RelocationHome(h,c)&&h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.None&&h.OJPECMNINNA!=c.LMPMNLLGOEC.LMMNKIOAGLC
   &&!owned.Any(x=>x.LMPMNLLGOEC.IPJHOOJBCKI==h.OJPECMNINNA||x.LMPMNLLGOEC.BKENJPNKMGI==h.OJPECMNINNA);
 static bool AdministrativeCandidate(CLAGKDECFID h,AFAGFHJONFJ c,List<AFAGFHJONFJ> owned)=>AdministrativeCandidateIndexed(h,c,owned,null);
 static bool AdministrativeCandidateIndexed(CLAGKDECFID h,AFAGFHJONFJ c,List<AFAGFHJONFJ> owned,List<CLAGKDECFID> local){
  if(!RelocationHome(h,c)||h.OJPECMNINNA==c.LMPMNLLGOEC.LMMNKIOAGLC||owned.Any(x=>x.LMPMNLLGOEC.IPJHOOJBCKI==h.OJPECMNINNA||x.LMPMNLLGOEC.BKENJPNKMGI==h.OJPECMNINNA))return false;
  if(h.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.PrivateMilitary)return true;
  if(h.OGPEJIEHDAN.HFBAJIGHOAF!=GCPCLLMKIMM.None)return false;
  var duty=h.BAPIJIJLLCA?.HFBAJIGHOAF??OEBGIDGEIIG.None;
  if(duty==OEBGIDGEIIG.None||duty==OEBGIDGEIIG.Explore)return true;
  // Fully staffed cities must not deadlock because every spare officer was
  // assigned a civil duty. Retain one local worker per civil duty and enough
  // military staff to command the remaining units. Research jobs stay excluded.
  int retain=duty==OEBGIDGEIIG.Military?Math.Max(2,((c.LMPMNLLGOEC.KJPPGIOCDID?.Count??0)+1)/2):1;
  int workers=0;var roster=local??(c.ABJNKABKKJM==null?new List<CLAGKDECFID>():c.ABJNKABKKJM.ToArray().ToList());foreach(var other in roster)if(RelocationHome(other,c)&&other.OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.None&&other.BAPIJIJLLCA?.HFBAJIGHOAF==duty)workers++;
  return workers>retain;
 }
 static IEnumerable<string> AdministrativeStaffingSteps(MKMOJNAJKJE f){
  if(!domestic.Value&&!rebalanceGarrisons.Value)yield break;
  var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;bool Valid()=>MICGPAMNDKM.IJPKEMPIFIH?.Pointer==world&&CurrentFaction(f);
  var owned=new List<AFAGFHJONFJ>();var cities=new List<AFAGFHJONFJ>();var native=f.PJMLPLDPEIM();
  if(native!=null)foreach(var c in native)if(c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL){owned.Add(c);if(StaffingDestination(c,f)&&!c.GKLIEHCHDLD())cities.Add(c);else if(Gate("staffing-excluded:"+c.OJPECMNINNA,30))Note($"officer-destination-excluded city={c.OJPECMNINNA} faction={f.IBGCLCAIBOL} reason={Eligibility(c,out _)} battle={c.GKLIEHCHDLD()}");}
  var heroes=new List<CLAGKDECFID>();int probes=0;
  // Include native CityMove jobs after save/load, not only transfers initiated
  // in this session. No army or soldier is required or modified by this path.
  foreach(var h in OfficerRegistrySnapshot()){
   if(probes++%8==0){yield return "staffing-roster";if(!Valid())yield break;}
   if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!h.GMILGFFJOIL)heroes.Add(h);
  }
  var cache=new Dictionary<string,OfficerBalancePolicy.Officer>();
  foreach(var h in heroes){if(probes++%8==0){yield return "staffing-ability";if(!Valid())yield break;}if(!string.IsNullOrEmpty(h.OJPECMNINNA))cache[h.OJPECMNINNA]=ReadOfficer(h);}
  // Index once per assessment; never re-scan the whole faction in sorting,
  // candidate comparisons or reporting. Refresh the two affected cities in
  // bounded slices before issuing each command, including native arrivals.
  var homes=new Dictionary<string,List<CLAGKDECFID>>();var arrivals=new Dictionary<string,List<CLAGKDECFID>>();var assigned=new Dictionary<string,int>();
  void Add(Dictionary<string,List<CLAGKDECFID>> map,string id,CLAGKDECFID h){if(string.IsNullOrEmpty(id))return;if(!map.TryGetValue(id,out var list))map[id]=list=new();list.Add(h);}
  foreach(var h in heroes){
   if(probes++%8==0){yield return "relocation-index";if(!Valid())yield break;}
   var id=h.LMPMNLLGOEC?.CNOMNBCPJDI;
   if(id!=null&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(id,out var city)&&RelocationHome(h,city))Add(homes,id,h);
   if(h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove)Add(arrivals,h.OGPEJIEHDAN.NHKNHHIBOGK,h);
   string planned=AssignedOfficerCity(h,f);if(planned!=null){assigned.TryGetValue(planned,out int count);assigned[planned]=count+1;}
  }
  List<CLAGKDECFID> Home(AFAGFHJONFJ city)=>homes.TryGetValue(city.OJPECMNINNA,out var list)?list:new();
  List<CLAGKDECFID> Arriving(AFAGFHJONFJ city)=>arrivals.TryGetValue(city.OJPECMNINNA,out var list)?list:new();
  List<OfficerBalancePolicy.Officer> Ratings(IEnumerable<CLAGKDECFID> list)=>list.Where(h=>cache.ContainsKey(h.OJPECMNINNA)).Select(h=>cache[h.OJPECMNINNA]).ToList();
  // Preserve every existing appointment. The vanilla Mayor UI assigns this
  // saved field directly (RowPress); only a present idle officer is appointed.
  if(domestic.Value)foreach(var c in cities){
   yield return "mayor-check";if(!Valid())yield break;if(!Eligible(c,out _)||!string.IsNullOrEmpty(c.LMPMNLLGOEC.BKENJPNKMGI))continue;
   var pool=c.EOMOIINOEMB(1,false,true,false,false,false,false);CLAGKDECFID best=null;double score=double.NegativeInfinity;
   if(pool!=null)foreach(var h in pool.ToArray()){yield return "mayor-candidate";if(!Valid())yield break;if(!MayorCandidate(h,c,owned))continue;
    var a=ReadOfficer(h).Ability;double value=(a[0]+a[1]+a[2]+a[3]+a[4]+a[5])/6+a[11]+a[8]*.5;
    if(value>score){score=value;best=h;}
   }
   if(best!=null&&Eligible(c,out _)&&string.IsNullOrEmpty(c.LMPMNLLGOEC.BKENJPNKMGI)&&MayorCandidate(best,c,owned)){
    c.LMPMNLLGOEC.BKENJPNKMGI=best.OJPECMNINNA;Note($"mayor-appoint city={c.OJPECMNINNA} faction={f.IBGCLCAIBOL} officer={best.OJPECMNINNA} score={score:F1} vacantOnly=true");
   }else if(best==null)Note($"mayor-blocked city={c.OJPECMNINNA} reason=no-local-idle-candidate home={Home(c).Count} arriving={Arriving(c).Count}");
  }
  if(!rebalanceGarrisons.Value||cities.Count<2)yield break;
  int Assigned(AFAGFHJONFJ city)=>assigned.TryGetValue(city.OJPECMNINNA,out int count)?count:0;
  int total=cities.Sum(c=>Assigned(c));
  int fair=(total+cities.Count-1)/cities.Count,sent=0;
  var tiers=cities.ToDictionary(c=>c.OJPECMNINNA,c=>BorderTier(c,f));
  var vulnerable=cities.ToDictionary(c=>c.OJPECMNINNA,c=>OccupationVulnerable(c));
  var quotas=OfficerDeploymentPolicy.Quotas(cities.Select(c=>new OfficerDeploymentPolicy.City{Id=c.OJPECMNINNA,Tier=tiers[c.OJPECMNINNA],Minimum=OfficerDemand(c,f,c.LMPMNLLGOEC.KJPPGIOCDID?.Count??0),Vulnerable=vulnerable[c.OJPECMNINNA]}).ToList(),total);
  Note($"officer-quota faction={f.IBGCLCAIBOL} cities=[{string.Join(";",cities.Select(c=>c.OJPECMNINNA+":tier"+tiers[c.OJPECMNINNA]+":home"+Home(c).Count+":incoming"+Arriving(c).Count+":target"+quotas[c.OJPECMNINNA]))}]");
  int batchLimit=Math.Min(256,total);bool progress=true;
  var corridorPlans=new Dictionary<IntPtr,HashSet<IntPtr>>();
  // Re-evaluate all recipients after each round. Incoming CityMove jobs count
  // immediately, so two donors never fill the same vacancy twice.
  while(progress&&sent<batchLimit){
   progress=false;
   foreach(var target in cities.OrderByDescending(c=>vulnerable[c.OJPECMNINNA]).ThenBy(c=>(Home(c).Count+Arriving(c).Count)/(double)Math.Max(1,quotas[c.OJPECMNINNA]))){
    yield return "relocation-target";if(!Valid())yield break;
    if(sent>=batchLimit)break;if(!StaffingDestination(target,f)||target.GKLIEHCHDLD())continue;
    var at=Home(target);var incoming=Arriving(target);int wanted=quotas[target.OJPECMNINNA];
    bool urgent=vulnerable[target.OJPECMNINNA]||IncomingHostile(target,f);
    if(!OfficerDeploymentPolicy.NeedsRelocation(at.Count+incoming.Count,Assigned(target),wanted,urgent))continue;
    AFAGFHJONFJ source=null;CLAGKDECFID chosen=null;double bestGain=double.NegativeInfinity;int keep=0,days=0;var rejected=new Dictionary<string,int>();
    void Reject(string why){rejected.TryGetValue(why,out int n);rejected[why]=n+1;}
    foreach(var candidateCity in cities){
     yield return "relocation-source";if(!Valid())yield break;
     if(candidateCity.Pointer==target.Pointer||!StaffingDestination(candidateCity,f)||IncomingHostile(candidateCity,f))continue;
     var home=Home(candidateCity);int reserve=Math.Max(OfficerDemand(candidateCity,f,candidateCity.LMPMNLLGOEC.KJPPGIOCDID?.Count??0),Math.Max(2,quotas[candidateCity.OJPECMNINNA]));
     if(home.Count<=reserve){Reject("home-officer-reserve");continue;}
     if(!corridorPlans.TryGetValue(candidateCity.Pointer,out var corridor)){
      var reachable=new List<AFAGFHJONFJ>();foreach(var phase in CapacityDestinationSteps(candidateCity,f,reachable)){yield return phase;if(!Valid())yield break;}
      corridorPlans[candidateCity.Pointer]=corridor=new HashSet<IntPtr>(reachable.Select(n=>n.Pointer));
     }
     if(!corridor.Contains(target.Pointer)){Reject("no-friendly-corridor");continue;}
     var a=new OfficerBalancePolicy.Profile(Ratings(home),OfficerAbilityDimensions);var b=new OfficerBalancePolicy.Profile(Ratings(at.Concat(incoming)),OfficerAbilityDimensions);
     yield return "relocation-native-pool";if(!Valid())yield break;
     var pool=candidateCity.EOMOIINOEMB(1,false,true,false,false,false,false);
     var movePool=(pool?.ToArray()??Array.Empty<CLAGKDECFID>()).Concat(home.Where(h=>h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.PrivateMilitary)).GroupBy(h=>h.OJPECMNINNA).Select(g=>g.First()).ToArray();
     if(movePool.Length==0){Reject("native-eligible-pool-empty");continue;}
     foreach(var h in movePool){
      if(probes++%8==0){yield return "relocation-candidate";if(!Valid())yield break;}
      if(!AdministrativeCandidateIndexed(h,candidateCity,owned,home)||!cache.TryGetValue(h.OJPECMNINNA,out var ability)){Reject("assigned-or-protected");continue;}
      if(!CanGate("officer-balance:"+h.OJPECMNINNA)){Reject("officer-cooldown");continue;}
      int travel=h.NBAPOJABAAO(target.OJPECMNINNA,false);if(travel<=0||travel>90){Reject("native-route");continue;}
      double gain=OfficerDeploymentPolicy.Transfer(a,b,ability,tiers[candidateCity.OJPECMNINNA],tiers[target.OJPECMNINNA]) + (vulnerable[target.OJPECMNINNA]?2d/(1+travel):0);if(!double.IsFinite(gain)){Reject("specialist-reserve");continue;}
      if(gain>bestGain){bestGain=gain;source=candidateCity;chosen=h;keep=reserve;days=travel;}
     }
    }
    if(chosen==null){if(Gate("staffing-block:"+target.OJPECMNINNA,30))Note($"officer-relocation-blocked target={target.OJPECMNINNA} home={at.Count} arriving={incoming.Count} wanted={wanted} reasons=[{string.Join(";",rejected.Select(x=>x.Key+":"+x.Value))}]");continue;}
    // Reuse graph reachability for scoring only. A capture/new invasion during
    // sliced planning must still cancel a now unsafe movement before commit.
    var currentCorridor=new List<AFAGFHJONFJ>();foreach(var phase in CapacityDestinationSteps(source,f,currentCorridor)){yield return phase;if(!Valid())yield break;}
    corridorPlans[source.Pointer]=new HashSet<IntPtr>(currentCorridor.Select(n=>n.Pointer));
    if(!corridorPlans[source.Pointer].Contains(target.Pointer))continue;
    var freshSource=new List<CLAGKDECFID>();var freshTarget=new List<CLAGKDECFID>();var freshArrivals=new List<CLAGKDECFID>();int freshAssigned=0;
    foreach(var h in heroes){
     if(probes++%8==0){yield return "relocation-revalidate";if(!Valid())yield break;}
     if(RelocationHome(h,source))freshSource.Add(h);
     if(RelocationHome(h,target))freshTarget.Add(h);
     if(AssignedOfficerCity(h,f)==target.OJPECMNINNA)freshAssigned++;
     if(h.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!h.GMILGFFJOIL&&h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&h.OGPEJIEHDAN.NHKNHHIBOGK==target.OJPECMNINNA)freshArrivals.Add(h);
    }
    homes[source.OJPECMNINNA]=freshSource;homes[target.OJPECMNINNA]=freshTarget;arrivals[target.OJPECMNINNA]=freshArrivals;
    yield return "relocation-validation";if(!Valid())yield break;
    if(!StaffingDestination(source,f)||!StaffingDestination(target,f)||IncomingHostile(source,f)||target.GKLIEHCHDLD()||!AdministrativeCandidateIndexed(chosen,source,owned,freshSource))continue;
    // A unit can leave or arrive while the sliced census is suspended. Check
    // local records again at commit without another whole-faction traversal.
    var sourceNow=freshSource.Where(h=>RelocationHome(h,source)).ToList();
    var localTarget=target.ABJNKABKKJM?.ToArray()??Array.Empty<CLAGKDECFID>();
    var targetNow=freshTarget.Concat(localTarget).Where(h=>RelocationHome(h,target))
     .Concat(freshArrivals.Where(h=>h.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!h.GMILGFFJOIL&&h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&h.OGPEJIEHDAN.NHKNHHIBOGK==target.OJPECMNINNA))
     .GroupBy(h=>h.OJPECMNINNA).Select(g=>g.First()).ToList();
    keep=Math.Max(keep,OfficerDemand(source,f,source.LMPMNLLGOEC.KJPPGIOCDID?.Count??0));
    if(sourceNow.Count<=keep||!OfficerDeploymentPolicy.NeedsRelocation(targetNow.Count,freshAssigned,wanted,OccupationVulnerable(target)||IncomingHostile(target,f)))continue;
    double checkedGain=OfficerBalancePolicy.Gain(Ratings(sourceNow),Ratings(targetNow),cache[chosen.OJPECMNINNA]);if(!double.IsFinite(checkedGain))continue;
    // Native independent CityMove, one command per hero, many commands per
    // assessment; no teleport, and no army or troop capacity prerequisite.
    TimedAI(f,"relocation-native-command",()=>{chosen.IFFJPNEJCIN(target.OJPECMNINNA,0,false);return true;});
    bool started=chosen.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&chosen.OGPEJIEHDAN.NHKNHHIBOGK==target.OJPECMNINNA;
    if(started)next["officer-balance:"+chosen.OJPECMNINNA]=Environment.TickCount64+300000;
    Note($"officer-relocation source={source.OJPECMNINNA} target={target.OJPECMNINNA} sourceGovernor={source.LMPMNLLGOEC.IPJHOOJBCKI} targetGovernor={target.LMPMNLLGOEC.IPJHOOJBCKI} crossDistrict={source.LMPMNLLGOEC.IPJHOOJBCKI!=target.LMPMNLLGOEC.IPJHOOJBCKI} officer={chosen.OJPECMNINNA} travelDays={days} abilityGain={checkedGain:F4} started={started} gameDay={NEINKOOBJIH.MIPFHCKDJOK:F4} paused={AEHAGJAKNEK.PAKENCLGOHK} independentOfTroops=true sourceBefore={sourceNow.Count} targetProjectedBefore={targetNow.Count} wanted={wanted} targetTier={tiers[target.OJPECMNINNA]} occupationPriority={vulnerable[target.OJPECMNINNA]}");
    if(started){Home(source).Remove(chosen);Add(arrivals,target.OJPECMNINNA,chosen);assigned[source.OJPECMNINNA]=Math.Max(0,Assigned(source)-1);assigned[target.OJPECMNINNA]=freshAssigned+1;sent++;progress=true;}yield return "relocation-commit";if(!Valid())yield break;
   }
  }
  Note($"officer-relocation-batch faction={f.IBGCLCAIBOL} sent={sent} fairTarget={fair} initialAvailable={total} cities={cities.Count} reason={(sent>=batchLimit?"work-limit":"balanced-or-safety-blocked")} projected=[{string.Join(";",cities.Select(c=>c.OJPECMNINNA+":"+Home(c).Count+"+"+Arriving(c).Count))}]");
 }
}
