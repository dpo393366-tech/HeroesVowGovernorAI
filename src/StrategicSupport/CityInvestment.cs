using System;
using System.Collections.Generic;
using System.Diagnostics;
using BepInEx.Configuration;
using HarmonyLib;

public sealed partial class StrategicSupport
{
    static ConfigEntry<bool> investment,construction,research,zoning,relaxBuildingAdjacency;
    static ConfigEntry<int> buildsPerCity,cityCashReserve,investmentTreasuryReserve;
    static bool investing;
    [ThreadStatic] static int nativeBuildScope;
    [ThreadStatic] static MKMOJNAJKJE researchOwner;
    static readonly Dictionary<IntPtr,int> nativeZones=new();
    sealed class NativeCityPlanState {public CivicFlow Flow;public PopulationModel Population;public Dictionary<string,int> Barracks;public GoalPlan Goals;}
    static readonly Dictionary<IntPtr,NativeCityPlanState> nativeCityPlanStates=new();
    static NativeCityPlanState NativePlanState(AFAGFHJONFJ c,MKMOJNAJKJE f){
        if(nativeBuildScope<=0)return new(){Flow=ReadCivicFlow(c),Population=PopulationSnapshot(c),Barracks=BarracksMissing(c,f),Goals=ReadGoals(c)};
        if(!nativeCityPlanStates.TryGetValue(c.Pointer,out var s))nativeCityPlanStates[c.Pointer]=s=new(){Flow=ReadCivicFlow(c),Population=PopulationSnapshot(c),Barracks=BarracksMissing(c,f),Goals=ReadGoals(c)};
        return s;
    }

