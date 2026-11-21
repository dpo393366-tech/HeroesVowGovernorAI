using System;
public static class OffensivePolicy
{
 public static string Readiness(int homeUnits,int availableUnits,int minimumHome,int treasury,int threshold,long upkeep){
  // Empty capacity and unfinished recruits are not expedition requirements.
  // The actual selected army must beat the target AFTER home reserves are kept.
  if(homeUnits<=minimumHome)return "minimum-garrison";
  if(availableUnits<=0)return "no-ready-troops";
  if(treasury<Math.Max(threshold,Math.Min(int.MaxValue,upkeep*3)))return "treasury";
  return null;
 }
 public static long PowerBudget(long home,long ready,long border,double retain)
  =>Math.Max(0,Math.Min(ready,home-Math.Max(border,(long)Math.Ceiling(home*retain))));
 public static int RequiredPower(long defense,double ratio)=> (int)Math.Min(int.MaxValue,Math.Ceiling(Math.Max(1000,defense)*ratio));
 public static bool FoodSafe(long city,long reserved,long homeFloor,int daily,int days,int loaded=0)
  =>daily>0&&days>0&&city-reserved>=homeFloor+Math.Max(0L,(long)daily*days-Math.Max(0,loaded));
}
