using System;using System.Collections.Generic;using System.Linq;using UnityEngine;
public sealed partial class StrategicSupport {
 // One ordered pass survives frame boundaries. A rotating cursor must never
 // turn a farther city's completed quote into priority over a nearer source.
 static IEnumerable<string> BattleSourceSteps(NJHDEHLHENN w,BattleState state,int side,bool direct,MKMOJNAJKJE f){
  var all=ProtectedCities(f);var anchor=w.IJGLCFJOOIK??AOHIBKLOAEI.PNKKPMCJIGA(w.GBHGENNMJCK);
  var nearby=anchor==null?new List<AFAGFHJONFJ>():all.Where(c=>c.Pointer==anchor.Pointer||anchor.FKMKGNPFLFP?.ToArray().Any(n=>n?.Pointer==c.Pointer)==true||c.FKMKGNPFLFP?.ToArray().Any(n=>n?.Pointer==anchor.Pointer)==true).ToList();
  // A cold loaded battle must not wait for every distant route. Resolve ALL
  // direct road-neighbours first, sort by their validated road ETA, dispatch,
  // then evaluate the rest. This is an emergency tier, not a claim that an
  // unknown remote route is globally slower. Once warm, retain global ranking.
  string stamp=RoadPolicyStamp(f);bool cold=false;
  foreach(var city in all){string key=w.Pointer+":"+w.HIENOINGBCN+":"+f.IBGCLCAIBOL+":"+city.OJPECMNINNA;if(!SavedBattleQuote(city,w,f,key,stamp,out _)){cold=true;break;}}
  if(cold&&nearby.Count>0&&nearby.Count<all.Count){
   Note($"battle-cold-local-first war={w.HIENOINGBCN} faction={f.IBGCLCAIBOL} nearby={nearby.Count} remaining={all.Count-nearby.Count} ranking=validated-road-eta-within-tier");
   var used=new HashSet<IntPtr>(nearby.Select(c=>c.Pointer));
   foreach(var phase in BattleSourceBatch(w,state,side,direct,f,nearby))yield return phase;
   foreach(var phase in BattleSourceBatch(w,state,side,direct,f,all.Where(c=>!used.Contains(c.Pointer)).ToList()))yield return phase;
  }else foreach(var phase in BattleSourceBatch(w,state,side,direct,f,all))yield return phase;
 }
 static IEnumerable<string> BattleSourceBatch(NJHDEHLHENN w,BattleState state,int side,bool direct,MKMOJNAJKJE f,List<AFAGFHJONFJ> sources){
  var world=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;var destination=w.GBHGENNMJCK;string faction=f.IBGCLCAIBOL,foe=w.EMMJBPMNFAK[1-side];
  bool Valid()=>Ready()&&CurrentFaction(f)&&MICGPAMNDKM.IJPKEMPIFIH?.Pointer==world&&battles.TryGetValue(w.Pointer,out var current)&&ReferenceEquals(current,state)&&w.HIENOINGBCN==state.Name&&IsDirect(w)==direct&&w.EMMJBPMNFAK!=null&&w.EMMJBPMNFAK.Length==2&&w.EMMJBPMNFAK[side]==faction&&w.EMMJBPMNFAK[1-side]==foe&&War(f,foe)&&Vector3.Distance(destination,w.GBHGENNMJCK)<.01f&&Soldiers(w,0,direct)>0&&Soldiers(w,1,direct)>0;
  if(!Valid())yield break;
  RefreshBattleCorridors(AttackTeams());
  long routeStarted=Environment.TickCount64;int reused=0,computed=0;
  string roadStamp=RoadPolicyStamp(f);var routes=new Dictionary<string,double>();var skipped=new List<string>();
  // Finish the candidate batch before ranking. Pending is not unreachable.
  // Each native path/graph operation yields under the existing frame budget.
  foreach(var city in sources){
   yield return "source-route";if(!Valid())yield break;
   if(!DistributionCity(city,out var owner)||owner.Pointer!=f.Pointer)continue;
   string key=w.Pointer+":"+w.HIENOINGBCN+":"+faction+":"+city.OJPECMNINNA;
   if(!SavedBattleQuote(city,w,f,key,roadStamp,out _)){computed++;
    foreach(var phase in BattleRouteQuote(city,w,f,key)){yield return phase;if(!Valid())yield break;}
   }
   else reused++;
   if(RoadPolicyStamp(f)!=roadStamp){state.Next=state.SideNext[side]=double.NegativeInfinity;Note($"battle-road-replan war={w.HIENOINGBCN} faction={faction} reason=policy-changed-during-quotes");yield break;}
   if(battleRoutes.TryGetValue(key,out var quote)&&quote.Stamp==roadStamp&&double.IsFinite(quote.Eta))routes[city.OJPECMNINNA]=quote.Eta;
   else {skipped.Add(city.OJPECMNINNA+":unreachable-or-changed");RequestCorridorBreakthrough(city,w,f);}
  }
  Note($"battle-road-cache war={w.HIENOINGBCN} faction={faction} reused={reused} calculated={computed} elapsedMs={Environment.TickCount64-routeStarted} next=rank-current-forces-by-road-eta");
  sources.RemoveAll(c=>!routes.ContainsKey(c.OJPECMNINNA));
  sources.Sort((a,b)=>{int n=routes[a.OJPECMNINNA].CompareTo(routes[b.OJPECMNINNA]);return n!=0?n:string.CompareOrdinal(a.OJPECMNINNA,b.OJPECMNINNA);});
  if(Gate("support-road-order:"+w.Pointer+":"+faction,10))Note($"battle-road-order war={w.HIENOINGBCN} faction={faction} pending=0 ordered=[{string.Join(";",sources.Select(c=>c.OJPECMNINNA+":"+routes[c.OJPECMNINNA].ToString("F1")))}] excluded=[{string.Join(";",skipped)}] basis=validated-road-eta");
  skipped.Clear();int rank=0;
  foreach(var c in sources){
   yield return "source-decision";if(!Valid())yield break;
   if(RoadPolicyStamp(f)!=roadStamp){state.Next=state.SideNext[side]=double.NegativeInfinity;Note($"battle-road-replan war={w.HIENOINGBCN} faction={faction} reason=ownership-or-passage-changed");yield break;}rank++;
   // No combat, troop, reservation or food snapshot is retained across yields.
   CleanMissions();var teams=AttackTeams();RefreshBattleCorridors(teams);var reserved=AttackReserved(teams);var loss=state.Loss[side];
   int own=Soldiers(w,side,direct),enemyCount=Soldiers(w,1-side,direct);double eta=routes[c.OJPECMNINNA];
   void Block(string why){skipped.Add(c.OJPECMNINNA+":"+why);if(Gate("battle-block:"+w.HIENOINGBCN+":"+c.OJPECMNINNA+":"+why,30))Note($"battle-support-blocked war={w.HIENOINGBCN} faction={faction} source={c.OJPECMNINNA} rank={rank} eta={eta:F1} reason={why}");}
   string sourceBlock=SupportSourceRules(c,teams,reserved,true);if(sourceBlock!=null){Block(sourceBlock);continue;}
   if(c.LMPMNLLGOEC.LMMNKIOAGLC!=faction){Block("ownership-changed");continue;}
   if(eta>maxTravelSeconds.Value){Block("arrival-too-late");continue;}
   var friends=new List<ReinforcementTimeline.Wave>();var enemies=new List<ReinforcementTimeline.Wave>();
   foreach(var t in teams){
    if(t.m_warInfo!=null)continue;
    if(t.m_belongPower==faction&&Same(t.m_targetWarInfo,w)&&CorridorArrivalClear(t,w,f)){
     double at=Travel(t,w);if(at<=maxTravelSeconds.Value&&double.IsFinite(TeamArrival(t,w.GBHGENNMJCK)))friends.Add(new(at,Math.Max(0,t.GetSoldierNum(false))));
    }else if(t.m_belongPower==foe&&(Same(t.m_targetWarInfo,w)||(w.IJGLCFJOOIK!=null&&t.m_targetCity==w.IJGLCFJOOIK.OJPECMNINNA))){
     double at=Travel(t,w);if(at<=maxTravelSeconds.Value&&double.IsFinite(TeamArrival(t,w.GBHGENNMJCK)))enemies.Add(new(at,Math.Max(0,t.GetSoldierNum(false))));
    }
   }
   if(direct){friends.AddRange(TimedReserveWaves(w,side,maxTravelSeconds.Value));enemies.AddRange(TimedReserveWaves(w,1-side,maxTravelSeconds.Value));}
   double horizon=Math.Max(eta,Math.Min(eta+30,enemies.Count>0?enemies.Max(x=>x.At):eta));
   int need=ReinforcementTimeline.Need(own,enemyCount,loss.FriendlyLoss,loss.EnemyLoss,eta,horizon,friends,enemies,battleCoverage.Value,0,out string reason,out double hold);
   if(Gate("battle-relay:"+w.HIENOINGBCN+":"+side+":"+c.OJPECMNINNA,15))Note($"battle-relay-forecast war={w.HIENOINGBCN} faction={faction} source={c.OJPECMNINNA} eta={eta:F1} holdSeconds={hold:F1} need={need} friendlyWaves={friends.Count} enemyWaves={enemies.Count} reason={reason}");
   if(need<=0){Block(reason);continue;}
   double now=state.LastClock;state.Next=Math.Min(state.Next,now+BattleRetryDelay(assessSeconds.Value,AOHIBKLOAEI.NIOKFCDLGFK()));
   state.SideNext[side]=Math.Min(state.SideNext[side],now+DecisionInterval(faction,BattleRetryDelay(assessSeconds.Value,AOHIBKLOAEI.NIOKFCDLGFK())));
   var sent=DispatchBattle(c,w,side,need,eta,direct,teams,reserved,actual=>ReinforcementTimeline.Need(own,enemyCount,loss.FriendlyLoss,loss.EnemyLoss,actual,Math.Max(actual,horizon),friends,enemies,battleCoverage.Value,0,out _,out _)>0);
   if(!sent){Block("assembly-or-route-rejected");continue;}
   Note($"battle-support team={sent.m_id} sourceGovernor={c.LMPMNLLGOEC.IPJHOOJBCKI} targetGovernor={w.IJGLCFJOOIK?.LMPMNLLGOEC?.IPJHOOJBCKI} war={w.HIENOINGBCN} faction={faction} source={c.OJPECMNINNA} own={own} enemy={enemyCount} lossPerSec={loss.FriendlyLoss:F1} enemyLossPerSec={loss.EnemyLoss:F1} etaEstimate={eta:F1} needSoldiers={need} sentSoldiers={sent.GetSoldierNum(false)} roadRank={rank} departureRequiresLeaveCity=true nearerResults=[{string.Join(";",skipped)}]");
   skipped.Add(c.OJPECMNINNA+":dispatched");
  }
 }
}