    void ConfigureInvestment()
    {
        ConfigureMaintenance(); ConfigureResearchInvestment();
        investment=Config.Bind("Investment","Enabled",true,"Actively invest surplus cash in AI-managed allied and enemy cities. Personally managed cities are excluded.");
        construction=Config.Bind("Investment","Construction",true,"Meet city goals with sustainable civic flows, maximize barracks, then balanced expansion and adjacency output. Uses native placement and costs.");
        research=Config.Bind("Investment","Research",true,"Reconsider native research when an AI city updates; retain prerequisites, research points and idle researcher requirements.");
        zoning=Config.Bind("Investment","SpecializeDistricts",true,"Prefer district synergies while allowing mixed commerce/agriculture for goals, civic balance and higher city output.");
        relaxBuildingAdjacency=Config.Bind("Investment","RelaxBuildingAdjacency",true,"Allow building sites without required road adjacency. Preserve footprint, terrain/walls, research, resource costs, private/public ownership and building caps.");
        buildsPerCity=Config.Bind("Investment","BuildingsPerCityUpdate",2,new ConfigDescription("Maximum extra buildings per native city update; work is bounded rather than scanned every frame.",new AcceptableValueRange<int>(1,8)));
        cityCashReserve=Config.Bind("Investment","CityCashReserve",10000,new ConfigDescription("Minimum local cash retained in addition to current recruitment needs.",new AcceptableValueRange<int>(0,10000000)));
        investmentTreasuryReserve=Config.Bind("Investment","TreasuryReserve",10000,new ConfigDescription("Treasury emergency cash retained for investment/research. Recruitment has its own reserve; all surplus above this is available for productive investment.",new AcceptableValueRange<int>(0,10000000)));
    }
    void HookInvestment(Harmony h)
    {
        HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
        h.Patch(AccessTools.Method(typeof(AFAGFHJONFJ),"JFOOAMEIMDO"),postfix:H(nameof(AfterRecruitInvestment)));
        h.Patch(AccessTools.Method(typeof(MKMOJNAJKJE),"GHCOLMGCKKD"),prefix:H(nameof(BeginBuildingPlan)),finalizer:H(nameof(EndBuildingPlan)));
        h.Patch(AccessTools.Method(typeof(PEGMABENGBB),"BFPBNAKIKEO"),prefix:H(nameof(BeginPlacementAdjacency)),postfix:H(nameof(FilterNativeZone)),finalizer:H(nameof(EndPlacementAdjacency)));
        h.Patch(AccessTools.Method(typeof(JNHOOCJJPJO),"PJNMLNMLAKA"),postfix:H(nameof(RefreshDistrictZone)));
        h.Patch(AccessTools.Method(typeof(MKMOJNAJKJE),"CIPAEBBIJLA"),prefix:H(nameof(FilterResearchBudget)));
        h.Patch(AccessTools.Method(typeof(FNFJODHIJFO),"OHBNFHKFNPH"),prefix:H(nameof(BeginResearch)),finalizer:H(nameof(EndResearch)));
        h.Patch(AccessTools.Method(typeof(FNFJODHIJFO),"OBIOEOJAHMM"),postfix:H(nameof(AfterGovernorEconomy)));
        h.Patch(AccessTools.Method(typeof(AFAGFHJONFJ),"JFOOAMEIMDO"),prefix:H(nameof(BeforeCapacityTick)),finalizer:H(nameof(AfterCapacityTick)));
        h.Patch(AccessTools.Method(typeof(OGABIJECHFG),"GMMDMPFFIGP"),prefix:H(nameof(BeforeBuildingChange)),finalizer:H(nameof(AfterBuildingChange)));
        h.Patch(AccessTools.Method(typeof(JNHOOCJJPJO),"HNIEPCENKGC"),prefix:H(nameof(BeforeBuildingRemoval)),finalizer:H(nameof(AfterBuildingRemoval)));
    }
    static bool RequiresStreet(PABIPDOFBFG b)=>b!=null&&b.FHKHHKBBKGM&&!(enabled?.Value==true&&relaxBuildingAdjacency?.Value==true);
    // Only native needStreet is relaxed during the original parcel check.
    // Its footprint and terrain/wall checks still run. Restore on exception.
    static void BeginPlacementAdjacency(PABIPDOFBFG __0,out bool __state){__state=__0?.FHKHHKBBKGM??false;if(__0!=null&&!RequiresStreet(__0))__0.FHKHHKBBKGM=false;}
    static void EndPlacementAdjacency(PABIPDOFBFG __0,bool __state){if(__0!=null)__0.FHKHHKBBKGM=__state;}
    static void BeginBuildingPlan(out int __state)
    {
        __state=nativeBuildScope;
        if(enabled.Value&&investment.Value&&construction.Value){nativeBuildScope++;nativeZones.Clear();nativeGoalSites.Clear();nativeCityPlanStates.Clear();}
    }
    static void EndBuildingPlan(int __state){nativeBuildScope=__state;if(__state==0)nativeZones.Clear();nativeGoalSites.Clear();nativeCityPlanStates.Clear();}
    static void RefreshDistrictZone(JNHOOCJJPJO __instance){InvalidatePlanningReads();if(__instance!=null){nativeZones.Remove(__instance.Pointer);if(__instance.AJJCCPCJPFC!=null){nativeGoalSites.Remove(__instance.AJJCCPCJPFC.Pointer);nativeCityPlanStates.Remove(__instance.AJJCCPCJPFC.Pointer);}}}
    static int EconomicKind(PABIPDOFBFG b){
        for(int depth=0;b!=null&&depth<32;depth++){
            if(b.HFBAJIGHOAF==MHCEFEODENB.Commerce)return 1;if(b.HFBAJIGHOAF==MHCEFEODENB.Agriculture)return 2;
            if(string.IsNullOrEmpty(b.MDOIJIAONJM)||AEHAGJAKNEK.ACIGFKMDOKE==null||!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(b.MDOIJIAONJM,out b))break;
        }return 0;
    }
    static int DistrictZone(JNHOOCJJPJO district)
    {
        int commerce=0,agriculture=0;
        if(district.AIMLPNBOIIF!=null)foreach(var instance in district.AIMLPNBOIIF){
            var b=instance?.GEKKLMPLDHJ;int kind=EconomicKind(b);
            if(kind==1)commerce+=Math.Max(1,b.MLAJLJKFGBD*b.PNPLBNAFMCM);
            if(kind==2)agriculture+=Math.Max(1,b.MLAJLJKFGBD*b.PNPLBNAFMCM);
        }
        if(commerce>agriculture)return 1;if(agriculture>commerce)return 2;
        return district.HFBAJIGHOAF==LDPAFPHFBCP.Commerce?1:district.HFBAJIGHOAF==LDPAFPHFBCP.Agriculture?2:0;
    }
    static void FilterNativeZone(PEGMABENGBB __instance,PABIPDOFBFG __0,ref bool __result)
    {
        if(!__result||nativeBuildScope<=0)return;
        try{
            var d=__instance?.GMIKIMACDBC;
            if(d==null||d.KFCOLLDEJNF!=HNDFFIEJNHI.Public||!Eligible(d.AJJCCPCJPFC,out var owner))return;
            if(__0.OCJNCECECMH==FFDDNPOHLHN.Private){__result=false;return;}
            var cached=NativePlanState(d.AJJCCPCJPFC,owner);var missing=cached.Barracks;
            double populationGain=cached.Population.Gain(__0.CGBGIDCEBGD+__0.EGBEEDFCIGH,__0.DHLNIKKLNKJ);
            bool civicRepair=cached.Flow.Popular<=0&&__0.DPFIFIOKHEI>0||cached.Flow.Security<=0&&__0.HKBPGJLMFIG>0||cached.Flow.Population<0&&populationGain>0;
            if(missing.Count>0&&!missing.ContainsKey(__0.OJPECMNINNA)&&!cached.Goals.Buildings.ContainsKey(__0.OJPECMNINNA)&&!civicRepair){__result=false;return;}
            if(!nativeZones.TryGetValue(d.Pointer,out int zone)){zone=DistrictZone(d);nativeZones[d.Pointer]=zone;}
            if(CivicBlock(cached.Flow,__0)!=null||cached.Flow.Population+populationGain<Math.Min(0,cached.Flow.Population)){__result=false;return;}
            ProtectNativeGoalSite(__instance,__0,ref __result);
        }catch(Exception e){Error(e);}
    }
    static bool FilterResearchBudget(MKMOJNAJKJE __instance,ref Il2CppSystem.Collections.Generic.List<FBLFDEEGJMH> __0,ref FBLFDEEGJMH __result)
    {
        try{return SelectResearch(__instance,ref __0,ref __result);}catch(Exception e){__result=null;Error(e);return false;}
    }
    static void AfterRecruitInvestment(AFAGFHJONFJ __instance,bool __0)
    {
        if(__0||investing)return;
        if(schedulerOnline){ScheduleCity(__instance);return;}
        try{
            if(!Ready()||!investment.Value)return;
            string eligibility=Eligibility(__instance,out var f);
            if(eligibility!=null){InvestmentAudit(__instance,"excluded:"+eligibility);return;}
            if(!__instance.NBKEECPNIJH(true)){InvestmentAudit(__instance,"native-city-management-disabled");return;}
            if(__instance.GKLIEHCHDLD()){InvestmentAudit(__instance,"siege");return;}
            if(!AIGate("invest:"+__instance.OJPECMNINNA,f.IBGCLCAIBOL,2))return;
            investing=true;var timer=Stopwatch.StartNew();int built=0;
            AuditCityIncome(__instance,f);
            var row=__instance.LMPMNLLGOEC;
            // Recruitment consumes actual money through the native staffing routine.
            // Do not refill troop HP or repeat the day's recruitment/supplement tick.
            if(budget.Value&&row.KJPPGIOCDID!=null&&row.KJPPGIOCDID.Count<__instance.JNLIOIHLDHH()&&row.PNHICLAGHGD>0){
                Fund(__instance,true);
                if(row.DNOGPFHMOMO>=50001&&AIGate("staff-invest:"+__instance.OJPECMNINNA,f.IBGCLCAIBOL,10))__instance.OBIOEOJAHMM();
            }
            TryResearch(__instance,f);
            if(maintenance.Value)try{MaintainCity(__instance,f);}catch(Exception e){Error(e);}
            if(construction.Value)for(int i=0;i<buildsPerCity.Value;i++){
                if(i>0&&timer.ElapsedMilliseconds>=12)break;
                if(!BuildBest(__instance,f))break;built++;
            }
            if(built>0||timer.ElapsedMilliseconds>20)Note($"investment city={__instance.OJPECMNINNA} built={built} elapsedMs={timer.ElapsedMilliseconds}");
            InvestmentAudit(__instance,$"evaluated built={built} treasury={f.DNOGPFHMOMO} cityCash={row.DNOGPFHMOMO} treasuryReserve={investmentTreasuryReserve.Value} localReserve={LocalReserve(__instance)} tech={f.IIBKLLKPNEH} strategy={f.PEGLDBENJMF}");
            if(timer.ElapsedMilliseconds<20)BalanceGarrison(__instance);
        }catch(Exception e){Error(e);}finally{investing=false;researchOwner=null; if(!__0)AfterCityAdministration(__instance);}
    }
    sealed class DistrictPlan
    {
        public JNHOOCJJPJO District;public int Zone;public double Money,Food,MoneyBonus,FoodBonus;
        public int Commerce,Agriculture;
        public List<PEGMABENGBB> Places=new();public List<BuildingEffectsPolicy.Building> Effects;public BuildingEffectsPolicy.Output Output;
    }
    static int LocalReserve(AFAGFHJONFJ c)
    {
        long reserve=Math.Max(cityCashReserve.Value,Math.Max(0L,c.LAGKJHBEPGF(false))*3);
        // Staffing's 50,001 entry threshold is not a construction reserve.
        // Keep real running costs, without locking every empty city behind that threshold.
        return (int)Math.Min(int.MaxValue,reserve);
    }
    static bool CanBuild(AFAGFHJONFJ c,MKMOJNAJKJE f,PABIPDOFBFG b,int localReserve)
        =>BuildBlock(c,f,b,localReserve)==null;
    static bool CurrentPlot(AFAGFHJONFJ c,JNHOOCJJPJO d,PEGMABENGBB p,long[] expected){
        if(d?.AJJCCPCJPFC?.Pointer!=c.Pointer||p?.GMIKIMACDBC?.Pointer!=d.Pointer||p.FAFNFFJAFKH==null||expected==null||expected.Length==0||expected.Length!=(long)p.PABEMCAGKJJ*p.HAPLIFADGBA||p.FAFNFFJAFKH.Length<expected.Length)return false;
        var remaining=new HashSet<long>(expected);foreach(var q in PlotCells(p))if(q==null||q.FCOJFBEHMAG!=-9999||!remaining.Remove(q.Pointer.ToInt64()))return false;
        return remaining.Count==0;
    }
    static string BuildBlock(AFAGFHJONFJ c,MKMOJNAJKJE f,PABIPDOFBFG b,int localReserve,bool planning=false)
    {
        if(b==null||b.POLCGKHKLHI||b.NPCHPDNFAME<0||b.MLAJLJKFGBD<=0||b.PNPLBNAFMCM<=0)return "definition-or-ai-ban";
        if(b.OCJNCECECMH==FFDDNPOHLHN.Private)return "private-estate-only";
        // Upgrades and consumable-item constructions keep their native execution path.
        if(!string.IsNullOrEmpty(b.MDOIJIAONJM)||!planning&&!HasMaterials(f,b))return "upgrade-or-item-cost";
        if(b.HFBAJIGHOAF==MHCEFEODENB.None||b.HFBAJIGHOAF==MHCEFEODENB.Wall&&!DefensiveBuilding(b)||b.HFBAJIGHOAF==MHCEFEODENB.Resources)return "other-category";
        if(!planning&&(f.IIBKLLKPNEH<Math.Max(0,b.ODKKMLDAPJL)||f.PEGLDBENJMF<Math.Max(0,b.ADGOBJJMEAO)))return "research-or-strategy-points";
        if(!planning&&!InvestmentPolicy.CanPurchase(f.DNOGPFHMOMO,c.LMPMNLLGOEC.DNOGPFHMOMO,b.NPCHPDNFAME,InvestmentReserve(f),localReserve))return "cash";
        if(!b.PLEHAHBBLGL(f.KCKGONGIMDA))return "technology";
        if(!b.MCGEOICKKEE(c,false))return "prerequisites";
        if(b.LOJCCNNJKHF(c,false))return "building-cap";
        return null;
    }
    static bool BuildBest(AFAGFHJONFJ c,MKMOJNAJKJE f){
        bool result=false;foreach(bool committed in BuildSteps(c,f))result|=committed;return result;
    }
    static IEnumerable<bool> BuildSteps(AFAGFHJONFJ c,MKMOJNAJKJE f)
    {
        string owner=c.LMPMNLLGOEC.LMMNKIOAGLC,governor=c.LMPMNLLGOEC.IPJHOOJBCKI;
        var worldAtStart=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
        if(c.FGBLEMGGOPO==null||AEHAGJAKNEK.ACIGFKMDOKE==null)yield break;
        var initialRevision=ReadLayoutRevision(c,f);
        int reserve=LocalReserve(c);bool hungry=FoodEmergency(c);var goals=ReadGoals(c);var actualGoalBuildings=new HashSet<string>(goals.Buildings.Keys);var civic=ReadCivicFlow(c);AddBarracksGoals(c,f,goals);var barracksMissing=BarracksMissing(c,f);var needs=goals.Buildings;var districts=new List<DistrictPlan>();
        foreach(var d in c.FGBLEMGGOPO){
            if(d==null||d.KFCOLLDEJNF!=HNDFFIEJNHI.Public||d.AJJCCPCJPFC?.Pointer!=c.Pointer||d.HGNFEMJOPPB==null)continue;
            if(d.PLIFMOIBPME&&Gate("wall-site-audit:"+d.OJPECMNINNA,120)){
                int free=0,unknown=0;if(d.FAFNFFJAFKH!=null)foreach(var wallRow in d.FAFNFFJAFKH)if(wallRow!=null)foreach(var q in wallRow)if(q!=null&&(int)q.HFBAJIGHOAF==1&&q.FCOJFBEHMAG==-9999){free++;if(WallSocketDirection(q.PMGDGAAICCG)<0)unknown++;}
                Note($"wall-site-audit city={c.OJPECMNINNA} district={d.OJPECMNINNA} emptyWallSockets={free} unknownDirections={unknown} nativeParcels={d.HGNFEMJOPPB.Count} existingBuildings={d.AIMLPNBOIIF?.Count??0} artificialQuota=false");
            }
            var effects=DistrictEffects(d);var plan=new DistrictPlan{Effects=effects,Output=BuildingEffectsPolicy.Evaluate(effects),District=d,Zone=zoning.Value?DistrictZone(d):0,Commerce=d.IMMLKOLPPHF(),Agriculture=d.KMDCHCBIAIG()};
            foreach(var p in d.HGNFEMJOPPB)if(p!=null&&p.GMIKIMACDBC?.Pointer==d.Pointer)plan.Places.Add(p);
            if(d.AIMLPNBOIIF!=null)foreach(var instance in d.AIMLPNBOIIF){var b=instance?.GEKKLMPLDHJ;if(b==null)continue;
                plan.Money+=Math.Max(0,b.CNCPIILGDEF);plan.Food+=Math.Max(0,b.AJJJJNDNNME);plan.MoneyBonus+=b.FIPONBEGNJO;plan.FoodBonus+=b.NIKFGJLNNCA;
            }
            if(plan.Places.Count>0||d.FAFNFFJAFKH?.Count>0)districts.Add(plan);
        }
        PABIPDOFBFG best=null;PEGMABENGBB spot=null;JNHOOCJJPJO block=null;int priorityBest=0;
        var choices=new List<LayoutChoice>();
        var vacantGeometry=new Dictionary<string,List<PEGMABENGBB>>();
        var blocked=new Dictionary<string,int>();int candidates=0,parcelProbes=0,sizeRejected=0,nativeRejected=0;
        var definitions=new List<PABIPDOFBFG>();foreach(var kv in AEHAGJAKNEK.ACIGFKMDOKE)definitions.Add(kv.Value);
        var slice=Stopwatch.StartNew();int probes=0;
        foreach(var b in definitions){
            if(++probes>=32||slice.Elapsed.TotalMilliseconds>=1){yield return false;probes=0;slice.Restart();}
            if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=worldAtStart||c.LMPMNLLGOEC.LMMNKIOAGLC!=owner||c.LMPMNLLGOEC.IPJHOOJBCKI!=governor)yield break;
            var reason=BuildBlock(c,f,b,reserve,true);if(reason!=null){blocked.TryGetValue(reason,out int n);blocked[reason]=n+1;continue;}candidates++;
            int defense=DefenseShortfall(c,f,b);
            int kind=EconomicKind(b);bool barracks=b.IKELHGAHBID>0;
            // Do not spend the barracks budget on cheaper alternatives merely
            // because the required barracks is not currently affordable.
            // Include the complete future layout; commit still enforces barracks first.
            bool required=needs.ContainsKey(b.OJPECMNINNA);
            foreach(var d in districts){
                // Mixed districts are evaluated by output, not forbidden.
                double goal=GoalBenefit(goals,d,b);
                bool feeds=b.AJJJJNDNNME>0||b.NIKFGJLNNCA>0;
                int priority=actualGoalBuildings.Contains(b.OJPECMNINNA)?10:((civic.Security<=0&&b.HKBPGJLMFIG>0)||(civic.Popular<=0&&b.DPFIFIOKHEI>0))?9:barracksMissing.ContainsKey(b.OJPECMNINNA)?8:hungry&&feeds?7:required?3:defense>0?3:goal>0?2:1;
                // Do not drop technology, strategy, recruiting, special or road
                // facilities merely because they have no direct cash income.
                double benefit=defense*500+goal*20+Math.Max(0,b.IKELHGAHBID)*10;
                var places=new List<PEGMABENGBB>(d.Places);
                // Native greedy parcels can hide legal 1x1/1x2 holes. Generate
                // every rectangle on a detached grid; live tiles stay intact.
                string geometryKey=d.District.Pointer+":"+Math.Min(b.MLAJLJKFGBD,b.PNPLBNAFMCM)+"x"+Math.Max(b.MLAJLJKFGBD,b.PNPLBNAFMCM)+":"+RequiresStreet(b)+":"+(b.HFBAJIGHOAF==MHCEFEODENB.Wall);
                if(!vacantGeometry.TryGetValue(geometryKey,out var geometry)){
                    geometry=new();vacantGeometry[geometryKey]=geometry;
                    foreach(var vacant in RebuildPlans(c,d.District,b,true)){
                        yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=worldAtStart||!CurrentCity(c,f,governor))yield break;
                        if(vacant!=null)geometry.Add(vacant.Place);
                    }
                }
                places.AddRange(geometry);
                foreach(var p in places){
                    if(++probes>=32||slice.Elapsed.TotalMilliseconds>=1){yield return false;probes=0;slice.Restart();}
                    if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=worldAtStart||c.LMPMNLLGOEC.LMMNKIOAGLC!=owner||c.LMPMNLLGOEC.IPJHOOJBCKI!=governor)yield break;
                    parcelProbes++;
                    if((long)p.PABEMCAGKJJ*p.HAPLIFADGBA!=(long)b.MLAJLJKFGBD*b.PNPLBNAFMCM){sizeRejected++;continue;}
                    if(!p.BFPBNAKIKEO(b)){nativeRejected++;continue;}
                    if(!CurrentPlot(c,d.District,p,Footprint(p))){blocked.TryGetValue("occupied-or-stale-parcel",out int staleCount);blocked["occupied-or-stale-parcel"]=staleCount+1;continue;}
                    var projected=new List<BuildingEffectsPolicy.Building>(d.Effects){BuildingProfile(b,d.District.OJPECMNINNA,PlotCells(p))};double structural=BuildingEffectsPolicy.Evaluate(projected).Value-d.Output.Value;
                    double score=InvestmentPolicy.Score(priority,benefit+structural,b.NPCHPDNFAME,b.MLAJLJKFGBD*b.PNPLBNAFMCM);
                    if(double.IsFinite(score)&&double.IsFinite(structural))choices.Add(MakeLayoutChoice(c,d,b,p,priority,benefit-goal*20,score));
                }
            }
        }
        LayoutChoice choice=null;LayoutIntent intent=null;
        if(initialRevision.StructureChange(ReadLayoutRevision(c,f)) is string changed){Note($"layout-scan-discard city={c.OJPECMNINNA} reason={changed}");yield break;}
        foreach(bool step in ChooseLayoutSteps(c,f,goals,choices,(x,y)=>{choice=x;intent=y;})){yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=worldAtStart||!CurrentCity(c,f,governor))yield break;}
        if(choice!=null){best=choice.Building;spot=choice.Place;block=choice.District;priorityBest=choice.Action.Priority;}
        if(best==null||spot==null){var reasons=new List<string>();foreach(var pair in blocked)reasons.Add(pair.Key+"="+pair.Value);InvestmentAudit(c,$"no-build-candidate allowed={candidates} publicDistricts={districts.Count} legalChoices={choices.Count} parcelProbes={parcelProbes} sizeRejected={sizeRejected} nativeRejected={nativeRejected} nativeBuildScope={nativeBuildScope} blocked=[{string.Join(",",reasons)}]");yield break;}
        if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=worldAtStart||c.LMPMNLLGOEC.LMMNKIOAGLC!=owner||c.LMPMNLLGOEC.IPJHOOJBCKI!=governor)yield break;
        reserve=LocalReserve(c);
        string commitBlock=BuildBlock(c,f,best,reserve);
        if(commitBlock==null&&initialRevision.StructureChange(ReadLayoutRevision(c,f)) is string revisionChange)commitBlock="plan-inputs-changed:"+revisionChange;
        var stillMissing=BarracksMissing(c,f);
        if(commitBlock==null)commitBlock=CivicBuildBlock(c,best);
        if(commitBlock==null&&!CurrentPlot(c,block,spot,choice.Action.Tiles))commitBlock="parcel-changed-or-occupied";
        if(commitBlock==null&&!spot.BFPBNAKIKEO(best))commitBlock="native-parcel-rejected";
        if(commitBlock!=null){layoutIntents.Remove(c.OJPECMNINNA);InvestmentAudit(c,$"build-commit-blocked building={best.OJPECMNINNA} district={block.OJPECMNINNA} reason={commitBlock} cost={best.NPCHPDNFAME} treasury={f.DNOGPFHMOMO} cityCash={c.LMPMNLLGOEC.DNOGPFHMOMO} reserve={reserve}");yield break;}
        int transfer=InvestmentPolicy.TransferForPurchase(f.DNOGPFHMOMO,c.LMPMNLLGOEC.DNOGPFHMOMO,best.NPCHPDNFAME,InvestmentReserve(f),reserve);
        var row=c.LMPMNLLGOEC;int treasury=f.DNOGPFHMOMO,cash=row.DNOGPFHMOMO,tech=f.IIBKLLKPNEH,strategy=f.PEGLDBENJMF;
        int count=block.AIMLPNBOIIF?.Count??0;string id=best.OJPECMNINNA,blockId=block.OJPECMNINNA;
        using var materials=PayMaterials(f,best);if(materials==null)yield break;
        bool committed=false;
        try{
            f.DNOGPFHMOMO=treasury-transfer;row.DNOGPFHMOMO=cash+transfer-best.NPCHPDNFAME;
            f.IIBKLLKPNEH=tech-Math.Max(0,best.ODKKMLDAPJL);f.PEGLDBENJMF=strategy-Math.Max(0,best.ADGOBJJMEAO);
            block.PJNMLNMLAKA(best,spot);
            committed=(block.AIMLPNBOIIF?.Count??0)>count;
            if(!committed)throw new InvalidOperationException("Native construction did not register a building: "+id);
        }finally{
            // Once native registration succeeded, its cost must stay paid even if scene refresh failed.
            if(committed||(block.AIMLPNBOIIF?.Count??0)>count){materials.Committed=true;var cycle=Budget(f);cycle.ConstructionSpent=(int)Math.Min(int.MaxValue,(long)cycle.ConstructionSpent+transfer);}
            if(!committed&&(block.AIMLPNBOIIF?.Count??0)<=count){f.DNOGPFHMOMO=treasury;row.DNOGPFHMOMO=cash;f.IIBKLLKPNEH=tech;f.PEGLDBENJMF=strategy;}
        }
        CommitLayout(c,intent);
        Note($"build-invest city={c.OJPECMNINNA} district={blockId} building={id} priority={priorityBest} cost={best.NPCHPDNFAME} transfer={transfer} cityCash={row.DNOGPFHMOMO}");
        yield return true;
    }
}




