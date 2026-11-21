using System;using System.Collections;using System.Collections.Generic;using System.Reflection;
public static class Options99Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static T Field<T>(Type t,string n)=>(T)t.GetField(n,F).GetValue(null);
 static void Setup(Type t)=>t.GetMethod("Setup",F).Invoke(null,null);
 static void Check(bool ok,string message){checks++;if(!ok)throw new Exception("Options99: "+message);}
 static void Plan(){var c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");var f=Field<MKMOJNAJKJE>(typeof(OffensiveTests),"f");foreach(var x in (IEnumerable<string>)Call("OffensiveSteps",c,f,"gov")){} }
 public static void Run(){
  int oldMode=GLIJMELMEHI.FKNPIBLCEDK;bool oldUpgrade=GLIJMELMEHI.MIHOJGEPPID;string oldPlayer=NEINKOOBJIH.OPIKIHPNCIP;
  try {
   Setup(typeof(MaintenanceTests));var c=Field<AFAGFHJONFJ>(typeof(MaintenanceTests),"c");var f=Field<MKMOJNAJKJE>(typeof(MaintenanceTests),"f");var d=Field<JNHOOCJJPJO>(typeof(MaintenanceTests),"d");
   var a=new PABIPDOFBFG{OJPECMNINNA="base",CNCPIILGDEF=100};var b=new PABIPDOFBFG{OJPECMNINNA="advanced",MDOIJIAONJM="base",CNCPIILGDEF=200,ADGOBJJMEAO=30,ODKKMLDAPJL=0};
   AEHAGJAKNEK.ACIGFKMDOKE[a.OJPECMNINNA]=a;AEHAGJAKNEK.ACIGFKMDOKE[b.OJPECMNINNA]=b;
   var x=new OGABIJECHFG{OJPECMNINNA=1,GMIKIMACDBC=d,GEKKLMPLDHJ=a,EMHNNCLBADE=100,OGFAHJHBCNG=a.OGFAHJHBCNG};d.AIMLPNBOIIF.Add(x);f.PEGLDBENJMF=1000;f.IIBKLLKPNEH=1000;
   NEINKOOBJIH.OPIKIHPNCIP="F";GLIJMELMEHI.MIHOJGEPPID=false;
   Check((string)Call("UpgradeBlock",c,f,x,b)=="native-upgrade-points-disabled","normal upgrade reports explicit native option");
   Check(!(bool)Call("RebuildAllowed",c,f,b),"demolition/rebuild cannot bypass upgrade consent");
   int cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;int points=f.PEGLDBENJMF;bool invoked=false;
   Check(!(bool)Call("SpendBuilding",c,f,b,(Action)(()=>invoked=true),(Func<bool>)(()=>invoked)),"commit stops a previously planned upgrade");
   Check(!invoked&&points==f.PEGLDBENJMF&&cash==f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO,"blocked commit neither mutates nor spends");
   b.ADGOBJJMEAO=0;b.ODKKMLDAPJL=30;Check((bool)Call("NativeUpgradePointsBanned",f,b),"native also protects technology points despite shorter UI label");
   b.ODKKMLDAPJL=0;Check(!(bool)Call("NativeUpgradePointsBanned",f,b),"cash-only upgrade still allowed");
   b.ADGOBJJMEAO=30;b.MDOIJIAONJM="";Check(!(bool)Call("NativeUpgradePointsBanned",f,b),"new construction is not disabled");b.MDOIJIAONJM="base";
   NEINKOOBJIH.OPIKIHPNCIP="other";Check(!(bool)Call("NativeUpgradePointsBanned",f,b),"player consent does not block NPC development");NEINKOOBJIH.OPIKIHPNCIP="F";
   GLIJMELMEHI.MIHOJGEPPID=true;
   Check((string)Call("UpgradeBlock",c,f,x,b)==null,"enable option immediately makes real upgrade eligible");
   Check((bool)Call("SpendBuilding",c,f,b,(Action)(()=>x.GMMDMPFFIGP(b.OJPECMNINNA,false)),(Func<bool>)(()=>x.GEKKLMPLDHJ==b)),"enabled option commits actual upgrade path");
   Check(f.PEGLDBENJMF==points-30&&x.GEKKLMPLDHJ==b,"one successful mutation pays once");
   NEINKOOBJIH.OPIKIHPNCIP=oldPlayer;
   GLIJMELMEHI.FKNPIBLCEDK=2;Setup(typeof(OffensiveTests));Plan();c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");Check(c.Calls==0,"no-war stops initial custom city attack");
   GLIJMELMEHI.FKNPIBLCEDK=1;Setup(typeof(OffensiveTests));Plan();c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");Check(c.Last!=null&&!c.Last.Dismissed,"frequent mode plans attack");
   GLIJMELMEHI.FKNPIBLCEDK=2;Check(!(bool)Call("GuardAttackStart",c.Last)&&c.Last.Dismissed,"option changed during assembly cancels tracked attack");
   GLIJMELMEHI.FKNPIBLCEDK=1;Setup(typeof(OffensiveTests));Plan();c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");((IDictionary)typeof(StrategicSupport).GetField("attackOrders",F).GetValue(null)).Clear();GLIJMELMEHI.FKNPIBLCEDK=2;
   Check(!(bool)Call("GuardAttackStart",c.Last)&&c.Last.Dismissed,"restored native assembly respects no-war");
   GLIJMELMEHI.FKNPIBLCEDK=1;Setup(typeof(OffensiveTests));Plan();c=Field<AFAGFHJONFJ>(typeof(OffensiveTests),"c");((IDictionary)typeof(StrategicSupport).GetField("attackOrders",F).GetValue(null)).Clear();c.Last.m_responseToPlayer=true;GLIJMELMEHI.FKNPIBLCEDK=2;
   Check((bool)Call("GuardAttackStart",c.Last)&&!c.Last.Dismissed,"explicit player dispatch is not cancelled");
   GLIJMELMEHI.FKNPIBLCEDK=0;double normal=(double)Call("NativeWarFrequencyMargin");GLIJMELMEHI.FKNPIBLCEDK=1;Check(normal-(double)Call("NativeWarFrequencyMargin")>.399,"native normal/frequent power-margin difference retained");
   GLIJMELMEHI.FKNPIBLCEDK=2;
   Emergency88Tests.Run();Camp89Tests.Run();
   typeof(Hunt94Tests).GetMethod("Setup",F).Invoke(null,new object[]{50});typeof(Hunt94Tests).GetMethod("RunHunt",F).Invoke(null,null);
   var hunter=Field<AFAGFHJONFJ>(typeof(Hunt94Tests),"source").Last;Check(hunter!=null&&!hunter.Dismissed,"nearby bandit defense still dispatches");
   Check(true,"existing emergency reinforcement, camp relief and bandit cases pass with no-war enabled");
  }finally{GLIJMELMEHI.FKNPIBLCEDK=oldMode;GLIJMELMEHI.MIHOJGEPPID=oldUpgrade;NEINKOOBJIH.OPIKIHPNCIP=oldPlayer;}
  Console.WriteLine($"PASS Options99 {checks} native option scopes, runtime changes, payment and dispatch boundaries; native calls mocked.");
 }
}
