using System;
using System.Linq;
using System.Collections.Generic;
public class MockPlugin{public MockPlugin Log=>this;public void LogError(object e){Console.WriteLine(e);}}
public partial class UI_RewardUI{public static UI_RewardUI m_UI;public static bool m_isDragging;public Go gameObject=new();public static implicit operator bool(UI_RewardUI x)=>x!=null;}
public partial class Go{public bool activeInHierarchy=>activeSelf;}
public partial class CLAGKDECFID{public bool GMILGFFJOIL;public bool JPHIGPAMLEI=>!instant;public void JCAAECBOAMI(string id){instant=false;given.Add(id);}}
public partial class CMHNADEDJIE{
 static int serial;public IntPtr Pointer=new(++serial);
 public static CMHNADEDJIE OCELGGJBGEN(string id,int n,bool a,bool b,float f)=>new(){OJPECMNINNA=id,NGBNIPKFPOE=n};public void Recycle(){}
}
public partial class MKMOJNAJKJE{
 public Il2CppSystem.Collections.Generic.List<CMHNADEDJIE> CNBLOJCGNFD=new(),EHGOOJLIHGH=new(),OILNHBFCJII=new(),FCJHJPANFJN=new(),OHDEBNFBGIE=new(),PCGHDEOMPBO=new(),OOACPLDPNHM=new(),ONEHAJPKPMP=new(),CJDLOOOEGPI=new(),EFHPAEBIGJK=new(),HFCLLCIPNBB=new();
 public Dictionary<string,Il2CppSystem.Collections.Generic.List<CMHNADEDJIE>> GPGCLMPBCBP=new();
}
public static class AutomaticTests{
 public static void Run(){
  int assertions=0;void Check(bool b,string name){assertions++;if(!b)throw new Exception(name);}
  UI_HeroListInfoUI.PDPLMJHLKJF.Clear();UI_RewardUI.ABJNKABKKJM.Clear();MLGOLGGHLMB.CBEGDDKANAO.Clear();
  AEHAGJAKNEK.GHNDPFCGKDJ.Clear();AEHAGJAKNEK.EGFJHINFDCA.Clear();
  var f=new MKMOJNAJKJE();var city=new AFAGFHJONFJ();f.cities.Add(city);
  var p=new CLAGKDECFID{Pointer=100,OJPECMNINNA="player",LMPMNLLGOEC=new(){LMMNKIOAGLC="mine"}};NEINKOOBJIH.POMNMBGDDIN=p;city.list.Add(p);
  for(int i=0;i<10;i++)city.list.Add(new(){Pointer=i+1,stat=100-i,OJPECMNINNA="h"+i,LMPMNLLGOEC=new(){LMMNKIOAGLC="mine"}});
  var row=new CMHNADEDJIE{OJPECMNINNA="gift",NGBNIPKFPOE=1000,price=10};f.CNBLOJCGNFD.Add(row);
  TreasuryAwards.RecipientPercent.Value=20;string result=TreasuryAwards.RunAutomatic(f);
  Check(result.Contains("percent=20")&&result.Contains("recipients=2"),"live 20% read");
  Check(f.GPGCLMPBCBP.Count==2&&row.NGBNIPKFPOE==992,"2 recipients x (instant+3 monthly), conservation");
  Check(p.given.Count==0,"player excluded");
  TreasuryAwards.RecipientPercent.Value=80;result=TreasuryAwards.RunAutomatic(f);
  Check(result.Contains("percent=80")&&result.Contains("recipients=8"),"80% next invocation without reload");
  Check(f.GPGCLMPBCBP.Count==8&&row.NGBNIPKFPOE==968,"only newly eligible recipients get stock");
  TreasuryAwards.RecipientPercent.Value=10;result=TreasuryAwards.RunAutomatic(f);Check(result.Contains("recipients=1"),"lowered setting respected immediately");
  Check(row.NGBNIPKFPOE==968,"full queues and instant flags prevent duplicates");
  Check(f.GPGCLMPBCBP.Values.All(q=>q.Count==3),"monthly cap");
  UI_RewardUI.m_UI=new();UI_RewardUI.m_UI.gameObject.activeSelf=true;
  Check(TreasuryAwards.RunAutomatic(f).Contains("manual-reward-screen"),"manual editing protected");UI_RewardUI.m_UI=null;
  f.IBGCLCAIBOL="other";Check(TreasuryAwards.RunAutomatic(f).Contains("owner"),"foreign treasury rejected");f.IBGCLCAIBOL="mine";
  // Exhaust one-item stock at the instant boundary: never use a recycled row twice.
  f.CNBLOJCGNFD.Clear();f.CNBLOJCGNFD.Add(new(){OJPECMNINNA="last",NGBNIPKFPOE=1});
  foreach(var h in city.list)h.instant=true;f.GPGCLMPBCBP.Clear();MLGOLGGHLMB.CBEGDDKANAO.Clear();TreasuryAwards.RecipientPercent.Value=100;
  Check(TreasuryAwards.RunAutomatic(f).Contains("granted=1"),"last item exactly once");Check(f.CNBLOJCGNFD.Count==0,"empty source removed");
  Console.WriteLine($"PASS automatic treasury {assertions} checks: live option changes, conservation, monthly cap, owner/UI protection");
 }
}
