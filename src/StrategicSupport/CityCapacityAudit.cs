using System;
using System.Collections.Generic;
public sealed partial class StrategicSupport {
 sealed class CapacityObservation {
  internal AFAGFHJONFJ City;internal string Faction,Building,Target;
  internal int Capacity,Units,Cash,Food,BuildingCapacity;internal long Soldiers;
 }
 static CapacityObservation ObserveCapacity(AFAGFHJONFJ c){
  try {
   if(c?.LMPMNLLGOEC==null)return null;
   var state=new CapacityObservation{City=c,Faction=c.LMPMNLLGOEC.LMMNKIOAGLC,Capacity=c.JNLIOIHLDHH(),Cash=c.LMPMNLLGOEC.DNOGPFHMOMO,Food=c.LMPMNLLGOEC.OEGNGFBCHFL};
   var seen=new HashSet<IntPtr>();if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&seen.Add(a.Pointer)){state.Units++;state.Soldiers+=Math.Max(0,a.FGHFOLKBHDL);}
   return state;
  }catch{return null;}
 }
 static void CapacityChange(CapacityObservation before,string cause){
  try {
   if(before==null)return;var after=ObserveCapacity(before.City);if(after==null)return;
   bool player=after.Faction==NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC;
   bool changed=before.Capacity!=after.Capacity||before.Units!=after.Units||before.Soldiers!=after.Soldiers;
   if(!changed&&before.BuildingCapacity==0)return;
   // Keep losses and capacity/slot changes immediately; coalesce ordinary growth.
   if(before.Capacity==after.Capacity&&before.Units==after.Units&&after.Soldiers>=before.Soldiers&&before.BuildingCapacity==0&&!Gate("capacity-growth:"+before.City.OJPECMNINNA,30))return;
   Note($"capacity-change city={before.City.OJPECMNINNA} faction={after.Faction} cause={cause} oldBuilding={before.Building} newBuilding={before.Target} capacity={before.Capacity}->{after.Capacity} units={before.Units}->{after.Units} soldiers={before.Soldiers}->{after.Soldiers} excess={Math.Max(0,after.Units-after.Capacity)} food={before.Food}->{after.Food} cash={before.Cash}->{after.Cash} aiScope={pumping||nativeBuildScope>0}");
  }catch{}
 }
 static void AuditCapacity(AFAGFHJONFJ c,string phase){
  try {
   var s=ObserveCapacity(c);if(s==null||!Gate("capacity-audit:"+c.OJPECMNINNA+":"+phase,30))return;
   var buildings=new List<string>();if(c.FGBLEMGGOPO!=null)foreach(var d in c.FGBLEMGGOPO)if(d?.KFCOLLDEJNF==HNDFFIEJNHI.Public&&d.AIMLPNBOIIF!=null)foreach(var b in d.AIMLPNBOIIF)if(b?.GEKKLMPLDHJ?.IKELHGAHBID>0)buildings.Add($"{b.GEKKLMPLDHJ.OJPECMNINNA}:cap={b.GEKKLMPLDHJ.IKELHGAHBID},completed={Completed(b)}");
   Note($"capacity-audit city={c.OJPECMNINNA} faction={s.Faction} phase={phase} capacity={s.Capacity} units={s.Units} soldiers={s.Soldiers} excess={Math.Max(0,s.Units-s.Capacity)} food={s.Food} cityCash={s.Cash} buildings=[{string.Join(";",buildings)}]");
  }catch{}
 }
 // The existing timing patch already owns this declaring type's Harmony
 // __state on JFOOAMEIMDO. A nested-call stack avoids a conflicting state type.
 [ThreadStatic] static Stack<CapacityObservation> capacityTicks;
 static void BeforeCapacityTick(AFAGFHJONFJ __instance){var state=ObserveCapacity(__instance);(capacityTicks??=new()).Push(state);
  if(state!=null&&state.Units>state.Capacity&&schedulerOnline&&Ready()&&Eligible(__instance,out var f)){
   var city=__instance;work.Add("capacity-emergency:"+city.OJPECMNINNA,()=>SafePhases(CapacityTransferSteps(city,f)).GetEnumerator());
  }
 }
 static void AfterCapacityTick(){if(capacityTicks?.Count>0)CapacityChange(capacityTicks.Pop(),"native-recruit-tick");}
 static void BeforeBuildingChange(OGABIJECHFG __instance,string __0,out CapacityObservation __state){
  __state=null;try{__state=ObserveCapacity(__instance?.GMIKIMACDBC?.AJJCCPCJPFC);if(__state!=null){__state.Building=__instance.GEKKLMPLDHJ?.OJPECMNINNA;__state.BuildingCapacity=__instance.GEKKLMPLDHJ?.IKELHGAHBID??0;__state.Target=__0;}}catch{}
 }
 static void AfterBuildingChange(CapacityObservation __state){InvalidatePlanningReads();CapacityChange(__state,"building-change");}
 static void BeforeBuildingRemoval(JNHOOCJJPJO __instance,OGABIJECHFG __0,out CapacityObservation __state){
  __state=null;try{__state=ObserveCapacity(__instance?.AJJCCPCJPFC);if(__state!=null){__state.Building=__0?.GEKKLMPLDHJ?.OJPECMNINNA;__state.BuildingCapacity=__0?.GEKKLMPLDHJ?.IKELHGAHBID??0;__state.Target="removed";}}catch{}
 }
 static void AfterBuildingRemoval(CapacityObservation __state){InvalidatePlanningReads();CapacityChange(__state,"building-removal");}
}
