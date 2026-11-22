using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

public sealed partial class StrategicSupport
{
    static ConfigEntry<bool> battleSupport;
    static ConfigEntry<float> assessSeconds, maxTravelSeconds, mobilizeSeconds, battleCoverage;
    sealed class BattleState {
        public string Name;public bool Direct;public double Next,LastClock=double.NegativeInfinity,LastSample=double.NegativeInfinity;
        public bool CapturedOriginals;public readonly HashSet<int> OriginalTeams=new();
        public readonly AttritionWindow[] Loss={new(),new()};
        public readonly double[] SideNext={double.NegativeInfinity,double.NegativeInfinity};
        public readonly int[] SidePeriod=new int[2];
        public readonly bool[] RouteAwaiting=new bool[2];
    }
    sealed class Mission {
        public Wo_DeployTeamCtrl Team;public NJHDEHLHENN War;public int Id,Side;
        public string Name,Faction,Source;public double ReadyAt;
        public bool Direct,Departed;public string ProbeSignature;public double ProbeEta;public long ProbeFood;
        public RoutePlan FailedCorridor;public long FailedCorridorRevision;public string FailedCorridorPolicy;public Vector3 FailedCorridorStart;public List<Vector3> PlannedRoad;public long DepartureTick;public double DepartureDay;public Vector3 DeparturePosition;
    }
    static readonly Dictionary<IntPtr,BattleState> battles=new();
    static readonly Dictionary<IntPtr,Mission> missions=new();
    static readonly List<IntPtr> removeMissions=new();
    static readonly List<Mission> moving=new();
    static IntPtr directWar;
    static double battleClock,nextMissionPoll;
    static bool battleBusy;
    void ConfigureBattle()
    {
        battleSupport=Config.Bind("BattleSupport","Enabled",true,"Send real regular troops to an ongoing losing battle, including direct command.");
        assessSeconds=Config.Bind("BattleSupport","AssessmentSeconds",10f,new ConfigDescription("Observation interval in normal-speed simulation seconds.",new AcceptableValueRange<float>(5,60)));
        maxTravelSeconds=Config.Bind("BattleSupport","MaxTravelSeconds",300f,new ConfigDescription("Maximum estimated mobilization and travel time.",new AcceptableValueRange<float>(30,900)));
        mobilizeSeconds=Config.Bind("BattleSupport","MobilizationSeconds",10f,new ConfigDescription("Minimum direct-command preparation time before movement.",new AcceptableValueRange<float>(0,120)));
        battleCoverage=Config.Bind("BattleSupport","TargetRatio",1.15f,new ConfigDescription("Projected friendly/enemy troop ratio at arrival.",new AcceptableValueRange<float>(1,1.5f)));
    }
    static void HookBattle(Harmony h)
    {
        HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
        h.Patch(AccessTools.Method(typeof(NJHDEHLHENN),"CDNDKCKEBEJ"),postfix:H(nameof(WorldBattleTick)));
        h.Patch(AccessTools.Method(typeof(NJHDEHLHENN),"Recycle"),prefix:H(nameof(ForgetBattle)));
        h.Patch(AccessTools.Method(typeof(Wa_Controller),"Update"),postfix:H(nameof(DirectBattleTick)));
        h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"Update"),prefix:H(nameof(OwnedMovement)));
        h.Patch(AccessTools.Method(typeof(Wo_DeployTeamCtrl),"UpdateHUDPosition"),prefix:H(nameof(BeforeWorldHud)));
        h.Patch(AccessTools.Method(typeof(Wo_VillageCtrl),"OnTriggerEnter"),prefix:H(nameof(BeforeVillageTrigger)));
    }
    static bool Same(NJHDEHLHENN a,NJHDEHLHENN b)=>a!=null&&b!=null&&a.Pointer==b.Pointer;
    static bool IsDirect(NJHDEHLHENN w)=>Same(Wa_Controller.m_warInfo,w)&&!Wa_Controller.m_isQuiting;
    static void ResetBattleState(){ResetBattleCorridors();ResetSupply();rejectedBattleDispatches.Clear();emergencyBudgets.Clear();battles.Clear();missions.Clear();directWar=IntPtr.Zero;battleClock=0;nextMissionPoll=0;}
    static void ForgetBattle(NJHDEHLHENN __instance)
    {
        if(__instance==null)return;
        ForgetCorridor(__instance);battles.Remove(__instance.Pointer);ForgetSupply(__instance);ForgetBattleDispatchRetries(__instance);
        // Hand any still-travelling army back to native destination/lifecycle handling.
        removeMissions.Clear();foreach(var p in missions)if(Same(p.Value.War,__instance))removeMissions.Add(p.Key);
        foreach(var key in removeMissions)missions.Remove(key);
    }
    static void WorldBattleTick(NJHDEHLHENN __instance)
    {
        if(!enabled.Value||(!battleSupport.Value&&!autoSupply.Value)||__instance==null||IsDirect(__instance))return;
        try { if(Ready()){double now=NEINKOOBJIH.MIPFHCKDJOK*(double)AOHIBKLOAEI.NIOKFCDLGFK();if(battleSupport.Value)Assess(__instance,now,false);SupplyTick(__instance,now,false);} }
        catch(Exception e){Error(e);}
    }
    static void DirectBattleTick()
    {
        try {
            var w=Wa_Controller.m_warInfo;
            if(w==null||Wa_Controller.m_isQuiting){directWar=IntPtr.Zero;return;}
            if(!enabled.Value||(!battleSupport.Value&&!autoSupply.Value)||!Ready())return;
            if(Wa_Controller.m_isBeforeWar||Wa_Controller.m_isByEvent||Wa_Controller.m_mockInfo!=null||Time.timeScale<=0||Wa_Controller.m_battleSpeed<=0)return;
            if(!Wa_Controller.m_isRoundMode&&AEHAGJAKNEK.PAKENCLGOHK)return;
            if(directWar!=w.Pointer){directWar=w.Pointer;battleClock=0;nextMissionPoll=0;battles.Remove(w.Pointer);}
            float speed=Wa_Controller.m_isRoundMode?GLIJMELMEHI.OHJCDAPNAOI switch {0=>.5f,2=>2f,_=>1f}:AEHAGJAKNEK.IAICEJLNKFJ;
            if(speed<=0)return;
            double dt=Time.unscaledDeltaTime*speed;battleClock+=dt;
            // Only our newly ordered reinforcements move here while the world is paused.
            // The native movement routine consumes its own path and calls FinishMove/JoinWar.
            if(battleSupport.Value){AdvanceMissions(w,speed);Assess(w,battleClock,true);}
            SupplyTick(w,battleClock,true);
        }catch(Exception e){Error(e);}
    }
    static bool OwnedMovement(Wo_DeployTeamCtrl __instance)
    {
        if(!enabled.Value||(!battleSupport.Value&&!autoSupply.Value)||__instance==null)return true;
        if(!missions.TryGetValue(__instance.Pointer,out var m)||!ValidMission(m))return true;
        // Do not intercept initialization/assembly, battles, or any replacement order.
        return !(IsDirect(m.War)&&__instance.m_warInfo==null&&Same(__instance.m_targetWarInfo,m.War)
            &&__instance.m_deployState==AKOPKFPKJPP.Expedition);
    }
    static void AdvanceMissions(NJHDEHLHENN w,float speed)
    {
        // Native arrival may recycle/merge an army or end the war. Never enumerate
        // the dictionary across that callback; reuse a snapshot without frame allocations.
        moving.Clear();foreach(var p in missions)moving.Add(p.Value);
        foreach(var m in moving){var t=m.Team;
            if(!ValidMission(m)){if(t!=null)missions.Remove(t.Pointer);continue;}
            if(!missions.TryGetValue(t.Pointer,out var current)||!ReferenceEquals(current,m))continue;
            if(!Same(m.War,w))continue;
            if(!m.Direct){m.Direct=true;m.ReadyAt=battleClock+mobilizeSeconds.Value;}
            if(battleClock<m.ReadyAt||t.m_isStarting||t.m_deployState!=AKOPKFPKJPP.Expedition||Time.deltaTime<=0)continue;
            if(t.m_listMovePos==null||t.m_listMovePos.Count==0)continue;
            // UpdateMove multiplies m_curSpeed by deltaTime; compensate Unity timeScale
            // so battle speed is applied exactly once, including 2x.
            float previous=t.m_curSpeed;
            try {t.m_curSpeed=t.GetMoveSpeed()*speed*Time.unscaledDeltaTime/Time.deltaTime;t.UpdateMove();}
            finally{if(t&&t.m_id==m.Id)t.m_curSpeed=previous;}
        }
        if(battleClock>=nextMissionPoll){nextMissionPoll=battleClock+10;
            if(missions.Count>0)Note($"battle-transit war={w.HIENOINGBCN} pending={missions.Count} battleSeconds={battleClock:F1}");}
    }
    static bool ValidMission(Mission m)=>m.Team&&m.Team.m_id==m.Id&&m.Team.m_belongPower==m.Faction
        &&m.War!=null&&m.War.HIENOINGBCN==m.Name&&m.Team.m_warInfo==null&&Same(m.Team.m_targetWarInfo,m.War);
    static void CleanMissions()
    {
        removeMissions.Clear();foreach(var p in missions)if(!ValidMission(p.Value)){ObserveCorridorOutcome(p.Value);removeMissions.Add(p.Key);}
        foreach(var p in removeMissions)missions.Remove(p);
    }
    static int Soldiers(NJHDEHLHENN w,int side,bool direct)
    {
        if(direct){int actual=ActiveBattleSoldiers(w,side);if(actual>=0)return actual;int my=w.MLFBHOOGBNA();if(my<0||my>1)return 0;
            long n=Math.Max(0,side==my?Wa_Controller.m_myCurSoldierNum:Wa_Controller.m_enemyCurSoldierNum);
            return (int)Math.Min(int.MaxValue,n);}
        return w.FGHFOLKBHDL!=null&&w.FGHFOLKBHDL.Length>side?Math.Max(0,w.FGHFOLKBHDL[side]):0;
    }
    static void Assess(NJHDEHLHENN w,double now,bool direct)
    {
        if(battleBusy||busy||w.EMMJBPMNFAK==null||w.EMMJBPMNFAK.Length<2||w.LGPMOKOLMLN==null)return;
        if(!battles.TryGetValue(w.Pointer,out var state)||state.Name!=w.HIENOINGBCN||state.Direct!=direct||now<state.LastClock)
            battles[w.Pointer]=state=new BattleState{Name=w.HIENOINGBCN,Direct=direct};
        state.LastClock=now;
        // Combat sampling follows simulation seconds; expensive source planning retains
        // its player/NPC cadence and runs incrementally on the existing queue.
        double observationPeriod=direct?1:assessSeconds.Value;
        if(now<state.Next)return;state.Next=now+observationPeriod;
        CleanMissions();
        int a=Soldiers(w,0,direct),b=Soldiers(w,1,direct);if(a<=0||b<=0)return;
        if(now>state.LastSample){int lossA=direct?BattleLossReading(w,0,a):a,lossB=direct?BattleLossReading(w,1,b):b;state.Loss[0].Sample(now,lossA,lossB);state.Loss[1].Sample(now,lossB,lossA);state.LastSample=now;}
        battleBusy=true;
        try {
            var teams=new List<Wo_DeployTeamCtrl>();var reserved=new HashSet<IntPtr>();
            if(ICGAEMLACJG.HFJIECIBFKB==null)return;
            foreach(var kv in ICGAEMLACJG.HFJIECIBFKB)if(kv.Value){teams.Add(kv.Value);Reserve(kv.Value,reserved);}
            RefreshBattleCorridors(teams);
            // Register already marching native reinforcements before the
            // per-side decision gate: the gate must not delay their movement.
            if(direct)foreach(var t in teams)if(t.m_warInfo==null&&Same(t.m_targetWarInfo,w)&&!t.m_isPrivate&&t.m_deployState==AKOPKFPKJPP.Expedition&&!missions.ContainsKey(t.Pointer)){
                int side=w.EMMJBPMNFAK[0]==t.m_belongPower?0:w.EMMJBPMNFAK[1]==t.m_belongPower?1:-1;
                if(side>=0&&side<2)missions[t.Pointer]=new Mission{Team=t,Id=t.m_id,War=w,Name=w.HIENOINGBCN,Side=side,Faction=t.m_belongPower,Source=t.m_assembleCity,Direct=true,ReadyAt=battleClock};
            }
            if(!state.CapturedOriginals){foreach(var t in teams)if(Same(t.m_warInfo,w))state.OriginalTeams.Add(t.m_id);state.CapturedOriginals=true;}
            for(int side=0;side<2;side++){
                var loss=state.Loss[side];if(loss.Samples<2){
                 if(Gate("battle-observe:"+w.HIENOINGBCN+":"+side,10))Note($"battle-initial-assessment war={w.HIENOINGBCN} side={side} samples={loss.Samples} gameDay={NEINKOOBJIH.MIPFHCKDJOK:F4} basis=current-strength attritionUnobserved=true");
                }
                string faction=w.EMMJBPMNFAK[side],foe=w.EMMJBPMNFAK[1-side];
                int period=DecisionDays(faction);
                if(state.SidePeriod[side]==period&&now<state.SideNext[side])continue;
                state.SidePeriod[side]=period;state.SideNext[side]=now+DecisionInterval(faction,direct?2:assessSeconds.Value);
                if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(faction,out var f)||f==null||!War(f,foe))continue;
                int own=side==0?a:b,enemyCount=side==0?b:a;
                if(own<enemyCount){
                    double retry=DecisionInterval(faction,BattleRetryDelay(assessSeconds.Value,AOHIBKLOAEI.NIOKFCDLGFK()));
                    state.SideNext[side]=Math.Min(state.SideNext[side],now+retry);state.Next=Math.Min(state.Next,now+retry);
                }
                // Include native support armies too, not only those created by this mod.
                int incoming=0,pending=0,totalPending=0;
                var friendlyWaves=new List<ReinforcementTimeline.Wave>();
                var enemyWaves=new List<ReinforcementTimeline.Wave>();
                foreach(var t in teams)if(t.m_belongPower==faction&&t.m_warInfo==null&&Same(t.m_targetWarInfo,w)&&CorridorArrivalClear(t,w,f)){
                    // Already dispatched native support must also keep travelling
                    // while direct command pauses the world; its destination is untouched.
                    totalPending++;double eta=Travel(t,w);
                    if(eta<maxTravelSeconds.Value&&double.IsFinite(TeamArrival(t,w.GBHGENNMJCK)))
                        {pending++;incoming=(int)Math.Min(int.MaxValue,(long)incoming+Math.Max(0,t.GetSoldierNum(false)));friendlyWaves.Add(new(eta,Math.Max(0,t.GetSoldierNum(false))));}
                }
                if(Gate("battle-assess:"+w.HIENOINGBCN+":"+side,10))Note($"battle-assess war={w.HIENOINGBCN} faction={faction} direct={direct} own={own} enemy={enemyCount} lossPerSec={loss.FriendlyLoss:F1} incoming={incoming} timelyPending={pending} totalPending={totalPending} originals={state.OriginalTeams.Count}");
                // Re-evaluate every feasible wave; two small near-city armies must not
                // block a larger, later wave that they can keep the battle alive for.
                foreach(var enemyTeam in teams)if(enemyTeam.m_belongPower==foe&&enemyTeam.m_warInfo==null&&(Same(enemyTeam.m_targetWarInfo,w)||(w.IJGLCFJOOIK!=null&&enemyTeam.m_targetCity==w.IJGLCFJOOIK.OJPECMNINNA))){
                    double at=Travel(enemyTeam,w);if(double.IsFinite(at)&&at<=maxTravelSeconds.Value&&double.IsFinite(TeamArrival(enemyTeam,w.GBHGENNMJCK)))enemyWaves.Add(new(at,Math.Max(0,enemyTeam.GetSoldierNum(false))));
                }
                if(direct){friendlyWaves.AddRange(TimedReserveWaves(w,side,maxTravelSeconds.Value));enemyWaves.AddRange(TimedReserveWaves(w,1-side,maxTravelSeconds.Value));}
                if(loss.FriendlyLoss<=0&&enemyWaves.Count==0&&own>=enemyCount*battleCoverage.Value)continue;
                int selectedSide=side;
                if(schedulerOnline){
                    long queuedAt=Environment.TickCount64;
                    bool added=work.Add("battle-support-plan:"+w.Pointer+":"+faction,()=>ObservedBattlePlan(w,state,selectedSide,direct,f,queuedAt).GetEnumerator());
                    if(direct&&Gate("battle-queue:"+w.Pointer+":"+faction,10))Note($"battle-plan-queue war={w.HIENOINGBCN} faction={faction} queued={added} simulationSeconds={now:F1} worldDay={NEINKOOBJIH.MIPFHCKDJOK:F4} campaignPaused={AEHAGJAKNEK.PAKENCLGOHK} queue={work.Count}");
                }
                else foreach(var phase in BattleSourceSteps(w,state,side,direct,f)){}
                work.Add("crossfront:"+w.HIENOINGBCN+":"+faction,()=>SafePhases(CrossFrontSteps(w,faction)).GetEnumerator());
            }
        }finally{battleBusy=false;}
    }
    static IEnumerable<string> ObservedBattlePlan(NJHDEHLHENN w,BattleState state,int side,bool direct,MKMOJNAJKJE faction,long queuedAt){
        long started=Environment.TickCount64;int steps=0;
        Note($"battle-plan-start war={state.Name} faction={faction.IBGCLCAIBOL} queueWaitMs={started-queuedAt}");
        using(var scan=SafePhases(BattleSourceSteps(w,state,side,direct,faction)).GetEnumerator()){
         bool more=true;while(more){long slice=System.Diagnostics.Stopwatch.GetTimestamp();int quantum=0;string phase="end";
          do{more=scan.MoveNext();if(!more)break;phase=scan.Current;steps++;quantum++;}
          while(quantum<32&&(System.Diagnostics.Stopwatch.GetTimestamp()-slice)*1000d/System.Diagnostics.Stopwatch.Frequency<.35);
          if(more)yield return phase;
         }
        }
        Note($"battle-plan-end war={state.Name} faction={faction.IBGCLCAIBOL} elapsedMs={Environment.TickCount64-started} steps={steps}");
    }
    static void Reserve(Wo_DeployTeamCtrl t,HashSet<IntPtr> reserved)
    {if(t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var a in group.Value)if(a!=null)reserved.Add(a.Pointer);}
    static double BattleRetryDelay(double normal,double daySeconds)=>Math.Min(normal,Math.Max(1,daySeconds));
    static double Travel(Wo_DeployTeamCtrl t,NJHDEHLHENN w)
    {
        double length=ArmyRemainingRoad(t,w.GBHGENNMJCK);
        double wait=missions.TryGetValue(t.Pointer,out var m)&&m.Direct?Math.Max(0,m.ReadyAt-battleClock):0;
        return BattlePolicy.Arrival(length,t.GetMoveSpeed(),wait+Math.Max(0,t.GetLeftAssembleDay())*AOHIBKLOAEI.NIOKFCDLGFK());
    }
    static Wo_DeployTeamCtrl DispatchBattle(AFAGFHJONFJ source,NJHDEHLHENN w,int side,int need,double estimate,bool direct,List<Wo_DeployTeamCtrl> teams,HashSet<IntPtr> reserved,Func<double,bool> arrivalUseful)
    {
        if(!DistributionCity(source,out var owner)||w.EMMJBPMNFAK==null||side<0||side>=w.EMMJBPMNFAK.Length||w.EMMJBPMNFAK[side]!=owner.IBGCLCAIBOL)return null;
        foreach(var t in teams){if(t.m_targetCity==source.OJPECMNINNA&&War(owner,t.m_belongPower))return null;
            if(t.m_warInfo?.IJGLCFJOOIK?.OJPECMNINNA==source.OJPECMNINNA)return null;}
        var troops=source.LMPMNLLGOEC.KJPPGIOCDID;if(troops==null)return null;
        long total=0;foreach(var a in troops)if(a!=null)total+=Math.Max(0,a.FGHFOLKBHDL);
        RefreshBattleCorridors(teams);
        if(!PreparedBattlePath(source.BMBMKPGEHBH,w,owner,false,out var corridorPoints,out double corridorLength))return null;
        Wo_DeployTeamCtrl team=null;
        string signature=BattleDispatchSignature(source,w,owner,teams);long sourceFood=source.LMPMNLLGOEC.OEGNGFBCHFL;
        if(!BattleDispatchReady(source,w,side,direct,signature,arrivalUseful))return null;
        try {
            int requested=(int)Math.Clamp(Math.Ceiling((double)need*Math.Max(1,Power(source))/Math.Max(1,total)),1,int.MaxValue);
            var previousExclusions=battleOriginalReserveExclusions;
            try {
                // The original combat army is already unavailable to the city.
                // Do not require 60% of that army to remain at home as well.
                // Later support commitments and all other fronts still count.
                battleOriginalReserveExclusions=battles.TryGetValue(w.Pointer,out var state)?state.OriginalTeams:null;
                supportGenerationCreated=false;supportGenerationFailure=null;
                team=GenerateEmergencyRelief(source,w,requested,estimate,teams,reserved);
            }finally{battleOriginalReserveExclusions=previousExclusions;}
            if(!team){if(supportGenerationCreated)RejectBattleDispatch(source,w,side,direct,signature,"assembly:"+(supportGenerationFailure??"unknown"),estimate,-1,sourceFood);return null;}
            if(team.GetMoveSpeed()<=0||!ValidateBattleDeparture(source,w,owner,out corridorPoints,out corridorLength)){RejectBattleDispatch(source,w,side,direct,signature,"assembled-route-unavailable",double.PositiveInfinity,team.m_id,sourceFood);InvalidateBattleRoad(source,w,owner);DismissSupport(team);return null;}
            double actual=corridorLength/team.GetMoveSpeed()+Math.Max(mobilizeSeconds.Value,Math.Max(0,team.GetLeftAssembleDay())*AOHIBKLOAEI.NIOKFCDLGFK());
            string rejected=actual>maxTravelSeconds.Value?"arrival-too-late":!arrivalUseful(actual)?"arrival-not-useful":!RelayFoodSafe(team,source,actual)?"journey-food":null;
            if(rejected!=null){RejectBattleDispatch(source,w,side,direct,signature,rejected,actual,team.m_id,sourceFood);if(rejected=="arrival-too-late")InvalidateBattleRoad(source,w,owner);DismissSupport(team);return null;}
            team.m_targetCity="";team.m_targetWarInfo=w;team.m_isEnterCity=false;
            int dispatchedId=team.m_id;
            missions[team.Pointer]=new Mission{Team=team,Id=team.m_id,War=w,Name=w.HIENOINGBCN,Side=side,Faction=owner.IBGCLCAIBOL,Source=source.OJPECMNINNA,Direct=direct,ReadyAt=battleClock+mobilizeSeconds.Value,ProbeSignature=signature,ProbeEta=actual,ProbeFood=sourceFood,PlannedRoad=new(corridorPoints)};
            
            if(!team.gameObject.activeSelf)team.gameObject.SetActive(true);
            // A disabled world hierarchy does not receive OnEnable in direct command.
            // Use the same startup routine once, only for this newly generated army.
            if(direct&&!team.gameObject.activeInHierarchy&&!team.m_isStarting&&team.m_deployState==AKOPKFPKJPP.Assemble)team.StartDeploy();
            if(!team||team.m_id!=dispatchedId||team.m_belongPower!=owner.IBGCLCAIBOL||!missions.TryGetValue(team.Pointer,out var committed)||committed.Id!=dispatchedId)return null;
            BackfillAfterCommit(source,w.IJGLCFJOOIK,owner);
            return team;
        }catch{if(team){missions.Remove(team.Pointer);try{DismissSupport(team);}catch{}}throw;}
    }
}
