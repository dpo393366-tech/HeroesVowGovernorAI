using System;
using System.Collections.Generic;
using HarmonyLib;

public sealed partial class StrategicSupport {
 // Native PEGM constructor allocates four entries, and the native generator
 // writes 2x2 sites into recycled objects without resizing this array.
 static FEEDONDCELP[] ParcelStorage(IReadOnlyList<FEEDONDCELP> cells){
  var buffer=new FEEDONDCELP[Math.Max(4,cells.Count)];
  for(int i=0;i<cells.Count;i++)buffer[i]=cells[i];return buffer;
 }
 static void RepairParcelStorage(PEGMABENGBB __result){
  if(__result==null)return;var old=__result.FAFNFFJAFKH;
  if(old!=null&&old.Length>=4)return;
  var buffer=new FEEDONDCELP[4];if(old!=null)for(int i=0;i<old.Length;i++)buffer[i]=old[i];
  __result.FAFNFFJAFKH=buffer;
  Note("parcel-pool-repaired previousLength="+(old?.Length??0)+" required=4");
 }
 static void HookParcelPool(Harmony h)=>h.Patch(AccessTools.Method(typeof(PEGMABENGBB),"OCELGGJBGEN"),postfix:new HarmonyMethod(typeof(StrategicSupport),nameof(RepairParcelStorage)));
}
