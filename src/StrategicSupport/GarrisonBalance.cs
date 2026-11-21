using System;
using System.Collections.Generic;
using BepInEx.Configuration;
public sealed partial class StrategicSupport
{
    static ConfigEntry<bool> rebalanceGarrisons;
    static bool rebalancing;
    void ConfigureGarrisonBalance()=>rebalanceGarrisons=Config.Bind("Defense","RebalanceGarrisons",true,"Balance officer numbers and native military/civil abilities by real army travel, retaining border defense and specialist replacements.");
    static int OfficerDemand(AFAGFHJONFJ c,MKMOJNAJKJE f,int units)
    {
        bool border=false;if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n?.LMPMNLLGOEC!=null&&War(f,n.LMPMNLLGOEC.LMMNKIOAGLC)){border=true;break;}
        return Math.Clamp((Math.Max(0,units)+1)/2+(border?2:0),3,12);
    }
    static bool BalanceGarrison(AFAGFHJONFJ source)
    {
        if(!support.Value||!rebalanceGarrisons.Value||rebalancing||!Ready()||!Eligible(source,out var f)||source.GKLIEHCHDLD()
            ||!AIGate("balance-check:"+source.OJPECMNINNA,f.IBGCLCAIBOL,30))return false;
        var teams=new List<Wo_DeployTeamCtrl>();var reserved=new HashSet<IntPtr>();
        if(ICGAEMLACJG.HFJIECIBFKB!=null)foreach(var kv in ICGAEMLACJG.HFJIECIBFKB){var t=kv.Value;if(!t)continue;teams.Add(t);
            if(t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)if(group.Value!=null)foreach(var a in group.Value)if(a!=null)reserved.Add(a.Pointer);
        }
        if(!ViableSource(source,teams,reserved))return false;
        int home=HomeOfficers(source,teams).Count,units=0;foreach(var a in source.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&!reserved.Contains(a.Pointer)&&a.FGHFOLKBHDL>0)units++;
        var peers=Neighbours(source,f.IBGCLCAIBOL);
        long pool=home;int cityCount=1;var participating=new HashSet<string>{source.OJPECMNINNA};
        foreach(var city in peers){if(!Eligible(city,out var owner)||owner.IBGCLCAIBOL!=f.IBGCLCAIBOL||city.GKLIEHCHDLD()
            ||!f.ANMMMOIKLEG.TryGetValue(city.LMPMNLLGOEC.IPJHOOJBCKI,out var gov)||gov.BNGFJOAJKDH==NEFBEOFPGJA.StopWar)continue;
            if(!participating.Add(city.OJPECMNINNA))continue;pool+=HomeOfficers(city,teams).Count;cityCount++;
        }
        foreach(var t in teams)if(t.m_belongPower==f.IBGCLCAIBOL&&t.m_targetCity!=null&&participating.Contains(t.m_targetCity))pool+=t.m_listHero?.Count??0;
        int fairShare=(int)Math.Min(12,(pool+cityCount-1)/cityCount);
        int sourceDemand=Math.Max(OfficerDemand(source,f,units),Math.Max(3,fairShare-1));if(home<=sourceDemand)return false;
        // Do not redistribute again before a committed incoming transfer arrives.
        foreach(var t in teams)if(t.m_isEnterCity&&t.m_belongPower==f.IBGCLCAIBOL&&t.m_targetCity==source.OJPECMNINNA)return false;
        var cache=new Dictionary<string,OfficerBalancePolicy.Officer>();
        var sourceRoster=OfficerRoster(source,teams,false,cache);
        var sourceProfile=new OfficerBalancePolicy.Profile(sourceRoster,OfficerAbilityDimensions);
        var baseline=new double[OfficerAbilityDimensions];int baselineCount=0;var allIds=new HashSet<string>();
        void Include(List<OfficerBalancePolicy.Officer> roster){foreach(var h in roster)if(allIds.Add(h.Id)){baselineCount++;for(int d=0;d<baseline.Length;d++)baseline[d]+=h.Ability[d];}}
        Include(sourceRoster);
        foreach(var city in peers)if(participating.Contains(city.OJPECMNINNA))Include(OfficerRoster(city,teams,true,cache));
        if(baselineCount==0)return false;for(int d=0;d<baseline.Length;d++)baseline[d]/=baselineCount;
        // Neighbouring/two-hop friendly cities only; no unbounded world scan.
        AFAGFHJONFJ target=null;double best=.015;int targetCount=0;OfficerBalancePolicy.Officer chosen=null;
        foreach(var c in peers){
            if(!Eligible(c,out var owner)||owner.IBGCLCAIBOL!=f.IBGCLCAIBOL||c.GKLIEHCHDLD()||FoodEmergency(c)
                ||!f.ANMMMOIKLEG.TryGetValue(c.LMPMNLLGOEC.IPJHOOJBCKI,out var gov)||gov.BNGFJOAJKDH==NEFBEOFPGJA.StopWar)continue;
            int arriving=0,arrivingUnits=0;bool attacked=false;
            foreach(var t in teams){if(t.m_targetCity!=c.OJPECMNINNA)continue;
                if(War(f,t.m_belongPower)){attacked=true;break;}
                if(t.m_belongPower!=f.IBGCLCAIBOL)continue;
                arriving+=t.m_listHero?.Count??0;if(t.m_dicDeployArmy!=null)foreach(var group in t.m_dicDeployArmy)arrivingUnits+=group.Value?.Count??0;
            }
            if(attacked)continue;int count=HomeOfficers(c,teams).Count+arriving,army=c.LMPMNLLGOEC.KJPPGIOCDID?.Count??0;
            int demand=Math.Max(OfficerDemand(c,f,army+arrivingUnits),fairShare);
            if(army+arrivingUnits>=c.JNLIOIHLDHH()||count>=Math.Max(demand,fairShare+1))continue;
            var targetRoster=OfficerRoster(c,teams,true,cache);
            var targetProfile=new OfficerBalancePolicy.Profile(targetRoster,OfficerAbilityDimensions);
            foreach(var officer in sourceRoster){
                if(!CanGate("officer-balance:"+officer.Id)||officer.Id==source.LMPMNLLGOEC.IPJHOOJBCKI||officer.Id==source.LMPMNLLGOEC.BKENJPNKMGI)continue;
                double gain=OfficerBalancePolicy.Gain(sourceProfile,targetProfile,officer,baseline);
                double countGain=.01*(sourceRoster.Count-targetRoster.Count-1);
                if(home-count<3&&gain-countGain<=1e-6)continue;
                if(gain>best){target=c;best=gain;targetCount=count;chosen=officer;}
            }
        }
        if(target==null)return false;
        Wo_DeployTeamCtrl team=null;rebalancing=true;selectedOfficer=chosen.Id;selectedOfficerCity=source.Pointer;
        try{
            try{team=GenerateSupport(source,1,teams,reserved);}
            finally{selectedOfficer=null;selectedOfficerCity=IntPtr.Zero;}
            if(!team){next["officer-balance:"+chosen.Id]=Environment.TickCount64+300000;return false;}
            if(team.m_listHero.Count!=1||team.m_listHero[0].OJPECMNINNA!=chosen.Id||home-team.m_listHero.Count<sourceDemand||team.IsPassEnemyCity(target.BMBMKPGEHBH)){DismissSupport(team);return false;}
            team.m_targetCity=target.OJPECMNINNA;team.m_isEnterCity=true;
            if(!team.gameObject.activeSelf)team.gameObject.SetActive(true);
            next["officer-balance:"+chosen.Id]=Environment.TickCount64+300000;
            Note($"garrison-balance source={source.OJPECMNINNA} target={target.OJPECMNINNA} officers={home}->{home-1} targetProjected={targetCount+1} officer={chosen.Id} abilityGain={best:F4} team={team.m_id}; native travel");return true;
        }catch{if(team){try{DismissSupport(team);}catch{}}throw;}finally{selectedOfficer=null;selectedOfficerCity=IntPtr.Zero;rebalancing=false;}
    }
}

