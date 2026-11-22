using System;
using System.Collections.Generic;
using System.Diagnostics;
using BepInEx.Configuration;

public sealed partial class StrategicSupport
{
    static ConfigEntry<bool> maintenance,repairBuildings,upgradeBuildings,relocateBuildings,demolishBuildings;
    sealed class RemodelJob {public OGABIJECHFG Old,Replacement;public JNHOOCJJPJO Source,Target;public string Faction,Governor,Definition;}
    static readonly Dictionary<string,RemodelJob> remodelJobs=new();
    void ConfigureMaintenance()
    {
        maintenance=Config.Bind("Maintenance","Enabled",true,"Repair/upgrade public buildings in AI-managed cities outside sieges; destructive remodeling is deferred in the currently loaded city.");
        repairBuildings=Config.Bind("Maintenance","Repair",true,"Pay for extra durability repair, up to 25 percent per management visit; passive native repair continues.");
        upgradeBuildings=Config.Bind("Maintenance","Upgrade",true,"Buy affordable native upgrades without bypassing research, prerequisites, costs or construction time.");
        relocateBuildings=Config.Bind("Maintenance","Relocate",true,"Build a replacement in a more productive district; retire the old building only after its replacement is complete. Requires Demolish.");
        demolishBuildings=Config.Bind("Maintenance","Demolish",true,"Retire misplaced duplicate economic buildings only when a completed replacement exists and city requirements remain protected.");
    }
    static bool Completed(OGABIJECHFG x)=>x!=null&&!x.EHABIIDOKGC()&&float.IsFinite(x.EMHNNCLBADE)&&float.IsFinite(x.BNMEOOJPOML);
    static bool PublicBuilding(AFAGFHJONFJ c,JNHOOCJJPJO d,OGABIJECHFG x)
        =>d!=null&&d.KFCOLLDEJNF==HNDFFIEJNHI.Public&&d.AJJCCPCJPFC?.Pointer==c.Pointer
        &&x!=null&&x.GMIKIMACDBC?.Pointer==d.Pointer&&x.GEKKLMPLDHJ!=null&&!x.MGLFFEANDOI;
    static bool Present(JNHOOCJJPJO d,OGABIJECHFG x)
    {
        if(d?.AIMLPNBOIIF==null||x==null)return false;
        foreach(var a in d.AIMLPNBOIIF)if(a?.Pointer==x.Pointer)return true;return false;
    }
    static bool OccupiedBuilding(OGABIJECHFG x)=>x.JOFCACIPJKK!=0||!string.IsNullOrEmpty(x.LHCJPKGANCK)
        ||(x.JMIADAFPGMC?.Count??0)>0||(x.HNEPDNGMAIL?.Count??0)>0;
    static bool SpendMaintenance(AFAGFHJONFJ c,MKMOJNAJKJE f,int cost,int tech,int strategy,Action execute,Func<bool> changed)
    {
        int local=LocalReserve(c);
        if(cost<0||tech<0||strategy<0||f.IIBKLLKPNEH<tech||f.PEGLDBENJMF<strategy
            ||!InvestmentPolicy.CanPurchase(f.DNOGPFHMOMO,c.LMPMNLLGOEC.DNOGPFHMOMO,cost,InvestmentReserve(f),local))return false;
        int transfer=InvestmentPolicy.TransferForPurchase(f.DNOGPFHMOMO,c.LMPMNLLGOEC.DNOGPFHMOMO,cost,InvestmentReserve(f),local);
        var row=c.LMPMNLLGOEC;int cash=row.DNOGPFHMOMO,treasury=f.DNOGPFHMOMO,t=f.IIBKLLKPNEH,s=f.PEGLDBENJMF;
        bool paid=false;
        try{
            f.DNOGPFHMOMO=treasury-transfer;row.DNOGPFHMOMO=cash+transfer-cost;
            f.IIBKLLKPNEH=t-tech;f.PEGLDBENJMF=s-strategy;paid=true;
            var nativeClock=Stopwatch.StartNew();
            try{execute();return changed();}finally{if(nativeClock.Elapsed.TotalMilliseconds>=8)Note($"maintenance-native city={c.OJPECMNINNA} elapsedMs={nativeClock.Elapsed.TotalMilliseconds:F2} cost={cost}");}
        }finally{
            // A native scene/cache refresh can throw after changing saved data.
            // Retain the payment whenever the data changed, including partial changes.
            if(paid&&changed()){var cycle=Budget(f);cycle.ConstructionSpent=(int)Math.Min(int.MaxValue,(long)cycle.ConstructionSpent+transfer);}
            else {f.DNOGPFHMOMO=treasury;row.DNOGPFHMOMO=cash;f.IIBKLLKPNEH=t;f.PEGLDBENJMF=s;}
        }
    }
    static double LocationValue(JNHOOCJJPJO d,PABIPDOFBFG b,OGABIJECHFG omit=null,PEGMABENGBB p=null)
    {
        var baseline=DistrictEffects(d,omit==null?null:new[]{omit});var before=BuildingEffectsPolicy.Evaluate(baseline);
        var added=BuildingProfile(b,d.OJPECMNINNA,p!=null?PlotCells(p):BuildingCells(omit));
        baseline.Add(added);return BuildingEffectsPolicy.Evaluate(baseline).Value-before.Value;
    }

