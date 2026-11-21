using System;using System.Collections.Generic;using System.Linq;using UnityEngine;
public sealed partial class StrategicSupport {
 // Lazy exact-road ordering: a distant city's path need not be computed when
 // even its geometric LOWER BOUND exceeds a validated nearer road. The bound
 // is never an ETA or a selection distance. Allow both native endpoints to
 // snap by five units, as ReliefPathCoreSteps does. Curved roads still win or
 // lose solely by their full native path length, including intermediate roads.
 sealed class NearestReturnRoads {
  readonly Vector3 from;readonly MKMOJNAJKJE faction;readonly IntPtr? world;readonly string stamp;
  readonly List<(AFAGFHJONFJ City,double Bound)> pending;
  readonly List<(AFAGFHJONFJ City,double Length)> ready=new();int next;bool invalid;
  public AFAGFHJONFJ City;public double Length;public int RoutesChecked,Pruned;
  public NearestReturnRoads(Vector3 origin,MKMOJNAJKJE f){
   from=origin;faction=f;world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;stamp=RoadPolicyStamp(f);
   pending=ReturnCities(f).Select(c=>(City:c,Bound:Math.Max(0d,Vector3.Distance(origin,c.BMBMKPGEHBH)-10.01)))
    .OrderBy(x=>x.Bound).ThenBy(x=>x.City.OJPECMNINNA,StringComparer.Ordinal).ToList();
  }
  bool Current()=>!invalid&&world==MICGPAMNDKM.IJPKEMPIFIH?.Pointer&&RoadPolicyStamp(faction)==stamp;
  public IEnumerable<string> Next(Func<bool> valid,Func<AFAGFHJONFJ,bool> eligible){
   City=null;Length=double.PositiveInfinity;
   if(!valid()||!Current()){invalid=true;yield break;}
   while(true){
    ready.RemoveAll(x=>!eligible(x.City));
    ready.Sort((a,b)=>{int order=a.Length.CompareTo(b.Length);return order!=0?order:StringComparer.Ordinal.Compare(a.City.OJPECMNINNA,b.City.OJPECMNINNA);});
    if(ready.Count>0&&(next>=pending.Count||pending[next].Bound>ready[0].Length+.01)){
     if(!valid()||!Current()){invalid=true;yield break;}
     City=ready[0].City;Length=ready[0].Length;ready.RemoveAt(0);Pruned=pending.Count-next;yield break;
    }
    if(next>=pending.Count)yield break;
    var candidate=pending[next++].City;
    if(!eligible(candidate))continue;
    yield return "nearest-return-city";
    if(!valid()||!Current()){invalid=true;yield break;}
    if(!eligible(candidate))continue;
    var road=new RoutePlan();RoutesChecked++;
    foreach(var phase in ReliefPathSteps(from,candidate.BMBMKPGEHBH,faction,null,true,road)){
     yield return phase;if(!valid()||world!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer){invalid=true;yield break;}
    }
    if(!Current()){invalid=true;yield break;}
    if(road.Success&&eligible(candidate))ready.Add((candidate,road.Length));
   }
  }
 }
}
