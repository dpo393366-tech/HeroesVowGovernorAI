using System;using System.IO;using System.IO.Compression;using System.Linq;using System.Text;using System.Text.Json;using System.Collections.Generic;using System.Net;using System.Net.Http;using System.Threading;using System.Threading.Tasks;using Governor.Updater;
class UpdaterTests {
 static int checks;static void Check(bool b,string why){checks++;if(!b)throw new Exception(why);}
 static void Reject(Action a,string why){bool failed=false;try{a();}catch{failed=true;}Check(failed,why);}
 static byte[] Pack(Dictionary<string,byte[]> contents,Action<Dictionary<string,byte[]>> change=null){
  var entries=new Dictionary<string,byte[]>();foreach(var p in contents)entries["GovernorAI/payload/"+p.Key+".dll"]=p.Value;
  entries["GovernorAI/manifest.json"]=JsonSerializer.SerializeToUtf8Bytes(new{id="governorai",version="1.5.44",files=contents.Select(p=>new{path=Engine.Relative(p.Key),sha256=Engine.Hash(p.Value)}).ToArray()});
  change?.Invoke(entries);using var m=new MemoryStream();using(var z=new ZipArchive(m,ZipArchiveMode.Create,true))foreach(var e in entries){using var s=z.CreateEntry(e.Key).Open();s.Write(e.Value);}return m.ToArray();
 }
 static byte[] Metadata(byte[] zip)=>JsonSerializer.SerializeToUtf8Bytes(new{draft=false,prerelease=false,assets=new[]{new{name=Engine.AssetName,browser_download_url=Engine.DownloadPrefix+"v1.5.44/"+Engine.AssetName,digest="sha256:"+Engine.Hash(zip)}}});
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.WriteLine(e);return 1;}}
 static void Run(string[] args){
  var old=Engine.Names.ToDictionary(n=>n,n=>File.ReadAllBytes(Path.Combine(args[0],n+".dll")));
  var newer=Engine.Names.ToDictionary(n=>n,n=>File.ReadAllBytes(Path.Combine(args[1],n+".dll")));
  var root=Path.GetFullPath(args[2]);Directory.CreateDirectory(root);var logs=new List<string>();
  void Reset(){foreach(var n in Engine.Names){var p=Engine.Destination(root,n);Directory.CreateDirectory(Path.GetDirectoryName(p));File.WriteAllBytes(p,old[n]);}
   var receipt=Path.Combine(Engine.StateRoot(root),"installed.json");if(File.Exists(receipt))File.Delete(receipt);logs.Clear();}
  bool Matches(Dictionary<string,byte[]> files)=>Engine.Names.All(n=>Engine.Hash(File.ReadAllBytes(Engine.Destination(root,n)))==Engine.Hash(files[n]));
  Check(Engine.PluginVersion(newer["StrategicSupport"],"StrategicSupport")==new Version("1.5.44"),"Actual BepInPlugin version parser");
  Check(Engine.PluginVersion(newer["TreasuryAwards"],"TreasuryAwards")==new Version("1.4.3"),"Treasury version parser");
  Check(Engine.PluginVersion(newer["HeroesVowSettings"],"HeroesVowSettings")==new Version("1.4.0"),"Settings version parser");
  Check(!AppDomain.CurrentDomain.GetAssemblies().Any(a=>Engine.Names.Contains(a.GetName().Name)),"Metadata inspection loaded a plugin");
  Reset();var config=Path.Combine(root,"BepInEx/user.cfg");File.WriteAllText(config,"user choice");var unrelated=Path.Combine(root,"BepInEx/plugins/other.dll");File.WriteAllText(unrelated,"unrelated");
  var zip=Pack(newer);var hash=Engine.Hash(zip);
  Reject(()=>Engine.Apply(root,zip,new string('0',64),logs.Add),"ZIP corruption allowed");Check(Matches(old),"Corruption wrote files");
  for(int fail=1;fail<=3;fail++){int point=fail;Reject(()=>Engine.Apply(root,zip,hash,logs.Add,i=>{if(i==point)throw new IOException("injected write failure");}),"Injected failure ignored");Check(Matches(old),"Transaction rollback failed at "+fail);Check(!File.Exists(Engine.JournalPath(root)),"Rollback journal remained");}
  Engine.Apply(root,zip,hash,logs.Add);Check(Matches(newer),"Real DLL update failed");Check(File.ReadAllText(config)=="user choice"&&File.ReadAllText(unrelated)=="unrelated","Unrelated files changed");
  Engine.Apply(root,zip,hash,logs.Add,_=>throw new Exception("No-op wrote a DLL"));Check(Matches(newer),"No-op changed files");
  var same=new Dictionary<string,byte[]>(newer);same["StrategicSupport"]=newer["StrategicSupport"].Concat(new byte[]{0x50}).ToArray();var sameZip=Pack(same);
  Engine.Apply(root,sameZip,Engine.Hash(sameZip),logs.Add);Check(Matches(same),"Same-version correction was not applied");
  var oldZip=Pack(old);Reject(()=>Engine.Apply(root,oldZip,Engine.Hash(oldZip),logs.Add),"Local newer core downgraded");Check(Matches(same),"Downgrade modified a file");
  Reset();File.Delete(Engine.Destination(root,"TreasuryAwards"));Reject(()=>Engine.Apply(root,zip,hash,logs.Add),"Removed module restored");Check(!File.Exists(Engine.Destination(root,"TreasuryAwards")),"Removed file resurrected");Reset();
  var bad=Pack(newer,e=>e["GovernorAI/payload/StrategicSupport.dll"]=new byte[]{1,2});Reject(()=>Engine.Apply(root,bad,Engine.Hash(bad),logs.Add),"Inner hash corruption allowed");Check(Matches(old),"Inner corruption wrote files");
  var wrong=new Dictionary<string,byte[]>(newer);wrong["StrategicSupport"]=newer["TreasuryAwards"];bad=Pack(wrong);Reject(()=>Engine.Apply(root,bad,Engine.Hash(bad),logs.Add),"Wrong DLL identity allowed");
  bad=Pack(newer,e=>e["../outside.dll"]=new byte[]{1});Reject(()=>Engine.Apply(root,bad,Engine.Hash(bad),logs.Add),"ZIP traversal allowed");
  Reject(()=>Engine.SafePath(root,Path.Combine(root,"..","outside")),"Destination escape allowed");
  var foreign=Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(Metadata(zip)).Replace(Engine.DownloadPrefix,"https://foreign.invalid/"));Reject(()=>Engine.ReadRelease(foreign),"Foreign release source allowed");
  Check(Engine.ReadRelease(Metadata(zip)).hash==hash,"Metadata SHA parse failed");
  // Simulate a process crash with the first DLL replaced and a durable journal.
  var dir=Path.Combine(root,"BepInEx/disabled-mods/GovernorAuto-test");Directory.CreateDirectory(dir);Directory.CreateDirectory(Engine.StateRoot(root));
  foreach(var n in Engine.Names)File.WriteAllBytes(Path.Combine(dir,n+".backup"),old[n]);
  var journal=new Journal{folder="GovernorAuto-test",modules=Engine.Names.Select(n=>new Module{name=n,sha256=Engine.Hash(old[n]),nextHash=Engine.Hash(newer[n])}).ToArray()};
  File.WriteAllText(Engine.JournalPath(root),JsonSerializer.Serialize(journal));File.WriteAllBytes(Engine.Destination(root,"StrategicSupport"),newer["StrategicSupport"]);
  Engine.Recover(root,logs.Add);Check(Matches(old),"Crash recovery failed");Check(!File.Exists(Engine.JournalPath(root)),"Crash journal not cleared");
  File.WriteAllText(Engine.JournalPath(root),JsonSerializer.Serialize(journal));File.WriteAllBytes(Path.Combine(dir,"TreasuryAwards.backup"),new byte[]{1});
  Reject(()=>Engine.Recover(root,logs.Add),"Bad recovery backup accepted");Check(Matches(old),"Partial restoration before validating all backups");File.WriteAllBytes(Path.Combine(dir,"TreasuryAwards.backup"),old["TreasuryAwards"]);Engine.Recover(root,logs.Add);
  // Full startup path through fake HTTP: first download, cached next start,
  // then changed asset at the SAME tag; no loaded module needed.
  var handler=new Handler{Zip=zip};using var client=new HttpClient(handler);
  Engine.Run(root,true,logs.Add,client);Check(Matches(newer)&&handler.Requests==2,"Startup download not applied");
  Engine.Run(root,true,logs.Add,client);Check(handler.Requests==3,"Unchanged update downloaded again");
  handler.Zip=sameZip;Engine.Run(root,true,logs.Add,client);Check(Matches(same)&&handler.Requests==5,"Same release hash change missed");
  handler.Fail=true;Engine.Run(root,true,logs.Add,client);Check(Matches(same),"HTTP failure changed files");
  var before=handler.Requests;Engine.Run(root,false,logs.Add,client);Check(before==handler.Requests,"Disabled updater used network");
  File.Delete(Engine.Destination(root,"TreasuryAwards"));Engine.Run(root,true,logs.Add,client);Check(before==handler.Requests,"Removed module still queried network");Reset();
  using(var timeoutClient=new HttpClient(new SlowHandler())){var timer=System.Diagnostics.Stopwatch.StartNew();Engine.Run(root,true,logs.Add,timeoutClient,TimeSpan.FromMilliseconds(40));Check(timer.Elapsed.TotalSeconds<3&&Matches(old),"Offline timeout delayed or modified installation");}
  using(var tiny=new HttpClient(new Handler{Zip=zip})){Reject(()=>Engine.Get(tiny,Engine.Api,4,CancellationToken.None).GetAwaiter().GetResult(),"Response size limit failed");}
  // Already-loaded plugins must prevent all file work and networking.
  System.Reflection.Assembly.Load(newer["HeroesVowSettings"]);before=handler.Requests;Engine.Run(root,true,logs.Add,client);Check(before==handler.Requests&&Matches(old),"Loaded-plugin guard failed");
  Check(logs.Any(s=>s.Contains("already loaded")),"Loaded guard not logged");
  Console.WriteLine("PASS "+checks+" updater transaction, real-DLL, lifecycle, same-tag, offline and scope checks; fixture="+root);
 }
 sealed class Handler:HttpMessageHandler {public int Requests;public bool Fail;public byte[] Zip;protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage r,CancellationToken c){Requests++;return Task.FromResult(new HttpResponseMessage(Fail?HttpStatusCode.ServiceUnavailable:HttpStatusCode.OK){Content=new ByteArrayContent(r.RequestUri.AbsoluteUri==Engine.Api?Metadata(Zip):Zip)});}}
 sealed class SlowHandler:HttpMessageHandler {protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage r,CancellationToken c){await Task.Delay(Timeout.Infinite,c);return new HttpResponseMessage(HttpStatusCode.OK);}}
}
