using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;
public static class Relief88Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static object Fixture(string n,params object[] a)=>typeof(Relief86Tests).GetMethod(n,F).Invoke(null,a);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("R88: "+why);}
 public static void Run(){
  Fixture("Setup");var a=(AFAGFHJONFJ)Fixture("City","Xinye",0f);var b=(AFAGFHJONFJ)Fixture("City","Runan",10f);Fixture("Link",a,b);Fixture("Expose",a,10f);
  a.FKMKGNPFLFP.Last().LMPMNLLGOEC.KJPPGIOCDID[0].CombatValue=40000;
  var f=NEINKOOBJIH.COCMABJFOFH["F"];var teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();
  double old=(double)Call("CoverDeadlineWithHome",a,f,teams,6000d),small=(double)Call("CoverDeadlineWithHome",a,f,teams,9000d);
  Check(old<17&&small>17,"same 15s backfill plus 2s margin: old max-withdrawal rejects, actual 1-unit departure survives");
  Fixture("Plan",a);var covers=(IDictionary)typeof(StrategicSupport).GetField("coverOrders",F).GetValue(null);Check(covers.Count==1,"marginal real source commits minimum backfill");
  var outgoing=(Wo_DeployTeamCtrl)Fixture("Generate",a);Check(outgoing!=null&&outgoing.GetSoldierNum(false)==1000,"near first wave released against timely real backfill");
  teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();Check((double)Call("CoverDeadline",a,f,teams)>17,"already departed first wave not subtracted a second time");
  Check(Fixture("Generate",a)==null,"same credited unit cannot fund an unsupported second first-wave unit");
  var follow=(Wo_DeployTeamCtrl)Fixture("Generate",b);Check(follow!=null&&follow.GetSoldierNum(false)==3000,"donor sends its remaining safe follow-up after minimum backfill");
  b.Last.m_targetCity="manual";foreach(var cover in covers.Values.Cast<object>().ToArray()){var team=(Wo_DeployTeamCtrl)cover.GetType().GetField("Team").GetValue(cover);team.m_targetCity="manual";Call("CheckRegionalCover",cover);}Check(Fixture("Generate",a)==null,"revoked backfill cannot authorize a later wave");
  for(int i=1;i<=9;i++){double deadline=(double)Call("CoverDeadlineWithHome",a,f,teams,i*1000d);double more=(double)Call("CoverDeadlineWithHome",a,f,teams,(i+1)*1000d);Check(more>=deadline,"keeping more actual troops never shortens holding time");}
  Check((double)Call("BattleRetryDelay",10d,2d)==2,"known unmet need rechecked after one game day, not ten simulation seconds");
  Check((double)Call("BattleRetryDelay",5d,10d)==5,"never slow an already faster configured observation");
  Check((double)Call("BattleRetryDelay",10d,0d)==1,"retry delay remains positive");
  Call("ResetWork");Call("ResetDefense");
  // A source becoming available immediately after the first pass is reconsidered
  // at t=2, while the old normal assessment would wait until t=10.
  typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);
  var source=(AFAGFHJONFJ)typeof(BattleIntegrationTests).GetField("source",F).GetValue(null);
  var war=(NJHDEHLHENN)typeof(BattleIntegrationTests).GetField("war",F).GetValue(null);
  source.HomeOfficerCount=1;Call("Assess",war,0d,true);int before=source.Calls;source.HomeOfficerCount=5;Call("Assess",war,2d,true);
  Check(source.Calls>before&&!source.Last.Dismissed,"actual assessment loop retries a rejected first dispatch at the urgent interval");
  Call("ResetBattleState");Call("ResetWork");Call("ResetDefense");Console.WriteLine($"PASS R88 {checks} actual residual, minimum first wave, remaining follow-up and urgent retry checks; mocked native calls.");
 }
}
