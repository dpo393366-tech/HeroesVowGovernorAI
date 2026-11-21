using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport {
 [ThreadStatic] static bool offenseGenerating;
 static long AttackBorderThreat(AFAGFHJONFJ c,MKMOJNAJKJE f,List<Wo_DeployTeamCtrl> teams){long sum=0,max=0;var seen=new HashSet<string>();foreach(var t in teams)if(t&&War(f,t.m_belongPower)&&(t.m_targetCity==c.OJPECMNINNA||t.m_warInfo?.IJGLCFJOOIK?.OJPECMNINNA==c.OJPECMNINNA))return long.MaxValue;if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n?.LMPMNLLGOEC!=null&&War(f,n.LMPMNLLGOEC.LMMNKIOAGLC)&&seen.Add(n.OJPECMNINNA)){if(!ThreatRoadLength(n,c,n.LMPMNLLGOEC.LMMNKIOAGLC,n.BMBMKPGEHBH,out _,out _))continue;long power=ReserveCityPower(n,teams);max=Math.Max(max,power);sum+=power;}return (long)Math.Ceiling((max+(sum-max)*.35)*Math.Round(coverage.Value,3));}
 // All extra consumers respect the same emergency and active mobilization floor.
 // Unspent recruitment quotas are not permanently locked away from research.
 static int SharedReserve(MKMOJNAJKJE f)=>Math.Max(ResearchReserve,Math.Max(investmentTreasuryReserve.Value,OffenseCashReserve(f)));
 static bool ActiveFront(AFAGFHJONFJ c,MKMOJNAJKJE f){if(c.FKMKGNPFLFP!=null)foreach(var n in c.FKMKGNPFLFP)if(n?.LMPMNLLGOEC!=null&&War(f,n.LMPMNLLGOEC.LMMNKIOAGLC))return true;return false;}
 static double Retention(AFAGFHJONFJ c,MKMOJNAJKJE f)=>Math.Round(ActiveFront(c,f)?retain.Value:Math.Max(.35,retain.Value-.20),3);
 static int ExpeditionLimit(MKMOJNAJKJE f){int readyCities=0;foreach(var kv in MICGPAMNDKM.IJPKEMPIFIH)if(kv.Value?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&Eligible(kv.Value,out _)&&!kv.Value.GKLIEHCHDLD())readyCities++;return Math.Clamp(1+readyCities/3,2,6);}
 static int MobilizationCost(AFAGFHJONFJ c)=>Math.Max(3000,Math.Min(attackTreasury.Value,(int)Math.Min(int.MaxValue,Math.Max(3000L,c.LAGKJHBEPGF(false)*3L))));
 static HashSet<string> ProtectedCivilOfficers(AFAGFHJONFJ c){
  var keep=new HashSet<string>();if(c?.ABJNKABKKJM==null)return keep;
  var idle=new List<CLAGKDECFID>();foreach(var h in c.ABJNKABKKJM)if(h?.LMPMNLLGOEC!=null&&!h.GMILGFFJOIL&&h.LMPMNLLGOEC.CNOMNBCPJDI==c.OJPECMNINNA&&h.LMPMNLLGOEC.LMMNKIOAGLC==c.LMPMNLLGOEC.LMMNKIOAGLC&&h.OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None)idle.Add(h);
  if(idle.Count==0)return keep;
  // Reserve complementary specialists, not the first arbitrary N names.
  foreach(var h in idle.OrderByDescending(h=>h.GGOIKBIJNCJ()+h.JLAMCIKNBON()).Take(Math.Min(2,Math.Max(1,idle.Count/4))))keep.Add(h.OJPECMNINNA);
  var civil=idle.OrderByDescending(h=>h.FKOIGKFHHKO(true)+h.FGCBICMIMOJ(true)).FirstOrDefault();if(civil!=null)keep.Add(civil.OJPECMNINNA);
  return keep;
 }
}
