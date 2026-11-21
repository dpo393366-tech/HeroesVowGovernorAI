using System;using System.Collections.Generic;using UnityEngine;
public sealed partial class StrategicSupport {
 // Returning survivors can enter any friendly city with room. Personal city
 // management remains excluded from building/recruitment/source automation.
 static List<AFAGFHJONFJ> ReturnCities(MKMOJNAJKJE f){
  var result=new List<AFAGFHJONFJ>();var cities=f.PJMLPLDPEIM();
  if(cities!=null)foreach(var c in cities)if(c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL)result.Add(c);return result;
 }
 static bool AtCompletedCity(Wo_DeployTeamCtrl t,AFAGFHJONFJ city){
  if(Vector3.Distance(t.GetPosition(),city.BMBMKPGEHBH)<=5)return true;
  return t.m_isStatic&&!t.m_isEnterCity&&(t.m_listMovePos?.Count??0)==0&&(t.GAANFLKOMKL?.Count??0)==0&&AOHIBKLOAEI.PNKKPMCJIGA(t.GetPosition())?.Pointer==city.Pointer;
 }
 static bool AdoptCompletedCityOrder(Wo_DeployTeamCtrl t){
  if(!t||ManualProtected(t)||t.m_deployState!=AKOPKFPKJPP.Expedition||t.m_isStarting||t.m_isEnterCity||t.m_isPrivate||t.m_responseToPlayer||t.IncludePlayer()||t.m_warInfo!=null||t.m_targetWarInfo!=null||OtherCampaignOrder(t)||defenseOrders.ContainsKey(t.Pointer)||coverOrders.ContainsKey(t.Pointer)||huntOrders.ContainsKey(t.Pointer))return false;
  if((t.m_listMovePos?.Count??0)+(t.GAANFLKOMKL?.Count??0)>0)return false;
  if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(t.m_belongPower??"",out var f)||f==null)return false;
  string id=!string.IsNullOrEmpty(t.m_targetCity)?t.m_targetCity:t.m_tmpTargetCity;
  if(string.IsNullOrEmpty(id)||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(id,out var city)||city.LMPMNLLGOEC?.LMMNKIOAGLC!=t.m_belongPower||city.GKLIEHCHDLD())return false;
  // A new hostile/manual travel order retains authority. Only a completed
  // friendly objective at the army's current arrival position is adopted.
  if(!string.IsNullOrEmpty(t.m_tmpTargetCity)&&t.m_tmpTargetCity!=id)return false;
  float gap=Vector3.Distance(t.GetPosition(),city.BMBMKPGEHBH);if(!AtCompletedCity(t,city))return false;
  var goal=new CampaignGoal{Id=t.m_id,Faction=t.m_belongPower,Goal=id,CompletedOrder=true};campaignGoals[t.Pointer]=goal;
  t.ClearTarget(false);t.m_tmpTargetCity="";t.StartCamp();
  work.Add("campaign:"+t.m_id,()=>SafePhases(RecoverCampaign(t,goal)).GetEnumerator());
  Note($"campaign-completed-city-adopted team={t.m_id} source={t.m_assembleCity} captured={id} distance={gap:F2} soldiers={t.GetSoldierNum(false)} action=protect-split-support-or-return nativeOrderFinished=true");return true;
 }
}
