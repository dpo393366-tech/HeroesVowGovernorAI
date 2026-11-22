using System;using System.Collections.Generic;
public sealed partial class StrategicSupport {
 // Prepare geographical distances while an invasion is still marching.
 // These are not troop commitments or permission to cross a future battle.
 static IEnumerable<string> WarmAttackRoads(Wo_DeployTeamCtrl army,int id,string goal){
  if(!LiveTeam(army)||army.m_id!=id||!MarchAttackCandidate(army,out var target)||target.OJPECMNINNA!=goal||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(army.m_belongPower,out var f))yield break;
  var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;var endpoint=target.BMBMKPGEHBH;string stamp=RoadPolicyStamp(f);int calculated=0,reused=0;
  foreach(var city in ProtectedCities(f)){
   yield return "attack-support-road-prepare";
   if(!LiveTeam(army)||army.m_id!=id||army.m_warInfo!=null||army.m_targetCity!=goal||!CurrentFaction(f)||world!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer||RoadPolicyStamp(f)!=stamp)yield break;
   string key=RoadCacheKey(city.BMBMKPGEHBH,endpoint,f,goal,false);
   if(savedBattleRoads.TryGetValue(key,out var known)&&known.World==world&&known.Stamp==stamp&&known.Success){reused++;continue;}
   var route=new RoutePlan();
   foreach(var step in ReliefPathSteps(city.BMBMKPGEHBH,endpoint,f,goal,false,route)){
    yield return step;if(!LiveTeam(army)||army.m_id!=id||army.m_warInfo!=null||army.m_targetCity!=goal||world!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer)yield break;
   }
   if(!route.Success)continue;
   // Same bounded storage as real battle roads. A changed corridor revision
   // MUST run FirstCorridorBlock before any warmed path can be used.
   TrimBattleRoads();savedBattleRoads[key]=new(){World=world,Stamp=stamp,Length=route.Length,Success=true,Checked=Environment.TickCount64,Points=new(route.Points),Legs=new(route.Legs),Corridors=-1};calculated++;
  }
  if(calculated>0)Note($"attack-support-roads-prepared team={id} target={goal} calculated={calculated} reused={reused} basis=actual-map-paths recheckBattleCorridors=true");
 }
}
