using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;
public static class Relief84Tests{
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int n;
 static object Call(string m,params object[] a)=>typeof(StrategicSupport).GetMethod(m,F).Invoke(null,a);
 static void Check(bool ok,string why){n++;if(!ok)throw new Exception("R84: "+why);}
 static void Setup()=>typeof(StrategicSafetyTests).GetMethod("Setup",F).Invoke(null,null);
 static AFAGFHJONFJ Source()=>(AFAGFHJONFJ)typeof(StrategicSafetyTests).GetField("c",F).GetValue(null);
 public static void Run(){
  Setup();var s=Source();var f=NEINKOOBJIH.COCMABJFOFH["F"];
  var wan=new AFAGFHJONFJ{OJPECMNINNA="CS1045",BMBMKPGEHBH=new(40)};wan.LMPMNLLGOEC.LMMNKIOAGLC="F";
  s.BMBMKPGEHBH=new(0);s.MEKIDAONKMD[wan.OJPECMNINNA]=new(){EJMDIPFNGMG=new()};
  AOHIBKLOAEI.RegionAt=p=>p.x<1?s:wan;
  object[] args={s.BMBMKPGEHBH,wan.BMBMKPGEHBH,f,2f,0d,null};
  Check((bool)Call("RouteSeconds",args)&&(double)args[4]==20,"live Xinye empty intermediate route accepted");
  var via=new AFAGFHJONFJ{OJPECMNINNA="CS1002",BMBMKPGEHBH=new(20)};via.LMPMNLLGOEC.LMMNKIOAGLC="F";
  s.MEKIDAONKMD[wan.OJPECMNINNA].EJMDIPFNGMG.Add(via);
  Check((bool)Call("RouteSeconds",args)&&(double)args[4]==20,"live Runan single intermediate route accepted");
  via.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!(bool)Call("RouteSeconds",args),"captured Xinye still blocks relief corridor");
  via.LMPMNLLGOEC.LMMNKIOAGLC="F";s.MEKIDAONKMD[wan.OJPECMNINNA].EJMDIPFNGMG.Add(via);
  Check(!(bool)Call("RouteSeconds",args),"cyclic corrupt road rejected");AOHIBKLOAEI.RegionAt=null;
  Setup();s=Source();f=NEINKOOBJIH.COCMABJFOFH["F"];
  var enemy=new AFAGFHJONFJ{OJPECMNINNA="front",Power=100000};enemy.LMPMNLLGOEC.LMMNKIOAGLC="enemy";
  var enemyUnit=new DHKINGPDKJJ{CombatValue=2000};enemy.LMPMNLLGOEC.KJPPGIOCDID.Add(enemyUnit);s.FKMKGNPFLFP.Add(enemy);
  Check((long)Call("BorderThreat",s,f,new List<Wo_DeployTeamCtrl>())==2400,"same raw power scale despite 100000 commander-adjusted city value");
  var team=(Wo_DeployTeamCtrl)Call("GenerateSupport",s,2000,new List<Wo_DeployTeamCtrl>(),new HashSet<IntPtr>());
  Check(team!=null,"mixed power scale no longer blocks otherwise safe lending");
  enemyUnit.CombatValue=20000;Check(Call("GenerateSupport",s,2000,new List<Wo_DeployTeamCtrl>(),new HashSet<IntPtr>())==null,"genuinely strong raw border still blocks");
  var deployed=new Wo_DeployTeamCtrl{m_belongPower="enemy",m_targetCity="elsewhere"};deployed.m_dicDeployArmy["enemy"]=new(){enemyUnit};
  Check((long)Call("ReserveCityPower",enemy,new List<Wo_DeployTeamCtrl>{deployed})==0,"already deployed enemy troops not counted twice as home threat");
  deployed.m_targetCity=s.OJPECMNINNA;Check((long)Call("BorderThreat",s,f,new List<Wo_DeployTeamCtrl>{deployed})==long.MaxValue,"direct invasion still overrides home budget");
  Setup();s=Source();f=NEINKOOBJIH.COCMABJFOFH["F"];s.Capacity=11;
  s.LMPMNLLGOEC.KJPPGIOCDID.Clear();for(int i=0;i<12;i++)s.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){FGHFOLKBHDL=9282,CombatValue=1975});
  s.LMPMNLLGOEC.OEGNGFBCHFL=5948278;
  var recipient=new AFAGFHJONFJ{OJPECMNINNA="recipient",Capacity=11};recipient.LMPMNLLGOEC.LMMNKIOAGLC="F";recipient.LMPMNLLGOEC.IPJHOOJBCKI="gov";s.FKMKGNPFLFP.Add(recipient);recipient.FKMKGNPFLFP.Add(s);MICGPAMNDKM.IJPKEMPIFIH[recipient.OJPECMNINNA]=recipient;
  enemy=new(){OJPECMNINNA="front",Power=100000};enemy.LMPMNLLGOEC.LMMNKIOAGLC="enemy";enemy.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){CombatValue=2000});s.FKMKGNPFLFP.Add(enemy);
  ICGAEMLACJG.KBPAJECPNOH.Clear();ICGAEMLACJG.FKKCPDEFODO[recipient.OJPECMNINNA]=new(){City=recipient};
  foreach(var phase in (IEnumerable<string>)Call("CapacityTransferSteps",s,f)){}
  var convoy=ICGAEMLACJG.KBPAJECPNOH.Values.Single();
  Check(convoy.m_toCity==recipient.OJPECMNINNA&&s.Last==null,"Runan 12/11 overflow uses native convoy");
  Check(convoy.m_listArmy.Count==1&&s.LMPMNLLGOEC.KJPPGIOCDID.Count==11,"only excess leaves with immediate source capacity relief");
  Check(s.LMPMNLLGOEC.OEGNGFBCHFL+convoy.m_food==5948278,"cargo is transferred not consumed as expedition upkeep");
  ICGAEMLACJG.KBPAJECPNOH.Clear();
  Console.WriteLine($"PASS R84 {n} native-route shape, consistent reserve scale, overflow-food regressions; mock movement only.");
 }
}
