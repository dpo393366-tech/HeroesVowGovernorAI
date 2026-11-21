using System;
using System.Collections.Generic;
using System.Linq;

// A labelled precontact estimate. Each force starts contributing losses only
// after its own arrival; a distant force cannot shorten an earlier holding time.
public static class TimedBorderDefense {
 public readonly struct Wave {public readonly double At,Power;public Wave(double at,double power){At=at;Power=power;}}
 public static double Collapse(double home,IEnumerable<Wave> input,double lossPeriod){
  if(!double.IsFinite(home)||home<=0||!double.IsFinite(lossPeriod)||lossPeriod<=0)return 0;
  var waves=input.ToList();foreach(var w in waves)if(!double.IsFinite(w.At)||w.At<0||!double.IsFinite(w.Power)||w.Power<0)return 0;
  waves.Sort((a,b)=>a.At.CompareTo(b.At));double time=0,power=0;
  foreach(var wave in waves){double rate=power/lossPeriod;
   if(rate>0&&home<=rate*(wave.At-time))return time+home/rate;
   home-=rate*(wave.At-time);time=wave.At;power+=wave.Power;
  }
  return power>0?time+home/(power/lossPeriod):double.PositiveInfinity;
 }
}
