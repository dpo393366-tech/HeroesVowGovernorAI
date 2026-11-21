using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
public class ANOPPJKAPIG:Native{public string OJPECMNINNA;public int ACGEFPPKLKH,ELBKMJKECNF=1000,GKHMPJMANAL=50;public float MMBBMCCFANH=1,JLEEIFPNNEI=1;}
public partial class AFAGFHJONFJ{public List<ANOPPJKAPIG> Recruitable=new();public List<ANOPPJKAPIG>JIBOFLLONAH(bool caps)=>Recruitable;}
public static class StrategicSafetyTests
{
    static int count;static AFAGFHJONFJ c;static MKMOJNAJKJE f;
    static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,args);
    static void Check(bool ok,string message){count++;if(!ok)throw new Exception(message);}
    static void Setup(){new StrategicSupport().Load();MICGPAMNDKM.IJPKEMPIFIH=new();ICGAEMLACJG.HFJIECIBFKB.Clear();NEINKOOBJIH.COCMABJFOFH.Clear();NEINKOOBJIH.JMIADAFPGMC.Clear();NEINKOOBJIH.AOPJBNBGPLP.Clear();
        f=new(){IBGCLCAIBOL="F",DNOGPFHMOMO=1000000};f.ANMMMOIKLEG["gov"]=new(){LMMNKIOAGLC="F"};f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};NEINKOOBJIH.COCMABJFOFH["F"]=f;
        c=new(){OJPECMNINNA="border"};c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.LMPMNLLGOEC.DNOGPFHMOMO=100000;MICGPAMNDKM.IJPKEMPIFIH[c.OJPECMNINNA]=c;
        for(int i=0;i<10;i++)c.LMPMNLLGOEC.KJPPGIOCDID.Add(new());Call("Ready");
    }
    static Wo_DeployTeamCtrl Generate(List<Wo_DeployTeamCtrl> teams=null,HashSet<IntPtr> reserved=null)=>(Wo_DeployTeamCtrl)Call("GenerateSupport",c,4000,teams??new(),reserved??new());
    static AFAGFHJONFJ Enemy(int power){var e=new AFAGFHJONFJ{OJPECMNINNA="enemy"+c.FKMKGNPFLFP.Count,Power=power};e.LMPMNLLGOEC.LMMNKIOAGLC="enemy";e.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){CombatValue=power,FGHFOLKBHDL=Math.Max(1,power)});c.FKMKGNPFLFP.Add(e);return e;}
    static ANOPPJKAPIG Quality(ANOPPJKAPIG original){object[] args={c,original};Call("ChooseRecruitQuality",args);return (ANOPPJKAPIG)args[1];}
    public static void Run(){
        Setup();var elite=c.LMPMNLLGOEC.KJPPGIOCDID[0];elite.CombatValue=12000;var team=Generate();
        Check(team!=null&&!team.m_dicDeployArmy.Values.SelectMany(v=>v).Contains(elite),"60 percent headcount cannot export most combat strength");
        Setup();Enemy(8000);Check(Generate()==null,"unattacked but exposed second front cannot lend below threat floor");
        Setup();Enemy(3000);Enemy(3000);team=Generate();Check(team!=null&&team.GetSoldierNum(false)<=2000,"two hostile borders combine their threat");
        Setup();team=Generate();Check(team!=null,"first support can use surplus");
        var reserved=team.m_dicDeployArmy.Values.SelectMany(v=>v).Select(a=>a.Pointer).ToHashSet();
        Check(Generate(new(){team},reserved)==null,"second simultaneous request cannot export troops already counted as departed or exhaust remaining guard");
        Enemy(9000);Check(Generate(new(){team},reserved)==null,"new hostile border blocks additional lending after first departure");
        Setup();var neighbour=Enemy(90000);f.AENJACOFBJK["enemy"].EBIOPDHEBOF=BHBNKONCOJD.Ally;Check(Generate()!=null,"allied neighbour is not treated as hostile");
        Setup();c.HomeOfficerCount=2;Check(Generate()==null,"last two officers retained");
        Setup();c.HomeOfficerCount=3;team=Generate();Check(team!=null&&team.m_listHero.Count==1,"third officer can lead surplus support");
        Setup();var away=new Wo_DeployTeamCtrl{m_belongPower="F",m_assembleCity="other"};away.m_listHero.Add(c.ABJNKABKKJM[0]);team=Generate(new(){away});Check(team!=null&&team.m_listHero.All(h=>h.OJPECMNINNA!=away.m_listHero[0].OJPECMNINNA),"native candidate filter skips deployed officer and selects available alternate");
        Setup();var attack=new Wo_DeployTeamCtrl{m_belongPower="enemy",m_targetCity=c.OJPECMNINNA};Check(Generate(new(){attack})==null,"active attack blocks lending even without neighbour data");
        Setup();var basic=new ANOPPJKAPIG{OJPECMNINNA="basic-cavalry",ACGEFPPKLKH=2};var tiger=new ANOPPJKAPIG{OJPECMNINNA="11702",ACGEFPPKLKH=2,MMBBMCCFANH=3.6f,ELBKMJKECNF=5000,GKHMPJMANAL=200};var archer=new ANOPPJKAPIG{OJPECMNINNA="12702",ACGEFPPKLKH=3,MMBBMCCFANH=3.2f,ELBKMJKECNF=5000,GKHMPJMANAL=200};c.Recruitable.AddRange(new[]{basic,tiger,archer});
        Check(Quality(basic)==tiger,"unlocked affordable Tiger-Leopard cavalry preferred in cavalry role");
        var basicArcher=new ANOPPJKAPIG{OJPECMNINNA="archer",ACGEFPPKLKH=3};Check(Quality(basicArcher)==archer,"unlocked Unmatched Flying Army preferred in ranged role");
        c.Recruitable.Remove(tiger);Check(Quality(basic)==basic,"unavailable or capped elite not force-unlocked");c.Recruitable.Add(tiger);
        c.LMPMNLLGOEC.DNOGPFHMOMO=12000;Check(Quality(basic)==basic,"elite spending cannot consume local operating reserve");
        c.LMPMNLLGOEC.DNOGPFHMOMO=100000;tiger.JLEEIFPNNEI=2;Check(Quality(basic)==basic,"extra food burden not forced");tiger.JLEEIFPNNEI=1;
        c.LMPMNLLGOEC.OEGNGFBCHFL=1;Check(Quality(basic)==basic,"food emergency retains native recruitment");
        c.LMPMNLLGOEC.OEGNGFBCHFL=10000000;c.Player=true;c.OPNKGGKHAGJ=false;c.LMPMNLLGOEC.BKENJPNKMGI="player";Check(Quality(basic)==basic,"player-managed recruitment unchanged");
        Setup();c.HomeOfficerCount=10;var dest=new AFAGFHJONFJ{OJPECMNINNA="thin",HomeOfficerCount=0};dest.LMPMNLLGOEC.LMMNKIOAGLC="F";dest.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.FKMKGNPFLFP.Add(dest);
        Check((bool)Call("BalanceGarrison",c)&&c.Last.m_targetCity=="thin"&&c.Last.GetSoldierNum(false)==1000&&c.Last.m_listHero.Count==1,"one real unit and commander sent to understaffed city");
        Check(!(bool)Call("BalanceGarrison",c),"balance check cooldown prevents immediate repeat");
        Setup();c.HomeOfficerCount=10;dest=new(){OJPECMNINNA="full",HomeOfficerCount=8};dest.LMPMNLLGOEC.LMMNKIOAGLC="F";dest.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.FKMKGNPFLFP.Add(dest);Check(!(bool)Call("BalanceGarrison",c),"already staffed city cannot attract more officers");
        Setup();c.HomeOfficerCount=10;dest=new(){OJPECMNINNA="thin",HomeOfficerCount=0};dest.LMPMNLLGOEC.LMMNKIOAGLC="F";dest.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.FKMKGNPFLFP.Add(dest);var incoming=new Wo_DeployTeamCtrl{m_targetCity="thin",m_belongPower="F"};for(int k=0;k<10;k++)incoming.m_listHero.Add(new(){OJPECMNINNA="inbound"+k});ICGAEMLACJG.HFJIECIBFKB[1]=incoming;Check(!(bool)Call("BalanceGarrison",c),"incoming commanders reserve destination demand");
        Setup();c.HomeOfficerCount=10;Enemy(20000);dest=new(){OJPECMNINNA="thin",HomeOfficerCount=0};dest.LMPMNLLGOEC.LMMNKIOAGLC="F";dest.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.FKMKGNPFLFP.Add(dest);Check(!(bool)Call("BalanceGarrison",c),"officer balancing cannot empty exposed front");
        Setup();c.HomeOfficerCount=10;dest=new(){OJPECMNINNA="thin",HomeOfficerCount=0};dest.LMPMNLLGOEC.LMMNKIOAGLC="F";dest.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.FKMKGNPFLFP.Add(dest);c.PathBlocked=true;Check(!(bool)Call("BalanceGarrison",c)&&c.Last.Dismissed,"blocked travel safely cancelled");
        var rng=new Random(813);for(int n=0;n<500;n++){Setup();int danger=rng.Next(0,12000);Enemy(danger);var sent=Generate();if(sent!=null){long remaining=c.LMPMNLLGOEC.KJPPGIOCDID.Sum(a=>a.GNCKPEPJDGF())-sent.m_dicDeployArmy.Values.SelectMany(v=>v).Sum(a=>a.GNCKPEPJDGF());Check(remaining>=Math.Ceiling(danger*1.2)&&remaining>=6000,"random border reserve invariant");}else Check(true,"unsafe support refused");}
        Setup();c.HomeOfficerCount=30;while(c.LMPMNLLGOEC.KJPPGIOCDID.Count<30)c.LMPMNLLGOEC.KJPPGIOCDID.Add(new());
        var cities=new List<AFAGFHJONFJ>{c};for(int k=0;k<3;k++){dest=new(){OJPECMNINNA="district"+k,HomeOfficerCount=0};dest.LMPMNLLGOEC.LMMNKIOAGLC="F";dest.LMPMNLLGOEC.IPJHOOJBCKI="gov";cities.Add(dest);}
        foreach(var city in cities)city.FKMKGNPFLFP=cities.Where(x=>x!=city).ToList();
        int moves=0;for(int tick=0;tick<60;tick++){
            ((IDictionary)typeof(StrategicSupport).GetField("next",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Clear();
            foreach(var source in cities.OrderByDescending(x=>x.HomeOfficerCount).ToArray()){
                if(!(bool)Call("BalanceGarrison",source))continue;moves++;var dispatched=source.Last;var destination=cities.Single(x=>x.OJPECMNINNA==dispatched.m_targetCity);
                source.HomeOfficerCount--;destination.HomeOfficerCount++;
                foreach(var a in dispatched.m_dicDeployArmy.Values.SelectMany(v=>v)){source.LMPMNLLGOEC.KJPPGIOCDID.Remove(a);destination.LMPMNLLGOEC.KJPPGIOCDID.Add(a);}
                ICGAEMLACJG.HFJIECIBFKB.Clear();
            }
            Check(cities.Sum(x=>x.HomeOfficerCount)==30&&cities.Sum(x=>x.LMPMNLLGOEC.KJPPGIOCDID.Count)==30,"long-run redistribution conserves officers and troops");
        }
        Check(cities.Max(x=>x.HomeOfficerCount)-cities.Min(x=>x.HomeOfficerCount)<=3&&moves<=25,"concentration converges without endless shuttling");
        Console.WriteLine("Simulated completed-travel distribution [30,0,0,0] -> ["+string.Join(",",cities.Select(x=>x.HomeOfficerCount))+$"], moves={moves}, 60 rounds; fixture only.");
        Console.WriteLine($"PASS {count} strategic safety/elite recruitment checks (stubs; no live campaign claim)");
    }
}

