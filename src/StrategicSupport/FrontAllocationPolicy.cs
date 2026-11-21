using System;
using System.Collections.Generic;
// Conservative multi-front budget: arriving enemies count in full. Expected
// enemy casualties are never spent before they actually occur.
public static class FrontAllocationPolicy {
 public readonly struct Arrival {public readonly double Eta,Men,Power;public Arrival(double eta,double men,double power){Eta=eta;Men=men;Power=power;}}
 public readonly struct Budget {public readonly double Men,Power,Horizon,EnemyMen;public Budget(double m,double p,double h,double e){Men=m;Power=p;Horizon=h;EnemyMen=e;}}
 public static Budget Available(double own,double enemy,double ownPower,double enemyPower,double loss,double horizon,IEnumerable<Arrival> arrivals,double margin=1.2){
  if(!double.IsFinite(own+enemy+ownPower+enemyPower+loss+horizon)||own<=0||ownPower<=0||horizon<0)return new();
  foreach(var a in arrivals){if(!double.IsFinite(a.Eta+a.Men+a.Power)||a.Eta<0||a.Men<0||a.Power<0)return new();horizon=Math.Max(horizon,a.Eta+10);enemy+=a.Men;enemyPower+=a.Power;}
  double attrition=Math.Max(0,loss)*horizon;
  return new(Math.Max(0,own-attrition-enemy*margin),Math.Max(0,ownPower-attrition*ownPower/own-enemyPower*margin),horizon,enemy);
 }
}