    static bool PreservesOutputs(PABIPDOFBFG a,PABIPDOFBFG b)=>b.CNCPIILGDEF>=a.CNCPIILGDEF&&b.AJJJJNDNNME>=a.AJJJJNDNNME
        &&b.CGBGIDCEBGD>=a.CGBGIDCEBGD&&b.EGBEEDFCIGH>=a.EGBEEDFCIGH&&b.HKBPGJLMFIG>=a.HKBPGJLMFIG&&b.DPFIFIOKHEI>=a.DPFIFIOKHEI
        &&b.FIPONBEGNJO>=a.FIPONBEGNJO&&b.NIKFGJLNNCA>=a.NIKFGJLNNCA&&b.LGCANBOEGGH>=a.LGCANBOEGGH&&b.AIFPACNFKDB>=a.AIFPACNFKDB
        &&b.IKELHGAHBID>=a.IKELHGAHBID&&b.OGFAHJHBCNG>=a.OGFAHJHBCNG&&BuildingEffectsPolicy.PreservesUtility(BuildingProfile(a,""),BuildingProfile(b,""));
    static bool CanUpgrade(AFAGFHJONFJ c,MKMOJNAJKJE f,OGABIJECHFG x,PABIPDOFBFG b)=>UpgradeBlock(c,f,x,b)==null;
    static string UpgradeBlock(AFAGFHJONFJ c,MKMOJNAJKJE f,OGABIJECHFG x,PABIPDOFBFG b){
        var a=x?.GEKKLMPLDHJ;
        if(a==null||b==null||b.OJPECMNINNA==a.OJPECMNINNA||b.MDOIJIAONJM!=a.OJPECMNINNA)return "not-direct-upgrade";
        if(b.OCJNCECECMH==FFDDNPOHLHN.Private||b.POLCGKHKLHI||b.NPCHPDNFAME<0||b.ODKKMLDAPJL<0||b.ADGOBJJMEAO<0)return "invalid-or-private";
        if(b.MLAJLJKFGBD!=a.MLAJLJKFGBD||b.PNPLBNAFMCM!=a.PNPLBNAFMCM)return "different-footprint";
        if(NativeUpgradePointsBanned(f,b))return "native-upgrade-points-disabled";
        if(!HasMaterials(f,b))return "materials";
        if(!PreservesOutputs(a,b))return "output-loss";
        if(!CivicReplacement(c,new[]{x},b))return "civic-or-population-loss";
        if(!UpgradePreservesRequiredCount(c,a,b))return "goal-count";
        var seen=new HashSet<string>{b.OJPECMNINNA};var node=a;
        for(int depth=0;node!=null;depth++){
            if(depth>=32||!seen.Add(node.OJPECMNINNA))return "cyclic-chain";
            if(string.IsNullOrEmpty(node.MDOIJIAONJM))break;
            if(!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(node.MDOIJIAONJM,out node))return "missing-base-definition";
        }
        if(!b.PLEHAHBBLGL(f.KCKGONGIMDA))return "technology";
        if(!b.MCGEOICKKEE(c,false))return "prerequisite";
        if(b.LOJCCNNJKHF(c,false))return "native-limit";
        if(f.IIBKLLKPNEH<b.ODKKMLDAPJL||f.PEGLDBENJMF<b.ADGOBJJMEAO)return "points";
        return InvestmentPolicy.CanPurchase(f.DNOGPFHMOMO,c.LMPMNLLGOEC.DNOGPFHMOMO,b.NPCHPDNFAME,InvestmentReserve(f),LocalReserve(c))?null:"cash-reserve";
    }
    static bool Retirable(AFAGFHJONFJ c,JNHOOCJJPJO d,OGABIJECHFG x)
    {
        var b=x.GEKKLMPLDHJ;
        if(!Completed(x)||OccupiedBuilding(x)||!b.KFCEGGHJGGB||b.POLCGKHKLHI||EconomicKind(b)==0
            ||!string.IsNullOrEmpty(b.MDOIJIAONJM)||(int)b.OFHFGEBGMBD!=0||b.DHLNIKKLNKJ!=0
            ||b.IKELHGAHBID!=0||b.KGIMAFCNEKE!=0||b.LCKEPHGCAPC!=0||b.HKBPGJLMFIG!=0||b.DPFIFIOKHEI!=0
            ||b.IIBKLLKPNEH!=0||b.PEGLDBENJMF!=0||b.HIKFEJKBPIK!=0||b.DJNPCHEJGNM!=0||b.EOJNDEDNEKE!=0||b.LAIEKKPFOKF!=0||b.EMHGLEDIOHK!=0||FoodEmergency(c))return false;
        // Preserve every explicit city goal (including already achieved goals),
        // and buildings referenced as a prerequisite by any registered definition.
        if(c.HFLAGGEFBOO!=null)foreach(var goal in c.HFLAGGEFBOO){if(goal?.CKIJPJFFFOA==null)continue;foreach(var condition in goal.CKIJPJFFFOA){
            if(condition==null)continue;
            if(condition.IJNEIHPHMEE==b.OJPECMNINNA)return false;
            if(condition.HFBAJIGHOAF==DDBHJKOIIPI.TwoBlockCommerce&&b.CGBGIDCEBGD>0&&d.IMMLKOLPPHF()>=condition.KLCDGPDHENB&&d.IMMLKOLPPHF()-b.CGBGIDCEBGD<condition.KLCDGPDHENB)return false;
            if(condition.HFBAJIGHOAF==DDBHJKOIIPI.TwoBlockAgriculture&&b.EGBEEDFCIGH>0&&d.KMDCHCBIAIG()>=condition.KLCDGPDHENB&&d.KMDCHCBIAIG()-b.EGBEEDFCIGH<condition.KLCDGPDHENB)return false;
            if(condition.HFBAJIGHOAF==DDBHJKOIIPI.CityCommerce&&c.NMDMENNPKDM()>=condition.KLCDGPDHENB&&c.NMDMENNPKDM()-b.CGBGIDCEBGD<condition.KLCDGPDHENB)return false;
            if(condition.HFBAJIGHOAF==DDBHJKOIIPI.CityAgriculture&&c.NMKKHJEMMFH()>=condition.KLCDGPDHENB&&c.NMKKHJEMMFH()-b.EGBEEDFCIGH<condition.KLCDGPDHENB)return false;
            // Income can include faction/mod multipliers; retain completed income
            // milestones rather than assuming an unverified marginal tax formula.
            if(goal.NHIPAJDALBA(c)&&(condition.HFBAJIGHOAF==DDBHJKOIIPI.CityMoneyIncome||condition.HFBAJIGHOAF==DDBHJKOIIPI.CityFoodIncome))return false;
        }}
        foreach(var kv in AEHAGJAKNEK.ACIGFKMDOKE)if(!string.IsNullOrEmpty(kv.Value?.EIMGMPDMAAJ)&&kv.Value.EIMGMPDMAAJ.Contains(b.OJPECMNINNA,StringComparison.Ordinal))return false;
        return true;
    }
    static List<JNHOOCJJPJO> DistrictSnapshot(AFAGFHJONFJ c){var a=new List<JNHOOCJJPJO>();foreach(var d in c.FGBLEMGGOPO)a.Add(d);return a;}
    static List<PEGMABENGBB> PlotSnapshot(JNHOOCJJPJO d){var a=new List<PEGMABENGBB>();foreach(var p in d.HGNFEMJOPPB)a.Add(p);return a;}
    static bool MaintainCity(AFAGFHJONFJ c,MKMOJNAJKJE f){bool changed=false;foreach(bool step in MaintenanceSteps(c,f))changed|=step;return changed;}
    static IEnumerable<bool> MaintenanceSteps(AFAGFHJONFJ c,MKMOJNAJKJE f)
    {
        if(!maintenance.Value||!Eligible(c,out var owner)||owner.Pointer!=f.Pointer||c.GKLIEHCHDLD()
            ||!AIGate("maintain:"+c.OJPECMNINNA,f.IBGCLCAIBOL,10)||c.FGBLEMGGOPO==null)yield break;
        foreach(var step in RepairWallRegistrations(c,f))yield return step;
        string governor=c.LMPMNLLGOEC.IPJHOOJBCKI;var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;var definitions=new List<PABIPDOFBFG>();foreach(var kv in AEHAGJAKNEK.ACIGFKMDOKE)definitions.Add(kv.Value);var rows=new List<(JNHOOCJJPJO d,OGABIJECHFG x)>();
        foreach(var d in c.FGBLEMGGOPO){if(d?.AIMLPNBOIIF==null)continue;foreach(var x in d.AIMLPNBOIIF)if(PublicBuilding(c,d,x))rows.Add((d,x));}
        // Most damaged facilities first, so enumeration order cannot starve repair.
        rows.Sort((a,b)=>((double)a.x.OGFAHJHBCNG/Math.Max(1,a.x.GEKKLMPLDHJ.OGFAHJHBCNG)).CompareTo((double)b.x.OGFAHJHBCNG/Math.Max(1,b.x.GEKKLMPLDHJ.OGFAHJHBCNG)));
        if(repairBuildings.Value)foreach(var (d,x) in rows){yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;if(!Present(d,x)||!PublicBuilding(c,d,x))continue;var b=x.GEKKLMPLDHJ;if(!Completed(x)||b.POLCGKHKLHI||b.OGFAHJHBCNG<=0||x.OGFAHJHBCNG>=b.OGFAHJHBCNG)continue;
            int before=x.OGFAHJHBCNG,amount=MaintenancePolicy.RepairAmount(before,b.OGFAHJHBCNG),cost=MaintenancePolicy.RepairCost(b.NPCHPDNFAME,amount,b.OGFAHJHBCNG);
            if(SpendMaintenance(c,f,cost,0,0,()=>x.OHILNFOEADD(amount),()=>x.OGFAHJHBCNG!=before)){
                Note($"maintain repair city={c.OJPECMNINNA} building={b.OJPECMNINNA} hp={before}->{x.OGFAHJHBCNG} cost={cost}");yield return true;break;
            }
        }

        // A missing barracks slot takes precedence over discretionary upgrades
        // and relocation; repairs above preserve facilities already in use.
        if(construction.Value&&BarracksMissing(c,f).Count>0)yield break;
        if(upgradeBuildings.Value){
            var upgradeIndex=new Dictionary<string,List<PABIPDOFBFG>>();
            foreach(var b in definitions)if(b!=null&&!string.IsNullOrEmpty(b.MDOIJIAONJM)){if(!upgradeIndex.TryGetValue(b.MDOIJIAONJM,out var list))upgradeIndex[b.MDOIJIAONJM]=list=new();list.Add(b);}
            var upgraded=new HashSet<IntPtr>();
            for(int pass=0;pass<8;pass++){
            // Each purchase resumes on a later queue slice and reevaluates the
            // remaining candidates against current costs, limits and city state.
            if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor)||!upgradeBuildings.Value)yield break;
            if(construction.Value&&BarracksMissing(c,f).Count>0)yield break;
            int capacityDeficit=CapacityShortfall(c);
            OGABIJECHFG selected=null;PABIPDOFBFG target=null;double score=0;
            var upgradeBlocks=new List<string>();int upgradeCandidates=0;
            foreach(var (d,x) in rows){yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;if(upgraded.Contains(x.Pointer)||!Present(d,x)||!PublicBuilding(c,d,x))continue;if(!Completed(x)||OccupiedBuilding(x))continue;var a=x.GEKKLMPLDHJ;
                if(!upgradeIndex.TryGetValue(a.OJPECMNINNA,out var upgrades))continue;
                foreach(var b in upgrades){yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;if(!Present(d,x)||!PublicBuilding(c,d,x)||!Completed(x)||OccupiedBuilding(x))break;
                    upgradeCandidates++;string blocked=b.IKELHGAHBID>a.IKELHGAHBID&&capacityDeficit==0&&(c.LMPMNLLGOEC.KJPPGIOCDID?.Count??0)>c.JNLIOIHLDHH()?"capacity-expansion-pending":UpgradeBlock(c,f,x,b);if(blocked!=null){if(upgradeBlocks.Count<40)upgradeBlocks.Add(a.OJPECMNINNA+"->"+b.OJPECMNINNA+":"+blocked);continue;}
                    double gain=LocationValue(d,b,x)-LocationValue(d,a,x)+(b.CGBGIDCEBGD-a.CGBGIDCEBGD+b.EGBEEDFCIGH-a.EGBEEDFCIGH)*20+(b.IKELHGAHBID-a.IKELHGAHBID)*100+(b.HKBPGJLMFIG-a.HKBPGJLMFIG+b.DPFIFIOKHEI-a.DPFIFIOKHEI)*100;
                    if(gain<=0)continue;double value=gain/Math.Max(1,b.NPCHPDNFAME);if(capacityDeficit>0&&b.IKELHGAHBID>a.IKELHGAHBID)value+=1000000;if(value>score){score=value;selected=x;target=b;}
                }

            }
            if(Gate("upgrade-audit:"+c.OJPECMNINNA,30))Note($"upgrade-audit city={c.OJPECMNINNA} candidates={upgradeCandidates} selected={target?.OJPECMNINNA} blocked=[{string.Join(";",upgradeBlocks)}]");
            if(selected!=null&&Present(selected.GMIKIMACDBC,selected)&&Completed(selected)&&!OccupiedBuilding(selected)&&CanUpgrade(c,f,selected,target)){var old=selected.GEKKLMPLDHJ;var oldProgress=selected.EMHNNCLBADE;
                if(SpendBuilding(c,f,target,()=>selected.GMMDMPFFIGP(target.OJPECMNINNA,false),()=>selected.GEKKLMPLDHJ?.Pointer!=old.Pointer||selected.EMHNNCLBADE!=oldProgress)){
                    upgraded.Add(selected.Pointer);
                    Note($"maintain upgrade city={c.OJPECMNINNA} building={old.OJPECMNINNA}->{target.OJPECMNINNA} batch={upgraded.Count}/8");yield return true;continue;
                }
            }
            break;
            }
            // Do not mix demolition/relocation into a visit that just changed
            // building definitions. The next visit obtains a fresh snapshot.
            if(upgraded.Count>0)yield break;
        }
        if(NEINKOOBJIH.JKGJNLHLJAJ?.Pointer==c.Pointer||!demolishBuildings.Value)yield break;
        if(remodelJobs.TryGetValue(c.OJPECMNINNA,out var job)){
            if(job.Faction!=f.IBGCLCAIBOL||job.Governor!=c.LMPMNLLGOEC.IPJHOOJBCKI||!Present(job.Source,job.Old)||!Present(job.Target,job.Replacement)
                ||!PublicBuilding(c,job.Source,job.Old)||!PublicBuilding(c,job.Target,job.Replacement)||job.Old.GEKKLMPLDHJ.OJPECMNINNA!=job.Definition
                ||job.Replacement.GEKKLMPLDHJ.OJPECMNINNA!=job.Definition){remodelJobs.Remove(c.OJPECMNINNA);yield break;}
            var b=job.Old.GEKKLMPLDHJ;
            if(!Completed(job.Replacement)||job.Replacement.OGFAHJHBCNG<b.OGFAHJHBCNG||!Retirable(c,job.Source,job.Old))yield break;
            if(!MaintenancePolicy.WorthMoving(LocationValue(job.Source,b,job.Old),LocationValue(job.Target,b,job.Replacement),b.NPCHPDNFAME)){remodelJobs.Remove(c.OJPECMNINNA);yield break;}
            if(!AIGate("remodel:"+c.OJPECMNINNA,f.IBGCLCAIBOL,120))yield break;
            job.Source.HNIEPCENKGC(job.Old);
            if(!Present(job.Source,job.Old)){remodelJobs.Remove(c.OJPECMNINNA);Note($"maintain demolish city={c.OJPECMNINNA} building={b.OJPECMNINNA} replacement={job.Replacement.OJPECMNINNA}");yield return true;yield break;}
            yield break;
        }
        foreach(var (d,x) in rows){yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;if(!Present(d,x)||!PublicBuilding(c,d,x))continue;var b=x.GEKKLMPLDHJ;int kind=EconomicKind(b),zone=DistrictZone(d);
            if(zone==0||kind==0||zone==kind||!Retirable(c,d,x))continue;
            double oldValue=LocationValue(d,b,x);bool replacementExists=false;
            foreach(var (other,y) in rows){if(other.Pointer==d.Pointer||y.GEKKLMPLDHJ.OJPECMNINNA!=b.OJPECMNINNA||DistrictZone(other)!=kind)continue;
                replacementExists=true;
            }
            if(replacementExists||!relocateBuildings.Value||!CanBuild(c,f,b,LocalReserve(c)))continue;
            foreach(var other in DistrictSnapshot(c)){if(other==null||other.Pointer==d.Pointer||other.KFCOLLDEJNF!=HNDFFIEJNHI.Public||other.AJJCCPCJPFC?.Pointer!=c.Pointer||other.HGNFEMJOPPB==null||DistrictZone(other)!=kind)continue;
                foreach(var p in PlotSnapshot(other)){yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;if(!Present(d,x)||!Retirable(c,d,x))break;if(p==null||p.GMIKIMACDBC?.Pointer!=other.Pointer||!p.BFPBNAKIKEO(b)||!MaintenancePolicy.WorthMoving(oldValue,LocationValue(other,b,null,p),b.NPCHPDNFAME))continue;
                    if(!AIGate("remodel:"+c.OJPECMNINNA,f.IBGCLCAIBOL,120))yield break;
                    int count=other.AIMLPNBOIIF.Count;var before=new HashSet<IntPtr>();foreach(var item in other.AIMLPNBOIIF)before.Add(item.Pointer);
                    void CreateReplacement(){try{other.PJNMLNMLAKA(b,p);}finally{
                        foreach(var created in other.AIMLPNBOIIF)if(!before.Contains(created.Pointer)&&created.GEKKLMPLDHJ?.OJPECMNINNA==b.OJPECMNINNA){
                            remodelJobs[c.OJPECMNINNA]=new RemodelJob{Old=x,Replacement=created,Source=d,Target=other,Faction=f.IBGCLCAIBOL,Governor=c.LMPMNLLGOEC.IPJHOOJBCKI,Definition=b.OJPECMNINNA};break;
                        }
                    }}
                    if(SpendBuilding(c,f,b,CreateReplacement,()=>other.AIMLPNBOIIF.Count>count)){
                        Note($"maintain relocate-build city={c.OJPECMNINNA} building={b.OJPECMNINNA} from={d.OJPECMNINNA} to={other.OJPECMNINNA}; original retained until completion");yield return true;yield break;
                    }
                }
            }

        }
        yield break;
    }
}
