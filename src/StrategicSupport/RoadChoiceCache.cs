using System;using System.Collections.Generic;using System.Linq;using UnityEngine;
public sealed partial class StrategicSupport {
 sealed class RoadChoice {public IntPtr? World;public string Stamp;public long Until;public double Length;public List<Vector3> Legs;}
 static readonly Dictionary<string,RoadChoice> roadChoices=new();
 sealed class RoadCityPolicy {public IntPtr City;public string Owner;public int Relation;public readonly List<IntPtr> Edges=new();}
 sealed class RoadPolicyState {public IntPtr? World;public string Stamp;public readonly List<RoadCityPolicy> Cities=new();}
 static readonly Dictionary<IntPtr,RoadPolicyState> roadPolicyStates=new();
 static string RoadCacheKey(Vector3 a,Vector3 b,MKMOJNAJKJE f,string allowed,bool escape)=>f.Pointer+"|"+a.x.ToString("R")+","+a.y.ToString("R")+","+a.z.ToString("R")+"|"+b.x.ToString("R")+","+b.y.ToString("R")+","+b.z.ToString("R")+"|"+allowed+"|"+escape;
 // Ownership and passage changes invalidate routes immediately, even inside
 // the short cache lifetime. Actual native navigation is replayed at use.
 static string RoadPolicyStamp(MKMOJNAJKJE f){
  // Compare every live owner/relation/edge, without allocating and sorting a
  // new world-sized string for each path. No TTL, hash collision or stale
  // ownership window: any difference falls back to the original exact stamp.
  var registry=MICGPAMNDKM.IJPKEMPIFIH;
  if(roadPolicyStates.TryGetValue(f.Pointer,out var state)&&state.World==registry?.Pointer){
   bool same=true;int i=0;
   foreach(var pair in registry){var c=pair.Value;if(c==null)continue;
    if(i>=state.Cities.Count){same=false;break;}var before=state.Cities[i++];
    string owner=c.LMPMNLLGOEC?.LMMNKIOAGLC??"";int relation=f.AENJACOFBJK.TryGetValue(owner,out var rel)&&rel!=null?(int)rel.EBIOPDHEBOF:-1;
    if(before.City!=c.Pointer||before.Owner!=owner||before.Relation!=relation){same=false;break;}
    int e=0;if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n!=null){if(e>=before.Edges.Count||before.Edges[e++]!=n.Pointer){same=false;break;}}
    if(!same||e!=before.Edges.Count){same=false;break;}
   }
   if(same&&i==state.Cities.Count)return state.Stamp;
  }
  state=new(){World=registry?.Pointer};
  foreach(var pair in registry){var c=pair.Value;if(c==null)continue;string owner=c.LMPMNLLGOEC?.LMMNKIOAGLC??"";
   var item=new RoadCityPolicy{City=c.Pointer,Owner=owner,Relation=f.AENJACOFBJK.TryGetValue(owner,out var rel)&&rel!=null?(int)rel.EBIOPDHEBOF:-1};
   if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n!=null)item.Edges.Add(n.Pointer);state.Cities.Add(item);
  }
  state.Stamp=BuildRoadPolicyStamp(f);if(roadPolicyStates.Count>=128)roadPolicyStates.Clear();roadPolicyStates[f.Pointer]=state;return state.Stamp;
 }
 static string BuildRoadPolicyStamp(MKMOJNAJKJE f){
  var values=new List<string>();
  foreach(var pair in MICGPAMNDKM.IJPKEMPIFIH){var c=pair.Value;if(c==null)continue;
   string owner=c.LMPMNLLGOEC?.LMMNKIOAGLC??"";int relation=f.AENJACOFBJK.TryGetValue(owner,out var rel)&&rel!=null?(int)rel.EBIOPDHEBOF:-1;
   var edges=new List<string>();if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n!=null)edges.Add(n.Pointer.ToString());edges.Sort(StringComparer.Ordinal);
   values.Add(c.Pointer+":"+owner+":"+relation+":"+string.Join(",",edges));
  }
  values.Sort(StringComparer.Ordinal);return string.Join(";",values);
 }
 static void AppendRoad(RoutePlan target,RoutePlan leg){target.Length+=leg.Length;foreach(var p in leg.Points)if(target.Points.Count==0||Vector3.Distance(target.Points[target.Points.Count-1],p)>.01f)target.Points.Add(p);}
 static void CopyRoad(RoutePlan target,RoutePlan source){target.Success=source.Success;target.Length=source.Length;target.Points=new(source.Points);target.Legs=new(source.Legs);target.Failure=source.Failure;}
}
