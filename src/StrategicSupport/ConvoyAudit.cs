using System;using System.Collections.Generic;using System.Linq;using HarmonyLib;
public sealed partial class StrategicSupport {
 // Observational only: never redirect, dismiss, credit or debit native cargo.
 // FinishMove credits m_toCity; Dismiss credits m_fromCity in the installed game.
 sealed class CargoCitySnapshot {
  public AFAGFHJONFJ City;public int Food,Money;public HashSet<IntPtr> Units=new();
 }
 sealed class CargoSnapshot {
  public int Id,Food,Money;public IntPtr Pointer;public string From,To,Faction,Operation;public bool Private;
  public long Soldiers;public IntPtr[] Units;public CargoCitySnapshot Source,Destination;
 }
 static readonly Dictionary<IntPtr,(int Id,string Outcome)> cargoOutcomes=new();
 static CargoCitySnapshot CargoCity(string id){
  if(string.IsNullOrEmpty(id)||MICGPAMNDKM.IJPKEMPIFIH==null||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(id,out var c)||c?.LMPMNLLGOEC==null)return null;
  var s=new CargoCitySnapshot{City=c,Food=c.LMPMNLLGOEC.OEGNGFBCHFL,Money=c.LMPMNLLGOEC.DNOGPFHMOMO};
  if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var u in c.LMPMNLLGOEC.KJPPGIOCDID)if(u!=null)s.Units.Add(u.Pointer);return s;
 }
 static CargoSnapshot CaptureCargo(Wo_TransportTeamCtrl t,string operation){
  if(t==null)return null;
  var units=t.m_listArmy?.ToArray().Where(u=>u!=null).ToArray()??Array.Empty<DHKINGPDKJJ>();
  return new(){Id=t.m_id,Pointer=t.Pointer,From=t.m_fromCity,To=t.m_toCity,Faction=t.m_belongPower,Private=t.m_isPrivate,Food=t.m_food,Money=t.m_money,Units=units.Select(u=>u.Pointer).Distinct().ToArray(),Soldiers=units.Sum(u=>(long)Math.Max(0,u.FGHFOLKBHDL)),Source=CargoCity(t.m_fromCity),Destination=CargoCity(t.m_toCity),Operation=operation};
 }
 static string CargoDelta(CargoCitySnapshot before,CargoSnapshot cargo,out bool credited){
  credited=false;if(before==null)return "missing-city";
  var after=CargoCity(before.City.OJPECMNINNA);if(after==null||after.City.Pointer!=before.City.Pointer)return "city-replaced";
  long food=(long)after.Food-before.Food,money=(long)after.Money-before.Money;
  int added=cargo.Units.Count(p=>!before.Units.Contains(p)&&after.Units.Contains(p));
  bool nonempty=cargo.Food>0||cargo.Money>0||cargo.Units.Length>0;
  credited=nonempty&&food==cargo.Food&&money==cargo.Money&&(!cargo.Private&&added==cargo.Units.Length||cargo.Units.Length==0);
  return $"foodDelta:{food},moneyDelta:{money},publicUnitsAdded:{added}/{cargo.Units.Length},fullCreditVerified:{credited}";
 }
 static void FinishCargoAudit(CargoSnapshot s,Exception error){
  if(s==null)return;
  string source=CargoDelta(s.Source,s,out bool returned),destination=CargoDelta(s.Destination,s,out bool delivered);
  string outcome=error!=null?"native-error":s.Operation=="departure"?"departed":delivered?"destination-credited":returned?"source-refunded":"credit-not-verified";
  if(cargoOutcomes.Count>2048)cargoOutcomes.Clear();
  cargoOutcomes[s.Pointer]=(s.Id,outcome);
  Note($"convoy-audit operation={s.Operation} convoy={s.Id} faction={s.Faction} source={s.From} destination={s.To} private={s.Private} soldiers={s.Soldiers} food={s.Food} money={s.Money} outcome={outcome} sourceDelta=[{source}] destinationDelta=[{destination}] error={error?.GetType().Name??"none"}");
 }
 static void HookConvoyAudit(Harmony h){
  HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
  h.Patch(AccessTools.Method(typeof(Wo_TransportTeamCtrl),"RemoveFromCity"),prefix:H(nameof(BeginCargoDeparture)),finalizer:H(nameof(EndCargoDeparture)));
  h.Patch(AccessTools.Method(typeof(Wo_TransportTeamCtrl),"FinishMove"),prefix:H(nameof(BeginCargoArrival)),finalizer:H(nameof(EndCargoArrival)));
  h.Patch(AccessTools.Method(typeof(Wo_TransportTeamCtrl),"Dismiss"),prefix:H(nameof(BeginCargoDismissal)),finalizer:H(nameof(EndCargoDismissal)));
  h.Patch(AccessTools.Method(typeof(Wo_TransportTeamCtrl),"Recycle"),prefix:H(nameof(RecycleCargoAudit)));
 }
 static void RecycleCargoAudit(Wo_TransportTeamCtrl __instance){try{
  var t=__instance;if(t==null)return;
  string outcome=cargoOutcomes.TryGetValue(t.Pointer,out var old)&&old.Id==t.m_id?old.Outcome:"no-prior-event";
  Note($"convoy-recycle convoy={t.m_id} source={t.m_fromCity} destination={t.m_toCity} lastOutcome={outcome} food={t.m_food} units={t.m_listArmy?.Count??0}");
  cargoOutcomes.Remove(t.Pointer);
 }catch(Exception e){Error(e);}}
 static void BeginCargoDeparture(Wo_TransportTeamCtrl __instance,out CargoSnapshot __state){__state=null;try{__state=CaptureCargo(__instance,"departure");}catch(Exception e){Error(e);}}
 static void EndCargoDeparture(CargoSnapshot __state,Exception __exception){try{FinishCargoAudit(__state,__exception);}catch(Exception e){Error(e);}}
 static void BeginCargoArrival(Wo_TransportTeamCtrl __instance,out CargoSnapshot __state){__state=null;try{__state=CaptureCargo(__instance,"arrival");}catch(Exception e){Error(e);}}
 static void EndCargoArrival(CargoSnapshot __state,Exception __exception){try{FinishCargoAudit(__state,__exception);}catch(Exception e){Error(e);}}
 static void BeginCargoDismissal(Wo_TransportTeamCtrl __instance,out CargoSnapshot __state){__state=null;try{__state=CaptureCargo(__instance,"dismiss");Note($"convoy-dismiss-observed convoy={__instance.m_id} source={__instance.m_fromCity} destination={__instance.m_toCity} managedCaller={Environment.StackTrace.Replace('\r',' ').Replace('\n',' ')}");}catch(Exception e){Error(e);}}
 static void EndCargoDismissal(CargoSnapshot __state,Exception __exception){try{FinishCargoAudit(__state,__exception);}catch(Exception e){Error(e);}}
}
