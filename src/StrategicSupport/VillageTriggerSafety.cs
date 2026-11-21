using System;
using UnityEngine;

public sealed partial class StrategicSupport
{
    static bool BeforeVillageTrigger(Wo_VillageCtrl __instance,Collider __0)
    {
        if(!enabled.Value||!__0)return true;
        var t=__0.GetComponent<Wo_DeployTeamCtrl>();
        if(!t||t.m_listHero!=null&&t.m_listHero.Count>0)return true;
        // The native trigger indexes m_listHero[0] in three notification paths.
        // A recycled/leaderless collider must not start capture or battle work.
        if(Gate("village-empty-army:"+t.m_id,30))Note($"village-trigger-blocked team={t.m_id} faction={t.m_belongPower} source={t.m_assembleCity} target={t.m_targetCity} state={t.m_deployState} units={TeamTroops(t).Count} reason=empty-hero-list village={__instance?.m_id}");
        return false;
    }
}
