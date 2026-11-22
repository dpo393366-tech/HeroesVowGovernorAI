using System;using System.Collections.Generic;using System.Linq;using UnityEngine;
public sealed partial class StrategicSupport {
 static long TimelyCargoFood(AFAGFHJONFJ target,int deadlineDays){
  long total=0;double speed=Wo_TransportTeamCtrl.GetMoveSpeed(target.LMPMNLLGOEC.LMMNKIOAGLC);float seconds=AOHIBKLOAEI.NIOKFCDLGFK();
  var seen=new HashSet<IntPtr>();
  foreach(var t in CityConvoys()){
   if(t.m_toCity!=target.OJPECMNINNA||t.m_belongPower!=target.LMPMNLLGOEC.LMMNKIOAGLC||t.m_food<=0||!seen.Add(t.Pointer)||t.m_listMovePos==null||t.m_listMovePos.Count==0)continue;
   var position=t.GetPosition();double length=0;foreach(var corner in t.m_listMovePos){length+=Vector3.Distance(position,corner);position=corner;}
   length+=CompleteMapLeg(position,target.BMBMKPGEHBH);
   int days=CapacityJourneyDays(length,speed,seconds);if(days>=1&&days<=deadlineDays)total+=t.m_food;
  }return total;
 }
 static IEnumerable<string> CapacityFoodBackfillSteps(AFAGFHJONFJ evacuating,AFAGFHJONFJ target,MKMOJNAJKJE f,long targetFoodGoal,int arrivalDays){
  var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;double arrival=NEINKOOBJIH.MIPFHCKDJOK+arrivalDays;
  bool Valid()=>Ready()&&CurrentFaction(f)&&MICGPAMNDKM.IJPKEMPIFIH?.Pointer==world&&autoSupply?.Value==true&&SafeLogisticsEndpoint(evacuating,f)&&SafeLogisticsEndpoint(target,f)&&CapacityRoom(target,AttackTeams())>0;
  if(!Valid())yield break;
  var ranked=new List<(AFAGFHJONFJ City,double Length)>();
  foreach(var phase in RankCityRoutes(ProtectedCities(f),target,f,true,ranked)){yield return "evacuation-food-"+phase;if(!Valid())yield break;}
  foreach(var candidate in ranked){
   yield return "evacuation-food-donor";if(!Valid())yield break;
   int deadline=Math.Max(0,(int)Math.Floor(arrival-NEINKOOBJIH.MIPFHCKDJOK));if(deadline<=0)yield break;
   long need=Math.Max(0,targetFoodGoal-target.LMPMNLLGOEC.OEGNGFBCHFL-TimelyCargoFood(target,deadline));if(need<=0)yield break;
   var donor=candidate.City;if(donor.Pointer==target.Pointer||donor.Pointer==evacuating.Pointer||!SafeLogisticsEndpoint(donor,f))continue;
   int days=CapacityJourneyDays(candidate.Length,Wo_TransportTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL),AOHIBKLOAEI.NIOKFCDLGFK());if(days<1||days>deadline)continue;
   // Emergency evacuation retains five days inland, fifteen on a border,
   // rather than demanding a normal 30-day surplus while soldiers are lost.
   long reserve=Math.Max(10000,ResidentFoodNeed(donor)*(ActiveFront(donor,f)?15L:5L));
   long spare=Math.Max(0,(long)donor.LMPMNLLGOEC.OEGNGFBCHFL-ReservedFood(donor.OJPECMNINNA)-reserve);
   int amount=(int)Math.Min(int.MaxValue,Math.Min(need,spare));if(amount<=0)continue;
   var route=new RoutePlan();
   if(DispatchCityConvoy(donor,target,f,new List<DHKINGPDKJJ>(),amount,route,days,"capacity-food-backfill"))
    Note($"capacity-food-funded evacuating={evacuating.OJPECMNINNA} donor={donor.OJPECMNINNA} target={target.OJPECMNINNA} food={amount} etaDays={days} troopDeadlineDays={deadline} nativeCargoOnly=true");
  }
 }
}
