using System;
using HarmonyLib;
using BepInEx.Configuration;

public sealed partial class StrategicSupport
{
    static ConfigEntry<bool> preferVeterans;
    void ConfigureRecruitmentQuality(){preferVeterans=Config.Bind("Recruitment","PreferEliteWithinRole",true,"Prefer the strongest affordable unlocked troop in the native-selected role. Preserve faction limits, role composition, recruitment costs and upkeep.");}
    void HookRecruitmentQuality(Harmony h)=>h.Patch(AccessTools.Method(typeof(AFAGFHJONFJ),"OBJLKMHCCAL"),postfix:new HarmonyMethod(typeof(StrategicSupport),nameof(ChooseRecruitQuality)));
    static void ChooseRecruitQuality(AFAGFHJONFJ __instance,ref ANOPPJKAPIG __result)
    {
        try{
            if(!Ready()||!budget.Value||!preferVeterans.Value||__result==null||!float.IsFinite(__result.MMBBMCCFANH)||!Eligible(__instance,out _)||FoodEmergency(__instance))return;
            var available=__instance.JIBOFLLONAH(true);if(available==null)return;
            var best=__result;
            long reserve=Math.Max(cityCashReserve.Value,Math.Max(0L,__instance.LAGKJHBEPGF(false))*3);
            foreach(var candidate in available){
                if(candidate==null||candidate.ACGEFPPKLKH!=__result.ACGEFPPKLKH||!float.IsFinite(candidate.MMBBMCCFANH)
                    ||candidate.MMBBMCCFANH<=best.MMBBMCCFANH||candidate.ELBKMJKECNF<0||candidate.GKHMPJMANAL<0)continue;
                // The native candidate list already enforces unlocks and the
                // faction's per-type cap. Keep three extra upkeep payments.
                if((long)__instance.LMPMNLLGOEC.DNOGPFHMOMO-candidate.ELBKMJKECNF-3L*candidate.GKHMPJMANAL<reserve)continue;
                if(!float.IsFinite(candidate.JLEEIFPNNEI)||candidate.JLEEIFPNNEI>Math.Max(1,__result.JLEEIFPNNEI))continue;
                best=candidate;
            }
            if(best.Pointer!=__result.Pointer){Note($"recruit-quality city={__instance.OJPECMNINNA} from={__result.OJPECMNINNA} to={best.OJPECMNINNA} role={best.ACGEFPPKLKH} elite={best.MMBBMCCFANH}");__result=best;}
        }catch(Exception e){Error(e);}
    }
}
