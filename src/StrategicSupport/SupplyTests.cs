using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace UnityEngine.AI {
 public enum NavMeshPathStatus{PathComplete,PathPartial}
 public struct NavMeshHit{public Vector3 position;}
 public class NavMeshPath{public Vector3[] corners;public NavMeshPathStatus status;}
 public static class NavMesh{
  public const int AllAreas=-1;public static bool Blocked;public static float Detour;
  public static bool SamplePosition(Vector3 p,out NavMeshHit hit,float distance,int area){hit=new(){position=p};return !Blocked;}
  public static int Calls105;
  public static bool CalculatePath(Vector3 a,Vector3 b,int mask,NavMeshPath p){Calls105++;p.corners=Detour>0?new[]{a,new Vector3(a.x,0,Detour),new Vector3(b.x,0,Detour),b}:new[]{a,b};p.status=NavMeshPathStatus.PathComplete;return !Blocked;}
 }
}
public static partial class AOHIBKLOAEI{public static AFAGFHJONFJ Region;public static System.Func<Vector3,AFAGFHJONFJ> RegionAt;public static AFAGFHJONFJ PNKKPMCJIGA(Vector3 p)=>RegionAt!=null?RegionAt(p):Region;}
public partial class Wo_TransportTeamCtrl:Native{public static float GetMoveSpeed(string faction)=>2;}
public partial class Wo_DeployTeamCtrl{
 public int Food=200,DailyFood=20,Morale=100;public bool ThrowFood,ThrowAfterFood;public int FoodCap=int.MaxValue;
 public int GetFood()=>Food;public int GetOneDayFood()=>DailyFood;public int GetMorale()=>Morale;
 public void AddFood(int n){if(ThrowFood)throw new Exception("food injection fault");Food=(int)Math.Min(FoodCap,(long)Food+n);if(ThrowAfterFood)throw new Exception("food post-credit fault");}
}
public static class SupplyTests
{
 static int tests;static NJHDEHLHENN war;static AFAGFHJONFJ city;static Wo_DeployTeamCtrl army;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,args);
 static void Check(bool b,string text){tests++;if(!b)throw new Exception(text);}
 static void Tick(double t)=>Call("SupplyTick",war,t,true);
 static long Reserved()=> (long)Call("ReservedFood",city.OJPECMNINNA,null);
 static void Setup(){
  MICGPAMNDKM.IJPKEMPIFIH=new();ICGAEMLACJG.HFJIECIBFKB.Clear();NEINKOOBJIH.COCMABJFOFH.Clear();
  var f=new MKMOJNAJKJE{IBGCLCAIBOL="F"};NEINKOOBJIH.COCMABJFOFH["F"]=f;
  f.AENJACOFBJK["enemy"]=new(){EBIOPDHEBOF=BHBNKONCOJD.War};
  city=new(){OJPECMNINNA="C",BMBMKPGEHBH=new(0)};city.LMPMNLLGOEC.LMMNKIOAGLC="F";city.LMPMNLLGOEC.OEGNGFBCHFL=100000;
  city.LMPMNLLGOEC.KJPPGIOCDID.Add(new(){FGHFOLKBHDL=1000});MICGPAMNDKM.IJPKEMPIFIH["C"]=city;
  war=new(){GBHGENNMJCK=new(20)};army=new(){m_id=88,m_belongPower="F",m_warInfo=war};army.m_dicDeployArmy["g"]=new(){new(){FGHFOLKBHDL=1000}};war.LGPMOKOLMLN[0].Add(army);
  UnityEngine.AI.NavMesh.Blocked=false;UnityEngine.AI.NavMesh.Detour=0;AOHIBKLOAEI.Region=null;
  new StrategicSupport().Load();Call("Ready");
  Wa_Controller.m_warInfo=war;Wa_Controller.m_battleUI=new();Wa_Controller.FoodRefreshes=0;
  Wa_Controller.m_isQuiting=Wa_Controller.m_isBeforeWar=Wa_Controller.m_isByEvent=Wa_Controller.m_isRoundMode=false;
  Wa_Controller.m_mockInfo=null;Wa_Controller.m_battleSpeed=1;Time.timeScale=Time.deltaTime=Time.unscaledDeltaTime=1;
  AEHAGJAKNEK.PAKENCLGOHK=false;AEHAGJAKNEK.IAICEJLNKFJ=1;
 }
 public static void Run(){
  Setup();Tick(0);long reserved=Reserved();Check(reserved>0,"low supplies trigger without troop losses");
  Check(city.LMPMNLLGOEC.OEGNGFBCHFL==100000&&army.Food==200,"no instant delivery or early debit");
  Tick(5);Check(Reserved()==reserved,"pending request prevents duplicate reservation");Tick(19);Check(army.Food==200,"ETA respected");
  army.Food=0;Tick(24);Check(army.Food==1200&&city.LMPMNLLGOEC.OEGNGFBCHFL==98800,"arrival debit equals credit");int food=army.Food;Tick(29);Check(army.Food==food,"no duplicate arrival");
  Check(Wa_Controller.FoodRefreshes==1,"native food-exhaustion state refreshed once at delivery");
  Setup();Tick(0);city.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Tick(25);Check(army.Food==200&&city.LMPMNLLGOEC.OEGNGFBCHFL==100000&&Reserved()==0,"captured source cancels without theft or loss");
  Setup();Tick(0);city.LMPMNLLGOEC.OEGNGFBCHFL=30001;army.Food=0;Tick(25);Check(army.Food==1&&city.LMPMNLLGOEC.OEGNGFBCHFL==30000,"arrival cannot consume home reserve after other spending");
  Setup();city.FKMKGNPFLFP.Add(new(){LMPMNLLGOEC=new(){LMMNKIOAGLC="enemy"}});city.LMPMNLLGOEC.OEGNGFBCHFL=60000;Tick(0);Check(Reserved()==0,"frontier stock retained at double floor");
  Setup();Tick(0);army.m_id++;Tick(25);Check(army.Food==200,"recycled team cannot receive old shipment");
  Setup();Tick(0);war.LGPMOKOLMLN[0].Clear();Tick(25);Check(army.Food==200&&Reserved()==0,"withdrawn army cancels");
  Setup();Tick(0);Call("ForgetBattle",war);Check(Reserved()==0&&city.LMPMNLLGOEC.OEGNGFBCHFL==100000,"battle end frees all reservations");
  Setup();Tick(0);Call("ResetBattleState");Check(Reserved()==0&&army.Food==200,"save/world reset conserves food");
  Setup();Tick(0);UnityEngine.AI.NavMesh.Blocked=true;Tick(25);Check(army.Food==200&&Reserved()==0,"route lost before arrival cancels");
  Setup();Tick(0);UnityEngine.AI.NavMesh.Detour=30;Tick(25);Check(army.Food==200&&Reserved()>0,"longer valid detour delays original ETA");Tick(55);Check(army.Food==1200&&Reserved()==0,"detour supplies arrive only after revised ETA");
  Setup();UnityEngine.AI.NavMesh.Blocked=true;Tick(0);Check(Reserved()==0,"no complete native path no shipment");
  Setup();AOHIBKLOAEI.Region=new(){OJPECMNINNA="hostile",LMPMNLLGOEC=new(){LMMNKIOAGLC="third-party"}};Tick(0);Check(Reserved()==0,"hostile transit rejected");
  Setup();Tick(0);army.Food=5000;Tick(25);Check(army.Food==5000&&city.LMPMNLLGOEC.OEGNGFBCHFL==100000,"native/manual resupply prevents oversupply");
  Setup();ICGAEMLACJG.HFJIECIBFKB[1]=new(){m_belongPower="enemy",m_targetCity="C"};Tick(0);Check(Reserved()==0,"invaded source retains food");
  Setup();Tick(0);army.ThrowFood=true;bool fault=false;try{Tick(25);}catch{fault=true;}Check(!fault&&army.Food==200&&city.LMPMNLLGOEC.OEGNGFBCHFL==100000&&Reserved()==0,"failed native credit is contained, rolls back debit and releases reservation");
  Setup();Tick(0);army.ThrowAfterFood=true;Tick(25);
  Check(army.Food+city.LMPMNLLGOEC.OEGNGFBCHFL==100200&&army.Food==1200&&Reserved()==0,"post-credit exception must not duplicate food by refunding delivered stock");
  Setup();Tick(0);army.FoodCap=700;Tick(25);
  Check(army.Food==700&&city.LMPMNLLGOEC.OEGNGFBCHFL==99500,"native food cap refunds only undelivered portion");
  Setup();Tick(0);Call("SupplyTick",war,10000d,false);Check(army.Food==200,"world/direct clock transition restarts request without instant delivery");
  Setup();army.Food=100000;Tick(0);Check(Reserved()==0,"well supplied army not topped up unnecessarily");
  Setup();Tick(0);army.m_dicDeployArmy.Clear();Tick(25);Check(army.Food==200&&Reserved()==0,"destroyed army gets no food");
  Setup();Tick(0);for(int i=0;i<100;i++){Time.timeScale=0;Call("DirectBattleTick");}Check(army.Food==200,"paused battle does not advance supply clock");
  Setup();
  ((BepInEx.Configuration.ConfigEntry<bool>)typeof(StrategicSupport).GetField("battleSupport",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Value=false;
  for(int i=0;i<26;i++)Call("DirectBattleTick");Check(army.Food==1200,"food supply remains enabled independently of troop reinforcement");
  Setup();Tick(0);((BepInEx.Configuration.ConfigEntry<bool>)typeof(StrategicSupport).GetField("autoSupply",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Value=false;Tick(25);
  Check(Reserved()==0&&army.Food==200,"disabled supply cancels outstanding reservations");
  Setup();city.LMPMNLLGOEC.OEGNGFBCHFL=31000;Tick(0);var war2=new NJHDEHLHENN{HIENOINGBCN="second",GBHGENNMJCK=new(20)};
  var army2=new Wo_DeployTeamCtrl{m_id=99,m_belongPower="F",m_warInfo=war2};army2.m_dicDeployArmy["g"]=new(){new(){FGHFOLKBHDL=1000}};war2.LGPMOKOLMLN[0].Add(army2);
  Call("SupplyTick",war2,0d,true);Check(Reserved()==1000,"two battles cannot reserve the same remaining city food");
  var o=new SupplyObservation{Soldiers=1000,Food=1000,Burn=10,Morale=50,MoraleLoss=5};
  Check(SupplyPolicy.Amount(o,20,120,30,5000)>0,"observed morale decline requests earlier");
  o.Loss=100;Check(SupplyPolicy.Amount(o,20,120,30,5000)==0,"destroyed before arrival not promised rescue");
  o.Loss=0;Check(SupplyPolicy.Amount(o,double.NaN,120,30,5000)==0,"invalid ETA rejected");
  var r=new Random(53);
  for(int i=0;i<20000;i++){
   int stock=r.Next(int.MaxValue),reserve=r.Next(int.MaxValue),held=r.Next(int.MaxValue);int spare=SupplyPolicy.Available(stock,reserve,held);
   Check(spare>=0&&(long)spare<=Math.Max(0L,(long)stock-reserve-held),"bounded available food");
   o=new(){Soldiers=r.Next(1,1000000),Food=r.Next(10000000),Burn=r.NextDouble()*100000,Loss=r.NextDouble()*1000,Morale=r.Next(101),MoraleLoss=r.NextDouble()*2};
   int n=SupplyPolicy.Amount(o,r.NextDouble()*1000,120,30,spare);Check(n>=0&&n<=spare,"reservation within stock");
  }
  Console.WriteLine($"PASS {tests} supply policy, resource conservation and lifecycle assertions (mocked native path/clock; not gameplay)");
 }
}
