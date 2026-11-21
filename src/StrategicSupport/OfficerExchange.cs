using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 static readonly Dictionary<string,int> exchangeCursor=new();
 static void ResetCoordination(){exchangeCursor.Clear();}
 static bool ExchangeCity(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams){
  if(c==null||!Eligible(c,out var own)||own.Pointer!=f.Pointer||c.GKLIEHCHDLD()||FoodEmergency(c)||!AttackPermission(c,f,c.LMPMNLLGOEC.IPJHOOJBCKI))return false;
  foreach(var t in teams)if(t.m_assembleCity==c.OJPECMNINNA||t.m_targetCity==c.OJPECMNINNA)return false;
  return true;
 }
 static bool TryOfficerExchange(AFAGFHJONFJ source){
  if(!rebalanceGarrisons.Value||!support.Value||!Eligible(source,out var f)||!AIGate("exchange:"+source.OJPECMNINNA,f.IBGCLCAIBOL,60))return false;
  var teams=AttackTeams();if(!ExchangeCity(source,f,teams))return false;var reserve=AttackReserved(teams);
  var cache=new Dictionary<string,OfficerBalancePolicy.Officer>();var a=OfficerRoster(source,teams,false,cache);if(a.Count<4)return false;
  var cities=new List<AFAGFHJONFJ>();foreach(var kv in MICGPAMNDKM.IJPKEMPIFIH)if(kv.Value?.Pointer!=source.Pointer&&kv.Value?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL)cities.Add(kv.Value);
  if(cities.Count==0)return false;cities.Sort((x,y)=>string.CompareOrdinal(x.OJPECMNINNA,y.OJPECMNINNA));exchangeCursor.TryGetValue(source.OJPECMNINNA,out int cursor);
  AFAGFHJONFJ target=null;OfficerBalancePolicy.Officer give=null,take=null;double best=.025;
  bool Movable(AFAGFHJONFJ city,OfficerBalancePolicy.Officer h)=>!FamilyCityOfficerProtected(city,h.Id)&&h.Id!=city.LMPMNLLGOEC.IPJHOOJBCKI&&h.Id!=city.LMPMNLLGOEC.BKENJPNKMGI&&CanGate("officer-balance:"+h.Id);
  for(int i=0;i<Math.Min(2,cities.Count);i++){
   var city=cities[(cursor+i)%cities.Count];if(!ExchangeCity(city,f,teams)||!ViableSource(city,teams,reserve))continue;
   var b=OfficerRoster(city,teams,false,cache);if(b.Count<4)continue;
   foreach(var x in a.Where(x=>Movable(source,x)).OrderByDescending(x=>x.Ability.Sum()).Take(16))foreach(var y in b.Where(y=>Movable(city,y)).OrderBy(y=>y.Ability.Sum()).Take(16)){
    double score=OfficerBalancePolicy.ExchangeGain(a,b,x,y);if(score>best){best=score;target=city;give=x;take=y;}
   }
  }exchangeCursor[source.OJPECMNINNA]=(cursor+2)%cities.Count;if(target==null)return false;
  Wo_DeployTeamCtrl first=null,second=null;bool committed=false;rebalancing=true;
  Wo_DeployTeamCtrl Make(AFAGFHJONFJ c,OfficerBalancePolicy.Officer officer){selectedOfficer=officer.Id;selectedOfficerCity=c.Pointer;try{return GenerateSupport(c,1,teams,reserve);}finally{selectedOfficer=null;selectedOfficerCity=IntPtr.Zero;}}
  try{
   first=Make(source,give);if(!first||first.m_listHero?.Count!=1||first.m_listHero[0].OJPECMNINNA!=give.Id)return false;
   second=Make(target,take);if(!second||second.m_listHero?.Count!=1||second.m_listHero[0].OJPECMNINNA!=take.Id)return false;
   if(first.IsPassEnemyCity(target.BMBMKPGEHBH)||second.IsPassEnemyCity(source.BMBMKPGEHBH)||!CurrentCity(source,f,source.LMPMNLLGOEC.IPJHOOJBCKI)||!CurrentCity(target,f,target.LMPMNLLGOEC.IPJHOOJBCKI))return false;
   int aDays=first.m_listHero[0].NBAPOJABAAO(target.OJPECMNINNA,false),bDays=second.m_listHero[0].NBAPOJABAAO(source.OJPECMNINNA,false);if(aDays<=0||bDays<=0||Math.Max(aDays,bDays)>45)return false;
   first.m_targetCity=target.OJPECMNINNA;first.m_isEnterCity=true;second.m_targetCity=source.OJPECMNINNA;second.m_isEnterCity=true;
   first.gameObject.SetActive(true);second.gameObject.SetActive(true);committed=true;
   next["officer-balance:"+give.Id]=next["officer-balance:"+take.Id]=Environment.TickCount64+300000;
   Note($"officer-exchange a={source.OJPECMNINNA} b={target.OJPECMNINNA} give={give.Id} take={take.Id} gain={best:F4} nativeTravel=true");return true;
  }finally{selectedOfficer=null;selectedOfficerCity=IntPtr.Zero;rebalancing=false;if(!committed){if(first)DismissSupport(first);if(second)DismissSupport(second);}}
 }
}
