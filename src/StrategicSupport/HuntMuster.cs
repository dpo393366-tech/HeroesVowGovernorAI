using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 sealed class HuntMusterPlan {public string Faction,City;public int Target;public IntPtr TargetPointer;public double Until;}
 sealed class HuntPowerSample {public string Faction;public long Raw,Budget;public int Adjusted;}
 static readonly Dictionary<string,HuntMusterPlan> huntMusters=new();
 static readonly Dictionary<string,HuntPowerSample> huntPowerSamples=new();
 static string MusterKey(string faction,Wo_DeployTeamCtrl target)=>faction+"/"+HuntProbeKey(target);
 static void RememberHuntPower(AFAGFHJONFJ c,Wo_DeployTeamCtrl enemy,Wo_DeployTeamCtrl team){
  long raw=RawArmy(team);int adjusted=team.GetCombatVal();if(raw<=0||adjusted<=0)return;
  if(huntPowerSamples.Count>2048)huntPowerSamples.Clear();
  long budget=raw;if(NEINKOOBJIH.COCMABJFOFH.TryGetValue(c.LMPMNLLGOEC.LMMNKIOAGLC,out var f))budget=AttackStock(c,f,AttackTeams().Where(t=>t.Pointer!=team.Pointer).ToList()).Budget;
  huntPowerSamples[c.OJPECMNINNA+"/"+HuntProbeKey(enemy)]=new(){Faction=c.LMPMNLLGOEC.LMMNKIOAGLC,Raw=raw,Budget=budget,Adjusted=adjusted};
 }
 // Estimates only size a native shipment. Actual commander-adjusted strength
 // is checked again by HuntSteps after the cargo physically arrives.
 static long HuntMusterMissing(AFAGFHJONFJ c,MKMOJNAJKJE f,Wo_DeployTeamCtrl enemy){
  if(!huntPowerSamples.TryGetValue(c.OJPECMNINNA+"/"+HuntProbeKey(enemy),out var sample)||sample.Faction!=f.IBGCLCAIBOL)return 0;
  var teams=AttackTeams();var stock=AttackStock(c,f,teams);
  double needed=Math.Ceiling((double)HuntPower(enemy)*sample.Raw/sample.Adjusted);
  // An actual failed full-pool assembly is authoritative. Do not pretend
  // unused roster power was commanded; credit only subsequent budget growth.
  long usable=Math.Min(stock.Budget,sample.Raw+Math.Max(0,stock.Budget-sample.Budget));
  double extra=Math.Max(0,needed-usable)/Math.Max(.1,1-Retention(c,f));
  long inbound=0;var seen=new HashSet<IntPtr>();foreach(var convoy in CityConvoys())if(convoy.m_belongPower==f.IBGCLCAIBOL&&convoy.m_toCity==c.OJPECMNINNA)
   foreach(var unit in convoy.m_listArmy)if(unit!=null&&seen.Add(unit.Pointer))inbound+=Math.Max(0,unit.GNCKPEPJDGF());
  return (long)Math.Min(int.MaxValue,Math.Max(0,Math.Ceiling(extra)-inbound));
 }
 static bool HuntMusterCity(AFAGFHJONFJ city){
  if(city?.LMPMNLLGOEC==null)return false;double now=NEINKOOBJIH.MIPFHCKDJOK;
  return huntMusters.Values.Any(p=>p.City==city.OJPECMNINNA&&p.Faction==city.LMPMNLLGOEC.LMMNKIOAGLC&&p.Until>=now&&ICGAEMLACJG.HFJIECIBFKB.TryGetValue(p.Target,out var t)&&t.Pointer==p.TargetPointer&&Raider(t)&&t.m_warInfo==null);
 }
 static IEnumerable<string> HuntMusterSteps(MKMOJNAJKJE f,Wo_DeployTeamCtrl enemy){
  if(!CurrentFaction(f)||!Raider(enemy))yield break;
  foreach(var pair in huntMusters.ToArray()){
   var p=pair.Value;
   if(!ICGAEMLACJG.HFJIECIBFKB.TryGetValue(p.Target,out var live)||live.Pointer!=p.TargetPointer||!Raider(live)||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(p.City,out var city)||city.LMPMNLLGOEC?.LMMNKIOAGLC!=p.Faction)huntMusters.Remove(pair.Key);
  }
  string key=MusterKey(f.IBGCLCAIBOL,enemy);double day=NEINKOOBJIH.MIPFHCKDJOK;
  if(enemy.m_warInfo!=null||huntOrders.Values.Any(o=>o.Faction==f.IBGCLCAIBOL&&o.TargetId==enemy.m_id&&LiveTeam(o.Team))){huntMusters.Remove(key);yield break;}
  var cities=f.PJMLPLDPEIM()?.ToArray();if(cities==null||cities.Length<2)yield break;
  // Expired or no-longer-authorized staging does not strand native shipments:
  // their cargo still reaches its friendly city; only the extra attack claim ends.
  if(huntMusters.TryGetValue(key,out var old)&&old.Until<day){huntMusters.Remove(key);Note($"hunt-muster-ended target={enemy.m_id} city={old.City} reason=deadline nativeShipmentsRetained=true");yield break;}
  var destinations=new List<(AFAGFHJONFJ City,int Days)>();
  foreach(var c in cities.Where(c=>c?.LMPMNLLGOEC!=null&&RaiderNear(c,f,enemy)).ToArray()){
   yield return "hunt-muster-destination";if(!CurrentFaction(f)||!Raider(enemy)||enemy.m_warInfo!=null)yield break;
   if(!RaiderNear(c,f,enemy))continue;
   string blocked=!SafeLogisticsEndpoint(c,f)?"unsafe-logistics-endpoint":!AttackPermission(c,f,c.LMPMNLLGOEC.IPJHOOJBCKI)?"governor-policy":CapacityRoom(c,AttackTeams())<=0?"staging-full":HuntMusterMissing(c,f,enemy)<=0?"no-measured-deficit-or-inbound-covered":null;
   if(blocked!=null){if(Gate("hunt-muster-block:"+enemy.m_id+":"+c.OJPECMNINNA,30))Note($"hunt-muster-block target={enemy.m_id} staging={c.OJPECMNINNA} reason={blocked}");continue;}
   if(huntMusters.Any(p=>p.Key!=key&&p.Value.City==c.OJPECMNINNA&&p.Value.Faction==f.IBGCLCAIBOL&&p.Value.Until>=day))continue;
   if(HuntRoute(c,f,enemy,out int days))destinations.Add((c,days));
  }
  var destination=destinations.OrderBy(x=>old!=null&&x.City.OJPECMNINNA==old.City?0:1).ThenBy(x=>x.Days).FirstOrDefault().City;
  if(destination==null)yield break;
  var ranked=new List<(AFAGFHJONFJ City,double Length)>();
  foreach(var phase in RankCityRoutes(cities.ToList(),destination,f,true,ranked)){yield return "hunt-muster-"+phase;if(!CurrentFaction(f)||!Raider(enemy)||enemy.m_warInfo!=null)yield break;}
  foreach(var donor in ranked){
   yield return "hunt-muster-source";if(!CurrentFaction(f)||!Raider(enemy)||enemy.m_warInfo!=null)yield break;
   long need=HuntMusterMissing(destination,f,enemy);int room=CapacityRoom(destination,AttackTeams());if(need<=0||room<=0)break;
   var source=donor.City;if(source.Pointer==destination.Pointer||HuntMusterCity(source)||!SafeLogisticsEndpoint(source,f)||!AttackPermission(source,f,source.LMPMNLLGOEC.IPJHOOJBCKI)||!AttackPermission(destination,f,destination.LMPMNLLGOEC.IPJHOOJBCKI))continue;
   int days=CapacityJourneyDays(donor.Length,Wo_TransportTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL),AOHIBKLOAEI.NIOKFCDLGFK());if(days<1||days>20)continue;
   var before=new HashSet<int>(CityConvoys().Select(t=>t.m_id));
   foreach(var phase in CityShipmentSteps(source,destination,f,room,need,"hunt-muster")){
    yield return phase;if(!CurrentFaction(f)||!Raider(enemy)||enemy.m_warInfo!=null)yield break;
   }
   var created=CityConvoys().Where(t=>!before.Contains(t.m_id)&&t.m_fromCity==source.OJPECMNINNA&&t.m_toCity==destination.OJPECMNINNA&&t.m_listArmy.Count>0).ToList();
   if(created.Count==0)continue;
   if(old==null||old.City!=destination.OJPECMNINNA){old=new(){Faction=f.IBGCLCAIBOL,City=destination.OJPECMNINNA,Target=enemy.m_id,TargetPointer=enemy.Pointer,Until=day+60};huntMusters[key]=old;}
   Note($"hunt-muster-dispatched target={enemy.m_id} source={source.OJPECMNINNA} staging={destination.OJPECMNINNA} convoys=[{string.Join(",",created.Select(t=>t.m_id))}] soldiers={created.Sum(t=>t.m_listArmy.ToArray().Sum(a=>(long)Math.Max(0,a.FGHFOLKBHDL)))} etaDays={days} remainingRawEstimate={HuntMusterMissing(destination,f,enemy)} actualPowerRecheckOnArrival=true");
  }
 }
}
