using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace UnityEngine {
 public class LineRenderer:Native{public bool enabled=true;public static implicit operator bool(LineRenderer x)=>x!=null;}
 public class Collider{public object Component;public T GetComponent<T>() where T:class=>Component as T;public static implicit operator bool(Collider x)=>x!=null;}
}
public class HUD_DeployTeam:Native{public GameObject gameObject=new(){activeSelf=true};public static implicit operator bool(HUD_DeployTeam x)=>x!=null;}
public class Wo_VillageCtrl:Native{public string m_id;}
public partial class Wo_DeployTeamCtrl{public HUD_DeployTeam m_hud;public LineRenderer m_lineRenderer,m_lineRendererArrow;}
public partial class MKMOJNAJKJE {public Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl> GIGPDOPDGLM=new(),CFLABFGBJLE=new();}
public static class Runtime89Tests
{
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("R89 runtime: "+why);}
 public static void Run(){
  typeof(CityIntegrationTests).GetMethod("Setup",F).Invoke(null,null);var faction=NEINKOOBJIH.COCMABJFOFH["F"];var city=MICGPAMNDKM.IJPKEMPIFIH["city"];
  var plain=new Wo_DeployTeamCtrl{m_id=1001,m_belongPower="F",m_targetCity="city",m_deployState=AKOPKFPKJPP.Expedition};
  var entry=new Wo_DeployTeamCtrl{m_id=1002,m_belongPower="F",m_targetCity="city",m_isEnterCity=true,m_deployState=AKOPKFPKJPP.Expedition};
  var coordinate=new Wo_DeployTeamCtrl{m_id=1003,m_belongPower="F",m_targetCity="",m_deployState=AKOPKFPKJPP.Expedition};coordinate.m_listMovePos.Add(new(3));
  var lost=new Wo_DeployTeamCtrl{m_id=1004,m_belongPower="F",m_targetCity="removed",m_deployState=AKOPKFPKJPP.Expedition};
  var war=new Wo_DeployTeamCtrl{m_id=1005,m_belongPower="F",m_targetCity="city",m_targetWarInfo=new(),m_deployState=AKOPKFPKJPP.Expedition};
  faction.GIGPDOPDGLM.AddRange(new[]{plain,coordinate,entry,lost,war});var original=faction.GIGPDOPDGLM;
  object[] args={faction,null};Call("BeforeNativeTeamRefresh",args);
  Check(faction.GIGPDOPDGLM.Count==1&&faction.GIGPDOPDGLM[0]==plain,"invalid city keys and explicit native/mod orders excluded only from city-retarget loop");
  foreach(var t in faction.GIGPDOPDGLM){var destination=MICGPAMNDKM.IJPKEMPIFIH[t.m_targetCity];t.m_isEnterCity=false;}
  Check(entry.m_isEnterCity&&coordinate.m_listMovePos.Count==1&&war.m_targetWarInfo!=null,"native retarget cannot erase entry, coordinate path or battle objective");
  Call("AfterNativeTeamRefresh",args[1]);Check(ReferenceEquals(original,faction.GIGPDOPDGLM)&&original.Count==5,"original faction registry restored, no armies removed");
  args=new object[]{faction,null};Call("BeforeNativeTeamRefresh",args);try{throw new Exception("native failure");}catch{}finally{Call("AfterNativeTeamRefresh",args[1]);}
  Check(ReferenceEquals(original,faction.GIGPDOPDGLM),"registry also restored on native exception");
  args=new object[]{faction,null};Call("BeforeNativeTeamRefresh",args);var replacement=new Il2CppSystem.Collections.Generic.List<Wo_DeployTeamCtrl>();faction.GIGPDOPDGLM=replacement;Call("AfterNativeTeamRefresh",args[1]);Check(ReferenceEquals(replacement,faction.GIGPDOPDGLM),"unrelated registry replacement is not overwritten by cleanup");
  var village=new Wo_VillageCtrl();var empty=new Wo_DeployTeamCtrl{m_id=2000};empty.m_listHero.Clear();
  Check(!(bool)Call("BeforeVillageTrigger",village,new Collider{Component=empty}),"empty hero array cannot reach native index zero");
  empty.m_listHero.Add(new());Check((bool)Call("BeforeVillageTrigger",village,new Collider{Component=empty}),"valid army retains native village interactions");
  Check((bool)Call("BeforeVillageTrigger",village,new Collider()),"unrelated collider retains native handling");
  Wa_Controller.m_warInfo=null;Wa_Controller.m_mockInfo=null;Call("SyncWorldVisuals");ICGAEMLACJG.HFJIECIBFKB.Clear();
  var team=new Wo_DeployTeamCtrl{m_id=3000,m_hud=new(),m_lineRenderer=new(),m_lineRendererArrow=new()};ICGAEMLACJG.HFJIECIBFKB[team.m_id]=team;
  Wa_Controller.m_warInfo=new();Call("SyncWorldVisuals");
  Check(!team.m_hud.gameObject.activeSelf&&!team.m_lineRenderer.enabled&&!team.m_lineRendererArrow.enabled,"world HUD and route arrows hidden during tactical battle");
  team.m_hud.gameObject.SetActive(true);Check(!(bool)Call("BeforeWorldHud",team)&&!team.m_hud.gameObject.activeSelf,"native HUD refresh cannot reshow a marching icon in battle");
  Wa_Controller.m_warInfo=null;Call("SyncWorldVisuals");Check(team.m_hud.gameObject.activeSelf&&team.m_lineRenderer.enabled,"world visuals restored when tactical scene ends");
  team.m_hud.gameObject.SetActive(false);Wa_Controller.m_warInfo=new();Call("SyncWorldVisuals");Wa_Controller.m_warInfo=null;Call("SyncWorldVisuals");Check(!team.m_hud.gameObject.activeSelf,"a HUD already hidden before battle stays hidden afterward");
  team.m_hud.gameObject.SetActive(true);Wa_Controller.m_warInfo=new();Call("SyncWorldVisuals");team.m_id++;Wa_Controller.m_warInfo=null;Call("SyncWorldVisuals");Check(!team.m_hud.gameObject.activeSelf,"pooled army identity mismatch prevents stale HUD resurrection");
  Console.WriteLine($"PASS R89 {checks} native order refresh, village collision, battle HUD and restoration checks; native calls mocked.");
 }
}
