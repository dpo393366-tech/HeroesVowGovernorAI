using System;using System.Collections.Generic;using System.Linq;using System.Reflection;
using BepInEx;using BepInEx.Configuration;using BepInEx.Unity.IL2CPP;using HarmonyLib;using UnityEngine;
namespace Vow.Settings {
 [BepInPlugin("local.heroesvow.unifiedsettings","Heroes Vow convenience F12 settings","1.4.0")]
 [BepInDependency("local.heroesvow.treasuryawards",BepInDependency.DependencyFlags.SoftDependency)]
 [BepInDependency("local.heroesvow.governorwatch",BepInDependency.DependencyFlags.SoftDependency)]
 public sealed class UnifiedSettings:BasePlugin {
  internal static UnifiedSettings instance;
  public override void Load(){instance=this;AddComponent<SettingsPanel>();SettingsPanel.Discover();Log.LogInfo("F12 1.4.0: independent governor, treasury and recruitment settings.");}
 }
 public sealed class SettingsPanel:MonoBehaviour {
  public SettingsPanel(IntPtr p):base(p){}
  internal static readonly SettingsRegistry menu=new SettingsRegistry();
  static readonly Harmony patches=new Harmony("local.heroesvow.unifiedsettings");
  const BindingFlags Fields=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static;
  static readonly Dictionary<MethodBase,FieldInfo> overlays=new Dictionary<MethodBase,FieldInfo>();
  static readonly List<FieldInfo> openFields=new List<FieldInfo>();
  static readonly HashSet<string> faults=new HashSet<string>();
  static readonly string[] modules={"GovernorWatch","TreasuryAwards","StrategicSupport"};
  static ConfigEntry<int> treasuryPercent;static Type governorApi;static int governorTab,recruitGender;static string recruitAmount="30",recruitMessage="";
  Vector2 scroll;float nextScan;
  internal static void Discover(){
   foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()){
    if(!modules.Contains(assembly.GetName().Name))continue;
    try{
     if(assembly.GetName().Name=="StrategicSupport"){
      governorApi=assembly.GetType("StrategicSupport");menu.Add("R","도독 AI",Governor);continue;
     }
     if(assembly.GetName().Name=="TreasuryAwards"){
      var entry=assembly.GetType("TreasuryAwards")?.GetField("RecipientPercent",Fields)?.GetValue(null) as ConfigEntry<int>;
      if(entry!=null){treasuryPercent=entry;menu.Add("R","도독 AI",Governor);}
      continue;
     }
     foreach(var type in assembly.GetTypes()){
     if(type.Name!="Panel")continue;
     var gui=type.GetMethod("OnGUI",BindingFlags.Public|BindingFlags.Instance);
     if(gui==null||overlays.ContainsKey(gui))continue;
     if(type.Name=="Panel"){
      var draw=type.GetField("Draw",Fields);var key=type.GetField("key",Fields)?.GetValue(null) as string;
      var opened=type.GetField("opened",Fields);var title=type.GetField("title",Fields)?.GetValue(null) as string;
      if(draw?.GetValue(null) is not Action||key==null||opened?.FieldType!=typeof(bool))continue;
      patches.Patch(gui,prefix:new HarmonyMethod(typeof(SettingsPanel),nameof(LegacyGUI)));
      overlays.Add(gui,type.GetField("Overlay",Fields));openFields.Add(opened);
      // Hide controls only; retain each module's updates and gameplay overlays.
      if(key=="G")continue;
      menu.Add(key,title,()=>((Action)draw.GetValue(null))?.Invoke());
     }
     UnifiedSettings.instance.Log.LogInfo("Settings UI registered: "+assembly.GetName().Name+" / "+type.FullName);
    }}catch(Exception e){Fault(assembly.GetName().Name,e);}
   }
  }
  static bool LegacyGUI(MethodBase __originalMethod){
   if(!overlays.TryGetValue(__originalMethod,out var field))return true;
   // Timers/map forecasts are gameplay overlays, not settings windows.
   try{(field?.GetValue(null) as Action)?.Invoke();}catch(Exception e){Fault("overlay "+__originalMethod.DeclaringType.FullName,e);}
   return false;
  }
  static void Fault(string name,Exception e){if(faults.Add(name))UnifiedSettings.instance.Log.LogError(name+": "+e);}
  public void Update(){if(Input.GetKeyDown(KeyCode.F12)&&!Input.GetKey(KeyCode.LeftControl)&&!Input.GetKey(KeyCode.RightControl)&&!Input.GetKey(KeyCode.LeftShift)&&!Input.GetKey(KeyCode.RightShift)&&!Input.GetKey(KeyCode.LeftAlt)&&!Input.GetKey(KeyCode.RightAlt)){menu.open=!menu.open;scroll=Vector2.zero;}}
  public void LateUpdate(){
   if(Time.unscaledTime>=nextScan){nextScan=Time.unscaledTime+2;Discover();}
   // Synchronize convenience panels only; never touch the independent parry input gate.
   foreach(var field in openFields)field.SetValue(null,menu.open);
  }
  public void OnGUI(){
   if(!menu.open||menu.tabs.Count==0)return;
   var layout=new SettingsLayout(Screen.width,Screen.height,menu.tabs.Count);
   var box=new Rect(layout.x,layout.y,layout.width,layout.height);GUI.Box(box,"F12 · 도독 AI · 국고 · 등용");
   if(GUI.Button(new Rect(box.xMax-66,box.y+5,58,26),"닫기")){menu.open=false;foreach(var field in openFields)field.SetValue(null,false);return;}
   for(int i=0;i<menu.tabs.Count;i++){
    var tab=menu.tabs[i];var r=new Rect(box.x+10+(i%layout.columns)*layout.tabWidth,box.y+36+(i/layout.columns)*34,layout.tabWidth-4,30);
    var old=GUI.backgroundColor;if(tab.key==menu.selected)GUI.backgroundColor=new Color(.5f,.8f,1);
    bool clicked=GUI.Button(r,tab.title);GUI.backgroundColor=old;
    if(clicked&&menu.selected!=tab.key){menu.Select(tab.key);scroll=Vector2.zero;AppDomain.CurrentDomain.SetData("HeroesVow.Expansion.SettingsTab",tab.key);return;}
   }
   GUILayout.BeginArea(new Rect(box.x+12,box.y+layout.contentY,box.width-24,layout.contentHeight));
   try{scroll=GUILayout.BeginScrollView(scroll);try{menu.DrawSelected();}finally{GUILayout.EndScrollView();}}finally{GUILayout.EndArea();}
  }
  static object GovernorCall(string name,params object[] args){try{return governorApi?.GetMethod(name,BindingFlags.Public|BindingFlags.Static)?.Invoke(null,args);}catch(Exception e){Fault("governor "+name,e);return "처리를 완료하지 못했습니다. 로그를 확인해 주세요.";}}
  static void Governor(){
   GUILayout.BeginHorizontal();if(GUILayout.Button("국고 분배"))governorTab=0;if(GUILayout.Button("자동 등용"))governorTab=1;if(GUILayout.Button("보고 설정"))governorTab=2;GUILayout.EndHorizontal();
   if(governorTab==2){ArrivalReports();return;}
   if(governorTab==0){if(treasuryPercent!=null)Treasury();else GUILayout.Label("국고 분배 모드 로딩 대기");return;}
   if(governorApi?.GetMethod("RecruitmentQueueRows")==null){GUILayout.Label("목록형 자동 등용을 지원하는 도독 AI 버전이 필요합니다.");return;}
   GUILayout.Label((string)GovernorCall("RecruitmentQueueStatus"));
   GUILayout.BeginHorizontal();if(GUILayout.Toggle(recruitGender==0,"전체"))recruitGender=0;if(GUILayout.Toggle(recruitGender==1,"남성만"))recruitGender=1;if(GUILayout.Toggle(recruitGender==2,"여성만"))recruitGender=2;GUILayout.EndHorizontal();
   GUILayout.Label("실제 합류 목표 인원 · 능력이 높은 재야 장수와 포로 우선");recruitAmount=GUILayout.TextField(recruitAmount,4);
   if(GUILayout.Button("예약 등록"))recruitMessage=int.TryParse(recruitAmount,out int count)?(string)GovernorCall("StartRecruitmentQueue",count,recruitGender):"인원을 숫자로 입력해 주세요.";
   GUILayout.Label("위에서부터 하나씩 실행합니다. 선두 예약 중지 시 뒤 예약도 대기합니다.");
   var rows=GovernorCall("RecruitmentQueueRows") as string[][];int changed=0;string operation=null;
   if(rows==null||rows.Length==0)GUILayout.Label("등록된 예약이 없습니다.");
   else foreach(var row in rows){
    if(row==null||row.Length<7||!int.TryParse(row[0],out int id))continue;
    var color=GUI.backgroundColor;GUI.backgroundColor=row[6]=="2"?new Color(.9f,.75f,.4f):row[4].StartsWith("실행 중")?new Color(.4f,.85f,.55f):color;
    GUILayout.BeginVertical("box");GUI.backgroundColor=color;
    try{
     GUILayout.BeginHorizontal();GUILayout.Label("#"+id+" · "+row[1]+" · 성공 "+row[2]);if(GUILayout.Button("X",GUILayout.Width(32))){changed=id;operation="cancel";}GUILayout.EndHorizontal();
     GUILayout.Label(row[4]+"  |  진행 중 임무 "+row[3]+"명");
     if(!string.IsNullOrEmpty(row[5])&&GUILayout.Button(row[5])){changed=id;operation=row[5]=="중지"?"pause":"resume";}
    }finally{GUILayout.EndVertical();}
   }
   if(operation!=null)recruitMessage=(string)(operation=="cancel"?GovernorCall("CancelRecruitmentQueue",changed):GovernorCall("SetRecruitmentQueuePaused",changed,operation=="pause"));
   GUILayout.Label("X: 새 파견 종료. 이미 진행 중인 임무를 정리한 뒤 다음 예약으로 넘어갑니다. 완료 항목의 X는 목록에서 제거합니다.");
   if(!string.IsNullOrEmpty(recruitMessage))GUILayout.Label(recruitMessage);
  }
  static void ArrivalReports(){
   var ordinary=governorApi?.GetField("ShowCityArrivalReport",Fields)?.GetValue(null) as ConfigEntry<bool>;
   var capacity=governorApi?.GetField("ShowCityCapacityReport",Fields)?.GetValue(null) as ConfigEntry<bool>;
   if(ordinary==null||capacity==null){GUILayout.Label("보고 설정을 지원하는 도독 AI 버전이 필요합니다.");return;}
   bool show=GUILayout.Toggle(ordinary.Value,"부대 입성 보고 표시");if(show!=ordinary.Value)ordinary.Value=show;
   GUILayout.Label("일반 입성 보고창 · 켜도 바닐라 전령 보고 설정을 따릅니다.");
   show=GUILayout.Toggle(capacity.Value,"병력 수용량 초과 보고 표시");if(show!=capacity.Value)capacity.Value=show;
   GUILayout.Label("입성 시 수용량 초과 보고창 · 바닐라 설정과 별도로 끌 수 있습니다.");
   GUILayout.Label("변경 즉시 저장·적용 · 이미 열린 보고창은 유지됩니다.");
   GUILayout.Label("보고창만 제어합니다. 부대 입성·병력 수용량·초과 병력 처리는 그대로입니다.");
  }
  static void Treasury(){
   Slider("분배 대상 · 능력치 상위 (%)",treasuryPercent,1,100);
   GUILayout.Label("다음 자동분배 실행부터 적용 · 주인공 제외 · 인원 올림");
   GUILayout.Label("현재 장비보다 확실히 개선되는 장비만 · 무기는 현재와 같은 종류");
   GUILayout.Label("무력/지력 성향에 맞는 성장 소비품 우선 · 남은 빈칸은 가격순");
  }
  static void Slider(string label,ConfigEntry<int> entry,int min,int max){GUILayout.Label(label+"  "+entry.Value);int n=Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(entry.Value,min,max)),min,max);if(n!=entry.Value)entry.Value=n;}
 }
}