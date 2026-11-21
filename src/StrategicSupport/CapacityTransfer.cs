using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public sealed partial class StrategicSupport {
 // Remove only repeated references to the SAME native troop. Never merge units
 // by type/id or alter their soldiers, quality, ownership, or capacity.
 static void NormalizeTroopReferences(AFAGFHJONFJ c){
  var list=c?.LMPMNLLGOEC?.KJPPGIOCDID;if(list==null)return;
  var seen=new HashSet<IntPtr>();int removed=0;
  for(int i=0;i<list.Count;i++){var a=list[i];if(a==null||seen.Add(a.Pointer))continue;list.RemoveAt(i--);removed++;}
  if(removed>0)Note($"troop-reference-repair city={c.OJPECMNINNA} duplicatesRemoved={removed} remaining={list.Count} soldiersNotModified=true");
 }
 static void DismissSupport(Wo_DeployTeamCtrl t){
  if(!t)return;string source=t.m_assembleCity;
  try{t.Dismiss(true,false);}finally{if(source!=null&&MICGPAMNDKM.IJPKEMPIFIH!=null&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(source,out var c))NormalizeTroopReferences(c);}
 }
 sealed class CapacityOrder {public Wo_DeployTeamCtrl Team;public int Id;public string Faction,Source,Target;}
 static readonly Dictionary<IntPtr,CapacityOrder> capacityOrders=new();
 static List<AFAGFHJONFJ> CapacityDestinations(AFAGFHJONFJ source,MKMOJNAJKJE f){
  var result=new List<AFAGFHJONFJ>();foreach(var phase in CapacityDestinationSteps(source,f,result)){}return result;
 }
 static IEnumerable<string> CapacityDestinationSteps(AFAGFHJONFJ source,MKMOJNAJKJE f,List<AFAGFHJONFJ> result){
  var generation=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;var queue=new Queue<AFAGFHJONFJ>();var seen=new HashSet<string>{source.OJPECMNINNA};queue.Enqueue(source);
  while(queue.Count>0&&seen.Count<=256){var city=queue.Dequeue();if(city.FKMKGNPFLFP==null)continue;
   foreach(var n in city.FKMKGNPFLFP.ToArray()){
    yield return "friendly-corridor-edge";if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation){result.Clear();yield break;}
    if(n?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&seen.Add(n.OJPECMNINNA)&&!n.GKLIEHCHDLD()&&!IncomingHostile(n,f)){result.Add(n);queue.Enqueue(n);}
   }
  }
 }
 static HashSet<IntPtr> TeamTroops(Wo_DeployTeamCtrl t){var result=new HashSet<IntPtr>();if(t?.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var a in group.Value)if(a!=null)result.Add(a.Pointer);return result;}
 // Current occupants and ALL inbound regular units reserve slots, including
 // native orders restored from saves. Outbound assembly does not free a target
 // slot early; overlapping home/incoming references are counted just once.
 static int CapacityRoom(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams,Wo_DeployTeamCtrl except=null){
  var units=new HashSet<IntPtr>();if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null)units.Add(a.Pointer);
  foreach(var t in teams)if(t&&t.Pointer!=except?.Pointer&&!t.m_isPrivate&&t.m_warInfo==null&&t.m_belongPower==c.LMPMNLLGOEC.LMMNKIOAGLC&&t.m_targetCity==c.OJPECMNINNA)units.UnionWith(TeamTroops(t));
  AddConvoyUnits(c,units);return Math.Max(0,c.JNLIOIHLDHH()-units.Count);
 }
 static bool DestinationFoodSafe(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams,Wo_DeployTeamCtrl incoming){
  var units=new Dictionary<IntPtr,int>();
  void Add(DHKINGPDKJJ a){if(a!=null)units[a.Pointer]=Math.Max(0,a.FGHFOLKBHDL);}
  if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)Add(a);
  foreach(var t in teams.Concat(new[]{incoming}))if(t&&(t.Pointer==incoming?.Pointer||t.m_targetCity==c.OJPECMNINNA)&&!t.m_isPrivate&&t.m_belongPower==c.LMPMNLLGOEC.LMMNKIOAGLC&&t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var a in group.Value)Add(a);
  foreach(var convoy in CityConvoys())if(convoy.m_toCity==c.OJPECMNINNA&&convoy.m_belongPower==c.LMPMNLLGOEC.LMMNKIOAGLC)foreach(var a in convoy.m_listArmy)Add(a);
  return c.LMPMNLLGOEC.OEGNGFBCHFL+ConvoyFood(c)>=units.Values.Sum(n=>(long)n)*5;
 }
 static string TransferTargetBlock(AFAGFHJONFJ source,AFAGFHJONFJ target,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,Wo_DeployTeamCtrl except=null){
  if(!DistributionCity(target,out var owner)||owner.IBGCLCAIBOL!=f.IBGCLCAIBOL)return "destination-not-managed-friendly";
  if(target.Pointer==source.Pointer||target.GKLIEHCHDLD()||IncomingHostile(target,f))return "destination-unsafe";
  if(!CapacityDestinations(source,f).Any(c=>c.Pointer==target.Pointer))return "no-friendly-corridor";
  if(CapacityRoom(target,teams,except)==0)return "destination-full-or-reserved";
  if(FoodEmergency(target))return "destination-food-shortage";
  return null;
 }
 // Capacity relocation reduces the source's ongoing consumption. Reserve food
 // for the troops actually staying, plus the real journey+10 days for movers.
 // The 30/60-day donor floor for battle supply is not a prerequisite to evacuate
 // an overfull city; it previously deadlocked cities with millions of food.
 static long CapacityFoodFloor(AFAGFHJONFJ source,Wo_DeployTeamCtrl leaving){
  var sent=TeamTroops(leaving);var seen=new HashSet<IntPtr>();long soldiers=0;
  foreach(var a in source.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!sent.Contains(a.Pointer)&&seen.Add(a.Pointer))soldiers+=Math.Max(0,a.FGHFOLKBHDL);
  return Math.Max(10000,soldiers*5);
 }
 // Army path length and army speed use the same world-time units. Personal
 // travel estimates have different speed rules and are not an army-day limit.
 static int CapacityJourneyDays(double length,double speed,double secondsPerDay){
  if(!double.IsFinite(length)||length<0||!double.IsFinite(speed)||speed<=0||!double.IsFinite(secondsPerDay)||secondsPerDay<=0)return -1;
  double days=Math.Ceiling(length/speed/secondsPerDay);
  return double.IsFinite(days)&&days<=int.MaxValue-10?Math.Max(1,(int)days):-1;
 }
 static IEnumerable<string> CapacityTransferSteps(AFAGFHJONFJ source,MKMOJNAJKJE f){
  if(!support.Value||!rebalanceGarrisons.Value||!Ready()||!DistributionCity(source,out _)||!SafeLogisticsEndpoint(source,f)||!AIGate("capacity-transfer:"+source.OJPECMNINNA,f.IBGCLCAIBOL,1))yield break;
  NormalizeTroopReferences(source);var teams=AttackTeams();var reserved=AttackReserved(teams);
  int excess=source.LMPMNLLGOEC.KJPPGIOCDID.ToArray().Count(a=>a!=null&&!reserved.Contains(a.Pointer))-source.JNLIOIHLDHH();if(excess<=0)yield break;
  if(!source.LMPMNLLGOEC.KJPPGIOCDID.ToArray().Any(a=>LogisticsUnit(a,reserved))){if(Gate("capacity-no-cargo:"+source.OJPECMNINNA,15))Note($"capacity-transfer-block city={source.OJPECMNINNA} excess={excess} reason=no-unassigned-movable-units");yield break;}
  var candidates=new List<AFAGFHJONFJ>();foreach(var phase in CapacityDestinationSteps(source,f,candidates))yield return phase;
  candidates.RemoveAll(c=>CapacityRoom(c,AttackTeams())<=0);
  var ranked=new List<(AFAGFHJONFJ City,double Length)>();foreach(var phase in RankCityRoutes(candidates,source,f,false,ranked))yield return phase;
  foreach(var candidate in ranked){var target=candidate.City;
   yield return "capacity-convoy-target";if(!Ready()||!DistributionCity(source,out var owner)||owner.IBGCLCAIBOL!=f.IBGCLCAIBOL||!SafeLogisticsEndpoint(source,f))yield break;
   teams=AttackTeams();reserved=AttackReserved(teams);excess=source.LMPMNLLGOEC.KJPPGIOCDID.ToArray().Count(a=>a!=null&&!reserved.Contains(a.Pointer))-source.JNLIOIHLDHH();if(excess<=0)yield break;
   if(!DistributionCity(target,out owner)||owner.IBGCLCAIBOL!=f.IBGCLCAIBOL)continue;
   foreach(var phase in CityShipmentSteps(source,target,f,excess,long.MaxValue,"capacity-overflow"))yield return phase;
  }
 }
 static bool GuardCapacityTransfer(Wo_DeployTeamCtrl t){
  if(!capacityOrders.TryGetValue(t.Pointer,out var order))return true;
  // Recycled teams and user-redirection are not our orders anymore.
  if(!t||ManualProtected(t)||t.m_id!=order.Id||t.m_belongPower!=order.Faction||t.m_assembleCity!=order.Source||t.m_targetCity!=order.Target||!t.m_isEnterCity||t.m_isPrivate||t.IncludePlayer()) {capacityOrders.Remove(t.Pointer);return true;}
  if(t.m_deployState!=AKOPKFPKJPP.Assemble){capacityOrders.Remove(t.Pointer);return true;}
  string reason="world-or-ownership-changed";
  try {
   if(Ready()&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(order.Source,out var source)&&MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(order.Target,out var target)&&DistributionCity(source,out var f)&&f.IBGCLCAIBOL==order.Faction){
    var teams=AttackTeams();reason=TransferTargetBlock(source,target,f,teams,t);
    if(reason==null&&CapacityRoom(target,teams,t)<TeamTroops(t).Count)reason="destination-capacity-changed";
    if(reason==null&&!DestinationFoodSafe(target,teams,t))reason="destination-food-reserve-changed";
    if(reason==null&&(IncomingHostile(source,f)||source.GKLIEHCHDLD()))reason="source-now-under-attack";
    if(reason==null&&t.IsPassEnemyCity(target.BMBMKPGEHBH))reason="route-changed";
    if(reason==null){var sent=TeamTroops(t);var others=teams.Where(x=>x.Pointer!=t.Pointer).ToList();var reserved=AttackReserved(others);
     long kept=0;int units=0;foreach(var a in source.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!sent.Contains(a.Pointer)&&!reserved.Contains(a.Pointer)){kept+=Math.Max(0,a.GNCKPEPJDGF());units++;}
     if(units<minUnits.Value||kept<BorderThreat(source,f,others))reason="source-defense-changed";
     if(!OffensivePolicy.FoodSafe(source.LMPMNLLGOEC.OEGNGFBCHFL,ReservedFood(source.OJPECMNINNA),CapacityFoodFloor(source,t),t.GetOneDayFood(),Math.Max(10,t.m_targetFoodDay),t.GetFood()))reason="source-provisions-changed";
    }
   }
   if(reason==null)return true;
  }catch(Exception e){Error(e);reason="revalidation-error";}
  capacityOrders.Remove(t.Pointer);DismissSupport(t);Note($"capacity-transfer-cancel team={order.Id} source={order.Source} target={order.Target} reason={reason}");return false;
 }
}

