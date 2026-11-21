using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 [ThreadStatic] static bool checkingGoalSite;
 static readonly Dictionary<IntPtr,Dictionary<IntPtr,string>> nativeGoalSites=new();
 // Rebuild this agenda from actual goals on every decision/load. The research
 // selector consumes those same goals; a locked goal must retain a legal site.
 static Dictionary<IntPtr,string> FutureGoalSites(AFAGFHJONFJ c,MKMOJNAJKJE f,GoalPlan goals,bool lockedOnly=false){
  var reserved=new Dictionary<IntPtr,string>();var barracks=BarracksMissing(c,f);
  var targets=goals.Buildings.Count>0?goals.Buildings:barracks;
  if(targets.Count==0)return reserved;
  foreach(var goal in targets.OrderBy(x=>x.Key,StringComparer.Ordinal)){
   if(!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(goal.Key,out var def)||def==null||def.POLCGKHKLHI||def.OCJNCECECMH==FFDDNPOHLHN.Private)continue;
   if(!def.PLEHAHBBLGL(f.KCKGONGIMDA))continue;
   int missing=goal.Value;
   foreach(var d in c.FGBLEMGGOPO){if(d?.KFCOLLDEJNF!=HNDFFIEJNHI.Public||d.HGNFEMJOPPB==null)continue;
    foreach(var p in d.HGNFEMJOPPB){if(missing<=0)break;if(p?.FAFNFFJAFKH==null||p.FAFNFFJAFKH.Length==0||!p.BFPBNAKIKEO(def)||!CurrentPlot(c,d,p,Footprint(p)))continue;
     bool overlap=false;foreach(var cell in PlotCells(p))if(cell==null||reserved.ContainsKey(cell.Pointer)){overlap=true;break;}if(overlap)continue;
     foreach(var cell in PlotCells(p))reserved[cell.Pointer]=goal.Key;missing--;
    }
   }
  }return reserved;
 }
 static bool ConsumesFutureSite(PEGMABENGBB p,PABIPDOFBFG b,Dictionary<IntPtr,string> sites){if(p?.FAFNFFJAFKH!=null)foreach(var cell in PlotCells(p))if(cell!=null&&sites.TryGetValue(cell.Pointer,out var goal)&&goal!=b.OJPECMNINNA)return true;return false;}
 static void ProtectNativeGoalSite(PEGMABENGBB p,PABIPDOFBFG b,ref bool valid){
  if(!valid||checkingGoalSite||p?.GMIKIMACDBC?.AJJCCPCJPFC==null)return;var c=p.GMIKIMACDBC.AJJCCPCJPFC;if(!Eligible(c,out var f))return;
  var cached=NativePlanState(c,f);var flow=cached.Flow;if(flow.Popular<=0&&b.DPFIFIOKHEI>0||flow.Security<=0&&b.HKBPGJLMFIG>0)return;
  checkingGoalSite=true;try{if(!nativeGoalSites.TryGetValue(c.Pointer,out var sites))nativeGoalSites[c.Pointer]=sites=FutureGoalSites(c,f,cached.Goals);valid=!ConsumesFutureSite(p,b,sites);}finally{checkingGoalSite=false;}
 }
 static void AuditDevelopmentAgenda(AFAGFHJONFJ c,MKMOJNAJKJE f){
  if(!Gate("agenda:"+c.OJPECMNINNA,30))return;var goals=ReadGoals(c);var stages=new List<string>();
  foreach(var goal in goals.Buildings){if(!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(goal.Key,out var b))continue;
   string stage=!b.PLEHAHBBLGL(f.KCKGONGIMDA)?"research":!HasMaterials(f,b)?"materials":!CanBuild(c,f,b,LocalReserve(c))?"prerequisite-or-budget":"build-or-rebuild";stages.Add(goal.Key+":"+stage+":"+goal.Value);
  }
  int pending=0,complete=0;if(c.HFLAGGEFBOO!=null)foreach(var goal in c.HFLAGGEFBOO)if(goal!=null&&!goal.PIHDGCBCMCP){if(goal.NHIPAJDALBA(c))complete++;else pending++;}
  Note($"development-agenda city={c.OJPECMNINNA} completed={complete} unfinished={pending} stages=[{string.Join(",",stages)}] treasuryFloor={SharedReserve(f)}");
 }
}
