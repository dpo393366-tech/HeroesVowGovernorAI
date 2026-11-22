using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using UnityEngine;
public static class Evacuation124Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;static AFAGFHJONFJ source,target,donor;static MKMOJNAJKJE faction;static DHKINGPDKJJ large;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static AFAGFHJONFJ City(string n,float x)=>(AFAGFHJONFJ)typeof(Relief86Tests).GetMethod("City",F).Invoke(null,new object[]{n,x});
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Evacuation124: "+why);}
 static void Setup(){
  typeof(Logistics93Tests).GetMethod("Setup",F).Invoke(null,null);ICGAEMLACJG.HFJIECIBFKB.Clear();MICGPAMNDKM.IJPKEMPIFIH.Clear();Call("ResetSupply");
  source=City("ZhongShan",0);target=City("HeNei",10);donor=City("Ye",9);faction=NEINKOOBJIH.COCMABJFOFH["F"];
  source.Capacity=9;large=source.LMPMNLLGOEC.KJPPGIOCDID[9];large.FGHFOLKBHDL=10000;source.LMPMNLLGOEC.KJPPGIOCDID[0].FGHFOLKBHDL=1;source.LMPMNLLGOEC.OEGNGFBCHFL=20000;
  target.LMPMNLLGOEC.KJPPGIOCDID.RemoveRange(2,8);target.Capacity=3;target.LMPMNLLGOEC.OEGNGFBCHFL=0;donor.LMPMNLLGOEC.OEGNGFBCHFL=200000;
 }
 static List<Wo_TransportTeamCtrl> Convoys()=>ICGAEMLACJG.KBPAJECPNOH.Values.ToList();
 static void Ship(){int count=0;foreach(var phase in (IEnumerable<string>)Call("CityShipmentSteps",source,target,faction,1,long.MaxValue,"capacity-overflow"))if(++count>5000)throw new Exception("unbounded evacuation");}
 public static void Run(){
  Setup();target.LMPMNLLGOEC.OEGNGFBCHFL=10000000;Ship();
  Check(Convoys().Count==1&&Convoys()[0].m_listArmy.Contains(large),"limited slot rescues 10000 soldiers before a one-soldier unit");
  Check(source.LMPMNLLGOEC.KJPPGIOCDID.Count==9&&source.LMPMNLLGOEC.KJPPGIOCDID.Any(a=>a.FGHFOLKBHDL==1),"only excess units leave and no soldier is deleted");
  Setup();int food=donor.LMPMNLLGOEC.OEGNGFBCHFL;Ship();var cargo=Convoys();
  Check(cargo.Count==2&&cargo.Any(t=>t.m_fromCity=="Ye"&&t.m_listArmy.Count==0&&t.m_food>0)&&cargo.Any(t=>t.m_fromCity=="ZhongShan"&&t.m_listArmy.Contains(large)),"poor overfull city evacuates after another city commits timely real food cargo");
  Check(donor.LMPMNLLGOEC.OEGNGFBCHFL>=50000&&source.LMPMNLLGOEC.OEGNGFBCHFL==20000,"donor keeps five-day rear reserve and evacuee stock is not overdrafted");
  Check(target.LMPMNLLGOEC.OEGNGFBCHFL==0&&!target.LMPMNLLGOEC.KJPPGIOCDID.Contains(large)&&donor.LMPMNLLGOEC.OEGNGFBCHFL+cargo.Sum(t=>t.m_food)==food,"neither food nor troops are credited before physical delivery");
  Ship();Check(Convoys().Count==2,"repeated decision cannot double fund or overfill target");
  Setup();donor.BMBMKPGEHBH=new(10000);Ship();Check(Convoys().Count==0&&source.LMPMNLLGOEC.KJPPGIOCDID.Contains(large),"food donor arriving later than evacuees cannot authorize departure");
  Setup();donor.LMPMNLLGOEC.OEGNGFBCHFL=50000;Ship();Check(Convoys().Count==0,"no funds below emergency reserve do not invent food");
  Setup();target.Capacity=2;Ship();Check(Convoys().Count==0,"full target creates neither troop nor food shipment");
  Setup();donor.LMPMNLLGOEC.OEGNGFBCHFL=80000;var second=City("SecondFoodCity",8);second.LMPMNLLGOEC.OEGNGFBCHFL=100000;Ship();Check(Convoys().Count==3&&Convoys().Count(t=>t.m_listArmy.Count==0)==2,"two food donors can jointly fund one evacuation");
  Check(donor.LMPMNLLGOEC.OEGNGFBCHFL>=50000&&second.LMPMNLLGOEC.OEGNGFBCHFL>=50000,"each food donor independently retains its reserve");
  Setup();int beforeFood=donor.LMPMNLLGOEC.OEGNGFBCHFL;var e=((IEnumerable<string>)Call("CityShipmentSteps",source,target,faction,1,long.MaxValue,"capacity-overflow")).GetEnumerator();
  while(e.MoveNext()){if(e.Current=="evacuation-food-donor"){target.Capacity=2;break;}}while(e.MoveNext()){}e.Dispose();Check(Convoys().Count==0&&donor.LMPMNLLGOEC.OEGNGFBCHFL==beforeFood,"capacity filled during yielded funding scan cancels before any debit");
  Setup();((BepInEx.Configuration.ConfigEntry<bool>)typeof(StrategicSupport).GetField("autoSupply",F).GetValue(null)).Value=false;Ship();Check(Convoys().Count==0,"disabled auto supply does not dispatch extra food");
  Setup();var slow=new Wo_TransportTeamCtrl{m_id=1241,m_fromCity="Ye",m_toCity="HeNei",m_belongPower="F",m_food=999999,Position=new(10000)};slow.m_listMovePos.Add(target.BMBMKPGEHBH);ICGAEMLACJG.KBPAJECPNOH[1241]=slow;
  Check((long)Call("TimelyCargoFood",target,1)==0,"late existing convoy is excluded from evacuation coverage");
  slow.Position=target.BMBMKPGEHBH;Check((long)Call("TimelyCargoFood",target,1)==999999,"arriving convoy can cover admission without phantom credit");
  Console.WriteLine($"PASS Evacuation124 {checks} largest-unit, timed multi-city food funding, conservation and policy checks; native convoy movement mocked.");
 }
}
