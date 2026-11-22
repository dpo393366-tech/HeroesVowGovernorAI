using System;using System.Collections.Generic;using UnityEngine;using UnityEngine.AI;
public sealed partial class StrategicSupport {
 // Map geometry belongs to the map, not to a faction or its diplomatic state.
 // Positive native corner chains survive captures and policy-cache refreshes.
 // Moving endpoints form different keys; no rounding or straight-line substitute.
 sealed class FixedRoadLeg {public Vector3 End;public Vector3[] Points;public double Length;}
 static readonly Dictionary<(float,float,float,float,float,float),FixedRoadLeg> fixedRoadLegs=new();
 static readonly Queue<(float,float,float,float,float,float)> fixedRoadOrder=new();
 static IntPtr? fixedRoadWorld;static int fixedRoadCorners;
 static (float,float,float,float,float,float) FixedRoadKey(Vector3 a,Vector3 b)=>(a.x,a.y,a.z,b.x,b.y,b.z);
 static void ResetFixedRoads(){fixedRoadLegs.Clear();fixedRoadOrder.Clear();fixedRoadCorners=0;fixedRoadWorld=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;}
 static bool SavedFixedRoad(Vector3 a,Vector3 b,out FixedRoadLeg leg){
  if(fixedRoadWorld!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer)ResetFixedRoads();
  leg=null;return fixedRoadWorld!=null&&fixedRoadLegs.TryGetValue(FixedRoadKey(a,b),out leg);
 }
 static void ForgetFixedRoad(Vector3 a,Vector3 b){var key=FixedRoadKey(a,b);if(fixedRoadLegs.TryGetValue(key,out var old)){fixedRoadCorners-=old.Points.Length;fixedRoadLegs.Remove(key);}}
 static void StoreFixedRoad(Vector3 a,Vector3 b,FixedRoadLeg leg){
  if(fixedRoadWorld!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer)ResetFixedRoads();
  if(fixedRoadWorld==null||leg.Points.Length>1024)return;
  var key=FixedRoadKey(a,b);if(fixedRoadLegs.TryGetValue(key,out var prior))fixedRoadCorners-=prior.Points.Length;else fixedRoadOrder.Enqueue(key);
  fixedRoadLegs[key]=leg;fixedRoadCorners+=leg.Points.Length;
  if(fixedRoadOrder.Count>16384){fixedRoadOrder.Clear();foreach(var present in fixedRoadLegs.Keys)fixedRoadOrder.Enqueue(present);}
  // Evict individual oldest legs, never clear all routes when the map grows.
  while(fixedRoadLegs.Count>8192||fixedRoadCorners>262144){var oldest=fixedRoadOrder.Dequeue();if(fixedRoadLegs.TryGetValue(oldest,out var old)){fixedRoadCorners-=old.Points.Length;fixedRoadLegs.Remove(oldest);}}
 }
 static bool CalculateFixedRoad(Vector3 from,Vector3 to,out FixedRoadLeg leg){
  leg=null;
  bool Failed(){ForgetFixedRoad(from,to);return false;}
  if(!NavMesh.SamplePosition(from,out var a,5,NavMesh.AllAreas)||!NavMesh.SamplePosition(to,out var b,5,NavMesh.AllAreas))return Failed();
  var path=new NavMeshPath();if(!NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,path)||path.status!=NavMeshPathStatus.PathComplete)return Failed();
  var corners=path.corners;if(corners==null||corners.Length==0||corners.Length>1024||Vector3.Distance(corners[corners.Length-1],b.position)>1)return Failed();
  double length=0;for(int i=1;i<corners.Length;i++)length+=Vector3.Distance(corners[i-1],corners[i]);if(!double.IsFinite(length))return Failed();
  leg=new(){End=b.position,Points=corners,Length=length};StoreFixedRoad(from,to,leg);return true;
 }
}
