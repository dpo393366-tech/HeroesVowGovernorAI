using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public sealed partial class StrategicSupport {
 // Native convoys own/save their cargo. Never credit a destination early or
 // store a second cargo ledger that could duplicate soldiers after loading.
 static List<Wo_TransportTeamCtrl> CityConvoys(){
  var result=new List<Wo_TransportTeamCtrl>();
  if(ICGAEMLACJG.KBPAJECPNOH!=null)foreach(var pair in ICGAEMLACJG.KBPAJECPNOH){var t=pair.Value;if(t&&!t.m_isPrivate&&t.m_listArmy!=null)result.Add(t);}
  return result;
 }
 static void AddConvoyUnits(AFAGFHJONFJ city,HashSet<IntPtr> units,Wo_TransportTeamCtrl except=null){
  foreach(var t in CityConvoys())if(t.Pointer!=except?.Pointer&&t.m_belongPower==city.LMPMNLLGOEC.LMMNKIOAGLC&&t.m_toCity==city.OJPECMNINNA)
   foreach(var a in t.m_listArmy)if(a!=null)units.Add(a.Pointer);
 }
 static long ConvoyFood(AFAGFHJONFJ city)=>CityConvoys().Where(t=>t.m_belongPower==city.LMPMNLLGOEC.LMMNKIOAGLC&&t.m_toCity==city.OJPECMNINNA).Sum(t=>(long)Math.Max(0,t.m_food));
 static long TimelyConvoyDefense(AFAGFHJONFJ city,MKMOJNAJKJE f,double deadline,HashSet<IntPtr> counted,out int units){
  units=0;long power=0;
  foreach(var t in CityConvoys()){
   if(t.m_belongPower!=f.IBGCLCAIBOL||t.m_toCity!=city.OJPECMNINNA||t.m_listMovePos==null||t.m_listMovePos.Count==0)continue;
   double length=0;var at=t.GetPosition();foreach(var point in t.m_listMovePos){length+=Vector3.Distance(at,point);at=point;}
   length+=CompleteMapLeg(at,city.BMBMKPGEHBH);
   double speed=Wo_TransportTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL);if(speed<=0||!double.IsFinite(speed)||!double.IsFinite(length)||length/speed>deadline)continue;
   foreach(var a in t.m_listArmy)if(a!=null&&counted.Add(a.Pointer)){power+=Math.Max(0,a.GNCKPEPJDGF());units++;}
  }return power;
 }
 static bool LogisticsUnit(DHKINGPDKJJ a,HashSet<IntPtr> reserved)=>a!=null&&!ManualProtected(a)&&a.FGHFOLKBHDL>0&&!reserved.Contains(a.Pointer)
  &&(a.GDHPLIFJCBN==PMNFKDJNLDJ.None||a.GDHPLIFJCBN==PMNFKDJNLDJ.Train||a.GDHPLIFJCBN==PMNFKDJNLDJ.Supplement)
  &&string.IsNullOrEmpty(a.OLMAJLLIJDM); // Do not cancel a native officer assignment as a side effect.
 static bool SafeLogisticsEndpoint(AFAGFHJONFJ c,MKMOJNAJKJE f)=>c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!c.GKLIEHCHDLD()&&!IncomingHostile(c,f);
 static long ResidentFoodNeed(AFAGFHJONFJ city){
  var troops=new Dictionary<IntPtr,int>();void Add(DHKINGPDKJJ a){if(a!=null)troops[a.Pointer]=Math.Max(0,a.FGHFOLKBHDL);}
  if(city.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in city.LMPMNLLGOEC.KJPPGIOCDID)Add(a);
  foreach(var t in AttackTeams())if(t.m_belongPower==city.LMPMNLLGOEC.LMMNKIOAGLC&&t.m_isEnterCity&&t.m_targetCity==city.OJPECMNINNA&&t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var a in group.Value)Add(a);
  foreach(var t in CityConvoys())if(t.m_belongPower==city.LMPMNLLGOEC.LMMNKIOAGLC&&t.m_toCity==city.OJPECMNINNA)foreach(var a in t.m_listArmy)Add(a);
  return troops.Values.Sum(x=>(long)x);
 }
 static IEnumerable<string> CityFoodBalanceSteps(MKMOJNAJKJE f){
  if(!autoSupply.Value||!CurrentFaction(f))yield break;
  var cities=ProtectedCities(f);var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
  bool Valid()=>Ready()&&CurrentFaction(f)&&MICGPAMNDKM.IJPKEMPIFIH?.Pointer==world&&autoSupply.Value;
  foreach(var target in cities){
   yield return "city-food-target";if(!Valid())yield break;if(!SafeLogisticsEndpoint(target,f))continue;
   long Need(int days){long residents=ResidentFoodNeed(target);return Math.Max(0,Math.Max(CityFoodFloor(target,f.IBGCLCAIBOL),residents*30)+residents*days-target.LMPMNLLGOEC.OEGNGFBCHFL-ConvoyFood(target));}
   if(Need(0)<=0)continue;
   var ranked=new List<(AFAGFHJONFJ City,double Length)>();foreach(var phase in RankCityRoutes(cities,target,f,true,ranked)){yield return "food-"+phase;if(!Valid())yield break;}
   foreach(var candidate in ranked){
    yield return "city-food-source";if(!Valid())yield break;var source=candidate.City;
    if(Need(0)<=0)break;if(source.Pointer==target.Pointer||!SafeLogisticsEndpoint(source,f)||!SafeLogisticsEndpoint(target,f))continue;
    int days=CapacityJourneyDays(candidate.Length,Wo_TransportTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL),AOHIBKLOAEI.NIOKFCDLGFK());if(days<1)continue;
    long spare=Math.Max(0,(long)source.LMPMNLLGOEC.OEGNGFBCHFL-ReservedFood(source.OJPECMNINNA)-Math.Max(CityFoodFloor(source,f.IBGCLCAIBOL),ResidentFoodNeed(source)*30));
    int amount=(int)Math.Min(int.MaxValue,Math.Min(spare,Need(days)));if(amount<=0)continue;
    var route=new RoutePlan();if(DispatchCityConvoy(source,target,f,new List<DHKINGPDKJJ>(),amount,route,days,"food-balance"))Note($"city-food-balanced source={source.OJPECMNINNA} target={target.OJPECMNINNA} sourceGovernor={source.LMPMNLLGOEC.IPJHOOJBCKI} targetGovernor={target.LMPMNLLGOEC.IPJHOOJBCKI} food={amount} etaDays={days} factionWide=true");
   }
  }
 }
 static IEnumerable<string> CityShipmentSteps(AFAGFHJONFJ source,AFAGFHJONFJ target,MKMOJNAJKJE f,int maximumUnits,long wantedPower,string reason){
  var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
  bool Valid()=>Ready()&&CurrentFaction(f)&&MICGPAMNDKM.IJPKEMPIFIH?.Pointer==world&&source.Pointer!=target.Pointer&&SafeLogisticsEndpoint(source,f)&&SafeLogisticsEndpoint(target,f);
  if(!Valid()||maximumUnits<=0)yield break;
  var route=new RoutePlan();foreach(var phase in ReliefPathSteps(source.BMBMKPGEHBH,target.BMBMKPGEHBH,f,null,false,route)){yield return "logistics-"+phase;if(!Valid())yield break;}
  if(!route.Success){Note($"city-logistics-block source={source.OJPECMNINNA} target={target.OJPECMNINNA} reason=route:{route.Failure}");yield break;}
  double speed=Wo_TransportTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL);int days=CapacityJourneyDays(route.Length,speed,AOHIBKLOAEI.NIOKFCDLGFK());if(days<1)yield break;
  // Fresh admission after the yielded route search; incoming native convoys
  // and deployed armies reserve the same destination slots.
  var teams=AttackTeams();var reserved=AttackReserved(teams);var row=source.LMPMNLLGOEC;
  var home=row.KJPPGIOCDID.ToArray().AsEnumerable().Where(a=>a!=null&&!reserved.Contains(a.Pointer)).GroupBy(a=>a.Pointer).Select(g=>g.First()).ToList();
  long homePower=home.Sum(a=>(long)Math.Max(0,a.GNCKPEPJDGF()));
  bool overflow=reason=="capacity-overflow";
  long keepPower=overflow?0:BorderThreat(source,f,teams);
  int keepUnits=overflow?Math.Max(0,source.JNLIOIHLDHH()):(OccupationVulnerable(source)?Math.Max(2,minUnits.Value):minUnits.Value);
  int limit=Math.Min(maximumUnits,CapacityRoom(target,teams));
  if(reason=="capacity-overflow")limit=Math.Min(limit,Math.Max(0,home.Count-source.JNLIOIHLDHH()));
  else wantedPower=Math.Min(wantedPower,HomeProtectionNeed(target,f,teams));
  var selected=new List<DHKINGPDKJJ>();long movedPower=0;
  foreach(var a in home.Where(a=>LogisticsUnit(a,reserved)).OrderBy(a=>overflow?(long)a.GNCKPEPJDGF():-(long)a.GNCKPEPJDGF())){
   if(selected.Count>=limit||reason!="capacity-overflow"&&movedPower>=wantedPower)break;
   long value=Math.Max(0,a.GNCKPEPJDGF());
   if(home.Count-selected.Count-1<keepUnits||homePower-movedPower-value<keepPower)continue;
   selected.Add(a);movedPower+=value;
  }
  if(selected.Count==0){if(Gate("logistics-block:"+source.OJPECMNINNA+":"+target.OJPECMNINNA,15))Note($"city-logistics-block source={source.OJPECMNINNA} target={target.OJPECMNINNA} reason=no-unassigned-surplus units={home.Count} eligible={home.Count(a=>LogisticsUnit(a,reserved))} homePower={homePower} room={limit} keepPower={keepPower} overflow={overflow}");yield break;}
  long movingSoldiers=selected.Sum(a=>(long)Math.Max(0,a.FGHFOLKBHDL));
  long keptSoldiers=home.Sum(a=>(long)Math.Max(0,a.FGHFOLKBHDL))-movingSoldiers;
  var targetUnits=new Dictionary<IntPtr,int>();
  foreach(var a in target.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null)targetUnits[a.Pointer]=Math.Max(0,a.FGHFOLKBHDL);
  foreach(var t in teams)if(t.m_belongPower==f.IBGCLCAIBOL&&t.m_targetCity==target.OJPECMNINNA&&t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)foreach(var a in group.Value)if(a!=null)targetUnits[a.Pointer]=Math.Max(0,a.FGHFOLKBHDL);
  foreach(var t in CityConvoys())if(t.m_belongPower==f.IBGCLCAIBOL&&t.m_toCity==target.OJPECMNINNA)foreach(var a in t.m_listArmy)if(a!=null)targetUnits[a.Pointer]=Math.Max(0,a.FGHFOLKBHDL);
  long destinationSoldiers=targetUnits.Values.Sum(n=>(long)n);
  // Supply existing residents while the cargo travels plus five days for the
  // resulting garrison. Cargo is carried stock, not expedition upkeep.
  long required=Math.Max(0,(destinationSoldiers+movingSoldiers)*5+destinationSoldiers*days-target.LMPMNLLGOEC.OEGNGFBCHFL-ConvoyFood(target));
  long spare=Math.Max(0,(long)row.OEGNGFBCHFL-ReservedFood(source.OJPECMNINNA)-Math.Max(10000,keptSoldiers*5));
  if(required>spare||required>int.MaxValue){Note($"city-logistics-block source={source.OJPECMNINNA} target={target.OJPECMNINNA} reason=destination-provisions need={required} spare={spare}");yield break;}
  if(!Valid()||CapacityRoom(target,AttackTeams())<selected.Count)yield break;
  DispatchCityConvoy(source,target,f,selected,(int)required,route,days,reason);
 }
 static bool DispatchCityConvoy(AFAGFHJONFJ source,AFAGFHJONFJ target,MKMOJNAJKJE f,List<DHKINGPDKJJ> selected,int food,RoutePlan route,int days,string reason){
  if(!ICGAEMLACJG.FKKCPDEFODO.TryGetValue(target.OJPECMNINNA,out var cityCtrl)||!cityCtrl||ICGAEMLACJG.KBPAJECPNOH==null)return false;
  var row=source.LMPMNLLGOEC;var original=row.KJPPGIOCDID.ToArray();int oldFood=row.OEGNGFBCHFL,oldMoney=row.DNOGPFHMOMO;
  var states=selected.Select(a=>(Unit:a,Job:a.GDHPLIFJCBN,Worker:a.OLMAJLLIJDM)).ToList();Wo_TransportTeamCtrl convoy=null;bool touched=false,committed=false;
  try{
   if(!SafeLogisticsEndpoint(source,f)||!SafeLogisticsEndpoint(target,f)||food<0||food>oldFood||selected.Count>CapacityRoom(target,AttackTeams())||selected.Any(a=>!original.Contains(a)||!LogisticsUnit(a,AttackReserved(AttackTeams()))))return false;
   if(!ReliefPath(source.BMBMKPGEHBH,target.BMBMKPGEHBH,f,null,false,out var freshPoints,out double freshLength))return false;
   route.Points=freshPoints;route.Length=freshLength;
   convoy=Wo_TransportTeamCtrl.GetOne(false,false);if(!convoy)return false;
   convoy.m_belongPower=f.IBGCLCAIBOL;convoy.m_fromCity=source.OJPECMNINNA;convoy.m_toCity=target.OJPECMNINNA;convoy.m_money=0;convoy.m_food=food;
   convoy.m_listArmy.Clear();foreach(var a in selected)convoy.m_listArmy.Add(a);
   convoy.JBPGMCBOCFO(source.BMBMKPGEHBH);convoy.MoveToCity(cityCtrl,true,false);
   if(convoy.m_toCity!=target.OJPECMNINNA||convoy.m_listMovePos==null)throw new InvalidOperationException("Native convoy rejected destination");
   convoy.GAANFLKOMKL?.Clear();convoy.m_listMovePos.Clear();foreach(var p in route.Points)convoy.m_listMovePos.Add(p);convoy.IAFGLIMLMGA=target.BMBMKPGEHBH;
   if(convoy.m_listMovePos.Count==0)throw new InvalidOperationException("Empty convoy path");
   touched=true;convoy.RemoveFromCity();
   if(selected.Any(a=>row.KJPPGIOCDID.Contains(a))||row.OEGNGFBCHFL!=oldFood-food||convoy.m_food!=food)throw new InvalidOperationException("Native convoy cargo debit mismatch");
   ICGAEMLACJG.OKLHGICPMHB(convoy);if(!ICGAEMLACJG.KBPAJECPNOH.TryGetValue(convoy.m_id,out var registered)||registered.Pointer!=convoy.Pointer)throw new InvalidOperationException("Convoy registration failed");
   convoy.gameObject.SetActive(true);committed=true;
   Note($"city-logistics-order convoy={convoy.m_id} source={source.OJPECMNINNA} target={target.OJPECMNINNA} units={selected.Count} soldiers={selected.Sum(a=>(long)a.FGHFOLKBHDL)} foodCargo={food} etaDays={days} reason={reason} method=native-transport officers=independent-city-move");
   ScheduleStaffing(f);return true;
  }catch(Exception e){Error(e);return false;}
  finally{if(!committed&&convoy){
   if(ICGAEMLACJG.KBPAJECPNOH.TryGetValue(convoy.m_id,out var entry)&&entry.Pointer==convoy.Pointer)ICGAEMLACJG.KBPAJECPNOH.Remove(convoy.m_id);
   if(touched){row.KJPPGIOCDID.Clear();foreach(var a in original)row.KJPPGIOCDID.Add(a);row.OEGNGFBCHFL=oldFood;row.DNOGPFHMOMO=oldMoney;foreach(var s in states){s.Unit.GDHPLIFJCBN=s.Job;s.Unit.OLMAJLLIJDM=s.Worker;}}
   convoy.m_listArmy.Clear();convoy.m_food=0;convoy.m_money=0;convoy.Recycle();
  }}
 }
}

