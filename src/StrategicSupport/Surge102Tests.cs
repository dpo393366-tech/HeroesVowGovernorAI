using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;
public static class Surge102Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int n;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string why){n++;if(!ok)throw new Exception("Surge102: "+why);}
 static T Field<T>(Type type,string name)=>(T)type.GetField(name,F).GetValue(null);
 public static void Run(){
  var allies=new List<ReinforcementTimeline.Wave>{new(30,1000000),new(60,1000000),new(80,1000000)};
  var foes=new List<ReinforcementTimeline.Wave>{new(50,1733286)};
  int shortNeed=ReinforcementTimeline.Need(536205,0,28888.1,0,90,90,allies,foes,1.15,50,out _,out _,true);
  int farNeed=ReinforcementTimeline.Need(536205,0,28888.1,0,170,170,allies,foes,1.15,50,out _,out _,true);
  Check(shortNeed==0&&farNeed==0,"already sufficient precontact waves do not trigger world-wide late deployments from unlimited guessed attrition");
  Check(ReinforcementTimeline.Need(10000,30000,100,0,120,120,new List<ReinforcementTimeline.Wave>(),new List<ReinforcementTimeline.Wave>(),1.15,0,out string why,out _) ==0 && why=="collapse-before-wave","observed battle losses retain strict deadline");
  Check(ReinforcementTimeline.Need(10000,0,500,0,30,60,new List<ReinforcementTimeline.Wave>(),new List<ReinforcementTimeline.Wave>{new(10,50000)},1.15,10,out _,out _,true)>0,"real insufficient defense still requests support");
  typeof(IntegrationTests).GetMethod("Prepare",F).Invoke(null,null);Call("Ready");Call("ResetDefense");Call("ResetWork");
  var f=NEINKOOBJIH.COCMABJFOFH["F"];var source=Field<AFAGFHJONFJ>(typeof(IntegrationTests),"source");var city=MICGPAMNDKM.IJPKEMPIFIH["target"];
  foreach(var step in (IEnumerable<string>)Call("DefenseSteps",f)){}
  var t=source.Last;Check(t!=null,"fixture has real production defense order");
  var orders=Field<IDictionary>(typeof(StrategicSupport),"defenseOrders");var o=orders[t.Pointer];
  t.m_deployState=AKOPKFPKJPP.Expedition;t.m_listMovePos.Clear();t.m_listMovePos.Add(city.BMBMKPGEHBH);t.Position=new(5);t.m_targetCity="";
  NEINKOOBJIH.MIPFHCKDJOK+=10;
  var friends=new List<ReinforcementTimeline.Wave>();var enemies=new List<ReinforcementTimeline.Wave>();
  Call("RefreshDefenseWaves",f,city,new List<Wo_DeployTeamCtrl>{t},friends,enemies);
  Check(friends.Count==1,"moving army stays counted despite ten-day delayed watcher");
  NEINKOOBJIH.MIPFHCKDJOK+=3;Call("RefreshDefenseWaves",f,city,new List<Wo_DeployTeamCtrl>{t},friends,enemies);
  Check(friends.Count==0,"actually stationary army is still excluded after progress deadline");
  var war=new NJHDEHLHENN{IJGLCFJOOIK=null,EMMJBPMNFAK=new[]{"F","enemy"},GBHGENNMJCK=city.BMBMKPGEHBH};
  var enemy=ICGAEMLACJG.HFJIECIBFKB[1];enemy.m_targetCity="";enemy.m_warInfo=war;
  AOHIBKLOAEI.RegionAt=p=>city;ICGAEMLACJG.FKKCPDEFODO[city.OJPECMNINNA]=new(){City=city};
  Check((bool)Call("DefenseThreat",city,f,new List<Wo_DeployTeamCtrl>{enemy}),"field battle preserves threat when siege city reference is null");
  Call("CheckDefenseOrder",o);Check(orders.Contains(t.Pointer)&&t.m_targetWarInfo==war,"existing rally joins field battle instead of threat-ended camp");
  var elsewhere=new AFAGFHJONFJ{OJPECMNINNA="other-region"};elsewhere.LMPMNLLGOEC.LMMNKIOAGLC="F";AOHIBKLOAEI.RegionAt=p=>elsewhere;
  Check(!(bool)Call("DefenseBattleMatches",war,city,f),"unrelated field battle is not mistaken for destination defense");
  AOHIBKLOAEI.RegionAt=p=>city;war.EMMJBPMNFAK=new[]{"other","enemy"};Check(!(bool)Call("DefenseBattleMatches",war,city,f),"nonparticipant cannot join unrelated battle");
  typeof(Intercept90Tests).GetMethod("Setup",F).Invoke(null,null);
  var interceptor=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"army");enemy=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"enemy");
  var remote=new AFAGFHJONFJ{OJPECMNINNA="remote"};remote.LMPMNLLGOEC.LMMNKIOAGLC="enemy";AOHIBKLOAEI.RegionAt=p=>remote;
  Call("RecoverHostileStandoff",interceptor);Check(!interceptor.m_responseToPlayer,"stale remote pursuit is released, not repeatedly marched into camp");
  typeof(Intercept90Tests).GetMethod("Setup",F).Invoke(null,null);interceptor=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"army");enemy=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"enemy");AOHIBKLOAEI.RegionAt=p=>remote;
  interceptor.m_deployState=AKOPKFPKJPP.Assemble;Check(!(bool)Call("GuardFailedIntercept",interceptor)&&interceptor.Dismissed,"remote no-purpose interceptor is rejected before departure");
  typeof(Intercept90Tests).GetMethod("Setup",F).Invoke(null,null);interceptor=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"army");enemy=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"enemy");AOHIBKLOAEI.RegionAt=p=>remote;enemy.m_targetCity=interceptor.m_assembleCity;
  object[] args={interceptor,enemy,null};Check((bool)Call("InterceptRelevant",args),"enemy actually attacking own city remains eligible across regions");
  AOHIBKLOAEI.RegionAt=null;Lifecycle();Call("ResetWork");Console.WriteLine("PASS Surge102 "+n+" production regressions; native movement mocked, live outcomes pending.");
 }
 static void Lifecycle(){
  typeof(Intercept90Tests).GetMethod("Setup",F).Invoke(null,null);
  var army=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"army");var enemy=Field<Wo_DeployTeamCtrl>(typeof(Intercept90Tests),"enemy");var faction=NEINKOOBJIH.COCMABJFOFH[army.m_belongPower];
  army.m_responseTeam=null;Call("RecoverHostileStandoff",army);
  Check(!army.m_responseToPlayer&&(bool)Call("CampaignControlled",new object[]{army,null}),"save-loaded null response flag no longer excludes army from all recovery owners");
  faction.CFLABFGBJLE.Add(army);var original=faction.CFLABFGBJLE;
  object[] args={faction,null};Call("BeforeNativeTeamRefresh",args);
  // Model the independently disassembled second native loop, including its
  // counterintuitive null-target -> true flag assignment.
  foreach(var t in faction.CFLABFGBJLE)if(!t.m_responseTeam){t.m_responseTeam=null;t.m_responseToPlayer=true;}
  Call("AfterNativeTeamRefresh",args[1]);
  Check(!army.m_responseToPlayer&&ReferenceEquals(faction.CFLABFGBJLE,original)&&original.Count==1,"cancelled interception cannot be resurrected by native second loop; registry restored");
  army.m_responseToPlayer=true;army.m_responseTeam=enemy;army.m_attackTarget=enemy;army.m_deployState=AKOPKFPKJPP.Expedition;army.m_listMovePos.Add(new(90));
  Check(!(bool)Call("NativeResponseRefreshAllowed",army),"active pursuit does not get redundant SetAttackTarget state reset");
  army.m_attackTarget=null;army.m_listMovePos.Clear();army.m_deployState=AKOPKFPKJPP.Camp;
  Check((bool)Call("NativeResponseRefreshAllowed",army),"valid ordinary automatic response remains enabled");
  army.m_targetWarInfo=new();Check(!(bool)Call("NativeResponseRefreshAllowed",army),"response registry cannot erase committed battle destination");
  army.m_responseTeam=null;Call("RepairOrphanResponse",army);Check(army.m_targetWarInfo!=null&&!army.m_responseToPlayer,"orphan repair preserves real relief mission");
  army.m_isPrivate=true;army.m_responseToPlayer=true;Call("RepairOrphanResponse",army);Check(army.m_responseToPlayer,"private follow is not adopted");
  army.m_isPrivate=false;army.m_responseTeam=enemy;enemy.m_belongPower=army.m_belongPower;Call("RepairOrphanResponse",army);Check(army.m_responseToPlayer,"registered friendly follow is preserved");
  args=new object[]{faction,null};Call("BeforeNativeTeamRefresh",args);try{throw new Exception("injected native failure");}catch{}finally{Call("AfterNativeTeamRefresh",args[1]);}
  Check(ReferenceEquals(faction.CFLABFGBJLE,original),"response registry restored after native failure too");
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");
  army=Field<Wo_DeployTeamCtrl>(typeof(Campaign81Tests),"t");var home=Field<AFAGFHJONFJ>(typeof(Campaign81Tests),"home");faction=NEINKOOBJIH.COCMABJFOFH[army.m_belongPower];army.Combat=1;
  typeof(Campaign81Tests).GetMethod("Run",F).Invoke(null,null);Check(army.m_isEnterCity,"return fixture committed before native startup");
  army.MoveToCity(ICGAEMLACJG.FKKCPDEFODO[home.OJPECMNINNA],false,false);army.m_listMovePos.Clear();
  Call("RouteNewRelief",army);Check(army.m_isEnterCity&&army.m_targetCity==home.OJPECMNINNA&&army.m_listMovePos.Count>0,"delayed split model StartDeploy restores entry rather than zero-path orphan");
  var incoming=new Wo_DeployTeamCtrl{m_id=12345,m_belongPower=army.m_belongPower,m_targetCity=home.OJPECMNINNA,m_isEnterCity=true,m_deployState=AKOPKFPKJPP.Expedition};
  for(int i=0;i<50;i++)incoming.m_dicDeployArmy["units"+i]=new(){new()};ICGAEMLACJG.HFJIECIBFKB[incoming.m_id]=incoming;
  Check((int)Call("CapacityRoom",home,new List<Wo_DeployTeamCtrl>{army,incoming},army)==0,"planning still reserves all incoming units");
  Check((bool)Call("GuardCampaignEntry",army,home),"actual arrival admits into physical room despite overbooked future arrivals");
  home.Capacity=0;Call("ResetWork");Check(!(bool)Call("GuardCampaignEntry",army,home)&&string.IsNullOrEmpty(army.m_targetCity)&&!army.m_isEnterCity&&Field<WorkQueue>(typeof(StrategicSupport),"work").Count>0,"real full arrival releases reservation and immediately queues split/replan");
 }
}
