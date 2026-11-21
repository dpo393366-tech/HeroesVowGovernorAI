using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;

public static class MaintenanceTests
{
    static int tests;static AFAGFHJONFJ c;static MKMOJNAJKJE f;static JNHOOCJJPJO d;
    static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,args);
    static void Check(bool value,string name){tests++;if(!value)throw new Exception(name);}
    static void Toggle(string name,bool value)=>((ConfigEntry<bool>)typeof(StrategicSupport).GetField(name,BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Value=value;
    static void Tick(){((IDictionary)typeof(StrategicSupport).GetField("next",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Clear();}
    static bool RunCity(){Tick();return (bool)Call("MaintainCity",c,f);}
    static void Setup(){
        new StrategicSupport().Load();MICGPAMNDKM.IJPKEMPIFIH=new();NEINKOOBJIH.COCMABJFOFH.Clear();NEINKOOBJIH.JKGJNLHLJAJ=null;AEHAGJAKNEK.ACIGFKMDOKE.Clear();
        f=new(){IBGCLCAIBOL="F",DNOGPFHMOMO=1000000};f.ANMMMOIKLEG["gov"]=new(){LMMNKIOAGLC="F"};NEINKOOBJIH.COCMABJFOFH["F"]=f;
        c=new(){OJPECMNINNA="maintenance-city"};c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="gov";c.LMPMNLLGOEC.DNOGPFHMOMO=100000;
        MICGPAMNDKM.IJPKEMPIFIH[c.OJPECMNINNA]=c;d=new(){OJPECMNINNA="source",AJJCCPCJPFC=c};c.FGBLEMGGOPO.Add(d);Call("Ready");
    }
    static PABIPDOFBFG Def(string id,int income=100){var b=new PABIPDOFBFG{OJPECMNINNA=id,CNCPIILGDEF=income};AEHAGJAKNEK.ACIGFKMDOKE[id]=b;return b;}
    static OGABIJECHFG Instance(JNHOOCJJPJO block,PABIPDOFBFG b){var x=new OGABIJECHFG{GMIKIMACDBC=block,GEKKLMPLDHJ=b,OJPECMNINNA=block.AIMLPNBOIIF.Count+1};block.AIMLPNBOIIF.Add(x);return x;}
    static OGABIJECHFG Layout(){
        var b=Def("market");var old=Instance(d,b);var farm=Def("farm",0);farm.HFBAJIGHOAF=MHCEFEODENB.Agriculture;farm.MLAJLJKFGBD=8;Instance(d,farm);
        var other=new JNHOOCJJPJO{OJPECMNINNA="target",AJJCCPCJPFC=c,HFBAJIGHOAF=LDPAFPHFBCP.Commerce};c.FGBLEMGGOPO.Add(other);
        var bonus=Def("exchange",0);bonus.FIPONBEGNJO=1;Instance(other,bonus);other.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=other});return old;
    }
    public static void Run(){
        Setup();var x=Instance(d,Def("market"));x.OGFAHJHBCNG=400;long before=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
        Check(RunCity()&&x.OGFAHJHBCNG==650,"repair bounded to quarter health");Check(before-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==38,"proportional repair paid once");
        Check(!(bool)Call("MaintainCity",c,f),"maintenance interval blocks duplicate same tick");
        RunCity();RunCity();Check(x.OGFAHJHBCNG==1000,"repair reaches cap without overshoot");
        Setup();x=Instance(d,Def("market"));x.OGFAHJHBCNG=10;c.Siege=true;Check(!RunCity()&&x.OGFAHJHBCNG==10,"siege blocks repairs");
        c.Siege=false;NEINKOOBJIH.JKGJNLHLJAJ=c;Check(RunCity()&&x.OGFAHJHBCNG>10,"loaded delegated city can repair");NEINKOOBJIH.JKGJNLHLJAJ=null;c.Player=true;c.OPNKGGKHAGJ=false;c.LMPMNLLGOEC.BKENJPNKMGI="player";Check(!RunCity(),"player city protected");
        Setup();x=Instance(d,Def("market"));x.MGLFFEANDOI=true;x.OGFAHJHBCNG=10;Check(!RunCity(),"private building protected");
        Setup();x=Instance(d,Def("market"));x.OGFAHJHBCNG=10;f.DNOGPFHMOMO=10000;c.LMPMNLLGOEC.DNOGPFHMOMO=10000;Check(!RunCity()&&x.OGFAHJHBCNG==10,"cannot repair using reserve");
        Setup();x=Instance(d,Def("base"));var up=Def("advanced",200);up.MDOIJIAONJM="base";up.NPCHPDNFAME=5000;
        NEINKOOBJIH.JKGJNLHLJAJ=c;before=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;Check(RunCity()&&x.GEKKLMPLDHJ==up&&x.EHABIIDOKGC(),"loaded delegated city upgrade retains native build time");Check(before-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==5000,"upgrade cost once");Check(!RunCity(),"no repeat upgrade during construction");
        Setup();x=Instance(d,Def("base"));up=Def("advanced",200);up.MDOIJIAONJM="base";up.TechAllowed=false;Check(!RunCity(),"locked upgrade rejected");up.TechAllowed=true;up.JADLIKBKOHG="item";up.GJPOJINPCHP=1;Check(!RunCity(),"item cost not bypassed");
        up.JADLIKBKOHG=null;up.AJJJJNDNNME=-10;Check(!RunCity(),"upgrade that loses food rejected");up.AJJJJNDNNME=0;x.GEKKLMPLDHJ.MDOIJIAONJM="advanced";Check(!RunCity(),"cyclic upgrade data rejected");
        Setup();x=Instance(d,Def("base"));up=Def("advanced",200);up.MDOIJIAONJM="base";x.ThrowBeforeUpgrade=true;before=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
        try{RunCity();}catch(TargetInvocationException){}Check(before==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO,"pre-change failure refunds");
        x.ThrowBeforeUpgrade=false;x.ThrowAfterUpgrade=true;try{RunCity();}catch(TargetInvocationException){}Check(before-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==600,"post-change failure stays paid");
        Setup();x=Layout();before=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;Check(RunCity()&&d.AIMLPNBOIIF.Contains(x)&&c.FGBLEMGGOPO[1].AIMLPNBOIIF.Count==2,"relocation builds before removing original");
        Check(!RunCity()&&d.AIMLPNBOIIF.Contains(x)&&c.FGBLEMGGOPO[1].AIMLPNBOIIF.Count==2,"unfinished replacement neither removes original nor creates duplicates");
        var replacement=c.FGBLEMGGOPO[1].AIMLPNBOIIF[1];replacement.EMHNNCLBADE=replacement.BNMEOOJPOML;
        Check(RunCity()&&!d.AIMLPNBOIIF.Contains(x),"completed replacement allows native removal");Check(before-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==600,"relocation charges full build with no demolition money creation");Check(!RunCity(),"stable layout does not oscillate");
        Setup();x=Layout();RunCity();replacement=c.FGBLEMGGOPO[1].AIMLPNBOIIF[1];replacement.EMHNNCLBADE=100;Toggle("demolishBuildings",false);Check(!RunCity()&&d.AIMLPNBOIIF.Contains(x),"demolition option blocks pending retirement");
        Setup();x=Layout();RunCity();replacement=c.FGBLEMGGOPO[1].AIMLPNBOIIF[1];replacement.EMHNNCLBADE=100;c.LMPMNLLGOEC.IPJHOOJBCKI="newGov";f.ANMMMOIKLEG["newGov"]=new(){LMMNKIOAGLC="F"};Check(!RunCity()&&d.AIMLPNBOIIF.Contains(x),"ownership changes abandon job safely");
        Setup();x=Layout();RunCity();replacement=c.FGBLEMGGOPO[1].AIMLPNBOIIF[1];replacement.EMHNNCLBADE=100;MICGPAMNDKM.IJPKEMPIFIH=new();Call("Ready");Check(!RunCity()&&d.AIMLPNBOIIF.Contains(x),"reload never infers an old demolition order");
        Setup();x=Layout();c.HFLAGGEFBOO.Add(new(){Complete=true,CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE="market",KLCDGPDHENB=1}}});Check(!RunCity(),"completed building milestone protected");
        Setup();x=Layout();var dependent=Def("needs-market");dependent.EIMGMPDMAAJ="market,other";Check(!RunCity(),"prerequisite list protected");
        Setup();x=Layout();c.FGBLEMGGOPO[1].NoRegister=true;before=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;Check(!RunCity()&&before==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO&&d.AIMLPNBOIIF.Contains(x),"failed relocation preserves original and money");
        Check(!MaintenancePolicy.WorthMoving(100,105,1)&&!MaintenancePolicy.WorthMoving(100,200,20000),"marginal and long-payback moves rejected");
        var random=new Random(517);for(int i=0;i<10000;i++){int maximum=random.Next(1,int.MaxValue),hp=random.Next(maximum),price=random.Next(int.MaxValue);int amount=MaintenancePolicy.RepairAmount(hp,maximum),cost=MaintenancePolicy.RepairCost(price,amount,maximum);Check(amount>0&&(long)hp+amount<=maximum&&cost>0&&cost<=Math.Max(1,price),"repair arithmetic invariant");}
        Console.WriteLine($"PASS {tests} building-maintenance checks (native-call stubs and randomized arithmetic; not live gameplay)");
    }
}


