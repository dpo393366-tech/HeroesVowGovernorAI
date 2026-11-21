using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;
public static class Dispatch119Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static AFAGFHJONFJ source,target;static NJHDEHLHENN war;static MKMOJNAJKJE faction;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Field<T>(string n)=>(T)typeof(StrategicSupport).GetField(n,F).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Dispatch119: "+why);}
 static void Setup(){
  typeof(Emergency88Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetManualOrders");MLGOLGGHLMB.CBEGDDKANAO.Clear();
  source=(AFAGFHJONFJ)typeof(BattleIntegrationTests).GetField("source",F).GetValue(null);war=(NJHDEHLHENN)typeof(BattleIntegrationTests).GetField("war",F).GetValue(null);faction=NEINKOOBJIH.COCMABJFOFH["F"];
  NEINKOOBJIH.MIPFHCKDJOK=100;NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC="F";
 }
 static Wo_DeployTeamCtrl Send(){var teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();return (Wo_DeployTeamCtrl)Call("DispatchBattle",source,war,0,10000,30d,false,teams,Call("AttackReserved",teams),new Func<double,bool>(_=>true));}
 static Wo_DeployTeamCtrl Depart(){var t=Send();t.m_deployState=AKOPKFPKJPP.Expedition;Call("RecordAttackDeparture",t,source);Call("RecordDeployResult",t);return t;}
 static void Cancel(Wo_DeployTeamCtrl t){Call("RecordDismissal",t);t.Dismiss(true,false);}
 static void AttackSetup(){typeof(OffensiveTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetOffense");source=(AFAGFHJONFJ)typeof(OffensiveTests).GetField("c",F).GetValue(null);target=(AFAGFHJONFJ)typeof(OffensiveTests).GetField("target",F).GetValue(null);faction=NEINKOOBJIH.COCMABJFOFH["F"];}
 static bool Attack()=>(bool)Call("TryAttack",source,target,faction,"gov");
 static bool Probe(int required)=>(bool)Call("CombatProbeReady",source,"city:"+target.Pointer,required,ICGAEMLACJG.HFJIECIBFKB.Values.ToList());
 public static void Run(){
  Setup();var t=Depart();Check(t.m_targetWarInfo==war&&t.m_listMovePos.Count>0,"reported LeaveCity + route install + expedition sequence uses real production callbacks");Cancel(t);
  for(int day=100;day<=102;day++){NEINKOOBJIH.MIPFHCKDJOK=day;Check(Send()==null,"early postdeparture rollback is remembered on day "+day);}Check(source.Calls==1,"same support does not regenerate daily after actual LeaveCity");
  NEINKOOBJIH.MIPFHCKDJOK=103;Check(Send()!=null,"bounded retry still permits support rather than permanently disabling it");
  Setup();t=Depart();t.Position=war.GBHGENNMJCK;Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"near-battle arrival cleanup excluded");
  Setup();t=Depart();t.Position=new Vector3(20);Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"progressed army excluded");
  Setup();t=Depart();t.m_warInfo=war;Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"battle join excluded");
  Setup();t=Depart();t.m_mergeTarget=new();Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"merge excluded");
  Setup();t=Depart();t.m_isEnterCity=true;Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"city entry excluded");
  Setup();t=Depart();t.m_targetWarInfo=new();Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"retargeted army excluded");
  Setup();t=Depart();NEINKOOBJIH.MIPFHCKDJOK=102;Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"later demobilization excluded");
  Setup();t=Depart();var mission=Field<IDictionary>("missions")[t.Pointer];mission.GetType().GetField("DepartureTick").SetValue(mission,Environment.TickCount64-11000);Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"late wall-clock cleanup excluded even while campaign clock stopped");
  Setup();t=Depart();t.m_id++;Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"recycled native pointer excluded");
  Setup();t=Depart();Call("ForgetBattle",war);Cancel(t);Check(Field<IDictionary>("rejectedBattleDispatches").Count==0,"finished battle excluded");
  Setup();var teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();string before=(string)Call("CombatProbeSignature",source,0,teams);source.LMPMNLLGOEC.KJPPGIOCDID.Add(source.LMPMNLLGOEC.KJPPGIOCDID[0]);string duplicate=(string)Call("CombatProbeSignature",source,0,teams);Call("NormalizeTroopReferences",source);Check(before==duplicate&&before==(string)Call("CombatProbeSignature",source,0,teams),"duplicate reference cleanup cannot masquerade as fresh soldiers and reopen retry");
  RunAttack();
 }
 public static void RunAttack(){
  AttackSetup();source.Generated=()=>source.Last.Morale=20;Check(!Attack()&&source.Last.Dismissed,"production low morale validation cancels assembly");Check(!Attack()&&source.Calls==1,"non-power validation failures also retained");
  AttackSetup();source.Generated=()=>source.Last.Combat=1;Check(!Attack(),"production insufficient actual adjusted power cancelled");target.Power++;Check(!Attack()&&source.Calls==1,"daily enemy estimate fluctuation does not regenerate identical weak army");
  AttackSetup();Call("FailedAttackProbe",source,target,123000,16695,"insufficient-attack-power");Check(!Probe(122999)&&!Probe(140000),"still insufficient threshold changes stay deferred");Check(Probe(16000),"enemy weakens enough for last actual assembled force: immediate reconsideration");
  AttackSetup();Call("FailedAttackProbe",source,target,123000,16695,"insufficient-attack-power");source.Roster.Add(new CLAGKDECFID{OJPECMNINNA="new-commander",CommandSlots=5,LMPMNLLGOEC=new HeroRow{CNOMNBCPJDI=source.OJPECMNINNA,LMMNKIOAGLC="F"}});Check(Probe(123000),"additional command capacity reopens failed attack");
  AttackSetup();Check(Attack(),"valid army accepted before native StartDeploy");source.Last.Morale=20;Check(!(bool)Call("GuardAttackStart",source.Last),"changed morale at departure cancels");Check(Field<IDictionary>("failedCombatProbes").Count==1,"late predeparture guard stores cancellation too");
  Call("ResetBattleState");Call("ResetOffense");Call("ResetWork");Call("ResetDefense");Call("ResetManualOrders");
  Console.WriteLine("PASS Dispatch119 "+checks+" production postdeparture rollback, normal lifecycle exclusions, duplicate cleanup, and attack retry checks; native calls mocked, reporter save unavailable.");
 }
}
