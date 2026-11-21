using System;
using System.Linq;
using System.Reflection;
public static class TreasuryPerfTests {
 static int checks;static MKMOJNAJKJE f;static AFAGFHJONFJ city;static CMHNADEDJIE stock;
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("Treasury R83: "+why);}
 static void Setup(int heroes=10,int items=1){
  UI_HeroListInfoUI.PDPLMJHLKJF.Clear();UI_RewardUI.ABJNKABKKJM.Clear();MLGOLGGHLMB.CBEGDDKANAO.Clear();AEHAGJAKNEK.GHNDPFCGKDJ.Clear();AEHAGJAKNEK.EGFJHINFDCA.Clear();UI_RewardUI.m_UI=null;UI_RewardUI.m_isDragging=false;
  f=new();city=new();f.cities.Add(city);NEINKOOBJIH.POMNMBGDDIN=new(){Pointer=10000,OJPECMNINNA="player",LMPMNLLGOEC=new(){LMMNKIOAGLC="mine"}};
  for(int i=0;i<heroes;i++)city.list.Add(new(){Pointer=i+1,stat=1000-i,OJPECMNINNA="h"+i,LMPMNLLGOEC=new(){LMMNKIOAGLC="mine"}});
  for(int i=0;i<items;i++)f.CNBLOJCGNFD.Add(new(){OJPECMNINNA="gift"+i,NGBNIPKFPOE=1000,price=1000-i});stock=f.CNBLOJCGNFD[0];TreasuryAwards.RecipientPercent.Value=100;
 }
 static bool Busy=>(bool)typeof(TreasuryAwards).GetField("busy",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
 public static void Run(){
  Setup(80,200);int scored=0,priced=0,turns=0;string result="";bool alwaysUnlocked=true;
  using(var it=TreasuryAwards.RunAutomaticSteps(f).GetEnumerator())while(it.MoveNext()){
   alwaysUnlocked&=!Busy;
   if(it.Current=="treasury-score")scored++;if(it.Current=="treasury-price")priced++;if(it.Current=="treasury-turn")turns++;
   if(it.Current.StartsWith("result:"))result=it.Current;
  }
  Check(alwaysUnlocked,"manual allocation lock never held over a yield");
  Check(scored==80&&priced==200&&turns>0,"large roster and all stock are incrementally inspected");
  Check(result.Contains("granted=32")&&city.list.Take(32).All(h=>h.given.Count==1)&&city.list.Skip(32).All(h=>h.given.Count==0),"first batch gives one per officer before next round");
  Check(stock.NGBNIPKFPOE==968,"sliced batch conserves stock");
  Setup();using(var it=TreasuryAwards.RunAutomaticSteps(f).GetEnumerator()){
   while(it.MoveNext()&&it.Current!="treasury-match"){}
   TreasuryAwards.RecipientPercent.Value=10;
   Check(it.MoveNext()&&it.Current.Contains("interrupted"),"live percentage edit aborts old candidate selection before debit");
   Check(stock.NGBNIPKFPOE==1000&&f.GPGCLMPBCBP.Count==0,"interruption spends nothing");
  }
  Check(TreasuryAwards.RunAutomatic(f).Contains("recipients=1")&&stock.NGBNIPKFPOE==996,"next run immediately uses new F12 percentage");
  Setup();using(var it=TreasuryAwards.RunAutomaticSteps(f).GetEnumerator()){
   while(it.MoveNext()&&it.Current!="treasury-match"){}
   UI_RewardUI.m_UI=new();UI_RewardUI.m_UI.gameObject.activeSelf=true;
   Check(!Busy,"manual screen can use its button while background iteration paused");
   Check(it.MoveNext()&&it.Current.Contains("interrupted"),"manual reward screen cancels background awards");
  }UI_RewardUI.m_UI=null;Check(stock.NGBNIPKFPOE==1000,"manual takeover causes no ghost debit");
  Setup();using(var it=TreasuryAwards.RunAutomaticSteps(f).GetEnumerator()){
   while(it.MoveNext()&&it.Current!="treasury-match"){}
   f.CNBLOJCGNFD.Clear();while(it.MoveNext()){}
  }Check(stock.NGBNIPKFPOE==1000&&city.list.All(h=>h.given.Count==0),"removed inventory row not consumed after yield");
  Setup();using(var it=TreasuryAwards.RunAutomaticSteps(f).GetEnumerator()){
   while(it.MoveNext()&&it.Current!="treasury-match"){}
   city.list[0].LMPMNLLGOEC.LMMNKIOAGLC="other";while(it.MoveNext()){}
  }Check(city.list[0].given.Count==0&&!f.GPGCLMPBCBP.ContainsKey("h0"),"officer changing faction during scan excluded");
  Setup();using(var it=TreasuryAwards.RunAutomaticSteps(f).GetEnumerator()){
   while(it.MoveNext()&&it.Current!="treasury-match"){}
   NEINKOOBJIH.POMNMBGDDIN.Pointer=20000;
   Check(it.MoveNext()&&it.Current.Contains("interrupted"),"world/player replacement aborts old iterator");
  }
  Setup();using(var it=TreasuryAwards.RunAutomaticSteps(f).GetEnumerator()){
   while(it.MoveNext()&&it.Current!="treasury-match"){}
  }Check(!Busy&&stock.NGBNIPKFPOE==1000,"disposing pending work leaves no lock or grant");
  Check(TreasuryAwards.RunAutomatic(f).Contains("granted=32"),"resumption after cancelled uncommitted turn is possible");
  Console.WriteLine($"PASS Treasury R83 sliced allocation checks={checks}; native effects mocked");
 }
}
