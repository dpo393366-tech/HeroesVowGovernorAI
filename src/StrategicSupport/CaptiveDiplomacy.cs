using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 static IEnumerable<string> CaptiveDiplomacySteps(MKMOJNAJKJE f,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> pool,List<CLAGKDECFID> missions){
  if(!autoDiplomacy.Value)yield break;
  if(pool.Count==0||f.DNOGPFHMOMO<ResearchReserve){if(Gate("ransom-block:"+f.IBGCLCAIBOL,30))Note($"ransom-blocked faction={f.IBGCLCAIBOL} reason={(pool.Count==0?"no-available-envoy":"treasury-reserve")} treasury={f.DNOGPFHMOMO} envoys={pool.Count}");yield break;}
  var captors=new List<MKMOJNAJKJE>();foreach(var entry in NEINKOOBJIH.COCMABJFOFH)if(entry.Value!=null&&entry.Key!=f.IBGCLCAIBOL)captors.Add(entry.Value);
  var candidates=new List<(MKMOJNAJKJE Captor,CLAGKDECFID Captive,double Value)>();
  foreach(var other in captors){
   yield return "ransom-captor";if(!CurrentFaction(f))yield break;
   if(missions.Any(h=>h.OGPEJIEHDAN?.LKEPGHJDHGG==other.IBGCLCAIBOL)||!DayReady("ransom:"+f.IBGCLCAIBOL+":"+other.IBGCLCAIBOL,7))continue;
   var captured=other.ILBMLPCNGJI(f.IBGCLCAIBOL);if(captured==null)continue;
   foreach(var h in captured.ToArray()){yield return "ransom-captive";if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!CaptiveMissionPending(missions,h.OJPECMNINNA))candidates.Add((other,h,OfficerDeploymentPolicy.Talent(ReadOfficer(h),3)));}
  }
  foreach(var candidate in candidates.OrderByDescending(x=>x.Value)){
   string city=AEHAGJAKNEK.KDHLLDHILCO(candidate.Captor.IBGCLCAIBOL)?.LMPMNLLGOEC?.CNOMNBCPJDI;if(string.IsNullOrEmpty(city))continue;
   (AFAGFHJONFJ City,CLAGKDECFID Hero) envoy=default;double score=double.NegativeInfinity;
   foreach(var p in pool){yield return "ransom-envoy";if(!CurrentFaction(f))yield break;if(!SafeStateOfficer(p.City,p.Hero))continue;
    int days=p.Hero.NBAPOJABAAO(city,false);if(days<=0||days>90)continue;double value=p.Hero.LHEKNHNFPIE()/(1+days/10d);if(value>score){score=value;envoy=p;}
   }
   if(envoy.Hero==null)continue;
   var present=candidate.Captor.ILBMLPCNGJI(f.IBGCLCAIBOL);
   if(!CurrentFaction(f)||f.DNOGPFHMOMO<ResearchReserve||present==null||!present.ToArray().Any(h=>h?.OJPECMNINNA==candidate.Captive.OJPECMNINNA)||!SafeStateOfficer(envoy.City,envoy.Hero)||!MissionCapacity(envoy.City,f)||CaptiveMissionPending(ActiveMissions(f),candidate.Captive.OJPECMNINNA))continue;
   DayReady("ransom:"+f.IBGCLCAIBOL+":"+candidate.Captor.IBGCLCAIBOL,0,true);
   envoy.Hero.FHEMHGIGNDE(candidate.Captor.IBGCLCAIBOL,new CHBFLDOKEHC{HFBAJIGHOAF=IPNKPODNAIN.ReturnCaptive,BOEPOCFEHKG=candidate.Captive.OJPECMNINNA});
   Note($"ransom-request faction={f.IBGCLCAIBOL} captor={candidate.Captor.IBGCLCAIBOL} captive={candidate.Captive.OJPECMNINNA} envoy={envoy.Hero.OJPECMNINNA} job={envoy.Hero.OGPEJIEHDAN?.HFBAJIGHOAF} nativeProposal=true");
   yield return "diplomacy-commit";yield break;
  }
 }
}

