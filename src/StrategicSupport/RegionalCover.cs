using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed partial class StrategicSupport {
 sealed class CoverOrder {
  public Wo_DeployTeamCtrl Team;public int Id;public string Faction,Source,Target,Front;
  public long BaselinePower,BaselineSoldiers;public double Day,Length;public bool Routed;
 }
 static readonly Dictionary<IntPtr,CoverOrder> coverOrders=new();
 static readonly Dictionary<string,string> coverPlanning=new();
 static readonly Dictionary<string,double> coverPlannedDay=new();
 static bool CoverPlanning(string faction)=>coverPlanning.Values.Contains(faction);
 static IEnumerable<string> CoverPlanningSteps(string key,AFAGFHJONFJ c,AFAGFHJONFJ front,MKMOJNAJKJE f){
  bool finished=false;try{foreach(var phase in RegionalCoverSteps(c,front,new HashSet<string>()))yield return phase;finished=true;}
  finally{coverPlanning.Remove(key);if(finished&&CurrentFaction(f)){ScheduleDefense(f);foreach(var state in battles.Values)state.Next=double.NegativeInfinity;}}
 }
 sealed class ReliefLoan {public Wo_DeployTeamCtrl Team;public int Id;public string City,Faction;public long Power,Soldiers;}
 static readonly Dictionary<IntPtr,ReliefLoan> reliefLoans=new();
 static void TrackReliefLoan(Wo_DeployTeamCtrl team,AFAGFHJONFJ city,long power,long soldiers){
  reliefLoans[team.Pointer]=new(){Team=team,Id=team.m_id,City=city.OJPECMNINNA,Faction=city.LMPMNLLGOEC.LMMNKIOAGLC,Power=power,Soldiers=soldiers};
 }
 static bool CheckReliefLoan(ReliefLoan loan){
  if(loan==null)return false;
  // A queued snapshot may outlive a pooled army or a replacement loan. Only
  // the record that still owns this ledger entry may cancel or reroute it.
  if(ReferenceEquals(loan.Team,null)){foreach(var entry in reliefLoans.ToArray())if(ReferenceEquals(entry.Value,loan))reliefLoans.Remove(entry.Key);return false;}
  if(!reliefLoans.TryGetValue(loan.Team.Pointer,out var activeLoan)||!ReferenceEquals(activeLoan,loan))return false;
  var t=loan.Team;if(!t||ManualProtected(t)||t.m_id!=loan.Id||t.m_belongPower!=loan.Faction||t.m_isPrivate||t.m_responseToPlayer||t.IncludePlayer()){reliefLoans.Remove(t.Pointer);return false;}
  if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(loan.City,out var home)||home.LMPMNLLGOEC?.LMMNKIOAGLC!=loan.Faction||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(loan.Faction,out var f)){reliefLoans.Remove(t.Pointer);return false;}
  var teams=AttackTeams();long raw=ReserveCityPower(home,teams),men=HomeSoldiers(home,teams);
  if(raw>=loan.Power&&men>=loan.Soldiers){reliefLoans.Remove(t.Pointer);return false;}
  RegionalCoverCredit(home,f,teams,out long credit,out long soldiers,out _,out _);
  if(raw+credit>=loan.Power&&men+soldiers>=loan.Soldiers)return false;
  Note($"regional-loan-reassess team={t.m_id} home={loan.City} raw={raw} credited={credit} required={loan.Power} soldiers={men} incoming={soldiers} requiredSoldiers={loan.Soldiers} inBattle={t.m_warInfo!=null} reason=backfill-no-longer-timely");
  reliefLoans.Remove(t.Pointer);ScheduleDefense(f);
  if(t.m_warInfo!=null)return false; // no forced withdrawal or teleport from combat
  if(coverOrders.TryGetValue(t.Pointer,out var cover)&&ValidCover(cover)){ReleaseCover(cover,"upstream-cover-lost");return true;}
  if(defenseOrders.TryGetValue(t.Pointer,out var order)&&SameDefense(order)){ReleaseDefense(order,"upstream-cover-lost",true);return true;}
  if(missions.TryGetValue(t.Pointer,out var mission)&&ValidMission(mission)){
   missions.Remove(t.Pointer);if(t.m_deployState==AKOPKFPKJPP.Assemble){DismissSupport(t);return true;}
   t.StartCamp();t.m_targetCity="";t.m_targetWarInfo=null;
   var goal=new CampaignGoal{Id=t.m_id,Faction=loan.Faction,Goal=loan.City,Returning=true};campaignGoals[t.Pointer]=goal;work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());return true;
  }
  return false; // unowned/manual replacement orders remain untouched
 }
 static IEnumerable<string> RegionalLoanSteps(){
  // Repeat only when an actual cancellation invalidated a downstream promise.
  // Each cancellation removes a loan, so the graph cannot spin indefinitely.
  var admitted=new HashSet<string>();foreach(var owner in reliefLoans.Values.Select(x=>x.Faction).Distinct())if(AIGate("loan-watch:"+owner,owner,2))admitted.Add(owner);
  int bound=reliefLoans.Count;for(int pass=0;pass<bound;pass++){
   bool changed=false;foreach(var loan in reliefLoans.Values.ToArray()){yield return "regional-loan-recheck";if(admitted.Contains(loan.Faction))changed|=CheckReliefLoan(loan);}if(!changed)yield break;
  }
 }
 static bool ValidCover(CoverOrder o)=>o.Team&&!ManualProtected(o.Team)&&o.Team.m_id==o.Id&&o.Team.m_belongPower==o.Faction&&!o.Team.m_isPrivate&&!o.Team.m_responseToPlayer&&!o.Team.IncludePlayer()&&o.Team.m_warInfo==null&&!OtherCampaignOrder(o.Team)&&(string.IsNullOrEmpty(o.Team.m_targetCity)||o.Team.m_targetCity==o.Target);
 static long RawArmy(Wo_DeployTeamCtrl t){long value=0;var seen=new HashSet<IntPtr>();if(t?.m_dicDeployArmy!=null)foreach(var g in t.m_dicDeployArmy)if(g.Value!=null)foreach(var a in g.Value)if(a!=null&&seen.Add(a.Pointer))value+=Math.Max(0,a.GNCKPEPJDGF());return value;}
 static long HomeSoldiers(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams){var used=AttackReserved(teams);long count=0;var seen=new HashSet<IntPtr>();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!used.Contains(a.Pointer)&&seen.Add(a.Pointer))count+=Math.Max(0,a.FGHFOLKBHDL);return count;}
 // Observed intent is distinct from a possible redirection. An army already
 // attacking Wan is counted at Wan, not simultaneously as an immediate attack
 // against every lending city. A changed order triggers the existing threat poll.
 static bool IsCounterattackCandidate(Wo_DeployTeamCtrl t,AFAGFHJONFJ city){
  if(!t||t.m_warInfo!=null)return false;
  if(t.m_targetWarInfo!=null)return t.m_targetWarInfo.IJGLCFJOOIK?.OJPECMNINNA==city.OJPECMNINNA;
  if(!string.IsNullOrEmpty(t.m_targetCity)&&t.m_targetCity!=city.OJPECMNINNA&&MICGPAMNDKM.IJPKEMPIFIH.ContainsKey(t.m_targetCity))return false;
  if(t.m_attackTarget||t.m_mergeTarget||!string.IsNullOrEmpty(t.m_targetCamp)||t.m_targetVillage!=null||t.m_targetPass!=null)return false;
  return true;
 }
 static long UncommittedCityPower(AFAGFHJONFJ c,HashSet<IntPtr> reserved){
  long raw=0;var seen=new HashSet<IntPtr>();if(c?.LMPMNLLGOEC?.KJPPGIOCDID!=null)foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&a.FGHFOLKBHDL>0&&!reserved.Contains(a.Pointer)&&seen.Add(a.Pointer))raw+=Math.Max(0,a.GNCKPEPJDGF());return raw;
 }
 [ThreadStatic] static Dictionary<IntPtr,(List<TimedBorderDefense.Wave> Waves,string Detail)> transactionWaves;
 static List<TimedBorderDefense.Wave> CounterattackWaves(AFAGFHJONFJ city,MKMOJNAJKJE faction,List<Wo_DeployTeamCtrl> teams,out string detail){
  if(transactionWaves!=null&&transactionWaves.TryGetValue(city.Pointer,out var cached)){detail=cached.Detail;return cached.Waves;}
  var result=ComputeCounterattackWaves(city,faction,teams,out detail);
  if(transactionWaves!=null)transactionWaves[city.Pointer]=(result,detail);
  return result;
 }
 static List<TimedBorderDefense.Wave> ComputeCounterattackWaves(AFAGFHJONFJ city,MKMOJNAJKJE faction,List<Wo_DeployTeamCtrl> teams,out string detail){
  var waves=new List<TimedBorderDefense.Wave>();var parts=new List<string>();var seen=new HashSet<string>();var reserved=AttackReserved(teams);var speeds=new Dictionary<string,float>();
  float Speed(string owner){if(!speeds.TryGetValue(owner,out float speed)){speed=Wo_DeployTeamCtrl.GetMoveSpeed(owner,false);foreach(var t in teams)if(t&&t.m_belongPower==owner)speed=Math.Max(speed,t.GetMoveSpeed());speeds[owner]=speed;}return speed;}
  if(city.FKMKGNPFLFP!=null)foreach(var neighbour in city.FKMKGNPFLFP){
   string owner=neighbour?.LMPMNLLGOEC?.LMMNKIOAGLC;if(string.IsNullOrEmpty(owner)||!seen.Add(neighbour.OJPECMNINNA)||!War(faction,owner))continue;
   long power=UncommittedCityPower(neighbour,reserved);if(power<=0)continue;
   if(!ThreatRoadLength(neighbour,city,owner,neighbour.BMBMKPGEHBH,out double length,out string why)){parts.Add($"city:{neighbour.OJPECMNINNA}/{why}");continue;}
   float speed=Speed(owner);if(speed<=0)continue;double at=length/speed;
   waves.Add(new(at,power));parts.Add($"city:{neighbour.OJPECMNINNA}@{at:F1}/raw={power}");
  }
  foreach(var t in teams){if(!t||!War(faction,t.m_belongPower)||t.m_warInfo!=null)continue;
   if(!IsCounterattackCandidate(t,city)){parts.Add($"team:{t.m_id}/committed={t.m_targetWarInfo?.IJGLCFJOOIK?.OJPECMNINNA??t.m_targetCity}/not-local");continue;}
   long power=RawArmy(t);if(power<=0)continue;var origin=AOHIBKLOAEI.PNKKPMCJIGA(t.GetPosition());
   if(origin==null&&!string.IsNullOrEmpty(t.m_assembleCity))MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(t.m_assembleCity,out origin);
   if(!ThreatRoadLength(origin,city,t.m_belongPower,t.GetPosition(),out double length,out string why)){parts.Add($"team:{t.m_id}/{why}");continue;}
   float speed=t.GetMoveSpeed();if(speed<=0)continue;double at=length/speed;
   waves.Add(new(at,power));parts.Add($"team:{t.m_id}@{at:F1}/raw={power}");
  }
  detail=string.Join(";",parts);return waves;
 }
 static double CounterattackEarliest(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams){
  if(c.GKLIEHCHDLD()||IncomingHostile(c,f))return 0;
  var waves=CounterattackWaves(c,f,teams,out _);return waves.Count>0?waves.Min(w=>w.At):double.PositiveInfinity;
 }
 static double CoverDeadline(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams)=>CoverDeadlineWithHome(c,f,teams,ReserveCityPower(c,teams));
 static double CoverDeadlineWithHome(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,double remaining){
  if(c.GKLIEHCHDLD()||IncomingHostile(c,f))return 0;
  var waves=CounterattackWaves(c,f,teams,out string detail);
  double day=Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK()),home=ReserveCityPower(c,teams);
  double deadline=TimedBorderDefense.Collapse(Math.Max(0,remaining),waves,Math.Max(60,20*day));
  if(Gate("cover-threat-detail:"+c.OJPECMNINNA,15))Note($"regional-cover-threat city={c.OJPECMNINNA} homeRaw={home:F0} plannedDepartureRaw={Math.Max(0,home-remaining):F0} retainedRaw={remaining:F0} latestBackfill={deadline:F1} waves=[{detail}] lossBasis=actual-remaining-timed-precontact");
  return deadline;
 }
 static double CoverArrival(CoverOrder o,AFAGFHJONFJ c,MKMOJNAJKJE f){
  var t=o.Team;if(!ValidCover(o)||t.GetMorale()<30||t.GetFood()<Math.Max(1,t.GetOneDayFood())*5L)return double.PositiveInfinity;
  if(Vector3.Distance(t.GetPosition(),c.BMBMKPGEHBH)<2.2f)return 0;
  if(t.m_deployState==AKOPKFPKJPP.Camp)return double.PositiveInfinity;
  if(t.GetMoveSpeed()<=0)return double.PositiveInfinity;
  if(t.m_deployState==AKOPKFPKJPP.Assemble)return o.Length/t.GetMoveSpeed()+Math.Max(mobilizeSeconds.Value,Math.Max(0,t.GetLeftAssembleDay())*AOHIBKLOAEI.NIOKFCDLGFK());
  if(!o.Routed)return double.PositiveInfinity;
  return TeamArrival(t,c.BMBMKPGEHBH);
 }
 static void RegionalCoverCredit(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,out long power,out long soldiers,out long baseline,out long baselineSoldiers){
  RegionalCoverCreditForHome(c,f,teams,ReserveCityPower(c,teams),out power,out soldiers,out baseline,out baselineSoldiers);
 }
 static void RegionalCoverCreditForHome(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams,long remaining,out long power,out long soldiers,out long baseline,out long baselineSoldiers){
  power=soldiers=baseline=baselineSoldiers=0;if(coverOrders.Count==0)return;
  double deadline=CoverDeadlineWithHome(c,f,teams,remaining),margin=Math.Max(1,AOHIBKLOAEI.NIOKFCDLGFK());
  foreach(var o in coverOrders.Values){if(o.Target!=c.OJPECMNINNA||o.Faction!=f.IBGCLCAIBOL||!ValidCover(o))continue;
   double eta=CoverArrival(o,c,f);if(!double.IsFinite(eta)||(eta>0&&eta+margin>=deadline))continue;
   power+=RawArmy(o.Team);soldiers+=Math.Max(0,o.Team.GetSoldierNum(false));baseline=Math.Max(baseline,o.BaselinePower);baselineSoldiers=Math.Max(baselineSoldiers,o.BaselineSoldiers);
  }
  if(power>0&&Gate("cover-credit:"+c.OJPECMNINNA,10))Note($"regional-cover-credit city={c.OJPECMNINNA} incomingRaw={power} incomingSoldiers={soldiers} restoreRaw={baseline} restoreSoldiers={baselineSoldiers} counterattackContact={CounterattackEarliest(c,f,teams):F1} latestBackfill={deadline:F1} holdBasis=timed-precontact-assumption actualCommittedOnly=true");
 }
 static Wo_DeployTeamCtrl GenerateRelief(AFAGFHJONFJ c,int requested,AFAGFHJONFJ front,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved){
  if(!EmergencyFor(c)&&CoverPlanning(c.LMPMNLLGOEC.LMMNKIOAGLC))return null; // wait for decision work, not a timed city cooldown
  var t=GenerateSupport(c,requested,teams,reserved);if(t||front==null||c.Pointer==front.Pointer)return t;
  QueueRegionalCover(c,front,teams,reserved);return null;
 }
 static void QueueRegionalCover(AFAGFHJONFJ c,AFAGFHJONFJ front,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved){
  if(front==null||c.Pointer==front.Pointer)return;
  string key="defense-cover:"+c.OJPECMNINNA+":"+front.OJPECMNINNA;double day=Math.Floor((double)NEINKOOBJIH.MIPFHCKDJOK);
  if(DistributionCity(c,out var f)&&SupportSourceBlock(c,teams,reserved)==null&&BorderThreat(c,f,teams)!=long.MaxValue&&(!coverPlannedDay.TryGetValue(key,out double planned)||day<planned||day-planned>=DecisionDays(f.IBGCLCAIBOL))){
   if(work.Add(key,()=>CoverPlanningSteps(key,c,front,f).GetEnumerator())){coverPlanning[key]=f.IBGCLCAIBOL;coverPlannedDay[key]=day;}
  }
 }
 static void BackfillAfterCommit(AFAGFHJONFJ c,AFAGFHJONFJ front,MKMOJNAJKJE f){
  if(front==null)return;
  var teams=AttackTeams();long required=BorderThreat(c,f,teams),remaining=ReserveCityPower(c,teams);
  if(required!=long.MaxValue&&remaining<required){QueueRegionalCover(c,front,teams,AttackReserved(teams));Note($"regional-cover-after-relief city={c.OJPECMNINNA} front={front.OJPECMNINNA} remainingRaw={remaining} potentialThreatRaw={required} planningActive={CoverPlanning(f.IBGCLCAIBOL)}");}
 }
 static bool CoverCycle(string from,string to){
  var pending=new Queue<string>();var seen=new HashSet<string>();pending.Enqueue(to);
  while(pending.Count>0){var at=pending.Dequeue();if(at==from)return true;if(!seen.Add(at))continue;foreach(var o in coverOrders.Values)if(ValidCover(o)&&o.Source==at)pending.Enqueue(o.Target);}return false;
 }
 static DHKINGPDKJJ CoverUnit(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams){
  var reserved=AttackReserved(teams);long raw=ReserveCityPower(c,teams),soldiers=HomeSoldiers(c,teams);int count=0;
  foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!reserved.Contains(a.Pointer)&&a.FGHFOLKBHDL>0)count++;
  if(count<=minUnits.Value)return null;
  // Loan only within the existing physical retention limit. Incoming cover
  // never authorizes sending the last local troops or their commanders.
  DHKINGPDKJJ best=null;foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(Available(a)&&!reserved.Contains(a.Pointer)&&a.GNCKPEPJDGF()>0&&a.FGHFOLKBHDL<=soldiers*(1-Math.Round(retain.Value,3))&&a.GNCKPEPJDGF()<=raw*(1-Math.Round(retain.Value,3))&&(best==null||a.FGHFOLKBHDL<best.FGHFOLKBHDL))best=a;return best;
 }
 static Wo_DeployTeamCtrl GenerateBackfill(AFAGFHJONFJ c,int requested,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved,long soldiers,long power){
  long oldSoldiers=backfillSoldierGoal,oldPower=backfillPowerGoal;
  try{backfillSoldierGoal=Math.Max(1,soldiers);backfillPowerGoal=Math.Max(1,power);return GenerateSupport(c,requested,teams,reserved);}
  finally{backfillSoldierGoal=oldSoldiers;backfillPowerGoal=oldPower;}
 }
 static IEnumerable<string> RegionalCoverSteps(AFAGFHJONFJ receiver,AFAGFHJONFJ front,HashSet<string> expanded){
  yield return "cover-network-node";
  if(!Ready()||!DistributionCity(receiver,out var f)||!CurrentFaction(f)||front?.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||!expanded.Add(receiver.OJPECMNINNA))yield break;
  var teams=AttackTeams();if(!DefenseThreat(front,f,teams)&&!front.GKLIEHCHDLD())yield break;
  var officers=HomeOfficers(receiver,teams);if(officers.Count<=Math.Max(2,(officers.Count+2)/3))yield break;
  var wanted=CoverUnit(receiver,teams);if(wanted==null||SupportSourceBlock(receiver,teams,AttackReserved(teams))!=null)yield break;
  long neededRaw=Math.Max(1,wanted.GNCKPEPJDGF()),neededSoldiers=wanted.FGHFOLKBHDL;
  // Traverse the actual friendly adjacency graph. A terminal donor must have
  // independent surplus. Deeper nodes may lend only after real backfill orders
  // have been committed behind them; circular promises never create credit.
  var neighbours=receiver.FKMKGNPFLFP?.ToArray();if(neighbours==null)yield break;
  var candidates=neighbours.Where(c=>c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&c.Pointer!=front.Pointer&&!expanded.Contains(c.OJPECMNINNA)).ToArray();
  var ranked=new List<(AFAGFHJONFJ City,double Length)>();foreach(var phase in RankCityRoutes(candidates,receiver,f,true,ranked))yield return phase;
  foreach(var candidate in ranked){var donor=candidate.City;
   yield return "cover-network-edge";if(!CurrentFaction(f)||receiver.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||donor.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL)yield break;
   teams=AttackTeams();if(!DefenseThreat(front,f,teams)&&!front.GKLIEHCHDLD())yield break;
   RegionalCoverCredit(receiver,f,teams,out long credited,out long creditedSoldiers,out _,out _);if(credited>=neededRaw&&creditedSoldiers>=neededSoldiers)yield break;
   var reserved=AttackReserved(teams);if(SupportSourceBlock(donor,teams,reserved)!=null||CoverCycle(donor.OJPECMNINNA,receiver.OJPECMNINNA))continue;
   var route=new RoutePlan();foreach(var phase in ReliefPathSteps(donor.BMBMKPGEHBH,receiver.BMBMKPGEHBH,f,null,false,route)){
    yield return phase;if(!CurrentFaction(f)||receiver.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||donor.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL)yield break;
   }
   if(!route.Success)continue;double length=route.Length;
   double speed=Wo_DeployTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL,false),deadline=CoverDeadlineWithHome(receiver,f,teams,Math.Max(0,ReserveCityPower(receiver,teams)-neededRaw)),margin=Math.Max(1,AOHIBKLOAEI.NIOKFCDLGFK());
   if(speed<=0||length/speed+mobilizeSeconds.Value+margin>=deadline){Note($"regional-cover-block source={donor.OJPECMNINNA} target={receiver.OJPECMNINNA} reason=counterattack-before-backfill eta={length/Math.Max(.1,speed)+mobilizeSeconds.Value:F1} safetyMargin={margin:F1} plannedDepartureRaw={neededRaw} deadline={deadline:F1}");continue;}
   int Request()=> (int)Math.Clamp(Math.Ceiling(Math.Max((neededRaw-credited)/(double)Math.Max(1,ReserveCityPower(donor,teams)),(neededSoldiers-creditedSoldiers)/(double)Math.Max(1,HomeSoldiers(donor,teams)))*Math.Max(1,Power(donor))),1,int.MaxValue);
   var team=GenerateBackfill(donor,Request(),teams,reserved,neededSoldiers-creditedSoldiers,neededRaw-credited);
   if(!team){foreach(var phase in RegionalCoverSteps(donor,front,expanded))yield return phase;
    teams=AttackTeams();reserved=AttackReserved(teams);if(SupportSourceBlock(donor,teams,reserved)!=null)continue;team=GenerateBackfill(donor,Request(),teams,reserved,neededSoldiers-creditedSoldiers,neededRaw-credited);
   }
   if(!team)continue;bool committed=false;
   try{
    teams=AttackTeams();deadline=CoverDeadlineWithHome(receiver,f,teams,Math.Max(0,ReserveCityPower(receiver,teams)-neededRaw));
    double eta=length/Math.Max(.001,team.GetMoveSpeed())+Math.Max(mobilizeSeconds.Value,Math.Max(0,team.GetLeftAssembleDay())*AOHIBKLOAEI.NIOKFCDLGFK());
    if(!CurrentFaction(f)||front.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||receiver.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||eta+margin>=deadline||!RelayFoodSafe(team,donor,eta)||IncomingHostile(donor,f)){DismissSupport(team);continue;}
    var order=new CoverOrder{Team=team,Id=team.m_id,Faction=f.IBGCLCAIBOL,Source=donor.OJPECMNINNA,Target=receiver.OJPECMNINNA,Front=front.OJPECMNINNA,BaselinePower=ReserveCityPower(receiver,teams),BaselineSoldiers=HomeSoldiers(receiver,teams),Day=NEINKOOBJIH.MIPFHCKDJOK,Length=length};
    // Preserve the original defense baseline across multiple partial donors.
    foreach(var old in coverOrders.Values)if(old.Target==order.Target&&old.Faction==order.Faction&&ValidCover(old)){order.BaselinePower=Math.Max(order.BaselinePower,old.BaselinePower);order.BaselineSoldiers=Math.Max(order.BaselineSoldiers,old.BaselineSoldiers);}
    team.m_targetCity=order.Target;team.m_isEnterCity=true;coverOrders[team.Pointer]=order;committed=true;
    if(!team.gameObject.activeSelf)team.gameObject.SetActive(true);
    Note($"regional-cover-ordered team={team.m_id} source={order.Source} target={order.Target} front={order.Front} soldiers={team.GetSoldierNum(false)} raw={RawArmy(team)} eta={eta:F1} deadline={deadline:F1} depthVisited={expanded.Count} actualDeparturePending=true");ScheduleDefense(f);
   }catch{if(!committed&&team)DismissSupport(team);throw;}
  }
 }
 static bool GuardCoverEntry(Wo_DeployTeamCtrl t,AFAGFHJONFJ c){
  if(!t||!coverOrders.TryGetValue(t.Pointer,out var o)||!ValidCover(o)||c?.OJPECMNINNA!=o.Target)return true;
  if(c.LMPMNLLGOEC?.LMMNKIOAGLC==o.Faction&&!c.GKLIEHCHDLD()&&CanAdmitReturn(c,t)&&DestinationFoodSafe(c,AttackTeams(),t)){
   coverOrders.Remove(t.Pointer);Note($"regional-cover-entry team={o.Id} target={o.Target} soldiers={t.GetSoldierNum(false)} capacityChecked=true");return true;
  }
  t.StartCamp();t.m_targetCity="";t.m_isEnterCity=false;return false;
 }
 static void CheckRegionalCover(CoverOrder o){
  if(o==null)return;
  if(ReferenceEquals(o.Team,null)){foreach(var entry in coverOrders.ToArray())if(ReferenceEquals(entry.Value,o))coverOrders.Remove(entry.Key);return;}
  if(!coverOrders.TryGetValue(o.Team.Pointer,out var activeCover)||!ReferenceEquals(activeCover,o))return;
  if(!ValidCover(o)){coverOrders.Remove(o.Team.Pointer);Note($"regional-cover-invalid team={o.Id} target={o.Target} creditRevoked=true");return;}
  var t=o.Team;if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(o.Faction,out var f)||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Target,out var target)||target.LMPMNLLGOEC?.LMMNKIOAGLC!=o.Faction){ReleaseCover(o,"destination-lost");return;}
  if(t.m_deployState==AKOPKFPKJPP.Assemble)return;
  var teams=AttackTeams();var war=teams.Select(x=>x.m_warInfo).FirstOrDefault(w=>w?.IJGLCFJOOIK?.OJPECMNINNA==o.Target&&w.EMMJBPMNFAK?.Contains(o.Faction)==true);
  if(war!=null){if(!Same(t.m_targetWarInfo,war)&&!MoveRelief(t,target,war,false,f))ReleaseCover(o,"route-lost");return;}
  bool frontActive=MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.Front,out var front)&&front.LMPMNLLGOEC?.LMMNKIOAGLC==o.Faction&&(DefenseThreat(front,f,teams)||front.GKLIEHCHDLD());
  bool localThreat=DefenseThreat(target,f,teams);
  bool arrived=Vector3.Distance(t.GetPosition(),target.BMBMKPGEHBH)<2.2f;
  bool room=(arrived?CanAdmitReturn(target,t):CapacityRoom(target,teams,t)>=TeamTroops(t).Count)&&!target.GKLIEHCHDLD()&&DestinationFoodSafe(target,teams,t);
  if(Gate("cover-decision:"+o.Id,15))Note($"regional-cover-decision team={o.Id} source={o.Source} target={o.Target} front={o.Front} frontActive={frontActive} localThreat={localThreat} room={room} homeRaw={ReserveCityPower(target,teams)} historicalRaw={o.BaselinePower} homeSoldiers={HomeSoldiers(target,teams)} historicalSoldiers={o.BaselineSoldiers} ageDays={NEINKOOBJIH.MIPFHCKDJOK-o.Day:F1}");
  // A real garrison replaces the promised backfill; historical strength is
  // not a reason to keep it outside forever. Entry rechecks capacity above.
  if(room&&Vector3.Distance(t.GetPosition(),target.BMBMKPGEHBH)<2.2f&&ICGAEMLACJG.FKKCPDEFODO.TryGetValue(o.Target,out var ctrl)){
   t.MoveToCity(ctrl,true,true);return;
  }
  if(!frontActive&&!localThreat){ReleaseCover(o,"current-threat-ended");return;}
  if(!localThreat&&ReserveCityPower(target,teams)>=o.BaselinePower&&HomeSoldiers(target,teams)>=o.BaselineSoldiers){ReleaseCover(o,"home-defense-restored");return;}
  if(t.GetFood()<Math.Max(1,t.GetOneDayFood())*5L||t.GetMorale()<30){ReleaseCover(o,"food-or-morale");return;}
  if(Vector3.Distance(t.GetPosition(),target.BMBMKPGEHBH)<2.2f){if(t.m_deployState!=AKOPKFPKJPP.Camp)t.StartCamp();t.m_targetCity="";t.m_isEnterCity=false;return;}
  if(!o.Routed||t.m_deployState==AKOPKFPKJPP.Camp){o.Routed=MoveRelief(t,target,null,false,f);if(!o.Routed)ReleaseCover(o,"route-lost");}
 }
 static void ReleaseCover(CoverOrder o,string why){
  var t=o.Team;if(t.m_deployState==AKOPKFPKJPP.Assemble){coverOrders.Remove(t.Pointer);reliefLoans.Remove(t.Pointer);DismissSupport(t);Note($"regional-cover-release team={o.Id} target={o.Target} reason={why} unstarted=true");return;}
  t.StartCamp();t.m_targetCity="";t.m_targetWarInfo=null;coverOrders.Remove(t.Pointer);
  var goal=new CampaignGoal{Id=t.m_id,Faction=o.Faction,Goal=why=="home-defense-restored"?o.Front:o.Source,Returning=why!="home-defense-restored"};campaignGoals[t.Pointer]=goal;work.Add("campaign:"+t.m_id,()=>RecoverCampaign(t,goal).GetEnumerator());
  Note($"regional-cover-release team={o.Id} target={o.Target} reason={why}");
 }
}
