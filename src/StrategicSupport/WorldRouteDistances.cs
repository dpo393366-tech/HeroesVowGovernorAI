using System;using System.Collections.Generic;using System.Linq;using UnityEngine;using UnityEngine.AI;
public sealed partial class StrategicSupport {
 // Sorting quotes are yielded; commands still revalidate their route after
 // intervening captures or roster changes. Never rank by straight-line distance.
 static IEnumerable<string> RankCityRoutes(IEnumerable<AFAGFHJONFJ> candidates,AFAGFHJONFJ fixedCity,MKMOJNAJKJE f,bool inbound,List<(AFAGFHJONFJ City,double Length)> ranked){
  var generation=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
  foreach(var c in candidates.ToArray()){
   yield return "rank-route-city";if(!CurrentFaction(f)||generation!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer)yield break;
   if(c==null||c.Pointer==fixedCity.Pointer)continue;var route=new RoutePlan();
   foreach(var phase in ReliefPathSteps(inbound?c.BMBMKPGEHBH:fixedCity.BMBMKPGEHBH,inbound?fixedCity.BMBMKPGEHBH:c.BMBMKPGEHBH,f,null,false,route)){yield return phase;if(!CurrentFaction(f)||generation!=MICGPAMNDKM.IJPKEMPIFIH?.Pointer)yield break;}
   if(route.Success)ranked.Add((c,route.Length));
  }
  ranked.Sort((a,b)=>a.Length.CompareTo(b.Length));
 }
 static double CompleteMapLeg(Vector3 from,Vector3 to){
  if((from-to).sqrMagnitude<.000001f)return 0;
  if(!NavMesh.SamplePosition(from,out var a,5,NavMesh.AllAreas)||!NavMesh.SamplePosition(to,out var b,5,NavMesh.AllAreas))return double.PositiveInfinity;
  var path=new NavMeshPath();if(!NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,path)||path.status!=NavMeshPathStatus.PathComplete)return double.PositiveInfinity;
  var corners=path.corners;if(corners==null||corners.Length==0||Vector3.Distance(corners[corners.Length-1],b.position)>1)return double.PositiveInfinity;
  double length=0;for(int i=1;i<corners.Length;i++)length+=Vector3.Distance(corners[i-1],corners[i]);return length;
 }
 static double ArmyRemainingRoad(Wo_DeployTeamCtrl t,Vector3 target){
  double length=0;var pos=t.GetPosition();var path=t.m_listMovePos;
  if(path!=null&&path.Count>0){
   // These are the actual remaining navigation corners, not guessed waypoints.
   foreach(var corner in path){length+=Vector3.Distance(pos,corner);pos=corner;}
   // A partial/stale route must not invent a straight shortcut to its endpoint.
   return length+CompleteMapLeg(pos,target);
  }
  if(NEINKOOBJIH.COCMABJFOFH.TryGetValue(t.m_belongPower,out var f)&&f!=null){
   var city=AOHIBKLOAEI.PNKKPMCJIGA(target);string allowed=city!=null&&War(f,city.LMPMNLLGOEC?.LMMNKIOAGLC)?city.OJPECMNINNA:null;
   return ReliefPath(pos,target,f,allowed,true,out _,out double road)?road:double.PositiveInfinity;
  }
  return CompleteMapLeg(pos,target);
 }
 sealed class SupplyQuote {public long Until;public double Eta;public Vector3 Destination;}
 static readonly Dictionary<string,SupplyQuote> supplyQuotes=new();
 static IEnumerable<string> SupplyQuoteSteps(AFAGFHJONFJ c,NJHDEHLHENN war,MKMOJNAJKJE f,string key){
  var route=new RoutePlan();var end=war.GBHGENNMJCK;var name=war.HIENOINGBCN;
  foreach(var phase in ReliefPathSteps(c.BMBMKPGEHBH,end,f,BattleTransitRegion(war,f),false,route)){yield return phase;if(!CurrentFaction(f)||c.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||war.HIENOINGBCN!=name||Vector3.Distance(end,war.GBHGENNMJCK)>.01f)yield break;}
  double speed=Wo_TransportTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL);double eta=route.Success&&speed>0?10+route.Length/speed:double.PositiveInfinity;
  if(supplyQuotes.Count>1024)supplyQuotes.Clear();supplyQuotes[key]=new(){Until=Environment.TickCount64+15000,Eta=eta,Destination=end};
 }
 static double SupplyRank(AFAGFHJONFJ c,NJHDEHLHENN war,string faction){
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(faction,out var f)||f==null||FoodSpare(c,faction)<=0)return double.PositiveInfinity;
  string key=c.Pointer+":"+war.Pointer+":"+war.HIENOINGBCN+":"+faction;
  if(supplyQuotes.TryGetValue(key,out var q)&&q.Until>Environment.TickCount64&&Vector3.Distance(q.Destination,war.GBHGENNMJCK)<.01f)return q.Eta;
  if(schedulerOnline){work.Add("supply-route:"+key,()=>SupplyQuoteSteps(c,war,f,key).GetEnumerator());return double.PositiveInfinity;}
  foreach(var phase in SupplyQuoteSteps(c,war,f,key)){}return supplyQuotes.TryGetValue(key,out q)?q.Eta:double.PositiveInfinity;
 }
}
