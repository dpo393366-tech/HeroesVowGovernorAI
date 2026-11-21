using System;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;
public partial class Wo_TransportTeamCtrl {
 public static int Serial=50000;public static bool FailMove,FailDebit,FailRegister;
 public bool m_isPrivate;public string m_belongPower,m_fromCity,m_toCity;public int m_money,m_food,m_id;
 public Il2CppSystem.Collections.Generic.List<DHKINGPDKJJ> m_listArmy=new();
 public Il2CppSystem.Collections.Generic.List<Vector3> m_listMovePos=new(),GAANFLKOMKL=new();public Vector3 IAFGLIMLMGA,Position;
 public GameObject gameObject=new();public bool Recycled;
 public static implicit operator bool(Wo_TransportTeamCtrl t)=>t!=null&&!t.Recycled;
 public static Wo_TransportTeamCtrl GetOne(bool p,bool active)=>new(){m_isPrivate=p,gameObject=new(){activeSelf=active}};
 public void JBPGMCBOCFO(Vector3 p){Position=p;}public Vector3 GetPosition()=>Position;
 public void MoveToCity(Ci_CityCtrl c,bool enter,bool sound){if(FailMove){m_toCity="rejected";return;}m_listMovePos.Add(c.City.BMBMKPGEHBH);}
 public void RemoveFromCity(){var row=MICGPAMNDKM.IJPKEMPIFIH[m_fromCity].LMPMNLLGOEC;foreach(var a in m_listArmy){row.KJPPGIOCDID.Remove(a);a.GDHPLIFJCBN=PMNFKDJNLDJ.Transport;}row.OEGNGFBCHFL-=m_food;if(FailDebit)throw new Exception("injected partial debit");}
 public void Recycle(){Recycled=true;gameObject.SetActive(false);}
}
public static partial class ICGAEMLACJG {
 public static Dictionary<int,Wo_TransportTeamCtrl> KBPAJECPNOH=new();
 public static void OKLHGICPMHB(Wo_TransportTeamCtrl t){t.m_id=++Wo_TransportTeamCtrl.Serial;KBPAJECPNOH[t.m_id]=t;if(Wo_TransportTeamCtrl.FailRegister)throw new Exception("injected registration fault");}
}
public static class Logistics93Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool value,string name){if(!value)throw new Exception("R93 "+name);checks++;}
 static void Setup(){typeof(Relief86Tests).GetMethod("Setup",F).Invoke(null,null);ICGAEMLACJG.KBPAJECPNOH.Clear();Wo_TransportTeamCtrl.FailMove=Wo_TransportTeamCtrl.FailDebit=Wo_TransportTeamCtrl.FailRegister=false;}
 static AFAGFHJONFJ City(string id,float x)=> (AFAGFHJONFJ)typeof(Relief86Tests).GetMethod("City",F).Invoke(null,new object[]{id,x});
 static void Link(AFAGFHJONFJ a,AFAGFHJONFJ b)=>typeof(Relief86Tests).GetMethod("Link",F).Invoke(null,new object[]{a,b});
 public static void Run(){
  for(int failure=0;failure<4;failure++){
   Setup();var s=City("ship-source",0);var d=City("ship-target",10);Link(s,d);d.LMPMNLLGOEC.KJPPGIOCDID.Clear();var f=NEINKOOBJIH.COCMABJFOFH["F"];
   var cargo=new DHKINGPDKJJ{FGHFOLKBHDL=1200};s.LMPMNLLGOEC.KJPPGIOCDID.Add(cargo);int before=s.LMPMNLLGOEC.OEGNGFBCHFL;
   Wo_TransportTeamCtrl.FailMove=failure==1;Wo_TransportTeamCtrl.FailDebit=failure==2;Wo_TransportTeamCtrl.FailRegister=failure==3;
   var type=typeof(StrategicSupport).GetNestedType("RoutePlan",F);var plan=Activator.CreateInstance(type,true);type.GetField("Success").SetValue(plan,true);type.GetField("Points").SetValue(plan,new List<Vector3>{s.BMBMKPGEHBH,d.BMBMKPGEHBH});
   bool sent=(bool)Call("DispatchCityConvoy",s,d,f,new List<DHKINGPDKJJ>{cargo},200,plan,1,"test");
   Check(sent==(failure==0),"transaction outcome "+failure);
   if(sent){
    Check(s.Last==null,"convoy does not assemble an army");Check(!s.LMPMNLLGOEC.KJPPGIOCDID.Contains(cargo)&&s.LMPMNLLGOEC.OEGNGFBCHFL==before-200,"native source debit");
    Check(!d.LMPMNLLGOEC.KJPPGIOCDID.Contains(cargo),"no early destination credit");
    Check((int)Call("CapacityRoom",d,new List<Wo_DeployTeamCtrl>(),null)==d.Capacity-1,"native convoy reserves capacity");
    Check((long)Call("ConvoyFood",d)==200,"cargo counted once");
   }else Check(ICGAEMLACJG.KBPAJECPNOH.Count==0&&s.LMPMNLLGOEC.KJPPGIOCDID.Contains(cargo)&&s.LMPMNLLGOEC.OEGNGFBCHFL==before&&cargo.GDHPLIFJCBN==PMNFKDJNLDJ.None,"fault rollback preserves resources "+failure);
  }
  Setup();Check(!(bool)Call("LogisticsUnit",new DHKINGPDKJJ{OLMAJLLIJDM="busy-officer"},new HashSet<IntPtr>()),"native worker assignment protected");
  Console.WriteLine("PASS R93 "+checks+" convoy transaction, capacity reservation and failure recovery checks; native calls mocked.");
 }
}

