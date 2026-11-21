using System;using BepInEx.Configuration;using HarmonyLib;
public sealed partial class StrategicSupport {
 public static ConfigEntry<bool> ProtectFamilyPrivateTraining,ExcludeFamilyAutoRelocation;
 void ConfigureFamilyProtection(){
  ProtectFamilyPrivateTraining=Config.Bind("OfficerDeployment","ProtectFamilyPrivateTraining",true,"Exclude player-household officers tending private troops from automatic city relocation. Direct player commands remain available.");
  ExcludeFamilyAutoRelocation=Config.Bind("OfficerDeployment","ExcludeFamilyAutoRelocation",false,"Exclude all player-household officers from automatic city relocation, including idle officers. Independent of ruler/governor rank; F12 Officer Deployment.");
 }
 static bool FamilyRelocationProtected(CLAGKDECFID h){
  if(enabled?.Value!=true||h==null||string.IsNullOrEmpty(h.OJPECMNINNA)||NEINKOOBJIH.JMIADAFPGMC?.Contains(h.OJPECMNINNA)!=true)return false;
  if(ExcludeFamilyAutoRelocation?.Value==true)return true;
  if(ProtectFamilyPrivateTraining?.Value!=true)return false;
  // The private-training module can briefly clear the job while rebuilding
  // assignments. The actual local troop worker is still protected in that gap.
  if(MICGPAMNDKM.IJPKEMPIFIH==null||!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(h.LMPMNLLGOEC?.CNOMNBCPJDI??"",out var c)||c.CHCFKBDFGGD==null)return false;
  foreach(var a in c.CHCFKBDFGGD)if(a!=null&&a.OLMAJLLIJDM==h.OJPECMNINNA&&a.MGLFFEANDOI
   &&(a.GDHPLIFJCBN==PMNFKDJNLDJ.None||a.GDHPLIFJCBN==PMNFKDJNLDJ.Train||a.GDHPLIFJCBN==PMNFKDJNLDJ.Supplement)
   &&(a.MJNINDNBJHA&&a.FGHFOLKBHDL<EBNFFALGBPA.LLLBAAAEIBB||a.LBMKIPABHME&&(a.IKNJJGAHFGL<EBNFFALGBPA.JPNKCMJHIAP||a.EDIMNMBBCFM<EBNFFALGBPA.KMHPGABPIME)))return true;
  return false;
 }
 static bool FamilyCityOfficerProtected(AFAGFHJONFJ c,string id){
  if(NEINKOOBJIH.JMIADAFPGMC?.Contains(id)!=true||c.ABJNKABKKJM==null)return false;
  foreach(var h in c.ABJNKABKKJM)if(h?.OJPECMNINNA==id)return FamilyRelocationProtected(h);return false;
 }
 static void HookFamilyProtection(Harmony h){
  HarmonyMethod H(string n)=>new HarmonyMethod(typeof(StrategicSupport),n);
  h.Patch(AccessTools.Method(typeof(CLAGKDECFID),"IFFJPNEJCIN"),prefix:H(nameof(GuardFamilyRelocation)));
  // The native transfer UI issues IFFJPNEJCIN from RowPress synchronously.
  h.Patch(AccessTools.Method(typeof(UI_HeroListInfoUI),"RowPress"),prefix:H(nameof(BeginManualInput)),finalizer:H(nameof(EndManualInput)));
 }
 static bool GuardFamilyRelocation(CLAGKDECFID __instance,string __0){
  try {
   var h=__instance;
   if(ManualInput||h?.LMPMNLLGOEC==null||h.GMILGFFJOIL||h.OJPECMNINNA==NEINKOOBJIH.OPIKIHPNCIP||h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.CityMove||!FamilyRelocationProtected(h))return true;
   // Intercept ordinary same-faction transfers only. Capture, faction changes,
   // invalid destination handling and current travel completion stay native.
   var world=MICGPAMNDKM.IJPKEMPIFIH;string source=h.LMPMNLLGOEC.CNOMNBCPJDI,owner=h.LMPMNLLGOEC.LMMNKIOAGLC;
   if(world==null||string.IsNullOrEmpty(owner)||source==__0||!world.TryGetValue(source??"",out var from)||!world.TryGetValue(__0??"",out var to)||from.LMPMNLLGOEC?.LMMNKIOAGLC!=owner||to.LMPMNLLGOEC?.LMMNKIOAGLC!=owner)return true;
   if(Gate("family-relocation:"+h.OJPECMNINNA,30))Note($"family-relocation-protected officer={h.OJPECMNINNA} source={source} target={__0} reason={(ExcludeFamilyAutoRelocation.Value?"household-option":"private-training")} nativeCommandSkipped=true");
   return false;
  }catch(Exception e){Error(e);return true;}
 }
}
