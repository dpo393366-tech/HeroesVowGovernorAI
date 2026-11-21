using System;
using System.Collections.Generic;
using System.Linq;

// A wave contributes only after its arrival. A later wave cannot revive a
// position that collapsed before it arrived. Values are either soldiers OR
// combat power; callers must never mix the two within one forecast.
public static class ReinforcementTimeline {
 public readonly struct Wave {
  public readonly double At,Strength;
  public Wave(double at,double strength){At=at;Strength=strength;}
 }
 public static int Need(double own,double foe,double ownLoss,double foeLoss,double arrival,double horizon,
  IReadOnlyList<Wave> allies,IReadOnlyList<Wave> enemies,double ratio,double combatStart,out string reason,out double hold,bool unobservedPrecontact=false){
  reason="invalid-forecast";hold=0;
  if(!double.IsFinite(own)||!double.IsFinite(foe)||own<0||foe<0||!double.IsFinite(arrival)||arrival<0||!double.IsFinite(horizon)||horizon<arrival
   ||!double.IsFinite(ownLoss)||!double.IsFinite(foeLoss)||ownLoss<0||foeLoss<0||!double.IsFinite(combatStart)||combatStart<0)return 0;
  var times=new SortedSet<double>{0,arrival,horizon,Math.Min(horizon,combatStart)};
  foreach(var w in allies.Concat(enemies)){
   if(!double.IsFinite(w.At)||w.At<0||!double.IsFinite(w.Strength)||w.Strength<0)return 0;
   if(w.At<=horizon)times.Add(w.At);
  }
  double a=own,b=foe,previous=0,required=0;bool shortage=false,uncertainCollapse=false;
  // Do not extrapolate a short burst into a guaranteed enemy annihilation.
  double maxEnemyLoss=foe*.5,enemyLost=0;
  // An unobserved rate is a safety allowance, not unlimited guaranteed damage.
  // Cap it to a quarter of forces that have actually arrived in this forecast.
  double maxOwnLoss=unobservedPrecontact?own*.25:double.PositiveInfinity,ownLost=0;
  hold=double.PositiveInfinity;
  foreach(double at in times){
   double dt=Math.Max(0,at-Math.Max(previous,combatStart));
   if(b>0&&dt>0){
    double ownDamage=Math.Min(Math.Max(0,maxOwnLoss-ownLost),ownLoss*dt);
    if(ownLoss>0&&a<=ownDamage&&double.IsPositiveInfinity(hold))hold=Math.Max(previous,combatStart)+Math.Max(0,a)/ownLoss;
    a-=ownDamage;ownLost+=ownDamage;double lost=Math.Min(Math.Max(0,maxEnemyLoss-enemyLost),foeLoss*dt);b=Math.Max(0,b-lost);enemyLost+=lost;
   }
   // Strictly before this candidate's arrival; equality is too late as well.
   if(at<=arrival&&b>0&&a<=0){
    if(!unobservedPrecontact){reason="collapse-before-wave";return 0;}
    // A guessed pre-battle loss rate is not evidence of actual defeat. Send
    // only the source's safe surplus; monitor real ownership/battle afterwards.
    // Actual in-progress battles keep the strict observed-collapse rule.
    uncertainCollapse=true;
   }
   foreach(var w in allies)if(w.At==at){a+=w.Strength;if(unobservedPrecontact)maxOwnLoss+=w.Strength*.25;}
   foreach(var w in enemies)if(w.At==at)b+=w.Strength;
   if(at>=arrival&&b>0){shortage|=a<b*.95;required=Math.Max(required,b*Math.Clamp(ratio,1,1.5)-a);}
   previous=at;
  }
  reason=shortage?(uncertainCollapse?"uncertain-precontact-relay":"relay-needed"):"covered-by-timed-waves";
  return shortage?(int)Math.Clamp(Math.Ceiling(required),0,int.MaxValue):0;
 }
}
