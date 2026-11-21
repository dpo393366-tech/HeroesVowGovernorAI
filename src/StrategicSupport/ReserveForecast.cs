using System;
using System.Collections.Generic;
using System.Reflection;

public sealed partial class StrategicSupport
{
    static List<ReinforcementTimeline.Wave> TimedReserveWaves(NJHDEHLHENN w,int side,double horizon){
        var waves=new List<ReinforcementTimeline.Wave>();
        EnsureWavesType();
        if(wavesType==null)return waves;
        if(wavesType?.GetMethod("ReserveSchedule") is MethodInfo schedule){
            if(schedule.Invoke(null,new object[]{w.Pointer,side,horizon}) is double[] data)for(int i=0;i+1<data.Length;i+=2)if(double.IsFinite(data[i])&&data[i]>=0&&data[i]<=horizon&&data[i+1]>0)waves.Add(new(data[i],(int)Math.Min(int.MaxValue,data[i+1])));
            return waves;
        }
        if(wavesType?.GetMethod("ImmediateReserveSoldiers") is System.Reflection.MethodInfo immediate){int ready=Convert.ToInt32(immediate.Invoke(null,new object[]{w.Pointer,side}));if(ready>0)waves.Add(new(0,ready));return waves;}
        double remaining=Math.Max(0,WaveNumber("remaining",double.PositiveInfinity));
        double interval=Math.Max(1,WaveNumber("interval",67.5f));int previous=0;
        for(int i=0;i<32;i++){
            double at=remaining+i*interval;if(at>horizon)break;
            int count=ReserveForecast(w,side,at);if(count>previous)waves.Add(new(at,count-previous));previous=Math.Max(previous,count);
        }
        return waves;
    }
    static Type wavesType;static long nextWavesLookup;
    static object Wave(string name)=>wavesType?.GetField(name,BindingFlags.Static|BindingFlags.NonPublic)?.GetValue(null);
    static double WaveNumber(string name,double fallback){
        var value=Wave(name);if(value==null)return fallback;
        if(value is double d)return d;
        var raw=value.GetType().GetProperty("Value")?.GetValue(value)??value.GetType().GetField("Value")?.GetValue(value);
        return raw==null?fallback:Convert.ToDouble(raw);
    }
    static void EnsureWavesType(){
        if(wavesType!=null||Environment.TickCount64<nextWavesLookup)return;nextWavesLookup=Environment.TickCount64+10000;
        foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())if((wavesType=assembly.GetType("Vow.Waves.ReserveWaves"))!=null)break;
    }
    static int ActiveBattleSoldiers(NJHDEHLHENN w,int side){
        EnsureWavesType();try{return wavesType?.GetMethod("ActiveSoldiers")?.Invoke(null,new object[]{w.Pointer,side}) is int n?n:-1;}catch(Exception e){Error(e);return -1;}
    }
    static int ReserveForecast(NJHDEHLHENN w,int side,double horizon){
        if(!double.IsFinite(horizon)||horizon<0)return 0;
        try{
            if(wavesType==null&&Environment.TickCount64>=nextWavesLookup){
                nextWavesLookup=Environment.TickCount64+10000;
                foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())if((wavesType=assembly.GetType("Vow.Waves.ReserveWaves"))!=null)break;
            }
            // Unknown scheduler: do not promise reserves that may not arrive.
            if(wavesType==null||Wave("active") is not bool active||!active||Wave("battle") is not IntPtr battle||battle!=w.Pointer)return 0;
            if(wavesType.GetMethod("ReserveSchedule") is MethodInfo schedule){long sum=0;if(schedule.Invoke(null,new object[]{w.Pointer,side,horizon}) is double[] data)for(int i=0;i+1<data.Length;i+=2)if(data[i]>=0&&data[i]<=horizon&&data[i+1]>0)sum+=(int)Math.Min(int.MaxValue,data[i+1]);return (int)Math.Min(int.MaxValue,sum);}
            if(wavesType.GetMethod("ImmediateReserveSoldiers") is System.Reflection.MethodInfo immediate)return Convert.ToInt32(immediate.Invoke(null,new object[]{w.Pointer,side}));
            double remaining=Math.Max(0,WaveNumber("remaining",double.PositiveInfinity));
            double interval=Math.Max(1,WaveNumber("interval",67.5));
            if(remaining>horizon)return 0;
            int cap=(int)Math.Clamp(WaveNumber("cap",20),1,32),per=(int)Math.Clamp(WaveNumber("count",2),1,8);
            var field=w.GPFFMNKOFCH[side];var reserve=w.KJPPGIOCDID[side];if(field==null||reserve==null)return 0;
            int slots=Math.Min(Math.Max(0,cap-field.Count),(int)Math.Min(32,(1+Math.Floor((horizon-remaining)/interval))*per));
            long soldiers=0;var seen=new System.Collections.Generic.HashSet<IntPtr>();foreach(var a in field)if(a!=null)seen.Add(a.Pointer);
            foreach(var a in reserve){if(slots<=0)break;if(a==null)break;if(!seen.Add(a.Pointer))continue;soldiers+=Math.Max(0,a.FGHFOLKBHDL);slots--;}
            return (int)Math.Min(int.MaxValue,soldiers);
        }catch(Exception e){Error(e);return 0;}
    }
    static int BattleLossReading(NJHDEHLHENN w,int side,int fallback){
        try{if(wavesType?.GetMethod("CumulativeLoss") is MethodInfo method){long loss=Convert.ToInt64(method.Invoke(null,new object[]{w.Pointer,side}));if(loss>=0)return (int)Math.Max(0,int.MaxValue-loss);}}catch(Exception e){Error(e);}
        return fallback;
    }
    static bool TargetsSupportBattle(Wo_DeployTeamCtrl t,NJHDEHLHENN w){
        if(t.m_targetWarInfo!=null)return Same(t.m_targetWarInfo,w);
        if(Same(t.m_attackTarget?.m_warInfo,w)||Same(t.m_mergeTarget?.m_warInfo,w))return true;
        return w.IJGLCFJOOIK!=null&&!string.IsNullOrEmpty(t.m_targetCity)&&t.m_targetCity==w.IJGLCFJOOIK.OJPECMNINNA;
    }
    static double HudArrival(Wo_DeployTeamCtrl t,NJHDEHLHENN w){
        // HUD consumes the actual remaining route, never performs a navigation search.
        var path=t.m_listMovePos;var at=t.GetPosition();double length=0;
        if(path==null||path.Count==0)return UnityEngine.Vector3.Distance(at,w.GBHGENNMJCK)<1?0:double.PositiveInfinity;
        foreach(var corner in path){length+=UnityEngine.Vector3.Distance(at,corner);at=corner;}
        if(UnityEngine.Vector3.Distance(at,w.GBHGENNMJCK)>1)return double.PositiveInfinity;
        double wait=missions.TryGetValue(t.Pointer,out var m)&&m.Direct?Math.Max(0,m.ReadyAt-battleClock):0;
        float battleSpeed=Wa_Controller.m_isRoundMode?GLIJMELMEHI.OHJCDAPNAOI switch{0=>.5f,2=>2f,_=>1}:AEHAGJAKNEK.IAICEJLNKFJ;
        bool owned=missions.TryGetValue(t.Pointer,out var owner)&&owner.Direct&&ValidMission(owner);
        // Native UpdateMove advances m_curSpeed * deltaTime. Our owned transport
        // compensates timeScale explicitly; manual native movement does not.
        double perWallSecond=owned?t.GetMoveSpeed()*battleSpeed:Math.Max(0,t.m_curSpeed)*Math.Max(.001,UnityEngine.Time.timeScale);
        double preparation=(wait+Math.Max(0,t.GetLeftAssembleDay())*AOHIBKLOAEI.NIOKFCDLGFK())/Math.Max(.001,battleSpeed);
        return BattlePolicy.Arrival(length,perWallSecond,preparation);
    }
    public static double SupportArrivalSeconds(IntPtr teamId,IntPtr warId){
        var w=Wa_Controller.m_warInfo;if(w==null||w.Pointer!=warId||Wa_Controller.m_isQuiting)return double.PositiveInfinity;
        if(ICGAEMLACJG.HFJIECIBFKB!=null)foreach(var pair in ICGAEMLACJG.HFJIECIBFKB){var t=pair.Value;
            if(t!=null&&t.Pointer==teamId&&t.m_warInfo==null&&TargetsSupportBattle(t,w))return HudArrival(t,w);
        }
        return double.PositiveInfinity;
    }

}

