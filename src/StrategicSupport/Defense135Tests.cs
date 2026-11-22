using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;
public static class Defense135Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Defense135: "+why);}
 static IDictionary Orders=>Field<IDictionary>(typeof(StrategicSupport),"defenseOrders");
 static IDictionary Goals=>Field<IDictionary>(typeof(StrategicSupport),"campaignGoals");
 static Wo_DeployTeamCtrl t,enemy;static AFAGFHJONFJ home;static MKMOJNAJKJE faction;
 static void Setup(){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetDefense");Call("ResetWork");
  t=Field<Wo_DeployTeamCtrl>(typeof(Campaign81Tests),"t");home=Field<AFAGFHJONFJ>(typeof(Campaign81Tests),"home");faction=NEINKOOBJIH.COCMABJFOFH["F"];
  AOHIBKLOAEI.RegionAt=p=>home;home.Capacity=0;t.Position=home.BMBMKPGEHBH;
  enemy=new(){m_id=13501,m_belongPower="enemy",m_targetCity=home.OJPECMNINNA,m_deployState=AKOPKFPKJPP.Expedition,Position=new(80)};
  ICGAEMLACJG.HFJIECIBFKB[enemy.m_id]=enemy;
 }
 static void Recover(){foreach(var s in (IEnumerable<string>)Call("RecoverCampaign",t,Goals[t.Pointer])){}}
 static IEnumerable<string> Forever(){while(true)yield return "busy";}
 static void Pump(){Field<WorkQueue>(typeof(StrategicSupport),"work").Pump(10000,200,(k,ms)=>{},e=>throw e);}
 public static void Run(){
  Setup();int soldiers=t.GetSoldierNum(false);Recover();
  Check(Orders.Contains(t.Pointer)&&!Goals.Contains(t.Pointer),"threatened full city keeps field defender instead of partition return");
  Check(!t.Dismissed&&!t.m_isEnterCity&&t.CityMoves==0&&t.GetSoldierNum(false)==soldiers,"handoff preserves soldiers and avoids teleport/admission");
  var order=Orders[t.Pointer];Call("CheckDefenseOrder",order);Check(Orders.Contains(t.Pointer)&&!t.m_isEnterCity,"stale offensive city target cleared before defense checks");
  var war=new NJHDEHLHENN{IJGLCFJOOIK=home,EMMJBPMNFAK=new[]{"F","enemy"},GBHGENNMJCK=new(12)};enemy.m_warInfo=war;enemy.m_targetCity="";
  // The global census may contain hundreds of tasks. A camp event schedules
  // only this existing order and gets a share of the unchanged frame budget.
  Call("ResetWork");var queue=Field<WorkQueue>(typeof(StrategicSupport),"work");for(int i=0;i<800;i++)queue.Add("background:"+i,()=>Forever().GetEnumerator());
  t.StartCamp();Call("NotifyDefenseCamp",t);int size=queue.Count;Call("NotifyDefenseCamp",t);Check(queue.Count==size,"repeated camp events coalesce");Pump();
  Check(t.m_targetWarInfo==war&&t.m_listMovePos.Count>0,"camp defender joins battle without waiting for whole-world census");
  Setup();Recover();order=Orders[t.Pointer];ICGAEMLACJG.HFJIECIBFKB.Remove(enemy.m_id);Call("CheckDefenseOrder",order);
  Check(!Orders.Contains(t.Pointer)&&Goals.Contains(t.Pointer),"ended threat releases ownership for normal return");
  ICGAEMLACJG.HFJIECIBFKB[enemy.m_id]=enemy;Check((bool)Call("RetainThreatenedLocalDefense",t,Goals[t.Pointer],faction),"renewed invasion after threat-ended release can reacquire local defense");
  Setup();t.m_isPrivate=true;Recover();Check(!Orders.Contains(t.Pointer),"private army not adopted");
  Setup();t.m_responseToPlayer=true;Recover();Check(!Orders.Contains(t.Pointer),"player follow untouched");
  Setup();t.Food=0;Recover();Check(!Orders.Contains(t.Pointer),"starving army retains ability to retreat");
  Setup();t.Morale=20;Recover();Check(!Orders.Contains(t.Pointer),"broken morale does not acquire new local defense order");
  Setup();Recover();order=Orders[t.Pointer];Call("ReleaseDefense",order,"battle-route-unavailable",true);
  Check(!(bool)Call("RetainThreatenedLocalDefense",t,Goals[t.Pointer],faction),"failed route return is not immediately recaptured into defense loop");
  Setup();AOHIBKLOAEI.RegionAt=p=>null;Recover();Check(!Orders.Contains(t.Pointer),"unknown region not assumed to be threatened home");
  Setup();var goal=Goals[t.Pointer];goal.GetType().GetField("Returning").SetValue(goal,true);Recover();Check(Orders.Contains(t.Pointer),"returning flag cannot discard current local defense need");
  Setup();ICGAEMLACJG.HFJIECIBFKB.Remove(enemy.m_id);home.Capacity=30;
  var steps=((IEnumerable<string>)Call("ReturnPartitionSteps",t,Goals[t.Pointer],faction)).GetEnumerator();
  bool reached=false;while(steps.MoveNext())if(steps.Current=="return-partition-order"){reached=true;break;}
  Check(reached,"fixture reaches final return dispatch slice");ICGAEMLACJG.HFJIECIBFKB[enemy.m_id]=enemy;while(steps.MoveNext()){}
  Check(Orders.Contains(t.Pointer)&&t.CityMoves==0,"new invasion between planning and return commit retains army");
  AOHIBKLOAEI.RegionAt=null;Call("ResetDefense");Call("ResetWork");Console.WriteLine("PASS Defense135 "+checks+" threatened-city retention and event-driven rally handoff checks; native APIs mocked.");
 }
}
