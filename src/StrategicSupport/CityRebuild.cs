using System;
using System.Collections.Generic;
using System.Linq;

public sealed partial class StrategicSupport {
 static bool RebuildAllowed(AFAGFHJONFJ c,MKMOJNAJKJE f,PABIPDOFBFG b){
  if(string.IsNullOrEmpty(b.MDOIJIAONJM))return CanBuild(c,f,b,LocalReserve(c));
  if(NativeUpgradePointsBanned(f,b))return false;
  return !b.POLCGKHKLHI&&b.OCJNCECECMH!=FFDDNPOHLHN.Private&&b.NPCHPDNFAME>=0&&b.ODKKMLDAPJL>=0&&b.ADGOBJJMEAO>=0&&b.MLAJLJKFGBD>0&&b.PNPLBNAFMCM>0
   &&(c.ELKFAJKBPKJ(b.MDOIJIAONJM,false,true)?.Count??0)>0&&b.PLEHAHBBLGL(f.KCKGONGIMDA)&&b.MCGEOICKKEE(c,false)&&!b.LOJCCNNJKHF(c,false)&&HasMaterials(f,b)
   &&f.IIBKLLKPNEH>=b.ODKKMLDAPJL&&f.PEGLDBENJMF>=b.ADGOBJJMEAO&&InvestmentPolicy.CanPurchase(f.DNOGPFHMOMO,c.LMPMNLLGOEC.DNOGPFHMOMO,b.NPCHPDNFAME,InvestmentReserve(f),LocalReserve(c));
 }
 static long RenewalCash(AFAGFHJONFJ c,MKMOJNAJKJE f)=>Math.Max(0L,(long)c.LMPMNLLGOEC.DNOGPFHMOMO-LocalReserve(c))+Math.Max(0L,(long)f.DNOGPFHMOMO-ResearchFundsReserve(f));
 static bool IncomeHeadroom(AFAGFHJONFJ c,JNHOOCJJPJO d,IEnumerable<OGABIJECHFG> removed,PABIPDOFBFG b,bool food,int target,bool replacementReady=true){
  double old=0,total=0;
  foreach(var district in c.FGBLEMGGOPO)if(district?.AIMLPNBOIIF!=null)foreach(var x in district.AIMLPNBOIIF)if(x?.GEKKLMPLDHJ!=null)total+=Math.Max(0,food?x.GEKKLMPLDHJ.AJJJJNDNNME:x.GEKKLMPLDHJ.CNCPIILGDEF);
  foreach(var x in removed)old+=Math.Max(0,food?x.GEKKLMPLDHJ.AJJJJNDNNME:x.GEKKLMPLDHJ.CNCPIILGDEF);
  double loss=Math.Max(0,old-(replacementReady?Math.Max(0,food?b.AJJJJNDNNME:b.CNCPIILGDEF):0));if(loss==0)return true;
  double current=food?c.NBAKOCHGMBM(false):c.EMPGDOLPPFI();
  // Use native total income and a conservative proportional multiplier. The
  // post-registration native goal check below is authoritative, not this estimate.
  double estimate=loss*Math.Max(1,Math.Abs(current)/Math.Max(1,total))*2;
  return current-estimate>=Math.Min(current,target);
 }
 sealed class RebuildPlan {internal JNHOOCJJPJO District;internal PEGMABENGBB Place;internal PABIPDOFBFG Building;internal List<OGABIJECHFG> Removed;internal Dictionary<IntPtr,int> Occupancy;internal double Loss;internal int Priority;}
 static bool ProductiveRebuild(RebuildPlan p){
  var before=BuildingEffectsPolicy.Evaluate(DistrictEffects(p.District));var after=DistrictEffects(p.District,p.Removed);
  after.Add(BuildingProfile(p.Building,p.District.OJPECMNINNA,PlotCells(p.Place)));
  return MaintenancePolicy.WorthMoving(before.Value,BuildingEffectsPolicy.Evaluate(after).Value,p.Building.NPCHPDNFAME);
 }

