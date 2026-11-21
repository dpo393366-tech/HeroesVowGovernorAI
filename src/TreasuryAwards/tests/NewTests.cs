using System;using System.Linq;using System.Reflection;
static class NewTests{
 static int checks;
 static void Ok(bool b,string s){checks++;if(!b)throw new Exception(s);}
 static AwardRules.Gear G(double x,int kind=0,string soul="")=>new(){kind=kind,values=new[]{x,0d},souls=new[]{soul,"",""}};
 static void RunUI(UI_RewardUI u)=>typeof(TreasuryAwards).GetMethod("Run",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{u});
 static CLAGKDECFID H(int id,int f,int w)=>new(){Pointer=id,stat=100,force=f,wise=w,OJPECMNINNA=id.ToString(),LMPMNLLGOEC=new(){LMMNKIOAGLC="mine"}};
 static void Stock(string id,int price,KKDFJCCMGHK e=null,Drug d=null,int n=1){UI_RewardUI.m_listBagItem[0].Add(new(){OJPECMNINNA=id,price=price,NGBNIPKFPOE=n});if(e!=null)AEHAGJAKNEK.GHNDPFCGKDJ[id]=e;if(d!=null)AEHAGJAKNEK.EGFJHINFDCA[id]=d;}
 static string[] All(CLAGKDECFID h)=>h.given.Concat(h.queue).Where(x=>!string.IsNullOrEmpty(x)).ToArray();
 public static void Run(){
  Ok(AwardRules.RecipientCount(5,50)==3,"odd percentage ceiling");Ok(AwardRules.RecipientCount(10,1)==1,"1 percent");Ok(AwardRules.RecipientCount(10,100)==10,"100 percent");Ok(AwardRules.RecipientCount(0,50)==0,"empty roster");
  Ok(!AwardRules.Upgrade(G(5),G(5),true),"equal blocked");Ok(!AwardRules.Upgrade(G(5),G(6),false),"downgrade blocked");Ok(AwardRules.Upgrade(G(6),G(5),true),"strict improvement");Ok(!AwardRules.Upgrade(G(999,1),G(1,0),true),"wrong weapon kind despite stronger stats");
  var mixed=G(20);mixed.values[1]=-1;Ok(!AwardRules.Upgrade(mixed,G(1),false),"mixed downgrade preserved");
  Ok(!AwardRules.Upgrade(G(10),G(1,0,"soul"),false),"existing soul protected");Ok(AwardRules.Upgrade(G(10,0,"soul"),G(1,0,"soul"),false),"same soul improved");
  Ok(AwardRules.GrowthRole(2,0)==1&&AwardRules.GrowthRole(0,2)==2&&AwardRules.GrowthRole(2,2)==0,"native growth fields");
  Ok(AwardRules.PreferredGrowth(1,100,50)&&!AwardRules.PreferredGrowth(2,100,50)&&!AwardRules.PreferredGrowth(1,50,50),"role preference and tie");
  var p=H(1000,200,200);var warrior=H(10,150,20);var scholar=H(11,20,140);var u=new UI_RewardUI();
  TreasuryAwards.RecipientPercent.Value=100;MLGOLGGHLMB.CBEGDDKANAO.Clear();AEHAGJAKNEK.GHNDPFCGKDJ.Clear();AEHAGJAKNEK.EGFJHINFDCA.Clear();
  UI_HeroListInfoUI.PDPLMJHLKJF.Clear();UI_RewardUI.ABJNKABKKJM.Clear();UI_RewardUI.m_playerPower=new();UI_RewardUI.m_playerPower.cities.Add(new());UI_RewardUI.m_playerPower.cities[0].list.AddRange(new[]{p,warrior,scholar});
  NEINKOOBJIH.POMNMBGDDIN=p;UI_RewardUI.m_curHero=warrior;UI_RewardUI.m_rewards=new[]{"","","",""};UI_RewardUI.m_listBagItem[0].Clear();
  Stock("intGrowth",10000,d:new(){GDNLOKFFHDD=2});Stock("strGrowth",10,d:new(){IJGILOJIOAM=2});
  var old=new KKDFJCCMGHK{CIJIOJJFBEI=NGPAOHKHFPE.Weapon,EPKCKLBHPGG=2,LDDKGCHHEKI=100};AEHAGJAKNEK.GHNDPFCGKDJ["old"]=old;
  warrior.LMPMNLLGOEC.ONOGINEKIJH=new[]{"old"};
  AEHAGJAKNEK.GHNDPFCGKDJ["weak"]=new(){CIJIOJJFBEI=NGPAOHKHFPE.Weapon,EPKCKLBHPGG=2,LDDKGCHHEKI=10};scholar.LMPMNLLGOEC.ONOGINEKIJH=new[]{"weak"};
  Stock("equal",9000,new(){CIJIOJJFBEI=NGPAOHKHFPE.Weapon,EPKCKLBHPGG=2,LDDKGCHHEKI=100});
  Stock("wrongKind",8000,new(){CIJIOJJFBEI=NGPAOHKHFPE.Weapon,EPKCKLBHPGG=4,LDDKGCHHEKI=1000});
  Stock("title",100,n:8);RunUI(u);
  Ok(warrior.given.Single()=="strGrowth"&&scholar.given.Single()=="intGrowth","growth routed before expensive fallback across roster");
  Ok(!All(warrior).Contains("equal")&&All(scholar).Contains("equal"),"equal equipment skipped for leader, passed to next weaker officer");
  Ok(!All(warrior).Contains("wrongKind")&&!All(scholar).Contains("wrongKind")&&UI_RewardUI.m_listBagItem[0].Any(x=>x.OJPECMNINNA=="wrongKind"),"incompatible weapon retained");
  Ok(All(warrior).Length==4&&All(scholar).Length==4,"two-pass four-slot limit");
  Ok(All(p).Length==0,"protagonist excluded at 100 percent");
  Ok(MLGOLGGHLMB.CBEGDDKANAO["local.heroesvow.treasuryawards/v1/mine/cycle"]==2,"two-pass cycle completed once");
  Console.WriteLine("PASS "+checks+" new percentage/equipment/growth integration cases (mock UI).");
 }
}
