using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
public static class Cadence89Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;
 static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Field<T>(string n)=>(T)typeof(StrategicSupport).GetField(n,F).GetValue(null);
 static T Data<T>(object o,string n)=>(T)o.GetType().GetField(n).GetValue(o);
 static T Fixture<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Cadence89: "+why);}
 static void Setup(Type t)=>t.GetMethod("Setup",F).Invoke(null,null);
 public static void Run(){
  string old=NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC;
  try {
   Call("ResetWork");NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
   foreach(int seconds in new[]{2,5,10,15,20,30,60,120}){
    string key="cadence89:"+seconds;
    long before=Environment.TickCount64;Check((bool)Call("AIGate",key,"F",seconds),"allied first evaluation admitted");
    long after=Environment.TickCount64,until=Field<Dictionary<string,long>>("next")[key];
    Check(until>=before+seconds*1000L&&until<=after+seconds*1000L,"allied interval unchanged");
    Check(!(bool)Call("AIGate",key,"F",seconds),"repeated evaluation blocked");
    before=Environment.TickCount64;Check((bool)Call("AIGate",key,"enemy",seconds),"city ownership change resets cadence");
    after=Environment.TickCount64;until=Field<Dictionary<string,long>>("next")[key];
    Check(until>=before+seconds*3000L&&until<=after+seconds*3000L,"NPC interval exactly threefold");
   }
   Call("AIGate","switch89","F",10);NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="";
   Check((bool)Call("AIGate","switch89","F",10),"retirement reclassifies former faction as NPC immediately");
   NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
   Check((bool)Call("AIGate","switch89","F",10),"rejoining does not retain NPC timer");
   Battle();Supply();
  }finally{NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC=old;Call("ResetWork");Call("ResetBattleState");}
  Console.WriteLine($"PASS R89 {checks} allied/NPC cadence checks including battle and supply lifecycle; mocked clocks/native calls, no live verification.");
 }
 static void Battle(){
  Setup(typeof(BattleIntegrationTests));Call("ResetBattleState");
  NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
  var war=Fixture<NJHDEHLHENN>(typeof(BattleIntegrationTests),"war");
  var coverage=Field<ConfigEntry<float>>("battleCoverage");float previous=coverage.Value;coverage.Value=1;
  try{
   Wa_Controller.m_myCurSoldierNum=Wa_Controller.m_enemyCurSoldierNum=10000;
   Call("Assess",war,0d,true);var state=Field<IDictionary>("battles")[war.Pointer];var next=Data<double[]>(state,"SideNext");
   Check(next[0]==2&&next[1]==6,"direct battle evaluates allied at two and NPC at six simulation seconds");
   Call("Assess",war,2d,true);Check(next[0]==4&&next[1]==6,"NPC not reevaluated by allied tick");
   Call("Assess",war,4d,true);Check(next[0]==6&&next[1]==6,"NPC second allied tick still skipped");
   Call("Assess",war,6d,true);Check(next[0]==8&&next[1]==12,"NPC resumes at six simulation seconds");
   // An already ordered NPC army is adopted even while its decision is gated.
   var t=new Wo_DeployTeamCtrl{m_id=8991,m_belongPower="enemy",m_targetWarInfo=war,m_deployState=AKOPKFPKJPP.Expedition,m_assembleCity="enemyCity"};
   t.m_listMovePos.Add(war.GBHGENNMJCK);ICGAEMLACJG.HFJIECIBFKB[t.m_id]=t;
   Call("Assess",war,40d,true);
   Check(Field<IDictionary>("missions").Contains(t.Pointer),"already marching NPC support is registered during decision cooldown");
   typeof(StrategicSupport).GetField("battleClock",F).SetValue(null,40d);Call("AdvanceMissions",war,1f);
   Check(t.Moves==1&&t.Position.x==2,"NPC marching speed remains unchanged");
   NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Call("Assess",war,50d,true);
   Check(next[0]==56&&next[1]==52,"changing current faction changes both side intervals");
  }finally{coverage.Value=previous;}
 }
 static void Supply(){
  Setup(typeof(SupplyTests));NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
  var war=Fixture<NJHDEHLHENN>(typeof(SupplyTests),"war");Call("SupplyTick",war,0d,true);
  var state=Field<IDictionary>("foodBattles")[war.Pointer];var next=Data<double[]>(state,"NextDecision");
  Check(next[0]==5&&next[1]==15,"supply decisions allied five NPC fifteen seconds");
  Call("SupplyTick",war,5d,true);Check(next[0]==10&&next[1]==15,"NPC supply reassessment skipped between intervals");
  Call("SupplyTick",war,15d,true);Check(next[0]==20&&next[1]==30,"NPC supply reassessment resumes at fifteen");
  // Retire so this pending allied delivery now belongs to an NPC faction.
  NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="";
  var army=Fixture<Wo_DeployTeamCtrl>(typeof(SupplyTests),"army");army.Food=0;
  Call("SupplyTick",war,24d,true);Check(army.Food==1200,"pending supply arrives at original ETA after faction reclassification");
 }
}
