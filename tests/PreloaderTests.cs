using System;using System.IO;using System.Reflection;using BepInEx;using BepInEx.Preloader.Core.Patching;
class PreloaderTests {
 static int Main(string[] a){try{
  string root=Path.GetFullPath(a[0]);Paths.SetExecutablePath(Path.Combine(root,"ThreeKingdom.exe"),Path.Combine(root,"BepInEx"),Path.Combine(root,"BepInEx/interop"),false,new[]{AppContext.BaseDirectory});
  using var patcher=new AssemblyPatcher((bytes,path)=>Assembly.Load(bytes));patcher.AddPatchersFromDirectory(Path.Combine(root,"BepInEx/patchers"));
  if(patcher.PatcherContext.PatcherPlugins.Count!=1)throw new Exception("Actual preloader did not discover exactly one updater");
  patcher.PatchAndLoad();var log=Path.Combine(root,"BepInEx/GovernorAI-update.log");if(!File.Exists(log))throw new Exception("Initialize did not execute");
  var text=File.ReadAllText(log);Console.WriteLine(text);
  if(a.Length>1&&a[1]=="online"&&!text.Contains("ready for this launch"))throw new Exception("Online update did not complete in actual preloader");
  if(a.Length>1&&a[1]=="offline"&&!text.Contains("disabled"))throw new Exception("Disabled option ignored");
  Console.WriteLine("PASS actual BepInEx preloader discovery and Initialize; no game launched.");return 0;
 }catch(Exception e){Console.WriteLine(e);return 1;}}
}
