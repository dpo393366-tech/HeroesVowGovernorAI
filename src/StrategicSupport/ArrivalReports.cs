using System;
using System.Text.RegularExpressions;
using BepInEx.Configuration;
using HarmonyLib;

public sealed partial class StrategicSupport
{
    public static ConfigEntry<bool> ShowCityArrivalReport, ShowCityCapacityReport;
    [ThreadStatic] static int arrivalReportDepth;
    const string ArrivalReportText="报！ANAME部队已进入BNAME！";
    const string CapacityReportText="报！ANAME部队已进入BNAME，城中部队已超出上限，请扩建军营！";
    static string arrivalTemplate,capacityTemplate;
    static Regex arrivalPattern,capacityPattern;
    static long suppressedArrivals,suppressedCapacity;

    void ConfigureArrivalReports()
    {
        ShowCityArrivalReport=Config.Bind("Reports","ShowCityArrivalReport",true,"Show ordinary army city-entry reports when enabled by vanilla report settings. F12 Governor AI / Reports; takes effect immediately.");
        ShowCityCapacityReport=Config.Bind("Reports","ShowCityCapacityReport",true,"Show army city-entry capacity-exceeded report popups. Only the popup is filtered; army entry and capacity rules remain unchanged.");
    }
    static void HookArrivalReports(Harmony h)
    {
        HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
        ArrivalReportScope.Hook(h);
        h.Patch(AccessTools.Method(typeof(UI_NotifyUI),"OpenUI"),prefix:H(nameof(FilterArrivalReport)));
    }
    // Harmony keys __state by declaring type. Keep this scope separate from
    // existing EnterCity manual-order patches, which use a ManualRelease state.
    static class ArrivalReportScope {
        public static void Hook(Harmony h){
            HarmonyMethod H(string n)=>new HarmonyMethod(typeof(ArrivalReportScope),n);
            h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"EnterCity"),prefix:H(nameof(BeginArrivalReport)),finalizer:H(nameof(EndArrivalReport)));
        }
        static void BeginArrivalReport(out int __state){__state=arrivalReportDepth;arrivalReportDepth++;}
        static void EndArrivalReport(int __state){arrivalReportDepth=__state;}
    }
    static bool MatchesArrivalTemplate(string text,string template,ref string cached,ref Regex pattern)
    {
        if(string.IsNullOrEmpty(text)||string.IsNullOrEmpty(template)||!template.Contains("ANAME")||!template.Contains("BNAME"))return false;
        if(cached!=template||pattern==null){
            pattern=new Regex("\\A"+Regex.Escape(template).Replace("ANAME",".+?").Replace("BNAME",".+?")+"\\z",RegexOptions.Singleline|RegexOptions.CultureInvariant,TimeSpan.FromMilliseconds(10));
            cached=template;
        }
        return pattern.IsMatch(text);
    }
    static bool FilterArrivalReport(string __0)
    {
        if(arrivalReportDepth<=0||(ShowCityArrivalReport?.Value!=false&&ShowCityCapacityReport?.Value!=false))return true;
        try {
            // Classify the more specific capacity template first, even when enabled:
            // in some languages it extends the ordinary arrival template.
            bool isCapacity=MatchesArrivalTemplate(__0,EKJGHIIIHNN.INIAGDLNOAP(CapacityReportText,0),ref capacityTemplate,ref capacityPattern);
            if(isCapacity&&ShowCityCapacityReport?.Value!=false)return true;
            bool blockedCapacity=isCapacity;
            bool blockedArrival=!isCapacity&&ShowCityArrivalReport?.Value==false&&MatchesArrivalTemplate(__0,EKJGHIIIHNN.INIAGDLNOAP(ArrivalReportText,0),ref arrivalTemplate,ref arrivalPattern);
            if(!blockedCapacity&&!blockedArrival)return true;
            if(blockedCapacity)suppressedCapacity++;else suppressedArrivals++;
            if(Gate("arrival-report-filter",30))Note($"arrival-report-filter normal={suppressedArrivals} capacity={suppressedCapacity} latest={(blockedCapacity?"capacity":"arrival")}");
            return false;
        }catch(Exception e){Error(e);return true;}
    }
}
