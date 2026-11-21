using System;using System.IO;using System.Linq;using System.Reflection;using System.Runtime.Loader;using System.Collections.Generic;using Vow.Settings;
class SettingsTest {
 static void Check(bool b,string s){if(!b)throw new Exception(s);Console.WriteLine("PASS "+s);}
 static void Main(){AssemblyLoadContext.Default.Resolving+=(c,n)=>{string p=Path.Combine(Environment.GetEnvironmentVariable("HV_BEPINEX"),"core",n.Name+".dll");return File.Exists(p)?c.LoadFromAssemblyPath(p):null;};Run();}
 static void Run(){
  string[] names={"DuelCounters","FieldEncounters","ReserveWaves","GovernorWatch","EnemyTempo","DuelAggression","FramePacing","DodgeLimit"};string[] keys={"T","E","W","G","U","C","X"};
  var types=new List<Type>();var instances=new List<object>();var draws=new Dictionary<string,int>();int overlay=0;
  for(int i=0;i<names.Length;i++){
   var a=Assembly.LoadFrom(Path.GetFullPath("settings-test/"+names[i]+".dll"));var t=a.GetType(i==7?"Vow.Dodge.DodgePanel":"Fake.Panel");types.Add(t);instances.Add(Activator.CreateInstance(t));
   if(i<7){string k=keys[i];draws[k]=0;t.GetField("key").SetValue(null,k);t.GetField("title").SetValue(null,k);t.GetField("Draw").SetValue(null,(Action)(()=>draws[k]++));t.GetField("Overlay").SetValue(null,(Action)(()=>overlay++));}
  }
  var treasury=Assembly.LoadFrom(Path.GetFullPath("settings-test/TreasuryAwards.dll")).GetType("TreasuryAwards");
  var recipient=(BepInEx.Configuration.ConfigEntry<int>)treasury.GetField("RecipientPercent").GetValue(null);
  var governor=Assembly.LoadFrom(Path.GetFullPath("settings-test/StrategicSupport.dll")).GetType("StrategicSupport");
  new UnifiedSettings().Load();var ui=new SettingsPanel(IntPtr.Zero);var menu=SettingsPanel.menu;
  Check(menu.tabs.Select(x=>x.key).SequenceEqual(new[]{"R"}),"F12 exposes only existing convenience tabs");
  Check(!menu.Select("T")&&!menu.Select("Q"),"parry and dodge never attach to F12");
  SettingsPanel.Discover();Check(menu.tabs.Count==1,"repeated discovery is idempotent");
  for(int i=0;i<types.Count;i++)types[i].GetMethod("OnGUI").Invoke(instances[i],null);
  Check((int)types[3].GetField("originalDraws").GetValue(null)==0&&(int)types[1].GetField("originalDraws").GetValue(null)==1&&(int)types[2].GetField("originalDraws").GetValue(null)==1,"convenience legacy windows intercepted");
  Check((int)types[0].GetField("originalDraws").GetValue(null)==1&&(int)types[7].GetField("originalDraws").GetValue(null)==1,"independent parry and dodge rendering untouched");
  Check(overlay==7,"all gameplay overlays still run");
  UnityEngine.Input.f11=true;ui.Update();for(int i=0;i<types.Count;i++)types[i].GetMethod("Update").Invoke(instances[i],null);ui.LateUpdate();UnityEngine.Input.f11=false;
  Check(!menu.open,"F11 does not open convenience settings");Check((bool)types[0].GetField("opened").GetValue(null),"F11 still opens parry");
  Check(types.Skip(1).Take(3).All(t=>!(bool)t.GetField("opened").GetValue(null)),"F11 leaves convenience panels closed");
  UnityEngine.Input.f12=true;ui.Update();for(int i=0;i<types.Count;i++)types[i].GetMethod("Update").Invoke(instances[i],null);ui.LateUpdate();UnityEngine.Input.f12=false;
  Check(menu.open&&types.Skip(1).Take(3).All(t=>(bool)t.GetField("opened").GetValue(null)),"F12 opens convenience UI and synchronizes owned gates");
  Check((bool)types[0].GetField("opened").GetValue(null),"F12 does not change parry open state");
  menu.Select("R");UnityEngine.GUILayout.sliders=0;ui.OnGUI();Check(recipient.Value==50&&UnityEngine.GUILayout.sliders==1,"treasury value preserved on open");
  UnityEngine.GUILayout.edit=0;UnityEngine.GUILayout.replacement=75;UnityEngine.GUILayout.sliders=0;ui.OnGUI();Check(recipient.Value==75,"treasury editable in F12");UnityEngine.GUILayout.edit=-1;
  UnityEngine.GUILayout.click="자동 등용";ui.OnGUI();
  UnityEngine.GUILayout.select="여성만";UnityEngine.GUILayout.text="30";UnityEngine.GUILayout.click="예약 등록";ui.OnGUI();
  Check((int)governor.GetField("Amount").GetValue(null)==30&&(int)governor.GetField("Gender").GetValue(null)==2,"recruitment tab sends exact count and female filter");
  UnityEngine.GUILayout.select=null;UnityEngine.GUILayout.text="bad";UnityEngine.GUILayout.click="예약 등록";ui.OnGUI();Check((int)governor.GetField("Calls").GetValue(null)==1,"invalid count never starts queue");
  UnityEngine.GUILayout.click="중지";ui.OnGUI();Check((bool)governor.GetField("Paused").GetValue(null),"pause calls governor queue");
  UnityEngine.GUILayout.click="재개";ui.OnGUI();Check(!(bool)governor.GetField("Paused").GetValue(null),"resume calls governor queue");
  UnityEngine.GUILayout.click="X";ui.OnGUI();Check((int)governor.GetField("CancelledId").GetValue(null)==1,"per-card X uses stable queue ID");
  UnityEngine.GUILayout.click="X";UnityEngine.GUILayout.skipMatches=1;ui.OnGUI();Check((int)governor.GetField("CancelledId").GetValue(null)==2,"second-card X never cancels first card");
  UnityEngine.GUILayout.click="중지";UnityEngine.GUILayout.skipMatches=1;ui.OnGUI();Check((int)governor.GetField("PausedId").GetValue(null)==2&&!(bool)governor.GetField("Paused").GetValue(null),"second-card pause never pauses first card");
  UnityEngine.GUILayout.text=null;
  UnityEngine.GUILayout.click="보고 설정";ui.OnGUI();
  var arrival=(BepInEx.Configuration.ConfigEntry<bool>)governor.GetField("ShowCityArrivalReport").GetValue(null);
  var capacity=(BepInEx.Configuration.ConfigEntry<bool>)governor.GetField("ShowCityCapacityReport").GetValue(null);
  Check(arrival.Value&&capacity.Value,"report defaults preserved on opening");
  UnityEngine.GUILayout.select="병력 수용량 초과 보고 표시";ui.OnGUI();UnityEngine.GUILayout.select=null;
  Check(arrival.Value&&!capacity.Value,"capacity-only toggle updates governor config immediately");
  UnityEngine.GUILayout.select="부대 입성 보고 표시";ui.OnGUI();UnityEngine.GUILayout.select=null;
  Check(!arrival.Value&&!capacity.Value,"ordinary toggle independent from capacity");
  SettingsPanel.Discover();ui.OnGUI();Check(!arrival.Value&&!capacity.Value,"report rediscovery preserves choices");
  UnityEngine.GUILayout.select="병력 수용량 초과 보고 표시";ui.OnGUI();UnityEngine.GUILayout.select=null;
  Check(!arrival.Value&&capacity.Value,"capacity reports can be reenabled independently");
  UnityEngine.GUILayout.click="장수 배치";ui.OnGUI();
  var train=(BepInEx.Configuration.ConfigEntry<bool>)governor.GetField("ProtectFamilyPrivateTraining").GetValue(null);
  var family=(BepInEx.Configuration.ConfigEntry<bool>)governor.GetField("ExcludeFamilyAutoRelocation").GetValue(null);
  Check(train.Value&&!family.Value,"family deployment defaults: unfinished private training protected, whole household opt-in");
  UnityEngine.GUILayout.select="가문 장수 전체 자동 전근 제외";ui.OnGUI();UnityEngine.GUILayout.select=null;Check(train.Value&&family.Value,"whole household toggle independent of trainer protection");
  UnityEngine.GUILayout.select="사병 육성 중인 가문 장수 보호";ui.OnGUI();UnityEngine.GUILayout.select=null;Check(!train.Value&&family.Value,"trainer toggle does not silently release whole household protection");
  SettingsPanel.Discover();ui.OnGUI();Check(!train.Value&&family.Value,"family relocation choices survive panel rediscovery");
  Check(!arrival.Value&&capacity.Value,"family toggles preserve independent report settings");
  foreach(var k in new[]{"E","W"})Check(!menu.Select(k),"separate module excluded from governor tabs "+k);
  var parryBefore=(bool)types[0].GetField("opened").GetValue(null);
  UnityEngine.Input.f12=true;foreach(var t in types.Select((t,i)=>(t,i)))t.t.GetMethod("Update").Invoke(instances[t.i],null);ui.Update();ui.LateUpdate();UnityEngine.Input.f12=false;
  Check(!menu.open&&types.Skip(1).Take(3).All(t=>!(bool)t.GetField("opened").GetValue(null)),"F12 closes regardless of update order");Check((bool)types[0].GetField("opened").GetValue(null)==parryBefore,"closing F12 preserves F11 state");
  Check(recipient.Value==75,"rediscovery preserves saved settings");
  menu.open=true;UnityEngine.GUI.click="닫기";ui.OnGUI();Check(!menu.open,"close button returns without throwing ExitGUI across IL2CPP");
  menu.open=false;UnityEngine.Input.f12=true;
  foreach(var modifier in new[]{UnityEngine.KeyCode.LeftControl,UnityEngine.KeyCode.RightControl,UnityEngine.KeyCode.LeftShift,UnityEngine.KeyCode.RightShift,UnityEngine.KeyCode.LeftAlt}){UnityEngine.Input.held=modifier;ui.Update();Check(!menu.open,"modified F12 does not open governor "+modifier);}
  UnityEngine.Input.held=null;UnityEngine.Input.f12=false;
  foreach(var size in new[]{(640,480),(1280,720),(1920,1080),(2560,1440)}){var l=new SettingsLayout(size.Item1,size.Item2,3);Check(l.x>=0&&l.y>=0&&l.x+l.width<=size.Item1&&l.y+l.height<=size.Item2&&l.contentHeight>120,"layout fits "+size);}
 }
}
namespace BepInEx {
 [AttributeUsage(AttributeTargets.Class)]public sealed class BepInPlugin:Attribute{public BepInPlugin(string a,string b,string c){}}
 [AttributeUsage(AttributeTargets.Class,AllowMultiple=true)]public sealed class BepInDependency:Attribute{public enum DependencyFlags{SoftDependency}public BepInDependency(string a,DependencyFlags b){}}
}
namespace BepInEx.Configuration {public class ConfigFile { public ConfigEntry<T> Bind<T>(string section,string key,T value,string description)=>new(value); }public class ConfigEntry<T>{public T Value{get;set;}public ConfigEntry(T v){Value=v;}}}
namespace BepInEx.Unity.IL2CPP {public abstract class BasePlugin {public BepInEx.Configuration.ConfigFile Config=new();public class Logger{public void LogInfo(object v){}public void LogError(object v){Console.WriteLine(v);}}public Logger Log=new Logger();public abstract void Load();public T AddComponent<T>(){return (T)Activator.CreateInstance(typeof(T),IntPtr.Zero);}}}
namespace UnityEngine {
 public class MonoBehaviour{public MonoBehaviour(IntPtr p){}}
 public enum KeyCode{F11,F12,LeftControl,RightControl,LeftShift,RightShift,LeftAlt,RightAlt}public static class Input{public static KeyCode? held;public static bool GetKey(KeyCode k)=>held==k;public static bool f11,f12;public static bool GetKeyDown(KeyCode k)=>k==KeyCode.F11?f11:f12;}
 public static class Time{public static float unscaledTime;}public static class Screen{public static int width=1280,height=720;}
 public struct Vector2{public static Vector2 zero=>default;}
 public struct Color{public Color(float a,float b,float c){}}
 public struct Rect{public float x,y,width,height;public float xMax=>x+width;public Rect(float a,float b,float c,float d){x=a;y=b;width=c;height=d;}}
 public static class GUI{public static Color backgroundColor;public static void Box(Rect r,string s){}public static string click;public static bool Button(Rect r,string s){if(click!=s)return false;click=null;return true;}}
 public static class GUIUtility{public static void ExitGUI(){throw new Exception("ExitGUI test path");}}
 public class GUILayoutOption {}
 public static class GUILayout{public static GUILayoutOption Width(float n)=>new();public static void BeginVertical(string style){}public static void EndVertical(){}public static int skipMatches;public static string click,select,text;public static void BeginHorizontal(){}public static void EndHorizontal(){}public static bool Button(string s,params GUILayoutOption[] options){if(click!=s)return false;if(skipMatches>0){skipMatches--;return false;}click=null;return true;}public static string TextField(string s,int max)=>text??s;public static bool? toggle;public static int sliders,edit=-1;public static float replacement;public static void BeginArea(Rect r){}public static void EndArea(){}public static Vector2 BeginScrollView(Vector2 p)=>p;public static void EndScrollView(){}public static bool Toggle(bool b,string s){if(s=="전체"||s=="남성만"||s=="여성만")return select==null?b:select==s;if(s=="부대 입성 보고 표시"||s=="병력 수용량 초과 보고 표시"||s=="사병 육성 중인 가문 장수 보호"||s=="가문 장수 전체 자동 전근 제외")return select==s?!b:b;if(!s.StartsWith("패링 이펙트 표시")&&!s.StartsWith("회피 제한 사용"))throw new Exception("Unexpected checkbox "+s);return toggle??b;}public static void Label(string s){}public static float HorizontalSlider(float v,float a,float b)=>sliders++==edit?replacement:v;}
 public static class Mathf{public static int Clamp(int v,int a,int b)=>Math.Clamp(v,a,b);public static int RoundToInt(float v)=>(int)Math.Round(v);public static float Round(float v)=>(float)Math.Round(v);public static float Clamp(float v,float a,float b)=>Math.Clamp(v,a,b);}
}


