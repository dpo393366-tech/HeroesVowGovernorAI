using System;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class StrategicSupport
{
    static bool BeforeArmyNavigationDrawing(Wa_ArmyCtrl __instance){
        var t=__instance;if(!t)return false;
        // Native drawing dereferences both line renderers even for an empty
        // path. Reserve/model initialization and disposal need not finish in
        // the same frame as this callback. Guard only missing draw resources;
        // never clear destinations or suppress movement/combat updates.
        bool ready=t.m_listDestination!=null&&t.GFJGCKCEIHC&&t.KIJLPJKGJMJ;
        if(ready&&t.m_listDestination.Count>0)ready=t.m_attribute!=null&&t.m_transform&&t.m_navAgent&&t.NJEMJKJHHBE!=null;
        if(ready)return true;
        if(t.GFJGCKCEIHC)t.GFJGCKCEIHC.enabled=false;
        if(t.KIJLPJKGJMJ)t.KIJLPJKGJMJ.enabled=false;
        if(Gate("army-navigation-ready:"+t.Pointer,30))Note($"army-navigation-deferred army={t.Pointer} destinations={t.m_listDestination!=null} attribute={t.m_attribute!=null} lines={(bool)t.GFJGCKCEIHC}/{(bool)t.KIJLPJKGJMJ} transform={(bool)t.m_transform} agent={(bool)t.m_navAgent} scratch={t.NJEMJKJHHBE!=null} movementPreserved=true");
        return false;
    }
    sealed class WorldVisualMask
    {
        public Wo_DeployTeamCtrl Team;public int Id;
        public HUD_DeployTeam Hud;public bool HudActive;
        public LineRenderer Line,Arrow;public bool LineEnabled,ArrowEnabled;
    }
    static readonly Dictionary<IntPtr,WorldVisualMask> worldVisualMasks=new();
    static bool visualsInBattle;static long nextVisualSweep;
    static bool BattleDisplayActive()=>Wa_Controller.m_warInfo!=null||Wa_Controller.m_mockInfo!=null;
    static void MaskWorldVisual(Wo_DeployTeamCtrl t)
    {
        if(!t)return;
        if(!worldVisualMasks.TryGetValue(t.Pointer,out var mask)||mask.Id!=t.m_id)
            worldVisualMasks[t.Pointer]=mask=new(){Team=t,Id=t.m_id};
        if(t.m_hud){
            if(mask.Hud?.Pointer!=t.m_hud.Pointer){mask.Hud=t.m_hud;mask.HudActive=t.m_hud.gameObject.activeSelf;}
            if(t.m_hud.gameObject.activeSelf)t.m_hud.gameObject.SetActive(false);
        }
        if(t.m_lineRenderer){
            if(mask.Line?.Pointer!=t.m_lineRenderer.Pointer){mask.Line=t.m_lineRenderer;mask.LineEnabled=mask.Line.enabled;}
            if(mask.Line.enabled)mask.Line.enabled=false;
        }
        if(t.m_lineRendererArrow){
            if(mask.Arrow?.Pointer!=t.m_lineRendererArrow.Pointer){mask.Arrow=t.m_lineRendererArrow;mask.ArrowEnabled=mask.Arrow.enabled;}
            if(mask.Arrow.enabled)mask.Arrow.enabled=false;
        }
    }
    static bool BeforeWorldHud(Wo_DeployTeamCtrl __instance)
    {
        if(!enabled.Value||!BattleDisplayActive())return true;
        try{MaskWorldVisual(__instance);}catch(Exception e){Error(e);}return false;
    }
    static void SyncWorldVisuals()
    {
        bool active=enabled.Value&&BattleDisplayActive();
        if(active){
            if(visualsInBattle&&Environment.TickCount64<nextVisualSweep)return;
            visualsInBattle=true;nextVisualSweep=Environment.TickCount64+250;
            if(ICGAEMLACJG.HFJIECIBFKB!=null)foreach(var kv in ICGAEMLACJG.HFJIECIBFKB)MaskWorldVisual(kv.Value);
            return;
        }
        if(!visualsInBattle&&worldVisualMasks.Count==0)return;
        foreach(var mask in worldVisualMasks.Values){var t=mask.Team;
            // A pooled/recycled HUD can now belong to a different army. Restore
            // only the exact still-registered team and its current components.
            if(!t||t.m_id!=mask.Id||ICGAEMLACJG.HFJIECIBFKB==null||!ICGAEMLACJG.HFJIECIBFKB.TryGetValue(mask.Id,out var current)||current?.Pointer!=t.Pointer)continue;
            if(mask.Hud&&t.m_hud?.Pointer==mask.Hud.Pointer&&mask.HudActive)mask.Hud.gameObject.SetActive(true);
            if(mask.Line&&t.m_lineRenderer?.Pointer==mask.Line.Pointer&&mask.LineEnabled)mask.Line.enabled=true;
            if(mask.Arrow&&t.m_lineRendererArrow?.Pointer==mask.Arrow.Pointer&&mask.ArrowEnabled)mask.Arrow.enabled=true;
        }
        Note($"battle-world-visuals restoredTeams={worldVisualMasks.Count} worldMovementPreserved=true");
        worldVisualMasks.Clear();visualsInBattle=false;
    }
}
