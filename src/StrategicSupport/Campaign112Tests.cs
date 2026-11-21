using System;using System.Collections;using System.Collections.Generic;using System.Reflection;
public static class Campaign112Tests {
 static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static object Field(string n)=>typeof(Campaign81Tests).GetField(n,BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
 static IDictionary Goals=>(IDictionary)typeof(StrategicSupport).GetField("campaignGoals",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
 static WorkQueue Queue=>(WorkQueue)typeof(StrategicSupport).GetField("work",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("R112: "+why);}
 static Wo_DeployTeamCtrl Setup(){MLGOLGGHLMB.CBEGDDKANAO.Clear();typeof(Campaign81Tests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);Queue.Clear();var t=(Wo_DeployTeamCtrl)Field("t");t.ClearTarget(false);return t;}
 static object Replacement(Wo_DeployTeamCtrl t){var g=Activator.CreateInstance(typeof(StrategicSupport).GetNestedType("CampaignGoal",BindingFlags.NonPublic),true);g.GetType().GetField("Id").SetValue(g,t.m_id);g.GetType().GetField("Faction").SetValue(g,t.m_belongPower);g.GetType().GetField("Goal").SetValue(g,t.m_assembleCity);g.GetType().GetField("Returning").SetValue(g,true);return g;}
 static void Pump()=>Queue.Pump(10000,10000,(s,ms)=>{},e=>throw e);
 public static void Run(){
  // Exact live order: ClearTarget -> StartCamp postfix enqueues orphan goal ->
  // surplus-return replaces it -> second Add rejected by the pending key.
  var t=Setup();t.ClearTarget(false);t.StartCamp();var latest=Replacement(t);Goals[t.Pointer]=latest;
  Check(!Queue.Add("campaign:"+t.m_id,()=>((IEnumerable<string>)Call("RecoverCampaign",t,latest)).GetEnumerator()),"postfix already owns queue key");
  Pump();Check(t.CityMoves>0&&t.m_isEnterCity,"pending orphan task adopts return handoff and issues actual movement");
  // More than one same-frame handoff still consumes the newest intent once.
  t=Setup();Call("AfterCampaignDay",t);Goals[t.Pointer]=Replacement(t);Goals[t.Pointer]=Replacement(t);Pump();
  Check(t.CityMoves==1&&t.m_isEnterCity,"multiple handoffs cannot lose return or double issue");
  t=Setup();Call("AfterCampaignDay",t);latest=Replacement(t);Goals[t.Pointer]=latest;t.m_id++;Pump();
  Check(t.CityMoves==0,"queued native identity cannot adopt recycled army");
  t=Setup();Call("AfterCampaignDay",t);Goals[t.Pointer]=Replacement(t);t.m_targetCity="manual-new-goal";Pump();
  Check(t.CityMoves==0,"new explicit goal remains protected");
  t=Setup();Call("AfterCampaignDay",t);Goals[t.Pointer]=Replacement(t);t.m_warInfo=new();Pump();
  Check(t.CityMoves==0,"battle arrival before execution remains protected");
  t=Setup();Call("AfterCampaignDay",t);Goals[t.Pointer]=Replacement(t);t.m_isPrivate=true;Pump();
  Check(t.CityMoves==0,"private army never adopted");
  t=Setup();Call("AfterCampaignDay",t);Goals[t.Pointer]=Replacement(t);t.m_responseToPlayer=true;Pump();
  Check(t.CityMoves==0,"player following army never adopted");
  // A busy key must not falsely consume the replacement goal's daily decision.
  t=Setup();Call("AfterCampaignDay",t);latest=Replacement(t);Goals[t.Pointer]=latest;Call("AfterCampaignDay",t);
  Check((double)latest.GetType().GetField("CheckedDay").GetValue(latest)==-1,"rejected admission does not mark daily decision completed");
  Pump();Check(t.CityMoves==1,"daily admission and handoff converge on one move");
  Check(!OfficerDeploymentPolicy.NeedsRelocation(5,9,9,false),"away officers keep their normal city allocation");
  Check(!OfficerDeploymentPolicy.NeedsRelocation(8,8,9,false),"one ordinary rounding seat does not cause a transfer");
  Check(OfficerDeploymentPolicy.NeedsRelocation(5,5,9,false),"substantial imbalance still transfers officers");
  Check(OfficerDeploymentPolicy.NeedsRelocation(0,9,9,false),"empty city receives minimum workers despite away officers");
  Check(!OfficerDeploymentPolicy.NeedsRelocation(2,9,9,false),"two actual workers stop repeated temporary replacements");
  Check(OfficerDeploymentPolicy.NeedsRelocation(8,9,9,true),"occupation or threat bypasses normal stability margin");
  Check(!OfficerDeploymentPolicy.NeedsRelocation(9,9,9,true),"emergency does not overfill its quota");
  var f=(MKMOJNAJKJE)Field("f");var h=new CLAGKDECFID{OJPECMNINNA="assignment-test",LMPMNLLGOEC=new(){LMMNKIOAGLC=f.IBGCLCAIBOL,CNOMNBCPJDI=t.m_assembleCity}};
  h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;
  Check((string)Call("AssignedOfficerCity",h,f)==t.m_assembleCity,"temporary research duty retains permanent slot");
  h.GMILGFFJOIL=true;Check(Call("AssignedOfficerCity",h,f)==null,"captive is not available assigned strength");h.GMILGFFJOIL=false;
  var destination=(AFAGFHJONFJ)Field("target");destination.LMPMNLLGOEC.LMMNKIOAGLC=f.IBGCLCAIBOL;
  h.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.CityMove;h.OGPEJIEHDAN.NHKNHHIBOGK=destination.OJPECMNINNA;
  Check((string)Call("AssignedOfficerCity",h,f)==destination.OJPECMNINNA,"in-flight relocation reserves destination only");
  Console.WriteLine("PASS R112 "+checks+" campaign handoff regressions; native movement mocked, not live gameplay.");
 }
}
