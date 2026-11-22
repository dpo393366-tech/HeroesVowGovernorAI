using System;
using System.Collections.Generic;
using System.Diagnostics;
public sealed partial class StrategicSupport {
 // Legacy call sites keep their action semantics; profiling is removed.
 static T TimedAI<T>(MKMOJNAJKJE f,string phase,Func<T> action)=>action();
 static double ScoreStrategy(JFLCKHMFGPJ s,StatecraftPolicy.Needs needs,out string reason){
  reason=null;double benefit=0,loss=0;
  if(s.MDPGDMKHDAG==null||s.MDPGDMKHDAG.Count==0){reason="missing-effects";return 0;}
  foreach(var e in s.MDPGDMKHDAG){
   if(e==null){reason="null-effect";return 0;}int kind=(int)e.HFBAJIGHOAF,amount=e.HLNCKLGGECA;
   // Parsed targeted effects use the native 100..113 enum range. They are
   // not production effects 0..13 even when JSON encodes the same low number.
   if(kind>=100&&kind<=113){
    bool useful=(kind==100||kind==101||kind==102||kind==105||kind==109||kind==110||kind==112||kind==113)?amount<0:(kind==103||kind==104||kind==108||kind==111)?amount>0:false;
    if(!useful){reason="target-effect-not-beneficial-"+kind;return 0;}benefit+=50+Math.Min(100,Math.Abs((double)amount));continue;
   }
   if(e.ODEKPKAIGPD<0||e.ODEKPKAIGPD>1){reason="target-selection-required-scope-"+e.ODEKPKAIGPD;return 0;}
   if(e.LPIEHGBCPLP<0||e.LPIEHGBCPLP>1){reason="unknown-effect-mode";return 0;}
   double score=StatecraftPolicy.Effect(kind,amount,needs);
   if(score>=0){benefit+=score;continue;}
   // Only quantified production/speed tradeoffs can be netted. Unknown effects
   // and negative healing/loyalty remain explicit rejections, never ignored.
   if(amount<0&&amount>int.MinValue&&kind>=0&&kind<=38&&kind!=25&&kind!=26){
    double cost=StatecraftPolicy.Effect(kind,-amount,needs);
    if(cost>=0){loss+=Math.Max(cost,Math.Min(100,-(double)amount));continue;}
   }
   reason=$"unsupported-effect-{kind}:{amount}";return 0;
  }
  double net=benefit-loss*1.5;
  if(net<=0)reason=loss>0?$"tradeoff-benefit-{benefit:F0}-loss-{loss:F0}":"no-current-benefit";
  return Math.Max(0,net);
 }
 static void AuditFaction(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities){
  if(!Gate("faction-census:"+f.IBGCLCAIBOL,60))return;
  int wars=0,fronts=0,neutral=0,managed=0,hold=0,armies=0,battles=0;long cityCash=0;var border=new HashSet<string>();
  if(f.AENJACOFBJK!=null)foreach(var rel in f.AENJACOFBJK)if(rel.Value?.EBIOPDHEBOF==BHBNKONCOJD.War)wars++;
  foreach(var c in cities){cityCash+=c.LMPMNLLGOEC.DNOGPFHMOMO;if(Eligible(c,out _))managed++;
   if(f.ANMMMOIKLEG.TryGetValue(c.LMPMNLLGOEC.IPJHOOJBCKI??"",out var g)&&g?.BNGFJOAJKDH==NEFBEOFPGJA.StopWar)hold++;
   if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n?.LMPMNLLGOEC!=null&&border.Add(n.OJPECMNINNA)){if(string.IsNullOrEmpty(n.LMPMNLLGOEC.LMMNKIOAGLC))neutral++;else if(War(f,n.LMPMNLLGOEC.LMMNKIOAGLC))fronts++;}
  }
  foreach(var t in AttackTeams())if(t.m_belongPower==f.IBGCLCAIBOL){armies++;if(t.m_warInfo!=null)battles++;}
  Note($"faction-census faction={f.IBGCLCAIBOL} playerFaction={f.IBGCLCAIBOL==NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC} cities={cities.Count} managed={managed} stopWarCities={hold} warRelations={wars} hostileNeighbours={fronts} neutralNeighbours={neutral} treasury={f.DNOGPFHMOMO} cityCashTotal={cityCash} activeTeams={armies} battleTeams={battles}");
 }
}
