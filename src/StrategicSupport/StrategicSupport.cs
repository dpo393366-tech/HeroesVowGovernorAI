using System;
using System.Collections.Generic;
using System.Diagnostics;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

[BepInPlugin("local.heroesvow.strategicsupport","Strategic Support","1.5.44")]
public sealed partial class StrategicSupport:BasePlugin
{
    static ManualLogSource log;
    static ConfigEntry<bool> enabled,support,budget;
    static ConfigEntry<int> reserveCash,cityLimit,foodDays,minUnits;
    static ConfigEntry<float> budgetShare,retain,coverage;
    static bool busy;
    static IntPtr world;
    static readonly Dictionary<string,long> next=new();
    static long nextError;
    [ThreadStatic] static int governorStaffing;
    public override void Load()
    {
        log=Log; ConfigurePerformance(); ConfigureArrivalReports(); ConfigureFamilyProtection();
        enabled=Config.Bind("General","Enabled",true,"Enable strategic support and recruitment funding.");
        support=Config.Bind("Defense","Enabled",true,"Reinforce threatened friendly cities using available regular garrisons.");
        budget=Config.Bind("Recruitment","Enabled",true,"Transfer real treasury funds to eligible AI-managed cities before native recruitment/staffing.");
        reserveCash=Config.Bind("Recruitment","TreasuryReserve",100000,new ConfigDescription("Minimum treasury reserve.",new AcceptableValueRange<int>(0,100000000)));
        budgetShare=Config.Bind("Recruitment","SpareBudgetShare",0.20f,new ConfigDescription("Maximum share of treasury above reserve per transfer.",new AcceptableValueRange<float>(0,0.5f)));
        cityLimit=Config.Bind("Recruitment","TransferLimit",100000,new ConfigDescription("Maximum single city transfer.",new AcceptableValueRange<int>(0,10000000)));
        retain=Config.Bind("Defense","RetainFraction",0.6f,new ConfigDescription("Retain this fraction of source soldiers, including previous support commitments.",new AcceptableValueRange<float>(0.5f,0.9f)));
        minUnits=Config.Bind("Defense","MinimumGarrisonUnits",2,new ConfigDescription("Never send the last garrison units.",new AcceptableValueRange<int>(1,20)));
        coverage=Config.Bind("Defense","ThreatCoverage",1.2f,new ConfigDescription("Target defensive combat power relative to incoming hostile armies.",new AcceptableValueRange<float>(1,2)));
        foodDays=Config.Bind("Defense","FoodDays",60,new ConfigDescription("Native supply target for a support army.",new AcceptableValueRange<int>(30,150)));
        ConfigureOffense(); ConfigureAdministration(); ConfigureStatecraft(); ConfigureBattle(); ConfigureSupply(); ConfigureInvestment(); ConfigureRecruitmentQuality(); ConfigureGarrisonBalance();
        var h=new Harmony("local.heroesvow.strategicsupport");
        HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
        try {
            HookConvoyAudit(h); HookArrivalReports(h); HookManualOrders(h); HookFamilyProtection(h); HookParcelPool(h); HookOffense(h); HookAdministration(h); HookBattle(h); HookInvestment(h);
            h.Patch(AccessTools.Method(typeof(FNFJODHIJFO),"GHNEGBJANJG"),postfix:H(nameof(AfterPlan)));
            h.Patch(AccessTools.Method(typeof(FNFJODHIJFO),"OBIOEOJAHMM"),prefix:H(nameof(BeginStaff)),finalizer:H(nameof(EndStaff)));
            h.Patch(AccessTools.Method(typeof(AFAGFHJONFJ),"OBIOEOJAHMM"),prefix:H(nameof(BeforeStaff)));
            h.Patch(AccessTools.Method(typeof(AFAGFHJONFJ),"JFOOAMEIMDO"),prefix:H(nameof(BeforeRecruit)));
            HookRecruitmentQuality(h); HookOfficerSelection(h); StartWorkScheduler();
        } catch(Exception e) {Note("STARTUP FAILED "+e);h.UnpatchSelf();throw;}
        Note("Strategic Support 1.5.44 loaded; build=defense-intercept-r135; event-driven defense handoff, local defender retention and allied interception purpose; native wall socket registration and legacy zero-progress recovery; ready construction has a separate bounded execution share; corridor-aware support and bounded breakthrough planning; editor compatibility r125 retained; independent cached construction, background layout, research, maintenance and goal redevelopment; family protection, native construction time, costs, goal placement and civic safety retained.");
    }
    static void Error(Exception e){if(Environment.TickCount64<nextError)return;nextError=Environment.TickCount64+30000;Note("ERROR "+e);log.LogWarning(e.ToString());}
    static bool Ready()
    {
        if(!enabled.Value || NEINKOOBJIH.POMNMBGDDIN==null || MICGPAMNDKM.IJPKEMPIFIH==null)return false;
        var p=MICGPAMNDKM.IJPKEMPIFIH.Pointer;
        if(world!=p){world=p;ResetManualOrders();ResetWork();ResetDefense();occupations.Clear();huntOrders.Clear();standoffs.Clear();failedIntercepts.Clear();campaignGoals.Clear();capacityOrders.Clear();ResetOffense();cargoOutcomes.Clear();ResetCoordination();next.Clear();budgets.Clear();remodelJobs.Clear();economyCursors.Clear();layoutIntents.Clear();statecraftDays.Clear();ResetBattleState();}
        return true;
    }
    static bool Eligible(AFAGFHJONFJ c,out MKMOJNAJKJE f)
        =>Eligibility(c,out f)==null;
    static string Eligibility(AFAGFHJONFJ c,out MKMOJNAJKJE f)
    {
        f=null;var row=c?.LMPMNLLGOEC;
        if(row==null || string.IsNullOrEmpty(row.LMMNKIOAGLC) || string.IsNullOrEmpty(row.IPJHOOJBCKI))return "no-faction-or-governor";
        if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(row.LMMNKIOAGLC,out f) || f?.ANMMMOIKLEG==null)return "no-faction-record";
        if(!f.ANMMMOIKLEG.TryGetValue(row.IPJHOOJBCKI,out var g) || g==null)return "no-governor-record";
        // Preserve the player's personal district and manually managed cities.
        if(row.IPJHOOJBCKI==NEINKOOBJIH.OPIKIHPNCIP)return "player-personal-governor";
        // AKIJOBLFLCJ also matches the faction ruler. It cannot distinguish
        // delegated cities in the player's faction from personal management.
        // Native NBKEECPNIJH checks actual governor/mayor assignments and only
        // then consults the personal automation toggle. Do not mutate that toggle.
        if(!c.NBKEECPNIJH(true))return "native-city-management-disabled";
        return g.LMMNKIOAGLC==row.LMMNKIOAGLC?null:"governor-faction-mismatch";
    }
    static bool Gate(string key,int seconds)
    {
        long now=Environment.TickCount64;
        if(next.TryGetValue(key,out long until)&&now<until)return false;
        next[key]=now+seconds*1000L;return true;
    }
    static void BeginStaff(out int __state){__state=governorStaffing;governorStaffing++;}
    static void EndStaff(int __state){governorStaffing=__state;}
    static void BeforeStaff(AFAGFHJONFJ __instance){if(governorStaffing>0)Fund(__instance,true);}
    static void BeforeRecruit(AFAGFHJONFJ __instance,bool __0){if(!__0)Fund(__instance,false);}
    static void Fund(AFAGFHJONFJ c,bool staffing)
    {
        if(busy)return;
        try {
            if(!Ready() || !budget.Value || !Eligible(c,out var f))return;
            var row=c.LMPMNLLGOEC; var armies=row.KJPPGIOCDID;if(armies==null || row.PNHICLAGHGD<=0)return;
            bool need=false;
            foreach(var a in armies)if(a!=null && a.MGLFFEANDOI && a.MJNINDNBJHA && a.FGHFOLKBHDL<EBNFFALGBPA.LLLBAAAEIBB){need=true;break;}
            if(!need && !(staffing && armies.Count<c.JNLIOIHLDHH()))return;
            long cost=Math.Max(0,c.LAGKJHBEPGF(false));
            long desired=staffing ? Math.Max(50001, cost*3) : cost*3;
            int amount=StrategyPolicy.Transfer(f.DNOGPFHMOMO,row.DNOGPFHMOMO,desired,Math.Max(reserveCash.Value,SharedReserve(f)),1,Budget(f).RecruitmentCap);
            if(amount<=0 || !AIGate("fund:"+c.OJPECMNINNA,f.IBGCLCAIBOL,10))return;
            int treasury=f.DNOGPFHMOMO, cash=row.DNOGPFHMOMO;
            // Data setters prevent the income-multiplier mod treating a transfer as new income.
            // Both balances are committed on the game thread before native spending resumes.
            f.DNOGPFHMOMO=treasury-amount;
            try {row.DNOGPFHMOMO=cash+amount;} catch {f.DNOGPFHMOMO=treasury;throw;}
            Note($"fund city={c.OJPECMNINNA} faction={f.IBGCLCAIBOL} amount={amount} treasury={treasury}->{f.DNOGPFHMOMO} cityCash={cash}->{row.DNOGPFHMOMO}");
        } catch(Exception e){Error(e);}
    }
    static bool War(MKMOJNAJKJE faction,string other)=>!string.IsNullOrEmpty(other) && faction.AENJACOFBJK!=null
        && faction.AENJACOFBJK.TryGetValue(other,out var rel)&&rel!=null&&rel.EBIOPDHEBOF==BHBNKONCOJD.War;
    static int Power(AFAGFHJONFJ city){return Math.Max(0,city.GNCKPEPJDGF(0,false,1f,out int count));}
    // JLDJEOJJLDM's native roster filter checks headcount, not MGLFFEANDOI.
    // Keep explicit busy-state and live-team reservation guards at our callers.
    static bool Available(DHKINGPDKJJ a)=>a!=null&&!ManualProtected(a)&&a.FGHFOLKBHDL>=100&&a.FGHFOLKBHDL>EBNFFALGBPA.LLLBAAAEIBB*.4
        &&(a.GDHPLIFJCBN==PMNFKDJNLDJ.None||a.GDHPLIFJCBN==PMNFKDJNLDJ.Supplement||a.GDHPLIFJCBN==PMNFKDJNLDJ.Train);
    static List<AFAGFHJONFJ> Neighbours(AFAGFHJONFJ target,string faction)
    {
        var result=new List<AFAGFHJONFJ>();var seen=new HashSet<string>{target.OJPECMNINNA};
        var frontier=new List<AFAGFHJONFJ>{target};
        for(int hop=0;hop<2;hop++){
            var following=new List<AFAGFHJONFJ>();
            foreach(var c in frontier){if(c.FKMKGNPFLFP==null)continue;foreach(var n in c.FKMKGNPFLFP)
                if(n?.LMPMNLLGOEC?.LMMNKIOAGLC==faction&&seen.Add(n.OJPECMNINNA)){result.Add(n);following.Add(n);}}
            frontier=following;
        }
        return result;
    }
    static void AfterPlan(FNFJODHIJFO __instance)
    {
        AfterGovernorEconomy(__instance);
        try { if(Ready()&&__instance!=null&&NEINKOOBJIH.COCMABJFOFH.TryGetValue(__instance.LMMNKIOAGLC,out var f)) {
            if(schedulerOnline)ScheduleRegularDefense(f);
            else if(AIGate("defense-evaluation:"+f.IBGCLCAIBOL,f.IBGCLCAIBOL,2))foreach(var phase in DefenseSteps(f)){}
        }} catch(Exception e){Error(e);}
    }
}
