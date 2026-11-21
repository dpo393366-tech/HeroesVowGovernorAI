using System;using System.Reflection;using System.Collections;using System.Collections.Generic;using System.Linq;
public static class Relief85Tests{
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("R85: "+why);}
 public static void Run(){
  typeof(StrategicSafetyTests).GetMethod("Setup",F).Invoke(null,null);
  var c=(AFAGFHJONFJ)typeof(StrategicSafetyTests).GetField("c",F).GetValue(null);
  var f=NEINKOOBJIH.COCMABJFOFH["F"];var teams=new List<Wo_DeployTeamCtrl>();
  var gates=(IDictionary)typeof(StrategicSupport).GetField("next",F).GetValue(null);
  gates["source:"+c.OJPECMNINNA]=Environment.TickCount64+60000;
  Check(Call("SupportSourceBlock",c,teams,new HashSet<IntPtr>())==null,"stale city-wide cooldown cannot block real surplus");
  var relocating=c.LMPMNLLGOEC.KJPPGIOCDID[0];var moved=new Wo_DeployTeamCtrl{m_belongPower="F",m_assembleCity=c.OJPECMNINNA,m_targetCity="rear"};moved.m_dicDeployArmy["x"]=new(){relocating};teams.Add(moved);
  var reserved=new HashSet<IntPtr>{relocating.Pointer};
  var aid=(Wo_DeployTeamCtrl)Call("GenerateSupport",c,10000,teams,reserved);
  Check(aid!=null,"relief can follow capacity transfer immediately");
  var sent=aid.m_dicDeployArmy.Values.SelectMany(x=>x).ToList();
  Check(!sent.Contains(relocating)&&sent.Sum(a=>a.FGHFOLKBHDL)<=3000,"previously dispatched unit excluded and total retained reserve preserved");
  teams.Add(aid);foreach(var a in sent)reserved.Add(a.Pointer);
  Check(Call("GenerateSupport",c,10000,teams,reserved)==null,"no cooldown does not permit repeated lending beyond reserve");
  teams.Add(new(){m_belongPower="enemy",m_targetCity=c.OJPECMNINNA});
  Check((string)Call("SupportSourceBlock",c,teams,reserved)=="source-under-attack","direct source invasion still blocks");
  teams.RemoveAt(teams.Count-1);f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;
  Check((string)Call("SupportSourceBlock",c,teams,reserved)!="stop-war-policy","StopWar governs conquest, not internal support");
  typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);
  var battle=(NJHDEHLHENN)typeof(BattleIntegrationTests).GetField("war",F).GetValue(null);
  var source=(AFAGFHJONFJ)typeof(BattleIntegrationTests).GetField("source",F).GetValue(null);
  typeof(StrategicSupport).GetField("schedulerOnline",F).SetValue(null,false);
  Call("Assess",battle,0d,true);
  Check(source.Calls==1,"clear initial deficit acts on first observation, not ten seconds later");
  typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);
  battle=(NJHDEHLHENN)typeof(BattleIntegrationTests).GetField("war",F).GetValue(null);
  source=(AFAGFHJONFJ)typeof(BattleIntegrationTests).GetField("source",F).GetValue(null);
  typeof(StrategicSupport).GetField("schedulerOnline",F).SetValue(null,true);
  Call("Assess",battle,0d,true);Check(source.Calls==0,"initial path work is queued");
  var queue=(WorkQueue)typeof(StrategicSupport).GetField("work",F).GetValue(null);
  queue.Pump(1000,1000,(x,y)=>{},e=>throw e);
  Call("Assess",battle,0d,true);
  Check(source.Calls==1,"completed path permits same-clock reassessment without extra ten-second wait");
  var states=(IDictionary)typeof(StrategicSupport).GetField("battles",F).GetValue(null);
  var state=states[battle.Pointer];var losses=(AttritionWindow[])state.GetType().GetField("Loss").GetValue(state);
  Check(losses[0].Samples==1,"path-ready retry does not duplicate casualty observations");
  typeof(StrategicSupport).GetField("schedulerOnline",F).SetValue(null,false);Call("ResetWork");
  Console.WriteLine($"PASS R85 {checks} consecutive-order safety checks; native actions mocked.");
 }
}
