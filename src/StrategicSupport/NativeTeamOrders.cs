using System;
using System.Linq;
using HarmonyLib;

public sealed partial class StrategicSupport
{
    sealed class TeamRefreshScope
    {
        public MKMOJNAJKJE Faction;
        public Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl> Original,Filtered,Responses,FilteredResponses;
    }
    static bool HasDirectedMovement(Wo_DeployTeamCtrl t)
    {
        if(t.m_isEnterCity||t.m_targetWarInfo!=null||OtherCampaignOrder(t))return true;
        if(missions.TryGetValue(t.Pointer,out var mission)&&ValidMission(mission))return true;
        if(defenseOrders.TryGetValue(t.Pointer,out var defense)&&SameDefense(defense))return true;
        if(coverOrders.TryGetValue(t.Pointer,out var cover)&&ValidCover(cover))return true;
        if(capacityOrders.TryGetValue(t.Pointer,out var capacity)&&capacity.Id==t.m_id&&capacity.Faction==t.m_belongPower&&capacity.Target==t.m_targetCity)return true;
        return campaignGoals.TryGetValue(t.Pointer,out var campaign)&&campaign.Id==t.m_id&&campaign.Faction==t.m_belongPower&&campaign.Ordered
            &&(campaign.OrderTarget==t.m_targetCity||(campaign.Staging&&string.IsNullOrEmpty(t.m_targetCity)));
    }
    static void BeforeNativeTeamRefresh(MKMOJNAJKJE __instance,out TeamRefreshScope __state)
    {
        __state=null;if(!Ready()||__instance?.GIGPDOPDGLM==null)return;
        var original=__instance.GIGPDOPDGLM;
        var filtered=new Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl>();
        foreach(var t in original.ToArray()){
            if(!t||ManualProtected(t))continue;
            if(t.m_deployState!=AKOPKFPKJPP.Expedition){filtered.Add(t);continue;}
            bool valid=!string.IsNullOrEmpty(t.m_targetCity)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_targetCity,out var city)&&city!=null;
            bool directed=HasDirectedMovement(t);
            if(valid&&!directed){filtered.Add(t);continue;}
            if(Gate("native-team-order:"+t.m_id,30))Note($"native-team-order-preserved team={t.m_id} faction={t.m_belongPower} city={t.m_targetCity} entry={t.m_isEnterCity} targetWar={t.m_targetWarInfo!=null} path={t.m_listMovePos?.Count} reason={(directed?"explicit-order":"empty-or-invalid-city")} nativeRetargetSkipped=true");
            // Coordinate-only movement remains valid. Only truly orphaned teams
            // enter the normal daily recovery queue; never invent a city key.
            if(!valid&&!directed&&CampIdle(t))AfterCampaignDay(t);
        }
        // BLOFJHJPIOH's first loop assumes every marching team has a city key and
        // calls MoveToDes(pos,true,true). ClearTarget(true) preserves the key but
        // clears m_isEnterCity and all war/army orders. Filter only that loop;
        // Its SECOND loop also calls SetAttackTarget, clearing entry/war orders,
        // and sets responseToPlayer=true when responseTeam is null. A cancelled
        // pursuit must not be resurrected merely because it remains in that list.
        __state=new(){Faction=__instance,Original=original,Filtered=filtered};
        __instance.GIGPDOPDGLM=filtered;
        if(__instance.CFLABFGBJLE!=null){
            __state.Responses=__instance.CFLABFGBJLE;
            __state.FilteredResponses=new();
            foreach(var t in __state.Responses.ToArray())if(NativeResponseRefreshAllowed(t))__state.FilteredResponses.Add(t);
            __instance.CFLABFGBJLE=__state.FilteredResponses;
        }
    }
    static void AfterNativeTeamRefresh(TeamRefreshScope __state)
    {
        if(__state?.Faction!=null&&__state.Faction.GIGPDOPDGLM?.Pointer==__state.Filtered?.Pointer)
            __state.Faction.GIGPDOPDGLM=__state.Original;
        if(__state?.Faction!=null&&__state.FilteredResponses!=null&&__state.Faction.CFLABFGBJLE?.Pointer==__state.FilteredResponses.Pointer)
            __state.Faction.CFLABFGBJLE=__state.Responses;
    }

    static bool NativeResponseRefreshAllowed(Wo_DeployTeamCtrl t){
        if(!t||ManualProtected(t))return false;
        if(t.m_isPrivate||t.IncludePlayer())return true;
        RepairOrphanResponse(t);
        if(!t.m_responseToPlayer)return false;
        if(t.m_responseTeam&&!InterceptRelevant(t,t.m_responseTeam,out _))return false;
        if(t.m_warInfo!=null||t.m_targetWarInfo!=null||t.m_isEnterCity||!string.IsNullOrEmpty(t.m_targetCity)||!string.IsNullOrEmpty(t.m_targetCamp)||t.m_mergeTarget||t.m_targetVillage!=null||t.m_targetPass!=null||defenseOrders.ContainsKey(t.Pointer)||coverOrders.ContainsKey(t.Pointer)||missions.ContainsKey(t.Pointer)||fieldReliefOrders.ContainsKey(t.Pointer))return false;
        // A valid moving interception already has this target. Reissuing it is
        // not progress and can clear unrelated state established after dispatch.
        if(t.m_attackTarget&&t.m_responseTeam&&t.m_attackTarget.Pointer==t.m_responseTeam.Pointer&&t.m_deployState==AKOPKFPKJPP.Expedition&&((t.m_listMovePos?.Count??0)+(t.GAANFLKOMKL?.Count??0)>0))return false;
        return true;
    }
}
