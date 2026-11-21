using System;using System.Linq;using System.Reflection;using System.Collections.Generic;
public static class Command98Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,a);
 static void Check(bool b,string n){checks++;if(!b)throw new Exception("Command98 "+n);}
 // DLL-derived capacity model, NOT the native binary executing: one home
 // officer, per-hero MJICLKFOAPI cap, whole native unit records. Equal power
 // and very large requestedPower isolate the commander bottleneck.
 static Wo_DeployTeamCtrl NativeCapacityModel(AFAGFHJONFJ c,int leave,int requested,Il2CppSystem.Collections.Generic.List<CLAGKDECFID> pool){
  var t=new Wo_DeployTeamCtrl{m_belongPower=c.LMPMNLLGOEC.LMMNKIOAGLC,m_assembleCity=c.OJPECMNINNA};t.m_id=(int)t.Pointer;
  var officers=pool.Take(Math.Max(0,pool.Count-1)).ToList();
  foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID.Take(c.LMPMNLLGOEC.KJPPGIOCDID.Count-leave)){
   var h=officers.FirstOrDefault(h=>!t.m_dicDeployArmy.ContainsKey(h.OJPECMNINNA)||t.m_dicDeployArmy[h.OJPECMNINNA].Count<h.MJICLKFOAPI(true));if(h==null)continue;
   if(!t.m_dicDeployArmy.ContainsKey(h.OJPECMNINNA)){t.m_dicDeployArmy[h.OJPECMNINNA]=new();t.m_listHero.Add(h);}t.m_dicDeployArmy[h.OJPECMNINNA].Add(a);
  }
  if(t.m_listHero.Count==0)return null;ICGAEMLACJG.HFJIECIBFKB[t.m_id]=t;return t;
 }
 static (AFAGFHJONFJ,NJHDEHLHENN) Setup(int heroes,int cap,int units=10){
  typeof(BattleIntegrationTests).GetMethod("Setup",F).Invoke(null,null);Call("ResetBattleState");Call("ResetDefense");Call("ResetWork");
  ICGAEMLACJG.KBPAJECPNOH.Clear();
  var c=(AFAGFHJONFJ)typeof(BattleIntegrationTests).GetField("source",F).GetValue(null);var w=(NJHDEHLHENN)typeof(BattleIntegrationTests).GetField("war",F).GetValue(null);
  c.Roster=Enumerable.Range(0,heroes).Select(i=>new CLAGKDECFID{OJPECMNINNA="h"+i,CommandSlots=cap,LMPMNLLGOEC=new HeroRow{CNOMNBCPJDI=c.OJPECMNINNA,LMMNKIOAGLC="F"}}).ToList();
  c.LMPMNLLGOEC.KJPPGIOCDID.Clear();for(int i=0;i<units;i++)c.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){FGHFOLKBHDL=10000,CombatValue=1000});
  c.AssemblyModel=NativeCapacityModel;return(c,w);
 }
 static Wo_DeployTeamCtrl Send(AFAGFHJONFJ c,NJHDEHLHENN w,bool urgent){var teams=ICGAEMLACJG.HFJIECIBFKB.Values.ToList();var used=(HashSet<IntPtr>)Call("AttackReserved",teams);return urgent?(Wo_DeployTeamCtrl)Call("GenerateEmergencyRelief",c,w,int.MaxValue,30d,teams,used):(Wo_DeployTeamCtrl)Call("GenerateSupport",c,int.MaxValue,teams,used);}
 public static void Run(){
  foreach(int cap in new[]{1,3,5}){
   var(c,w)=Setup(2,cap);var sent=Send(c,w,true);int men=sent?.GetSoldierNum(false)??0;
   Check(men==cap*10000,"two officers preserve one, use remaining command slots "+cap);
   Check(c.LMPMNLLGOEC.KJPPGIOCDID.Count==10,"authoritative city roster restored");
   Check(Send(c,w,true)==null,"last officer not double booked");
   Console.WriteLine($"SIM98 heroes=2 unitCapPerHero={cap} home=100000 sent={men} left={100000-men} reserveFloor=35000 capModified=false");
  }
  {var(c,w)=Setup(1,5);Check(Send(c,w,true)==null,"no commander available even at high cap");Check(ICGAEMLACJG.KBPAJECPNOH.Count==0,"support failure has no automatic convoy fallback");}
  {var(c,w)=Setup(4,1);var sent=Send(c,w,false);Check(sent!=null&&sent.m_listHero.Count==2&&sent.GetSoldierNum(false)==20000,"ordinary 4-hero city sends two instead of assembling three and cancelling all");}
  {var(c,w)=Setup(4,3);var sent=Send(c,w,false);Check(sent?.GetSoldierNum(false)==40000,"larger command capacity still obeys regular60-percent reserve");}
  // Exercise real logistics guard, not a made-up transport-to-battle API.
  {var(c,w)=Setup(0,1);var f=NEINKOOBJIH.COCMABJFOFH["F"];c.Siege=true;Check(!(bool)Call("SafeLogisticsEndpoint",c,f),"active battle is not a transport endpoint");}
  var random=new Random(98117);
  for(int trial=0;trial<120;trial++){
   int heroes=random.Next(1,12),cap=random.Next(1,6),units=random.Next(3,31);var(c,w)=Setup(heroes,cap,units);var sent=Send(c,w,true);
   if(sent==null)continue;
   var army=sent.m_dicDeployArmy.Values.SelectMany(x=>x).ToArray();int keep=Math.Max(1,(heroes+3)/4);
   Check(sent.m_listHero.Count<=heroes-keep,"home officer floor across randomized cases");
   Check(sent.m_dicDeployArmy.Values.All(x=>x.Count<=cap),"no illegal native command capacity");
   Check(army.Distinct().Count()==army.Length,"one troop belongs to one commander");
   Check(sent.GetSoldierNum(false)<=units*10000*.65+1,"physical soldier reserve across randomized cases");
   Check(army.All(c.LMPMNLLGOEC.KJPPGIOCDID.Contains),"no invented soldiers");
  }
  {var(c,w)=Setup(3,2);string old=NEINKOOBJIH.OPIKIHPNCIP;NEINKOOBJIH.OPIKIHPNCIP="F";GLIJMELMEHI.BEMGDNDJFMF=true;
   Check((bool)Call("NativeResearchBanned",NEINKOOBJIH.COCMABJFOFH["F"]),"player governor ban honored");
   Check(!(bool)Call("NativeResearchBanned",new MKMOJNAJKJE{IBGCLCAIBOL="NPC"}),"player research ban not imposed on NPC factions");
   GLIJMELMEHI.BEMGDNDJFMF=false;Check(!(bool)Call("NativeResearchBanned",NEINKOOBJIH.COCMABJFOFH["F"]),"runtime option changes read immediately");NEINKOOBJIH.OPIKIHPNCIP=old;
   GLIJMELMEHI.FKNPIBLCEDK=0;double conservative=(double)Call("NativeWarFrequencyMargin");GLIJMELMEHI.FKNPIBLCEDK=1;Check(conservative>(double)Call("NativeWarFrequencyMargin"),"native frequency reaches custom offense threshold");
  }
  Call("ResetBattleState");Call("ResetWork");Call("ResetDefense");Console.WriteLine($"PASS Command98 {checks} policy, officer scarcity, native-capacity-model and reservation checks; native calls mocked.");
 }
}
