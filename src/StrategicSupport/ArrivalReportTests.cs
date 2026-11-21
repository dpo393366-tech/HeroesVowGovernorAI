using System;
using System.Reflection;
using BepInEx.Configuration;

// Localization stand-in; native translation is exercised only in the game.
public static class EKJGHIIIHNN {
    public static string Language="zh";public static bool Fail;
    public static string INIAGDLNOAP(string text,int count){
        if(Fail)throw new Exception("translation unavailable");
        if(Language=="zh")return text;
        bool capacity=text.Contains("超出");
        if(Language=="ko")return capacity?"보고! ANAME 부대가 BNAME에 진입했으나 성내 부대가 상한을 초과했습니다. 병영을 확장해 주십시오!":"보고! ANAME 부대가 BNAME에 진입했습니다!";
        return capacity?"ANAME entered BNAME: capacity exceeded!":"ANAME entered BNAME!";
    }
}
public static class ArrivalReportTests {
    const BindingFlags Flags=BindingFlags.Static|BindingFlags.NonPublic;
    static int checks;
    static object Call(string name,params object[] args)=>(typeof(StrategicSupport).GetMethod(name,Flags)??typeof(StrategicSupport).GetNestedType("ArrivalReportScope",BindingFlags.NonPublic).GetMethod(name,Flags)).Invoke(null,args);
    static bool Allow(string text)=>(bool)Call("FilterArrivalReport",text);
    static int Begin(){object[] state={0};Call("BeginArrivalReport",state);return (int)state[0];}
    static void Check(bool value,string label){checks++;if(!value)throw new Exception("Report108: "+label);}
    static string Message(bool capacity)=>EKJGHIIIHNN.INIAGDLNOAP(capacity?"报！ANAME部队已进入BNAME，城中部队已超出上限，请扩建军营！":"报！ANAME部队已进入BNAME！",0).Replace("ANAME","<color=red>오미제천</color>").Replace("BNAME","여남");
    public static void Run(){
        StrategicSupport.ShowCityArrivalReport=new ConfigEntry<bool>(true);StrategicSupport.ShowCityCapacityReport=new ConfigEntry<bool>(true);
        foreach(var lang in new[]{"zh","ko","en"}){
            EKJGHIIIHNN.Language=lang;
            foreach(bool ordinary in new[]{false,true})foreach(bool capacity in new[]{false,true}){
                StrategicSupport.ShowCityArrivalReport.Value=ordinary;StrategicSupport.ShowCityCapacityReport.Value=capacity;
                Check(Allow(Message(false))&&Allow(Message(true)),"identical text outside EnterCity passes");
                int old=Begin();
                try{
                    Check(Allow(Message(false))==ordinary,"ordinary switch independent");
                    Check(Allow(Message(true))==capacity,"capacity switch independent");
                    Check(Allow("자동 등용 완료")&&Allow("국고 분배 완료")&&Allow("적군이 접근 중")&&Allow(null),"unrelated/empty messages survive");
                    Check(Allow(Message(true)+" additional confirmation"),"partial text cannot match");
                }finally{Call("EndArrivalReport",old);}
            }
        }
        StrategicSupport.ShowCityArrivalReport.Value=false;StrategicSupport.ShowCityCapacityReport.Value=false;
        int outer=Begin(),inner=Begin();Call("EndArrivalReport",inner);Check(!Allow(Message(true)),"nested exit retains outer scope");
        try{throw new InvalidOperationException("native failure");}catch(InvalidOperationException){}finally{Call("EndArrivalReport",outer);}
        Check(Allow(Message(true)),"exception finalization clears scope");
        int scope=Begin();
        try{
            Check(!Allow(Message(true)),"disabled before live toggle");StrategicSupport.ShowCityCapacityReport.Value=true;
            Check(Allow(Message(true)),"toggle takes effect without restart");
            StrategicSupport.ShowCityCapacityReport.Value=false;EKJGHIIIHNN.Fail=true;
            Check(Allow("unexpected"),"translation failure preserves report");
        }finally{EKJGHIIIHNN.Fail=false;Call("EndArrivalReport",scope);}
        StrategicSupport.ShowCityArrivalReport.Value=true;StrategicSupport.ShowCityCapacityReport.Value=true;
        Console.WriteLine($"PASS Report108 {checks} scoped report/config/localization checks; native UI mocked.");
    }
}
