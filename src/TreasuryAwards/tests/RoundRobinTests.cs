using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
public static class RoundRobinTests
{
 static int checks;static MKMOJNAJKJE f;static CLAGKDECFID[] heroes;static UI_RewardUI ui;
 const string Prefix="local.heroesvow.treasuryawards/v1/mine/";
 static void Check(bool b,string why){checks++;if(!b)throw new Exception("round robin: "+why);}
 static void Setup(int count,bool instant=false){
  MLGOLGGHLMB.CBEGDDKANAO.Clear();AEHAGJAKNEK.GHNDPFCGKDJ.Clear();AEHAGJAKNEK.EGFJHINFDCA.Clear();
  UI_HeroListInfoUI.PDPLMJHLKJF.Clear();UI_RewardUI.ABJNKABKKJM.Clear();UI_RewardUI.m_UI=null;UI_RewardUI.m_isDragging=false;
  f=new();var city=new AFAGFHJONFJ();f.cities.Add(city);UI_RewardUI.m_playerPower=f;
  NEINKOOBJIH.POMNMBGDDIN=new(){Pointer=9999,OJPECMNINNA="player",LMPMNLLGOEC=new(){LMMNKIOAGLC="mine"}};
  heroes=Enumerable.Range(0,count).Select(i=>new CLAGKDECFID{Pointer=i+1,stat=1000-i,OJPECMNINNA="h"+i,instant=instant,LMPMNLLGOEC=new(){LMMNKIOAGLC="mine"}}).ToArray();
  city.list.AddRange(heroes);TreasuryAwards.RecipientPercent.Value=100;
  UI_RewardUI.m_listBagItem[0]=f.CNBLOJCGNFD;UI_RewardUI.m_curHero=heroes[0];UI_RewardUI.m_rewards=new[]{"","","",""};UI_RewardUI.m_curPage=0;UI_RewardUI.m_viewType=0;ui=new();
 }
 static void Stock(string id,int n,int price=1)=>f.CNBLOJCGNFD.Add(new(){OJPECMNINNA=id,NGBNIPKFPOE=n,price=price});
 static void Manual()=>typeof(TreasuryAwards).GetMethod("Run",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,new object[]{ui});
 static int AutoCount(int i)=>heroes[i].given.Count+(f.GPGCLMPBCBP.TryGetValue(heroes[i].OJPECMNINNA,out var q)?q.Count:0);
 static string[] AutoQueue(int i)=>f.GPGCLMPBCBP.TryGetValue(heroes[i].OJPECMNINNA,out var q)?q.Select(r=>r.OJPECMNINNA).ToArray():Array.Empty<string>();
 public static void Run(){
  foreach(bool manual in new[]{false,true}){
   Setup(3);for(int i=0;i<6;i++)Stock("gift"+i,1,100-i);
   if(manual)Manual();else TreasuryAwards.RunAutomatic(f);
   for(int i=0;i<3;i++){var actual=manual?heroes[i].queue.Where(x=>x!="").ToArray():AutoQueue(i);Check(actual.SequenceEqual(new[]{"gift"+i,"gift"+(i+3)}),"price-ranked A1 B1 C1 A2 B2 C2, manual="+manual);}
   Setup(3);Stock("gift",4);if(manual)Manual();else TreasuryAwards.RunAutomatic(f);
   var counts=heroes.Select((h,i)=>manual?h.queue.Count(x=>x!=""):AutoCount(i)).ToArray();
   Check(counts.SequenceEqual(new[]{2,1,1}),"scarce four items spread 2/1/1, manual="+manual);
   Setup(3);Stock("first",1);if(manual)Manual();else TreasuryAwards.RunAutomatic(f);
   // Retain only persistent flags, exactly the data surviving a save/load.
   MLGOLGGHLMB.CBEGDDKANAO=new Dictionary<string,int>(MLGOLGGHLMB.CBEGDDKANAO);Stock("second",1);
   if(manual)Manual();else TreasuryAwards.RunAutomatic(f);
   Check((manual?heroes[1].queue[0]:AutoQueue(1).FirstOrDefault())=="second","refill resumes next officer from saved turn, manual="+manual);
   Check((manual?heroes[0].queue.Count(x=>x!=""):AutoCount(0))==1,"top officer does not get second before others, manual="+manual);
  }
  Setup(40);Stock("large",1000);TreasuryAwards.RunAutomatic(f);
  Check(Enumerable.Range(0,40).Select(AutoCount).SequenceEqual(Enumerable.Repeat(1,32).Concat(Enumerable.Repeat(0,8))),"32-item budget cannot fill first officers before last eight");
  TreasuryAwards.RunAutomatic(f);
  Check(Enumerable.Range(0,40).Select(AutoCount).SequenceEqual(Enumerable.Repeat(2,24).Concat(Enumerable.Repeat(1,16))),"next callback finishes remaining eight then next round");
  for(int i=0;i<4;i++)TreasuryAwards.RunAutomatic(f);
  Check(Enumerable.Range(0,40).All(i=>AutoCount(i)==3)&&f.CNBLOJCGNFD[0].NGBNIPKFPOE==880,"monthly cap and stock conservation across calls");
  Setup(3);heroes[0].force=150;heroes[0].wise=20;heroes[1].force=20;heroes[1].wise=150;
  Stock("strength",1,100);AEHAGJAKNEK.EGFJHINFDCA["strength"]=new(){IJGILOJIOAM=3};Stock("ordinary",2,50);TreasuryAwards.RunAutomatic(f);
  Check(Enumerable.Range(0,3).All(i=>AutoCount(i)==1),"preferred growth consumes a turn; fallback goes to others");
  Setup(3);Stock("gift",10);f.GPGCLMPBCBP["h0"]=new(){new(),new(),new()};TreasuryAwards.RunAutomatic(f);
  Check(AutoCount(0)==3&&AutoCount(1)==3&&AutoCount(2)==3,"full recipient skipped without stalling remaining roster");
  Setup(3);Stock("gift",9);MLGOLGGHLMB.CBEGDDKANAO[Prefix+"seen/h0"]=1;TreasuryAwards.RunAutomatic(f);
  Check(AutoCount(0)==0&&AutoCount(1)==3&&AutoCount(2)==3,"legacy completed recipient history preserved");
  Setup(3);Stock("first",1);Manual();Stock("second",1);TreasuryAwards.RunAutomatic(f);
  Check(AutoQueue(1).SequenceEqual(new[]{"second"}),"manual to automatic shares saved next turn");
  Setup(3);Stock("first",1);TreasuryAwards.RunAutomatic(f);Stock("second",1);Manual();
  Check(heroes[1].queue[0]=="second","automatic to manual shares saved next turn");
  Console.WriteLine("PASS round-robin fairness/continuation/native-slot checks="+checks+" (mocked native UI; not gameplay)");
 }
}
