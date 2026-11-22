using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed partial class StrategicSupport {
 static IEnumerable<FEEDONDCELP> WallSockets(JNHOOCJJPJO d){
  if(d?.FAFNFFJAFKH==null)yield break;
  foreach(var row in d.FAFNFFJAFKH)if(row!=null)foreach(var q in row)
   if(q!=null&&(int)q.HFBAJIGHOAF==1&&q.GMIKIMACDBC?.Pointer==d.Pointer)yield return q;
 }
 static Vector3 WallLocalPosition(AFAGFHJONFJ c,JNHOOCJJPJO d,FEEDONDCELP q)=>q.BJHLIMBCBNN-c.FCIJDABCHEN-d.BJHLIMBCBNN;
 static bool FinitePosition(Vector3 p)=>float.IsFinite(p.x)&&float.IsFinite(p.y)&&float.IsFinite(p.z);
 static FEEDONDCELP ConstructionSocket(AFAGFHJONFJ c,JNHOOCJJPJO d,PEGMABENGBB p){
  if(!d.PLIFMOIBPME||p.PABEMCAGKJJ!=1||p.HAPLIFADGBA!=1)return null;
  var cells=PlotCells(p).ToArray();if(cells.Length!=1)return null;var q=cells[0];
  if(q==null||q.FCOJFBEHMAG!=-9999||WallSocketDirection(q.PMGDGAAICCG)<0||!FinitePosition(WallLocalPosition(c,d,q)))return null;
  return WallSockets(d).Any(t=>t.Pointer==q.Pointer)?q:null;
 }
 static void ShowWallBuilding(AFAGFHJONFJ c,OGABIJECHFG x){
  if(NEINKOOBJIH.JKGJNLHLJAJ?.Pointer!=c.Pointer)return;
  var ctrl=Ci_BuildingCtrl.GetOne(x);if(ctrl&&MICGPAMNDKM.DKDKOJNIGAJ!=null)MICGPAMNDKM.DKDKOJNIGAJ[x.OJPECMNINNA]=ctrl;
 }
 static void RemoveWallBuilding(JNHOOCJJPJO d,OGABIJECHFG x){
  d.HNIEPCENKGC(x);
  if(MICGPAMNDKM.DKDKOJNIGAJ!=null&&MICGPAMNDKM.DKDKOJNIGAJ.TryGetValue(x.OJPECMNINNA,out var ctrl)&&ctrl){ctrl.RemoveBuilding(false);MICGPAMNDKM.DKDKOJNIGAJ.Remove(x.OJPECMNINNA);}
  RefreshDistrictZone(d);
 }
 static void RegisterConstruction(AFAGFHJONFJ c,JNHOOCJJPJO d,PABIPDOFBFG b,PEGMABENGBB p){
  if(!d.PLIFMOIBPME){d.PJNMLNMLAKA(b,p);return;}
  var q=ConstructionSocket(c,d,p);
  if(q==null||b.HFBAJIGHOAF!=MHCEFEODENB.Wall||!DefensiveBuilding(b))throw new InvalidOperationException("Invalid wall construction socket");
  // PJNMLNMLAKA computes a ground-grid centre from row/column indices.
  // Wall sites are independent 3D sockets. Match ClickBuildingQuad's native
  // registration path and retain native cost, duration and progress handling.
  var x=new OGABIJECHFG{GEKKLMPLDHJ=b,BHPDJIDJHPF=b.KAPKDBGFHEG(default),
   BJHLIMBCBNN=WallLocalPosition(c,d,q),EOMMBIBIEJN=(GHIHNCIFMBB)WallSocketDirection(q.PMGDGAAICCG),
   OGFAHJHBCNG=b.OGFAHJHBCNG,ABEAICBDKLD=b.OGFAHJHBCNG>0,EMHNNCLBADE=0,BNMEOOJPOML=b.DONIEGIDACP};
  d.MAOKGGOJNPM(x,false,true);
  bool registered=false;if(c.NBEJCGLJHJA!=null)foreach(var t in c.NBEJCGLJHJA)if(t?.Pointer==x.Pointer){registered=true;break;}
  if(!Present(d,x)||q.FCOJFBEHMAG!=x.OJPECMNINNA||x.JOFCACIPJKK!=q.PMGDGAAICCG||!registered){
   if(Present(d,x))RemoveWallBuilding(d,x);
   throw new InvalidOperationException("Wall registration did not link the socket and native construction list");
  }
  RefreshDistrictZone(d);ShowWallBuilding(c,x);
  Note($"wall-build-registered city={c.OJPECMNINNA} building={b.OJPECMNINNA} instance={x.OJPECMNINNA} socket={q.PAHKDHJPFKD},{q.GHMMBOIKHNC} local={x.BJHLIMBCBNN} progress={x.EMHNNCLBADE}/{x.BNMEOOJPOML} nativeWallList=true");
 }
 // Only the old mod's precise zero-progress ground-grid placement signature
 // is repaired. Completed, private, correctly linked or material-paid buildings
 // are not cancelled. No guessed construction completion is granted.
 static bool BrokenWallRegistration(AFAGFHJONFJ c,JNHOOCJJPJO d,OGABIJECHFG x,List<FEEDONDCELP> sockets){
  var b=x?.GEKKLMPLDHJ;
  if(!PublicBuilding(c,d,x)||!d.PLIFMOIBPME||b==null||!DefensiveBuilding(b)||b.HFBAJIGHOAF!=MHCEFEODENB.Wall
   ||x.EMHNNCLBADE!=0||x.BNMEOOJPOML<=0||x.JOFCACIPJKK!=0||x.MGLFFEANDOI||!string.IsNullOrEmpty(x.LHCJPKGANCK)
   ||b.OCJNCECECMH==FFDDNPOHLHN.Private||b.NPCHPDNFAME<0||b.ODKKMLDAPJL!=0||b.ADGOBJJMEAO!=0||!string.IsNullOrEmpty(b.JADLIKBKOHG))return false;
  if(!FinitePosition(x.BJHLIMBCBNN)||Math.Abs(x.BJHLIMBCBNN.y)>.01f)return false;
  if(sockets.Any(q=>q.FCOJFBEHMAG==x.OJPECMNINNA||(WallLocalPosition(c,d,q)-x.BJHLIMBCBNN).sqrMagnitude<1))return false;
  return sockets.Any(q=>Math.Abs(WallLocalPosition(c,d,q).y)>1
   &&Math.Abs(x.BJHLIMBCBNN.x-(q.PAHKDHJPFKD*20+10))<.01f&&Math.Abs(x.BJHLIMBCBNN.z-(q.GHMMBOIKHNC*20+10))<.01f);
 }
 static IEnumerable<bool> RepairWallRegistrations(AFAGFHJONFJ c,MKMOJNAJKJE f){
  string governor=c.LMPMNLLGOEC.IPJHOOJBCKI;
  foreach(var d in c.FGBLEMGGOPO){
   if(d?.PLIFMOIBPME!=true||d.AIMLPNBOIIF==null)continue;
   var sockets=WallSockets(d).ToList();var broken=new List<OGABIJECHFG>();foreach(var x in d.AIMLPNBOIIF)if(BrokenWallRegistration(c,d,x,sockets))broken.Add(x);
   foreach(var x in broken){
    yield return false;if(!CurrentCity(c,f,governor))yield break;
    if(!Present(d,x)||!BrokenWallRegistration(c,d,x,sockets))continue;
    int refund=x.GEKKLMPLDHJ.NPCHPDNFAME;
    if((long)c.LMPMNLLGOEC.DNOGPFHMOMO+refund>int.MaxValue)continue;
    RemoveWallBuilding(d,x);
    if(Present(d,x))throw new InvalidOperationException("Invalid wall construction could not be removed");
    c.LMPMNLLGOEC.DNOGPFHMOMO+=refund;layoutIntents.Remove(c.OJPECMNINNA);
    Note($"wall-build-recovered city={c.OJPECMNINNA} instance={x.OJPECMNINNA} building={x.GEKKLMPLDHJ.OJPECMNINNA} reason=legacy-ground-grid-zero-progress refund={refund} reconstruction=normal-native-sockets");
    yield return true;
   }
  }
  if(Gate("wall-progress:"+c.OJPECMNINNA,60)){
   int count=0,linked=0;var samples=new List<string>();
   foreach(var d in c.FGBLEMGGOPO)if(d?.PLIFMOIBPME==true&&d.AIMLPNBOIIF!=null){
    var sockets=WallSockets(d).ToList();
    foreach(var x in d.AIMLPNBOIIF)if(x?.GEKKLMPLDHJ?.HFBAJIGHOAF==MHCEFEODENB.Wall&&x.EMHNNCLBADE<x.BNMEOOJPOML){
     count++;bool occupied=sockets.Any(q=>q.FCOJFBEHMAG==x.OJPECMNINNA);if(occupied)linked++;
     if(samples.Count<12)samples.Add($"{x.OJPECMNINNA}:{x.EMHNNCLBADE:F2}/{x.BNMEOOJPOML:F2}:socket={occupied}");
    }
   }
   if(count>0)Note($"wall-build-progress city={c.OJPECMNINNA} projects={count} linked={linked} samples=[{string.Join(";",samples)}]");
  }
 }
}
