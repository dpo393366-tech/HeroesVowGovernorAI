using System;
using System.Collections.Generic;

public sealed partial class StrategicSupport
{
    static bool ViableSource(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved)=>SupportSourceBlock(c,teams,reserved)==null;
    static string SupportSourceBlock(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved)=>SupportSourceRules(c,teams,reserved,false);
    static string SupportSourceRules(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved,bool urgent){
        if(!DistributionCity(c,out var f))return "no-faction-record";if(c.GKLIEHCHDLD())return "source-under-attack";
        foreach(var t in teams)if((t.m_targetCity==c.OJPECMNINNA&&War(f,t.m_belongPower))||t.m_warInfo?.IJGLCFJOOIK?.OJPECMNINNA==c.OJPECMNINNA)return "source-under-attack";
        var troops=c.LMPMNLLGOEC.KJPPGIOCDID;if(troops==null)return "no-troop-roster";
        long total=0;int home=0,ready=0;
        foreach(var a in troops)if(a!=null&&!reserved.Contains(a.Pointer)){total+=Math.Max(0,a.FGHFOLKBHDL);if(a.FGHFOLKBHDL>0)home++;if(Available(a))ready++;}
        if(ready==0)return "no-ready-units";if(home<=(urgent?1:minUnits.Value))return "minimum-garrison";
        return c.LMPMNLLGOEC.OEGNGFBCHFL>=total*5?null:"source-food-reserve";
    }
    [ThreadStatic] static HashSet<int> battleOriginalReserveExclusions;
    [ThreadStatic] static HashSet<string> generatingHomeOfficers;
    [ThreadStatic] static IntPtr generatingHomeCity;
    [ThreadStatic] static int generatingOfficerPoolLimit;
    [ThreadStatic] static int capacityTransferLimit;
    [ThreadStatic] static int supportDestinationSlots;
    [ThreadStatic] static int capacityTransferFoodDays;
    [ThreadStatic] static long backfillSoldierGoal,backfillPowerGoal;
    static bool CountsAsSupportCommitment(Wo_DeployTeamCtrl t)=>offenseGenerating||battleOriginalReserveExclusions==null||!battleOriginalReserveExclusions.Contains(t.m_id);
    // A native generator consumes a temporary candidate view synchronously. The
    // authoritative city roster is restored BEFORE activation, dismissal or travel.
    // No troop status, ownership, money or soldier count is edited to make it legal.
    static Wo_DeployTeamCtrl GenerateSupport(AFAGFHJONFJ city,int requested,
        List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved)
    {
        var previous=transactionWaves;transactionWaves=new();var started=System.Diagnostics.Stopwatch.GetTimestamp();
        try{return GenerateSupportCore(city,requested,teams,reserved);}
        finally{transactionWaves=previous;RecordPhaseCost("native/support-selection",(System.Diagnostics.Stopwatch.GetTimestamp()-started)*1000d/System.Diagnostics.Stopwatch.Frequency);}
    }
    static Wo_DeployTeamCtrl GenerateSupportCore(AFAGFHJONFJ city,int requested,
        List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved)
    {
        Wo_DeployTeamCtrl Fail(string reason){if(Gate("assembly-block:"+city.OJPECMNINNA+":"+reason.Split(' ')[0],30))Note($"assembly-block city={city.OJPECMNINNA} faction={city.LMPMNLLGOEC?.LMMNKIOAGLC} purpose={(capacityTransferLimit>0?"capacity-transfer":offenseGenerating?"attack":rebalancing?"officer-transfer":"support")} reason={reason}");return null;}
        MKMOJNAJKJE faction;if(!(offenseGenerating?Eligible(city,out faction):DistributionCity(city,out faction)))return Fail("source-not-authorized");
        if((offenseGenerating||rebalancing||capacityTransferLimit>0)&&CoverPlanning(faction.IBGCLCAIBOL))return Fail("regional-cover-planning");
        NormalizeTroopReferences(city);
        var row=city.LMPMNLLGOEC;var original=row.KJPPGIOCDID;
        string sourceFaction=row.LMMNKIOAGLC,sourceGovernor=row.IPJHOOJBCKI;
        if(original==null)return Fail("no-roster");
        bool urgent=EmergencyFor(city)&&!offenseGenerating&&!rebalancing&&capacityTransferLimit==0;
        int minimumUnits=urgent?1:minUnits.Value;
        var officers=HomeOfficers(city,teams);int keepOfficers=urgent?Math.Max(1,(officers.Count+3)/4):Math.Max(2,(officers.Count+2)/3);
        if(officers.Count<=keepOfficers)return Fail($"home-command home={officers.Count} keep={keepOfficers}");
        long border=offenseGenerating?AttackBorderThreat(city,faction,teams):BorderThreat(city,faction,teams);if(border==long.MaxValue)return Fail("incoming-attack");
        var combat=new Dictionary<IntPtr,int>();long homeCombat=0,awayCombat=0;
        var candidates=new List<DHKINGPDKJJ>();var home=new HashSet<IntPtr>();long total=0,away=0;
        foreach(var a in original){
            if(a==null)continue;if(!home.Add(a.Pointer))return Fail("duplicate-native-unit");
            // A troop already in a live team cannot also count as home protection.
            if(reserved.Contains(a.Pointer))continue;
            total+=Math.Max(0,a.FGHFOLKBHDL);
            int value=Math.Max(0,a.GNCKPEPJDGF());combat[a.Pointer]=value;homeCombat+=value;
            if(Available(a))candidates.Add(a);
        }
        foreach(var t in teams)if(t.m_belongPower==row.LMMNKIOAGLC&&t.m_assembleCity==city.OJPECMNINNA&&CountsAsSupportCommitment(t))
        {   away+=Math.Max(0,t.GetSoldierNum(false));
            if(t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var a in group.Value)if(a!=null)awayCombat+=Math.Max(0,a.GNCKPEPJDGF());
        }
        int homeUnits=0;foreach(var a in original)if(a!=null&&!reserved.Contains(a.Pointer)&&a.FGHFOLKBHDL>0)homeUnits++;
        long keep=capacityTransferLimit>0?0:(long)Math.Ceiling((total+away)*(offenseGenerating?Retention(city,faction):Math.Round(retain.Value,3)));
        long budget=Math.Max(0,total-keep);
        if(urgent){
            var baseline=EmergencyBaseline(city,emergencyContext.War,teams,total,homeCombat);emergencyContext.Budget=baseline;
            double fraction=EmergencyFraction(city,faction,teams,emergencyContext.Eta);
            keep=(long)Math.Ceiling(baseline.Soldiers*fraction);budget=Math.Max(0,total-keep);
            long oldBorder=border;border=(long)Math.Ceiling(baseline.Power*fraction);
            if(Gate("emergency-budget:"+city.OJPECMNINNA,5))Note($"emergency-support-budget city={city.OJPECMNINNA} battle={emergencyContext.War?.HIENOINGBCN??"incoming-march"} homeSoldiers={total} baselineSoldiers={baseline.Soldiers} awaySoldiers={away} keepSoldiers={keep} fraction={fraction:F2} homeRaw={homeCombat} baselineRaw={baseline.Power} oldBorder={oldBorder} keepRaw={border} minimumUnits={minimumUnits} keepOfficers={keepOfficers} mode=support-priority");
        }
        long originalBorder=border;long coverPower=0,coverSoldiers=0,coverBaseline=0,coverBaselineSoldiers=0;
        if(!offenseGenerating&&!rebalancing&&capacityTransferLimit==0)RegionalCoverCredit(city,faction,teams,out coverPower,out coverSoldiers,out coverBaseline,out coverBaselineSoldiers);
        if(coverPower>0){
            border=Math.Max(0,Math.Min(border,coverBaseline)-coverPower);
            if(urgent){border=Math.Max(border,(long)Math.Ceiling(emergencyContext.Budget.Power*.2));keep=Math.Max((long)Math.Ceiling(emergencyContext.Budget.Soldiers*.2),Math.Max(0,keep-coverSoldiers));}
            else keep=Math.Max(keep,Math.Max(0,coverBaselineSoldiers-coverSoldiers));budget=Math.Max(0,total-keep);
        }
        long keepCombat=urgent||capacityTransferLimit>0?border:Math.Max(border,(long)Math.Ceiling((homeCombat+awayCombat)*(offenseGenerating?Retention(city,faction):Math.Round(retain.Value,3))));
        long combatBudget=Math.Max(0,homeCombat-keepCombat);
        int slots=Math.Max(0,homeUnits-minimumUnits);
        if(rebalancing)slots=Math.Min(1,slots);
        if(capacityTransferLimit>0)slots=Math.Min(capacityTransferLimit,slots);
        if(supportDestinationSlots>0)slots=Math.Min(supportDestinationSlots,slots);
        if(slots==0||budget<100||capacityTransferLimit==0&&row.OEGNGFBCHFL<total*5)return Fail($"home-reserve slots={slots} soldierBudget={budget} cityFood={row.OEGNGFBCHFL} foodFloor={total*5}");
        candidates.Sort((a,b)=>b.FGHFOLKBHDL.CompareTo(a.FGHFOLKBHDL));
        var selected=new Il2CppSystem.Collections.Generic.List<DHKINGPDKJJ>();
        long pickedSoldiers=0,pickedPower=0;
        foreach(var a in candidates){if(selected.Count>=slots||(backfillSoldierGoal>0&&backfillPowerGoal>0&&pickedSoldiers>=backfillSoldierGoal&&pickedPower>=backfillPowerGoal))break;if(a.FGHFOLKBHDL>budget||combat[a.Pointer]<=0||combat[a.Pointer]>combatBudget)continue;
            if(coverPower>0){
                long remaining=homeCombat-pickedPower-combat[a.Pointer];
                RegionalCoverCreditForHome(city,faction,teams,remaining,out long timely,out long timelyMen,out long baselineRaw,out long baselineMen);
                if(timely<=0||remaining+timely<baselineRaw||total-pickedSoldiers-a.FGHFOLKBHDL+timelyMen<baselineMen)continue;
            }
            selected.Add(a);budget-=a.FGHFOLKBHDL;combatBudget-=combat[a.Pointer];pickedSoldiers+=a.FGHFOLKBHDL;pickedPower+=combat[a.Pointer];}
        if(selected.Count==0)return Fail($"no-selectable-unit ready={candidates.Count} selected={selected.Count} homePower={homeCombat} border={border} keptPower={keepCombat} freePower={combatBudget}");
        Wo_DeployTeamCtrl team=null;
        var previousOfficers=generatingHomeOfficers;var previousCity=generatingHomeCity;int previousLimit=generatingOfficerPoolLimit;
        try {
            generatingHomeOfficers=officers;generatingHomeCity=city.Pointer;
            // Native JLDJEOJJLDM(...,false,false) keeps its last eligible
            // officer home. Bound the pool before allocation, rather than
            // rejecting the whole assembled army for using too many officers.
            generatingOfficerPoolLimit=officers.Count-keepOfficers+1;
            row.KJPPGIOCDID=selected;
            team=TimedAI(faction,"support-native-generator",()=>city.JLDJEOJJLDM(0,Math.Max(1,requested),capacityTransferFoodDays>0?capacityTransferFoodDays:foodDays.Value,false,false));
        }finally {row.KJPPGIOCDID=original;generatingHomeOfficers=previousOfficers;generatingHomeCity=previousCity;generatingOfficerPoolLimit=previousLimit;transactionWaves?.Clear();}
        if(!team)return Fail($"native-generator-empty selected={selected.Count} requested={requested} homeOfficers={officers.Count}");
        bool valid=row.LMMNKIOAGLC==sourceFaction&&row.IPJHOOJBCKI==sourceGovernor&&team.m_belongPower==sourceFaction
            &&!team.IncludePlayer()&&!team.m_isPrivate&&team.m_listHero!=null&&team.m_listHero.Count>0&&team.m_dicDeployArmy!=null;
        var sentOfficers=new HashSet<string>();
        if(valid)foreach(var h in team.m_listHero)if(h==null||h.GMILGFFJOIL||h.LMPMNLLGOEC?.CNOMNBCPJDI!=city.OJPECMNINNA||!officers.Contains(h.OJPECMNINNA)||!sentOfficers.Add(h.OJPECMNINNA)||NEINKOOBJIH.AOPJBNBGPLP?.Contains(h.OJPECMNINNA)==true){valid=false;break;}
        if(officers.Count-sentOfficers.Count<keepOfficers)valid=false;
        var ids=new HashSet<IntPtr>();long sent=0;
        if(valid)foreach(var group in team.m_dicDeployArmy){if(group.Value==null){valid=false;break;}foreach(var a in group.Value){
            if(!Available(a)||reserved.Contains(a.Pointer)||!original.Contains(a)||!selected.Contains(a)||!ids.Add(a.Pointer)){valid=false;break;}
            sent+=a.FGHFOLKBHDL;
        }}
        long sentCombat=0;foreach(var id in ids)if(combat.TryGetValue(id,out int value))sentCombat+=value;
        if(ids.Count==0||homeUnits-ids.Count<minimumUnits||total-sent<keep||homeCombat-sentCombat<keepCombat)valid=false;
        if(valid&&coverPower>0){
            RegionalCoverCreditForHome(city,faction,teams,homeCombat-sentCombat,out long timely,out long timelyMen,out long baselineRaw,out long baselineMen);
            if(timely<=0||homeCombat-sentCombat+timely<baselineRaw||total-sent+timelyMen<baselineMen)valid=false;
        }
        if(!valid){DismissSupport(team);return Fail($"native-result-invalid sentUnits={ids.Count} sentOfficers={sentOfficers.Count} keptOfficers={officers.Count-sentOfficers.Count} sentPower={sentCombat} keptPower={homeCombat-sentCombat} requiredKeep={keepCombat}");}
        if(Gate("assembly-command:"+city.OJPECMNINNA,15))Note($"assembly-command city={city.OJPECMNINNA} requestedPower={requested} homeOfficers={officers.Count} keepOfficers={keepOfficers} selectedUnits={selected.Count} selectedSoldiers={pickedSoldiers} sentUnits={ids.Count} sentOfficers={sentOfficers.Count} sentSoldiers={sent} unselectedSoldiers={total-pickedSoldiers} unassignedSelectedSoldiers={pickedSoldiers-sent} nativeCommandCaps=true transportFallback=false");
        if(urgent){
            if(IncomingHostile(city,faction)||city.GKLIEHCHDLD()){DismissSupport(team);return Fail("emergency-source-invaded-during-assembly");}
            if(!DistributionCity(city,out var currentOwner)||currentOwner.Pointer!=faction.Pointer){DismissSupport(team);return Fail("emergency-ownership-changed-during-assembly");}
            emergencyContext.Budget.Sent.Add(team.m_id);
        }
        if(coverPower>0&&homeCombat-sentCombat<originalBorder)TrackReliefLoan(team,city,coverBaseline,coverBaselineSoldiers);
        return team;
    }
    static bool CanGate(string key)=>!next.TryGetValue(key,out long until)||Environment.TickCount64>=until;
    // No city-wide time lock: live troop/officer reservations and source reserves
    // govern consecutive orders, including emergency support after redistribution.
}
