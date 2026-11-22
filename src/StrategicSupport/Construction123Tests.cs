using System;using System.Collections.Generic;using System.Linq;using System.Reflection;
public static class Construction123Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string n,params object[] args)=>typeof(StrategicSupport).GetMethod(n,F).Invoke(null,args);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Construction123: "+why);}
 static AFAGFHJONFJ Setup(){typeof(CityIntegrationTests).GetMethod("Setup",F).Invoke(null,null);var c=(AFAGFHJONFJ)typeof(CityIntegrationTests).GetField("c",F).GetValue(null);AEHAGJAKNEK.ACIGFKMDOKE.Clear();for(int i=0;i<40;i++)AEHAGJAKNEK.ACIGFKMDOKE["candidate"+i]=new(){OJPECMNINNA="candidate"+i,CNCPIILGDEF=100};return c;}
 static string Change(object a,object b,string method)=>(string)a.GetType().GetMethod(method).Invoke(a,new[]{b});
 static IEnumerable<bool> Slices(IEnumerable<bool> steps,double ms=10000,int max=256)=>(IEnumerable<bool>)Call("ConstructionSlices",steps,ms,max);
 public static void Run(){
  var c=Setup();var f=NEINKOOBJIH.COCMABJFOFH["F"];var d=c.FGBLEMGGOPO[0];
  var old=new OGABIJECHFG{OJPECMNINNA=123001,GEKKLMPLDHJ=new(){OJPECMNINNA="existing"},EMHNNCLBADE=0,BNMEOOJPOML=100};d.AIMLPNBOIIF.Add(old);
  var before=Call("ReadLayoutRevision",c,f);old.EMHNNCLBADE=100;var after=Call("ReadLayoutRevision",c,f);
  Check(Change(before,after,"StructureChange")==null,"existing construction completion preserves vacant geometry");
  Check(Change(before,after,"Change")=="construction-or-goal-progress","completion still invalidates saved long term remainder");
  old.EMHNNCLBADE=0;bool bought=false;using(var scan=((IEnumerable<bool>)Call("BuildSteps",c,f)).GetEnumerator()){
   Check(scan.MoveNext()&&!scan.Current,"production scan pauses before commit");old.EMHNNCLBADE=100;
   while(scan.MoveNext())bought|=scan.Current;
  }Check(bought&&d.AIMLPNBOIIF.Count==2,"completion during production scan no longer discards legal construction");
  c=Setup();f=NEINKOOBJIH.COCMABJFOFH["F"];var goal=new HIADFOLCFII{Complete=false};c.HFLAGGEFBOO.Add(goal);before=Call("ReadLayoutRevision",c,f);goal.Complete=true;after=Call("ReadLayoutRevision",c,f);
  Check(Change(before,after,"StructureChange")==null&&Change(before,after,"Change")!=null,"goal progress replans remainder without rejecting geometry");
  before=after;goal.CKIJPJFFFOA.Add(new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE="new-goal",KLCDGPDHENB=2});after=Call("ReadLayoutRevision",c,f);
  Check(Change(before,after,"StructureChange")=="city-goals","actual goal definition change still cancels stale planning");
  before=after;c.Scale++;after=Call("ReadLayoutRevision",c,f);Check(Change(before,after,"StructureChange")=="city-expansion-or-plots","expansion retains invalidation");
  c=Setup();f=NEINKOOBJIH.COCMABJFOFH["F"];d=c.FGBLEMGGOPO[0];bought=false;
  using(var scan=((IEnumerable<bool>)Call("BuildSteps",c,f)).GetEnumerator()){
   Check(scan.MoveNext(),"structural mutation fixture begins");d.AIMLPNBOIIF.Add(new(){OJPECMNINNA=123002,GEKKLMPLDHJ=new(){OJPECMNINNA="new-building"}});
   while(scan.MoveNext())bought|=scan.Current;
  }Check(!bought,"new construction still invalidates stale sites");
  int read=0,mutations=0;bool disposed=false;
  IEnumerable<bool> Work(){try{for(int i=0;i<1024;i++){read++;yield return false;}mutations++;yield return true;mutations++;yield return true;}finally{disposed=true;}}
  using(var batch=Slices(Work()).GetEnumerator()){
   Check(batch.MoveNext()&&!batch.Current&&read==256,"one slice coalesces tiny scans with hard step bound");
   int waits=1;while(batch.MoveNext()&&!batch.Current)waits++;
   Check(waits==4&&read==1024&&mutations==1,"1024 scan yields become four queue turns without losing any probe");
  }Check(disposed&&mutations==1,"commit returns immediately; disposal prevents next mutation");
  read=0;Check(Slices(Enumerable.Repeat(false,10),0).Count()==10,"expired CPU budget returns each step");
  // All candidates and the full layout are retained. Only their scheduling differs.
  var actions=new List<DevelopmentPolicy.Action>();for(int tile=0;tile<12;tile++)for(int kind=0;kind<6;kind++)actions.Add(new(){Id="type"+kind,Tiles=new[]{(long)tile},Limit=6,Money=kind*20,Cost=100});
  List<int> direct=null,batched=null;int raw=0,packed=0;
  foreach(var x in CityLayoutOptimizer.Solve(actions,new(),r=>direct=r))raw++;
  foreach(var x in Slices(CityLayoutOptimizer.Solve(actions,new(),r=>batched=r),.5))packed++;
  Check(direct.SequenceEqual(batched)&&batched.Count==12,"coalescing preserves whole layout and exact action order");
  Check(packed<raw/4,"candidate coverage preserved with fewer shared queue turns");
  Console.WriteLine($"PASS Construction123 {checks} production completion/goal/expansion checks and bounded scheduling; solver queue yields {raw}->{packed}; mocks, not live FPS.");
 }
}
