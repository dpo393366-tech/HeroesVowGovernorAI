using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 sealed class AttackProbeCapacity {public long Raw,Slots;public double Quality;}
 // A non-mutating admission estimate, calibrated to the last ACTUAL assembled
 // power. It never substitutes raw troop power for GetCombatVal's final check.
 // No periodic creation/dismissal is needed to discover an unchanged shortage.
 static AttackProbeCapacity ReadAttackProbeCapacity(AFAGFHJONFJ c,List<Wo_DeployTeamCtrl> teams){
  var result=new AttackProbeCapacity();if(!NEINKOOBJIH.COCMABJFOFH.TryGetValue(c.LMPMNLLGOEC.LMMNKIOAGLC,out var f))return result;
  var home=HomeOfficers(c,teams);int keep=Math.Max(2,(home.Count+2)/3);int allowed=Math.Max(0,home.Count-keep);
  var commanders=new List<(int Slots,double Quality)>();
  var native=c.EOMOIINOEMB(1,false,true,false,false,false,false);
  if(native!=null)foreach(var h in native)if(h!=null&&home.Contains(h.OJPECMNINNA))commanders.Add((Math.Max(0,h.MJICLKFOAPI(true)),Math.Max(0,h.GGOIKBIJNCJ())+Math.Max(0,h.JLAMCIKNBON())+Math.Max(0,h.GNCHNFHKBKE())));
  // Native generation also retains its final eligible officer. Use the same
  // upper bound instead of assuming every home officer can lead an army.
  allowed=Math.Min(allowed,Math.Max(0,commanders.Count-1));
  foreach(var h in commanders.OrderByDescending(h=>h.Slots).ThenByDescending(h=>h.Quality).Take(allowed)){result.Slots+=h.Slots;result.Quality=Math.Max(result.Quality,h.Quality);}
  var stock=AttackStock(c,f,teams);var reserved=AttackReserved(teams);var seen=new HashSet<IntPtr>();var raw=new List<int>();
  foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null&&seen.Add(a.Pointer)&&!reserved.Contains(a.Pointer)&&Available(a))raw.Add(Math.Max(0,a.GNCKPEPJDGF()));
  int slots=(int)Math.Min(int.MaxValue,Math.Min(result.Slots,Math.Max(0,stock.Home-minUnits.Value)));
  result.Raw=Math.Min(stock.Budget,raw.OrderByDescending(x=>x).Take(slots).Sum(x=>(long)x));return result;
 }
}
