using System;
using System.Collections.Generic;
using System.Linq;using HarmonyLib;
public sealed partial class StrategicSupport {
 static void HookWorldTiming(Harmony h){
  h.Patch(AccessTools.Method(typeof(GlobalUpdateCtrl),"Update"),prefix:new HarmonyMethod(typeof(StrategicSupport),nameof(BeginWorldTiming)),finalizer:new HarmonyMethod(typeof(StrategicSupport),nameof(EndWorldTiming)));
 }
 static void BeginWorldTiming(out long __state){__state=System.Diagnostics.Stopwatch.GetTimestamp();}
 static void EndWorldTiming(long __state){if(__state>0)RecordPhaseCost("native/world-update-inclusive",(System.Diagnostics.Stopwatch.GetTimestamp()-__state)*1000d/System.Diagnostics.Stopwatch.Frequency);}
 sealed class PhaseCost {
  public long Allocated;public long Count;public double Total,Max;public readonly long[] Histogram=new long[9];
  public void Add(double ms){Count++;Total+=ms;Max=Math.Max(Max,ms);int i=0;while(i<8&&ms>phaseBounds[i])i++;Histogram[i]++;}
  public double P95(){long sum=0,target=(long)Math.Ceiling(Count*.95);for(int i=0;i<9;i++){sum+=Histogram[i];if(sum>=target)return i<8?phaseBounds[i]:Max;}return Max;}
 }
 static readonly double[] phaseBounds={.1,.25,.5,1,2,4,8,16};
 static readonly Dictionary<string,PhaseCost> phaseCosts=new();
 static long lastAllocated;static int lastGc0,lastGc1,lastGc2;
 static void RecordPhaseCost(string label,double ms){
  if(!double.IsFinite(ms)||ms<0)return;
  int colon=label.IndexOf(':'),slash=label.IndexOf('/');
  string key=colon>=0&&slash>colon?label.Substring(0,colon)+label.Substring(slash):label;
  if(!phaseCosts.TryGetValue(key,out var cost)){
   if(phaseCosts.Count>=256)key="other";
   if(!phaseCosts.TryGetValue(key,out cost))phaseCosts[key]=cost=new();
  }cost.Add(ms);
 }
 static void RecordPhaseAllocation(string label,long bytes){
  int colon=label.IndexOf(':'),slash=label.IndexOf('/');string key=colon>=0&&slash>colon?label.Substring(0,colon)+label.Substring(slash):label;
  if(phaseCosts.TryGetValue(key,out var cost))cost.Allocated+=Math.Max(0,bytes);
 }
 static void ReportPhaseCosts(){
  var rows=phaseCosts.OrderByDescending(p=>p.Value.Max).Take(16).Select(p=>$"{p.Key}:calls={p.Value.Count},totalMs={p.Value.Total:F2},maxMs={p.Value.Max:F2},p95UpperMs={p.Value.P95():F2},managedKiB={p.Value.Allocated/1024d:F1}");
  Note("ai-phase-cost ["+string.Join(" | ",rows)+"] nestedNativeTimingsOverlap=true");phaseCosts.Clear();
  long allocated=GC.GetTotalAllocatedBytes(false);int a=GC.CollectionCount(0),b=GC.CollectionCount(1),c=GC.CollectionCount(2);
  if(lastAllocated>0)Note($"ai-managed-memory allocatedMiB={(allocated-lastAllocated)/1048576d:F2} gc0={a-lastGc0} gc1={b-lastGc1} gc2={c-lastGc2} scope=all-CoreCLR-mods-not-Unity-native-heap directBattle={Wa_Controller.m_warInfo!=null} campaignPaused={AEHAGJAKNEK.PAKENCLGOHK}");
  lastAllocated=allocated;lastGc0=a;lastGc1=b;lastGc2=c;
 }
}
