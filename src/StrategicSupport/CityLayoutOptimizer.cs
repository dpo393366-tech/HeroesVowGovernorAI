using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
// Whole available footprint planning, cooperatively evaluated. This is a beam
// optimizer, not a claim of a mathematical global optimum for arbitrary mods.
public static class CityLayoutOptimizer {
 sealed class Plan {
  public List<int> Steps=new();public HashSet<long> Cells=new();public Dictionary<string,int> Counts=new();
  public Dictionary<string,BuildingEffectsPolicy.Output> Outputs=new();public Dictionary<string,double> Gains=new();public double Goals,Value,Money,Food,Popular,Security,Commerce,Agriculture,Population,PopulationAdd;public int Barracks;
 }
 static double Civic(Plan p)=>Math.Min(0,p.Popular-1)+Math.Min(0,p.Security-1)+Math.Min(0,p.Population);
 static int Compare(Plan a,Plan b){
  int n=a.Goals.CompareTo(b.Goals);if(n!=0)return n;
  n=Civic(a).CompareTo(Civic(b));if(n!=0)return n;
  n=a.Barracks.CompareTo(b.Barracks);if(n!=0)return n;
  // Explicit expansion requirements are already in Goals; thereafter balance
  // is a utility term, not an unlimited veto on technology/support buildings.
  n=a.Cells.Count.CompareTo(b.Cells.Count);if(n!=0)return n;
  return (a.Value+20*Math.Min(a.Commerce,a.Agriculture)+Math.Sqrt(Math.Max(0,a.Money)*Math.Max(0,a.Food/4))*2).CompareTo(b.Value+20*Math.Min(b.Commerce,b.Agriculture)+Math.Sqrt(Math.Max(0,b.Money)*Math.Max(0,b.Food/4))*2);
 }
 public static IEnumerable<bool> Solve(IReadOnlyList<DevelopmentPolicy.Action> actions,Dictionary<string,DevelopmentPolicy.Need> needs,Action<List<int>> done,double popular=1,double security=1,double commerce=0,double agriculture=0,double population=1,IReadOnlyList<BuildingEffectsPolicy.Building> existing=null,Func<double,double,double> growth=null){
  if(actions.Count==0){done(new());yield break;}
  double GoalScore(Dictionary<string,double> gains){double sum=0;foreach(var pair in needs)if(pair.Value.Missing>0){gains.TryGetValue(pair.Key,out double supplied);sum+=Math.Min(1,Math.Max(0,supplied)/pair.Value.Missing);}return sum;}
  var baseline=existing??Array.Empty<BuildingEffectsPolicy.Building>();var baseDistricts=baseline.GroupBy(b=>b.District??"").ToDictionary(g=>g.Key,g=>g.ToList());var baseOutputs=baseDistricts.ToDictionary(kv=>kv.Key,kv=>BuildingEffectsPolicy.Evaluate(kv.Value,false));var baseSpecials=new HashSet<int>(baseline.Select(b=>b.Special));var clock=Stopwatch.StartNew();
  // The same district prefix is shared by many whole-city beam candidates.
  // Cache its exact ordered output, not a sorted set: native truncation can
  // make multiplier order significant. This cache belongs to this snapshot
  // only, so research, expansion and other city changes cannot reuse it.
  var districtOutputs=new Dictionary<(string,string),BuildingEffectsPolicy.Output>();
  int maximum=actions.SelectMany(a=>a.Tiles).Distinct().Count();
  var beam=new List<Plan>{new(){Popular=popular,Security=security,Commerce=commerce,Agriculture=agriculture,Population=population}};Plan best=beam[0];int probes=0;
  for(int depth=0;depth<maximum;depth++){
   var next=new List<Plan>();var signatures=new HashSet<string>();
   foreach(var p in beam){
    var prior=p.Steps.Select(i=>actions[i]).ToList();
    for(int i=0;i<actions.Count;i++){
     if(++probes>=16||clock.Elapsed.TotalMilliseconds>=1){probes=0;yield return false;clock.Restart();}
     var a=actions[i];p.Counts.TryGetValue(a.Id,out int count);
     if(a.Tiles.Length==0||count>=a.Limit||a.Tiles.Any(p.Cells.Contains))continue;
     double synergy=0;foreach(var old in prior)if(a.Effects==null&&old.District==a.District)
      synergy+=old.Money*a.MoneyBonus+a.Money*old.MoneyBonus+(old.Food*a.FoodBonus+a.Food*old.FoodBonus)/4;
     var n=new Plan{Steps=new(p.Steps),Cells=new(p.Cells),Counts=new(p.Counts),Barracks=p.Barracks+(a.Priority==8?1:0),
      Gains=new(p.Gains),PopulationAdd=p.PopulationAdd+a.PopulationAdd,Population=p.Population+a.PopulationFlow,Popular=p.Popular+a.PopularFlow,Security=p.Security+a.SecurityFlow,Commerce=p.Commerce+a.Commerce,Agriculture=p.Agriculture+a.Agriculture,Value=p.Value+a.BaseValue+synergy-a.Cost*.001,Money=p.Money+a.Money,Food=p.Food+a.Food};
     n.Steps.Add(i);n.Cells.UnionWith(a.Tiles);n.Counts[a.Id]=count+1;
     if(growth!=null)n.Population=population+growth(n.Commerce-commerce+n.Agriculture-agriculture,n.PopulationAdd);
     if(a.Effects!=null){string district=a.District??"";
      var districtSteps=n.Steps.Where(j=>actions[j].Effects!=null&&actions[j].District==a.District).ToArray();
      var outputKey=(district,string.Join(",",districtSteps));
      if(!districtOutputs.TryGetValue(outputKey,out var output)){
       var projected=baseDistricts.TryGetValue(district,out var original)?new List<BuildingEffectsPolicy.Building>(original):new();
       foreach(int j in districtSteps)projected.Add(actions[j].Effects);
       output=BuildingEffectsPolicy.Evaluate(projected,false);
       if(districtOutputs.Count>=4096)districtOutputs.Clear();
       districtOutputs[outputKey]=output;
      }
      if(!p.Outputs.TryGetValue(district,out var before))baseOutputs.TryGetValue(district,out before);
      n.Outputs=new(p.Outputs);n.Outputs[district]=output;n.Value=p.Value+a.BaseValue-a.Cost*.001+output.Value-before.Value;n.Money=p.Money+output.Money-before.Money;n.Food=p.Food+output.Food-before.Food;
      int special=a.Effects.Special;if(special>0&&!baseSpecials.Contains(special)&&!prior.Any(step=>step.Effects?.Special==special))n.Value+=100;
     }

foreach(var gain in a.Gains){n.Gains.TryGetValue(gain.Key,out double old);n.Gains[gain.Key]=old+gain.Value;}if(a.Effects!=null){n.Gains["money"]=n.Money;n.Gains["food"]=n.Food;}n.Goals=GoalScore(n.Gains);
     if(Compare(n,best)>0)best=n;
     // Bound memory, not candidate coverage: every building/site is scored.
     string signature=string.Join(",",n.Steps.OrderBy(x=>x));if(!signatures.Add(signature))continue;
     int insertion=next.FindIndex(old=>Compare(n,old)>0);if(insertion<0)insertion=next.Count;
     if(insertion<12){next.Insert(insertion,n);if(next.Count>12){var removed=next[12];next.RemoveAt(12);signatures.Remove(string.Join(",",removed.Steps.OrderBy(x=>x)));}}
     else signatures.Remove(signature);
    }
   }
   if(next.Count==0)break;beam=next;
  }
  // Execute mandatory capacity and goals before optional filling, without
  // changing the selected geometry. Cash is checked at each actual purchase.
  var ordered=new List<int>();var remaining=new List<int>(best.Steps);double pf=popular,sf=security,gf=population,ec=0,pa=0;
  while(remaining.Count>0){
   int selected=-1;foreach(int i in remaining){var a=actions[i];
    double nextGrowth=growth==null?gf+a.PopulationFlow:population+growth(ec+a.Commerce+a.Agriculture,pa+a.PopulationAdd);
    if(pf+a.PopularFlow<Math.Min(1,pf)||sf+a.SecurityFlow<Math.Min(1,sf)||nextGrowth<Math.Min(0,gf))continue;
    if(selected<0||a.Priority>actions[selected].Priority||a.Priority==actions[selected].Priority&&a.Cost<actions[selected].Cost)selected=i;
   }
   if(selected<0)break;var step=actions[selected];ordered.Add(selected);remaining.Remove(selected);pf+=step.PopularFlow;sf+=step.SecurityFlow;ec+=step.Commerce+step.Agriculture;pa+=step.PopulationAdd;gf=growth==null?gf+step.PopulationFlow:population+growth(ec,pa);
  }
  done(ordered);yield return true;
 }
}
