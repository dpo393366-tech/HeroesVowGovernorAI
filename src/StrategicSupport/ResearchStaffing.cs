using System;
using System.Collections.Generic;
using System.Linq;

public sealed partial class StrategicSupport
{
    [ThreadStatic] static HashSet<IntPtr> researchTeam;
    [ThreadStatic] static Dictionary<IntPtr,AFAGFHJONFJ> researchHomes;
    static bool ResearchCity(AFAGFHJONFJ c)=>researchOwner!=null&&researchGovernor!=null&&c!=null&&Eligible(c,out var f)&&f.Pointer==researchOwner.Pointer
        &&f.ANMMMOIKLEG.TryGetValue(c.LMPMNLLGOEC.IPJHOOJBCKI,out var g)&&g?.Pointer==researchGovernor.Pointer&&!c.GKLIEHCHDLD()&&c.NBKEECPNIJH(false);
    static bool HostileBorder(AFAGFHJONFJ c){if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n?.LMPMNLLGOEC!=null&&War(researchOwner,n.LMPMNLLGOEC.LMMNKIOAGLC))return true;return false;}
    // Native EOM's third argument includes officers with a civil assignment.
    // The native routine still rejects prisoners, absentees and non-idle main jobs.
    static void IncludeResearchWorkers(AFAGFHJONFJ __instance,int __0,ref bool __2)
    {try{if(__0==1&&ResearchCity(__instance))__2=true;}catch(Exception e){Error(e);}}
    static bool CivilResearchCandidate(AFAGFHJONFJ c,CLAGKDECFID h)
        =>CivilResearchCandidateCounted(c,h,null);
    static bool CivilResearchCandidateCounted(AFAGFHJONFJ c,CLAGKDECFID h,Dictionary<OEBGIDGEIIG,int> counts)
    {
        if(h?.LMPMNLLGOEC==null||ManualProtected(h)||h.OJPECMNINNA==NEINKOOBJIH.OPIKIHPNCIP||h.OJPECMNINNA==c.LMPMNLLGOEC.IPJHOOJBCKI||h.OJPECMNINNA==c.LMPMNLLGOEC.BKENJPNKMGI
            ||h.LMPMNLLGOEC.LMMNKIOAGLC!=c.LMPMNLLGOEC.LMMNKIOAGLC||h.LMPMNLLGOEC.CNOMNBCPJDI!=c.OJPECMNINNA||h.OGPEJIEHDAN?.HFBAJIGHOAF!=GCPCLLMKIMM.None)return false;
        if(h.BAPIJIJLLCA==null||h.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.None)return true;
        var duty=h.BAPIJIJLLCA?.HFBAJIGHOAF;
        if(duty==OEBGIDGEIIG.Explore)return true;
        if(researchOwner==null||IncomingHostile(c,researchOwner)||c.GKLIEHCHDLD())return false;
        // A civil assignment alone is not an active primary mission. Native
        // research can borrow surplus workers; keep the actual local role staffed.
        int workers=counts==null?ResearchDutyCount(c,duty.Value):counts.TryGetValue(duty.Value,out int count)?count:0;
        if(duty==OEBGIDGEIIG.Manage)return !FoodEmergency(c)&&workers>1;
        if(duty==OEBGIDGEIIG.Security||duty==OEBGIDGEIIG.Patrol)return workers>1;
        if(duty==OEBGIDGEIIG.Military)return workers>ResearchMilitaryKeep(c);
        return false; // Wall repairs remain protected.
    }
    static int ResearchMilitaryKeep(AFAGFHJONFJ c)=>Math.Max(2,((c.LMPMNLLGOEC.KJPPGIOCDID?.Count??0)+1)/2);
    static int ResearchDutyCount(AFAGFHJONFJ c,OEBGIDGEIIG duty)=>c.ABJNKABKKJM?.ToArray().Count(h=>
        h?.LMPMNLLGOEC?.LMMNKIOAGLC==c.LMPMNLLGOEC.LMMNKIOAGLC&&h.LMPMNLLGOEC.CNOMNBCPJDI==c.OJPECMNINNA
        &&!h.GMILGFFJOIL&&h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None&&h.BAPIJIJLLCA?.HFBAJIGHOAF==duty)??0;
    static Dictionary<OEBGIDGEIIG,int> ResearchDutyCounts(AFAGFHJONFJ c){
        var counts=new Dictionary<OEBGIDGEIIG,int>();var roster=c.ABJNKABKKJM;
        if(roster!=null)foreach(var h in roster)if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==c.LMPMNLLGOEC.LMMNKIOAGLC&&h.LMPMNLLGOEC.CNOMNBCPJDI==c.OJPECMNINNA&&!h.GMILGFFJOIL&&h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None){
            var duty=h.BAPIJIJLLCA?.HFBAJIGHOAF??OEBGIDGEIIG.None;counts.TryGetValue(duty,out int n);counts[duty]=n+1;
        }return counts;
    }
    static bool ResearchDutyRoom(AFAGFHJONFJ c,CLAGKDECFID h,IEnumerable<CLAGKDECFID> selected){
        var duty=h.BAPIJIJLLCA?.HFBAJIGHOAF??OEBGIDGEIIG.None;
        if(duty==OEBGIDGEIIG.None||duty==OEBGIDGEIIG.Explore)return true;
        int keep=duty==OEBGIDGEIIG.Military?ResearchMilitaryKeep(c):1;
        int used=selected.Count(x=>researchHomes.TryGetValue(x.Pointer,out var home)&&home.Pointer==c.Pointer&&x.BAPIJIJLLCA?.HFBAJIGHOAF==duty);
        return ResearchDutyCount(c,duty)>keep+used;
    }
    static void FilterResearchWorkers(AFAGFHJONFJ c,ref Il2CppSystem.Collections.Generic.List<CLAGKDECFID> pool)
    {
        try{
        if(pool==null||!ResearchCity(c))return;
        // Scoped to this synchronous native callback; never retained across
        // assignment changes. Avoid one full roster scan for every candidate.
        var duties=ResearchDutyCounts(c);
        var filtered=new Il2CppSystem.Collections.Generic.List<CLAGKDECFID>();
        foreach(var h in pool)if(CivilResearchCandidateCounted(c,h,duties)&&(researchTeam==null||researchTeam.Contains(h.Pointer)))filtered.Add(h);
        pool=filtered;
        }catch(Exception e){pool=new();Error(e);}
    }
    static int ResearchCitySlots(AFAGFHJONFJ c)
    {
        int home=0,workingResearch=0;if(c.ABJNKABKKJM!=null)foreach(var h in c.ABJNKABKKJM){
            if(h?.LMPMNLLGOEC==null||h.LMPMNLLGOEC.LMMNKIOAGLC!=c.LMPMNLLGOEC.LMMNKIOAGLC||h.LMPMNLLGOEC.CNOMNBCPJDI!=c.OJPECMNINNA||h.GMILGFFJOIL)continue;
            var job=h.OGPEJIEHDAN?.HFBAJIGHOAF;if(job==GCPCLLMKIMM.None||job==GCPCLLMKIMM.Technology)home++;
            if(job==GCPCLLMKIMM.Technology)workingResearch++;
        }
        // Preserve an actual local reserve, not a fixed three-researcher ceiling.
        // CivilResearchCandidate still excludes rulers/governors/mayors, military
        // duties, absentees and every other active primary job.
        int reserve=Math.Max(HostileBorder(c)?6:4,(home+2)/3);
        return Math.Max(0,home-reserve-workingResearch);
    }
    static double ResearchOpportunity(CLAGKDECFID h)
    {
        if(!h.MGLFFEANDOI)return 0;
        if(h.BAPIJIJLLCA?.HFBAJIGHOAF==OEBGIDGEIIG.Explore)return 1000+Math.Max(0,h.FDFDCEGNPGC(true));
        return 10000+Math.Max(0,h.FKOIGKFHHKO(true))+Math.Max(0,h.FGCBICMIMOJ(true));
    }
    static List<CLAGKDECFID> ResearchTeamFor(GGOGLBGDEJJ def,int level,List<CLAGKDECFID> pool,int required)
    {
        var protectedIds=new HashSet<string>();if(researchHomes!=null)foreach(var city in researchHomes.Values.GroupBy(x=>x.Pointer).Select(g=>g.First()))protectedIds.UnionWith(ProtectedCivilOfficers(city));
        var chosen=new List<CLAGKDECFID>();var counts=new Dictionary<IntPtr,int>();var slotCache=new Dictionary<IntPtr,int>();
        foreach(var h in pool.OrderBy(ResearchOpportunity).ThenByDescending(h=>h.GNCHNFHKBKE())){
            if(protectedIds.Contains(h.OJPECMNINNA)||!def.OIFAMCDJHFP(h,level)||researchHomes==null||!researchHomes.TryGetValue(h.Pointer,out var c)||!ResearchDutyRoom(c,h,chosen))continue;
            if(!slotCache.TryGetValue(c.Pointer,out int slots))slotCache[c.Pointer]=slots=ResearchCitySlots(c);
            counts.TryGetValue(c.Pointer,out int used);if(used>=slots)continue;
            chosen.Add(h);counts[c.Pointer]=used+1;if(chosen.Count>=required)break;
        }
        return chosen;
    }
    static void ReserveResearchTeam(List<CLAGKDECFID> chosen,List<CLAGKDECFID> pool,GGOGLBGDEJJ def,int level)
    {
        researchTeam=new();foreach(var h in chosen)researchTeam.Add(h.Pointer);
        // Native OHBN ignores a city when its list has <=1 entries. Keep one
        // additional candidate for a single chosen researcher; the native
        // required-count limit still assigns exactly the requested number.
        foreach(var group in chosen.GroupBy(h=>researchHomes[h.Pointer].Pointer))if(group.Count()==1){
            var protectedIds=ProtectedCivilOfficers(researchHomes[group.First().Pointer]);
            var extra=pool.Where(h=>researchHomes[h.Pointer].Pointer==group.Key&&!researchTeam.Contains(h.Pointer)&&!protectedIds.Contains(h.OJPECMNINNA)
                &&(!def.OIFAMCDJHFP(h,level)||(ResearchCitySlots(researchHomes[h.Pointer])>group.Count()&&ResearchDutyRoom(researchHomes[h.Pointer],h,chosen))))
                .OrderBy(h=>def.OIFAMCDJHFP(h,level)?1:0).ThenBy(ResearchOpportunity).FirstOrDefault();if(extra!=null)researchTeam.Add(extra.Pointer);
        }
    }
}
