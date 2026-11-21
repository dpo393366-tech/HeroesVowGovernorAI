using System;
using System.Collections.Generic;

public sealed partial class StrategicSupport
{
    sealed class GoalPlan
    {
        public Dictionary<string,int> Buildings=new();
        public Dictionary<string,DevelopmentPolicy.Need> Needs=new();
    }
    static bool UpgradePreservesRequiredCount(AFAGFHJONFJ c,PABIPDOFBFG old,PABIPDOFBFG replacement=null){
        if(c.HFLAGGEFBOO==null)return true;
        foreach(var goal in c.HFLAGGEFBOO)if(goal?.CKIJPJFFFOA!=null)foreach(var q in goal.CKIJPJFFFOA){
            if(q==null||string.IsNullOrEmpty(q.IJNEIHPHMEE)||(q.IJNEIHPHMEE!=old.OJPECMNINNA&&q.IJNEIHPHMEE!=old.MDOIJIAONJM))continue;int required=q.KLCDGPDHENB;
            int count=c.ELKFAJKBPKJ(q.IJNEIHPHMEE,false,true)?.Count??0;
            if(q.HFBAJIGHOAF==DDBHJKOIIPI.BuildingMax){int scale=(int)c.MPGFDBPEHBO(true);var definition=old;if(q.IJNEIHPHMEE!=old.OJPECMNINNA&&!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(q.IJNEIHPHMEE,out definition))return false;if(definition.LJCICFHPMLM==null||scale<0||scale>=definition.LJCICFHPMLM.Length)return false;required=definition.LJCICFHPMLM[scale];}
            else if(q.HFBAJIGHOAF!=DDBHJKOIIPI.BuildingNum)continue;
            // Native goal checks call ELKFAJKBPKJ(id,false,true): a completed
            // direct upgrade counts toward its base definition's requirement.
            // Do not mistake this in-place upgrade for demolition of the base.
            bool stillCounts=replacement!=null&&(replacement.OJPECMNINNA==q.IJNEIHPHMEE||replacement.MDOIJIAONJM==q.IJNEIHPHMEE);
            if(required>0&&count<=required&&!stillCounts)return false;
        }return true;
    }
    static void AddNeed(GoalPlan plan,string key,double target,double current)
    {
        double missing=Math.Max(0,target-current);if(missing<=0)return;
        if(!plan.Needs.TryGetValue(key,out var prior)||missing>prior.Missing)
            plan.Needs[key]=new(){Missing=missing,Target=Math.Max(1,target)};
    }
    static GoalPlan ReadGoals(AFAGFHJONFJ c)
    {
        var plan=new GoalPlan();if(c.HFLAGGEFBOO==null)return plan;
        int scale=(int)c.MPGFDBPEHBO(true);
        // Registered construction already consumes a place and is a committed
        // investment. Use it as a floor for planned output, not a second addition
        // to native totals (which may already include part of that output).
        double projectedCommerce=0,projectedAgriculture=0,projectedMoney=0,projectedFood=0,projectedSecurity=0,projectedPopular=0;
        var registered=new Dictionary<string,int>();
        if(c.FGBLEMGGOPO!=null)foreach(var d in c.FGBLEMGGOPO){
            if(d?.AIMLPNBOIIF==null)continue;
            foreach(var x in d.AIMLPNBOIIF){var b=x?.GEKKLMPLDHJ;if(b==null)continue;
                registered.TryGetValue(b.OJPECMNINNA,out int n);registered[b.OJPECMNINNA]=n+1;
                if(!string.IsNullOrEmpty(b.MDOIJIAONJM)&&b.MDOIJIAONJM!=b.OJPECMNINNA){registered.TryGetValue(b.MDOIJIAONJM,out int baseN);registered[b.MDOIJIAONJM]=baseN+1;}
                projectedCommerce+=Math.Max(0,b.CGBGIDCEBGD);projectedAgriculture+=Math.Max(0,b.EGBEEDFCIGH);
                projectedMoney+=Math.Max(0,b.CNCPIILGDEF);projectedFood+=Math.Max(0,b.AJJJJNDNNME);
                projectedSecurity+=Math.Max(0,b.HKBPGJLMFIG);projectedPopular+=Math.Max(0,b.DPFIFIOKHEI);
            }
        }
        double commerce=Math.Max(c.NMDMENNPKDM(),projectedCommerce),agriculture=Math.Max(c.NMKKHJEMMFH(),projectedAgriculture);
        double money=Math.Max(c.EMPGDOLPPFI(),projectedMoney),food=Math.Max(c.NBAKOCHGMBM(false),projectedFood);
        double security=Math.Max(c.CBDMPGOEMMK(c.LMPMNLLGOEC.LMMNKIOAGLC),projectedSecurity),popular=Math.Max(c.PCNOLBELFMF(),projectedPopular);
        foreach(var goal in c.HFLAGGEFBOO){
            if(goal==null||goal.PIHDGCBCMCP||goal.DMNMNBGHNKM>scale||goal.CKIJPJFFFOA==null||goal.NHIPAJDALBA(c))continue;
            foreach(var condition in goal.CKIJPJFFFOA){
                if(condition==null)continue;int target=condition.KLCDGPDHENB;
                switch(condition.HFBAJIGHOAF){
                    case DDBHJKOIIPI.CityCommerce:AddNeed(plan,"commerce",target,commerce);break;
                    case DDBHJKOIIPI.CityAgriculture:AddNeed(plan,"agriculture",target,agriculture);break;
                    case DDBHJKOIIPI.CityMoneyIncome:AddNeed(plan,"money",target,money);break;
                    case DDBHJKOIIPI.CityFoodIncome:AddNeed(plan,"food",target,food);break;
                    case DDBHJKOIIPI.CitySecurity:AddNeed(plan,"security",target,security);break;
                    case DDBHJKOIIPI.CityPopularFeel:AddNeed(plan,"popular",target,popular);break;
                    case DDBHJKOIIPI.TwoBlockCommerce:AddDistrictNeeds(plan,c,true,target);break;
                    case DDBHJKOIIPI.TwoBlockAgriculture:AddDistrictNeeds(plan,c,false,target);break;
                }
                if(string.IsNullOrEmpty(condition.IJNEIHPHMEE))continue;
                int required=target;
                if(condition.HFBAJIGHOAF==DDBHJKOIIPI.BuildingMax){
                    if(!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(condition.IJNEIHPHMEE,out var b)||b.LJCICFHPMLM==null||scale<0||scale>=b.LJCICFHPMLM.Length)continue;
                    required=b.LJCICFHPMLM[scale];
                }else if(condition.HFBAJIGHOAF!=DDBHJKOIIPI.BuildingNum)continue;
                int current=c.ELKFAJKBPKJ(condition.IJNEIHPHMEE,false,true)?.Count??0;
                registered.TryGetValue(condition.IJNEIHPHMEE,out int planned);current=Math.Max(current,planned);
                if(required>current){plan.Buildings.TryGetValue(condition.IJNEIHPHMEE,out int n);plan.Buildings[condition.IJNEIHPHMEE]=Math.Max(n,required-current);AddNeed(plan,"building:"+condition.IJNEIHPHMEE,required,current);}
            }
        }
        return plan;
    }
    static void AddDistrictNeeds(GoalPlan plan,AFAGFHJONFJ city,bool commerce,int target)
    {
        if(city.FGBLEMGGOPO==null||target<=0)return;
        var deficits=new List<(JNHOOCJJPJO District,double Current)>();int complete=0;
        foreach(var d in city.FGBLEMGGOPO){
            if(d==null||d.KFCOLLDEJNF!=HNDFFIEJNHI.Public)continue;
            double planned=0;if(d.AIMLPNBOIIF!=null)foreach(var x in d.AIMLPNBOIIF)if(x?.GEKKLMPLDHJ!=null)planned+=Math.Max(0,commerce?x.GEKKLMPLDHJ.CGBGIDCEBGD:x.GEKKLMPLDHJ.EGBEEDFCIGH);
            double current=Math.Max(commerce?d.IMMLKOLPPHF():d.KMDCHCBIAIG(),planned);
            if(current>=target){complete++;continue;}
            deficits.Add((d,current));
        }
        deficits.Sort((a,b)=>b.Current.CompareTo(a.Current));
        for(int i=0;i<Math.Min(Math.Max(0,2-complete),deficits.Count);i++)
            AddNeed(plan,(commerce?"block-commerce:":"block-agriculture:")+deficits[i].District.OJPECMNINNA,target,deficits[i].Current);
    }
    static Dictionary<string,double> GoalContributions(DistrictPlan d,PABIPDOFBFG b)
        =>new(){["commerce"]=Math.Max(0,b.CGBGIDCEBGD),["agriculture"]=Math.Max(0,b.EGBEEDFCIGH),
            ["money"]=Math.Max(0,b.CNCPIILGDEF)+Math.Max(0,d.Money*b.FIPONBEGNJO),["food"]=Math.Max(0,b.AJJJJNDNNME)+Math.Max(0,d.Food*b.NIKFGJLNNCA),
            ["security"]=Math.Max(0,b.HKBPGJLMFIG),["popular"]=Math.Max(0,b.DPFIFIOKHEI),
            ["block-commerce:"+d.District.OJPECMNINNA]=Math.Max(0,b.CGBGIDCEBGD),["block-agriculture:"+d.District.OJPECMNINNA]=Math.Max(0,b.EGBEEDFCIGH),
            ["building:"+b.OJPECMNINNA]=1};
    static double GoalBenefit(GoalPlan g,DistrictPlan d,PABIPDOFBFG b)
        =>DevelopmentPolicy.GoalGain(g.Needs,new DevelopmentPolicy.Action{Gains=GoalContributions(d,b)},Array.Empty<DevelopmentPolicy.Action>());
}

