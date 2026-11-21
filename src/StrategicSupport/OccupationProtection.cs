using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 // Resource coordination is faction-wide. Construction, appointments and
 // conquest retain their separate governor permissions.
 static bool DistributionCity(AFAGFHJONFJ c,out MKMOJNAJKJE f){
  f=null;var id=c?.LMPMNLLGOEC?.LMMNKIOAGLC;
  return !string.IsNullOrEmpty(id)&&NEINKOOBJIH.COCMABJFOFH.TryGetValue(id,out f)&&f!=null;
 }
 static bool StaffingDestination(AFAGFHJONFJ c,MKMOJNAJKJE f){
  return f!=null&&!string.IsNullOrEmpty(f.IBGCLCAIBOL)&&c?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL;
 }
 static List<AFAGFHJONFJ> ProtectedCities(MKMOJNAJKJE f){var result=new List<AFAGFHJONFJ>();var native=f.PJMLPLDPEIM();if(native!=null)foreach(var c in native)if(StaffingDestination(c,f))result.Add(c);return result;}
 sealed class Occupation {public string Owner;public double Since;}
 static readonly Dictionary<string,Occupation> occupations=new();
 static int BorderTier(AFAGFHJONFJ c,MKMOJNAJKJE f){
  if(StateBorder(c,f)||IncomingHostile(c,f))return 3;
  if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&StateBorder(n,f))return 2;
  return 1;
 }
 static void ObserveOccupation(AFAGFHJONFJ c){
  double day=NEINKOOBJIH.MIPFHCKDJOK;string owner=c.LMPMNLLGOEC?.LMMNKIOAGLC??"";
  if(!occupations.TryGetValue(c.OJPECMNINNA,out var o))occupations[c.OJPECMNINNA]=new(){Owner=owner,Since=double.NegativeInfinity};
  else if(o.Owner!=owner){o.Owner=owner;o.Since=day;}
 }
 static bool OccupationVulnerable(AFAGFHJONFJ c){
  double day=NEINKOOBJIH.MIPFHCKDJOK;string owner=c.LMPMNLLGOEC?.LMMNKIOAGLC??"";
  if(!occupations.TryGetValue(c.OJPECMNINNA,out var o))occupations[c.OJPECMNINNA]=o=new(){Owner=owner,Since=double.NegativeInfinity};
  else if(o.Owner!=owner){o.Owner=owner;o.Since=day;}
  // Reconstruct vulnerability after loading, when capture history is absent.
  int locals=0;if(c.ABJNKABKKJM!=null)foreach(var h in c.ABJNKABKKJM)if(RelocationHome(h,c))locals++;
  return day>=o.Since&&day-o.Since<30||locals<=2&&string.IsNullOrEmpty(c.LMPMNLLGOEC.BKENJPNKMGI);
 }
}
