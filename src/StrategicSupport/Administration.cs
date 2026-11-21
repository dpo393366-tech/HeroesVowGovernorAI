using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;

public sealed partial class StrategicSupport
{
 static ConfigEntry<bool> domestic,titles,awards,delegatedMayor;
 void ConfigureAdministration(){
  domestic=Config.Bind("Administration","Domestic",true,"Assign idle/exploring officers to needed civil duties in delegated cities; preserve research and defense assignments.");
  titles=Config.Bind("Administration","Titles",true,"Fill vacant official titles and promote eligible officers without demoting existing holders.");
  awards=Config.Bind("Administration","TreasuryAwards",true,"Automatically distribute treasury items through Treasury Awards, reading its current F12 rules each run.");
  delegatedMayor=Config.Bind("Administration","HonorGovernorDelegation",true,"An appointed AI governor may manage a city even if the player is still its mayor. Personal player governor districts remain manual.");
 }
 static bool ExplicitDelegation(AFAGFHJONFJ c){
  var r=c?.LMPMNLLGOEC;var p=NEINKOOBJIH.POMNMBGDDIN;
  return enabled?.Value==true&&delegatedMayor?.Value==true&&r!=null&&p?.LMPMNLLGOEC!=null
   &&r.LMMNKIOAGLC==p.LMPMNLLGOEC.LMMNKIOAGLC&&r.BKENJPNKMGI==NEINKOOBJIH.OPIKIHPNCIP
   &&!string.IsNullOrEmpty(r.IPJHOOJBCKI)&&r.IPJHOOJBCKI!=NEINKOOBJIH.OPIKIHPNCIP
   &&NEINKOOBJIH.COCMABJFOFH.TryGetValue(r.LMMNKIOAGLC,out var f)&&f?.ANMMMOIKLEG!=null
   &&f.ANMMMOIKLEG.TryGetValue(r.IPJHOOJBCKI,out var g)&&g?.LMMNKIOAGLC==r.LMMNKIOAGLC;
 }
 static void HonorDelegation(AFAGFHJONFJ __instance,ref bool __result){
  try{if(!__result&&ExplicitDelegation(__instance))__result=true;}catch(Exception e){Error(e);}
 }
 void HookAdministration(Harmony h){
  HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
  h.Patch(AccessTools.Method(typeof(AFAGFHJONFJ),"NBKEECPNIJH"),postfix:H(nameof(HonorDelegation)));
 }
 static void AfterCityAdministration(AFAGFHJONFJ c){
  try{
   if(!Ready()||c?.LMPMNLLGOEC==null||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(c.LMPMNLLGOEC.LMMNKIOAGLC,out var f)||f==null)return;
   TryFactionStatecraft(f);
   if(domestic.Value&&Eligible(c,out _)&&!c.GKLIEHCHDLD()&&AIGate("domestic:"+c.OJPECMNINNA,f.IBGCLCAIBOL,30)&&DayReady("domestic:"+c.OJPECMNINNA,DecisionDays(f.IBGCLCAIBOL))){
    DayReady("domestic:"+c.OJPECMNINNA,0,true);AssignDomestic(c,f);
   }
   if(!AIGate("administration:"+f.IBGCLCAIBOL,f.IBGCLCAIBOL,30)||!DayReady("administration:"+f.IBGCLCAIBOL,DecisionDays(f.IBGCLCAIBOL)))return;
   DayReady("administration:"+f.IBGCLCAIBOL,0,true);
   if(titles.Value)try{AssignTitles(f);}catch(Exception e){Error(e);}
   if(awards.Value&&f.IBGCLCAIBOL==NEINKOOBJIH.POMNMBGDDIN.LMPMNLLGOEC.LMMNKIOAGLC){
    var type=AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType("TreasuryAwards")).FirstOrDefault(t=>t!=null);
    var api=type?.GetMethod("RunAutomatic",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Static);
    if(api==null)Note("treasury-auto unavailable: compatible Treasury Awards not loaded");
    else Note("treasury-auto "+api.Invoke(null,new object[]{f}));
   }
  }catch(Exception e){Error(e);}
 }
 static void AssignDomestic(AFAGFHJONFJ c,MKMOJNAJKJE f){
  var pool=c.EOMOIINOEMB(1,false,true,false,false,false,false);if(pool==null)return;
  var available=new List<CLAGKDECFID>();foreach(var h in pool)if(SafeStateOfficer(c,h))available.Add(h);
  var counts=new Dictionary<OEBGIDGEIIG,int>();
  if(c.ABJNKABKKJM!=null)foreach(var h in c.ABJNKABKKJM){
   if(h?.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||h.LMPMNLLGOEC.CNOMNBCPJDI!=c.OJPECMNINNA||h.OGPEJIEHDAN?.HFBAJIGHOAF!=GCPCLLMKIMM.None )continue;
   var duty=h.BAPIJIJLLCA.HFBAJIGHOAF;counts.TryGetValue(duty,out int n);counts[duty]=n+1;
  }
  int Count(OEBGIDGEIIG d)=>counts.TryGetValue(d,out int n)?n:0;
  var rejected=new HashSet<OEBGIDGEIIG>();
  for(int i=0,attempts=0;i<2&&available.Count>0&&attempts<6;attempts++){
   OEBGIDGEIIG duty;var flow=ReadCivicFlow(c);
   if(!rejected.Contains(OEBGIDGEIIG.Security)&&(flow.Security<=0||c.CBDMPGOEMMK(f.IBGCLCAIBOL)<80&&Count(OEBGIDGEIIG.Security)==0))duty=OEBGIDGEIIG.Security;
   else if(!rejected.Contains(OEBGIDGEIIG.Patrol)&&(flow.Popular<=0||c.PCNOLBELFMF()<80&&Count(OEBGIDGEIIG.Patrol)==0))duty=OEBGIDGEIIG.Patrol;
   else if(!rejected.Contains(OEBGIDGEIIG.Military)&&StateBorder(c,f)&&Count(OEBGIDGEIIG.Military)==0)duty=OEBGIDGEIIG.Military;
   else if(!rejected.Contains(OEBGIDGEIIG.Manage)&&Count(OEBGIDGEIIG.Manage)<Math.Max(1,(available.Count+Count(OEBGIDGEIIG.Manage))/3))duty=OEBGIDGEIIG.Manage;
   else break;
   double Fit(CLAGKDECFID h)=>duty==OEBGIDGEIIG.Manage?2d*h.DAEKNKAKHDA()+h.GNCHNFHKBKE():duty==OEBGIDGEIIG.Military?2d*h.GGOIKBIJNCJ()+h.JLAMCIKNBON():h.JLAMCIKNBON()+h.GNCHNFHKBKE()+h.DHOMPHMNCJB();
   var hero=available.OrderByDescending(Fit).ThenBy(h=>h.OJPECMNINNA,StringComparer.Ordinal).First();available.Remove(hero);
   if(!SafeStateOfficer(c,hero))continue;
   var beforeDuty=hero.BAPIJIJLLCA?.HFBAJIGHOAF;var beforeJob=hero.OGPEJIEHDAN?.HFBAJIGHOAF;
   hero.KFNJDLCBLDN(duty,false);
   bool started=hero.MGLFFEANDOI&&hero.BAPIJIJLLCA?.HFBAJIGHOAF==duty;
   bool assigned=hero.BAPIJIJLLCA?.HFBAJIGHOAF==duty&&hero.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None;
   if(assigned){counts[duty]=Count(duty)+1;i++;}else rejected.Add(duty);
   Note($"domestic city={c.OJPECMNINNA} officer={hero.OJPECMNINNA} duty={duty} assigned={assigned} working={started} beforeDuty={beforeDuty} beforeJob={beforeJob} afterJob={hero.OGPEJIEHDAN?.HFBAJIGHOAF} home={hero.LMPMNLLGOEC.CNOMNBCPJDI} faction={f.IBGCLCAIBOL}");
  }
 }
 static void AssignTitles(MKMOJNAJKJE f){foreach(var step in TitleSteps(f)){} }
 static IEnumerable<string> TitleSteps(MKMOJNAJKJE f){
  if(UI_OfficialTitleUI.IJGEDOMNOBK&&UI_OfficialTitleUI.IJGEDOMNOBK.gameObject.activeInHierarchy)yield break;
  var defs=AEHAGJAKNEK.NHBPCOGKDML;if(defs==null)yield break;
  var ruler=AEHAGJAKNEK.KDHLLDHILCO(f.IBGCLCAIBOL);if(ruler?.LMPMNLLGOEC==null)yield break;
  var generation=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
  var snapshot=TimedAI(f,"title-snapshot",()=>CopyOfficerRegistry());
  int registryCount=AEHAGJAKNEK.IEOHLJLICOA.Count,probes=0,changed=0;var assigned=new HashSet<IntPtr>();
  var roster=new List<CLAGKDECFID>();var occupied=new HashSet<int>();
  foreach(var h in snapshot){
   if(++probes%16==0){yield return "title-roster";if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;}
   if(h?.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL)continue;
   if(h.LMPMNLLGOEC.JAAMIHKIMKP>=0)occupied.Add(h.LMPMNLLGOEC.JAAMIHKIMKP);
   if(!h.GMILGFFJOIL&&h.OJPECMNINNA!=NEINKOOBJIH.OPIKIHPNCIP&&h.OJPECMNINNA!=f.IBGCLCAIBOL)roster.Add(h);
  }
  for(int index=0;index<defs.Count&&changed<4;index++){
   yield return "title-candidate";
   if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f)||AEHAGJAKNEK.IEOHLJLICOA.Count!=registryCount)yield break;
   if(UI_OfficialTitleUI.IJGEDOMNOBK&&UI_OfficialTitleUI.IJGEDOMNOBK.gameObject.activeInHierarchy)yield break;
   var d=defs[index];if(d==null||occupied.Contains(index))continue;
   if(d.AHPHHHONKPI<=ruler.LMPMNLLGOEC.DOGMGCIADDE&&ruler.LMPMNLLGOEC.JAAMIHKIMKP>2)continue;
   CLAGKDECFID best=null;int skill=int.MinValue,merit=int.MinValue;
   foreach(var h in roster){
    if(++probes%16==0){yield return "title-rank";if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;}
    if(h.LMPMNLLGOEC.LMMNKIOAGLC!=f.IBGCLCAIBOL||h.GMILGFFJOIL||assigned.Contains(h.Pointer)||(h.LMPMNLLGOEC.JAAMIHKIMKP>=0&&h.LMPMNLLGOEC.JAAMIHKIMKP<=index)||!d.JJOCEDGFKOC(h))continue;
    int a=h.HPIBIDDIKNN(d.KOFJGOLGLAM),b=h.NOHAMLJFMPF();
    if(best==null||a>skill||a==skill&&(b>merit||b==merit&&StringComparer.Ordinal.Compare(h.OJPECMNINNA,best.OJPECMNINNA)<0)){best=h;skill=a;merit=b;}
   }
   if(best==null)continue;
   bool conflict=false;
   foreach(var h in snapshot){if(++probes%16==0){yield return "title-revalidate";if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;}
    if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&h.LMPMNLLGOEC.JAAMIHKIMKP==index){conflict=true;break;}}
   if(conflict){occupied.Add(index);continue;}
   if(AEHAGJAKNEK.IEOHLJLICOA.Count!=registryCount||!CurrentFaction(f)||best.LMPMNLLGOEC.LMMNKIOAGLC!=f.IBGCLCAIBOL||best.GMILGFFJOIL||!d.JJOCEDGFKOC(best))continue;
   if(UI_OfficialTitleUI.IJGEDOMNOBK&&UI_OfficialTitleUI.IJGEDOMNOBK.gameObject.activeInHierarchy)yield break;
   int old=best.LMPMNLLGOEC.JAAMIHKIMKP;if(old>=0&&old<=index)continue;
   best.LMPMNLLGOEC.JAAMIHKIMKP=index;occupied.Add(index);if(old>=0)occupied.Remove(old);assigned.Add(best.Pointer);changed++;
   Note($"official-title faction={f.IBGCLCAIBOL} officer={best.OJPECMNINNA} title={old}->{index} nativeEligible=true");yield return "title-commit";
  }
 }
}
