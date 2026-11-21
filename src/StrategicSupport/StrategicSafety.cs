using System;
using System.Collections.Generic;

public sealed partial class StrategicSupport
{
    // Reserve budgets sum uncommanded troop power. Enemy city Power() includes
    // commander multipliers and must not be compared to that unscaled budget.
    // Already deployed troops are not an additional home-border army.
    static long ReserveCityPower(AFAGFHJONFJ city,List<Wo_DeployTeamCtrl> teams){
        var reserved=AttackReserved(teams);var seen=new HashSet<IntPtr>();long power=0;
        if(city?.LMPMNLLGOEC?.KJPPGIOCDID!=null)foreach(var a in city.LMPMNLLGOEC.KJPPGIOCDID)
            if(a!=null&&a.FGHFOLKBHDL>0&&!reserved.Contains(a.Pointer)&&seen.Add(a.Pointer))power+=Math.Max(0,a.GNCKPEPJDGF());
        return power;
    }
    static long BorderThreat(AFAGFHJONFJ city,MKMOJNAJKJE faction,List<Wo_DeployTeamCtrl> teams)
    {
        long danger=0;var seen=new HashSet<string>();
        if(city.FKMKGNPFLFP!=null)foreach(var neighbour in city.FKMKGNPFLFP){
            if(neighbour?.LMPMNLLGOEC==null||!seen.Add(neighbour.OJPECMNINNA)||!War(faction,neighbour.LMPMNLLGOEC.LMMNKIOAGLC))continue;
            if(!ThreatRoadLength(neighbour,city,neighbour.LMPMNLLGOEC.LMMNKIOAGLC,neighbour.BMBMKPGEHBH,out _,out string why)){if(Gate("border-screen:"+city.OJPECMNINNA+":"+neighbour.OJPECMNINNA,30))Note($"border-threat-screened city={city.OJPECMNINNA} enemyCity={neighbour.OJPECMNINNA} reason={why}");continue;}
            danger=Math.Min(int.MaxValue,danger+ReserveCityPower(neighbour,teams));
        }
        // Explicit attacks block lending even when the map's city power is stale.
        foreach(var t in teams)if(t&&War(faction,t.m_belongPower)&&(t.m_targetCity==city.OJPECMNINNA||t.m_warInfo?.IJGLCFJOOIK?.OJPECMNINNA==city.OJPECMNINNA))return long.MaxValue;
        return (long)Math.Ceiling(danger*Math.Round(coverage.Value,3));
    }
    static HashSet<string> HomeOfficers(AFAGFHJONFJ city,List<Wo_DeployTeamCtrl> teams)
    {
        var away=new HashSet<string>();foreach(var t in teams)if(t?.m_listHero!=null)foreach(var h in t.m_listHero)if(h!=null)away.Add(h.OJPECMNINNA);
        var home=new HashSet<string>();
        if(city.ABJNKABKKJM!=null)foreach(var h in city.ABJNKABKKJM){
            if(h==null||ManualProtected(h)||(int)h.HFBAJIGHOAF>1||h.GMILGFFJOIL||h.OJPECMNINNA==NEINKOOBJIH.OPIKIHPNCIP||h.LMPMNLLGOEC?.CNOMNBCPJDI!=city.OJPECMNINNA
                ||h.LMPMNLLGOEC.LMMNKIOAGLC!=city.LMPMNLLGOEC.LMMNKIOAGLC
                ||NEINKOOBJIH.AOPJBNBGPLP?.Contains(h.OJPECMNINNA)==true||away.Contains(h.OJPECMNINNA)||(h.OGPEJIEHDAN!=null&&h.OGPEJIEHDAN.HFBAJIGHOAF!=GCPCLLMKIMM.None))continue;
            home.Add(h.OJPECMNINNA);
        }
        return home;
    }
}
