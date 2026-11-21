using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public sealed partial class StrategicSupport {
 static IEnumerable<string> OccupationReliefSteps(MKMOJNAJKJE f){
  if(!support.Value||!rebalanceGarrisons.Value)yield break;
  var cities=ProtectedCities(f);var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
  bool Valid()=>CurrentFaction(f)&&MICGPAMNDKM.IJPKEMPIFIH?.Pointer==world;
  foreach(var target in cities){
   yield return "occupation-protection-target";if(!Valid())yield break;
   if(!OccupationVulnerable(target)||target.GKLIEHCHDLD()||IncomingHostile(target,f))continue; // active invasion uses the urgent relay
   var ranked=new List<(AFAGFHJONFJ City,double Length)>();foreach(var phase in RankCityRoutes(cities,target,f,true,ranked)){yield return phase;if(!Valid())yield break;}
   foreach(var candidate in ranked){var source=candidate.City;
    yield return "occupation-protection-source";if(!Valid())yield break;
    var teams=AttackTeams();long need=HomeProtectionNeed(target,f,teams);if(need<=0)break;
    if(source.Pointer==target.Pointer||!DistributionCity(source,out _)||!StaffingDestination(target,f)||OccupationVulnerable(source)||IncomingHostile(source,f))continue;
    int room=CapacityRoom(target,teams);if(room<=0)break;
    var connected=new List<AFAGFHJONFJ>();foreach(var phase in CapacityDestinationSteps(source,f,connected)){yield return phase;if(!Valid())yield break;}
    if(!connected.Any(c=>c.Pointer==target.Pointer))continue;
    foreach(var phase in CityShipmentSteps(source,target,f,room,need,"occupation-protection")){yield return phase;if(!Valid())yield break;}

   }
  }
 }
}
