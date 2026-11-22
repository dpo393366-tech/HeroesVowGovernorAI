using System;using System.Collections.Generic;using System.Reflection;using UnityEngine;
public static class Intercept135Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int n;static Wo_DeployTeamCtrl army,enemy;static MKMOJNAJKJE f;static AFAGFHJONFJ remote;
 static object Call(string name,params object[] a)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,a);
 static void Check(bool ok,string why){n++;if(!ok)throw new Exception("Intercept135: "+why);}
 static void Setup(){
  typeof(Intercept90Tests).GetMethod("Setup",F).Invoke(null,null);
  army=(Wo_DeployTeamCtrl)typeof(Intercept90Tests).GetField("army",F).GetValue(null);enemy=(Wo_DeployTeamCtrl)typeof(Intercept90Tests).GetField("enemy",F).GetValue(null);
  f=NEINKOOBJIH.COCMABJFOFH[army.m_belongPower];f.AENJACOFBJK["ally"]=new(){EBIOPDHEBOF=BHBNKONCOJD.Ally};
  remote=new(){OJPECMNINNA="remote135"};remote.LMPMNLLGOEC.LMMNKIOAGLC="enemy";AOHIBKLOAEI.RegionAt=p=>remote;
  enemy.m_targetWarInfo=new(){EMMJBPMNFAK=new[]{"enemy","ally"},HIENOINGBCN="Jinan"};
 }
 static bool Relevant()=> (bool)Call("InterceptRelevant",new object[]{army,enemy,null});
 public static void Run(){
  Setup();Check(Relevant(),"outside local frontier: enemy reinforcements to allied battle retain purpose");
  Call("RecoverHostileStandoff",army);Check(army.m_responseToPlayer&&army.m_attackTarget==enemy&&army.m_listMovePos.Count>0&&!army.m_isEnterCity,"Puyang interception resumes rather than being returned by local-region veto");
  Setup();army.m_deployState=AKOPKFPKJPP.Assemble;Check((bool)Call("GuardFailedIntercept",army)&&!army.Dismissed,"departure uses same allied battle purpose as camp recovery");
  Setup();enemy.m_targetWarInfo.EMMJBPMNFAK=new[]{"ally","enemy"};Check(Relevant(),"battle side order independent");
  Setup();enemy.m_targetWarInfo.EMMJBPMNFAK=new[]{"enemy",army.m_belongPower};Check(Relevant(),"own battle relief interception also retained");
  Setup();enemy.m_targetWarInfo.EMMJBPMNFAK=new[]{"enemy","unrelated"};Check(!Relevant(),"unrelated battle is not sufficient purpose");
  Setup();enemy.m_targetWarInfo.EMMJBPMNFAK=new[]{"unrelated","ally"};Check(!Relevant(),"target must actually belong to opposing battle side");
  Setup();enemy.m_targetWarInfo.FGHFOLKBHDL[1]=0;Check(!Relevant(),"finished battle does not preserve pursuit forever");
  Setup();enemy.m_targetWarInfo=null;Check(!Relevant(),"lost target falls back to local purpose test");
  Setup();f.AENJACOFBJK["ally"].EBIOPDHEBOF=BHBNKONCOJD.ArmyPass;Check(!Relevant(),"transit treaty is not military alliance");
  Setup();f.AENJACOFBJK["ally"].EBIOPDHEBOF=BHBNKONCOJD.StopWar;Check(!Relevant(),"ceasefire is not military alliance");
  Setup();f.AENJACOFBJK["ally"].EBIOPDHEBOF=BHBNKONCOJD.War;Check(!Relevant(),"broken alliance is reevaluated immediately");
  Setup();enemy.m_targetWarInfo=null;remote.LMPMNLLGOEC.LMMNKIOAGLC="ally";MICGPAMNDKM.IJPKEMPIFIH[remote.OJPECMNINNA]=remote;enemy.m_targetCity=remote.OJPECMNINNA;Check(Relevant(),"direct attack on ally is valid outside local frontier");
  Setup();enemy.m_targetWarInfo=null;enemy.m_attackTarget=new(){m_belongPower="ally"};Check(Relevant(),"hostile attack on allied field army is valid");
  Setup();army.m_isPrivate=true;Call("RecoverHostileStandoff",army);Check(army.m_attackTarget==null&&army.m_responseToPlayer,"private follow order untouched");
  Setup();army.m_warInfo=new();Call("RecoverHostileStandoff",army);Check(army.m_attackTarget==null&&army.m_warInfo!=null,"active combat untouched");
  Setup();enemy.m_warInfo=enemy.m_targetWarInfo;enemy.m_targetWarInfo=null;Check(Relevant(),"entered allied battle identified before stale-follow recovery");Call("RecoverHostileStandoff",army);Check(!army.m_responseToPlayer,"enemy entering combat does not leave endless follow state");
  AOHIBKLOAEI.RegionAt=null;Call("ResetWork");Console.WriteLine($"PASS Intercept135 {n} allied-purpose and safe fallback checks; native combat mocked.");
 }
}
