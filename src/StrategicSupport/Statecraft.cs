using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;

public sealed partial class StrategicSupport
{
 static ConfigEntry<bool> statecraft,autoStrategies,autoDiplomacy,autoSchemes;
 static readonly Dictionary<string,double> statecraftDays=new();
 void ConfigureStatecraft(){
  statecraft=Config.Bind("Statecraft","Enabled",true,"Situational statecraft for factions with delegated AI cities, using native strategy effects and travelling missions.");
  autoStrategies=Config.Bind("Statecraft","Strategies",true,"Use understood beneficial national/city strategies for actual needs; preserve native unlocks, costs, upkeep and cooldowns.");
  autoDiplomacy=Config.Bind("Statecraft","Diplomacy",true,"Send native ceasefire, allied aid or shared-enemy alliance proposals. Acceptance is decided by the game. No automatic concessions or treaty breaking.");
  autoSchemes=Config.Bind("Statecraft","Schemes",true,"Use native rescue, hostile officer raids and espionage with shared mission limits. Preserve research, governors and defense duties.");
 }
 static bool DayReady(string key,double days,bool commit=false){
  double now=NEINKOOBJIH.MIPFHCKDJOK;
  if(double.IsNaN(now)||double.IsInfinity(now))return false;
  if(statecraftDays.TryGetValue(key,out double then)&&now>=then&&now-then<days)return false;
  if(commit)statecraftDays[key]=now;return true;
 }
 static List<AFAGFHJONFJ> StateCities(MKMOJNAJKJE f){
  var list=new List<AFAGFHJONFJ>();var native=f.PJMLPLDPEIM();
  if(native!=null)foreach(var c in native)if(Eligible(c,out var owner)&&owner.Pointer==f.Pointer)list.Add(c);
  return list;
 }
 static bool StateBorder(AFAGFHJONFJ c,MKMOJNAJKJE f){if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n?.LMPMNLLGOEC!=null&&War(f,n.LMPMNLLGOEC.LMMNKIOAGLC))return true;return false;}
 static bool IncomingHostile(AFAGFHJONFJ c,MKMOJNAJKJE f){
  if(c.GKLIEHCHDLD())return true;
  if(ICGAEMLACJG.HFJIECIBFKB!=null)foreach(var kv in ICGAEMLACJG.HFJIECIBFKB){var t=kv.Value;if(t&&t.m_targetCity==c.OJPECMNINNA&&War(f,t.m_belongPower))return true;}
  return false;
 }
 static StatecraftPolicy.Needs StateNeeds(List<AFAGFHJONFJ> cities,MKMOJNAJKJE f){var n=new StatecraftPolicy.Needs();foreach(var _ in StateNeedsSteps(cities,f,n)){}return n;}
 static IEnumerable<bool> StateNeedsSteps(List<AFAGFHJONFJ> cities,MKMOJNAJKJE f,StatecraftPolicy.Needs n){
  int probes=0;n.Money=f.DNOGPFHMOMO<ResearchReserve?1:0;n.Growth=cities.Count>0;
  foreach(var c in cities){
   yield return false;if(!CurrentFaction(f))yield break;if(c?.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL)continue;
   n.Food|=FoodEmergency(c);n.Front|=StateBorder(c,f)||IncomingHostile(c,f);
   n.Security=Math.Max(n.Security,Math.Clamp((60-c.CBDMPGOEMMK(f.IBGCLCAIBOL))/60d,0,1));
   n.Popular=Math.Max(n.Popular,Math.Clamp((60-c.PCNOLBELFMF())/60d,0,1));
   n.Troops+=c.LMPMNLLGOEC.KJPPGIOCDID?.Count??0;
   if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&a.FGHFOLKBHDL>0&&a.FGHFOLKBHDL<EBNFFALGBPA.LLLBAAAEIBB)n.Recovery=true;
   if(c.ABJNKABKKJM!=null)foreach(var h in c.ABJNKABKKJM.ToArray()){if(probes++%8==0){yield return false;if(!CurrentFaction(f))yield break;}if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&!h.GMILGFFJOIL){
    n.Officers++;n.Explore|=h.MGLFFEANDOI&&h.BAPIJIJLLCA?.HFBAJIGHOAF==OEBGIDGEIIG.Explore;
    n.Diplomacy|=h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.Diplomacy||h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.StartDiplomacy;
   }
   }
   yield return false;if(!CurrentFaction(f))yield break;
   foreach(var x in ReadGoals(c).Needs){double deficit=Math.Min(1,x.Value.Missing/Math.Max(1,x.Value.Target));
    if(x.Key=="money"||x.Key=="commerce")n.Money=Math.Max(n.Money,deficit);
    if(x.Key=="security")n.Security=Math.Max(n.Security,deficit);
    if(x.Key=="popular")n.Popular=Math.Max(n.Popular,deficit);
   }
   if(c.FGBLEMGGOPO!=null)foreach(var d in c.FGBLEMGGOPO.ToArray())if(d?.AIMLPNBOIIF!=null)foreach(var b in d.AIMLPNBOIIF.ToArray()){if(probes++%8==0){yield return false;if(!CurrentFaction(f))yield break;}if(b!=null&&!Completed(b))n.Building=true;}
  }
  if(f.OINKOPJJGJH!=null)foreach(var r in f.OINKOPJJGJH)if((r.Value?.ABJNKABKKJM?.Count??0)>0)n.Research=true;
  n.NeedOfficers=n.Officers<cities.Count*8+n.Troops;
 }
 static bool StrategyDominates(JFLCKHMFGPJ next,JFLCKHMFGPJ old){
  if(next==null||old==null||next.OJPECMNINNA==old.OJPECMNINNA||next.PMPLECHDMNM!=old.PMPLECHDMNM||next.IABANLBEFMG!=0||old.IABANLBEFMG!=0||next.NDAPJMMIMKH!=0||old.NDAPJMMIMKH!=0||next.EHPDLGLPOBD!=0||old.EHPDLGLPOBD!=0||next.MDPGDMKHDAG==null||old.MDPGDMKHDAG==null)return false;
  var n=new Dictionary<string,long>();var o=new Dictionary<string,long>();
  bool Read(JFLCKHMFGPJ s,Dictionary<string,long> values){foreach(var e in s.MDPGDMKHDAG){
   if(e==null||(int)e.HFBAJIGHOAF<0||(int)e.HFBAJIGHOAF>47||e.ODEKPKAIGPD<0||e.ODEKPKAIGPD>1||e.LPIEHGBCPLP<0||e.LPIEHGBCPLP>1||StatecraftPolicy.Effect((int)e.HFBAJIGHOAF,e.HLNCKLGGECA,new())<0)return false;
   string key=(int)e.HFBAJIGHOAF+":"+e.ODEKPKAIGPD+":"+e.LPIEHGBCPLP;int kind=(int)e.HFBAJIGHOAF;long amount=e.HLNCKLGGECA;
   if(kind==13||kind==17||kind==18||kind==19||kind==22||kind==27||kind==47)amount= -amount;
   values.TryGetValue(key,out long prior);values[key]=prior+amount;
  }return values.Count>0;}
  return Read(next,n)&&Read(old,o)&&o.All(e=>n.TryGetValue(e.Key,out long value)&&value>=e.Value)&&n.Any(e=>!o.TryGetValue(e.Key,out long value)||e.Value>value);
 }
 static int StrategyReplacement(MKMOJNAJKJE f,JFLCKHMFGPJ s){
  int found=-1;for(int i=0;i<f.MEAMPLFJPNH.Count;i++){var a=f.MEAMPLFJPNH[i];if(a?.BHIBPLKOFKH?.PMPLECHDMNM!=s.PMPLECHDMNM)continue;
   if(found>=0||!StrategyDominates(s,a.BHIBPLKOFKH))return -1;found=i;
  }return found;
 }
 static string StrategyBlock(MKMOJNAJKJE f,JFLCKHMFGPJ s){
  if(f.ADJFGBAIACI!=null&&f.ADJFGBAIACI.TryGetValue(s.OJPECMNINNA,out int cd)&&cd>0)return "native-cooldown:"+cd;
  if(f.MEAMPLFJPNH!=null)foreach(var a in f.MEAMPLFJPNH)if(a?.BHIBPLKOFKH!=null){
   if(a.BHIBPLKOFKH.OJPECMNINNA==s.OJPECMNINNA)return "already-active";
   if(a.BHIBPLKOFKH.PMPLECHDMNM==s.PMPLECHDMNM&&StrategyReplacement(f,s)<0)return "active-family:"+a.BHIBPLKOFKH.OJPECMNINNA;
  }
  if(f.MEAMPLFJPNH!=null&&StrategyReplacement(f,s)>=0)return null;
  return !DayReady("strategy:"+f.IBGCLCAIBOL+":"+s.PMPLECHDMNM,Math.Max(7,s.EHPDLGLPOBD))?"family-retry-interval":null;
 }
 static bool ActiveStrategy(MKMOJNAJKJE f,JFLCKHMFGPJ s)=>StrategyBlock(f,s)!=null;
 static long StrategyUpkeep(MKMOJNAJKJE f,JFLCKHMFGPJ s,long upkeep){int old=StrategyReplacement(f,s);return old<0?upkeep:Math.Max(0,upkeep-Math.Max(0,f.MEAMPLFJPNH[old].BHIBPLKOFKH.CCBIHJLDGCG));}
 static bool TryStateStrategy(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities){bool done=false;foreach(bool step in StateStrategySteps(f,cities))done|=step;return done;}
 static string StrategyTarget(JFLCKHMFGPJ s,MKMOJNAJKJE f,List<AFAGFHJONFJ> cities){
  bool targeted=false,friendly=false,hostile=false;
  foreach(var e in s.MDPGDMKHDAG)if(e!=null&&(int)e.HFBAJIGHOAF>=100){targeted=true;if((int)e.HFBAJIGHOAF==108)friendly=true;else hostile=true;}
  if(!targeted)return "";if(friendly&&hostile)return null;
  string best=null;double score=double.NegativeInfinity;
  foreach(var kv in f.AENJACOFBJK){
   if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(kv.Key,out var other)||other==null||kv.Key==f.IBGCLCAIBOL)continue;
   if(friendly){if(kv.Value?.EBIOPDHEBOF!=BHBNKONCOJD.Ally)continue;}else if(!War(f,kv.Key)||!cities.Any(c=>Eligible(c,out _)&&AttackPermission(c,f,c.LMPMNLLGOEC.IPJHOOJBCKI,kv.Key)))continue;
   double value=0;foreach(var c in cities)if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n?.LMPMNLLGOEC?.LMMNKIOAGLC==kv.Key)value+=100;
   foreach(var t in AttackTeams())if(t&&t.m_belongPower==kv.Key&&cities.Any(c=>c.OJPECMNINNA==t.m_targetCity))value+=1000+Math.Max(0,t.GetSoldierNum(false))/1000d;
   if(value>score){best=kv.Key;score=value;}
  }return best;
 }
 static IEnumerable<bool> StateStrategySteps(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities)=>StateStrategyBatchSteps(f,cities,false);
 static IEnumerable<bool> StateStrategyBatchSteps(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities,bool batch){
  if(!autoStrategies.Value||f.MEAMPLFJPNH==null||f.ADJFGBAIACI==null)yield break;
  var needs=new StatecraftPolicy.Needs();foreach(var phase in StateNeedsSteps(cities,f,needs)){yield return phase;if(!CurrentFaction(f))yield break;}var options=TimedAI(f,"strategy-candidates",()=>f.NIDCNBBLPGB(0));if(options==null)yield break;
  long upkeep=0;foreach(var a in f.MEAMPLFJPNH)if(a?.BHIBPLKOFKH!=null)upkeep+=Math.Max(0,a.BHIBPLKOFKH.CCBIHJLDGCG);
  int production=TimedAI(f,"strategy-production",()=>f.GFHFKMKLGFB(false));JFLCKHMFGPJ best=null;double value=0;string bestTarget="";var ranked=new List<(JFLCKHMFGPJ Strategy,double Score,string Target)>();
  var rejected=new Dictionary<string,int>();var details=new List<string>();bool diagnostic=Gate("strategy-audit:"+f.IBGCLCAIBOL,30);int probes=0;
  var snapshot=new List<JFLCKHMFGPJ>();foreach(var option in options)snapshot.Add(option);
  string candidate="none";
  void Reject(string why){rejected.TryGetValue(why,out int n);rejected[why]=n+1;if(diagnostic&&details.Count<80)details.Add(candidate+":"+why);}
  foreach(var s in snapshot){
   yield return false;if(!CurrentFaction(f))yield break;probes++;candidate=s==null?"null":s.OJPECMNINNA+"/"+s.HIENOINGBCN?.INIAGDLNOAP();
   if(s==null){Reject("null");continue;}
   if(s.LGGBONFIIJN){Reject("native-ai-ban");continue;}
   string block=StrategyBlock(f,s);if(block!=null){Reject(block);continue;}
   if(s.MDPGDMKHDAG==null||s.MDPGDMKHDAG.Count==0){Reject("unsupported-effect");continue;}
   if(!StatecraftPolicy.Affordable(f.PEGLDBENJMF,s.AFPGDLJKEDA,s.CCBIHJLDGCG,StrategyUpkeep(f,s,upkeep),production)){Reject("points-or-upkeep");continue;}
   double score=ScoreStrategy(s,needs,out string effectReason);
   if(score<=0){Reject(effectReason??"no-current-benefit");continue;}
   if(!StrategyCivicSafe(s,cities)){Reject("normal-city-growth-would-turn-negative");continue;}
   string target=StrategyTarget(s,f,cities);if(target==null){Reject("no-valid-strategy-target");continue;}
   score/=1+s.AFPGDLJKEDA/100d+s.CCBIHJLDGCG/25d;
   ranked.Add((s,score,target));if(score>value){best=s;value=score;bestTarget=target;}
  }
  if(diagnostic)Note($"strategy-audit faction={f.IBGCLCAIBOL} points={f.PEGLDBENJMF} upkeep={upkeep}/{production} candidates={options.Count} selected={best?.OJPECMNINNA??"none"} rejected=[{string.Join(";",details)}]");
  if(best==null){Note($"statecraft-no-strategy faction={f.IBGCLCAIBOL} points={f.PEGLDBENJMF} candidates={options.Count} upkeep={upkeep}/{production} food={needs.Food} front={needs.Front} research={needs.Research} building={needs.Building} moneyNeed={needs.Money:F2} blocked=[{string.Join(",",rejected.Select(k=>k.Key+"="+k.Value))}]");yield break;}
  // Rank once; commit independently. A stale/failed best candidate must not
  // stop all other beneficial strategies. No effect/cost/cooldown is bypassed.
  ranked.Sort((a,b)=>b.Score.CompareTo(a.Score));int committed=0;
  foreach(var choice in ranked){
   best=choice.Strategy;value=choice.Score;bestTarget=choice.Target;
   yield return false;if(!CurrentFaction(f)||!autoStrategies.Value)yield break;
   string abort=null;
   var current=TimedAI(f,"strategy-revalidate",()=>f.NIDCNBBLPGB(0));bool eligible=false;
   if(current!=null)foreach(var row in current)if(row?.Pointer==best.Pointer){eligible=true;break;}
   if(!eligible||best.LGGBONFIIJN)abort="native-eligibility-changed";
   else abort=StrategyBlock(f,best);
   // Pure benefits have a positive baseline even without a current deficit.
   // Refresh costly census only for actual tradeoffs, not for every buff.
   var freshNeeds=needs;bool tradeoff=false;
   foreach(var effect in best.MDPGDMKHDAG)if(effect!=null&&(int)effect.HFBAJIGHOAF<100&&StatecraftPolicy.Effect((int)effect.HFBAJIGHOAF,effect.HLNCKLGGECA,needs)<0)tradeoff=true;
   if(abort==null&&tradeoff){freshNeeds=new();foreach(var phase in StateNeedsSteps(cities,f,freshNeeds)){yield return phase;if(!CurrentFaction(f))yield break;}}
   if(abort==null&&tradeoff){
    if(!autoStrategies.Value)yield break;
    current=TimedAI(f,"strategy-revalidate",()=>f.NIDCNBBLPGB(0));eligible=false;
    if(current!=null)foreach(var row in current)if(row?.Pointer==best.Pointer){eligible=true;break;}
    if(!eligible||best.LGGBONFIIJN)abort="native-eligibility-changed";else abort=StrategyBlock(f,best);
   }
   if(abort==null&&ScoreStrategy(best,freshNeeds,out _)<=0)abort="benefit-changed";
   InvalidatePlanningReads();
   if(abort==null&&!StrategyCivicSafe(best,cities))abort="civic-safety-changed";
   if(abort==null&&StrategyTarget(best,f,cities)!=bestTarget)abort="target-changed";
   upkeep=0;foreach(var active in f.MEAMPLFJPNH)if(active?.BHIBPLKOFKH!=null)upkeep+=Math.Max(0,active.BHIBPLKOFKH.CCBIHJLDGCG);
   production=TimedAI(f,"strategy-production",()=>f.GFHFKMKLGFB(false));
   if(abort==null&&!StatecraftPolicy.Affordable(f.PEGLDBENJMF,best.AFPGDLJKEDA,best.CCBIHJLDGCG,StrategyUpkeep(f,best,upkeep),production))abort="budget-changed";
   if(abort!=null){if(diagnostic)Note($"strategy-commit-block faction={f.IBGCLCAIBOL} id={best.OJPECMNINNA} reason={abort}");continue;}
   int before=f.PEGLDBENJMF;DLAFJLCLCBG pending=null;
   if(best.IABANLBEFMG==0||best.NDAPJMMIMKH>0)pending=DLAFJLCLCBG.OCELGGJBGEN(best,false);
   if((best.IABANLBEFMG==0||best.NDAPJMMIMKH>0)&&pending==null){Note($"strategy-commit-block faction={f.IBGCLCAIBOL} id={best.OJPECMNINNA} reason=native-pending-allocation");continue;}
   if(pending!=null){pending.ACPKBHHPKBH=bestTarget;pending.OLMAJLLIJDM=f.IBGCLCAIBOL;}
   DayReady("strategy:"+f.IBGCLCAIBOL+":"+best.PMPLECHDMNM,0,true);f.PEGLDBENJMF-=best.AFPGDLJKEDA;
   if(pending!=null){int replace=StrategyReplacement(f,best);string old=replace<0?null:f.MEAMPLFJPNH[replace].BHIBPLKOFKH.OJPECMNINNA;
    f.MEAMPLFJPNH.Add(pending);if(replace>=0){f.CHHLIPEMKCN(replace);Note($"strategy-upgrade faction={f.IBGCLCAIBOL} old={old} new={best.OJPECMNINNA} nativeStop=true");}
   }else {best.ALHDMCHMJNL(f.IBGCLCAIBOL,bestTarget,f.IBGCLCAIBOL);if(best.EHPDLGLPOBD>0)f.ADJFGBAIACI[best.OJPECMNINNA]=best.EHPDLGLPOBD;}
   InvalidatePlanningReads();committed++;
   Note($"statecraft-strategy faction={f.IBGCLCAIBOL} id={best.OJPECMNINNA} points={before}->{f.PEGLDBENJMF} upkeep={upkeep}+{best.CCBIHJLDGCG}/{production} score={value:F1} nativeEffect=true batch={batch}");yield return true;
   if(!batch||committed>=32)yield break;
  }
 }

 static bool MissionJob(GCPCLLMKIMM job)=>job==GCPCLLMKIMM.Diplomacy||job==GCPCLLMKIMM.StartDiplomacy||job==GCPCLLMKIMM.EndDiplomacy||job==GCPCLLMKIMM.Scheme||job==GCPCLLMKIMM.StartScheme||job==GCPCLLMKIMM.EndScheme;
 static List<CLAGKDECFID> ActiveMissions(MKMOJNAJKJE f){
  var result=new List<CLAGKDECFID>();
  // Travelling officers can disappear from city rosters. Count the native
  // global registry, including after loading a save, not just local residents.
  if(AEHAGJAKNEK.IEOHLJLICOA!=null)foreach(var kv in AEHAGJAKNEK.IEOHLJLICOA){var h=kv.Value;
   if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&h.OGPEJIEHDAN!=null&&MissionJob(h.OGPEJIEHDAN.HFBAJIGHOAF))result.Add(h);
  }return result;
 }
 static bool SafeStateOfficer(AFAGFHJONFJ c,CLAGKDECFID h)=>h?.LMPMNLLGOEC!=null&&!ManualProtected(h)&&!h.GMILGFFJOIL&&(h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None||h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.PrivateMilitary)
  &&h.OJPECMNINNA!=NEINKOOBJIH.OPIKIHPNCIP&&h.OJPECMNINNA!=c.LMPMNLLGOEC.LMMNKIOAGLC&&h.OJPECMNINNA!=c.LMPMNLLGOEC.IPJHOOJBCKI&&h.OJPECMNINNA!=c.LMPMNLLGOEC.BKENJPNKMGI
  &&h.LMPMNLLGOEC.CNOMNBCPJDI==c.OJPECMNINNA&&h.LMPMNLLGOEC.LMMNKIOAGLC==c.LMPMNLLGOEC.LMMNKIOAGLC
  &&(h.BAPIJIJLLCA==null||h.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.None||h.BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Explore);
 static List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> StateOfficers(List<AFAGFHJONFJ> cities,MKMOJNAJKJE f){
  var result=new List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>();foreach(var step in StateOfficerSteps(cities,f,result,new())){}return result;
 }
 static IEnumerable<string> StateOfficerSteps(List<AFAGFHJONFJ> cities,MKMOJNAJKJE f,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> result,List<CLAGKDECFID> missions){
  int probes=0;
  foreach(var h in OfficerRegistrySnapshot()){
   if(probes++%8==0)yield return "mission-registry";
   if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&h.OGPEJIEHDAN!=null&&MissionJob(h.OGPEJIEHDAN.HFBAJIGHOAF))missions.Add(h);
  }
  if(missions.Count>=2)yield break;
  foreach(var c in cities){yield return "mission-city";
   int home=0,away=missions.Count(h=>h.OGPEJIEHDAN.MIGIFJPINLB==c.OJPECMNINNA),researchers=0;
   if(c.ABJNKABKKJM!=null)foreach(var h in c.ABJNKABKKJM.ToArray()){yield return "mission-home";if(h?.LMPMNLLGOEC?.LMMNKIOAGLC!=f.IBGCLCAIBOL||h.GMILGFFJOIL||h.OGPEJIEHDAN==null)continue;
    var job=h.OGPEJIEHDAN.HFBAJIGHOAF;if(MissionJob(job))continue;
    if(h.LMPMNLLGOEC.CNOMNBCPJDI!=c.OJPECMNINNA)continue;
    if(job==GCPCLLMKIMM.Technology)researchers++;if(job==GCPCLLMKIMM.None||job==GCPCLLMKIMM.Technology||job==GCPCLLMKIMM.PrivateMilitary)home++;
   }
   if(IncomingHostile(c,f)||FoodEmergency(c)||StatecraftPolicy.MissionSlots(home,researchers,away,StateBorder(c,f))==0)continue;
   var pool=c.EOMOIINOEMB(1,false,true,false,false,false,false)?.ToArray();yield return "mission-pool-native";
   var candidates=(pool??Array.Empty<CLAGKDECFID>()).Concat(c.ABJNKABKKJM.ToArray().Where(h=>h?.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.PrivateMilitary)).GroupBy(h=>h.OJPECMNINNA).Select(g=>g.First());
   foreach(var h in candidates){yield return "mission-candidate";if(SafeStateOfficer(c,h))result.Add((c,h));}
  }
 }
 static AFAGFHJONFJ AidSource(MKMOJNAJKJE requester,MKMOJNAJKJE ally){
  var all=ally.PJMLPLDPEIM();if(all==null)return null;
  AFAGFHJONFJ best=null;int strength=0;var ruler=AEHAGJAKNEK.KDHLLDHILCO(ally.IBGCLCAIBOL);
  foreach(var c in all){
   // Native AskForHelp identifies the ALLY'S source city, not the invaded
   // destination. Preserve its capital, frontier and ongoing defense.
   if(c?.LMPMNLLGOEC==null||c.LMPMNLLGOEC.LMMNKIOAGLC!=ally.IBGCLCAIBOL||c.OJPECMNINNA==ruler?.LMPMNLLGOEC?.CNOMNBCPJDI
    ||IncomingHostile(c,ally)||StateBorder(c,ally)||FoodEmergency(c)||(c.LMPMNLLGOEC.KJPPGIOCDID?.Count??0)<3
    ||!requester.MHMHBNOMNIB(ally.IBGCLCAIBOL,c.OJPECMNINNA))continue;
   int power=Power(c);if(power>strength){best=c;strength=power;}
  }return best;
 }
 static bool TryStateDiplomacy(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> pool)=>StateDiplomacyWithMissions(f,cities,pool,ActiveMissions(f));
 static bool StateDiplomacyWithMissions(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> pool,List<CLAGKDECFID> missions){bool done=false;foreach(var phase in DiplomacySteps(f,cities,pool,missions))done|=phase=="diplomacy-commit";return done;}
 static IEnumerable<string> DiplomacySteps(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> pool,List<CLAGKDECFID> missions){
  if(!autoDiplomacy.Value||pool.Count==0||f.DNOGPFHMOMO<ResearchReserve||f.AENJACOFBJK==null)yield break;
  int wars=0;foreach(var rel in f.AENJACOFBJK)if(rel.Value?.EBIOPDHEBOF==BHBNKONCOJD.War)wars++;
  double own=0;var allOwned=f.PJMLPLDPEIM();if(allOwned!=null)foreach(var c in allOwned.ToArray()){yield return "diplomacy-own-power";if(c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL)own+=Power(c);}
  bool attacked=cities.Any(c=>IncomingHostile(c,f));
  MKMOJNAJKJE target=null;AFAGFHJONFJ aid=null;IPNKPODNAIN action=IPNKPODNAIN.Wait;double score=0;
  var relations=new List<(string Key,PPDKOMFGILG Value)>();foreach(var rel in f.AENJACOFBJK)relations.Add((rel.Key,rel.Value));
  foreach(var rel in relations){yield return "diplomacy-relation";
   if(rel.Value==null||rel.Key==f.IBGCLCAIBOL||missions.Any(h=>h.OGPEJIEHDAN.LKEPGHJDHGG==rel.Key)||!DayReady("diplomacy:"+f.IBGCLCAIBOL+":"+rel.Key,5*DecisionDays(f.IBGCLCAIBOL))||!NEINKOOBJIH.COCMABJFOFH.TryGetValue(rel.Key,out var other)||other==null)continue;
   var ruler=AEHAGJAKNEK.KDHLLDHILCO(other.IBGCLCAIBOL);if(ruler?.LMPMNLLGOEC==null||ruler.GMILGFFJOIL)continue;
   bool shared=false;if(other.AENJACOFBJK!=null)foreach(var enemy in f.AENJACOFBJK)if(enemy.Value?.EBIOPDHEBOF==BHBNKONCOJD.War&&War(other,enemy.Key)){shared=true;break;}
   double enemyPower=0;var otherCities=other.PJMLPLDPEIM();if(otherCities!=null)foreach(var c in otherCities.ToArray()){yield return "diplomacy-other-power";if(c?.LMPMNLLGOEC?.LMMNKIOAGLC==other.IBGCLCAIBOL)enemyPower+=Power(c);}
   double v=StatecraftPolicy.Diplomacy((int)rel.Value.EBIOPDHEBOF,wars,shared,own,enemyPower,attacked);
   AFAGFHJONFJ source=null;if(v>0&&rel.Value.EBIOPDHEBOF==BHBNKONCOJD.Ally){
    var alliedCities=other.PJMLPLDPEIM();int strength=0;
    if(alliedCities!=null)foreach(var c in alliedCities.ToArray()){
     yield return "diplomacy-aid-source";
     if(c?.LMPMNLLGOEC?.LMMNKIOAGLC!=other.IBGCLCAIBOL||c.OJPECMNINNA==ruler.LMPMNLLGOEC.CNOMNBCPJDI||IncomingHostile(c,other)||StateBorder(c,other)||FoodEmergency(c)||(c.LMPMNLLGOEC.KJPPGIOCDID?.Count??0)<3||!f.MHMHBNOMNIB(other.IBGCLCAIBOL,c.OJPECMNINNA))continue;
     int power=Power(c);if(power>strength){source=c;strength=power;}
    }
    if(source==null)continue;}
   if(v>score){score=v;target=other;aid=source;action=rel.Value.EBIOPDHEBOF==BHBNKONCOJD.War?IPNKPODNAIN.StopWar:rel.Value.EBIOPDHEBOF==BHBNKONCOJD.Ally?IPNKPODNAIN.AskForHelp:IPNKPODNAIN.Ally1;}
  }
  if(target==null)yield break;
  var rulerCity=AEHAGJAKNEK.KDHLLDHILCO(target.IBGCLCAIBOL)?.LMPMNLLGOEC?.CNOMNBCPJDI;if(string.IsNullOrEmpty(rulerCity))yield break;
  (AFAGFHJONFJ City,CLAGKDECFID Hero) envoy=default;double envoyScore=double.NegativeInfinity;
  foreach(var pair in pool){
   yield return "diplomacy-envoy";
   if(!SafeStateOfficer(pair.City,pair.Hero))continue;
   int travel=TimedAI(f,"diplomacy-native-route",()=>pair.Hero.NBAPOJABAAO(rulerCity,false));
   if(travel<=0||travel>90)continue;
   double value=pair.Hero.LHEKNHNFPIE()/(1+travel/30d);if(value>envoyScore){envoy=pair;envoyScore=value;}
  }
  if(envoy.Hero==null)yield break;
  yield return "diplomacy-validate";
  if(!CurrentFaction(f)||f.DNOGPFHMOMO<ResearchReserve||!SafeStateOfficer(envoy.City,envoy.Hero)||!MissionCapacity(envoy.City,f)||!Eligible(envoy.City,out var owner)||owner.Pointer!=f.Pointer||IncomingHostile(envoy.City,f))yield break;
  if(!f.AENJACOFBJK.TryGetValue(target.IBGCLCAIBOL,out var relation)||relation==null)yield break;
  var currentAction=relation.EBIOPDHEBOF==BHBNKONCOJD.War?IPNKPODNAIN.StopWar:relation.EBIOPDHEBOF==BHBNKONCOJD.Ally?IPNKPODNAIN.AskForHelp:IPNKPODNAIN.Ally1;
  if(action!=currentAction||!DayReady("diplomacy:"+f.IBGCLCAIBOL+":"+target.IBGCLCAIBOL,5*DecisionDays(f.IBGCLCAIBOL)))yield break;
  if(aid!=null&&(aid.LMPMNLLGOEC?.LMMNKIOAGLC!=target.IBGCLCAIBOL||IncomingHostile(aid,target)||StateBorder(aid,target)||FoodEmergency(aid)||!f.MHMHBNOMNIB(target.IBGCLCAIBOL,aid.OJPECMNINNA)))yield break;
  DayReady("diplomacy:"+f.IBGCLCAIBOL+":"+target.IBGCLCAIBOL,0,true);
  envoy.Hero.FHEMHGIGNDE(target.IBGCLCAIBOL,new CHBFLDOKEHC{HFBAJIGHOAF=action,BOEPOCFEHKG=aid?.OJPECMNINNA??""});
  Note($"statecraft-diplomacy faction={f.IBGCLCAIBOL} target={target.IBGCLCAIBOL} action={action} aidSource={aid?.OJPECMNINNA} envoy={envoy.Hero.OJPECMNINNA} job={envoy.Hero.OGPEJIEHDAN.HFBAJIGHOAF} started={MissionJob(envoy.Hero.OGPEJIEHDAN.HFBAJIGHOAF)} proposalOnly=true");yield return "diplomacy-commit";
 }
 static bool TryStateSpy(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> pool)=>StateSpyWithMissions(f,cities,pool,ActiveMissions(f));
 static bool StateSpyWithMissions(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> pool,List<CLAGKDECFID> missions){bool done=false;foreach(var phase in SpySteps(f,cities,pool,missions))done|=phase=="spy-commit";return done;}
 static IEnumerable<string> SpySteps(MKMOJNAJKJE f,List<AFAGFHJONFJ> cities,List<(AFAGFHJONFJ City,CLAGKDECFID Hero)> pool,List<CLAGKDECFID> missions){
  if(!autoSchemes.Value||pool.Count==0)yield break;
  var targets=new Dictionary<string,AFAGFHJONFJ>();foreach(var c in cities)if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)
   if(n?.LMPMNLLGOEC!=null&&War(f,n.LMPMNLLGOEC.LMMNKIOAGLC)&&!missions.Any(h=>h.OGPEJIEHDAN.NHKNHHIBOGK==n.OJPECMNINNA)&&DayReady("spy:"+f.IBGCLCAIBOL+":"+n.OJPECMNINNA,5*DecisionDays(f.IBGCLCAIBOL)))targets[n.OJPECMNINNA]=n;
  CLAGKDECFID actor=null;AFAGFHJONFJ home=null,target=null;double best=0;
  foreach(var n in targets.Values)foreach(var pair in pool){yield return "spy-candidate";
   if(f.ANMMMOIKLEG.TryGetValue(pair.City.LMPMNLLGOEC.IPJHOOJBCKI,out var governor)&&governor?.BNGFJOAJKDH==NEFBEOFPGJA.StopWar)continue;
   var h=pair.Hero;int travel=h.NBAPOJABAAO(n.OJPECMNINNA,false),ability=h.MLGPOHGNLOL(DOHGPECAMJC.Spy);
   if(travel<=0||travel>90||ability<60)continue;double v=ability/(1+travel/10d);if(v>best){actor=h;home=pair.City;target=n;best=v;}
  }
  yield return "spy-validate";
  if(actor==null||!SafeStateOfficer(home,actor)||!MissionCapacity(home,f)||!Eligible(home,out var owner)||owner.Pointer!=f.Pointer||!War(f,target.LMPMNLLGOEC.LMMNKIOAGLC)||IncomingHostile(home,f)||!DayReady("spy:"+f.IBGCLCAIBOL+":"+target.OJPECMNINNA,5*DecisionDays(f.IBGCLCAIBOL)))yield break;
  if(f.ANMMMOIKLEG.TryGetValue(home.LMPMNLLGOEC.IPJHOOJBCKI,out var currentGovernor)&&currentGovernor?.BNGFJOAJKDH==NEFBEOFPGJA.StopWar)yield break;
  DayReady("spy:"+f.IBGCLCAIBOL+":"+target.OJPECMNINNA,0,true);
  actor.IGIMIIDMGAH(target.OJPECMNINNA,"",DOHGPECAMJC.Spy,false);
  Note($"statecraft-spy faction={f.IBGCLCAIBOL} target={target.OJPECMNINNA} officer={actor.OJPECMNINNA} job={actor.OGPEJIEHDAN.HFBAJIGHOAF} started={MissionJob(actor.OGPEJIEHDAN.HFBAJIGHOAF)} nativeTravel=true");yield return "spy-commit";
 }
 static IEnumerable<string> SafeMissionPhases(IEnumerable<string> phases){
  using(var it=phases.GetEnumerator())while(true){
   bool more=false,failed=false;try{more=it.MoveNext();}catch(Exception e){Error(e);failed=true;}
   if(failed){yield return "mission-error";yield break;}if(!more)yield break;yield return it.Current;
  }
 }
 static void TryStatecraft(FNFJODHIJFO governor){
  if(governor!=null&&NEINKOOBJIH.COCMABJFOFH.TryGetValue(governor.LMMNKIOAGLC,out var f)){if(schedulerOnline)ScheduleFaction(f);else TryFactionStatecraft(f);}
 }
 static void TryFactionStatecraft(MKMOJNAJKJE f){foreach(var step in FactionStatecraftSteps(f)){} }
 static IEnumerable<string> FactionStatecraftSteps(MKMOJNAJKJE f){
  if(statecraft?.Value!=true||!Ready()||f==null)yield break;
  if(!AIGate("statecraft:"+f.IBGCLCAIBOL,f.IBGCLCAIBOL,30)||!DayReady("statecraft:"+f.IBGCLCAIBOL,DecisionDays(f.IBGCLCAIBOL)))yield break;
  var cities=StateCities(f);var strategicCities=new List<AFAGFHJONFJ>();var owned=f.PJMLPLDPEIM();
  if(owned!=null)foreach(var c in owned)if(c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL)strategicCities.Add(c);
  if(strategicCities.Count==0)yield break;
  var generation=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;
  DayReady("statecraft:"+f.IBGCLCAIBOL,0,true);TimedAI(f,"faction-census",()=>{AuditFaction(f,strategicCities);return true;});yield return "census";
  int started=0;
  for(int pass=0;pass<1;pass++){
   if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;
   bool success=false;
   using(var scan=StateStrategyBatchSteps(f,strategicCities,true).GetEnumerator())while(true){
    bool more;try{more=scan.MoveNext();}catch(Exception e){Error(e);break;}if(!more)break;
    success|=scan.Current;if(scan.Current)started++;yield return scan.Current?"strategy-commit":"strategy-scan";
    if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;
   }
   if(!success)break;
  }
  if(!CurrentFaction(f))yield break;
  ScheduleDiplomaticWork(f);
  Note($"statecraft-decision faction={f.IBGCLCAIBOL} eligibleCities={cities.Count} strategies={started} missions=independent-queue");
 }
 static void ScheduleDiplomaticWork(MKMOJNAJKJE f){
  if(statecraft?.Value!=true||f==null)return;string key="diplomatic-work:"+f.IBGCLCAIBOL;
  if(DecisionDue(key,f.IBGCLCAIBOL,f.Pointer,"",false)&&work.Add(key,()=>DiplomaticWork(f).GetEnumerator()))DecisionDue(key,f.IBGCLCAIBOL,f.Pointer,"",true);
 }
 static IEnumerable<string> DiplomaticWork(MKMOJNAJKJE f){
  if(!Ready()||!CurrentFaction(f)||statecraft?.Value!=true)yield break;
  var generation=MICGPAMNDKM.IJPKEMPIFIH?.Pointer;var cities=StateCities(f);
  var pool=new List<(AFAGFHJONFJ City,CLAGKDECFID Hero)>();var missions=new List<CLAGKDECFID>();
  foreach(var phase in StateOfficerSteps(cities,f,pool,missions)){
   yield return phase;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;
  }
  if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;
  int committed=0;int rotation=(int)Math.Floor((double)NEINKOOBJIH.MIPFHCKDJOK/Math.Max(1,DecisionDays(f.IBGCLCAIBOL)))%3;
  for(int stage=0;stage<5&&committed<3;stage++){
   int category=stage<2?stage:2+(stage-2+rotation)%3;
   pool.Clear();missions.Clear();foreach(var phase in StateOfficerSteps(cities,f,pool,missions)){yield return phase;if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;}
   IEnumerable<string> action=category==0?ActiveSchemeSteps(f,cities,pool,missions,true):category==1?CaptiveDiplomacySteps(f,pool,missions):category==2?DiplomacySteps(f,cities,pool,missions):category==3?ActiveSchemeSteps(f,cities,pool,missions,false):SpySteps(f,cities,pool,missions);
   foreach(var phase in SafeMissionPhases(action)){
    yield return phase;if(phase=="mission-error")yield break;
    if(phase=="diplomacy-commit"||phase=="scheme-commit"||phase=="spy-commit")committed++;
    if(MICGPAMNDKM.IJPKEMPIFIH?.Pointer!=generation||!CurrentFaction(f))yield break;
   }
  }
  Note($"diplomatic-assess faction={f.IBGCLCAIBOL} availableEnvoys={pool.Count} activeMissions={ActiveMissions(f).Count} committed={committed} categories=rescue,ransom,diplomacy,raid,spy");
 }
}
