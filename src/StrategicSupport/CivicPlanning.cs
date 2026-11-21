using System;
using System.Collections.Generic;
public sealed partial class StrategicSupport {
 sealed class CivicFlow {public double Popular,Security,Population;}
 static long civicSlice=-1;static readonly Dictionary<IntPtr,CivicFlow> sliceCivic=new();
 static CivicFlow ReadCivicFlow(AFAGFHJONFJ c){
  if(!WorkQueue.InStep)return ReadCivicFlowFresh(c);
  if(civicSlice!=WorkQueue.StepToken){sliceCivic.Clear();civicSlice=WorkQueue.StepToken;}
  if(!sliceCivic.TryGetValue(c.Pointer,out var flow))sliceCivic[c.Pointer]=flow=ReadCivicFlowFresh(c);return flow;
 }
 static void InvalidatePlanningReads(){sliceCivic.Clear();slicePopulation.Clear();}
 static CivicFlow ReadCivicFlowFresh(AFAGFHJONFJ c){
  int a=0,b=0,d=0,e=0,g=0;
  // Same monthly flow calculations as the city UI. Current stock (even 1000)
  // is deliberately not evidence of sustainable normal growth.
  int popular=c.JKLONBNMKCL(out a,out b,out d,out e,out g);
  popular-=Math.Min(0,c.HPIFFEHIDLK((LAFAADCAHAA)5,""));
  a=b=d=e=0;int security=c.MCDLJIAJMFI(out a,out b,out d,out e);
  security-=Math.Min(0,c.HPIFFEHIDLK((LAFAADCAHAA)4,""));
  // DJE subtracts plague deaths; KCF is the native pre-disaster growth.
  int population=c.KCFNACGFBFF();
  return new(){Popular=popular,Security=security,Population=population};
 }
 static bool CivicReplacement(AFAGFHJONFJ c,IEnumerable<OGABIJECHFG> removed,PABIPDOFBFG b){
  var flow=ReadCivicFlow(c);double popular=0,security=0,economy=0,population=0;
  foreach(var x in removed){if(x?.GEKKLMPLDHJ==null)continue;popular+=x.GEKKLMPLDHJ.DPFIFIOKHEI*30;security+=x.GEKKLMPLDHJ.HKBPGJLMFIG*30;economy+=x.GEKKLMPLDHJ.CGBGIDCEBGD+x.GEKKLMPLDHJ.EGBEEDFCIGH;population+=x.GEKKLMPLDHJ.DHLNIKKLNKJ;}
  // New goal buildings may be planned first, but a negative-flow transaction
  // waits for its compensating infrastructure to actually finish.
  return flow.Popular-popular+b.DPFIFIOKHEI*30>=Math.Min(1,flow.Popular)
   &&flow.Security-security+b.HKBPGJLMFIG*30>=Math.Min(1,flow.Security)
   &&(flow.Population+PopulationGain(c,b.CGBGIDCEBGD+b.EGBEEDFCIGH-economy,b.DHLNIKKLNKJ-population)>=Math.Min(0,flow.Population));
 }
 static string CivicBuildBlock(AFAGFHJONFJ c,PABIPDOFBFG b){InvalidatePlanningReads();var flow=ReadCivicFlow(c);string reason=CivicBlock(flow,b);if(reason!=null)return reason;
  return flow.Population+PopulationGain(c,b.CGBGIDCEBGD+b.EGBEEDFCIGH,b.DHLNIKKLNKJ)<Math.Min(0,flow.Population)?"population-net-decrease":null;
 }
 static string CivicBlock(CivicFlow flow,PABIPDOFBFG b){
  if(flow.Popular+b.DPFIFIOKHEI*30<Math.Min(1,flow.Popular))return "popular-net-decrease";
  if(flow.Security+b.HKBPGJLMFIG*30<Math.Min(1,flow.Security))return "security-net-decrease";
  return null;
 }
 static bool StrategyCivicSafe(JFLCKHMFGPJ strategy,List<AFAGFHJONFJ> cities){
  double popular=0,security=0,population=0;bool populationPercent=false;
  foreach(var effect in strategy.MDPGDMKHDAG){
   if(effect==null||effect.ODEKPKAIGPD>1)continue;
   int kind=(int)effect.HFBAJIGHOAF,amount=effect.HLNCKLGGECA;
   if(kind==29)popular+=amount*(effect.LPIEHGBCPLP==0?30:1);
   if(kind==31)security+=amount*(effect.LPIEHGBCPLP==0?30:1);
   if(kind==30)population+=amount;
   if(kind==14&&amount<0)populationPercent=true;
  }
  if(popular>=0&&security>=0&&population>=0&&!populationPercent)return true;
  foreach(var c in cities){var flow=ReadCivicFlow(c);
   if(popular<0&&flow.Popular+popular<1||security<0&&flow.Security+security<1||population<0&&flow.Population+population<1||populationPercent&&flow.Population<=0)return false;
  }return true;
 }
}
