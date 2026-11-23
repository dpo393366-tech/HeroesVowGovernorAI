using System;
using System.Collections.Generic;
using System.Diagnostics;

public sealed partial class StrategicSupport
{
    // Recompute from saved buildings and goals, so research completion, a load,
    // or a governor change never depends on a stale in-memory demolition order.
    static bool DefensiveBuilding(PABIPDOFBFG b)=>b!=null&&b.HFBAJIGHOAF==MHCEFEODENB.Wall;
    static int DefenseShortfall(AFAGFHJONFJ c,MKMOJNAJKJE f,PABIPDOFBFG b){
        if(!DefensiveBuilding(b))return 0;
        int fronts=0;if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)
            if(n?.LMPMNLLGOEC!=null&&War(f,n.LMPMNLLGOEC.LMMNKIOAGLC))fronts++;
        // Threat raises urgency; it is never a quota that forbids filling an
        // otherwise legal wall site, including in rear cities.
        return 1+Math.Min(2,fronts);
    }
    static bool RenewalVictim(AFAGFHJONFJ c,JNHOOCJJPJO d,OGABIJECHFG x,PABIPDOFBFG b)=>
        x?.GEKKLMPLDHJ!=null&&x.GEKKLMPLDHJ.MLAJLJKFGBD==b.MLAJLJKFGBD&&x.GEKKLMPLDHJ.PNPLBNAFMCM==b.PNPLBNAFMCM
        &&(!RequiresStreet(b)||x.GEKKLMPLDHJ.FHKHHKBBKGM)&&RenewalProtected(c,d,x,b)&&ReplacementGoalsSafe(c,d,new(){x},b);
    static bool RenewalProtected(AFAGFHJONFJ c,JNHOOCJJPJO d,OGABIJECHFG x,PABIPDOFBFG b){
        var a=x?.GEKKLMPLDHJ;
        if(a==null||!PublicBuilding(c,d,x)||!Completed(x)||OccupiedBuilding(x)||!a.KFCEGGHJGGB||a.POLCGKHKLHI||EconomicKind(a)==0
            ||a.OJPECMNINNA==b.OJPECMNINNA||b.HFBAJIGHOAF==MHCEFEODENB.Wall)return false;
        // Non-economic effects and capacity are never silently traded away.
        // Even an equally large replacement contributes zero capacity during
        // reconstruction unless it is a native direct upgrade. Keep capacity
        // buildings out of demolition/rebuild; CanUpgrade handles safe upgrades.
        if(a.OFHFGEBGMBD!=0||a.DHLNIKKLNKJ!=0||a.IKELHGAHBID!=0||a.KGIMAFCNEKE!=0||a.LCKEPHGCAPC!=0
            ||a.IIBKLLKPNEH!=0||a.PEGLDBENJMF!=0||a.HIKFEJKBPIK!=0||a.DJNPCHEJGNM!=0||a.EOJNDEDNEKE!=0||a.LAIEKKPFOKF!=0||a.EMHGLEDIOHK!=0)return false;
        if(!CivicReplacement(c,new[]{x},b))return false;
        // A food emergency must not lose either production or its multipliers.
        if(FoodEmergency(c)&&(a.AJJJJNDNNME>b.AJJJJNDNNME||a.NIKFGJLNNCA>b.NIKFGJLNNCA||a.AIFPACNFKDB>b.AIFPACNFKDB))return false;
        // Losing an effect building can reduce every neighbour: allow only a
        // monotone replacement of district/adjacency multipliers.
        if(a.FIPONBEGNJO>b.FIPONBEGNJO||a.NIKFGJLNNCA>b.NIKFGJLNNCA||a.LGCANBOEGGH>b.LGCANBOEGGH||a.AIFPACNFKDB>b.AIFPACNFKDB)return false;
        if(c.HFLAGGEFBOO!=null)foreach(var goal in c.HFLAGGEFBOO){if(goal?.CKIJPJFFFOA==null)continue;foreach(var q in goal.CKIJPJFFFOA){
            if(q==null)continue;int target=q.KLCDGPDHENB;
            if(q.IJNEIHPHMEE==a.OJPECMNINNA){
                if(q.HFBAJIGHOAF==DDBHJKOIIPI.BuildingMax){int scale=(int)c.MPGFDBPEHBO(true);if(a.LJCICFHPMLM==null||scale<0||scale>=a.LJCICFHPMLM.Length)return false;target=a.LJCICFHPMLM[scale];}
                if((c.ELKFAJKBPKJ(a.OJPECMNINNA,false,true)?.Count??0)<=target)return false;
            }
            bool Loses(double current,double before,double after)=>after<before&&current-before+after<Math.Min(current,target);
            switch(q.HFBAJIGHOAF){
                case DDBHJKOIIPI.CityCommerce:if(Loses(c.NMDMENNPKDM(),a.CGBGIDCEBGD,b.CGBGIDCEBGD))return false;break;
                case DDBHJKOIIPI.CityAgriculture:if(Loses(c.NMKKHJEMMFH(),a.EGBEEDFCIGH,b.EGBEEDFCIGH))return false;break;
                case DDBHJKOIIPI.TwoBlockCommerce:if(Loses(d.IMMLKOLPPHF(),a.CGBGIDCEBGD,b.CGBGIDCEBGD))return false;break;
                case DDBHJKOIIPI.TwoBlockAgriculture:if(Loses(d.KMDCHCBIAIG(),a.EGBEEDFCIGH,b.EGBEEDFCIGH))return false;break;
                // Native income can include unknown multipliers. Don't pretend
                // raw building income proves a completed income goal survives.
                case DDBHJKOIIPI.CityMoneyIncome:if(!IncomeHeadroom(c,d,new[]{x},b,false,target))return false;break;
                case DDBHJKOIIPI.CityFoodIncome:if(!IncomeHeadroom(c,d,new[]{x},b,true,target))return false;break;
            }
        }}
        foreach(var pair in AEHAGJAKNEK.ACIGFKMDOKE)
            if(!string.IsNullOrEmpty(pair.Value?.EIMGMPDMAAJ)&&pair.Value.EIMGMPDMAAJ.Contains(a.OJPECMNINNA,StringComparison.Ordinal)
                &&(c.ELKFAJKBPKJ(a.OJPECMNINNA,false,true)?.Count??0)<=1)return false;
        return true;
    }
    static IEnumerable<bool> RenewalSteps(AFAGFHJONFJ c,MKMOJNAJKJE f){
        if(!maintenance.Value||!demolishBuildings.Value||!construction.Value||!Eligible(c,out var own)||own.Pointer!=f.Pointer||c.GKLIEHCHDLD()
            ||!AIGate("renewal-scan:"+c.OJPECMNINNA,f.IBGCLCAIBOL,20))yield break;
        string governor=c.LMPMNLLGOEC.IPJHOOJBCKI;var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
        var goals=ReadGoals(c);var actualTargets=new System.Collections.Generic.HashSet<string>(goals.Buildings.Keys);AddCapacityGoal(c,f,goals);var barracksMissing=BarracksMissing(c,f);var defs=new List<PABIPDOFBFG>();foreach(var p in AEHAGJAKNEK.ACIGFKMDOKE)defs.Add(p.Value);
        var rows=new List<(JNHOOCJJPJO d,OGABIJECHFG x)>();foreach(var d in c.FGBLEMGGOPO){if(d?.AIMLPNBOIIF==null)continue;foreach(var x in d.AIMLPNBOIIF)if(PublicBuilding(c,d,x))rows.Add((d,x));}
        OGABIJECHFG victim=null;PABIPDOFBFG target=null;double best=0;var clock=Stopwatch.StartNew();int probes=0;
        foreach(var b in defs){
            // Even rejected definitions call native eligibility/city counters.
            // Yield before each definition, including the no-legal-plot case.
            yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;
            if(barracksMissing.Count>0&&(b==null||!barracksMissing.ContainsKey(b.OJPECMNINNA)&&!actualTargets.Contains(b.OJPECMNINNA)))continue;
            if(!CanBuild(c,f,b,LocalReserve(c)))continue;
            bool required=goals.Buildings.ContainsKey(b.OJPECMNINNA),empty=false;
            if(!required&&goals.Buildings.Count>0)continue;
            // Always prefer an existing legal empty plot to losing a building.
            foreach(var d in c.FGBLEMGGOPO){if(d?.KFCOLLDEJNF!=HNDFFIEJNHI.Public||d.HGNFEMJOPPB==null)continue;
                foreach(var p in d.HGNFEMJOPPB){
                    if(++probes>=24||clock.Elapsed.TotalMilliseconds>=1){yield return false;probes=0;clock.Restart();}
                    if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;
                    if(p!=null&&p.BFPBNAKIKEO(b)){empty=true;break;}
                }if(empty)break;
            }if(empty)continue;
            foreach(var (d,x) in rows){
                if(++probes>=24||clock.Elapsed.TotalMilliseconds>=1){yield return false;probes=0;clock.Restart();}
                if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;
                if(!RenewalVictim(c,d,x,b))continue;
                double before=LocationValue(d,x.GEKKLMPLDHJ,x),after=LocationValue(d,b,x);
                if(!required&&!MaintenancePolicy.WorthMoving(before,after,b.NPCHPDNFAME))continue;
                // Goal construction may trade some income, but must have ample
                // runway and must not remove the city's last economic facility.
                if(required&&after<before&&(rows.Count<2||RenewalCash(c,f)<b.NPCHPDNFAME+30000L))continue;
                double score=(actualTargets.Contains(b.OJPECMNINNA)?300000:b.IKELHGAHBID>0&&CapacityShortfall(c)>0?200000:required?100000:0)+(after-before)/Math.Max(1,b.NPCHPDNFAME)-before*.001;
                if(score>best){best=score;victim=x;target=b;}
            }
        }
        if(victim==null){foreach(bool step in RebuildSteps(c,f,goals))yield return step;yield break;}
        if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor)||!Present(victim.GMIKIMACDBC,victim)
            ||!CanBuild(c,f,target,LocalReserve(c))||!RenewalVictim(c,victim.GMIKIMACDBC,victim,target)||!AIGate("renewal-commit:"+c.OJPECMNINNA,f.IBGCLCAIBOL,120))yield break;
        var old=victim.GEKKLMPLDHJ;int id=victim.OJPECMNINNA;
        var completedGoals=new List<HIADFOLCFII>();if(c.HFLAGGEFBOO!=null)foreach(var g in c.HFLAGGEFBOO)if(g!=null&&g.NHIPAJDALBA(c))completedGoals.Add(g);
        float oldProgress=victim.EMHNNCLBADE;int oldHealth=victim.OGFAHJHBCNG;
        void Convert(){
            Exception error=null;try{victim.GMMDMPFFIGP(target.OJPECMNINNA,false);}catch(Exception e){error=e;}
            bool invalid=false;foreach(var g in completedGoals)if(!g.NHIPAJDALBA(c)){invalid=true;break;}
            if(invalid&&victim.GEKKLMPLDHJ?.OJPECMNINNA!=old.OJPECMNINNA){
                try{victim.GMMDMPFFIGP(old.OJPECMNINNA,true);}finally{victim.EMHNNCLBADE=oldProgress;victim.OGFAHJHBCNG=oldHealth;}
                throw new InvalidOperationException($"Renewal invalidated completed goal; restored original city={c.OJPECMNINNA} instance={id} old={old.OJPECMNINNA} replacement={target.OJPECMNINNA}");
            }
            if(error!=null)throw error;
        }
        // Native GMMDMPFFIGP updates city definition counts, construction progress,
        // district caches AND the loaded scene's Ci_BuildingCtrl. Identical shape
        // and stricter street rule retain the old legal parcel. There is no empty
        // demolition interval, guessed tile mutation, free completion or refund.
        bool done=SpendBuilding(c,f,target,
            Convert,()=>victim.GEKKLMPLDHJ?.OJPECMNINNA!=old.OJPECMNINNA);
        if(done){nativeZones.Remove(victim.GMIKIMACDBC.Pointer);Note($"renewal-replace city={c.OJPECMNINNA} instance={id} old={old.OJPECMNINNA} new={target.OJPECMNINNA} cost={target.NPCHPDNFAME} nativeConstruction=true");yield return true;}
    }
}

