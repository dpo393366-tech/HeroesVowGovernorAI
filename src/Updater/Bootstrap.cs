using System;
using System.Diagnostics;
using System.IO;
using BepInEx;
using BepInEx.Preloader.Core.Patching;

[PatcherPluginInfo("local.heroesvow.governor.updater", "Governor AI Startup Update", "1.0.0")]
public sealed class GovernorStartupUpdate : BasePatcher {
 public override void Initialize(){
  try {
   var enabled=Config.Bind("Update","Enabled",true,"Check the official Governor AI release before plugins load. Offline startup keeps installed files.");
   var root=Paths.GameRootPath;
   Action<string> log=s=>{Log.LogInfo(s);try{File.AppendAllText(Path.Combine(Paths.BepInExRootPath,"GovernorAI-update.log"),DateTime.Now.ToString("s")+" "+s+Environment.NewLine);}catch{}};
   using var current=Process.GetCurrentProcess();
   foreach(var other in Process.GetProcessesByName(current.ProcessName))using(other){
    if(other.Id==current.Id)continue;
    try{if(!other.HasExited&&string.Equals(Path.GetDirectoryName(other.MainModule.FileName),root,StringComparison.OrdinalIgnoreCase)){log("Another game instance is active; update skipped.");return;}}
    catch{log("Cannot exclude another running game; update skipped.");return;}
   }
   Governor.Updater.Engine.Run(root,enabled.Value,log);
  }catch(Exception e){Log.LogWarning("Governor update unavailable; installed files retained: "+e.Message);}
 }
}
