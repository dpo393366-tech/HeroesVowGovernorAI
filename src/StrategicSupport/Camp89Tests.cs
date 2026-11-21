using System;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;
public static class Camp89Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;static Wo_DeployTeamCtrl army;static NJHDEHLHENN war;static MKMOJNAJKJE faction;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static IDictionary Goals=>(IDictionary)typeof(StrategicSupport).GetField("campaignGoals",F).GetValue(null);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Camp89: "+why);}
 static void Setup(){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");Call("ResetBattleState");
  army=(Wo_DeployTeamCtrl)typeof(Campaign81Tests).GetField("t",F).GetValue(null);faction=NEINKOOBJIH.COCMABJFOFH["F"];
  war=new(){HIENOINGBCN="other-front",GBHGENNMJCK=new(60),FGHFOLKBHDL=new[]{3000,20000}};Wa_Controller.m_warInfo=null;
  ICGAEMLACJG.HFJIECIBFKB[8992]=new(){m_id=8992,m_belongPower="F",m_warInfo=war};
  AOHIBKLOAEI.Region=null;AOHIBKLOAEI.RegionAt=null;Call("RememberCampaign",army);
 }
 static void Decide(){foreach(var phase in (IEnumerable<string>)Call("RecoverCampaign",army,Goals[army.Pointer])){} }
 public static void Run(){
  Setup();int total=army.GetSoldierNum(false);Decide();Check(army.m_targetWarInfo==war&&army.m_deployState==AKOPKFPKJPP.Expedition,"idle camp reinforces losing nearby battle before a fresh offensive");
  Check(army.GetSoldierNum(false)==total&&army.Position.x==50&&!army.Dismissed,"existing soldiers travel without creation, teleport or field dismissal");
  Setup();faction.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;Decide();Check(army.m_targetWarInfo==war,"StopWar permits reinforcement of ongoing friendly battle");
  Setup();army.Morale=20;Decide();Check(army.m_targetWarInfo==null&&army.m_isEnterCity,"exhausted survivor returns instead of joining another battle");
  Setup();war.FGHFOLKBHDL=new[]{50000,1000};Decide();Check(army.m_targetWarInfo==null,"already winning battle does not collect every camped army");
  Setup();var it=((IEnumerable<string>)Call("RecoverCampaign",army,Goals[army.Pointer])).GetEnumerator();Check(it.MoveNext(),"campaign selection yields");army.m_listMovePos.Add(new(700));while(it.MoveNext()){}it.Dispose();Check(army.m_targetWarInfo==null,"new user/native movement during scan is preserved");
  Call("ResetWork");Call("ResetBattleState");Goals.Clear();Console.WriteLine($"PASS R89 {checks} camp battle/advance/return checks; native movement mocked.");
 }
}
