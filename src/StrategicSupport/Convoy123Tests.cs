using System;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;
public static class Convoy123Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int checks;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool value,string why){checks++;if(!value)throw new Exception("Convoy123: "+why);}
 static AFAGFHJONFJ City(string name,float x)=>(AFAGFHJONFJ)typeof(Relief86Tests).GetMethod("City",F).Invoke(null,new object[]{name,x});
 static bool Credited(object snapshot,object cargo){object[] args={snapshot,cargo,false};Call("CargoDelta",args);return (bool)args[2];}
 static object Field(object value,string name)=>value.GetType().GetField(name).GetValue(value);
 public static void Run(){
  typeof(Logistics93Tests).GetMethod("Setup",F).Invoke(null,null);
  var source=City("CS1027",0);var dest=City("CS1015",10);var unit=new DHKINGPDKJJ{FGHFOLKBHDL=12000};
  var convoy=new Wo_TransportTeamCtrl{m_id=123001,m_belongPower="F",m_fromCity="CS1027",m_toCity="CS1015",m_food=12345,m_money=67};convoy.m_listArmy.Add(unit);
  var s=Call("CaptureCargo",convoy,"arrival");long sourceFood=source.LMPMNLLGOEC.OEGNGFBCHFL,destFood=dest.LMPMNLLGOEC.OEGNGFBCHFL;
  Check(!Credited(Field(s,"Destination"),s),"vanished icon alone never proves delivery");
  dest.LMPMNLLGOEC.OEGNGFBCHFL+=convoy.m_food;dest.LMPMNLLGOEC.DNOGPFHMOMO+=convoy.m_money;
  Check(!Credited(Field(s,"Destination"),s),"food credit without troop admission is not a complete shipment");
  dest.LMPMNLLGOEC.KJPPGIOCDID.Add(unit);Check(Credited(Field(s,"Destination"),s),"exact cargo and original troop pointer in destination verifies arrival");
  Check(!Credited(Field(s,"Source"),s),"normal arrival is not source refund");
  Call("FinishCargoAudit",s,null);Check(source.LMPMNLLGOEC.OEGNGFBCHFL==sourceFood&&dest.LMPMNLLGOEC.OEGNGFBCHFL==destFood+12345&&convoy.m_food==12345&&!convoy.Recycled,"auditing never changes city stocks, cargo or native lifecycle");
  s=Call("CaptureCargo",convoy,"dismiss");source.LMPMNLLGOEC.OEGNGFBCHFL+=convoy.m_food;source.LMPMNLLGOEC.DNOGPFHMOMO+=convoy.m_money;source.LMPMNLLGOEC.KJPPGIOCDID.Add(unit);
  Check(Credited(Field(s,"Source"),s)&&!Credited(Field(s,"Destination"),s),"native dismissal refund is distinguished from delivery");
  Call("FinishCargoAudit",s,null);Check(convoy.m_toCity=="CS1015"&&!convoy.Recycled,"cancel audit does not cancel or reroute manual order");
  s=Call("CaptureCargo",convoy,"arrival");Check(!Credited(Field(s,"Destination"),s),"already present troop cannot be counted as newly delivered");
  MICGPAMNDKM.IJPKEMPIFIH.Remove("CS1015");Check(!Credited(Field(s,"Destination"),s),"missing destination cannot be reported delivered");
  convoy.m_toCity="CS1013";dest=City("CS1013",20);convoy.m_listArmy.Clear();convoy.m_money=0;s=Call("CaptureCargo",convoy,"arrival");dest.LMPMNLLGOEC.OEGNGFBCHFL+=convoy.m_food;Check(Credited(Field(s,"Destination"),s),"food-only convoy also verifies exact deposit");
  Console.WriteLine($"PASS Convoy123 {checks} arrival, source-refund, incomplete-credit and read-only audit checks; native delivery mocked.");
 }
}
