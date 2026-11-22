using System;
using System.Collections.Generic;
using System.Linq;

public sealed partial class StrategicSupport
{
    sealed class LayoutChoice
    {
        public PABIPDOFBFG Building;public PEGMABENGBB Place;public JNHOOCJJPJO District;
        public DevelopmentPolicy.Action Action;public double ImmediateScore;
    }
    sealed class LayoutIntent {public string Faction,Governor,CivicMode;public IntPtr City;public long Until;public List<string> Keys=new();public Dictionary<string,LayoutChoice> Choices=new();public LayoutRevision Revision;}
    static string LayoutCivicMode(AFAGFHJONFJ c){var flow=ReadCivicFlow(c);return $"{flow.Popular<=0}/{flow.Security<=0}/{flow.Population<0}/{FoodEmergency(c)}";}
    static LayoutIntent EmptyLayout(AFAGFHJONFJ c,MKMOJNAJKJE f)=>new(){Faction=f.IBGCLCAIBOL,Governor=c.LMPMNLLGOEC.IPJHOOJBCKI,City=c.Pointer,Revision=ReadLayoutRevision(c,f),CivicMode=LayoutCivicMode(c)};
    static readonly Dictionary<string,LayoutIntent> layoutIntents=new();
    static IEnumerable<FEEDONDCELP> PlotCells(PEGMABENGBB p){
        if(p?.FAFNFFJAFKH==null)yield break;long area=(long)p.PABEMCAGKJJ*p.HAPLIFADGBA;
        if(p.PABEMCAGKJJ<=0||p.HAPLIFADGBA<=0||area>p.FAFNFFJAFKH.Length)yield break;
        // Native BFPBNAKIKEO iterates width*height, not pooled array capacity.
        for(int i=0;i<area;i++)yield return p.FAFNFFJAFKH[i];
    }
    static long[] Footprint(PEGMABENGBB p){var tiles=new List<long>();foreach(var q in PlotCells(p))if(q!=null)tiles.Add(q.Pointer.ToInt64());tiles.Sort();return tiles.ToArray();}
    static LayoutChoice MakeLayoutChoice(AFAGFHJONFJ city,DistrictPlan d,PABIPDOFBFG b,PEGMABENGBB p,int priority,double value,double score)
    {
        var tiles=Footprint(p);int limit=int.MaxValue;int scale=(int)city.MPGFDBPEHBO(true);
        if(b.LJCICFHPMLM!=null&&scale>=0&&scale<b.LJCICFHPMLM.Length&&b.LJCICFHPMLM[scale]>=0)
            limit=Math.Max(0,b.LJCICFHPMLM[scale]-(city.ELKFAJKBPKJ(b.OJPECMNINNA,true,true)?.Count??0));
        string key=d.District.OJPECMNINNA+":"+b.OJPECMNINNA+":"+(tiles.Length>0?string.Join(",",tiles):p.Pointer.ToString());
        return new(){Building=b,Place=p,District=d.District,ImmediateScore=score,Action=new(){
            Id=b.OJPECMNINNA,District=d.District.OJPECMNINNA,Key=key,Tiles=tiles,Priority=priority,Cost=b.NPCHPDNFAME,
            Tech=Math.Max(0,b.ODKKMLDAPJL),Strategy=Math.Max(0,b.ADGOBJJMEAO),Zone=0,Limit=limit,
            PopularFlow=b.DPFIFIOKHEI*30,SecurityFlow=b.HKBPGJLMFIG*30,Commerce=b.CGBGIDCEBGD,Agriculture=b.EGBEEDFCIGH,BaseValue=value,Money=b.CNCPIILGDEF,Food=b.AJJJJNDNNME,
            Effects=BuildingProfile(b,d.District.OJPECMNINNA,PlotCells(p)),PopulationAdd=b.DHLNIKKLNKJ,MoneyBonus=b.FIPONBEGNJO,FoodBonus=b.NIKFGJLNNCA,Gains=GoalContributions(d,b)}};
    }
    static LayoutChoice ChooseLayout(AFAGFHJONFJ city,MKMOJNAJKJE faction,GoalPlan goals,List<LayoutChoice> all,out LayoutIntent intent){LayoutChoice result=null;LayoutIntent plan=null;foreach(var step in ChooseLayoutSteps(city,faction,goals,all,(x,y)=>{result=x;plan=y;})){}intent=plan;return result;}
    static IEnumerable<bool> ChooseLayoutSteps(AFAGFHJONFJ city,MKMOJNAJKJE faction,GoalPlan goals,List<LayoutChoice> all,Action<LayoutChoice,LayoutIntent> complete,bool background=false,Action<string> interrupted=null)
    {
        LayoutIntent intent;var revision=ReadLayoutRevision(city,faction);
        // Unlocked catalogue owns the current plan; newly unlocked goals trigger replanning and replacement.
        intent=null;if(all.Count==0){layoutIntents.Remove(city.OJPECMNINNA);complete(null,null);yield break;}
        int priority=all.Max(x=>x.Action.Priority);
        if(layoutIntents.TryGetValue(city.OJPECMNINNA,out var pending)){
            string changed=pending.Revision?.Change(revision)??(pending.Revision==null?"missing-state":null);
            if(changed==null&&pending.Faction==faction.IBGCLCAIBOL&&pending.Governor==city.LMPMNLLGOEC.IPJHOOJBCKI&&pending.Keys.Count>0){
                var follow=all.FirstOrDefault(x=>x.Action.Key==pending.Keys[0]);
                if(follow!=null&&CivicBuildBlock(city,follow.Building)==null){intent=pending;complete(follow,intent);yield break;}
            }
            Note($"layout-replan city={city.OJPECMNINNA} reason={changed??"owner-budget-priority-or-expired"} remaining={pending.Keys.Count}");
            nativeGoalSites.Remove(city.Pointer);
            if(!background)layoutIntents.Remove(city.OJPECMNINNA);
        }
        var shortlist=all.GroupBy(x=>x.Action.Key).Select(g=>g.First()).ToList();
        long cash=Math.Max(0L,(long)city.LMPMNLLGOEC.DNOGPFHMOMO-LocalReserve(city))+Math.Max(0L,(long)faction.DNOGPFHMOMO-InvestmentReserve(faction));
        var normal=ReadCivicFlow(city);var actualGoals=ReadGoals(city);
        long planStarted=Environment.TickCount64;int scanSteps=0;
        var populationModel=PopulationSnapshot(city);List<int> steps=new();foreach(bool step in CityLayoutOptimizer.Solve(shortlist.Select(x=>x.Action).ToList(),actualGoals.Needs,result=>steps=result,normal.Popular,normal.Security,city.NMDMENNPKDM(),city.NMKKHJEMMFH(),normal.Population,CityEffects(city),populationModel.Gain)){
            if(++scanSteps%2048==0&&Gate("layout-working:"+city.OJPECMNINNA,30))Note($"layout-working city={city.OJPECMNINNA} legalChoices={shortlist.Count} scanSteps={scanSteps} elapsedIncludingQueueMs={Environment.TickCount64-planStarted}");
            yield return false;
        }
        if(steps.Count==0){
            if(Gate("layout-empty:"+city.OJPECMNINNA,30))Note($"layout-empty city={city.OJPECMNINNA} legalChoices={shortlist.Count} zeroLimitChoices={shortlist.Count(x=>x.Action.Limit<=0)} popular={normal.Popular} security={normal.Security} population={normal.Population} populationRaw={populationModel.Raw} populationFull={populationModel.Full} nativeBuildScope={nativeBuildScope} buildings=[{string.Join(";",shortlist.GroupBy(x=>x.Action.Id).Select(g=>$"{g.Key}:sites={g.Count()}:limit={g.First().Action.Limit}:priority={g.First().Action.Priority}:popular={g.First().Action.PopularFlow}:security={g.First().Action.SecurityFlow}:population={g.First().Action.PopulationAdd}"))}]");
            complete(null,null);yield break;
        }
        var currentRevision=ReadLayoutRevision(city,faction);
        if(revision.StructureChange(currentRevision) is string invalidated){Note($"layout-plan-discard city={city.OJPECMNINNA} reason={invalidated} legalChoices={shortlist.Count} elapsedIncludingQueueMs={Environment.TickCount64-planStarted}");interrupted?.Invoke(invalidated);complete(null,null);yield break;}
        // Completion of existing work or a changing goal counter does not
        // invalidate vacant geometry. The first purchase still rechecks native
        // caps, money, materials, occupancy and actual civic flows. Replan the
        // remainder instead of repeatedly abandoning every construction action.
        bool progressed=revision.Progress!=currentRevision.Progress;
        if(progressed)Note($"layout-progress-revalidate city={city.OJPECMNINNA} first={shortlist[steps[0]].Action.Id} remainingGeometryRetained=true");
        intent=new(){Faction=faction.IBGCLCAIBOL,Governor=city.LMPMNLLGOEC.IPJHOOJBCKI,City=city.Pointer,Until=Environment.TickCount64+600000,Revision=currentRevision,CivicMode=LayoutCivicMode(city)};
        foreach(int step in steps){var entry=shortlist[step];intent.Keys.Add(entry.Action.Key);intent.Choices[entry.Action.Key]=entry;}
        if(steps.Count>1&&Gate("layout-log:"+city.OJPECMNINNA,10))Note($"layout-plan city={city.OJPECMNINNA} depth={steps.Count} sequence={string.Join(" -> ",steps.Select(i=>shortlist[i].Action.Id+"@"+shortlist[i].Action.District))} cashBudget={cash}");
        complete(shortlist[steps[0]],intent);yield return true;
    }
    static void CommitLayout(AFAGFHJONFJ city,LayoutIntent intent)
    {
        if(intent==null)return;if(intent.Keys.Count>0)intent.Keys.RemoveAt(0);
        if(NEINKOOBJIH.COCMABJFOFH.TryGetValue(intent.Faction,out var f))intent.Revision=ReadLayoutRevision(city,f);
        if(intent.Keys.Count>0)layoutIntents[city.OJPECMNINNA]=intent;else layoutIntents.Remove(city.OJPECMNINNA);
    }
    // 1 = purchased, 2 = valid plan awaiting resources, 0 = needs planning.
    // Never hold money/material reservations across a queue yield: the shared
    // transaction validates and pays atomically on Unity's main thread.
    static int ExecuteSavedLayout(AFAGFHJONFJ c,MKMOJNAJKJE f,out bool refresh){
        refresh=true;
        if(!layoutIntents.TryGetValue(c.OJPECMNINNA,out var plan)||plan.Revision==null||plan.City!=c.Pointer||plan.Faction!=f.IBGCLCAIBOL||plan.Governor!=c.LMPMNLLGOEC.IPJHOOJBCKI)return 0;
        var revision=ReadLayoutRevision(c,f);string change=plan.Revision.StructureChange(revision);
        if(change!=null&&change!="buildings-changed")return 0;
        if(plan.Keys.Count==0){refresh=plan.Revision.Change(revision)!=null||plan.CivicMode!=LayoutCivicMode(c);return refresh?0:2;}
        refresh=change!=null;
        var goals=ReadGoals(c);var barracks=BarracksMissing(c,f);var civic=ReadCivicFlow(c);
        // Completed/registered buildings already satisfy projected counts in
        // ReadGoals. Reorder the retained geometry, not the entire city search.
        int Priority(LayoutChoice x)=>goals.Buildings.ContainsKey(x.Building.OJPECMNINNA)?10:
            (civic.Popular<=0&&x.Building.DPFIFIOKHEI>0||civic.Security<=0&&x.Building.HKBPGJLMFIG>0)?9:
            barracks.ContainsKey(x.Building.OJPECMNINNA)?8:1;
        var choices=plan.Keys.Where(plan.Choices.ContainsKey).Select(k=>plan.Choices[k]).OrderByDescending(Priority).ToList();
        if(choices.Count==0){refresh=true;return 0;}
        foreach(var choice in choices){
            string district=choice.District.OJPECMNINNA;
            if(change!=null&&(!plan.Revision.Districts.TryGetValue(district,out var old)||!revision.Districts.TryGetValue(district,out var now)||old!=now))continue;
            if(!CurrentPlot(c,choice.District,choice.Place,choice.Action.Tiles)||!choice.Place.BFPBNAKIKEO(choice.Building)){refresh=true;continue;}
            string blocked=BuildBlock(c,f,choice.Building,LocalReserve(c));
            if(blocked=="cash"||blocked=="research-or-strategy-points"||blocked=="upgrade-or-item-cost")return 2;
            if(blocked!=null||CivicBuildBlock(c,choice.Building)!=null){refresh=true;continue;}
            // A newly missing mandatory building needs a new plan before any
            // optional square is spent. Redevelopment continues independently.
            bool buildableGoal=goals.Buildings.Keys.Any(id=>AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(id,out var b)&&BuildBlock(c,f,b,LocalReserve(c),true)==null);
            if(Priority(choice)<8&&(buildableGoal||barracks.Count>0)){refresh=true;return 0;}
            plan.Keys.Remove(choice.Action.Key);plan.Keys.Insert(0,choice.Action.Key);
            // Preserve unaffected districts while a background plan refreshes
            // changes elsewhere. Do not bless stale geometry after committing.
            var previous=plan.Revision;
            bool bought=PurchaseLayout(c,f,choice,plan,revision);
            if(refresh&&bought)plan.Revision=previous;
            if(!bought)refresh=true;
            return bought?1:0;
        }
        refresh=true;return 0;
    }
}
