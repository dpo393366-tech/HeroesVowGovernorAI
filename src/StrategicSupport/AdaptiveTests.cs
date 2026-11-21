using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using UnityEngine;

namespace Vow.Waves {
 public static class ReserveWaves {
    static bool active;static IntPtr battle;static double remaining;
    static ConfigEntry<float> count=new(2),cap=new(20),interval=new(67.5f);
    public static void Set(NJHDEHLHENN w,double seconds){active=w!=null;battle=w?.Pointer??IntPtr.Zero;remaining=seconds;}
 }
}
public static class AdaptiveTests
{
    const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;
    static int checks;
    static object Invoke(Type t,string n,params object[] a)=>t.GetMethod(n,F).Invoke(null,a);
    static object Core(string n,params object[] a)=>Invoke(typeof(StrategicSupport),n,a);
    static T Field<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
    static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
    static AFAGFHJONFJ Setup(){new StrategicSupport().Load();Invoke(typeof(BattleIntegrationTests),"Setup");Vow.Waves.ReserveWaves.Set(null,90);return Field<AFAGFHJONFJ>(typeof(BattleIntegrationTests),"source");}
    static NJHDEHLHENN War=>Field<NJHDEHLHENN>(typeof(BattleIntegrationTests),"war");
    static void Assess(double t)=>Core("Assess",War,t,true);
    static void Twice(){Assess(0);Assess(10);}
    static AFAGFHJONFJ City(string id)=>(AFAGFHJONFJ)Invoke(typeof(BattleIntegrationTests),"City",id,"F");
    static Wo_DeployTeamCtrl Generate(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams=null,HashSet<IntPtr> reserved=null)
        =>(Wo_DeployTeamCtrl)Core("GenerateSupport",c,500,teams??new(),reserved??new());
    public static void Run(){
        var c=Setup();var roster=c.LMPMNLLGOEC.KJPPGIOCDID;var busy=roster[0];busy.GDHPLIFJCBN=PMNFKDJNLDJ.Deploy;Twice();
        Check(c.Calls==1&&roster.Contains(busy)&&busy.GDHPLIFJCBN==PMNFKDJNLDJ.Deploy,"partial busy city dispatches without stealing busy unit");
        Check(ReferenceEquals(roster,c.LMPMNLLGOEC.KJPPGIOCDID),"city roster identity restored before activation");
        c=Setup();c.LMPMNLLGOEC.KJPPGIOCDID[0].FGHFOLKBHDL=99;Twice();Check(c.Calls==1,"99 soldier survivor no longer vetoes whole city");
        c=Setup();c.LMPMNLLGOEC.KJPPGIOCDID[0].FGHFOLKBHDL=10000;var t=Generate(c);Check(t!=null&&t.GetSoldierNum(false)==7000,"small eligible units dispatched without exposing strong defender");
        c=Setup();var w=War;for(int i=0;i<2;i++){t=new(){m_belongPower="F",m_targetWarInfo=w,Position=new(-2000)};t.m_id=(int)t.Pointer;t.m_dicDeployArmy["officer"]=new(){new()};ICGAEMLACJG.HFJIECIBFKB[t.m_id]=t;}
        Twice();Check(c.Calls==1,"late commitments do not block timely rescue");
        c=Setup();c.BMBMKPGEHBH=new(87);for(int i=0;i<12;i++){var x=City("blocked"+i);x.BMBMKPGEHBH=new(99-i);x.LMPMNLLGOEC.OEGNGFBCHFL=0;}Twice();Check(c.Calls==1,"thirteenth viable source found");
        c=Setup();Wa_Controller.m_enemyCurSoldierNum=10000;Twice();Check(c.Calls==0&&Field<AFAGFHJONFJ>(typeof(BattleIntegrationTests),"enemySource").Calls==0,"equal sides avoid reinforcement arms race");
        c=Setup();Assess(0);Wa_Controller.m_enemyCurSoldierNum++;Assess(10);Check(c.Calls==1,"small arrival does not erase decision window");
        c=Setup();War.KJPPGIOCDID[0].Add(new(){FGHFOLKBHDL=20000});Vow.Waves.ReserveWaves.Set(War,90);Twice();Check(c.Calls==1,"late reserve not counted as immediate support");
        c=Setup();War.KJPPGIOCDID[0].Add(new(){FGHFOLKBHDL=20000});Vow.Waves.ReserveWaves.Set(War,5);Twice();Check(c.Calls==0,"timely reserve prevents redundant external help");
        for(int i=0;i<20;i++)War.GPFFMNKOFCH[0].Add(new());Check((int)Core("ReserveForecast",War,0,100d)==0,"field cap prevents promising reserve slots");
        Vow.Waves.ReserveWaves.Set(null,90);
        var loss=new AttritionWindow();loss.Sample(0,15000,20000);loss.Sample(10,10000,20000);Check(loss.FriendlyLoss==0,"one burst requires corroboration");loss.Sample(20,10000,20000);loss.Sample(30,10000,20000);Check(loss.FriendlyLoss==0,"isolated burst rejected by interval median");
        loss=new();for(int i=0;i<6;i++)loss.Sample(i*10,10000-i*1000,20000);Check(loss.FriendlyLoss==100,"sustained collapse detected");Check(BattlePolicy.Need(1000,2000,100,0,9,0,1.15)==0,"genuinely impossible rescue remains rejected");
        c=Setup();var f=NEINKOOBJIH.COCMABJFOFH["F"];f.DNOGPFHMOMO=200000;var grants=new List<int>();
        for(int i=0;i<5;i++){var x=City("fund"+i);Core("Fund",x,true);grants.Add(x.LMPMNLLGOEC.DNOGPFHMOMO);}
        Check(grants.All(x=>x==20000)&&f.DNOGPFHMOMO==100000,"same-window comparable cities get equal recruitment shares");
        Invoke(typeof(CityIntegrationTests),"Setup");c=Field<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");f=Field<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");var d=Field<JNHOOCJJPJO>(typeof(CityIntegrationTests),"d");
        var barracks=(PABIPDOFBFG)Invoke(typeof(CityIntegrationTests),"Add","barracks",MHCEFEODENB.Military,600);barracks.IKELHGAHBID=1;
        var farm=(PABIPDOFBFG)Invoke(typeof(CityIntegrationTests),"Add","farm",MHCEFEODENB.Agriculture,600);farm.CNCPIILGDEF=0;farm.AJJJJNDNNME=400;c.LMPMNLLGOEC.OEGNGFBCHFL=0;
        Core("BuildBest",c,f);Check(d.AIMLPNBOIIF[0].GEKKLMPLDHJ==farm,"food emergency outranks barracks");
        Invoke(typeof(CityIntegrationTests),"Setup");c=Field<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");f=Field<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");f.DNOGPFHMOMO=101000;c.LMPMNLLGOEC.DNOGPFHMOMO=10000;
        Invoke(typeof(CityIntegrationTests),"Add","market",MHCEFEODENB.Commerce,1000);Core("AfterRecruitInvestment",c,false);
        Check(f.ANMMMOIKLEG["gov"].ResearchCalls==1&&f.DNOGPFHMOMO<101000&&f.DNOGPFHMOMO>=10000,"research tried first and surplus actually used while emergency reserve remains");
        c=Setup();roster=c.LMPMNLLGOEC.KJPPGIOCDID;c.ThrowGeneration=true;try{Generate(c);throw new Exception("missing native fault");}catch(TargetInvocationException){}
        Check(ReferenceEquals(roster,c.LMPMNLLGOEC.KJPPGIOCDID)&&roster.Count==10,"native failure restores full roster");
        c=Setup();c.LMPMNLLGOEC.KJPPGIOCDID.Add(c.LMPMNLLGOEC.KJPPGIOCDID[0]);Check(Generate(c)!=null&&c.LMPMNLLGOEC.KJPPGIOCDID.Count==10,"same-reference duplicate repaired without losing distinct troops");
        c=Setup();c.Generated=()=>UnityEngine.AI.NavMesh.Blocked=true;Twice();Check(c.Last.Dismissed&&(bool)Core("CanGate","source:source"),"physical route failure after generation rolls back without a cooldown");UnityEngine.AI.NavMesh.Blocked=false;
        c=Setup();var originalUnits=c.LMPMNLLGOEC.KJPPGIOCDID.ToArray();var reserved=new HashSet<IntPtr>{originalUnits[0].Pointer};
        var away=new Wo_DeployTeamCtrl{m_belongPower="F",m_assembleCity=c.OJPECMNINNA};away.m_dicDeployArmy["officer"]=new(){originalUnits[0]};
        t=Generate(c,new(){away},reserved);Check(t!=null&&!t.m_dicDeployArmy.Values.SelectMany(x=>x).Contains(originalUnits[0])&&t.GetSoldierNum(false)<=3000,"existing deployment is not counted twice as home garrison");
        c=Setup();var sourceRoster=c.LMPMNLLGOEC.KJPPGIOCDID;sourceRoster.Clear();sourceRoster.Add(new(){FGHFOLKBHDL=1000});sourceRoster.Add(new(){FGHFOLKBHDL=1000});Check(Generate(c)==null,"last two units cannot be sent");
        c=Setup();c.Invalid=true;NEINKOOBJIH.AOPJBNBGPLP.Add("retainer");Check(Generate(c)==null&&c.Last.Dismissed,"reserved companion rejected after native selection");
        c=Setup();c.WrongGeneratedFaction=true;Check(Generate(c)==null&&c.Last.Dismissed,"foreign generated army cannot be activated as support");
        c=Setup();c.OwnershipChangesDuringGeneration=true;Check(Generate(c)==null&&c.Last.Dismissed,"source ownership change during native generation cancels support");
        RandomSelections();
        Console.WriteLine($"PASS {checks} adaptive checks including 5000 randomized native-selection fixtures; not gameplay.");
    }
    static void RandomSelections(){
        var rng=new Random(151126);
        for(int iteration=0;iteration<5000;iteration++){
            var c=Setup();var roster=c.LMPMNLLGOEC.KJPPGIOCDID;roster.Clear();int count=rng.Next(1,31);
            for(int i=0;i<count;i++)roster.Add(new(){FGHFOLKBHDL=rng.Next(1,10001),GDHPLIFJCBN=rng.Next(4)==0?PMNFKDJNLDJ.Deploy:PMNFKDJNLDJ.None});
            c.LMPMNLLGOEC.OEGNGFBCHFL=int.MaxValue;var before=roster.ToArray();long total=before.Sum(a=>(long)a.FGHFOLKBHDL);
            double fraction=rng.Next(5,10)/10d;Field<ConfigEntry<float>>(typeof(StrategicSupport),"retain").Value=(float)fraction;
            var t=Generate(c);Check(ReferenceEquals(roster,c.LMPMNLLGOEC.KJPPGIOCDID)&&before.SequenceEqual(roster),"random candidate scope restores roster exactly");
            if(t==null)continue;
            var sent=t.m_dicDeployArmy.Values.SelectMany(x=>x).ToArray();
            Check(sent.Select(a=>a.Pointer).Distinct().Count()==sent.Length&&sent.All(a=>before.Contains(a)&&a.GDHPLIFJCBN!=PMNFKDJNLDJ.Deploy&&a.FGHFOLKBHDL>800),"random selection legal and unique");
            Check(before.Length-sent.Length>=2&&total-sent.Sum(a=>(long)a.FGHFOLKBHDL)>=Math.Ceiling(total*fraction),"random selection preserves minimum garrison and fraction");
        }
    }
}
