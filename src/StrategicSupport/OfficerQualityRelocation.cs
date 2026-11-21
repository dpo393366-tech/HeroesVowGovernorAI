using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 static IEnumerable<string> OfficerQualitySteps(MKMOJNAJKJE f){
  if(!rebalanceGarrisons.Value)yield break;
  var cities=ProtectedCities(f);var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
  bool Valid()=>CurrentFaction(f)&&MICGPAMNDKM.IJPKEMPIFIH?.Pointer==world;
  var registry=new List<CLAGKDECFID>();var moving=new HashSet<string>();int scanned=0;
  foreach(var h in OfficerRegistrySnapshot()){if(++scanned%16==0){yield return "officer-quality-roster";if(!Valid())yield break;}if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL)registry.Add(h);}
  foreach(var h in registry)if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove){moving.Add(h.OGPEJIEHDAN.MIGIFJPINLB??"");moving.Add(h.OGPEJIEHDAN.NHKNHHIBOGK??"");}
  foreach(var a in cities){
   yield return "officer-quality-source";if(!Valid())yield break;
   if(!StaffingDestination(a,f)||IncomingHostile(a,f)||moving.Contains(a.OJPECMNINNA))continue;
   var ah=registry.Where(h=>RelocationHome(h,a)).ToList();if(ah.Count<=Math.Max(3,OfficerDemand(a,f,a.LMPMNLLGOEC.KJPPGIOCDID?.Count??0)))continue;
   var ar=new List<OfficerBalancePolicy.Officer>();
   foreach(var h in ah){yield return "officer-quality-ability";if(!Valid())yield break;ar.Add(ReadOfficer(h));}
   var profileA=new OfficerBalancePolicy.Profile(ar,ar[0].Ability.Length);int at=BorderTier(a,f);
   foreach(var b in cities){
    yield return "officer-quality-target";if(!Valid())yield break;
    if(!StaffingDestination(b,f)||a.Pointer==b.Pointer||moving.Contains(b.OJPECMNINNA)||IncomingHostile(b,f))continue;
    int bt=BorderTier(b,f);if(bt<at)continue;
    var bh=registry.Where(h=>RelocationHome(h,b)).ToList();if(bh.Count<=Math.Max(3,OfficerDemand(b,f,b.LMPMNLLGOEC.KJPPGIOCDID?.Count??0)))continue;
    var br=new List<OfficerBalancePolicy.Officer>();
    foreach(var h in bh){yield return "officer-quality-ability";if(!Valid())yield break;br.Add(ReadOfficer(h));}
    var profileB=new OfficerBalancePolicy.Profile(br,br[0].Ability.Length);CLAGKDECFID give=null,take=null;double best=.025;
    var ac=new bool[ah.Count];var bc=new bool[bh.Count];
    for(int i=0;i<ah.Count;i++){yield return "officer-quality-eligibility";if(!Valid())yield break;ac[i]=AdministrativeCandidateIndexed(ah[i],a,cities,ah)&&CanGate("officer-balance:"+ah[i].OJPECMNINNA);}
    for(int i=0;i<bh.Count;i++){yield return "officer-quality-eligibility";if(!Valid())yield break;bc[i]=AdministrativeCandidateIndexed(bh[i],b,cities,bh)&&CanGate("officer-balance:"+bh[i].OJPECMNINNA);}int pairs=0;
    for(int i=0;i<ah.Count;i++)for(int j=0;j<bh.Count;j++){
     if(++pairs%8==0){yield return "officer-quality-pair";if(!Valid())yield break;}
     if(!ac[i]||!bc[j])continue;
     double gain=OfficerBalancePolicy.ExchangeGain(profileA,profileB,ar[i],br[j]);if(!double.IsFinite(gain))continue;
     double quality=OfficerDeploymentPolicy.Talent(ar[i],bt)-OfficerDeploymentPolicy.Talent(br[j],bt);
     // Higher tiers receive stronger commanders; equal tiers reduce imbalance.
     if(bt>at){if(quality<5)continue;gain=quality*(bt-at)/100d;}
     if(gain>best){best=gain;give=ah[i];take=bh[j];}
    }
    if(give==null)continue;
    var connected=new List<AFAGFHJONFJ>();foreach(var phase in CapacityDestinationSteps(a,f,connected)){yield return phase;if(!Valid())yield break;}
    if(!connected.Any(c=>c.Pointer==b.Pointer))continue;
    if(give.NBAPOJABAAO(b.OJPECMNINNA,false)<=0||take.NBAPOJABAAO(a.OJPECMNINNA,false)<=0)continue;
    // Do not replace ah while ar still describes the original indexed roster.
    // A failed commit then continues to another target with the same ar.
    var freshA=new List<CLAGKDECFID>();var freshB=new List<CLAGKDECFID>();int freshCount=0;
    foreach(var h in OfficerRegistrySnapshot()){
     if(++freshCount%16==0){yield return "officer-quality-revalidate-roster";if(!Valid())yield break;}
     if(RelocationHome(h,a))freshA.Add(h);if(RelocationHome(h,b))freshB.Add(h);
    }
    if(!Valid()||freshA.Count<=Math.Max(3,OfficerDemand(a,f,a.LMPMNLLGOEC.KJPPGIOCDID?.Count??0))||freshB.Count<=Math.Max(3,OfficerDemand(b,f,b.LMPMNLLGOEC.KJPPGIOCDID?.Count??0))||IncomingHostile(a,f)||IncomingHostile(b,f)||!AdministrativeCandidateIndexed(give,a,cities,freshA)||!AdministrativeCandidateIndexed(take,b,cities,freshB))continue;
    if(!StaffingDestination(a,f)||!StaffingDestination(b,f))continue;
    bool first=false,second=false;
    try {
     give.IFFJPNEJCIN(b.OJPECMNINNA,0,false);first=give.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&give.OGPEJIEHDAN.NHKNHHIBOGK==b.OJPECMNINNA;
     if(first){take.IFFJPNEJCIN(a.OJPECMNINNA,0,false);second=take.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove&&take.OGPEJIEHDAN.NHKNHHIBOGK==a.OJPECMNINNA;}
    }finally{
     if(first||second){moving.Add(a.OJPECMNINNA);moving.Add(b.OJPECMNINNA);next["officer-balance:"+give.OJPECMNINNA]=next["officer-balance:"+take.OJPECMNINNA]=Environment.TickCount64+300000;}
     Note($"officer-quality-exchange a={a.OJPECMNINNA} tierA={at} b={b.OJPECMNINNA} tierB={bt} give={give.OJPECMNINNA} take={take.OJPECMNINNA} first={first} second={second} score={best:F3} independentOfTroops=true");
    }
    if(first)break;
   }
  }
 }
}
