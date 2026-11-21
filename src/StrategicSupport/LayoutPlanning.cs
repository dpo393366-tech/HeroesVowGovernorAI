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
    sealed class LayoutIntent {public string Faction,Governor;public long Until;public List<string> Keys=new();public LayoutRevision Revision;}
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
    static IEnumerable<bool> ChooseLayoutSteps(AFAGFHJONFJ city,MKMOJNAJKJE faction,GoalPlan goals,List<LayoutChoice> all,Action<LayoutChoice,LayoutIntent> complete)
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
            layoutIntents.Remove(city.OJPECMNINNA);
        }
        var shortlist=all.GroupBy(x=>x.Action.Key).Select(g=>g.First()).ToList();
        long cash=Math.Max(0L,(long)city.LMPMNLLGOEC.DNOGPFHMOMO-LocalReserve(city))+Math.Max(0L,(long)faction.DNOGPFHMOMO-InvestmentReserve(faction));
        var normal=ReadCivicFlow(city);var actualGoals=ReadGoals(city);
        var populationModel=PopulationSnapshot(city);List<int> steps=new();foreach(bool step in CityLayoutOptimizer.Solve(shortlist.Select(x=>x.Action).ToList(),actualGoals.Needs,result=>steps=result,normal.Popular,normal.Security,city.NMDMENNPKDM(),city.NMKKHJEMMFH(),normal.Population,CityEffects(city),populationModel.Gain))yield return false;
        if(steps.Count==0){complete(null,null);yield break;}
        if(revision.Change(ReadLayoutRevision(city,faction))!=null){complete(null,null);yield break;}
        intent=new(){Faction=faction.IBGCLCAIBOL,Governor=city.LMPMNLLGOEC.IPJHOOJBCKI,Until=Environment.TickCount64+600000,Revision=revision};
        foreach(int step in steps)intent.Keys.Add(shortlist[step].Action.Key);
        if(steps.Count>1&&Gate("layout-log:"+city.OJPECMNINNA,10))Note($"layout-plan city={city.OJPECMNINNA} depth={steps.Count} sequence={string.Join(" -> ",steps.Select(i=>shortlist[i].Action.Id+"@"+shortlist[i].Action.District))} cashBudget={cash}");
        complete(shortlist[steps[0]],intent);yield return true;
    }
    static void CommitLayout(AFAGFHJONFJ city,LayoutIntent intent)
    {
        if(intent==null)return;if(intent.Keys.Count>0)intent.Keys.RemoveAt(0);
        if(NEINKOOBJIH.COCMABJFOFH.TryGetValue(intent.Faction,out var f))intent.Revision=ReadLayoutRevision(city,f);
        if(intent.Keys.Count>0)layoutIntents[city.OJPECMNINNA]=intent;else layoutIntents.Remove(city.OJPECMNINNA);
    }
}
