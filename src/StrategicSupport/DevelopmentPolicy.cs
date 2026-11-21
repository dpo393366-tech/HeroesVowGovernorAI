using System;
using System.Collections.Generic;
using System.Linq;

// Pure, bounded look-ahead. Never mutates native city objects during planning.
public static class DevelopmentPolicy
{
    public sealed class Need { public double Missing,Target; }
    public sealed class Action
    {
        public string Id,District,Key;
        public int Priority,Cost,Tech,Strategy,Zone,Limit=int.MaxValue;
        public long[] Tiles=Array.Empty<long>();
        public BuildingEffectsPolicy.Building Effects;
        public double PopulationFlow,PopulationAdd;
        public double BaseValue,Money,Food,MoneyBonus,FoodBonus,PopularFlow,SecurityFlow,Commerce,Agriculture;
        public Dictionary<string,double> Gains=new();
    }
    public static double GoalGain(Dictionary<string,Need> needs,Action action,IReadOnlyList<Action> prior)
    {
        double score=0;
        foreach(var pair in action.Gains){
            if(!needs.TryGetValue(pair.Key,out var need)||need.Missing<=0)continue;
            double supplied=0;foreach(var old in prior)if(old.Gains.TryGetValue(pair.Key,out double gain))supplied+=gain;
            double remaining=Math.Max(0,need.Missing-supplied);
            score+=1000*Math.Min(remaining,Math.Max(0,pair.Value))/Math.Max(1,need.Target)*(1+remaining/Math.Max(1,need.Target));
        }
        return score;
    }
    public static bool Overlap(Action a,Action b)
    {
        if(a.District!=b.District)return false;
        // Unknown footprints cannot safely be planned together.
        if(a.Tiles.Length==0||b.Tiles.Length==0)return true;
        foreach(long tile in a.Tiles)if(Array.IndexOf(b.Tiles,tile)>=0)return true;
        return false;
    }
    sealed class State {public List<int> Steps=new();public long Cost,Tech,Strategy;public double Value,Rank;}
    public static List<int> Plan(IReadOnlyList<Action> actions,Dictionary<string,Need> needs,long cash,int tech,int strategy){List<int> result=new();foreach(var step in PlanSteps(actions,needs,cash,tech,strategy,r=>result=r)){}return result;}
    public static IEnumerable<bool> PlanSteps(IReadOnlyList<Action> actions,Dictionary<string,Need> needs,long cash,int tech,int strategy,System.Action<List<int>> complete)
    {
        if(actions.Count==0){complete(new());yield break;}int probes=0;
        int firstPriority=actions.Max(a=>a.Priority);
        var beam=new List<State>{new()};State best=null;
        // Six buildings ahead, twelve alternatives retained per layer; future goal plots are reserved separately.
        // Native validation is rerun before each real purchase.
        for(int depth=0;depth<6;depth++){
            var next=new List<State>();
            foreach(var state in beam)for(int index=0;index<actions.Count;index++){
                if(++probes>=32){probes=0;yield return false;}var a=actions[index];if(depth==0&&a.Priority<firstPriority)continue;
                if(a.Cost<0||a.Tech<0||a.Strategy<0||state.Cost+a.Cost>cash||state.Tech+a.Tech>tech||state.Strategy+a.Strategy>strategy)continue;
                int same=0;bool blocked=false;var prior=new List<Action>();
                foreach(int oldIndex in state.Steps){var old=actions[oldIndex];prior.Add(old);if(old.Id==a.Id)same++;if(Overlap(old,a)||(old.District==a.District&&old.Zone>0&&a.Zone>0&&old.Zone!=a.Zone)){blocked=true;break;}}
                if(blocked||same>=a.Limit)continue;
                double synergy=0;
                foreach(var old in prior)if(old.District==a.District)
                    synergy+=old.Money*a.MoneyBonus+a.Money*old.MoneyBonus+(old.Food*a.FoodBonus+a.Food*old.FoodBonus)/4;
                double gain=Math.Max(0,a.BaseValue)+Math.Max(0,synergy)+GoalGain(needs,a,prior)*20;
                var n=new State{Steps=new(state.Steps),Cost=state.Cost+a.Cost,Tech=state.Tech+a.Tech,Strategy=state.Strategy+a.Strategy,
                    Value=state.Value+gain};n.Steps.Add(index);
                // Compare the whole package, not the artificially large sum of
                // per-building benefit/cost ratios. Later gains are discounted.
                n.Rank=n.Value/Math.Max(1,n.Cost)*(1-0.04*depth);
                next.Add(n);
                if(n.Value>0&&(best==null||n.Rank>best.Rank))best=n;
            }
            next.Sort((a,b)=>b.Rank.CompareTo(a.Rank));
            // Keep at least one branch per first action; zero-output effect
            // buildings must survive to pair with later production buildings.
            beam=new();var roots=new HashSet<int>();
            foreach(var n in next){var root=actions[n.Steps[0]];if((root.MoneyBonus>0||root.FoodBonus>0)&&roots.Add(n.Steps[0])){beam.Add(n);if(beam.Count>=4)break;}}
            foreach(var n in next)if(roots.Add(n.Steps[0])){beam.Add(n);if(beam.Count>=12)break;}
            if(beam.Count==0)break;
        }
        complete(best?.Steps??new());yield return true;
    }
}
