using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;
public static class Attack120Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;static AFAGFHJONFJ c,target;static MKMOJNAJKJE f;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Attack120: "+why);}
 static void Setup(){typeof(OffensiveTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetOffense");c=(AFAGFHJONFJ)typeof(OffensiveTests).GetField("c",F).GetValue(null);target=(AFAGFHJONFJ)typeof(OffensiveTests).GetField("target",F).GetValue(null);f=NEINKOOBJIH.COCMABJFOFH["F"];}
 static bool Ready(int required)=>(bool)Call("CombatProbeReady",c,"city:"+target.Pointer,required,ICGAEMLACJG.HFJIECIBFKB.Values.ToList());
 static void Fail()=>Call("FailedAttackProbe",c,target,122999,52080,"insufficient-attack-power");
 public static void Run(){
  Setup();Fail();for(int day=1;day<=73;day++){NEINKOOBJIH.MIPFHCKDJOK=100+day;foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID){a.FGHFOLKBHDL++;a.GDHPLIFJCBN=day%2==0?PMNFKDJNLDJ.Train:PMNFKDJNLDJ.None;}Check(!Ready(122999+day%7),"73-day power failure cannot reopen by time, tiny headcount gain, training state, or enemy estimate noise day="+day);}
  Check(c.Calls==0,"read-only 73-day assessments never create/dismiss troops");
  Setup();Fail();Check(Ready(52080),"defender weakens to actual measured assembly power: reopen immediately");
  Setup();Fail();foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.CombatValue=(int)(a.GNCKPEPJDGF()*1.01);Check(!Ready(122999),"tiny training power gain cannot trigger same hopeless assembly");
  foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.CombatValue*=3;Check(Ready(122999),"sufficient estimated usable power gain allows actual native validation");
  Setup();Fail();c.Roster.Add(new CLAGKDECFID{OJPECMNINNA="commander-new",CommandSlots=5,LMPMNLLGOEC=new HeroRow{CNOMNBCPJDI=c.OJPECMNINNA,LMMNKIOAGLC="F"}});Check(Ready(122999),"additional usable commander capacity reopens");
  Setup();Fail();c.LMPMNLLGOEC.KJPPGIOCDID.Add(c.LMPMNLLGOEC.KJPPGIOCDID[0]);Call("NormalizeTroopReferences",c);Check(!Ready(122999),"duplicate cleanup cannot reopen");
  Setup();Fail();target.LMPMNLLGOEC.LMMNKIOAGLC="other";Check(Ready(122999),"target ownership change invalidates prior failure");
  Setup();Fail();c.LMPMNLLGOEC.LMMNKIOAGLC="other";Check(Ready(122999),"source ownership change does not carry predecessor policy");
  Setup();Fail();NEINKOOBJIH.MIPFHCKDJOK=99;Check(Ready(122999),"save rollback invalidates future record");
  Setup();Fail();Check((bool)Call("CombatProbeReady",c,"city:other",122999,ICGAEMLACJG.HFJIECIBFKB.Values.ToList()),"other target unaffected");
  Setup();Fail();Call("ResetOffense");Check(Ready(122999),"save state reset clears power failures");
  Setup();Fail();var records=(IDictionary)typeof(StrategicSupport).GetField("failedCombatProbes",F).GetValue(null);var record=records[c.OJPECMNINNA+"/city:"+target.Pointer];var capacity=record.GetType().GetField("Capacity").GetValue(record);capacity.GetType().GetField("Raw").SetValue(capacity,0L);Check(Ready(122999),"previous zero usable budget becoming positive cannot be permanently stuck");
  Setup();Call("FailedAttackProbe",c,target,122999,0,"insufficient-attack-power");foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)a.CombatValue=a.GNCKPEPJDGF()*2;Check(Ready(122999),"zero actual result permits a new probe after material source improvement");
  Setup();Call("FailedAttackProbe",c,target,122999,52080,"low-morale");NEINKOOBJIH.MIPFHCKDJOK=130;Check(Ready(122999),"non-power failure retains bounded periodic retry");
  Setup();c.Generated=()=>c.Last.Combat=1;Check(!(bool)Call("TryAttack",c,target,f,"gov")&&c.Last.Dismissed,"actual production attack rejects insufficient assembled power once");
  for(int day=1;day<=73;day++){NEINKOOBJIH.MIPFHCKDJOK=100+day;Check(!(bool)Call("TryAttack",c,target,f,"gov"),"production attack stays deferred day="+day);}
  Check(c.Calls==1,"73 days through production dispatcher cause one failed assembly, not periodic notification storms");
  Call("ResetOffense");Call("ResetWork");Console.WriteLine("PASS Attack120 "+checks+" assertions including two 73-day loops; native generation mocked, no reporter save gameplay verification.");
 }
}
