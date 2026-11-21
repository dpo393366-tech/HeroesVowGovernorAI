using System;using System.Collections.Generic;using System.Reflection;using UnityEngine;using UnityEngine.AI;
public static class FieldBattle89Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool v,string why){checks++;if(!v)throw new Exception("R89 field battle: "+why);}
 public static void Run(){
  typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetBattleState");Call("ResetDefense");Call("ResetWork");
  var source=MICGPAMNDKM.IJPKEMPIFIH["source"];var enemy=MICGPAMNDKM.IJPKEMPIFIH["enemyCity"];var f=NEINKOOBJIH.COCMABJFOFH["F"];
  var war=new NJHDEHLHENN{HIENOINGBCN="field",GBHGENNMJCK=new(20),IJGLCFJOOIK=null};enemy.BMBMKPGEHBH=new(20);
  NavMesh.Blocked=false;NavMesh.Detour=0;AOHIBKLOAEI.RegionAt=p=>p.x>10?enemy:source;
  object[] old={source.BMBMKPGEHBH,war.GBHGENNMJCK,f,2f,0d,null};
  Check(!(bool)Call("RouteSeconds",old),"reproduces missing siege-city field rejecting hostile battlefield");
  var allowed=(string)Call("BattleTransitRegion",war,f);Check(allowed==enemy.OJPECMNINNA,"actual coordinate supplies field battle region");
  object[] fixedRoute={source.BMBMKPGEHBH,war.GBHGENNMJCK,f,2f,0d,allowed};
  Check((bool)Call("RouteSeconds",fixedRoute),"same source and coordinates now have legal reinforcement route");
  object[] supply={source,war,"F",0d};Check((bool)Call("SupplyRoute",supply),"food transport recognizes same field battle region");
  var third=new AFAGFHJONFJ{OJPECMNINNA="neutral"};third.LMPMNLLGOEC.LMMNKIOAGLC="third";
  AOHIBKLOAEI.RegionAt=p=>p.x>10?enemy:p.x>3?third:source;
  source.MEKIDAONKMD[enemy.OJPECMNINNA]=new(){EJMDIPFNGMG=new(){third}};
  Check(!(bool)Call("RouteSeconds",fixedRoute),"native third-party transit remains blocked before battlefield");
  Check(!(bool)Call("SupplyRoute",supply),"food transport likewise rejects unauthorized third-party transit");
  AOHIBKLOAEI.RegionAt=p=>third;Check(Call("BattleTransitRegion",war,f)==null,"field battle does not authorize unrelated region owner");
  war.IJGLCFJOOIK=enemy;Check((string)Call("BattleTransitRegion",war,f)==enemy.OJPECMNINNA,"ordinary siege-city handling retained");
  Check(Call("BattleTransitRegion",war,new MKMOJNAJKJE{IBGCLCAIBOL="outsider"})==null,"nonparticipant cannot acquire battlefield permission");
  AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=null;Wa_Controller.m_warInfo=null;Call("ResetBattleState");Call("ResetDefense");Call("ResetWork");
  Console.WriteLine($"PASS R89 {checks} field battle reinforcement/supply territory checks; native navigation mocked.");
 }
}
