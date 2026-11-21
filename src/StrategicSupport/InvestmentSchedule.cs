using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BepInEx;

public sealed partial class StrategicSupport
{
    static readonly Dictionary<IntPtr,int> economyCursors=new();
    static void Note(string message)
    {
        AsyncJournal.Write(message);
    }
    static void InvestmentAudit(AFAGFHJONFJ city,string reason)
    {
        if(city==null || !Gate("investment-audit:"+city.OJPECMNINNA+":"+reason.Split(' ')[0],30))return;
        var row=city.LMPMNLLGOEC;
        Note($"decision city={city.OJPECMNINNA} faction={row?.LMMNKIOAGLC} player={NEINKOOBJIH.OPIKIHPNCIP} governor={row?.IPJHOOJBCKI} mayor={row?.BKENJPNKMGI} personalAutomation={city.OPNKGGKHAGJ} nativeManaged={city.NBKEECPNIJH(true)} playerAffiliated={city.AKIJOBLFLCJ(NEINKOOBJIH.OPIKIHPNCIP)} {reason}");
    }
    static void AfterGovernorEconomy(FNFJODHIJFO __instance)
    {
        var governor=__instance;
        if(investing || governor==null)return;
        try{
            if(!Ready() || !investment.Value || !AIGate("governor-economy:"+governor.Pointer,governor.LMMNKIOAGLC,5))return;
            var cities=governor.PJMLPLDPEIM();
            if(cities==null || cities.Count==0)return;
            economyCursors.TryGetValue(governor.Pointer,out int cursor);
            var watch=Stopwatch.StartNew();int visited=0;
            // Rotate the native governor-owned city list; no per-frame world scan
            // and no requirement for successful recruitment or a war target.
            while(visited<Math.Min(2,cities.Count)){
                int index=cursor%cities.Count;cursor=(index+1)%cities.Count;visited++;
                var city=cities[index];
                if(city!=null && Eligible(city,out var faction) &&
                   faction.ANMMMOIKLEG.TryGetValue(city.LMPMNLLGOEC.IPJHOOJBCKI,out var actual) && actual?.Pointer==governor.Pointer)
                    AfterRecruitInvestment(city,false);
                else InvestmentAudit(city,"excluded:"+Eligibility(city,out _));
                if(watch.ElapsedMilliseconds>=20)break;
            }
            economyCursors[governor.Pointer]=cursor;
        }catch(Exception e){Error(e);}
        finally{try{TryStatecraft(governor);}catch(Exception e){Error(e);}}
    }
}


