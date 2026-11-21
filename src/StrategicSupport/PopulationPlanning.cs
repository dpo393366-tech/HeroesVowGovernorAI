using System;using System.Collections.Generic;
public sealed partial class StrategicSupport {
 sealed class PopulationModel {
  public double Native,Raw,Factor,Economic,Added,Cap;public bool Classic,Full;
  public double Gain(double economy,double addition){
   if(Full)return 0;
   double delta=economy*.04+(Classic?0:(Math.Min(Cap,Added+addition)-Math.Min(Cap,Added))*Factor);
   if(delta==0)return 0;
   // Anchor on the game's actual pre-disaster integer result. Its unknown
   // fractional remainder yields an interval, not an invented growth multiplier.
   if(!double.IsFinite(Raw)||Math.Abs(Raw)<.000001)return delta<0?double.NegativeInfinity:0;
   double lo=(Native<0?Native-1:Native)/Raw,hi=(Native<0?Native:Native+1)/Raw;
   return Math.Floor(Math.Min(lo*delta,hi*delta));
  }
 }
 static long populationSlice=-1;static readonly System.Collections.Generic.Dictionary<IntPtr,PopulationModel> slicePopulation=new();
 static PopulationModel PopulationSnapshot(AFAGFHJONFJ c){
  if(!WorkQueue.InStep)return PopulationSnapshotFresh(c);
  if(populationSlice!=WorkQueue.StepToken){slicePopulation.Clear();populationSlice=WorkQueue.StepToken;}
  if(!slicePopulation.TryGetValue(c.Pointer,out var model))slicePopulation[c.Pointer]=model=PopulationSnapshotFresh(c);return model;
 }
 static PopulationModel PopulationSnapshotFresh(AFAGFHJONFJ c){
  var row=c.LMPMNLLGOEC;int scale=(int)c.MPGFDBPEHBO(true);double tax=0;
  if(row.LGACDLFDIJG!=null&&row.LGACDLFDIJG.TryGetValue(row.LMMNKIOAGLC,out float rate))tax=rate;
  double factor=.5+tax*.001+(int)row.KCANKJJJNAC*.0005;
  bool classic=SettingCtrl.m_isZhengShi;double added=0;
  if(!classic&&c.FGBLEMGGOPO!=null)foreach(var d in c.FGBLEMGGOPO)if(d?.KFCOLLDEJNF==HNDFFIEJNHI.Public&&d.AIMLPNBOIIF!=null)foreach(var x in d.AIMLPNBOIIF)if(x?.GEKKLMPLDHJ!=null&&Completed(x))added+=x.GEKKLMPLDHJ.DHLNIKKLNKJ;
  var caps=EBNFFALGBPA.AIEJCKPDMLE;double cap=caps!=null&&scale>=0&&scale<caps.Length?caps[scale]:0;
  var limits=EBNFFALGBPA.LNBFFMONMCN;bool full=limits!=null&&scale>=0&&scale<limits.Length&&row.PNHICLAGHGD>=limits[scale];
  double economic=c.NMDMENNPKDM()+c.NMKKHJEMMFH();
  return new(){Native=c.KCFNACGFBFF(),Factor=factor,Classic=classic,Added=added,Cap=cap,Full=full,Economic=economic,
   Raw=factor*(row.PNHICLAGHGD*(classic?.002:.001)+(classic?300:50)+(!classic?Math.Min(cap,added):0))+economic*.04};
 }
 static double PopulationGain(AFAGFHJONFJ c,double economy,double addition)=>PopulationSnapshot(c).Gain(economy,addition);
}
