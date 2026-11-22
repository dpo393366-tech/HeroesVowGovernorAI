using System;using System.Collections.Generic;using UnityEngine;
public sealed partial class StrategicSupport {
 sealed class SavedBattleRoad {public IntPtr? World;public string Stamp;public double Length;public long Checked;public bool Success,Detour;public List<Vector3> Points,Legs;public NJHDEHLHENN Blocker;public long Corridors;}
 static readonly Dictionary<string,SavedBattleRoad> savedBattleRoads=new();
 static string BattleRoadKey(AFAGFHJONFJ c,NJHDEHLHENN w,MKMOJNAJKJE f)=>RoadCacheKey(c.BMBMKPGEHBH,w.GBHGENNMJCK,f,BattleTransitRegion(w,f),false);
 // Store distance rather than ETA: troop speed and preparation options can
 // change independently of geography. Reuse across wars at the same place.
 static bool SavedBattleQuote(AFAGFHJONFJ c,NJHDEHLHENN w,MKMOJNAJKJE f,string key,string stamp,out double eta){
  eta=double.PositiveInfinity;string road=BattleRoadKey(c,w,f);
  if(!savedBattleRoads.TryGetValue(road,out var cached)||cached.World!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer||cached.Stamp!=stamp)return false;
  if(cached.Corridors!=corridorRevision){
   if(!cached.Success||cached.Detour||FirstCorridorBlock(cached.Points,w,f)!=null)return false;
   cached.Corridors=corridorRevision;
  }
  if(!cached.Success&&cached.Blocker==null&&Environment.TickCount64-cached.Checked>=3000)return false;
  float speed=Wo_DeployTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL,false);
  if(cached.Success&&speed>0)eta=cached.Length/speed+mobilizeSeconds.Value;
  battleRoutes[key]=(eta,long.MaxValue,stamp);
  // Fixed geometry does not expire by wall clock. Ownership, diplomacy,
  // topology and battle corridors still validate independently above.
  return true;
 }
 static void TrimBattleRoads(){
  if(savedBattleRoads.Count>=8192){string oldest=null;long at=long.MaxValue;foreach(var p in savedBattleRoads)if(p.Value.Checked<at){at=p.Value.Checked;oldest=p.Key;}if(oldest!=null)savedBattleRoads.Remove(oldest);}
 }
 static void SaveBattleRoad(AFAGFHJONFJ c,NJHDEHLHENN w,MKMOJNAJKJE f,RoutePlan route,string stamp){
  TrimBattleRoads();savedBattleRoads[BattleRoadKey(c,w,f)]=new(){World=MICGPAMNDKM.IJPKEMPIFIH?.Pointer,Stamp=stamp,Length=route.Length,Success=route.Success,Checked=Environment.TickCount64,Points=new(route.Points),Legs=new(route.Legs),Blocker=route.CorridorBlocker,Detour=route.CorridorDetour,Corridors=corridorRevision};
 }
 static void InvalidateBattleRoad(AFAGFHJONFJ c,NJHDEHLHENN w,MKMOJNAJKJE f){
  string key=BattleRoadKey(c,w,f);var at=c.BMBMKPGEHBH;
  if(savedBattleRoads.TryGetValue(key,out var old)&&old.Legs!=null)foreach(var end in old.Legs){ForgetFixedRoad(at,end);at=end;}
  ForgetFixedRoad(c.BMBMKPGEHBH,w.GBHGENNMJCK);savedBattleRoads.Remove(key);
 }
}
