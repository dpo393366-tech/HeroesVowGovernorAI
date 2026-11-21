using System;using System.Reflection;using System.Linq;using System.Collections;using System.Collections.Generic;using UnityEngine;
public static class Relief87Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string method,params object[] args)=>typeof(StrategicSupport).GetMethod(method,F).Invoke(null,args);
 static object Fixture(string method,params object[] args)=>typeof(Relief86Tests).GetMethod(method,F).Invoke(null,args);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("R87: "+why);}
 public static void Run(){
  double Collapse(params TimedBorderDefense.Wave[] waves)=>TimedBorderDefense.Collapse(4950,waves,60);
  double local=Collapse(new TimedBorderDefense.Wave(20,35000));Check(Math.Abs(local-(20+4950d*60/35000))<.0001,"only local arrived force determines initial survival");
  Check(Collapse(new TimedBorderDefense.Wave(20,35000),new(500,9000000))==local,"far army cannot retroactively shorten first-wave survival");
  Check(local>23.9+2,"fixture reproduces timely Runan cover after remote/other-front contamination removed");
  Check(Collapse(new TimedBorderDefense.Wave(1,35000))<23.9+2,"genuine immediate local threat still blocks the same loan");
  Check(double.IsPositiveInfinity(Collapse()),"no relevant force does not invent immediate collapse");
  Check(Collapse(new TimedBorderDefense.Wave(double.NaN,100))==0,"invalid timing fails closed");
  Check(Collapse(new TimedBorderDefense.Wave(20,35000),new(20,35000))<local,"simultaneous actual local forces combine");
  Check(Collapse(new TimedBorderDefense.Wave(20,35000),new(22,35000))>Collapse(new TimedBorderDefense.Wave(20,70000)),"later second wave contributes only after arrival");
  Fixture("Setup");var a=(AFAGFHJONFJ)Fixture("City","Xinye",0f);var b=(AFAGFHJONFJ)Fixture("City","Runan",10f);Fixture("Link",a,b);Fixture("Expose",a,1000f);
  var f=NEINKOOBJIH.COCMABJFOFH["F"];var foe=ICGAEMLACJG.HFJIECIBFKB[1];foe.Position=new(1);foe.m_dicDeployArmy["test"]=new(){new(){FGHFOLKBHDL=100000,CombatValue=100000}};
  Check(!(bool)Call("IsCounterattackCandidate",foe,a),"army marching on another known city is not duplicated as an immediate Xinye attack");
  Fixture("Plan",a);var covers=(IDictionary)typeof(StrategicSupport).GetField("coverOrders",F).GetValue(null);Check(covers.Count==1,"near first-wave source receives real backfill despite nearby enemy committed to the other battle");
  Check(b.Last.GetSoldierNum(false)==1000,"backfill chooses only the unit needed instead of all forty percent surplus");
  var teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();var reserved=(HashSet<IntPtr>)Call("AttackReserved",teams);
  var first=(Wo_DeployTeamCtrl)Call("GenerateSupport",a,10000,teams,reserved);Check(first!=null&&first.GetSoldierNum(false)==1000,"Xinye first wave unlocked against the actual committed minimum backfill");
  teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();reserved=(HashSet<IntPtr>)Call("AttackReserved",teams);var follow=(Wo_DeployTeamCtrl)Call("GenerateSupport",b,10000,teams,reserved);Check(follow!=null&&follow.GetSoldierNum(false)==3000,"donor retains unused surplus for a direct follow-up wave");
  foe.m_targetCity=a.OJPECMNINNA;Check((bool)Call("IsCounterattackCandidate",foe,a)&&(double)Call("CoverDeadline",a,f,ICGAEMLACJG.HFJIECIBFKB.Values.ToList())==0,"real redirection to Xinye immediately blocks further lending");
  foe.m_targetCity="";Check((bool)Call("IsCounterattackCandidate",foe,a),"unassigned field army remains a possible local threat");
  foe.m_warInfo=new(){IJGLCFJOOIK=MICGPAMNDKM.IJPKEMPIFIH["front"]};Check(!(bool)Call("IsCounterattackCandidate",foe,a),"army already fighting elsewhere is not duplicated");
  var rng=new Random(8719);for(int i=0;i<500;i++){
   double home=rng.Next(100,50000),p=rng.Next(100,100000),at=rng.Next(1,100);double early=TimedBorderDefense.Collapse(home,new[]{new TimedBorderDefense.Wave(at,p)},60);
   double delayed=TimedBorderDefense.Collapse(home,new[]{new TimedBorderDefense.Wave(at+10,p)},60);Check(delayed>=early,"delaying a threat cannot worsen local survival");
  }
  Call("ResetDefense");Call("ResetWork");Console.WriteLine($"PASS R87 {checks} timed counterattack, observed intent and minimum backfill checks; native calls mocked, not live gameplay.");
 }
}
