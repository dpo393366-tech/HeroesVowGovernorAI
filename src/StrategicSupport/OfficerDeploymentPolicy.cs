using System;
using System.Collections.Generic;
using System.Linq;

public static class OfficerDeploymentPolicy {
 // Assigned commanders on campaign/duty still occupy their planned city slot.
 // Fill genuinely empty staffing quickly, but don't move an officer across
 // the map merely to correct a single ordinary rounding seat.
 public static bool NeedsRelocation(int presentAndIncoming,int assigned,int wanted,bool emergency){
  if(emergency)return presentAndIncoming<wanted;
  if(presentAndIncoming<2)return presentAndIncoming<Math.Min(2,wanted);
  return Math.Max(presentAndIncoming,assigned)<wanted-1;
 }
 public sealed class City {public string Id;public int Tier,Minimum;public bool Vulnerable;}
 // Allocate an exact, conserved number. Scarcity is distributed, never hidden
 // by independently rounding every city's share upward.
 public static Dictionary<string,int> Quotas(List<City> cities,int total){
  var result=cities.ToDictionary(c=>c.Id,c=>0);
  for(int n=0;n<Math.Max(0,total)&&cities.Count>0;n++){
   var c=cities.OrderByDescending(x=>result[x.Id]<Math.Min(2,x.Minimum)?10000+(x.Vulnerable?100:0)+x.Tier:result[x.Id]<x.Minimum?1000+(x.Vulnerable?100:0)+x.Tier:0)
    .ThenBy(x=>result[x.Id]/(double)Math.Max(1,x.Tier)).ThenBy(x=>x.Id,StringComparer.Ordinal).First();
   result[c.Id]++;
  }return result;
 }
 public static double Talent(OfficerBalancePolicy.Officer h,int tier){
  var a=h.Ability;if(a.Length<14)return a.Length==0?0:a.Average();
  double civil=(a[0]+a[1]+a[4]+a[5]+a[11])/5,combat=(a[2]+a[3]+a[8]+a[9]+a[10]+a[13])/6;
  return tier>=3?combat*.7+civil*.3:tier==2?(combat+civil)*.5:civil*.6+combat*.4;
 }
 public static double Transfer(OfficerBalancePolicy.Profile from,OfficerBalancePolicy.Profile to,OfficerBalancePolicy.Officer h,int sourceTier,int targetTier){
  double safe=OfficerBalancePolicy.Gain(from,to,h);if(!double.IsFinite(safe))return safe;
  return safe+(targetTier-sourceTier)*Talent(h,targetTier)/100d;
 }
}
