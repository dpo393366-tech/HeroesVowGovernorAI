using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class Audit105Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int n;
 static object Call(string method,params object[] args)=>typeof(StrategicSupport).GetMethod(method,F).Invoke(null,args);
 static object Fixture(string method,params object[] args)=>typeof(RouteOrder94Tests).GetMethod(method,F).Invoke(null,args);
 static T Field<T>(string name)=>(T)typeof(StrategicSupport).GetField(name,F).GetValue(null);
 static void Check(bool ok,string why){n++;if(!ok)throw new Exception("Audit105: "+why);}
 static IEnumerator<string> Steps(Action count,int limit){for(int i=0;i<limit;i++){count();yield return "step";}}
 public static void Run(){
  Fixture("Setup");var source=(AFAGFHJONFJ)typeof(RouteOrder94Tests).GetField("source",F).GetValue(null);var war=(NJHDEHLHENN)typeof(RouteOrder94Tests).GetField("war",F).GetValue(null);var f=NEINKOOBJIH.COCMABJFOFH["F"];
  string key="first",stamp=(string)Call("RoadPolicyStamp",f);NavMesh.Calls105=0;
  foreach(var phase in (IEnumerable<string>)Call("BattleRouteQuote",source,war,f,key)){}
  int cold=NavMesh.Calls105;Check(cold>0,"cold quote calculates real path adapter");
  var saved=Field<IDictionary>("savedBattleRoads");foreach(var entry in saved.Values)entry.GetType().GetField("Checked").SetValue(entry,Environment.TickCount64-600000);
  for(int i=0;i<200;i++){object[] args={source,war,f,key,stamp,0d};Check((bool)Call("SavedBattleQuote",args)&&(double)args[5]==60,"expired old TTL does not block valid saved distance reuse");}
  Check(NavMesh.Calls105==cold,"200 repeated decisions perform zero new native path calculations");
  var nextWar=new NJHDEHLHENN{HIENOINGBCN="another war",GBHGENNMJCK=war.GBHGENNMJCK};object[] cross={source,nextWar,f,"another",stamp,0d};Check((bool)Call("SavedBattleQuote",cross),"different battle at same location reuses road distance");
  Wo_DeployTeamCtrl.Speed105=4;cross[5]=0d;Check((bool)Call("SavedBattleQuote",cross)&&(double)cross[5]==35,"speed changes update ETA without recalculating roads");Wo_DeployTeamCtrl.Speed105=2;
  nextWar.GBHGENNMJCK=new(110);Check(!(bool)Call("SavedBattleQuote",cross),"moving battlefield cannot reuse old endpoint quote");
  source.LMPMNLLGOEC.LMMNKIOAGLC="enemy";object[] changed={source,war,f,key,Call("RoadPolicyStamp",f),0d};Check(!(bool)Call("SavedBattleQuote",changed),"ownership change invalidates stored passage immediately");source.LMPMNLLGOEC.LMMNKIOAGLC="F";
  NavMesh.Blocked=true;Call("InvalidateBattleRoad",source,war,f);foreach(var phase in (IEnumerable<string>)Call("BattleRouteQuote",source,war,f,key)){}
  object[] failed={source,war,f,key,Call("RoadPolicyStamp",f),0d};Check((bool)Call("SavedBattleQuote",failed)&&double.IsPositiveInfinity((double)failed[5]),"fresh physical blockage saved as unreachable");
  foreach(var entry in saved.Values)entry.GetType().GetField("Checked").SetValue(entry,Environment.TickCount64-4000);
  Check(!(bool)Call("SavedBattleQuote",failed),"negative result expires so repaired path can be found");NavMesh.Blocked=false;
  Call("ResetDefense");Check(saved.Count==0,"save/world reset removes route cache");
  var complete=((Wo_DeployTeamCtrl,AFAGFHJONFJ,AFAGFHJONFJ))Fixture("Completed");var t=complete.Item1;var city=complete.Item2;t.Position=new(city.BMBMKPGEHBH.x+5.503f);t.m_isStatic=true;
  Check((bool)Call("AdoptCompletedCityOrder",t),"Luoyang 5.5-distance stationary survivor in captured region enters recovery");
  complete=((Wo_DeployTeamCtrl,AFAGFHJONFJ,AFAGFHJONFJ))Fixture("Completed");t=complete.Item1;city=complete.Item2;t.Position=new(city.BMBMKPGEHBH.x+5.503f);t.m_isStatic=false;
  Check(!(bool)Call("AdoptCompletedCityOrder",t),"non-stationary active order remains untouched");t.m_isStatic=true;t.m_listMovePos.Add(new(110));Check(!(bool)Call("AdoptCompletedCityOrder",t),"remaining path prevents premature completion");t.m_listMovePos.Clear();AOHIBKLOAEI.RegionAt=p=>complete.Item3;Check(!(bool)Call("AdoptCompletedCityOrder",t),"different native region cannot be treated as captured destination");AOHIBKLOAEI.RegionAt=null;
  complete=((Wo_DeployTeamCtrl,AFAGFHJONFJ,AFAGFHJONFJ))Fixture("Completed");t=complete.Item1;city=complete.Item2;t.Position=new(city.BMBMKPGEHBH.x+5.503f);t.m_isStatic=true;
  NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC=t.m_belongPower;MLGOLGGHLMB.CBEGDDKANAO=new();NEINKOOBJIH.MIPFHCKDJOK=100;
  string manual="local.heroesvow.governor/manual/v1/"+t.m_belongPower+"/team/"+t.m_id;MLGOLGGHLMB.CBEGDDKANAO[manual]=-1;
  Call("ObserveManualTeam",t);Check(MLGOLGGHLMB.CBEGDDKANAO[manual]==105001,"captured-region completion starts manual five-day grace");
  Check(!(bool)Call("AdoptCompletedCityOrder",t),"recovery never takes manual army during grace");NEINKOOBJIH.MIPFHCKDJOK=105;
  Check((bool)Call("AdoptCompletedCityOrder",t),"stationary survivor recovers exactly when manual grace expires");AOHIBKLOAEI.RegionAt=null;
  for(int seed=0;seed<10;seed++){
   var q=new WorkQueue();int combat=0,ordinary=0,urgent=0,recruit=0;
   for(int i=0;i<250;i++)q.Add("campaign:"+i,()=>Steps(()=>urgent++,50));
   for(int i=0;i<150;i++)q.Add("city:"+i,()=>Steps(()=>ordinary++,50));
   q.Add("recruit-queue:F",()=>Steps(()=>recruit++,500));q.Add("battle-support-plan:F",()=>Steps(()=>combat++,100));
   q.Pump(1000,400,(s,ms)=>{},e=>throw e);
   Check(combat==100&&urgent>0&&ordinary>0&&recruit>0,"400 concurrent tasks cannot starve support or other work");q.Clear();Check(q.Count==0,"critical tasks clear with old save");
  }
  Call("ResetWork");Call("ResetDefense");Console.WriteLine("PASS Audit105 "+n+" cache reuse/invalidation, current speed, captured-city recovery and saturated scheduler assertions; native navigation mocked.");
 }
}
