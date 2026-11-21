using System;using System.Runtime.CompilerServices;
#if StrategicSupport
public static class StrategicSupport {
 public static BepInEx.Configuration.ConfigEntry<bool> ShowCityArrivalReport=new(true),ShowCityCapacityReport=new(true);
 public static BepInEx.Configuration.ConfigEntry<bool> ProtectFamilyPrivateTraining=new(true),ExcludeFamilyAutoRelocation=new(false);
 public static int Amount,Gender,Calls,CancelledId,PausedId;public static bool Paused;
 public static string RecruitmentQueueStatus()=>"성공 0 / 목표 30";
 public static string StartRecruitmentQueue(int amount,int gender){Amount=amount;Gender=gender;Calls++;return "OK";}
 public static void PauseRecruitmentQueue(bool pause){Paused=pause;}
 public static string SetRecruitmentQueuePaused(int id,bool pause){PausedId=id;if(id==1)Paused=pause;return "OK";}
 public static string CancelRecruitmentQueue(int id){CancelledId=id;return "OK";}
 public static string[][] RecruitmentQueueRows()=>new[]{new[]{"1","여성만","0 / 30명","0",Paused?"중지됨":"실행 중",Paused?"재개":"중지",Paused?"2":"1"},new[]{"2","남성만","0 / 10명","0","대기","중지","1"}};
}
#endif
#if TreasuryAwards
public static class TreasuryAwards {public static BepInEx.Configuration.ConfigEntry<int> RecipientPercent=new(50);}
#endif
#if DODGE
namespace Vow.Dodge {
 public static class DodgeLimit {
  public static BepInEx.Configuration.ConfigEntry<bool> Enabled=new(true);
  public static BepInEx.Configuration.ConfigEntry<int> Ceiling=new(30),Consecutive=new(2);
 }
 public class DodgePanel {
  public static int originalDraws,ticks;bool open;
  [MethodImpl(MethodImplOptions.NoInlining)]public void OnGUI(){originalDraws++;}
  public void Update(){ticks++;if(UnityEngine.Input.f11)open=!open;}
 }
}
#else
namespace Fake {public class Panel {
 public static Action Draw,Overlay;public static string key,title;public static bool opened;public static int originalDraws,ticks;
 [MethodImpl(MethodImplOptions.NoInlining)]public void OnGUI(){originalDraws++;Overlay?.Invoke();if(opened)Draw?.Invoke();}
 public void Update(){ticks++;
#if FieldEncounters || ReserveWaves || GovernorWatch
if(UnityEngine.Input.f12)opened=!opened;
#else
if(UnityEngine.Input.f11)opened=!opened;
#endif
}
}}
#endif

#if DuelCounters
namespace Vow.Counters {
 public static class MartialTurnFrequency {public static BepInEx.Configuration.ConfigEntry<float> frequency=new(1);}
 public static class StrengthPressure {public static BepInEx.Configuration.ConfigEntry<float> maxSpeed=new(1.5f),minWindow=new(.21f);}
 public static class ParryVisuals {public static BepInEx.Configuration.ConfigEntry<bool> warningEffects=new(true);}
 public static class DuelCounters {public static BepInEx.Configuration.ConfigEntry<float> quickBasicSpeed=new(.83f),cueWindow=new(.35f),earlyCueWindow=new(.2f),guardWindow=new(.3f),enemyCeiling=new(5);}
 public static class PlayerBattleRules {public static BepInEx.Configuration.ConfigEntry<float> multiplier=new(2);}
 public static class EnemyHealth {public static BepInEx.Configuration.ConfigEntry<float> multiplier=new(1);}
 public static class EnemyGroggy {public static BepInEx.Configuration.ConfigEntry<float> threshold=new(10),damageMultiplier=new(1.5f);}
}
#endif
#if EnemyTempo
namespace Vow.Governors {public static class EnemyTempo {public static BepInEx.Configuration.ConfigEntry<float> frequency=new(2);}}
#endif
#if FramePacing
namespace Vow.Performance {public static class FramePacing {public static BepInEx.Configuration.ConfigEntry<bool> enabled=new(false),distant=new(false);}}
#endif
