using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 sealed class RejectedBattleDispatch {
  public IntPtr War;public string Name,Signature,Reason;public double Day,Until,Eta;public int Count;public long Food;
 }
 static readonly Dictionary<string,RejectedBattleDispatch> rejectedBattleDispatches=new();
 [ThreadStatic] static string supportGenerationFailure;
 [ThreadStatic] static bool supportGenerationCreated;
 static string BattleDispatchKey(AFAGFHJONFJ c,NJHDEHLHENN w,int side,bool direct)=>w.Pointer+"|"+w.HIENOINGBCN+"|"+side+"|"+direct+"|"+c.Pointer+"|"+c.LMPMNLLGOEC.LMMNKIOAGLC;
 static string BattleDispatchSignature(AFAGFHJONFJ c,NJHDEHLHENN w,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams)=>
  MICGPAMNDKM.IJPKEMPIFIH?.Pointer+"|"+CombatProbeSignature(c,0,teams)+"|"+RoadPolicyStamp(f)+"|"+
  c.BMBMKPGEHBH.x+","+c.BMBMKPGEHBH.y+","+c.BMBMKPGEHBH.z+"|"+w.GBHGENNMJCK.x+","+w.GBHGENNMJCK.y+","+w.GBHGENNMJCK.z+
  "|"+Wo_DeployTeamCtrl.GetMoveSpeed(f.IBGCLCAIBOL,false)+"|"+maxTravelSeconds.Value+"|"+mobilizeSeconds.Value+"|"+foodDays.Value;
 static bool BattleDispatchReady(AFAGFHJONFJ c,NJHDEHLHENN w,int side,bool direct,string signature,Func<double,bool> useful){
  string key=BattleDispatchKey(c,w,side,direct);if(!rejectedBattleDispatches.TryGetValue(key,out var old))return true;
  double day=NEINKOOBJIH.MIPFHCKDJOK;
  bool changed=old.Signature!=signature||day<old.Day||
   (old.Reason=="arrival-not-useful"&&double.IsFinite(old.Eta)&&useful(old.Eta))||
   (old.Reason=="journey-food"&&c.LMPMNLLGOEC.OEGNGFBCHFL>old.Food+Math.Max(100,old.Food/10));
  if(changed){rejectedBattleDispatches.Remove(key);Note($"battle-retry-reopened source={c.OJPECMNINNA} war={w.HIENOINGBCN} previousReason={old.Reason} reason=conditions-changed");return true;}
  if(day>=old.Until)return true;
  if(Gate("battle-retry:"+key,15))Note($"battle-retry-deferred source={c.OJPECMNINNA} war={w.HIENOINGBCN} reason={old.Reason} retryDay={old.Until:F3} day={day:F3} unchangedFailures={old.Count} noArmyCreated=true");
  return false;
 }
 static void RejectBattleDispatch(AFAGFHJONFJ c,NJHDEHLHENN w,int side,bool direct,string signature,string reason,double eta,int teamId,long sourceFood){
  string key=BattleDispatchKey(c,w,side,direct);double day=NEINKOOBJIH.MIPFHCKDJOK;int count=1;
  if(rejectedBattleDispatches.TryGetValue(key,out var old)&&old.Signature==signature&&old.Reason==reason&&day>=old.Day)count=Math.Min(3,old.Count+1);
  if(rejectedBattleDispatches.Count>=1024)rejectedBattleDispatches.Clear();
  double until=day+Math.Min(7,3*count);
  rejectedBattleDispatches[key]=new(){War=w.Pointer,Name=w.HIENOINGBCN,Signature=signature,Reason=reason,Day=day,Until=until,Eta=eta,Count=count,Food=sourceFood};
  Note($"battle-dispatch-rejected source={c.OJPECMNINNA} faction={c.LMPMNLLGOEC.LMMNKIOAGLC} war={w.HIENOINGBCN} team={teamId} reason={reason} actualEta={eta:F1} retryDay={until:F3} attempt={count} scope=this-city-this-battle");
 }
 static void ObserveBattleDispatchDismissal(Wo_DeployTeamCtrl t){
  if(!t||!missions.TryGetValue(t.Pointer,out var m)||m.Id!=t.m_id||m.Faction!=t.m_belongPower||m.Departed||m.War==null||m.Name!=m.War.HIENOINGBCN||t.m_warInfo!=null||ManualProtected(t)||string.IsNullOrEmpty(m.ProbeSignature))return;
  missions.Remove(t.Pointer);
  if(!MICGPAMNDKM.IJPKEMPIFIH.TryGetValue(m.Source??"",out var c)||c.LMPMNLLGOEC?.LMMNKIOAGLC!=m.Faction)return;
  RejectBattleDispatch(c,m.War,m.Side,m.Direct,m.ProbeSignature,"native-cancel-before-departure",m.ProbeEta,t.m_id,m.ProbeFood);
  Note($"battle-departure-cancelled team={t.m_id} source={m.Source} war={m.Name} state={t.m_deployState} target={t.m_targetCity} targetWar={t.m_targetWarInfo?.HIENOINGBCN} power={t.GetCombatVal()} soldiers={t.GetSoldierNum(false)} nativeLeaveCityObserved=false");
 }
 static void ForgetBattleDispatchRetries(NJHDEHLHENN w){foreach(var key in rejectedBattleDispatches.Where(p=>p.Value.War==w.Pointer).Select(p=>p.Key).ToArray())rejectedBattleDispatches.Remove(key);}
}
