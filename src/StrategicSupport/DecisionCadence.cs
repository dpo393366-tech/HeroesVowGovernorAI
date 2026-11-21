using System;
using System.Collections.Generic;

public sealed partial class StrategicSupport {
 sealed class DecisionStamp {public double Day;public int Period;public IntPtr Identity;public string Owner,Governor;}
 static readonly Dictionary<string,DecisionStamp> decisionStamps=new();
 static int DecisionDays(string faction){
  string player=NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC;
  return !string.IsNullOrEmpty(player)&&player==faction?1:3;
 }
 static double DecisionInterval(string faction,double alliedSeconds)=>Math.Max(0,alliedSeconds)*DecisionDays(faction);
 static readonly Dictionary<string,(string Owner,int Period)> aiGatePeriods=new();
 static bool AIGate(string key,string faction,int seconds){
  var signature=(faction,DecisionDays(faction));
  if(aiGatePeriods.TryGetValue(key,out var previous)&&previous!=signature)next.Remove(key);
  aiGatePeriods[key]=signature;
  return Gate(key,(int)Math.Min(int.MaxValue,DecisionInterval(faction,seconds)));
 }
 static bool DecisionDue(string key,string owner,IntPtr identity,string governor,bool commit){
  double day=Math.Floor((double)NEINKOOBJIH.MIPFHCKDJOK);int period=DecisionDays(owner);
  if(decisionStamps.TryGetValue(key,out var old)&&old.Identity==identity&&old.Owner==owner&&old.Governor==governor&&old.Period==period&&day>=old.Day&&day-old.Day<period)return false;
  if(commit){decisionStamps[key]=new(){Day=day,Period=period,Identity=identity,Owner=owner,Governor=governor};
   if(Gate("decision-cadence:"+key,30))Note($"ai-decision-cadence key={key} faction={owner} playerFaction={NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC} days={period} day={day} scope=regular eventAndBattleIntervalMultiplier={period}");
  }return true;
 }
}
