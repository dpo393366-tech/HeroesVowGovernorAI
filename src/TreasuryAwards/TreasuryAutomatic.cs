using System;
using System.Collections.Generic;
using System.Linq;

public sealed partial class TreasuryAwards
{
 // Compatibility API for callers without a main-thread scheduler.
 public static string RunAutomatic(MKMOJNAJKJE power){
  string result="skipped:not-ready";
  foreach(var phase in RunAutomaticSteps(power))if(phase.StartsWith("result:"))result=phase.Substring(7);
  return result;
 }
 static string AutomaticBlock(MKMOJNAJKJE power){
  var player=NEINKOOBJIH.POMNMBGDDIN;
  if(busy||player?.LMPMNLLGOEC==null||power==null||power.IBGCLCAIBOL!=player.LMPMNLLGOEC.LMMNKIOAGLC)return "skipped:busy-or-owner";
  if(UI_RewardUI.m_UI&&UI_RewardUI.m_UI.gameObject.activeInHierarchy)return "skipped:manual-reward-screen";
  if(UI_RewardUI.m_isDragging)return "skipped:dragging";
  if(RecipientPercent==null||MLGOLGGHLMB.CBEGDDKANAO==null||power.GPGCLMPBCBP==null)return "skipped:not-ready";
  return null;
 }
 // Each MoveNext owns the lock only while executing. Pausing the game or
 // opening the manual reward screen never leaves the manual button locked.
 public static IEnumerable<string> RunAutomaticSteps(MKMOJNAJKJE power){
  string blocked=AutomaticBlock(power);if(blocked!=null){yield return "result:"+blocked;yield break;}
  var player=NEINKOOBJIH.POMNMBGDDIN;var playerPointer=player.Pointer;int percent=RecipientPercent.Value;
  bool Valid()=>AutomaticBlock(power)==null&&NEINKOOBJIH.POMNMBGDDIN?.Pointer==playerPointer&&RecipientPercent.Value==percent;
  using(var it=AllocateAutomaticSteps(power,player,percent).GetEnumerator())while(true){
   if(!Valid()){yield return "result:interrupted:owner-option-or-manual-ui";yield break;}
   bool more=false;string error=null;
   busy=true;try{more=it.MoveNext();}catch(Exception e){plugin.Log.LogError("Automatic treasury allocation stopped: "+e);error=e.GetType().Name;}finally{busy=false;}
   if(error!=null){yield return "result:failed:"+error;yield break;}
   if(!more)yield break;yield return it.Current;
  }
 }
 static IEnumerable<string> AllocateAutomaticSteps(MKMOJNAJKJE power,CLAGKDECFID player,int percent){
  var candidates=new Dictionary<string,CLAGKDECFID>(StringComparer.Ordinal);
  IEnumerable<string> Include(Il2CppSystem.Collections.Generic.List<CLAGKDECFID> list){
   if(list==null)yield break;
   foreach(var h in list.ToArray()){
    yield return "treasury-roster";
    if(h!=null&&h.Pointer!=player.Pointer&&h.LMPMNLLGOEC?.LMMNKIOAGLC==power.IBGCLCAIBOL&&!string.IsNullOrEmpty(h.OJPECMNINNA))candidates.TryAdd(h.OJPECMNINNA,h);
   }
  }
  foreach(var phase in Include(UI_HeroListInfoUI.PDPLMJHLKJF))yield return phase;
  foreach(var phase in Include(UI_RewardUI.ABJNKABKKJM))yield return phase;
  var cities=power.PJMLPLDPEIM();
  if(cities!=null)foreach(var city in cities.ToArray())if(city!=null){
   yield return "treasury-city";
   foreach(var phase in Include(city.EOMOIINOEMB(0,true,true,false,false,false,false)))yield return phase;
  }
  var ranked=new List<(string Id,CLAGKDECFID Hero,long Score)>();
  foreach(var pair in candidates){yield return "treasury-score";ranked.Add((pair.Key,pair.Value,Score(pair.Value)));}
  var heroes=ranked.OrderByDescending(x=>x.Score).ThenBy(x=>x.Id,StringComparer.Ordinal).Take(AwardRules.RecipientCount(candidates.Count,percent)).Select(x=>x.Hero).Where(h=>!h.GMILGFFJOIL).ToArray();
  var sources=new Dictionary<IntPtr,Il2CppSystem.Collections.Generic.List<CMHNADEDJIE>>();var stock=new List<Item>();
  foreach(var list in new[]{power.CNBLOJCGNFD,power.EHGOOJLIHGH,power.OILNHBFCJII,power.FCJHJPANFJN,power.OHDEBNFBGIE,power.PCGHDEOMPBO,power.OOACPLDPNHM,power.ONEHAJPKPMP,power.CJDLOOOEGPI,power.EFHPAEBIGJK,power.HFCLLCIPNBB}){
   if(list==null)continue;foreach(var row in list.ToArray()){
    yield return "treasury-price";
    if(row!=null&&sources.TryAdd(row.Pointer,list)&&list.Contains(row)){
     var single=new Il2CppSystem.Collections.Generic.List<CMHNADEDJIE>();single.Add(row);stock.AddRange(Stock(single));
    }
   }
  }
  stock=stock.OrderByDescending(x=>x.Price).ThenByDescending(x=>x.Grade).ThenBy(x=>x.Id,StringComparer.Ordinal).ToList();
  if(stock.Count==0||heroes.Length==0){yield return $"result:percent={percent} recipients={heroes.Length} granted=0 empty=true";yield break;}
  var flags=MLGOLGGHLMB.CBEGDDKANAO;string prefix="local.heroesvow.treasuryawards/v1/"+power.IBGCLCAIBOL+"/";
  int Read(string key)=>flags.TryGetValue(prefix+key,out int v)?v:0;
  void Write(string key,int value)=>flags[prefix+key]=value;
  int cycle=Math.Max(1,Read("cycle"));var instant=new HashSet<string>();
  bool CycleValid()=>Math.Max(1,Read("cycle"))==cycle;
  IEnumerable<string> GiveOne(CLAGKDECFID hero,int pass,Action<bool> done){
   string id=hero.OJPECMNINNA;
   // Drop irrelevant growth categories using managed metadata before any
   // per-item native work; generic gifts otherwise cause N*M failed probes.
   int force=hero.JLAMCIKNBON(),wise=hero.GNCHNFHKBKE();
   var choices=pass==0?stock.Where(item=>AwardRules.PreferredGrowth(item.GrowthRole,force,wise)).ToArray():stock.ToArray();
   foreach(var item in choices){
    yield return "treasury-match";
    if(!CycleValid()||hero.LMPMNLLGOEC?.LMMNKIOAGLC!=power.IBGCLCAIBOL||hero.GMILGFFJOIL)yield break;
    // All mutable constraints are read after the yield, in the same step as
    // the debit/queue transaction. Manual actions cannot invalidate a saved
    // queue count, stock count or immediate-grant permission between them.
    power.GPGCLMPBCBP.TryGetValue(id,out var queue);
    bool same=Read("epoch/"+id)==cycle;int count=same?Read("count/"+id):0,bits=same?Read("slots/"+id):0,occupied=bits;
    if(count>=20)yield break;
    if(queue!=null)foreach(var row in queue)if(row!=null)occupied|=EquipmentSlotBit(row.OJPECMNINNA);
    bool immediate=!hero.JPHIGPAMLEI&&!instant.Contains(id);
    if(!immediate&&(queue?.Count??0)>=3)yield break;
    if(pass==0&&!AwardRules.PreferredGrowth(item.GrowthRole,hero.JLAMCIKNBON(),hero.GNCHNFHKBKE()))continue;
    var source=sources[item.Row.Pointer];if(!source.Contains(item.Row)||item.Row.NGBNIPKFPOE<=0){stock.Remove(item);continue;}
    if((occupied&item.SlotBit)!=0||!Improves(hero,item))continue;
     CMHNADEDJIE queued=immediate?null:CMHNADEDJIE.OCELGGJBGEN(item.Id,1,false,false,0f);
     if(!immediate&&queued==null)continue;
     int before=item.Row.NGBNIPKFPOE;
     if(!immediate){
      if(queue==null){queue=new();power.GPGCLMPBCBP[id]=queue;}
      queue.Add(queued);
      try{item.Row.NGBNIPKFPOE=before-1;}catch{queue.Remove(queued);queued.Recycle();throw;}
     }else item.Row.NGBNIPKFPOE=before-1;
     Write("slots/"+id,bits|item.SlotBit);Write("epoch/"+id,cycle);Write("count/"+id,count+1);
     if(item.Row.NGBNIPKFPOE==0){source.Remove(item.Row);stock.Remove(item);item.Row.Recycle();}
     if(immediate){instant.Add(id);hero.JCAAECBOAMI(item.Id);}
    done(true);yield break;
   }
  }
  int granted=0;bool complete=false;
  int round=Read("rr-epoch")==cycle?Math.Max(1,Read("rr-round")):1;
  Write("rr-epoch",cycle);Write("rr-round",round);
  bool Done(CLAGKDECFID h)=>Read("seen/"+h.OJPECMNINNA)==cycle;
  for(int sweep=0;sweep<21&&stock.Count>0&&granted<32;sweep++){
   for(int pass=0;pass<2;pass++)foreach(var hero in heroes){
    yield return "treasury-turn";if(!CycleValid())yield break;
    string id=hero.OJPECMNINNA;
    if(Done(hero)||(Read("rr-epoch/"+id)==cycle&&Read("rr-turn/"+id)==round))continue;
    if(stock.Count==0||granted>=32)break;
    int before=Read("epoch/"+id)==cycle?Read("count/"+id):0;bool accepted=false;
    try{foreach(var phase in GiveOne(hero,pass,value=>accepted=value))yield return phase;}
    finally{
     if(accepted||(Read("epoch/"+id)==cycle&&Read("count/"+id)>before)){Write("rr-epoch/"+id,cycle);Write("rr-turn/"+id,round);}
    }
    if(accepted)granted++;else if(pass==1)Write("seen/"+id,cycle);
   }
   if(granted>=32||stock.Count==0)break;
   if(heroes.All(Done)){complete=true;if(cycle<int.MaxValue)Write("cycle",cycle+1);break;}
   if(round==int.MaxValue)break;round++;Write("rr-round",round);
  }
  yield return $"result:faction={power.IBGCLCAIBOL} percent={percent} recipients={heroes.Length} granted={granted} roundRobin=true continuation={!complete} nativeMonthlyQueue=true sliced=true";
 }
}
