using System;using System.Collections.Generic;using System.Linq;using System.Reflection;
public static class Queue104Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;const string Root="local.heroesvow.governor/recruit/v1/F/";static int checks;static MKMOJNAJKJE f;static AFAGFHJONFJ city;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static object Fixture(string name,params object[] args)=>typeof(GovernorFeatureTests).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string message){checks++;if(!ok){Console.WriteLine("DEBUG head="+Head+" rows="+string.Join(";",StrategicSupport.RecruitmentQueueRows().Select(r=>string.Join("|",r))));Console.WriteLine("DEBUG jobs="+string.Join(";",city.Roster.Select(h=>h.OJPECMNINNA+":"+h.OGPEJIEHDAN.HFBAJIGHOAF+":"+h.OGPEJIEHDAN.LKEPGHJDHGG)));throw new Exception("Queue104: "+message);}}
 static void Setup(){CLAGKDECFID.RecruitCallback104=null;Fixture("Setup");f=(MKMOJNAJKJE)typeof(GovernorFeatureTests).GetField("faction",F).GetValue(null);city=(AFAGFHJONFJ)typeof(GovernorFeatureTests).GetField("city",F).GetValue(null);Call("ResetWork");}
 static CLAGKDECFID Add(string id,bool female,int talent)=> (CLAGKDECFID)Fixture("Candidate",id,female?KLMDJNIBCLD.Female:KLMDJNIBCLD.Male,talent,false);
 static string Item(int id)=>Root+"queue/item/"+id+"/";
 static int Q(string key)=>MLGOLGGHLMB.CBEGDDKANAO.TryGetValue(key,out int n)?n:0;
 static int Head=>Q(Root+"queue/active");
 static void Drain(){int bound=0;foreach(var phase in (IEnumerable<string>)Call("RecruitmentQueueSteps",f))if(++bound>100000)throw new Exception("queue failed to terminate");}
 static bool Targeted(CLAGKDECFID h)=>AEHAGJAKNEK.IEOHLJLICOA.Values.Any(a=>a.OGPEJIEHDAN.LKEPGHJDHGG==h.OJPECMNINNA&&a.OGPEJIEHDAN.HFBAJIGHOAF!=GCPCLLMKIMM.None);
 public static void Run(){
  Setup();var female=Add("woman",true,99);var male=Add("man",false,90);var third=Add("third",true,70);
  StrategicSupport.StartRecruitmentQueue(1,2);StrategicSupport.StartRecruitmentQueue(1,1);StrategicSupport.StartRecruitmentQueue(1,0);
  Check(StrategicSupport.RecruitmentQueueRows().Length==3&&Head==1,"three independent items append in registration order");
  StrategicSupport.SetRecruitmentQueuePaused(1,true);Drain();Check(!Targeted(female)&&!Targeted(male)&&Head==1,"paused head blocks every later item");
  StrategicSupport.SetRecruitmentQueuePaused(3,false);Drain();Check(Head==1&&!Targeted(third),"resuming later item does not jump order");
  StrategicSupport.SetRecruitmentQueuePaused(2,true);StrategicSupport.SetRecruitmentQueuePaused(1,false);Drain();Check(Targeted(female)&&!Targeted(male)&&Q(Root+"pending")==1,"only first female mission launches");
  Fixture("Join",female);Drain();Check(Q(Item(1)+"done")==1&&Q(Item(1)+"status")==4&&Head==2&&!Targeted(male),"completed first item advances to paused second without dispatch");
  Check(UI_NotifyUI.Calls==1,"first completion notifies exactly once");Drain();Check(UI_NotifyUI.Calls==1,"repeat scans never repeat completion alert");
  StrategicSupport.SetRecruitmentQueuePaused(2,false);Drain();Check(Targeted(male)&&!Targeted(third)&&Q(Item(1)+"done")==1,"second applies its own male filter and preserves first count");
  Fixture("Join",male);Drain();Check(Head==3&&Targeted(third)&&Q(Item(2)+"done")==1,"third starts only after second success");
  Fixture("Join",third);Drain();Check(Head==0&&UI_NotifyUI.Calls==3&&StrategicSupport.RecruitmentQueueRows().All(r=>r[6]=="4"),"all items complete sequentially with three independent notifications");
  StrategicSupport.CancelRecruitmentQueue(2);Check(StrategicSupport.RecruitmentQueueRows().Select(r=>r[0]).SequenceEqual(new[]{"1","3"}),"completed-card X removes only its stable ID");
  Setup();female=Add("cancel-woman",true,99);male=Add("next-man",false,80);StrategicSupport.StartRecruitmentQueue(1,2);Drain();StrategicSupport.StartRecruitmentQueue(1,1);
  StrategicSupport.CancelRecruitmentQueue(1);Drain();Check(Head==1&&Q(Item(1)+"status")==3&&!Targeted(male),"cancelled active item drains existing job without running next concurrently");
  Fixture("Join",female);Drain();Check(Head==2&&Targeted(male)&&Q(Root+"done")==0&&UI_NotifyUI.Calls==0,"cancelled item's late result never credits or notifies next item");
  Check(StrategicSupport.RecruitmentQueueRows().Length==1,"drained cancelled item disappears from list");
  Setup();female=Add("cancel-before-commit",true,99);male=Add("after-cancel",false,80);StrategicSupport.StartRecruitmentQueue(1,2);StrategicSupport.StartRecruitmentQueue(1,1);
  using(var scan=((IEnumerable<string>)Call("RecruitmentQueueSteps",f)).GetEnumerator()){Check(scan.MoveNext(),"pause at genuine pre-dispatch phase");StrategicSupport.CancelRecruitmentQueue(1);while(scan.MoveNext()){} }
  Drain();Check(!Targeted(female)&&Targeted(male)&&Head==2,"cancel during suspended scan invalidates old work before next item dispatch");
  Setup();StrategicSupport.StartRecruitmentQueue(30,2);StrategicSupport.StartRecruitmentQueue(20,1);StrategicSupport.StartRecruitmentQueue(10,0);StrategicSupport.SetRecruitmentQueuePaused(1,true);StrategicSupport.CancelRecruitmentQueue(2);
  Check(StrategicSupport.RecruitmentQueueRows().Select(r=>r[0]).SequenceEqual(new[]{"1","3"})&&Head==1,"remove middle waiting item without shifting saved identities");
  Call("ResetWork");Call("ResetManualOrders");Check(StrategicSupport.RecruitmentQueueRows()[0][6]=="2","runtime reset preserves per-item pause and order in save flags");
  Setup();female=Add("legacy/name",true,99);var actor=city.Roster.First();actor.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Recruit;actor.OGPEJIEHDAN.LKEPGHJDHGG=female.OJPECMNINNA;
  foreach(var p in new[]{("generation",7),("goal",1),("gender",2),("pending",1),("enabled",1)})MLGOLGGHLMB.CBEGDDKANAO[Root+p.Item1]=p.Item2;
  MLGOLGGHLMB.CBEGDDKANAO[Root+"mission/"+Uri.EscapeDataString(female.OJPECMNINNA)+"/"+Uri.EscapeDataString(actor.OJPECMNINNA)]=7;
  var migrated=StrategicSupport.RecruitmentQueueRows();Check(migrated.Length==1&&migrated[0][0]=="7"&&migrated[0][3]=="1"&&Head==7,"legacy in-flight reservation migrates without restarting mission");
  StrategicSupport.StartRecruitmentQueue(10,1);Drain();Check(Head==7&&Q(Root+"pending")==1&&StrategicSupport.RecruitmentQueueRows()[1][0]=="8","new item queues behind migrated in-flight work");
  Fixture("Join",female);Drain();Check(Q(Item(7)+"done")==1&&Head==8,"migrated completion advances once");
  Setup();female=Add("old-save",true,90);StrategicSupport.StartRecruitmentQueue(1,2);using(var scan=((IEnumerable<string>)Call("RecruitmentQueueSteps",f)).GetEnumerator()){Check(scan.MoveNext(),"save switch after planning starts");MLGOLGGHLMB.CBEGDDKANAO=new();while(scan.MoveNext()){} }
  Check(StrategicSupport.RecruitmentQueueRows().Length==0&&!Targeted(female),"save switch cannot leak queued entries or execute old selection");
  Setup();female=Add("already-targeted",true,99);StrategicSupport.StartRecruitmentQueue(1,2);bool injected=false;
  using(var scan=((IEnumerable<string>)Call("RecruitmentQueueSteps",f)).GetEnumerator())while(scan.MoveNext())if(!injected&&scan.Current=="recruit-envoy"){
   injected=true;actor=city.Roster.Last();actor.OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Recruit;actor.OGPEJIEHDAN.LKEPGHJDHGG=female.OJPECMNINNA;
  }
  Check(injected&&city.Roster.Count(h=>h.OGPEJIEHDAN.LKEPGHJDHGG==female.OJPECMNINNA)==1&&Q(Root+"pending")==0,"external mission started mid-scan is rechecked before duplicate dispatch");
  Setup();female=Add("instant-woman",true,99);male=Add("instant-man",false,80);StrategicSupport.StartRecruitmentQueue(1,2);StrategicSupport.StartRecruitmentQueue(1,1);
  var instantOrder=new List<string>();CLAGKDECFID.RecruitCallback104=(envoy,target)=>{instantOrder.Add(target);Fixture("Join",AEHAGJAKNEK.IEOHLJLICOA[target]);};
  Drain();Check(Head==0&&instantOrder.SequenceEqual(new[]{female.OJPECMNINNA,male.OJPECMNINNA})&&UI_NotifyUI.Calls==2,"synchronous successes finish each queue in order and notify once each");
  Setup();female=Add("failed-native",true,99);male=Add("later",false,80);StrategicSupport.StartRecruitmentQueue(1,2);StrategicSupport.StartRecruitmentQueue(1,1);
  CLAGKDECFID.RecruitCallback104=(envoy,target)=>throw new InvalidOperationException("native failure");bool failed=false;try{Drain();}catch(InvalidOperationException){failed=true;}
  StrategicSupport.CancelRecruitmentQueue(1);CLAGKDECFID.RecruitCallback104=null;Drain();Check(failed&&Head==2&&Targeted(male)&&Q(Root+"pending")==1,"failed native dispatch reservation settles before cancelled queue advances");
  Setup();female=Add("paused-flight",true,99);male=Add("after-paused",false,80);StrategicSupport.StartRecruitmentQueue(1,2);StrategicSupport.StartRecruitmentQueue(1,1);Drain();StrategicSupport.SetRecruitmentQueuePaused(1,true);
  Drain();Check(Head==1&&!Targeted(male),"paused in-flight queue still blocks following queue");Fixture("Join",female);Drain();Check(Head==2&&Targeted(male)&&UI_NotifyUI.Calls==1,"already-issued success settles paused queue and starts next only after completion");
  for(int seed=0;seed<20;seed++){
   Setup();var rng=new Random(10400+seed);var expected=new List<int>();
   for(int i=1;i<=30;i++){StrategicSupport.StartRecruitmentQueue(rng.Next(1,50),rng.Next(3));expected.Add(i);if(i==1)StrategicSupport.SetRecruitmentQueuePaused(1,true);}
   for(int i=0;i<20;i++){int id=rng.Next(2,31);if(expected.Contains(id)){StrategicSupport.CancelRecruitmentQueue(id);expected.Remove(id);}StrategicSupport.SetRecruitmentQueuePaused(rng.Next(2,31),rng.Next(2)==0);}
   Check(StrategicSupport.RecruitmentQueueRows().Select(r=>int.Parse(r[0])).SequenceEqual(expected)&&Head==1,"random middle cancellation/pause/resume preserves strict FIFO");
  }
  Call("ResetWork");Console.WriteLine("PASS Queue104 "+checks+" serial queue, save migration, pause/resume/cancel, result ownership and duplicate-target checks; native missions mocked.");
 }
}
