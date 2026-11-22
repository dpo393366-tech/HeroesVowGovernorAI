using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class BattleRetry118Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static AFAGFHJONFJ source;static NJHDEHLHENN war;static MKMOJNAJKJE faction;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Field<T>(string n)=>(T)typeof(StrategicSupport).GetField(n,F).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("BattleRetry118: "+why);}
 static void Setup(){
  typeof(Emergency88Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetManualOrders");MLGOLGGHLMB.CBEGDDKANAO.Clear();
  source=(AFAGFHJONFJ)typeof(BattleIntegrationTests).GetField("source",F).GetValue(null);war=(NJHDEHLHENN)typeof(BattleIntegrationTests).GetField("war",F).GetValue(null);faction=NEINKOOBJIH.COCMABJFOFH["F"];
  NEINKOOBJIH.MIPFHCKDJOK=100;NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
 }
 static Wo_DeployTeamCtrl Send(Func<double,bool> useful=null){var teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();return (Wo_DeployTeamCtrl)Call("DispatchBattle",source,war,0,10000,30d,false,teams,Call("AttackReserved",teams),useful??(_=>true));}
 static void NativeCancel(Wo_DeployTeamCtrl t){Call("RecordDismissal",t);t.Dismiss(true,false);}
 static string Signature()=>(string)Call("BattleDispatchSignature",source,war,faction,ICGAEMLACJG.HFJIECIBFKB.Values.ToList());
 public static void Run(){
  Setup();Check(Send(_=>false)==null&&source.Calls==1&&source.Last.Dismissed,"first actual arrival rejection rolls back generated army");
  for(int i=0;i<143;i++){NEINKOOBJIH.MIPFHCKDJOK=100+i/143d;Check(Send(_=>false)==null,"repeated failed assessment produces no new army "+i);}
  Check(source.Calls==1,"143 unchanged assessments cause one assembly, not 143 notifications");
  NEINKOOBJIH.MIPFHCKDJOK=101;Send(_=>false);NEINKOOBJIH.MIPFHCKDJOK=102;Send(_=>false);Check(source.Calls==1,"next two daily decisions do not reassemble unchanged rejected force");
  Check(Send(_=>true)!=null&&source.Calls==2,"battle becomes supportable at measured ETA: retry immediately");
  Setup();Send(_=>false);war=new(){HIENOINGBCN="another war",GBHGENNMJCK=new(-100)};Check(Send()!=null&&source.Calls==2,"same city may support another battle immediately");
  Setup();Send(_=>false);source.LMPMNLLGOEC.KJPPGIOCDID.Add(new());Check(Send(_=>false)==null&&source.Calls==2,"new local troop roster reopens assessment");
  Setup();Send(_=>false);war.GBHGENNMJCK=new(80);Send(_=>false);Check(source.Calls==2,"battle movement invalidates rejected road/arrival snapshot");
  Setup();Send(_=>false);NEINKOOBJIH.MIPFHCKDJOK=103;Send(_=>false);Check(source.Calls==2,"bounded periodic retry can detect unobserved native changes");
  NEINKOOBJIH.MIPFHCKDJOK=106;Send(_=>false);Check(source.Calls==2,"second identical failure backs off longer without daily cancellation");
  Setup();var t=Send();Check(t!=null&&source.Calls==1,"native-cancel fixture starts actual mod assembly");NativeCancel(t);
  for(int day=100;day<=102;day++){NEINKOOBJIH.MIPFHCKDJOK=day;Check(Send()==null,"predeparture native cancellation retained on day "+day);}Check(source.Calls==1,"native cancellation memory prevents repeated generation");
  Setup();t=Send();Call("RecordAttackDeparture",t,source);t.Position=war.GBHGENNMJCK;NativeCancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"normal postdeparture arrival demobilization does not blacklist source");
  Setup();t=Send();t.m_id++;Call("RecordDismissal",t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"pooled army identity mismatch cannot reject new source");
  Setup();t=Send();t.m_warInfo=war;Call("RecordDismissal",t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"battle cleanup is not predeparture failure");
  Setup();Send(_=>false);Call("ForgetBattle",war);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"battle recycle clears failure records");
  Setup();Send(_=>false);Call("ResetBattleState");Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"save switch clears rejected attempts");
  Setup();string signature=Signature();Call("RejectBattleDispatch",source,war,0,false,signature,"journey-food",30d,1,(long)source.LMPMNLLGOEC.OEGNGFBCHFL);source.LMPMNLLGOEC.OEGNGFBCHFL*=2;
  Check((bool)Call("BattleDispatchReady",source,war,0,false,signature,new Func<double,bool>(_=>true)),"food replenishment reopens failed journey without waiting");
  Setup();long originalFood=source.LMPMNLLGOEC.OEGNGFBCHFL;source.Generated=()=>source.LMPMNLLGOEC.OEGNGFBCHFL=0;
  Send();source.LMPMNLLGOEC.OEGNGFBCHFL=(int)originalFood;Send();Check(source.Calls==1,"assembly rollback food refund is not mistaken for new supply");
  Setup();source.Generated=()=>source.Last.m_listHero.Clear();Send();Check(source.Last.Dismissed&&Field<IDictionary>("rejectedBattleDispatches").Count==1,"generator result rejected internally also records cancelled assembly");
  Call("ResetBattleState");Call("ResetWork");Call("ResetDefense");Call("ResetManualOrders");
  Console.WriteLine("PASS BattleRetry118 "+checks+" unchanged failure, native dismissal, early reopen and lifecycle checks; native calls mocked, reporter save not available.");
 }
}
