using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.AI;
public static class DepartureTests {
 static int count;
 static AFAGFHJONFJ c,target; static MKMOJNAJKJE f; static object previousOrder;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,args);
 static T Field<T>(Type t,string name)=>(T)t.GetField(name,BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
 static IDictionary Orders=>Field<IDictionary>(typeof(StrategicSupport),"attackOrders");
 static void Check(bool value,string name){if(!value)throw new Exception("departure regression: "+name);count++;}
 static void Setup(bool native=true){
  typeof(OffensiveTests).GetMethod("Setup",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,null);
  c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");target=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"target");f=Field<MKMOJNAJKJE>(typeof(OffensiveTests),"f");
  foreach(var step in (IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")){}
  Check(c.Last!=null&&!c.Last.Dismissed,"valid fixture assembled");
  previousOrder=Orders.Values.Cast<object>().Single();if(native)Orders.Clear();
 }
 static bool Start()=> (bool)Call("GuardAttackStart",c.Last);
 static string OldFullValidation()=>(string)Call("ValidateAttack",previousOrder,c,target,f);
 public static void Run(){
  Setup();f.DNOGPFHMOMO=0;
  Check(OldFullValidation()=="treasury-changed","old native adoption reproduces treasury veto");
  Check(Start()&&!c.Last.Dismissed&&Orders.Count==0,"native treasury no longer applies extra-planner floor or acquires ownership");
  Setup();NavMesh.Blocked=true;
  Check(OldFullValidation()=="route-blocked","old native adoption reproduces route veto");
  Check(Start()&&!c.Last.Dismissed,"native path finding is not vetoed by extra-planner NavMesh estimate");
  Setup();var group=c.Last.m_dicDeployArmy.Values.First();
  foreach(var unit in c.LMPMNLLGOEC.KJPPGIOCDID.ToArray())if(!group.Contains(unit)&&group.Count<8)group.Add(unit);
  Check(OldFullValidation()=="defense-reserve-changed","old native adoption reproduces different native retention decision veto");
  Check(Start()&&!c.Last.Dismissed&&Orders.Count==0,"native planner keeps responsibility for its garrison allocation");
  Setup();f.DNOGPFHMOMO=0;NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="unrelated-player";
  Check(Start()&&!c.Last.Dismissed,"enemy native army uses same corrected boundary");
  Setup();f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;
  Check(!Start()&&c.Last.Dismissed,"native StopWar still cancels");
  Setup();f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.AttackPower;f.ANMMMOIKLEG["gov"].LMAEGAJADKI="different";
  Check(!Start()&&c.Last.Dismissed,"native designated enemy still enforced");
  Setup();f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.AttackPower;f.ANMMMOIKLEG["gov"].LMAEGAJADKI="enemy";
  Check(Start()&&!c.Last.Dismissed,"matching designated enemy permitted");
  Setup();c.Last.m_responseToPlayer=true;f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;
  Check(Start()&&!c.Last.Dismissed,"explicit player order exempt");
  Setup();c.Last.m_isPrivate=true;f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;
  Check(Start()&&!c.Last.Dismissed,"private force exempt");
  Setup();c.Last.m_isEnterCity=true;f.ANMMMOIKLEG["gov"].BNGFJOAJKDH=NEFBEOFPGJA.StopWar;
  Check(Start()&&!c.Last.Dismissed,"friendly transfer exempt");
  Setup(false);f.DNOGPFHMOMO=0;Check(!Start()&&c.Last.Dismissed,"mod extra attack retains its treasury guard");
  Setup(false);NavMesh.Blocked=true;Check(!Start()&&c.Last.Dismissed,"mod extra attack retains route guard");
  Setup(false);foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID.Where(a=>!c.Last.m_dicDeployArmy.Values.SelectMany(v=>v).Contains(a)))a.CombatValue=1;
  Check(!Start()&&c.Last.Dismissed,"mod extra attack retains frontier defense guard");
  Setup();Check(Start()&&Start()&&Orders.Count==0&&!c.Last.Dismissed,"repeated assembly lifecycle checks never adopt native order");
  Console.WriteLine("PASS departure ownership regressions="+count+"; reproduces old treasury/route/reserve vetoes using full validation. Native planner and gameplay remain unexecuted.");
 }
}
