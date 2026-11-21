using System;using System.Collections.Generic;using System.Reflection;using UnityEngine;
public static class LiveBattle97Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Set(string n,object v)=>typeof(StrategicSupport).GetField(n,F).SetValue(null,v);
 static T Get<T>(string n)=>(T)typeof(StrategicSupport).GetField(n,F).GetValue(null);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("LiveBattle97: "+why);}
 static IEnumerator<string> Once(Action a){a();yield return "test";}
 public static void Run(){
  new StrategicSupport().Load();typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");Set("schedulerOnline",true);
  var w=Wa_Controller.m_warInfo;var q=Get<WorkQueue>("work");int battle=0,city=0,foreign=0;
  q.Add("battle-support-plan:"+w.Pointer+":F",()=>Once(()=>battle++));q.Add("city:reserved",()=>Once(()=>city++));q.Add("battle-support-plan:999999:F",()=>Once(()=>foreign++));
  AEHAGJAKNEK.PAKENCLGOHK=true;Wa_Controller.m_isRoundMode=true;Wa_Controller.m_battleSpeed=0;
  Call("PumpEconomy");Check(battle==0&&city==0,"actual tactical pause preserves all work");
  Wa_Controller.m_battleSpeed=1;Call("PumpEconomy");Check(battle==1&&city==0&&foreign==0,"active tactical combat processes current support while campaign paused, unrelated work remains queued");
  Wa_Controller.m_isRoundMode=false;Call("PumpEconomy");Check(city==0&&foreign==0,"real-time campaign pause is respected");
  AEHAGJAKNEK.PAKENCLGOHK=false;for(int i=0;i<5&&city==0;i++)Call("PumpEconomy");Check(city==1,"normal map work resumes");
  q.Clear();Set("schedulerOnline",false);
  var c=MICGPAMNDKM.IJPKEMPIFIH["source"];w.IJGLCFJOOIK=c;w.GBHGENNMJCK=new Vector3(10,0,10);
  var team=new Wo_DeployTeamCtrl{m_id=9701,m_belongPower="F",m_deployState=AKOPKFPKJPP.Expedition,m_targetCity=c.OJPECMNINNA,Position=new Vector3(0,0,0)};
  team.m_listMovePos.Add(new Vector3(10,0,0));team.m_listMovePos.Add(w.GBHGENNMJCK);ICGAEMLACJG.HFJIECIBFKB[team.m_id]=team;
  c.LMPMNLLGOEC.IPJHOOJBCKI="";
  Check(Math.Abs(StrategicSupport.SupportArrivalSeconds(team.Pointer,w.Pointer)-10)<.001,"manual no-governor ETA follows actual bent route, not straight distance");
  Time.timeScale=2;Check(Math.Abs(StrategicSupport.SupportArrivalSeconds(team.Pointer,w.Pointer)-5)<.001,"manual ETA uses native movement speed times actual timeScale exactly once");Time.timeScale=1;
  team.Position=new Vector3(5,0,0);Check(Math.Abs(StrategicSupport.SupportArrivalSeconds(team.Pointer,w.Pointer)-7.5)<.001,"ETA follows actual movement, no independent countdown");
  team.m_listMovePos[1]=new Vector3(99,0,99);Check(double.IsPositiveInfinity(StrategicSupport.SupportArrivalSeconds(team.Pointer,w.Pointer)),"stale route never promises arrival");
  team.m_listMovePos[1]=w.GBHGENNMJCK;team.m_targetCity="unrelated";Check(double.IsPositiveInfinity(StrategicSupport.SupportArrivalSeconds(team.Pointer,w.Pointer)),"unrelated target excluded");
  team.m_targetCity=null;team.m_attackTarget=new(){m_warInfo=w};Check(double.IsFinite(StrategicSupport.SupportArrivalSeconds(team.Pointer,w.Pointer)),"manual army-targeted relief included");
  Check(double.IsPositiveInfinity(StrategicSupport.SupportArrivalSeconds(team.Pointer,(IntPtr)99999)),"wrong battle excluded");
  Console.WriteLine($"PASS LiveBattle97 {checks} campaign-clock, selective queue, manual-support and route-ETA checks; native movement mocked.");
 }
}
