using System;using System.Collections.Generic;using System.Linq;using UnityEngine;using UnityEngine.AI;
public sealed partial class StrategicSupport {
 sealed class ThreatRoad {public long Until;public double Length;public bool Reachable;}
 static readonly Dictionary<string,ThreatRoad> threatRoads=new();
 static bool ThreatAlternative(AFAGFHJONFJ origin,AFAGFHJONFJ target,string attacker,Vector3 start,out double length){
  length=0;if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(attacker,out var f)||f==null)return false;
  var builder=new System.Text.StringBuilder("alternate:");builder.Append(origin.Pointer).Append(':').Append(target.Pointer).Append(':').Append(attacker).Append(':').Append(start.ToString());
  // A capture anywhere on a detour invalidates its cached permission result.
  foreach(var pair in MICGPAMNDKM.IJPKEMPIFIH)if(pair.Value?.LMPMNLLGOEC!=null)builder.Append('|').Append(pair.Value.Pointer).Append(':').Append(pair.Value.LMPMNLLGOEC.LMMNKIOAGLC);
  foreach(var pair in f.AENJACOFBJK)builder.Append('/').Append(pair.Key).Append(':').Append(pair.Value?.EBIOPDHEBOF);
  string key=builder.ToString();
  if(threatRoads.TryGetValue(key,out var old)&&old.Until>Environment.TickCount64){length=old.Length;return old.Reachable;}
  if(schedulerOnline){
   work.Add("threat-route:"+origin.Pointer+":"+target.Pointer+":"+attacker,()=>SafePhases(ThreatAlternativeSteps(key,start,target,f)).GetEnumerator());
   // Unknown is conservatively imminent, never "no enemy route". The sliced
   // result removes this temporary restriction on the next normal assessment.
   length=0;return true;
  }
  bool ok=ReliefPath(start,target.BMBMKPGEHBH,f,target.OJPECMNINNA,false,out _,out length);
  if(threatRoads.Count>512)threatRoads.Clear();threatRoads[key]=new(){Length=length,Reachable=ok,Until=Environment.TickCount64+10000};return ok;
 }
 static IEnumerable<string> ThreatAlternativeSteps(string key,Vector3 start,AFAGFHJONFJ target,MKMOJNAJKJE f){
  var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;var route=new RoutePlan();
  foreach(var phase in ReliefPathSteps(start,target.BMBMKPGEHBH,f,target.OJPECMNINNA,false,route)){yield return phase;if(world!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer||!CurrentFaction(f))yield break;}
  if(threatRoads.Count>512)threatRoads.Clear();threatRoads[key]=new(){Length=route.Length,Reachable=route.Success,Until=Environment.TickCount64+10000};
 }
 // A neighbouring map record is not proof that an enemy can bypass an allied
 // city. Use the native intermediate-city chain before measuring the road.
 static bool ThreatRoadLength(AFAGFHJONFJ origin,AFAGFHJONFJ target,string attacker,Vector3 start,out double length,out string why){
  length=0;why="unknown-origin";if(origin?.LMPMNLLGOEC==null||target?.LMPMNLLGOEC==null||string.IsNullOrEmpty(attacker))return false;
  var legs=new List<Vector3>();var seen=new HashSet<IntPtr>{origin.Pointer};var stamp=origin.Pointer+":"+target.Pointer+":"+attacker+":"+target.LMPMNLLGOEC.LMMNKIOAGLC;
  bool Transit(AFAGFHJONFJ c){string owner=c?.LMPMNLLGOEC?.LMMNKIOAGLC;return c!=null&&(string.IsNullOrEmpty(owner)||owner==attacker||(NEINKOOBJIH.COCMABJFOFH.TryGetValue(attacker,out var f)&&f.AENJACOFBJK.TryGetValue(owner,out var r)&&r!=null&&(r.EBIOPDHEBOF==BHBNKONCOJD.Ally||r.EBIOPDHEBOF==BHBNKONCOJD.ArmyPass)));}
  if(origin.Pointer!=target.Pointer){
   if(origin.MEKIDAONKMD!=null&&origin.MEKIDAONKMD.TryGetValue(target.OJPECMNINNA,out var road)&&road?.EJMDIPFNGMG!=null){
    foreach(var via in road.EJMDIPFNGMG){
     if(via==null){why="unknown-intermediate";return false;}if(via.Pointer==origin.Pointer||via.Pointer==target.Pointer)continue;
     if(!seen.Add(via.Pointer)){why="cyclic-road";return false;}
     if(!Transit(via)){why="screened-by:"+via.OJPECMNINNA;if(ThreatAlternative(origin,target,attacker,start,out length)){why="validated-alternate-road";return true;}return false;}
     stamp+=":"+via.Pointer+":"+via.LMPMNLLGOEC?.LMMNKIOAGLC;legs.Add(via.BMBMKPGEHBH);
    }
   }else if(origin.FKMKGNPFLFP?.ToArray().Any(c=>c?.Pointer==target.Pointer)!=true&&target.FKMKGNPFLFP?.ToArray().Any(c=>c?.Pointer==origin.Pointer)!=true){why="no-direct-road";return false;}
  }
  legs.Add(target.BMBMKPGEHBH);stamp+=":"+start.ToString();foreach(var leg in legs)stamp+=":"+leg.ToString();
  if(threatRoads.TryGetValue(stamp,out var cache)&&cache.Until>Environment.TickCount64){length=cache.Length;why=cache.Reachable?"native-road-cached":"no-complete-road";return cache.Reachable;}
  bool valid=true;var at=start;foreach(var leg in legs){
   if(!NavMesh.SamplePosition(at,out var a,5,NavMesh.AllAreas)||!NavMesh.SamplePosition(leg,out var b,5,NavMesh.AllAreas)){valid=false;break;}
   var path=new NavMeshPath();if(!NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,path)||path.status!=NavMeshPathStatus.PathComplete){valid=false;break;}
   var corners=path.corners;if(corners==null||corners.Length==0||Vector3.Distance(corners[corners.Length-1],b.position)>1){valid=false;break;}
   for(int i=1;i<corners.Length;i++)length+=Vector3.Distance(corners[i-1],corners[i]);at=b.position;
  }
  if(threatRoads.Count>512)threatRoads.Clear();threatRoads[stamp]=new(){Until=Environment.TickCount64+10000,Length=length,Reachable=valid};why=valid?"native-road":"no-complete-road";return valid;
 }
}