 static bool RebuildParcelCurrent(RebuildPlan p){foreach(var q in PlotCells(p.Place))if(q==null||!p.Occupancy.TryGetValue(q.Pointer,out int id)||q.FCOJFBEHMAG!=id)return false;return true;}
 static bool RebuildGroupSafe(AFAGFHJONFJ c,JNHOOCJJPJO d,List<OGABIJECHFG> removed,PABIPDOFBFG b){
  if(removed.Count==0||removed.Count>4)return false;
  if(!string.IsNullOrEmpty(b.MDOIJIAONJM)&&!removed.Any(x=>x.GEKKLMPLDHJ.OJPECMNINNA==b.MDOIJIAONJM))return false;
  foreach(var x in removed)if(!RenewalProtected(c,d,x,b))return false;
  if(!CivicReplacement(c,removed,b))return false;
  int remaining=0;foreach(var district in c.FGBLEMGGOPO)if(district?.AIMLPNBOIIF!=null)foreach(var x in district.AIMLPNBOIIF)
   if(x?.GEKKLMPLDHJ!=null&&EconomicKind(x.GEKKLMPLDHJ)!=0&&!removed.Any(v=>v.Pointer==x.Pointer))remaining++;
  if(remaining==0&&EconomicKind(b)==0)return false;
  return ReplacementGoalsSafe(c,d,removed,b);
 }
 // Same-size renewal and joined-parcel rebuilding must use the SAME native
 // completed-goal rules, including zero output while replacement is building.
 static bool ReplacementGoalsSafe(AFAGFHJONFJ c,JNHOOCJJPJO d,List<OGABIJECHFG> removed,PABIPDOFBFG b){
  if(c.HFLAGGEFBOO!=null)foreach(var goal in c.HFLAGGEFBOO){if(goal?.CKIJPJFFFOA==null)continue;bool preserveNow=goal.NHIPAJDALBA(c);foreach(var q in goal.CKIJPJFFFOA){if(q==null)continue;
   double loss=0,current=0;bool aggregate=true;
   switch(q.HFBAJIGHOAF){
    case DDBHJKOIIPI.CityCommerce:loss=removed.Sum(v=>v.GEKKLMPLDHJ.CGBGIDCEBGD)-(preserveNow?0:b.CGBGIDCEBGD);current=c.NMDMENNPKDM();break;
    case DDBHJKOIIPI.CityAgriculture:loss=removed.Sum(v=>v.GEKKLMPLDHJ.EGBEEDFCIGH)-(preserveNow?0:b.EGBEEDFCIGH);current=c.NMKKHJEMMFH();break;
    case DDBHJKOIIPI.TwoBlockCommerce:loss=removed.Sum(v=>v.GEKKLMPLDHJ.CGBGIDCEBGD)-(preserveNow?0:b.CGBGIDCEBGD);current=d.IMMLKOLPPHF();break;
    case DDBHJKOIIPI.TwoBlockAgriculture:loss=removed.Sum(v=>v.GEKKLMPLDHJ.EGBEEDFCIGH)-(preserveNow?0:b.EGBEEDFCIGH);current=d.KMDCHCBIAIG();break;
    default:aggregate=false;break;
   }
   if(aggregate&&loss>0&&current-loss<Math.Min(current,q.KLCDGPDHENB))return false;
   if(q.HFBAJIGHOAF==DDBHJKOIIPI.CityMoneyIncome&&!IncomeHeadroom(c,d,removed,b,false,q.KLCDGPDHENB,!preserveNow))return false;
   if(q.HFBAJIGHOAF==DDBHJKOIIPI.CityFoodIncome&&!IncomeHeadroom(c,d,removed,b,true,q.KLCDGPDHENB,!preserveNow))return false;
   if(preserveNow&&!string.IsNullOrEmpty(q.IJNEIHPHMEE)&&(q.HFBAJIGHOAF==DDBHJKOIIPI.BuildingNum||q.HFBAJIGHOAF==DDBHJKOIIPI.BuildingMax)){
    var counted=c.ELKFAJKBPKJ(q.IJNEIHPHMEE,false,true);int lossCount=0;
    if(counted!=null)foreach(var existing in counted)if(removed.Any(x=>x.Pointer==existing.Pointer))lossCount++;
    int required=q.KLCDGPDHENB;
    if(q.HFBAJIGHOAF==DDBHJKOIIPI.BuildingMax){int scale=(int)c.MPGFDBPEHBO(true);if(!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(q.IJNEIHPHMEE,out var def)||def.LJCICFHPMLM==null||scale<0||scale>=def.LJCICFHPMLM.Length)return false;required=def.LJCICFHPMLM[scale];}
    // A newly registered replacement is still under construction and is not
    // allowed to stand in for a completed native goal's existing building.
    if((counted?.Count??0)-lossCount<required)return false;
   }
   if(!string.IsNullOrEmpty(q.IJNEIHPHMEE)&&removed.Any(v=>v.GEKKLMPLDHJ.OJPECMNINNA==q.IJNEIHPHMEE)){
    int before=c.ELKFAJKBPKJ(q.IJNEIHPHMEE,false,true)?.Count??0,after=before-removed.Count(v=>v.GEKKLMPLDHJ.OJPECMNINNA==q.IJNEIHPHMEE)+(b.OJPECMNINNA==q.IJNEIHPHMEE?1:0),needed=q.KLCDGPDHENB;
    if(q.HFBAJIGHOAF==DDBHJKOIIPI.BuildingMax){var def=removed.First(v=>v.GEKKLMPLDHJ.OJPECMNINNA==q.IJNEIHPHMEE).GEKKLMPLDHJ;int scale=(int)c.MPGFDBPEHBO(true);if(def.LJCICFHPMLM==null||scale<0||scale>=def.LJCICFHPMLM.Length)return false;needed=def.LJCICFHPMLM[scale];}
    if(after<Math.Min(before,needed))return false;
   }
  }}return true;
 }
 // Ask the native layout generator on a detached copy. No live tile is made
 // vacant just to search; copied tiles have no scene controllers or UI handles.
 static IEnumerable<RebuildPlan> RebuildPlans(AFAGFHJONFJ c,JNHOOCJJPJO d,PABIPDOFBFG b,bool emptyOnly=false){
  if(d.FAFNFFJAFKH==null||d.FAFNFFJAFKH.Count==0)yield break;
  if(d.PLIFMOIBPME){
   // BBJBFAFMCCB returns immediately for wall districts. Their 1x1 sites
   // are independent sockets, not adjacent rectangles in the ground grid.
   if(!emptyOnly||!DefensiveBuilding(b)||b.MLAJLJKFGBD!=1||b.PNPLBNAFMCM!=1)yield break;
   foreach(var row in d.FAFNFFJAFKH){if(row==null)continue;foreach(var q in row){
    yield return null;
    if(q==null||(int)q.HFBAJIGHOAF!=1||q.FCOJFBEHMAG!=-9999||q.GMIKIMACDBC?.Pointer!=d.Pointer)continue;
    int direction=WallSocketDirection(q.PMGDGAAICCG);if(direction<0)continue;
    var place=new PEGMABENGBB{GMIKIMACDBC=d,PABEMCAGKJJ=1,HAPLIFADGBA=1,NEDMGJPJNKA=q.ICGFBPCAJCI,
     ILFHMILCBLA=(GHIHNCIFMBB)direction,FAFNFFJAFKH=ParcelStorage(new[]{q})};
    if(!place.BFPBNAKIKEO(b))continue;
    yield return new(){District=d,Place=place,Building=b,Removed=new(),Occupancy=new(){{q.Pointer,-9999}}};
   }}yield break;
  }
  var owners=new Dictionary<IntPtr,OGABIJECHFG>();var allowed=new HashSet<IntPtr>();
  foreach(var x in d.AIMLPNBOIIF){if(x==null)continue;bool safe=!emptyOnly&&RenewalProtected(c,d,x,b);var cells=x.JNMLGGLLANN();if(cells==null)continue;foreach(var q in cells)if(q!=null){owners[q.Pointer]=x;if(safe&&!emptyOnly)allowed.Add(q.Pointer);}}
  if(emptyOnly)foreach(var row in d.FAFNFFJAFKH)if(row!=null)foreach(var q in row)if(q!=null&&q.FCOJFBEHMAG==-9999)allowed.Add(q.Pointer);
  if(allowed.Count==0)yield break;
  var copy=new JNHOOCJJPJO{OJPECMNINNA=d.OJPECMNINNA,AJJCCPCJPFC=d.AJJCCPCJPFC,KFCOLLDEJNF=d.KFCOLLDEJNF,
   PLIFMOIBPME=d.PLIFMOIBPME,BJHLIMBCBNN=d.BJHLIMBCBNN,FMANEHEEDKP=d.FMANEHEEDKP,GHECBGMGBBK=d.GHECBGMGBBK,NDOFLDJGDOM=d.NDOFLDJGDOM,
   AIMLPNBOIIF=new(),HGNFEMJOPPB=new(),FAFNFFJAFKH=new()};
  var originals=new Dictionary<IntPtr,FEEDONDCELP>();int cellCount=0;
  foreach(var row in d.FAFNFFJAFKH){if(row==null)yield break;var cloned=new Il2CppSystem.Collections.Generic.List<FEEDONDCELP>();
   foreach(var q in row){if(q==null||++cellCount>4096)yield break;
    var n=new FEEDONDCELP{BJHLIMBCBNN=q.BJHLIMBCBNN,GMIKIMACDBC=copy,HFBAJIGHOAF=q.HFBAJIGHOAF,ICGFBPCAJCI=q.ICGFBPCAJCI,
     PAHKDHJPFKD=q.PAHKDHJPFKD,GHMMBOIKHNC=q.GHMMBOIKHNC,FCOJFBEHMAG=allowed.Contains(q.Pointer)?-9999:q.FCOJFBEHMAG,
     PMGDGAAICCG=q.PMGDGAAICCG,FPOIMBFMEDH=q.FPOIMBFMEDH,FGHAPGBJINB=q.FGHAPGBJINB};
    cloned.Add(n);originals[n.Pointer]=q;
   }copy.FAFNFFJAFKH.Add(cloned);
  }
  // BBJB is the actual native parcel generator. BCJ only refreshes existing
  // building/scene effects and cannot generate an empty copy's parcels.
  // Restrict each probe to the requested rectangle: native greedy 2x2 packing
  // must not swallow every legal 2x1 site needed by a relief building.
  foreach(var shape in new[]{(b.MLAJLJKFGBD,b.PNPLBNAFMCM),(b.PNPLBNAFMCM,b.MLAJLJKFGBD)}.Distinct()){
   int width=shape.Item1,height=shape.Item2;if(width<=0||height<=0)continue;
   for(int y=0;y+height<=copy.FAFNFFJAFKH.Count;y++)for(int x=0;x+width<=copy.FAFNFFJAFKH[y].Count;x++){
    yield return null;bool usable=true;var mask=new HashSet<IntPtr>();
    for(int yy=y;yy<y+height;yy++){if(copy.FAFNFFJAFKH[yy].Count<x+width){usable=false;break;}for(int xx=x;xx<x+width;xx++){
     var cell=copy.FAFNFFJAFKH[yy][xx];var orig=originals[cell.Pointer];
     if(orig.FCOJFBEHMAG!=-9999&&!allowed.Contains(orig.Pointer)){usable=false;break;}mask.Add(cell.Pointer);
    }}if(!usable)continue;
    foreach(var row in copy.FAFNFFJAFKH)foreach(var cell in row)cell.FCOJFBEHMAG=mask.Contains(cell.Pointer)?-9999:0;
    copy.BBJBFAFMCCB(false);
  foreach(var p in copy.HGNFEMJOPPB){if(p==null||!p.BFPBNAKIKEO(b)||p.FAFNFFJAFKH==null)continue;
   var tiles=new List<FEEDONDCELP>();var victims=new List<OGABIJECHFG>();bool valid=true;
   foreach(var q in PlotCells(p)){if(q==null||!originals.TryGetValue(q.Pointer,out var orig)){valid=false;break;}tiles.Add(orig);
    if(owners.TryGetValue(orig.Pointer,out var owner)){if(!allowed.Contains(orig.Pointer)){valid=false;break;}if(!victims.Any(v=>v.Pointer==owner.Pointer))victims.Add(owner);}
    else if(orig.FCOJFBEHMAG!=-9999){valid=false;break;}
   }
   if(!valid||(emptyOnly?victims.Count!=0:!RebuildGroupSafe(c,d,victims,b)))continue;
   var place=new PEGMABENGBB{GMIKIMACDBC=d,PABEMCAGKJJ=p.PABEMCAGKJJ,HAPLIFADGBA=p.HAPLIFADGBA,NEDMGJPJNKA=p.NEDMGJPJNKA,ILFHMILCBLA=p.ILFHMILCBLA,FAFNFFJAFKH=ParcelStorage(tiles)};
   yield return new(){District=d,Place=place,Building=b,Removed=victims,Occupancy=tiles.ToDictionary(q=>q.Pointer,q=>q.FCOJFBEHMAG),Loss=victims.Sum(v=>LocationValue(d,v.GEKKLMPLDHJ,v))};
  }}}
 }
 // Wall map socket sides: east/south/west/north. Native building rotations
 // are Up/Down/Left/Right, so the two encodings must not be cast directly.
 static int WallSocketDirection(int side)=>side switch{1=>3,2=>1,3=>2,4=>0,_=>-1};
 static IEnumerable<bool> RebuildSteps(AFAGFHJONFJ c,MKMOJNAJKJE f,GoalPlan goals){
  string governor=c.LMPMNLLGOEC.IPJHOOJBCKI;var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
  int capacityDeficit=CapacityShortfall(c);RebuildPlan best=null;var reasons=new List<string>();
   var barracksMissing=BarracksMissing(c,f);
   var actualTargets=ReadGoals(c).Buildings;var targets=new Dictionary<string,int>();var priorities=new Dictionary<string,int>();var civic=ReadCivicFlow(c);
   // Locked goals cannot block compensating services, capacity or unlocked replacements.
   foreach(var kv in AEHAGJAKNEK.ACIGFKMDOKE){var def=kv.Value;if(def==null||!def.PLEHAHBBLGL(f.KCKGONGIMDA))continue;
    int priority=actualTargets.ContainsKey(kv.Key)?10:(civic.Popular<=0&&def.DPFIFIOKHEI>0||civic.Security<=0&&def.HKBPGJLMFIG>0)?9:barracksMissing.ContainsKey(kv.Key)?8:EconomicKind(def)>0?1:0;
    if(priority>0){targets[kv.Key]=1;priorities[kv.Key]=priority;}
   }
  foreach(var goal in targets){
   if(!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(goal.Key,out var b)||b==null)continue;
   if(!DayReady("rebuild-rejected:"+c.OJPECMNINNA+":"+b.OJPECMNINNA,3)){reasons.Add(goal.Key+":native-goal-retry");continue;}
   if(!RebuildAllowed(c,f,b)){reasons.Add(goal.Key+":"+(BuildBlock(c,f,b,LocalReserve(c))??"upgrade-prerequisite"));continue;}
   if(RenewalCash(c,f)<b.NPCHPDNFAME+30000L){reasons.Add(goal.Key+":cash-runway");continue;}
   foreach(var d in DistrictSnapshot(c)){
    yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;
    if(d?.KFCOLLDEJNF!=HNDFFIEJNHI.Public)continue;
    using(var plans=RebuildPlans(c,d,b).GetEnumerator())while(true){
     bool more;try{more=plans.MoveNext();}catch(Exception e){reasons.Add(goal.Key+":layout-error-"+e.GetType().Name);Error(e);break;}if(!more)break;
     var p=plans.Current;if(p!=null){p.Priority=priorities[goal.Key];if(p.Priority==1&&!ProductiveRebuild(p))p=null;}
     if(p!=null&&(best==null||p.Priority>best.Priority||p.Priority==best.Priority&&p.Loss<best.Loss))best=p;
     yield return false;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor))yield break;
    }
   }reasons.Add(goal.Key+":no-legal-safe-parcel");
  }
  if(best==null){InvestmentAudit(c,"renewal-blocked details=["+string.Join(";",reasons)+"]");yield break;}
  if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=world||!CurrentCity(c,f,governor)||(best.Priority==10&&!ReadGoals(c).Buildings.ContainsKey(best.Building.OJPECMNINNA))
   ||!RebuildAllowed(c,f,best.Building)||(best.Priority==1&&!ProductiveRebuild(best))||!RebuildGroupSafe(c,best.District,best.Removed,best.Building)
   ||best.Removed.Any(x=>!Present(best.District,x))||!RebuildParcelCurrent(best)||!AIGate("renewal-commit:"+c.OJPECMNINNA,f.IBGCLCAIBOL,1))yield break;
  bool changed=false,goalRejected=false;var oldIds=new HashSet<IntPtr>();foreach(var x in best.District.AIMLPNBOIIF)if(x!=null)oldIds.Add(x.Pointer);
  var completedGoals=new List<HIADFOLCFII>();if(c.HFLAGGEFBOO!=null)foreach(var g in c.HFLAGGEFBOO)if(g!=null&&g.NHIPAJDALBA(c))completedGoals.Add(g);
  List<OGABIJECHFG> CreatedRows(){var rows=new List<OGABIJECHFG>();foreach(var x in best.District.AIMLPNBOIIF)if(x!=null&&!oldIds.Contains(x.Pointer))rows.Add(x);return rows;}
  bool Created()=>CreatedRows().Any(x=>x.GEKKLMPLDHJ?.OJPECMNINNA==best.Building.OJPECMNINNA);
  void Replace(){
   try{
    foreach(var x in best.Removed){best.District.HNIEPCENKGC(x);if(MICGPAMNDKM.DKDKOJNIGAJ!=null&&MICGPAMNDKM.DKDKOJNIGAJ.TryGetValue(x.OJPECMNINNA,out var ctrl)&&ctrl)ctrl.RemoveBuilding(false);}
    if(!best.Place.BFPBNAKIKEO(best.Building))throw new InvalidOperationException("Rebuild parcel changed");
    best.District.PJNMLNMLAKA(best.Building,best.Place);changed=Created();
    if(!changed)throw new InvalidOperationException("Rebuild was not registered");
    if(completedGoals!=null&&completedGoals.Any(g=>!g.NHIPAJDALBA(c))){goalRejected=true;throw new InvalidOperationException("Rebuild invalidated a completed goal");}
   }catch{
    // A native renderer can throw after successful registration. Keep a valid
    // committed building and its payment. Otherwise restore saved originals.
    changed=Created();
    if(changed&&(completedGoals==null||completedGoals.All(g=>g.NHIPAJDALBA(c))))return;
    foreach(var x in CreatedRows()){
     best.District.HNIEPCENKGC(x);if(MICGPAMNDKM.DKDKOJNIGAJ!=null&&MICGPAMNDKM.DKDKOJNIGAJ.TryGetValue(x.OJPECMNINNA,out var ctrl)&&ctrl)ctrl.RemoveBuilding(false);
    }
    foreach(var x in best.Removed)if(!Present(best.District,x)){best.District.FPFEFLILLFN(x,false);if(NEINKOOBJIH.JKGJNLHLJAJ?.Pointer==c.Pointer)Ci_BuildingCtrl.GetOne(x);}
    best.District.BCJALLCJCNL();best.District.BBJBFAFMCCB(false);changed=false;
    if(goalRejected){
     if(completedGoals.Any(g=>!g.NHIPAJDALBA(c)))throw new InvalidOperationException("Rebuild rollback did not restore completed goals");
     DayReady("rebuild-rejected:"+c.OJPECMNINNA+":"+best.Building.OJPECMNINNA,0,true);
     Note($"renewal-rollback city={c.OJPECMNINNA} target={best.Building.OJPECMNINNA} reason=completed-goal-during-construction originalsRestored=true fundsRefunded=true retryDays=3");return;
    }throw;
   }
  }
  bool done=SpendBuilding(c,f,best.Building,Replace,()=>changed);
  if(done){nativeZones.Remove(best.District.Pointer);Note($"renewal-rebuild city={c.OJPECMNINNA} removed=[{string.Join(",",best.Removed.Select(x=>x.GEKKLMPLDHJ.OJPECMNINNA))}] target={best.Building.OJPECMNINNA} nativeConstruction=true");yield return true;}
 }
}
