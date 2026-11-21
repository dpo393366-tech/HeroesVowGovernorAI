using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

internal static class AsyncJournal
{
 static readonly object sync=new(),writer=new();static readonly Queue<string> lines=new();
 static Timer timer;static string path;static int dropped;
 internal static void Start(string file){lock(sync){if(timer!=null)return;path=file;timer=new Timer(_=>Flush(),null,1000,1000);}}
 internal static void Write(string message){
  string line=DateTime.Now.ToString("O")+" "+message;
  lock(sync){if(lines.Count>=8192){dropped++;return;}lines.Enqueue(line);}
 }
 internal static void Flush(bool wait=false){
  if(wait)Monitor.Enter(writer);else if(!Monitor.TryEnter(writer))return;
  try{
   var batch=new List<string>();int lost;
   lock(sync){if(path==null)return;while(lines.Count>0&&batch.Count<1024)batch.Add(lines.Dequeue());lost=dropped;dropped=0;}
   if(lost>0)batch.Add(DateTime.Now.ToString("O")+" journal-overflow dropped="+lost);
   if(batch.Count==0)return;
   if(File.Exists(path)&&new FileInfo(path).Length>8*1048576){
    for(int n=3;n>=1;n--){string old=n==1?path+".previous":path+".previous"+n;if(File.Exists(old))File.Move(old,path+".previous"+(n+1),true);}
    File.Move(path,path+".previous",true);
   }
   File.AppendAllLines(path,batch);
  }catch{/* Disk errors never block simulation or retry in a tight loop. */}
  finally{Monitor.Exit(writer);}
 }
}
