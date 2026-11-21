using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 static bool MissionCapacity(AFAGFHJONFJ city,MKMOJNAJKJE f){
  if(!CurrentFaction(f)||!Eligible(city,out var owner)||owner.Pointer!=f.Pointer||IncomingHostile(city,f)||FoodEmergency(city))return false;
  int home=0,research=0;foreach(var h in city.ABJNKABKKJM.ToArray())if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&h.LMPMNLLGOEC.CNOMNBCPJDI==city.OJPECMNINNA&&!h.GMILGFFJOIL){
   var job=h.OGPEJIEHDAN?.HFBAJIGHOAF;if(job==GCPCLLMKIMM.None||job==GCPCLLMKIMM.Technology||job==GCPCLLMKIMM.PrivateMilitary)home++;if(job==GCPCLLMKIMM.Technology)research++;
  }
  int away=ActiveMissions(f).Count(h=>h.OGPEJIEHDAN.MIGIFJPINLB==city.OJPECMNINNA);
  return StatecraftPolicy.MissionSlots(home,research,away,StateBorder(city,f))>0;
 }
 static bool CaptiveMissionPending(List<CLAGKDECFID> missions,string captive){
  foreach(var h in missions){var job=h.OGPEJIEHDAN;if(job==null)continue;
   if(job.LKEPGHJDHGG==captive)return true;
   if(job.ALGOEDPACKF!=null)foreach(var p in job.ALGOEDPACKF)if(p?.HFBAJIGHOAF==IPNKPODNAIN.ReturnCaptive&&p.BOEPOCFEHKG==captive)return true;
  }return false;
 }
 static bool SchemeTargetPresent(AFAGFHJONFJ city,CLAGKDECFID target,MKMOJNAJKJE f,DOHGPECAMJC kind){
  if(target?.LMPMNLLGOEC==null||city?.LMPMNLLGOEC==null||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(city.OJPECMNINNA,out var live)||live.Pointer!=city.Pointer)return false;
  if(kind==DOHGPECAMJC.Rescue)return city.LMPMNLLGOEC.LMMNKIOAGLC!=f.IBGCLCAIBOL&&target.LMPMNLLGOEC.LMMNKIOAGLC==f.IBGCLCAIBOL&&city.JHNBHLPJKMO?.ToArray().Any(h=>h?.Pointer==target.Pointer)==true;
  return War(f,city.LMPMNLLGOEC.LMMNKIOAGLC)&&target.LMPMNLLGOEC.LMMNKIOAGLC==city.LMPMNLLGOEC.LMMNKIOAGLC&&target.LMPMNLLGOEC.CNOMNBCPJDI==city.OJPECMNINNA&&!target.GMILGFFJOIL&&
   (target.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None||target.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.Technology||target.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.PrivateMilitary)&&city.ABJNKABKKJM.ToArray().Any(h=>h?.Pointer==target.Pointer);
 }
 static IEnumerable<string> ActiveSchemeSteps(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> pool,List<CLAGKDECFID> missions,bool rescue){
  if(!autoSchemes.Value||pool.Count==0)yield break;var kind=rescue?DOHGPECAMJC.Rescue:DOHGPECAMJC.Murder;
  var candidates=new List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>();
  var nearby=new HashSet<string>();foreach(var c in cities)if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n!=null)nearby.Add(n.OJPECMNINNA);
  var targets=new List<AFAGFHJONFJ>();foreach(var entry in MICGPAMNDKM.IJPKEMPIFIH)if(entry.Value?.LMPMNLLGOEC!=null)targets.Add(entry.Value);
  foreach(var city in targets){yield return "active-scheme-target-city";if(!CurrentFaction(f))yield break;
   if(city.LMPMNLLGOEC.LMMNKIOAGLC==f.IBGCLCAIBOL||!rescue&&(!nearby.Contains(city.OJPECMNINNA)||!War(f,city.LMPMNLLGOEC.LMMNKIOAGLC)))continue;
   var list=rescue?city.JHNBHLPJKMO:city.ABJNKABKKJM;if(list==null)continue;
   foreach(var h in list.ToArray())if(SchemeTargetPresent(city,h,f,kind)&&!CaptiveMissionPending(missions,h.OJPECMNINNA)&&DayReady("active-scheme:"+f.IBGCLCAIBOL+":"+h.OJPECMNINNA,(rescue?2:5)*DecisionDays(f.IBGCLCAIBOL)))candidates.Add((city,h));
  }
  (AFAGFHJONFJ City,CLAGKDECFID Hero) actor=default,victim=default;double best=double.NegativeInfinity;int eta=0;
  foreach(var target in candidates)foreach(var p in pool){
   yield return "active-scheme-candidate";if(!CurrentFaction(f))yield break;if(!SafeStateOfficer(p.City,p.Hero))continue;
   if(!rescue&&!AttackPermission(p.City,f,p.City.LMPMNLLGOEC.IPJHOOJBCKI,target.City.LMPMNLLGOEC.LMMNKIOAGLC))continue;
   int ability=p.Hero.MLGPOHGNLOL(kind),days=p.Hero.NBAPOJABAAO(target.City.OJPECMNINNA,false);if(ability<60||days<=0||days>90)continue;
   // Prefer peaceful return requests when a rescue is relatively weak and
   // diplomacy is affordable. War, poverty or a skilled rescuer favors rescue.
   if(rescue&&autoDiplomacy.Value&&!War(f,target.City.LMPMNLLGOEC.LMMNKIOAGLC)&&f.DNOGPFHMOMO>=ResearchReserve&&ability<75)continue;
   double value=(ability+OfficerDeploymentPolicy.Talent(ReadOfficer(target.Hero),3)*.3)/(1+days/15d);
   if(value>best){best=value;actor=p;victim=target;eta=days;}
  }
  yield return "active-scheme-validate";
  if(actor.Hero==null||!CurrentFaction(f)||!SafeStateOfficer(actor.City,actor.Hero)||!MissionCapacity(actor.City,f)||!SchemeTargetPresent(victim.City,victim.Hero,f,kind)||CaptiveMissionPending(ActiveMissions(f),victim.Hero.OJPECMNINNA))yield break;
  if(!rescue&&!AttackPermission(actor.City,f,actor.City.LMPMNLLGOEC.IPJHOOJBCKI,victim.City.LMPMNLLGOEC.LMMNKIOAGLC))yield break;
  string key="active-scheme:"+f.IBGCLCAIBOL+":"+victim.Hero.OJPECMNINNA;if(!DayReady(key,(rescue?2:5)*DecisionDays(f.IBGCLCAIBOL)))yield break;
  DayReady(key,0,true);actor.Hero.IGIMIIDMGAH(victim.City.OJPECMNINNA,victim.Hero.OJPECMNINNA,kind,false);
  bool started=MissionJob(actor.Hero.OGPEJIEHDAN?.HFBAJIGHOAF??GCPCLLMKIMM.None);
  Note($"statecraft-active-scheme faction={f.IBGCLCAIBOL} kind={kind} city={victim.City.OJPECMNINNA} target={victim.Hero.OJPECMNINNA} officer={actor.Hero.OJPECMNINNA} etaDays={eta} started={started} nativeOutcome=true");
  if(started)yield return "scheme-commit";
 }
}
