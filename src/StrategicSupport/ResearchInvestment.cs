using System;
using System.Collections.Generic;
using BepInEx.Configuration;

public sealed partial class StrategicSupport
{
    static ConfigEntry<int> researchReserve;
    [ThreadStatic] static FNFJODHIJFO researchGovernor;
    [ThreadStatic] static int researchNativeCandidates,researchAffordable,researchStaffable;
    [ThreadStatic] static string researchSelected;
    static int ResearchReserve=>researchReserve?.Value??30000;
    // UI BanGroupTech -> GLIJMELMEHI static +0x72. Native faction entry
    // checks this only when the faction leader is the player (OPIKIHPNCIP).
    static bool NativeResearchBanned(MKMOJNAJKJE f)=>f!=null&&f.IBGCLCAIBOL==NEINKOOBJIH.OPIKIHPNCIP&&GLIJMELMEHI.BEMGDNDJFMF;
    void ConfigureResearchInvestment()=>researchReserve=Config.Bind("Research","TreasuryReserve",30000,new ConfigDescription("Minimum faction treasury retained AFTER research spending. City cash is not faction treasury.",new AcceptableValueRange<int>(0,10000000)));
    sealed class ResearchScope {public MKMOJNAJKJE Owner;public FNFJODHIJFO Governor;public HashSet<IntPtr> Team;public Dictionary<IntPtr,AFAGFHJONFJ> Homes;}
    static void BeginResearch(FNFJODHIJFO __instance,out ResearchScope __state)
    {
        __state=new(){Owner=researchOwner,Governor=researchGovernor,Team=researchTeam,Homes=researchHomes};
        researchOwner=null;researchGovernor=null;researchTeam=null;researchHomes=null;
        try{
            if(!Ready()||!investment.Value||!research.Value||__instance==null)return;
            var cities=__instance.PJMLPLDPEIM();if(cities==null)return;
            foreach(var c in cities)if(Eligible(c,out var f)&&f.ANMMMOIKLEG[c.LMPMNLLGOEC.IPJHOOJBCKI]?.Pointer==__instance.Pointer){researchOwner=f;researchGovernor=__instance;break;}
        }catch(Exception e){Error(e);}
    }
    static void EndResearch(ResearchScope __state){researchOwner=__state?.Owner;researchGovernor=__state?.Governor;researchTeam=__state?.Team;researchHomes=__state?.Homes;}
    static void TryResearch(AFAGFHJONFJ city,MKMOJNAJKJE faction)
    {
        if(NativeResearchBanned(faction)){if(Gate("native-research-ban:"+faction.IBGCLCAIBOL,30))Note($"research-blocked faction={faction.IBGCLCAIBOL} reason=vanilla-governor-research-ban");return;}
        if(!research.Value||!AIGate("research-invest:"+faction.IBGCLCAIBOL+":"+city.LMPMNLLGOEC.IPJHOOJBCKI,faction.IBGCLCAIBOL,10))return;
        if(faction.DNOGPFHMOMO<=ResearchFundsReserve(faction)){InvestmentAudit(city,$"research-blocked treasury={faction.DNOGPFHMOMO} reserve={ResearchFundsReserve(faction)} researchFloor={ResearchReserve} cityCash={city.LMPMNLLGOEC.DNOGPFHMOMO} reason=treasury-reserve");return;}
        var governor=faction.ANMMMOIKLEG[city.LMPMNLLGOEC.IPJHOOJBCKI];
        int before=faction.DNOGPFHMOMO,points=faction.IIBKLLKPNEH;
        researchNativeCandidates=researchAffordable=researchStaffable=0;researchSelected=null;
        researchOwner=faction;researchGovernor=governor;
        try{governor.OHBNFHKFNPH();}catch(Exception e){Error(e);}finally{researchOwner=null;researchGovernor=null;}
        if(before!=faction.DNOGPFHMOMO)Note($"research-invest faction={faction.IBGCLCAIBOL} governor={city.LMPMNLLGOEC.IPJHOOJBCKI} selected={researchSelected} spent={before-faction.DNOGPFHMOMO} treasury={faction.DNOGPFHMOMO} reserve={ResearchReserve}");
        var running=new List<string>();var pending=new List<(int Cost,string Detail)>();int finished=0,remaining=0;
        if(faction.OINKOPJJGJH!=null)foreach(var kv in faction.OINKOPJJGJH){var t=kv.Value;if(t==null)continue;
            if((t.ABJNKABKKJM?.Count??0)>0)running.Add(kv.Key+":"+t.ABJNKABKKJM.Count);
            if(AEHAGJAKNEK.HMHMMONMBAP!=null&&AEHAGJAKNEK.HMHMMONMBAP.TryGetValue(kv.Key,out var def)&&def?.DONIEGIDACP!=null){
                if(t.LBGMKEHHPAA>=0&&t.LBGMKEHHPAA<def.DONIEGIDACP.Length&&t.FDNECLDGCHN<def.DONIEGIDACP[t.LBGMKEHHPAA]){
                    remaining++;
                    if((t.ABJNKABKKJM?.Count??0)==0){int cost=t.PKPLGNACAPG();int requiredPoints=def.DDGCODBDINJ!=null&&t.LBGMKEHHPAA<def.DDGCODBDINJ.Length?def.DDGCODBDINJ[t.LBGMKEHHPAA]:-1;
                        pending.Add((cost,$"{kv.Key}:cost={cost}/points={t.CNLBJJNJLNG}+{faction.IIBKLLKPNEH}/{requiredPoints}"));}
                }else finished++;
            }
        }
        string stage=before!=faction.DNOGPFHMOMO?"started":researchNativeCandidates==0?"native-prerequisites-or-officer-eligibility":researchAffordable==0?"reserve-or-cost":researchStaffable==0?"idle-qualified-researchers":"native-start-did-not-spend";
        InvestmentAudit(city,$"research-state treasury={faction.DNOGPFHMOMO} reserve={ResearchReserve} cityCash={city.LMPMNLLGOEC.DNOGPFHMOMO} points={points}->{faction.IIBKLLKPNEH} candidates={researchNativeCandidates} affordable={researchAffordable} staffed={researchStaffable} selected={researchSelected??"none"} running=[{string.Join(",",running)}] remaining={remaining} finished={finished} stage={stage}");
        if(before==faction.DNOGPFHMOMO&&researchNativeCandidates==0){pending.Sort((a,b)=>a.Cost.CompareTo(b.Cost));var details=new List<string>();for(int i=0;i<Math.Min(8,pending.Count);i++)details.Add(pending[i].Detail);InvestmentAudit(city,$"research-pending [{string.Join(";",details)}] native eligibility still required");}
    }
    sealed class ResearchNeeds {public bool Food,Front;public double MoneyGoal,FoodGoal,SecurityGoal,PopularGoal,MoneyBase,FoodBase;public int DefenseConstruction;public double[] Troops=new double[5];public HashSet<string> UnlockGoals=new();}
    static string BuildingUnlockTechnology(string id)=>id switch {"JZ10006"=>"34","JZ10007"=>"35","JZ20005"=>"36","JZ50015"=>"37","JZ30004"=>"38",_=>null};
    static ResearchNeeds ReadResearchNeeds(FNFJODHIJFO governor,MKMOJNAJKJE faction)
    {
        var result=new ResearchNeeds();var cities=governor?.PJMLPLDPEIM();if(cities==null)return result;
        foreach(var c in cities){if(c==null||!Eligible(c,out _))continue;
            result.Food|=FoodEmergency(c);
            if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var army in c.LMPMNLLGOEC.KJPPGIOCDID){var kind=army?.AAKKDAPJMHC;if(kind==null)continue;int role=(int)kind.ACGEFPPKLKH;if(role>=0&&role<5)result.Troops[role]+=Math.Max(0,army.FGHFOLKBHDL);}
            if(c.FKMKGNPFLFP!=null)foreach(var other in c.FKMKGNPFLFP)if(other?.LMPMNLLGOEC!=null&&War(faction,other.LMPMNLLGOEC.LMMNKIOAGLC))result.Front=true;
            var goals=ReadGoals(c);
            foreach(var goal in goals.Buildings){
                if(!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(goal.Key,out var b)||b==null||b.PLEHAHBBLGL(faction.KCKGONGIMDA))continue;
                var visited=new HashSet<string>();
                while(b!=null&&visited.Count<32&&visited.Add(b.OJPECMNINNA)){
                    string unlock=BuildingUnlockTechnology(b.OJPECMNINNA);if(unlock!=null){result.UnlockGoals.Add(unlock);break;}
                    if(string.IsNullOrEmpty(b.MDOIJIAONJM)||!AEHAGJAKNEK.ACIGFKMDOKE.TryGetValue(b.MDOIJIAONJM,out b))break;
                }
            }
            foreach(var pair in goals.Needs){double deficit=Math.Min(1,pair.Value.Missing/Math.Max(1,pair.Value.Target));
                if(pair.Key=="money"||pair.Key=="commerce"||pair.Key.StartsWith("block-commerce:"))result.MoneyGoal=Math.Max(result.MoneyGoal,deficit);
                if(pair.Key=="food"||pair.Key=="agriculture"||pair.Key.StartsWith("block-agriculture:"))result.FoodGoal=Math.Max(result.FoodGoal,deficit);
                if(pair.Key=="security")result.SecurityGoal+=deficit;
                if(pair.Key=="popular")result.PopularGoal+=deficit;
            }
            if(c.FGBLEMGGOPO!=null)foreach(var d in c.FGBLEMGGOPO)if(d?.AIMLPNBOIIF!=null)foreach(var x in d.AIMLPNBOIIF){
                if(x?.GEKKLMPLDHJ==null)continue;result.MoneyBase+=Math.Max(0,x.GEKKLMPLDHJ.CNCPIILGDEF);result.FoodBase+=Math.Max(0,x.GEKKLMPLDHJ.AJJJJNDNNME);
                if(!Completed(x)&&x.GEKKLMPLDHJ.HFBAJIGHOAF==MHCEFEODENB.Wall)result.DefenseConstruction++;
            }
        }
        return result;
    }
    // IDs and meanings verified against installed Technology.json. Unknown mod
    // technologies retain a neutral score; native eligibility is never invented.
    static double ResearchUrgency(string id,ResearchNeeds n)
    {
        if(!int.TryParse(id,out int key))return 100;
        if(n.UnlockGoals.Contains(id))return n.Food&&(key==36||key==37)?4900:3500;
        if(key==20||key==21||key==22||key==23||key==36||key==37){
            bool direct=key==21||key==23||key==37;
            return (n.Food?(direct&&n.FoodBase>0?5000:3900):n.FoodGoal>0?3000:300)+Math.Min(400,n.FoodBase/1000)+n.FoodGoal*100;
        }
        if(key==16||key==17||key==18||key==19||key==34||key==35)
            return (n.MoneyGoal>0?3000:400)+Math.Min(400,n.MoneyBase/1000)+n.MoneyGoal*100;
        if(key>=24&&key<=26)return n.DefenseConstruction>0?3100+Math.Min(200,n.DefenseConstruction*20):n.Front?2700:120;
        // Granary remodeling is siege morale recovery, NOT food production.
        // Patrol/security and population are also distinct native effects.
        if(key==27||key==28||key==29)return n.Front?2800:120;
        if(key==32)return n.SecurityGoal>0?3200+Math.Min(200,n.SecurityGoal*100):180;
        if(key==31||key==38)return n.PopularGoal>0?3100+Math.Min(200,n.PopularGoal*100):180;
        if(key==33)return n.PopularGoal>0||n.SecurityGoal>0?3000:180;
        if(key==30)return 200;
        if((key>=1&&key<=15)||(key>=40&&key<=62)){
            int role=key<=12?(key-1)/3:key==41||key==42||key==43||key==44||key==62?0:key==40||key==45||key==46||key==47||key==48||key==49||key==52?1:key==50||key==51||key==53||key==54?2:key==55||key==56||key==57||key==61?3:-1;
            double soldiers=role<0?0:n.Troops[role]+(role==2||role==3?n.Troops[4]*.5:0);
            return (n.Front?2400:150)+(soldiers>0?Math.Min(400,100+soldiers/100):0);
        }
        return 100;
    }
    static double ResearchWithPrerequisites(string id,ResearchNeeds needs,MKMOJNAJKJE faction)
    {
        double best=ResearchUrgency(id,needs);if(AEHAGJAKNEK.HMHMMONMBAP==null)return best;
        foreach(var kv in AEHAGJAKNEK.HMHMMONMBAP){
            // A finished/running descendant must not inflate a prerequisite.
            if(faction.OINKOPJJGJH==null||!faction.OINKOPJJGJH.TryGetValue(kv.Key,out var record)||record==null||kv.Value?.DONIEGIDACP==null)continue;
            int level=record.LBGMKEHHPAA;if(level<0||level>=kv.Value.DONIEGIDACP.Length||record.FDNECLDGCHN>=kv.Value.DONIEGIDACP[level]||(record.ABJNKABKKJM?.Count??0)>0)continue;
            double urgency=ResearchUrgency(kv.Key,needs);if(urgency<=best)continue;
            var frontier=new List<string>{kv.Key};var seen=new HashSet<string>();
            for(int depth=0;depth<8&&frontier.Count>0;depth++){
                var next=new List<string>();foreach(var key in frontier){
                    if(!seen.Add(key)||!AEHAGJAKNEK.HMHMMONMBAP.TryGetValue(key,out var def)||def?.ANNMPIPJGNC==null)continue;
                    foreach(var prerequisite in def.ANNMPIPJGNC){if(prerequisite==id)best=Math.Max(best,urgency*Math.Pow(.85,depth+1));else next.Add(prerequisite);}
                }frontier=next;
            }
        }
        return best;
    }
    static bool SelectResearch(MKMOJNAJKJE faction,ref Il2CppSystem.Collections.Generic.List<FBLFDEEGJMH> candidates,ref FBLFDEEGJMH selected)
    {
        if(researchOwner==null||faction.Pointer!=researchOwner.Pointer||candidates==null)return true;
        researchNativeCandidates=candidates.Count;
        var affordable=new Il2CppSystem.Collections.Generic.List<FBLFDEEGJMH>();
        foreach(var t in candidates){if(t==null)continue;int cost=t.PKPLGNACAPG();if(cost>=0&&(long)faction.DNOGPFHMOMO-cost>=ResearchFundsReserve(faction))affordable.Add(t);}
        researchAffordable=affordable.Count;candidates=affordable;
        if(affordable.Count==0){selected=null;return false;}
        // Retain filtering-only behavior when there is no automatic governor
        // context (also useful for direct budget contract tests).
        if(researchGovernor==null)return true;
        researchTeam=null;researchHomes=new();
        var idle=new List<CLAGKDECFID>();var seen=new HashSet<IntPtr>();var cities=researchGovernor.PJMLPLDPEIM();
        if(cities!=null)foreach(var c in cities){if(c==null||!Eligible(c,out _)||!c.NBKEECPNIJH(false)||c.GKLIEHCHDLD())continue;
            var local=c.EOMOIINOEMB(1,false,false,false,false,false,false);if(local==null||local.Count<=1)continue;
            foreach(var h in local)if(h!=null&&seen.Add(h.Pointer)){idle.Add(h);researchHomes[h.Pointer]=c;}
        }
        var needs=ReadResearchNeeds(researchGovernor,faction);double best=double.NegativeInfinity,bestUrgency=0;selected=null;int selectedQualified=0,selectedRequired=0;var staffBlocks=new List<string>();List<CLAGKDECFID> bestTeam=null;
        foreach(var t in affordable){
            if(AEHAGJAKNEK.HMHMMONMBAP==null||!AEHAGJAKNEK.HMHMMONMBAP.TryGetValue(t.GPEOLOAMMHP,out var def)||def==null)continue;
            int level=t.LBGMKEHHPAA;if(level<0||def.DAJIHBICKBE==null||level>=def.DAJIHBICKBE.Length||def.DONIEGIDACP==null||level>=def.DONIEGIDACP.Length)continue;
            if(t.FDNECLDGCHN>=def.DONIEGIDACP[level]||(t.ABJNKABKKJM?.Count??0)>0)continue;
            int required=Math.Max(1,def.DAJIHBICKBE[level]);var team=ResearchTeamFor(def,level,idle,required);int qualified=team.Count;
            if(qualified<required){if(staffBlocks.Count<8)staffBlocks.Add(t.GPEOLOAMMHP+":"+qualified+"/"+required);continue;}
            researchStaffable++;double urgency=ResearchWithPrerequisites(t.GPEOLOAMMHP,needs,faction);
            double remaining=Math.Max(1,def.DONIEGIDACP[level]-t.FDNECLDGCHN);
            // Cost, work and occupied officers can change decisions within the
            // same need tier; they cannot overwhelm an immediate food crisis.
            double score=urgency+150/(1+t.PKPLGNACAPG()/10000d+remaining/10000d+required);
            if(score>best){best=score;selected=t;bestUrgency=urgency;selectedQualified=qualified;selectedRequired=required;bestTeam=team;}
        }
        researchSelected=selected?.GPEOLOAMMHP;
        if(bestTeam!=null)ReserveResearchTeam(bestTeam,idle,AEHAGJAKNEK.HMHMMONMBAP[selected.GPEOLOAMMHP],selected.LBGMKEHHPAA);
        if(selected==null&&Gate("research-staff:"+faction.IBGCLCAIBOL+":"+researchGovernor.Pointer,10))Note($"research-staff-block faction={faction.IBGCLCAIBOL} idle={idle.Count} qualified/required=[{string.Join(",",staffBlocks)}]");
        if(selected!=null&&Gate("research-choice:"+faction.IBGCLCAIBOL+":"+researchGovernor.Pointer,10))Note($"research-choice faction={faction.IBGCLCAIBOL} tech={researchSelected} level={selected.LBGMKEHHPAA} cost={selected.PKPLGNACAPG()} after={faction.DNOGPFHMOMO-selected.PKPLGNACAPG()} urgency={bestUrgency:F1} qualified={selectedQualified}/{selectedRequired} foodEmergency={needs.Food} foodGoal={needs.FoodGoal:F2} moneyGoal={needs.MoneyGoal:F2} securityGoal={needs.SecurityGoal:F2} popularGoal={needs.PopularGoal:F2} front={needs.Front}");
        // The original governor routine still assigns qualified heroes, pays
        // normal costs and advances normal research time after this selection.
        return false;
    }
}
