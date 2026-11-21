using System;using System.Linq;using System.Reflection;using System.Collections.Generic;using UnityEngine;
public static class Allocation95Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string reason){checks++;if(!ok)throw new Exception("Allocation95: "+reason);}
 public static void Run(){
  var none=Array.Empty<FrontAllocationPolicy.Arrival>();var baseline=FrontAllocationPolicy.Available(100000,48000,100000,48000,0,30,none);
  Check(baseline.Men==42400&&baseline.Power==42400,"100k versus 48k permits partial release while retaining 1.2 advantage");
  var wave=FrontAllocationPolicy.Available(100000,48000,100000,48000,0,30,new[]{new FrontAllocationPolicy.Arrival(200,40000,40000)});
  Check(wave.Men==0&&wave.Power==0&&wave.Horizon==210,"distant enemy support included beyond initial horizon; apparent victory revoked");
  var losses=FrontAllocationPolicy.Available(100000,48000,100000,48000,1000,50,none);Check(losses.Men==0,"rapid own casualties revoke donation without assuming enemy casualties");
  var rng=new Random(950);for(int i=0;i<250;i++){double own=rng.Next(1000,200000),enemy=rng.Next(1000,100000);var b=FrontAllocationPolicy.Available(own,enemy,own,enemy,0,30,none);var c=FrontAllocationPolicy.Available(own,enemy,own,enemy,0,30,new[]{new FrontAllocationPolicy.Arrival(100,rng.Next(1,10000),1000)});Check(c.Men<=b.Men&&c.Power<=b.Power,"new enemy wave can never increase withdrawable resources");}
  typeof(GovernorFeatureTests).GetMethod("Setup",F).Invoke(null,null);var f=(MKMOJNAJKJE)typeof(GovernorFeatureTests).GetField("faction",F).GetValue(null);var city=(AFAGFHJONFJ)typeof(GovernorFeatureTests).GetField("city",F).GetValue(null);
  f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};ICGAEMLACJG.HFJIECIBFKB.Clear();Call("ResetWork");Call("ResetDefense");Call("ResetBattleState");Wa_Controller.m_warInfo=null;UnityEngine.AI.NavMesh.Blocked=false;UnityEngine.AI.NavMesh.Detour=0;AOHIBKLOAEI.RegionAt=null;AOHIBKLOAEI.Region=city;
  Wo_DeployTeamCtrl Army(int id,string owner,int men,int power,float x){var t=new Wo_DeployTeamCtrl{m_id=id,m_belongPower=owner,m_assembleCity=city.OJPECMNINNA,Combat=power,Food=1000000,Position=new(x),m_deployState=AKOPKFPKJPP.Expedition};t.gameObject.SetActive(true);t.m_dicDeployArmy["unit"]=new(){new(){FGHFOLKBHDL=men}};ICGAEMLACJG.HFJIECIBFKB[id]=t;return t;}
  var victim=Army(1625,f.IBGCLCAIBOL,10000,10000,10);var enemyArmy=Army(1794,"enemy",21361,21361,20);enemyArmy.m_attackTarget=victim;enemyArmy.m_listMovePos.Add(victim.Position);
  victim.gameObject.SetActive(true);enemyArmy.gameObject.SetActive(true);
  object[] args={enemyArmy,f,null};Check((bool)Call("FieldThreat",args)&&ReferenceEquals(args[2],victim),"live Zhuge case detected through attackTarget even when targetCity is empty");
  var chasingNeed=(int)Call("FieldNeed",enemyArmy,victim,f,30d,null);Check(chasingNeed>0,"army pursuit has positive emergency demand");
  var donor=Army(1642,f.IBGCLCAIBOL,20000,20000,15);var war=new NJHDEHLHENN{HIENOINGBCN="Yeoreung",EMMJBPMNFAK=new[]{f.IBGCLCAIBOL,"enemy"},FGHFOLKBHDL=new[]{100000,48000}};donor.m_targetWarInfo=war;
  Army(1519,f.IBGCLCAIBOL,100000,100000,100).m_warInfo=war;Army(1592,"enemy",48000,48000,100).m_warInfo=war;
  Check((bool)Call("SpareMarch",donor,f),"already present Yeoreung force suffices without Dong army");
  Check((bool)Call("InstallFieldRelief",donor,enemyArmy,victim,f)&&donor.m_attackTarget==enemyArmy&&donor.m_targetWarInfo==null,"real production command changes surplus incoming army to urgent interception");
  Check(donor.m_listMovePos.Count>0&&!donor.m_isEnterCity,"interception installs validated world path without entering a full city");
  Check((int)Call("FieldNeed",enemyArmy,victim,f,30d,null)<chasingNeed,"committed mobile relief reduces remaining shared demand");
  var fieldOrders=(System.Collections.IDictionary)typeof(StrategicSupport).GetField("fieldReliefOrders",F).GetValue(null);var pending=fieldOrders[donor.Pointer];pending.GetType().GetField("AwaitingDeparture").SetValue(pending,true);donor.m_attackTarget=null;donor.m_listMovePos.Clear();
  Call("RouteNewRelief",donor);Check(donor.m_attackTarget==enemyArmy&&donor.m_listMovePos.Count>0,"native assembly departure restores an otherwise cleared pursuit order and validated road");
  pending=fieldOrders[donor.Pointer];pending.GetType().GetField("AwaitingDeparture").SetValue(pending,true);donor.m_targetCity="manual-city";donor.m_attackTarget=null;Call("RouteNewRelief",donor);
  Check(!fieldOrders.Contains(donor.Pointer)&&donor.m_targetCity=="manual-city"&&donor.m_attackTarget==null,"departure never overwrites a replacement manual city order");donor.m_targetCity="";
  var old=enemyArmy;ICGAEMLACJG.HFJIECIBFKB[1794]=new(){m_id=1794,m_belongPower="enemy"};args=new object[]{old,f,null};Check(!(bool)Call("FieldThreat",args),"pooled or removed enemy identity invalidates threat");
  ICGAEMLACJG.HFJIECIBFKB[1794]=old;victim.m_warInfo=war;args=new object[]{old,f,null};Check(!(bool)Call("FieldThreat",args),"actual battle handoff does not duplicate precontact support");victim.m_warInfo=null;
  var reinforce=Army(1900,"enemy",40000,40000,20);reinforce.m_targetWarInfo=war;reinforce.m_listMovePos.Add(war.GBHGENNMJCK);donor.m_targetWarInfo=war;
  Check(!(bool)Call("SpareMarch",donor,f),"changing enemy reinforcements immediately cancels spare classification");
  donor.m_isPrivate=true;Check(!(bool)Call("MobileCandidate",donor,f),"private army cannot be reassigned");
  Call("ResetDefense");Call("ResetWork");ICGAEMLACJG.HFJIECIBFKB.Clear();
  var crisis=new NJHDEHLHENN{HIENOINGBCN="reversal95",EMMJBPMNFAK=new[]{f.IBGCLCAIBOL,"enemy"},FGHFOLKBHDL=new[]{10000,20000},GBHGENNMJCK=new(4)};
  Army(9501,f.IBGCLCAIBOL,10000,10000,4).m_warInfo=crisis;Army(9502,"enemy",20000,20000,4).m_warInfo=crisis;
  var reserve=Army(9503,f.IBGCLCAIBOL,30000,30000,0);reserve.m_dicDeployArmy.Clear();reserve.m_deployState=AKOPKFPKJPP.Camp;
  for(int i=0;i<6;i++){var h=new CLAGKDECFID{OJPECMNINNA="reserve95-"+i};reserve.m_listHero.Add(h);reserve.m_dicDeployArmy[h.OJPECMNINNA]=new(){new(){FGHFOLKBHDL=5000,CombatValue=5000}};}
  Call("RememberCampaign",reserve);var gs=(System.Collections.IDictionary)typeof(StrategicSupport).GetField("campaignGoals",F).GetValue(null);var g=gs[reserve.Pointer];g.GetType().GetField("Returning").SetValue(g,true);
  foreach(var phase in (IEnumerable<string>)Call("ReassignReserveSteps",f)){}
  var sent=ICGAEMLACJG.HFJIECIBFKB.Values.Where(t=>t.m_targetWarInfo==crisis).ToList();
  Check(sent.Count==1&&sent[0].m_id!=reserve.m_id&&reserve.GetSoldierNum(false)>0,"reversal reuses a returning reserve and sends only a necessary split");
  Check(sent[0].GetSoldierNum(false)+reserve.GetSoldierNum(false)==30000,"urgent reassignment conserves remaining home-return force");
  foreach(var phase in (IEnumerable<string>)Call("ReassignReserveSteps",f)){}
  Check(ICGAEMLACJG.HFJIECIBFKB.Values.Count(t=>t.m_targetWarInfo==crisis)==1,"committed first wave prevents duplicate promise in later multi-front scan");
  var wave95=Army(9504,"enemy",50000,50000,4);wave95.m_targetWarInfo=crisis;wave95.m_listMovePos.Add(crisis.GBHGENNMJCK);
  foreach(var phase in (IEnumerable<string>)Call("ReassignReserveSteps",f)){}
  Check(reserve.m_targetWarInfo==crisis,"new hostile arrivals revoke return decision and recall remaining reserve");
  // A frontline surplus with no other outstanding battle can release troops
  // to real city capacity; a later threat is handled by the same reserve pass.
  ICGAEMLACJG.HFJIECIBFKB.Clear();Call("ResetDefense");Call("ResetWork");
  var won=new NJHDEHLHENN{HIENOINGBCN="safe-surplus95",EMMJBPMNFAK=new[]{f.IBGCLCAIBOL,"enemy"},FGHFOLKBHDL=new[]{100000,10000},AOKBHBMECBD=new[]{100000,10000}};
  var big=Army(9510,f.IBGCLCAIBOL,100000,100000,0);big.m_dicDeployArmy.Clear();for(int i=0;i<10;i++){var h=new CLAGKDECFID{OJPECMNINNA="big95-"+i};big.m_listHero.Add(h);var a=new DHKINGPDKJJ{FGHFOLKBHDL=10000,CombatValue=10000};big.m_dicDeployArmy[h.OJPECMNINNA]=new(){a};won.KJPPGIOCDID[0].Add(a);}big.m_warInfo=won;
  Army(9511,"enemy",10000,10000,0).m_warInfo=won;city.Capacity=30;city.LMPMNLLGOEC.OEGNGFBCHFL=10000000;city.LMPMNLLGOEC.KJPPGIOCDID.Clear();ICGAEMLACJG.FKKCPDEFODO[city.OJPECMNINNA]=new(){City=city};
  foreach(var phase in (IEnumerable<string>)Call("WorldSurplusReturnSteps",f)){}
  var returning=ICGAEMLACJG.HFJIECIBFKB.Values.Where(t=>t.m_isEnterCity).ToList();
  Check(returning.Count==1&&big.GetSoldierNum(false)>=13000,"winning world battle releases only surplus to a real return destination");
  Check(returning[0].GetSoldierNum(false)+big.GetSoldierNum(false)==100000&&won.FGHFOLKBHDL[0]==big.GetSoldierNum(false),"released troops removed from battle accounting without casualty or duplication");
  Call("ResetDefense");Call("ResetWork");AOHIBKLOAEI.Region=null;
  Console.WriteLine($"PASS Allocation95 {checks} multi-front budget, enemy arrivals, pursuit identity and actual command-path tests; native actions mocked.");
 }
}
