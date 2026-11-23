using System;using System.Collections.Generic;using System.Reflection;using System.Linq;
public static class Manual144Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int n;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethods(F).Single(x=>x.Name==name&&x.GetParameters().Length==args.Length&&(name!="ManualProtected"||x.GetParameters()[0].ParameterType==args[0].GetType())).Invoke(null,args);
 static void Check(bool ok,string why){n++;if(!ok)throw new Exception("Manual144: "+why);}
 static void Setup()=>typeof(Manual98Tests).GetMethod("Setup",F).Invoke(null,null);
 static Wo_DeployTeamCtrl Army(int id=24995){var h=new CLAGKDECFID{OJPECMNINNA="manual"+id,LMPMNLLGOEC=new(){LMMNKIOAGLC="F",CNOMNBCPJDI="CS1034"}};var a=new DHKINGPDKJJ{OJPECMNINNA=id,LMMNKIOAGLC="F",FGHFOLKBHDL=360000};return new(){m_id=id,m_belongPower="F",m_assembleCity="CS1034",m_targetCity="CS1033",m_deployState=AKOPKFPKJPP.Assemble,m_listHero=new(){h},m_dicDeployArmy=new(){{h.OJPECMNINNA,new(){a}}}};}
 static bool Protected(object t)=>(bool)Call("ManualProtected",t);
 static object Begin(){object[] a={null};Call("BeginManualDeploy",a);return a[0];}
 static void Commit(Wo_DeployTeamCtrl t){var scope=Begin();try{Call("AfterManualAssembly",t);}finally{Call("EndManualDeploy",scope);}}
 public static void Run(){
  Setup();var t=Army();Call("AfterManualAssembly",t);Check(!Protected(t),"ordinary leader refresh does not adopt AI army");
  object[] click={0};Call("BeginManualInput",click);try{Call("AfterManualAssembly",t);Check(!Protected(t),"opening UI or HUD alone does not claim AI army");}finally{Call("EndManualInput",click[0]);}
  // Native UI writes fields and refreshes the leader, then yields. No team
  // command method is called until StartDeploy resumes outside the UI scope.
  Commit(t);Check(Protected(t),"auto-assembly committed before delayed StartDeploy");Check(Protected(t.m_listHero[0])&&Protected(t.m_dicDeployArmy[t.m_listHero[0].OJPECMNINNA][0]),"officers and 360000 soldiers protected together");
  NEINKOOBJIH.MIPFHCKDJOK=170;Check((bool)Call("GuardAttackStart",t)&&!t.Dismissed,"assembly remains protected beyond 30 days");t.m_deployState=AKOPKFPKJPP.Expedition;
  object[] candidate={t,null};Check(!(bool)Call("MarchAttackCandidate",candidate),"automatic strength withdrawal excludes manual invasion");
  foreach(var phase in (IEnumerable<string>)Call("CheckAttackMarch",t,t.m_id,"CS1033")){}Check(t.m_targetCity=="CS1033"&&t.m_deployState==AKOPKFPKJPP.Expedition&&!t.Dismissed,"queued march recheck cannot divert manual invasion");
  Call("ResetManualOrders");Check(Protected(t),"assembly authority survives saved-flag restoration");
  t.m_targetCity="";t.m_deployState=AKOPKFPKJPP.Camp;Call("ObserveManualTeam",t);NEINKOOBJIH.MIPFHCKDJOK=174.999;Check(Protected(t),"full five days after actual completion");NEINKOOBJIH.MIPFHCKDJOK=175;Check(!Protected(t),"released at five days");
  Setup();t=Army();Commit(t);object[] released={t,null};Call("BeforeManualTeamRelease",released);var hero=t.m_listHero[0];var troop=t.m_dicDeployArmy[hero.OJPECMNINNA][0];t.m_listHero.Clear();t.m_dicDeployArmy.Clear();Call("AfterManualTeamRelease",released[1]);Check(!Protected(t)&&!Protected(hero)&&!Protected(troop),"explicit disband remains immediate release");
  Setup();t=Army();var outer=Begin();var inner=Begin();Call("EndManualDeploy",inner);Call("AfterManualAssembly",t);Call("EndManualDeploy",outer);Check(Protected(t),"nested confirmation restores parent scope");var ai=Army(24996);Call("AfterManualAssembly",ai);Check(!Protected(ai),"scope ends after coroutine step");
  Setup();t=Army();outer=Begin();Call("BeginManualGovernor");try{Call("AfterManualAssembly",t);}finally{Call("EndManualGovernor");Call("EndManualDeploy",outer);}Check(!Protected(t),"native AI nested in confirmation does not acquire authority");
  Setup();t=Army();outer=Begin();try{throw new Exception("cancel");}catch{}finally{Call("EndManualDeploy",outer);}Call("AfterManualAssembly",t);Check(!Protected(t),"exception/cancel leaves no authority scope");
  Setup();t=Army();t.m_listHero.Clear();Commit(t);Check(!Protected(t),"uncommitted empty roster not protected");
  Setup();t=Army();t.m_belongPower="E";Commit(t);Check(!Protected(t),"enemy team not claimed by player");
  Setup();t=Army();click=new object[]{0};Call("BeginManualInput",click);try{Check((bool)Call("GuardAttackStart",t)&&Protected(t),"synchronous manual order is captured before AI guard");}finally{Call("EndManualInput",click[0]);}
  Setup();for(int i=0;i<100;i++){var scope=Begin();Call("EndManualDeploy",scope);}t=Army();Call("AfterManualAssembly",t);Check(!Protected(t),"100 yielded frames do not leak authority to later AI work");
  // Use a fully valid autonomous attack fixture as the positive control, so
  // the manual exclusion cannot pass just because the city/faction is absent.
  typeof(Offense141Tests).GetMethod("Setup",F).Invoke(null,null);
  t=(Wo_DeployTeamCtrl)typeof(Offense141Tests).GetField("army",F).GetValue(null);
  t.m_listHero.Add(new(){OJPECMNINNA="commander",LMPMNLLGOEC=new(){LMMNKIOAGLC="F"}});
  var goal=t.m_targetCity;candidate=new object[]{t,null};Check((bool)Call("MarchAttackCandidate",candidate),"positive control autonomous march is eligible");
  NEINKOOBJIH.POMNMBGDDIN=new(){OJPECMNINNA="player",LMPMNLLGOEC=new(){LMMNKIOAGLC="F"}};
  typeof(Offense141Tests).GetMethod("Enemy",F).Invoke(null,new object[]{14400,200000,140000,"enemy"});
  using(var check=((IEnumerable<string>)Call("CheckAttackMarch",t,t.m_id,goal)).GetEnumerator()){
   Check(check.MoveNext(),"automatic check queued before manual override");Commit(t);
   while(check.MoveNext()){}Check(t.m_targetCity==goal&&t.m_deployState==AKOPKFPKJPP.Expedition&&!t.Dismissed,"real queued check respects newly committed manual authority");
  }
  Call("ResetWork");Call("ResetDefense");Call("ResetOffense");Setup();
  Console.WriteLine($"PASS Manual144 {n} native confirmation sequence, delayed departure, queued withdrawal, five-day grace and scope isolation checks; native actions mocked.");
 }
}
