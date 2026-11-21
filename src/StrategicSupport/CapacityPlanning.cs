using System;
using System.Collections.Generic;
public sealed partial class StrategicSupport {
 static int CapacityShortfall(AFAGFHJONFJ c){
  var state=ObserveCapacity(c);if(state==null)return 0;
  int shortage=Math.Max(0,state.Units-state.Capacity);if(shortage==0)return 0;
  // Effective capacity can have faction multipliers. Wait for a committed
  // expansion instead of guessing that multiplier and overbuilding on each tick.
  if(c.FGBLEMGGOPO!=null)foreach(var d in c.FGBLEMGGOPO)if(d?.KFCOLLDEJNF==HNDFFIEJNHI.Public&&d.AIMLPNBOIIF!=null)foreach(var x in d.AIMLPNBOIIF){
   var b=x?.GEKKLMPLDHJ;if(b==null||Completed(x)||b.IKELHGAHBID<=0)continue;
   int old=0;if(!string.IsNullOrEmpty(b.MDOIJIAONJM)&&AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(b.MDOIJIAONJM,out var basis))old=Math.Max(0,basis.IKELHGAHBID);
   if(b.IKELHGAHBID>old)return 0;
  }return shortage;
 }
 static GoalPlan ReadRenewalGoals(AFAGFHJONFJ c,MKMOJNAJKJE f){var goals=ReadGoals(c);AddCapacityGoal(c,f,goals);return goals;}
 static void AddCapacityGoal(AFAGFHJONFJ c,MKMOJNAJKJE f,GoalPlan goals){
  AddBarracksGoals(c,f,goals);
  int shortage=CapacityShortfall(c);if(shortage<=0)return;
  PABIPDOFBFG best=null;double value=0;var rejected=new List<string>();
  foreach(var kv in AEHAGJAKNEK.ACIGFKMDOKE){var b=kv.Value;if(b==null||b.IKELHGAHBID<=0||!string.IsNullOrEmpty(b.MDOIJIAONJM))continue;
   string reason=BuildBlock(c,f,b,LocalReserve(c));if(reason!=null){if(rejected.Count<20)rejected.Add(b.OJPECMNINNA+":"+reason);continue;}
   double score=(double)Math.Min(shortage,b.IKELHGAHBID)/Math.Max(1,b.NPCHPDNFAME);
   if(score>value){value=score;best=b;}
  }
  // This is an internal emergency target; native city goals are never edited.
  if(best!=null){goals.Buildings.TryGetValue(best.OJPECMNINNA,out int existing);goals.Buildings[best.OJPECMNINNA]=Math.Max(1,existing);AddNeed(goals,"building:"+best.OJPECMNINNA,1,0);}
  if(Gate("capacity-plan:"+c.OJPECMNINNA,30))Note($"capacity-plan city={c.OJPECMNINNA} shortage={shortage} target={best?.OJPECMNINNA} blocked=[{string.Join(";",rejected)}]");
 }
}
