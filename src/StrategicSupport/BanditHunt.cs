using System;using System.Collections.Generic;using System.Linq;using UnityEngine;
public sealed partial class StrategicSupport {
 // Explicit vanilla BigMapArmy definitions: exclude private/event armies and
 // groups which have already become diplomatic factions.
 static readonly HashSet<string> raiderDefinitions=new(){"BMA100108","BMA100209","BMA100109","BMA100309","BMA100409","BMA100509","BMA100609","BMA100709"};
 sealed class HuntOrder {public Wo_DeployTeamCtrl Team,Target;public int Id,TargetId;public string Faction,Source,Governor;public double Started,Checked=-1,ProgressDay;public Vector3 Position;}
 static readonly Dictionary<IntPtr,HuntOrder> huntOrders=new();
 static bool LiveTeam(Wo_DeployTeamCtrl t)=>t&&ICGAEMLACJG.HFJIECIBFKB.TryGetValue(t.m_id,out var live)&&live?.Pointer==t.Pointer;
 static bool Raider(Wo_DeployTeamCtrl t)=>LiveTeam(t)&&string.IsNullOrEmpty(t.m_belongPower)&&t.m_bma!=null&&raiderDefinitions.Contains(t.m_bma.OJPECMNINNA)&&!t.m_isPrivate&&!t.IncludePlayer()&&!t.m_isInstantTeam&&t.GetSoldierNum(false)>0;
 static void ScheduleBanditHunt(MKMOJNAJKJE f){
  string key="hunt:"+f.IBGCLCAIBOL;
  if(offensive?.Value!=true||!DecisionDue(key,f.IBGCLCAIBOL,f.Pointer,"",false))return;
  if(work.Add(key,()=>HuntSteps(f).GetEnumerator()))DecisionDue(key,f.IBGCLCAIBOL,f.Pointer,"",true);
 }
 static bool RaiderNear(AFAGFHJONFJ source,MKMOJNAJKJE f,Wo_DeployTeamCtrl enemy){
  var region=AOHIBKLOAEI.PNKKPMCJIGA(enemy.GetPosition());
  return region!=null&&region.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&(region.Pointer==source.Pointer||source.FKMKGNPFLFP?.ToArray().Any(c=>c?.Pointer==region.Pointer)==true);
 }
 static bool HuntRoute(AFAGFHJONFJ city,MKMOJNAJKJE f,Wo_DeployTeamCtrl target,out int days){
  days=0;float speed=Wo_DeployTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL,false),seconds=AOHIBKLOAEI.NIOKFCDLGFK();
  if(speed<=0||seconds<=0||!ReliefPath(city.BMBMKPGEHBH,target.GetPosition(),f,null,false,out _,out double length))return false;
  days=Math.Max(1,(int)Math.Ceiling(length/speed/seconds));return days<=20;
 }
 static int HuntPower(Wo_DeployTeamCtrl target)=>(int)Math.Min(int.MaxValue,Math.Ceiling(Math.Max(1,target.GetCombatVal())*1.35));
 static IEnumerable<string> HuntSteps(MKMOJNAJKJE f){
  if(!Ready()||!CurrentFaction(f))yield break;
  var targets=AttackTeams().Where(Raider).ToArray();
  if(Gate("hunt-census:"+f.IBGCLCAIBOL,30))Note($"hunt-census faction={f.IBGCLCAIBOL} recognized={targets.Length} targets=[{string.Join(";",targets.Select(t=>t.m_id+":"+t.GetSoldierNum(false)))}]");
  foreach(var enemy in targets){
   var candidates=new List<(AFAGFHJONFJ City,int Days)>();
   var owned=f.PJMLPLDPEIM();if(owned==null)yield break;
   foreach(var city in owned.ToArray()){
    yield return "hunt-source";if(!CurrentFaction(f)||!Raider(enemy))break;
    if(city?.LMPMNLLGOEC==null)continue;
    bool near=RaiderNear(city,f,enemy),permission=AttackPermission(city,f,city.LMPMNLLGOEC.IPJHOOJBCKI),danger=IncomingHostile(city,f);
    if(Gate("hunt-source:"+enemy.m_id+":"+city.OJPECMNINNA,15)){var stock=AttackStock(city,f,AttackTeams());Note($"hunt-source target={enemy.m_id} city={city.OJPECMNINNA} near={near} policyAllowed={permission} incomingHostile={danger} homeUnits={stock.Home} readyUnits={stock.Ready} rawSurplus={stock.Budget} requiredAdjusted={HuntPower(enemy)} comparison=native-assembled-team cash={f.DNOGPFHMOMO} mobilization={MobilizationCost(city)}");}
    if(!near||!permission||danger)continue;
    var teams=AttackTeams();if(teams.Any(t=>t.m_belongPower==f.IBGCLCAIBOL&&(t.m_assembleCity==city.OJPECMNINNA&&t.m_deployState==AKOPKFPKJPP.Assemble||t.m_attackTarget?.Pointer==enemy.Pointer)))continue;
    if(huntOrders.Values.Any(o=>o.Faction==f.IBGCLCAIBOL&&o.TargetId==enemy.m_id&&LiveTeam(o.Team)))continue;
    if(!ViableSource(city,teams,AttackReserved(teams))||AttackStock(city,f,teams).Budget<=0||f.DNOGPFHMOMO<MobilizationCost(city))continue;
    if(!CombatProbeReady(city,HuntProbeKey(enemy),HuntPower(enemy),teams))continue;
    if(HuntRoute(city,f,enemy,out int days))candidates.Add((city,days));else if(Gate("hunt-route:"+enemy.m_id+":"+city.OJPECMNINNA,15))Note($"hunt-block target={enemy.m_id} city={city.OJPECMNINNA} reason=route-or-travel-limit");
   }
   foreach(var choice in candidates.OrderBy(x=>x.Days)){
    yield return "hunt-dispatch";if(!CurrentFaction(f)||!Raider(enemy)||enemy.m_warInfo!=null)break;
    var c=choice.City;var teams=AttackTeams();var gov=c.LMPMNLLGOEC.IPJHOOJBCKI;
    if(!AttackPermission(c,f,gov)||IncomingHostile(c,f)||!RaiderNear(c,f,enemy)||!HuntRoute(c,f,enemy,out int days))continue;
    if(teams.Any(t=>t.m_belongPower==f.IBGCLCAIBOL&&(t.m_assembleCity==c.OJPECMNINNA&&t.m_deployState==AKOPKFPKJPP.Assemble||t.m_attackTarget?.Pointer==enemy.Pointer)))continue;
    if(!ViableSource(c,teams,AttackReserved(teams))||AttackStock(c,f,teams).Budget<=0||f.DNOGPFHMOMO<MobilizationCost(c))continue;
    if(!CombatProbeReady(c,HuntProbeKey(enemy),HuntPower(enemy),teams))continue;
    Wo_DeployTeamCtrl team=null;
    try{
     // Native requested power can stop commander allocation before the final
     // adjusted army reaches HuntPower. Request the safe available pool;
     // GenerateSupport still enforces home troops/officers/food reserves.
     offenseGenerating=true;try{team=GenerateSupport(c,int.MaxValue,teams,AttackReserved(teams));}finally{offenseGenerating=false;}
     if(!team){FailedProbe(c,HuntProbeKey(enemy),HuntPower(enemy));Note($"hunt-block city={c.OJPECMNINNA} target={enemy.m_id} reason=native-assembly-empty-or-home-reserve");continue;}
     team.m_targetFoodDay=Math.Max(foodDays.Value,days*2+15);
     int actual=team.GetCombatVal(),required=HuntPower(enemy);
     RememberHuntPower(c,enemy,team);
     Note($"hunt-power-probe city={c.OJPECMNINNA} target={enemy.m_id} rawSelected={RawArmy(team)} actualAdjusted={actual} requiredAdjusted={required} soldiers={team.GetSoldierNum(false)} sameScale=true");
     if(actual<required){DismissSupport(team);FailedProbe(c,HuntProbeKey(enemy),required);Note($"hunt-block city={c.OJPECMNINNA} target={enemy.m_id} reason=assembled-power-insufficient actualAdjusted={actual} requiredAdjusted={required}");continue;}
     if(!Raider(enemy)||IncomingHostile(c,f)||!AttackPermission(c,f,gov)||team.GetMorale()<60||!OffensivePolicy.FoodSafe(c.LMPMNLLGOEC.OEGNGFBCHFL,ReservedFood(c.OJPECMNINNA),CityFoodFloor(c,f.IBGCLCAIBOL),team.GetOneDayFood(),team.m_targetFoodDay,team.GetFood())){DismissSupport(team);continue;}
     team.m_targetCity="";team.m_isEnterCity=false;team.m_attackTarget=enemy;
     huntOrders[team.Pointer]=new(){Team=team,Target=enemy,Id=team.m_id,TargetId=enemy.m_id,Faction=f.IBGCLCAIBOL,Source=c.OJPECMNINNA,Governor=gov,Started=NEINKOOBJIH.MIPFHCKDJOK,ProgressDay=NEINKOOBJIH.MIPFHCKDJOK,Position=team.GetPosition()};
     if(!team.gameObject.activeSelf)team.gameObject.SetActive(true);
     Note($"hunt-ordered team={team.m_id} target={enemy.m_id} definition={enemy.m_bma.OJPECMNINNA} source={c.OJPECMNINNA} faction={f.IBGCLCAIBOL} power={team.GetCombatVal()} required={HuntPower(enemy)} travelDays={days} nativeAssembly=true");break;
    }catch{if(team){huntOrders.Remove(team.Pointer);if(team.m_deployState==AKOPKFPKJPP.Assemble)DismissSupport(team);}throw;}
   }
   foreach(var phase in HuntMusterSteps(f,enemy))yield return phase;
  }
 }
 static void EndHunt(HuntOrder o,string reason){
  var t=o.Team;huntOrders.Remove(t.Pointer);
  Note($"hunt-ended team={o.Id} target={o.TargetId} reason={reason} state={t.m_deployState}");
  if(t.m_warInfo!=null){campaignGoals[t.Pointer]=new(){Id=t.m_id,Faction=t.m_belongPower,Goal=o.Source,Returning=true};return;}
  if(t.m_deployState==AKOPKFPKJPP.Assemble){DismissSupport(t);return;}
  t.ClearTarget(false);t.StartCamp();
  var goal=new CampaignGoal{Id=t.m_id,Faction=t.m_belongPower,Goal=o.Source,Returning=true};campaignGoals[t.Pointer]=goal;
  work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());
 }
 static string HuntBlock(HuntOrder o){
  if(!Raider(o.Target)||o.Target.m_id!=o.TargetId)return "target-ended";
  if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Source,out var c)||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(o.Faction,out var f)||!AttackPermission(c,f,o.Governor))return "source-policy-changed";
  if(IncomingHostile(c,f)||!RaiderNear(c,f,o.Target))return "defense-priority-or-target-left";
  if(o.Team.GetCombatVal()<HuntPower(o.Target)||o.Team.GetMorale()<40)return "power-or-morale";
  if(!HuntRoute(c,f,o.Target,out int days))return "route-unavailable";
  if(o.Team.m_deployState!=AKOPKFPKJPP.Assemble&&o.Team.GetFood()<(long)Math.Max(1,o.Team.GetOneDayFood())*(days+5))return "return-food-reserve";
  return null;
 }
 static bool GuardHuntStart(Wo_DeployTeamCtrl t){
  if(!t||!huntOrders.TryGetValue(t.Pointer,out var o)||o.Id!=t.m_id)return true;
  string block=HuntBlock(o);if(block==null)return true;EndHunt(o,block);return false;
 }
 static void CheckHunt(HuntOrder o){
  var t=o.Team;
  if(!LiveTeam(t)||t.m_id!=o.Id||t.m_belongPower!=o.Faction){huntOrders.Remove(t.Pointer);return;}
  if(ManualProtected(t)||t.m_isPrivate||t.IncludePlayer()||t.m_responseToPlayer){huntOrders.Remove(t.Pointer);return;}
  if(t.m_warInfo!=null){EndHunt(o,"joined-battle");return;}
  if(t.m_attackTarget?.Pointer!=o.Target.Pointer){huntOrders.Remove(t.Pointer);return;}
  double day=NEINKOOBJIH.MIPFHCKDJOK;
  if(!DecisionDue("hunt-progress:"+t.m_id,o.Faction,t.Pointer,"",true))return;o.Checked=Math.Floor(day);
  string block=HuntBlock(o);if(block!=null){EndHunt(o,block);return;}
  if(t.m_deployState==AKOPKFPKJPP.Assemble){if(day-o.Started>30)EndHunt(o,"assembly-timeout");return;}
  if(Vector3.Distance(t.GetPosition(),o.Position)>.5f){o.Position=t.GetPosition();o.ProgressDay=day;}
  else if(day-o.ProgressDay>=3){EndHunt(o,"no-movement-safe-return");return;}
  if(t.m_deployState==AKOPKFPKJPP.Camp)t.SetAttackTarget(o.Target,false);
 }
}
