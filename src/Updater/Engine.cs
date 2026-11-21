using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Governor.Updater;
internal sealed class Module { public string name {get;set;} public string sha256 {get;set;} public string nextHash {get;set;} }
internal sealed class Payload { public string version {get;set;} public Module[] modules {get;set;} }
internal sealed class Journal { public string folder {get;set;} public Module[] modules {get;set;} }
internal static class Engine {
 internal const string Api="https://api.github.com/repos/dpo393366-tech/HeroesVowGovernorAI/releases/latest";
 internal const string DownloadPrefix="https://github.com/dpo393366-tech/HeroesVowGovernorAI/releases/download/";
 internal const string AssetName="HeroesVow_GovernorAI_Standalone.zip";
 internal static readonly string[] Names={"StrategicSupport","TreasuryAwards","HeroesVowSettings"};
 internal static string Relative(string n)=>n=="HeroesVowSettings"?"BepInEx/plugins/HeroesVowSettings.dll":"BepInEx/plugins/"+n+"/"+n+".dll";
 internal static string Destination(string root,string n)=>Path.Combine(root,Relative(n));
 internal static string StateRoot(string root)=>Path.Combine(root,"BepInEx/cache/HeroesVowGovernorUpdate");
 internal static string JournalPath(string root)=>Path.Combine(StateRoot(root),"pending.json");
 internal static string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b));
 static string FileHash(string p)=>Hash(File.ReadAllBytes(p));
 static bool Same(string a,string b)=>string.Equals(a,b,StringComparison.OrdinalIgnoreCase);
 static bool Digest(string s)=>s!=null&&s.Length==64&&s.All(Uri.IsHexDigit);
 internal static void SafePath(string root,string path){
  root=Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar);path=Path.GetFullPath(path);
  if(!path.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase))throw new IOException("Path escaped game directory.");
  for(var p=path;p!=null&&p.Length>=root.Length;p=Path.GetDirectoryName(p))
   if((File.Exists(p)||Directory.Exists(p))&&(File.GetAttributes(p)&FileAttributes.ReparsePoint)!=0)throw new IOException("Linked update path preserved.");
 }
 internal static void ValidateModules(Module[] modules){
  if(modules==null||modules.Length!=3||modules.Any(m=>m==null||!Names.Contains(m.name)||!Digest(m.sha256))||modules.Select(m=>m.name).Distinct().Count()!=3)
   throw new InvalidDataException("Unexpected update module list.");
 }
 // Inspect metadata without loading a mod into the preloader process.
 internal static Version PluginVersion(byte[] bytes,string name){
  using var pe=new PEReader(new MemoryStream(bytes));var m=pe.GetMetadataReader();
  if(m.GetString(m.GetAssemblyDefinition().Name)!=name)throw new InvalidDataException("Module identity mismatch: "+name);
  foreach(var h in m.CustomAttributes){var a=m.GetCustomAttribute(h);if(a.Constructor.Kind!=HandleKind.MemberReference)continue;
   var ctor=m.GetMemberReference((MemberReferenceHandle)a.Constructor);if(ctor.Parent.Kind!=HandleKind.TypeReference)continue;
   var t=m.GetTypeReference((TypeReferenceHandle)ctor.Parent);
   if(m.GetString(t.Namespace)!="BepInEx"||m.GetString(t.Name)!="BepInPlugin")continue;
   var b=m.GetBlobReader(a.Value);if(b.ReadUInt16()!=1)continue;b.ReadSerializedString();b.ReadSerializedString();
   if(Version.TryParse(b.ReadSerializedString(),out var v))return v;
  }throw new InvalidDataException("Plugin version missing: "+name);
 }
 internal static bool InstalledMatches(string root,Payload p){
  try{ValidateModules(p.modules);return p.modules.All(m=>{var path=Destination(root,m.name);SafePath(root,path);return File.Exists(path)&&Same(FileHash(path),m.sha256);});}catch{return false;}
 }
 internal static async Task<byte[]> Get(HttpClient client,string url,int limit,CancellationToken token){
  using var response=await client.GetAsync(url,HttpCompletionOption.ResponseHeadersRead,token).ConfigureAwait(false);response.EnsureSuccessStatusCode();
  if(response.Content.Headers.ContentLength>limit)throw new InvalidDataException("Update response too large.");
  using var input=await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);using var output=new MemoryStream();var buffer=new byte[32768];int n;
  while((n=await input.ReadAsync(buffer,0,buffer.Length,token).ConfigureAwait(false))>0){if(output.Length+n>limit)throw new InvalidDataException("Update response exceeds size limit.");output.Write(buffer,0,n);}return output.ToArray();
 }
 internal static (string url,string hash) ReadRelease(byte[] data){
  using var doc=JsonDocument.Parse(data);var r=doc.RootElement;
  if(r.GetProperty("draft").GetBoolean()||r.GetProperty("prerelease").GetBoolean())throw new InvalidDataException("Not a stable release.");
  var assets=r.GetProperty("assets").EnumerateArray().Where(a=>a.GetProperty("name").GetString()==AssetName).ToArray();if(assets.Length!=1)throw new InvalidDataException("Update asset missing or ambiguous.");
  var url=assets[0].GetProperty("browser_download_url").GetString();var digest=assets[0].GetProperty("digest").GetString();
  if(url==null||!url.StartsWith(DownloadPrefix,StringComparison.Ordinal)||digest==null||!digest.StartsWith("sha256:",StringComparison.Ordinal)||!Digest(digest.Substring(7)))throw new InvalidDataException("Unexpected update source or hash.");
  return (url,digest.Substring(7));
 }
 internal static void Run(string root,bool enabled,Action<string> log,HttpClient transport=null,TimeSpan? timeout=null){
  using var gate=new Mutex(false,"Local\\HeroesVowConvenienceInstaller");bool held=false;
  try{
   try{held=gate.WaitOne(0);}catch(AbandonedMutexException){held=true;}if(!held){log("Installer/update active; skipped.");return;}
   if(AppDomain.CurrentDomain.GetAssemblies().Any(a=>Names.Contains(a.GetName().Name))){log("Plugins already loaded; update deferred until next startup.");return;}
   Recover(root,log);
   if(!enabled){log("Automatic update disabled.");return;}
   foreach(var n in Names){var dest=Destination(root,n);SafePath(root,dest);if(!File.Exists(dest)){log("A module was removed; automatic reinstall skipped.");return;}}
   // No old-version hash allowlist: standalone and all-in-one installations can update.
   using var owned=transport==null?new HttpClient{Timeout=Timeout.InfiniteTimeSpan}:null;var client=transport??owned;
   if(!client.DefaultRequestHeaders.UserAgent.Any())client.DefaultRequestHeaders.UserAgent.ParseAdd("HeroesVowGovernorAI-StartupUpdater/1.0");
   using var cancel=new CancellationTokenSource(timeout??TimeSpan.FromSeconds(15));using var metadataCancel=CancellationTokenSource.CreateLinkedTokenSource(cancel.Token);metadataCancel.CancelAfter(TimeSpan.FromSeconds(5));
   var remote=ReadRelease(Get(client,Api,1024*1024,metadataCancel.Token).GetAwaiter().GetResult());
   var receipt=Path.Combine(StateRoot(root),"installed.json");SafePath(root,receipt);
   if(File.Exists(receipt))try{using var d=JsonDocument.Parse(File.ReadAllBytes(receipt));var c=d.RootElement;
    if(Same(c.GetProperty("hash").GetString(),remote.hash)&&InstalledMatches(root,JsonSerializer.Deserialize<Payload>(c.GetProperty("payload").GetRawText()))){log("Already up to date.");return;}
   }catch{}
   var archive=Get(client,remote.url,20*1024*1024,cancel.Token).GetAwaiter().GetResult();cancel.Token.ThrowIfCancellationRequested();
   var payload=Apply(root,archive,remote.hash,log);
   Directory.CreateDirectory(StateRoot(root));File.WriteAllText(receipt+".new",JsonSerializer.Serialize(new{hash=remote.hash,payload}));File.Move(receipt+".new",receipt,true);
   log("Startup update verified: "+payload.version+"; ready for this launch.");
  }catch(Exception e){log("Update unavailable/rolled back; retaining installed files: "+e.Message);}
  finally{if(held)gate.ReleaseMutex();}
 }
 internal static Payload Apply(string root,byte[] archive,string expected,Action<string> log,Action<int> afterReplace=null){
  if(!Same(Hash(archive),expected))throw new InvalidDataException("Downloaded ZIP hash mismatch.");
  using var zip=new ZipArchive(new MemoryStream(archive),ZipArchiveMode.Read);
  if(zip.Entries.Count>64||zip.Entries.Any(e=>e.Length>12*1024*1024)||zip.Entries.Sum(e=>e.Length)>32*1024*1024||zip.Entries.Select(e=>e.FullName).Distinct(StringComparer.OrdinalIgnoreCase).Count()!=zip.Entries.Count)throw new InvalidDataException("Unexpected ZIP size or duplicate entries.");
  foreach(var e in zip.Entries)if(e.FullName.Contains('\\')||e.FullName.StartsWith('/')||e.FullName.Contains(':')||e.FullName.Split('/').Contains(".."))throw new InvalidDataException("Unsafe ZIP path.");
  var entry=zip.GetEntry("GovernorAI/manifest.json")??throw new InvalidDataException("Manifest missing.");
  using var stream=entry.Open();using var doc=JsonDocument.Parse(stream);var manifest=doc.RootElement;
  if(manifest.GetProperty("id").GetString()!="governorai")throw new InvalidDataException("Wrong package.");
  var files=manifest.GetProperty("files").EnumerateArray().ToArray();
  var payload=new Payload{version=manifest.GetProperty("version").GetString(),modules=Names.Select(n=>{
   var rows=files.Where(f=>f.GetProperty("path").GetString()==Relative(n)).ToArray();if(rows.Length!=1)throw new InvalidDataException("Missing or duplicate module: "+n);
   return new Module{name=n,sha256=rows[0].GetProperty("sha256").GetString()};}).ToArray()};ValidateModules(payload.modules);
  var content=new Dictionary<string,byte[]>();
  foreach(var m in payload.modules){
   var e=zip.GetEntry("GovernorAI/payload/"+m.name+".dll")??throw new InvalidDataException("Payload missing.");
   using var input=e.Open();using var output=new MemoryStream();input.CopyTo(output);var bytes=output.ToArray();if(!Same(Hash(bytes),m.sha256))throw new InvalidDataException("Module hash mismatch.");
   var target=Destination(root,m.name);SafePath(root,target);if(!File.Exists(target))throw new IOException("Removed module preserved.");
   var local=PluginVersion(File.ReadAllBytes(target),m.name);if(PluginVersion(bytes,m.name)<local)throw new InvalidDataException("Newer local module preserved: "+m.name);
   content.Add(m.name,bytes);
  }
  if(InstalledMatches(root,payload))return payload;
  var dir=Path.Combine(root,"BepInEx/disabled-mods","GovernorAuto-"+DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")+"-"+Guid.NewGuid().ToString("N"));SafePath(root,dir);SafePath(root,JournalPath(root));
  Directory.CreateDirectory(dir);var old=new List<Module>();
  foreach(var m in payload.modules){
   File.WriteAllBytes(Path.Combine(dir,m.name+".new"),content[m.name]);var dest=Destination(root,m.name);var hash=FileHash(dest);File.Copy(dest,Path.Combine(dir,m.name+".backup"));
   if(!Same(FileHash(Path.Combine(dir,m.name+".backup")),hash))throw new IOException("Backup verification failed.");old.Add(new Module{name=m.name,sha256=hash,nextHash=m.sha256});
  }
  Directory.CreateDirectory(StateRoot(root));var journal=JournalPath(root);File.WriteAllText(journal+".new",JsonSerializer.Serialize(new Journal{folder=Path.GetFileName(dir),modules=old.ToArray()}));File.Move(journal+".new",journal);
  try{
   int i=0;foreach(var m in payload.modules){var target=Destination(root,m.name);SafePath(root,target);
    if(!Same(FileHash(target),old.Single(x=>x.name==m.name).sha256))throw new IOException("Target changed during update.");
    File.Replace(Path.Combine(dir,m.name+".new"),target,null);afterReplace?.Invoke(++i);if(!Same(FileHash(target),m.sha256))throw new IOException("Installed file verification failed.");
   }
   File.Delete(journal);log("Updated governor/treasury/settings before plugin loading. Backup: "+dir);return payload;
  }catch{Recover(root,log);throw;}
 }
 internal static void Recover(string root,Action<string> log){
  var path=JournalPath(root);SafePath(root,path);if(!File.Exists(path))return;
  var j=JsonSerializer.Deserialize<Journal>(File.ReadAllBytes(path));ValidateModules(j.modules);
  if(j.folder==null||!j.folder.StartsWith("GovernorAuto-",StringComparison.Ordinal)||Path.GetFileName(j.folder)!=j.folder||j.modules.Any(m=>!Digest(m.nextHash)))throw new InvalidDataException("Invalid recovery record.");
  var dir=Path.Combine(root,"BepInEx/disabled-mods",j.folder);SafePath(root,dir);
  foreach(var m in j.modules){var backup=Path.Combine(dir,m.name+".backup");SafePath(root,backup);var dest=Destination(root,m.name);SafePath(root,dest);
   if(!Same(FileHash(backup),m.sha256))throw new IOException("Damaged recovery backup.");
   if(File.Exists(dest)&&!Same(FileHash(dest),m.sha256)&&!Same(FileHash(dest),m.nextHash))throw new IOException("Externally changed module preserved during recovery.");
  }
  foreach(var m in j.modules){var dest=Destination(root,m.name);if(File.Exists(dest)&&Same(FileHash(dest),m.sha256))continue;
   var tmp=Path.Combine(dir,m.name+".restore");File.Copy(Path.Combine(dir,m.name+".backup"),tmp,true);if(File.Exists(dest))File.Replace(tmp,dest,null);else File.Move(tmp,dest);
  }
  File.Delete(path);log("Recovered interrupted update from verified backups.");
 }
}
