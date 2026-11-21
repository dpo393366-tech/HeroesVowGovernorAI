using System;
using System.Collections.Generic;
using HarmonyLib;
public sealed partial class StrategicSupport
{
    const int OfficerAbilityDimensions=14;
    [ThreadStatic] static string selectedOfficer;
    [ThreadStatic] static IntPtr selectedOfficerCity;
    static void HookOfficerSelection(Harmony h)=>h.Patch(AccessTools.Method(typeof(AFAGFHJONFJ),"EOMOIINOEMB"),prefix:new HarmonyMethod(typeof(StrategicSupport),nameof(IncludeResearchWorkers)),postfix:new HarmonyMethod(typeof(StrategicSupport),nameof(FilterBalanceOfficer)));
    // Filter the native eligible list only during our synchronous generation.
    // Do not replace the city's roster, change hero jobs, or force eligibility.
    static void FilterBalanceOfficer(AFAGFHJONFJ __instance,ref Il2CppSystem.Collections.Generic.List<CLAGKDECFID> __result)
    {
        FilterResearchWorkers(__instance,ref __result);
        if(!ManualInput&&ManualAIGeneration&&enabled?.Value==true&&__result!=null){
            var available=new Il2CppSystem.Collections.Generic.List<CLAGKDECFID>();
            foreach(var h in __result)if(!ManualProtected(h))available.Add(h);__result=available;
        }
        if(generatingHomeOfficers!=null&&__instance.Pointer==generatingHomeCity&&__result!=null){
            var allowed=new Il2CppSystem.Collections.Generic.List<CLAGKDECFID>();
            foreach(var h in __result)if(h!=null&&generatingHomeOfficers.Contains(h.OJPECMNINNA)&&(!rebalancing||!FamilyRelocationProtected(h)))allowed.Add(h);
            if(generatingOfficerPoolLimit>0&&allowed.Count>generatingOfficerPoolLimit){
                // Prefer native command capacity; never raise it or alter rank.
                var ranked=new System.Collections.Generic.List<CLAGKDECFID>();foreach(var h in allowed)ranked.Add(h);
                ranked.Sort((a,b)=>{int n=b.MJICLKFOAPI(true).CompareTo(a.MJICLKFOAPI(true));return n!=0?n:string.CompareOrdinal(a.OJPECMNINNA,b.OJPECMNINNA);});
                allowed.Clear();for(int i=0;i<generatingOfficerPoolLimit;i++)allowed.Add(ranked[i]);
            }
            __result=allowed;
        }
        if(selectedOfficer==null||__instance.Pointer!=selectedOfficerCity||__result==null)return;
        var filtered=new Il2CppSystem.Collections.Generic.List<CLAGKDECFID>();
        foreach(var h in __result)if(h!=null&&h.OJPECMNINNA==selectedOfficer)filtered.Add(h);
        __result=filtered;
    }
    static OfficerBalancePolicy.Officer ReadOfficer(CLAGKDECFID h)
    {
        // Native getters verified against HLIMKFFGEPO effect indices 18,19,25,
        // 26,21,22,23,24: commerce/agriculture/conscript/train/security/patrol/explore/wall.
        // These include the character's attributes, profession and native bonuses.
        return new OfficerBalancePolicy.Officer(h.OJPECMNINNA,
            Math.Max(0,h.FKOIGKFHHKO(true)),Math.Max(0,h.FGCBICMIMOJ(true)),
            Math.Max(0,h.JNFEILANOGB(true,true)),Math.Max(0,h.IEGIBHMNKGO(true,true)),
            Math.Max(0,h.JIJBCAFMAKG(true,false)),Math.Max(0,h.KLNPHPCJPHP(true)),
            Math.Max(0,h.FDFDCEGNPGC(true)),Math.Max(0,h.IKIAIPPNKOF(true)),
            // Native attribute array indices 0..5: rule/force/wise/politics/charm/will.
            Math.Max(0,h.GGOIKBIJNCJ()),Math.Max(0,h.JLAMCIKNBON()),Math.Max(0,h.GNCHNFHKBKE()),
            Math.Max(0,h.DAEKNKAKHDA()),Math.Max(0,h.DHOMPHMNCJB()),Math.Max(0,h.IHMMCAALFKG()));
    }
    static List<OfficerBalancePolicy.Officer> OfficerRoster(AFAGFHJONFJ city,List<Wo_DeployTeamCtrl> teams,bool includeArrivals,
        Dictionary<string,OfficerBalancePolicy.Officer> cache)
    {
        var result=new List<OfficerBalancePolicy.Officer>();var ids=HomeOfficers(city,teams);var added=new HashSet<string>();
        void Add(CLAGKDECFID h){if(h==null||string.IsNullOrEmpty(h.OJPECMNINNA)||!added.Add(h.OJPECMNINNA))return;
            if(!cache.TryGetValue(h.OJPECMNINNA,out var value))cache[h.OJPECMNINNA]=value=ReadOfficer(h);result.Add(value);}
        if(city.ABJNKABKKJM!=null)foreach(var h in city.ABJNKABKKJM)if(h!=null&&ids.Contains(h.OJPECMNINNA))Add(h);
        if(includeArrivals)foreach(var t in teams)if(t.m_belongPower==city.LMPMNLLGOEC.LMMNKIOAGLC&&t.m_targetCity==city.OJPECMNINNA&&t.m_listHero!=null)foreach(var h in t.m_listHero)Add(h);
        return result;
    }
}

