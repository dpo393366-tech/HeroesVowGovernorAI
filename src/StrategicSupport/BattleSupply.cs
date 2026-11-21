using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.AI;

public sealed partial class StrategicSupport
{
    static ConfigEntry<bool> autoSupply;
    static ConfigEntry<float> supplyCover,supplyBuffer,supplyMaxTravel;
    sealed class FoodWatch {
        public Wo_DeployTeamCtrl Team;public int Id;public SupplyObservation Trend=new();
    }
    sealed class FoodOrder {
        public FoodWatch Watch;public AFAGFHJONFJ City;public string CityId,Faction;
        public int Amount;public double Due,RouteSeconds;public Vector3 Destination;
    }
    sealed class FoodBattle {
        public NJHDEHLHENN War;public string Name;public bool Direct;public double Next,Last;
        public readonly Dictionary<IntPtr,FoodWatch> Watches=new();
        public readonly List<FoodOrder> Orders=new();
        public readonly int[] Cursor=new int[2];
        public readonly double[] NextDecision={double.NegativeInfinity,double.NegativeInfinity};
        public readonly int[] DecisionPeriod=new int[2];
    }
    static readonly Dictionary<IntPtr,FoodBattle> foodBattles=new();
    static bool foodBusy;
    void ConfigureSupply()
    {
        autoSupply=Config.Bind("BattleSupply","Enabled",true,"Predict and deliver food from same-faction cities, including player armies. Timed reservations; city debit and army credit commit together at arrival. No physical convoy object.");
        supplyCover=Config.Bind("BattleSupply","CoverageSeconds",120f,new ConfigDescription("Target food endurance at arrival, in normal-speed simulation seconds.",new AcceptableValueRange<float>(30,600)));
        supplyBuffer=Config.Bind("BattleSupply","EarlyRequestSeconds",30f,new ConfigDescription("Request before predicted exhaustion by ETA plus this safety buffer.",new AcceptableValueRange<float>(10,120)));
        supplyMaxTravel=Config.Bind("BattleSupply","MaxTravelSeconds",600f,new ConfigDescription("Longest accepted supply route including preparation time.",new AcceptableValueRange<float>(30,1800)));
    }
    static void ResetSupply()=>foodBattles.Clear();
    static void ForgetSupply(NJHDEHLHENN war){if(war!=null)foodBattles.Remove(war.Pointer);}
    static long ReservedFood(string id,FoodOrder except=null)
    {
        long n=0;foreach(var b in foodBattles.Values)foreach(var o in b.Orders)
            if(!ReferenceEquals(o,except)&&o.CityId==id)n+=o.Amount;
        return n;
    }
    static long CityFoodFloor(AFAGFHJONFJ city,string faction)
    {
        long soldiers=0;var troops=city.LMPMNLLGOEC.KJPPGIOCDID;
        if(troops!=null)foreach(var t in troops)if(t!=null)soldiers+=Math.Max(0,t.FGHFOLKBHDL);
        bool front=false;
        if(city.FKMKGNPFLFP!=null)foreach(var n in city.FKMKGNPFLFP)
            if(n?.LMPMNLLGOEC!=null&&NEINKOOBJIH.COCMABJFOFH.TryGetValue(faction,out var own)&&War(own,n.LMPMNLLGOEC.LMMNKIOAGLC)){front=true;break;}
        return Math.Max(10000,soldiers*(front?60L:30L));
    }
    static bool SafeFoodCity(AFAGFHJONFJ c,string faction)
    {
        if(c?.LMPMNLLGOEC?.LMMNKIOAGLC!=faction||c.GKLIEHCHDLD())return false;
        if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(faction,out var owner)||owner==null)return false;
        if(ICGAEMLACJG.HFJIECIBFKB!=null)foreach(var kv in ICGAEMLACJG.HFJIECIBFKB){var t=kv.Value;
            if(t&&War(owner,t.m_belongPower)&&(t.m_targetCity==c.OJPECMNINNA||t.m_warInfo?.IJGLCFJOOIK?.OJPECMNINNA==c.OJPECMNINNA))return false;
        }
        return true;
    }
    static int FoodSpare(AFAGFHJONFJ c,string faction,FoodOrder except=null)
        =>SafeFoodCity(c,faction)?SupplyPolicy.Available(c.LMPMNLLGOEC.OEGNGFBCHFL,CityFoodFloor(c,faction),ReservedFood(c.OJPECMNINNA,except)):0;
    static bool FoodMember(FoodBattle b,FoodWatch watch)
    {
        var t=watch.Team;if(!t||t.m_id!=watch.Id||!Same(t.m_warInfo,b.War)||t.GetSoldierNum(false)<=0)return false;
        if(b.War.HIENOINGBCN!=b.Name||b.War.LGPMOKOLMLN==null)return false;
        foreach(var group in b.War.LGPMOKOLMLN)if(group!=null)foreach(var member in group)
            if(member&&member.Pointer==t.Pointer&&member.m_id==watch.Id)return true;
        return false;
    }
    static bool SupplyRoute(AFAGFHJONFJ city,NJHDEHLHENN war,string faction,out double eta)
    {
        eta=double.PositiveInfinity;
        double speed=Wo_TransportTeamCtrl.GetMoveSpeed(faction);
        if(!double.IsFinite(speed)||speed<=0)return false;
        if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(faction,out var owner)||owner==null)return false;
        if(!ReliefPath(city.BMBMKPGEHBH,war.GBHGENNMJCK,owner,BattleTransitRegion(war,owner),false,out _,out double length))return false;
        eta=10+length/speed;
        return double.IsFinite(eta)&&eta<=supplyMaxTravel.Value;
    }
    static void SupplyTick(NJHDEHLHENN war,double now,bool direct)
    {
        if(!autoSupply.Value){ResetSupply();return;}
        if(foodBusy||war==null||!double.IsFinite(now)||war.EMMJBPMNFAK==null||war.EMMJBPMNFAK.Length!=2||war.LGPMOKOLMLN==null)return;
        foodBusy=true;
        try{
            if(!foodBattles.TryGetValue(war.Pointer,out var b)||b.Name!=war.HIENOINGBCN||b.Direct!=direct||now<b.Last)
                foodBattles[war.Pointer]=b=new FoodBattle{War=war,Name=war.HIENOINGBCN,Direct=direct};
            b.Last=now;if(now<b.Next)return;b.Next=now+5;
            // Each queued quantity is only a reservation. Cancellation, loading another save,
            // leaving battle, or a mode change cannot lose/duplicate food. Native city expenses
            // can consume reservations, so delivery revalidates current stock and defense needs.
            bool delivered=false;var deferredCredits=new HashSet<IntPtr>();
            for(int i=b.Orders.Count-1;i>=0;i--){var o=b.Orders[i];
                if(!FoodMember(b,o.Watch)||o.Watch.Team.m_belongPower!=o.Faction
                    ||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(o.CityId,out var current)||current==null||current.Pointer!=o.City.Pointer
                    ||!SafeFoodCity(current,o.Faction)||Vector3.Distance(o.Destination,war.GBHGENNMJCK)>5){b.Orders.RemoveAt(i);continue;}
                if(now<o.Due)continue;
                // A changed territorial route invalidates the order instead of teleporting cargo.
                if(!SupplyRoute(current,war,o.Faction,out double currentRoute)){b.Orders.RemoveAt(i);continue;}
                // A newly opened detour or slower transport cannot retain the
                // old shorter ETA. Faster routes never create an early delivery.
                if(currentRoute>o.RouteSeconds+.01){o.Due+=currentRoute-o.RouteSeconds;o.RouteSeconds=currentRoute;if(now<o.Due)continue;}
                var team=o.Watch.Team;int beforeFood=Math.Max(0,team.GetFood());
                double burn=Math.Max(o.Watch.Trend.Burn,Math.Max(0,team.GetOneDayFood())/Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK()));
                int need=(int)Math.Clamp(Math.Ceiling(burn*supplyCover.Value-beforeFood),0,int.MaxValue);
                int amount=Math.Min(o.Amount,Math.Min(need,Math.Min(FoodSpare(current,o.Faction,o),int.MaxValue-beforeFood)));
                b.Orders.RemoveAt(i); // Remove before credit; a callback/retry cannot credit twice.
                if(amount<=0)continue;
                int stock=current.LMPMNLLGOEC.OEGNGFBCHFL;
                current.LMPMNLLGOEC.OEGNGFBCHFL=stock-amount;
                Exception creditError=null;
                try{team.AddFood(amount);}catch(Exception e){creditError=e;}
                // A native callback can throw AFTER changing army food, or clamp
                // the credit. Reconcile observed delivery rather than blindly
                // refunding the entire debit and duplicating already delivered food.
                int credited=(int)Math.Clamp((long)team.GetFood()-beforeFood,0,amount);
                int refund=amount-credited;
                if(refund>0)current.LMPMNLLGOEC.OEGNGFBCHFL=(int)Math.Min(int.MaxValue,(long)current.LMPMNLLGOEC.OEGNGFBCHFL+refund);
                if(credited>0){o.Watch.Trend.Credit(credited);delivered=true;}
                if(creditError!=null||credited<amount)deferredCredits.Add(team.Pointer);
                if(creditError!=null)Error(creditError);
                Note($"supply-arrive war={b.Name} team={o.Watch.Id} source={o.CityId} food={credited} cityRemaining={current.LMPMNLLGOEC.OEGNGFBCHFL} delayedSeconds={Math.Max(0,now-o.Due):F1} creditException={creditError!=null}");
            }
            if(delivered&&direct&&IsDirect(war)&&Wa_Controller.m_battleUI!=null)Wa_Controller.RefreshFoodUI();
            var seen=new HashSet<IntPtr>();var requests=new List<FoodWatch>();
            for(int side=0;side<Math.Min(2,war.LGPMOKOLMLN.Length);side++){
                var group=war.LGPMOKOLMLN[side];if(group==null)continue;
                string sideFaction=war.EMMJBPMNFAK[side];int period=DecisionDays(sideFaction);
                bool assess=b.DecisionPeriod[side]!=period||now>=b.NextDecision[side];
                if(assess){b.DecisionPeriod[side]=period;b.NextDecision[side]=now+DecisionInterval(sideFaction,5);}
                foreach(var t in group){if(!t||!Same(t.m_warInfo,war)||t.m_belongPower!=war.EMMJBPMNFAK[side]||!seen.Add(t.Pointer))continue;
                    if(!b.Watches.TryGetValue(t.Pointer,out var watch)||watch.Id!=t.m_id)b.Watches[t.Pointer]=watch=new FoodWatch{Team=t,Id=t.m_id};
                    watch.Trend.Sample(now,t.GetFood(),t.GetSoldierNum(false),t.GetMorale(),Math.Max(0,t.GetOneDayFood())/Math.Max(.1,AOHIBKLOAEI.NIOKFCDLGFK()));
                    bool pending=false;foreach(var o in b.Orders)if(ReferenceEquals(o.Watch,watch)){pending=true;break;}
                    if(assess&&!pending&&watch.Trend.Soldiers>0&&!deferredCredits.Contains(t.Pointer))requests.Add(watch);
                }
            }
            var obsolete=new List<IntPtr>();foreach(var p in b.Watches)if(!seen.Contains(p.Key))obsolete.Add(p.Key);foreach(var key in obsolete)b.Watches.Remove(key);
            requests.Sort((a,z)=>SupplyPolicy.Priority(a.Trend).CompareTo(SupplyPolicy.Priority(z.Trend)));
            // Bounded path work per assessment, across both sides. Requests do not require troop losses.
            int routes=0,ordered=0;var sideRoutes=new int[2];
            foreach(var watch in requests){if(routes>=6||ordered>=2||b.Orders.Count>=16)break;
                string faction=watch.Team.m_belongPower;int side=war.EMMJBPMNFAK[0]==faction?0:1;
                if(sideRoutes[side]>=3||SupplyPolicy.Priority(watch.Trend)>supplyMaxTravel.Value+supplyBuffer.Value)continue;
                var cities=new List<AFAGFHJONFJ>();foreach(var kv in MICGPAMNDKM.IJPKEMPIFIH)if(kv.Value?.LMPMNLLGOEC?.LMMNKIOAGLC==faction)cities.Add(kv.Value);
                cities=cities.Select(c=>(City:c,Eta:SupplyRank(c,war,faction))).Where(x=>double.IsFinite(x.Eta)).OrderBy(x=>x.Eta).Select(x=>x.City).ToList();
                FoodOrder best=null;double bestEta=double.PositiveInfinity;int inspected=0;
                for(int j=0;j<cities.Count&&routes<6&&sideRoutes[side]<3&&inspected<3;j++){
                    var city=cities[j];int spare=FoodSpare(city,faction);if(spare<=0)continue;
                    routes++;sideRoutes[side]++;inspected++;if(!SupplyRoute(city,war,faction,out double eta))continue;
                    int amount=SupplyPolicy.Amount(watch.Trend,eta,supplyCover.Value,supplyBuffer.Value,spare);
                    if(amount<=0||eta>=bestEta)continue;
                    bestEta=eta;best=new FoodOrder{Watch=watch,City=city,CityId=city.OJPECMNINNA,Faction=faction,Amount=amount,Due=now+eta,RouteSeconds=eta,Destination=war.GBHGENNMJCK};
                }
                if(cities.Count>0)b.Cursor[side]=(b.Cursor[side]+Math.Max(1,inspected))%cities.Count;
                if(best==null)continue;b.Orders.Add(best);ordered++;
                Note($"supply-request war={b.Name} team={watch.Id} source={best.CityId} food={best.Amount} eta={bestEta:F1}s endurance={watch.Trend.Food/Math.Max(.001,watch.Trend.Burn):F1}s morale={watch.Trend.Morale} moraleLoss={watch.Trend.MoraleLoss:F3}/s soldierLoss={watch.Trend.Loss:F3}/s");
            }
        }catch(Exception e){Error(e);}finally{foodBusy=false;}
    }
}
