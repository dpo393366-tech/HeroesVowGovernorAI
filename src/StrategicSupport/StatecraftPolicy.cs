using System;
public static class StatecraftPolicy
{
 public sealed class Needs {public bool Food,Front,Research,Building,Explore,Diplomacy,NeedOfficers,Recovery,Growth;public double Money,Security,Popular;public int Troops,Officers;}
 // Only understood national/city benefits are automated. Unknown/private,
 // interactive and hostile effects stay under the original game's control.
 public static double Effect(int kind,int amount,Needs n)
 {
  // Negative expenditure modifiers are benefits, not harmful effects.
  // Keep negative income/popularity/combat modifiers excluded; do not infer
  // that every negative value or mixed tradeoff is safe.
  if(kind==13||kind==17||kind==18||kind==19||kind==22||kind==27){
   if(amount>0)return -1;
   double value=kind==19?(n.Troops>0?6:0):kind==22?(n.Front?5:1):kind==27?(n.Officers>0?5:0):n.Money>0?4:1;
   return value*Math.Min(100,-(double)amount);
  }
  // TransportInstant is a negative delivery-duration modifier (e.g. -100%).
  if(kind==47)return amount<=0?Math.Max(1,Math.Min(100,-(double)amount))*4:-1;
  if(amount==0)return 0;
  if(amount<0)return -1;
  double urgency=kind switch {
   0 or 5 or 36 or 37 or 38=>n.Front&&n.Troops>0?5:0,
   1=>n.Front?6:0, 2 or 15 or 32 or 34=>n.Money>0?3+4*n.Money:n.Growth?1:0,
   3 or 16 or 33 or 35=>n.Food?10:n.Growth?1:0,4=>n.Building?5:0,
   6 or 7 or 31=>n.Security>0?4+5*n.Security:0,
   11=>n.Research?5:0,29=>n.Popular>0?4+5*n.Popular:0,
   8=>n.Explore?4:0,9 or 24=>n.NeedOfficers?4:0,
   10=>n.Diplomacy||n.Front?4:0,12=>3,
   14 or 30=>n.Growth?2:0,20 or 21=>n.Front?5:0,
   23=>n.Recovery?6:0,28=>n.Officers>0?2:0,
   // Recognized beneficial components do not invalidate a mixed strategy.
   // Pure loyalty/healing is deferred until its exact deficit is measured.
   25 or 26 or 40 or 41=>n.Officers>0?2:0,39=>n.Recovery?6:1,
   42=>n.Officers>0?3:1,43=>3,44 or 45 or 46=>2,
   _=>-1};
  return urgency<0?-1:Math.Max(1,urgency)*Math.Min(100,amount);
 }
 public static bool Affordable(int points,int start,int upkeep,long activeUpkeep,int production)
  =>start>=0&&upkeep>=0&&points>=start&&activeUpkeep>=0
    &&(upkeep==0||(activeUpkeep+upkeep<=Math.Max(0,production)&&(long)points-start>=2L*(activeUpkeep+upkeep)));
 public static int MissionSlots(int home,int research,int away,bool front)
  =>Math.Max(0,Math.Min(front?2:4,Math.Max(0,home+away-Math.Max(0,research)-(front?6:3))/4)-away);
 public static double Diplomacy(int relation,int wars,bool sharedEnemy,double own,double enemy,bool underAttack)
  =>relation==2?(wars>0&&(underAttack||enemy>Math.Max(1,own)*1.5)?300:0)
    :relation==3?(wars>0&&underAttack?250:0)
    :sharedEnemy&&wars>0?150:0;
}
