using System;
using System.Collections.Generic;
public sealed partial class StrategicSupport {
 static List<(CMHNADEDJIE Row,Il2CppSystem.Collections.Generic.List<CMHNADEDJIE> Stock)> MaterialRows(MKMOJNAJKJE f,string id){
  var rows=new List<(CMHNADEDJIE,Il2CppSystem.Collections.Generic.List<CMHNADEDJIE>)>();var seen=new HashSet<IntPtr>();
  foreach(var list in new[]{f.CNBLOJCGNFD,f.EHGOOJLIHGH,f.OILNHBFCJII,f.FCJHJPANFJN,f.OHDEBNFBGIE,f.PCGHDEOMPBO,f.OOACPLDPNHM,f.ONEHAJPKPMP,f.CJDLOOOEGPI,f.EFHPAEBIGJK,f.HFCLLCIPNBB}){
   if(list==null)continue;foreach(var row in list)if(row!=null&&row.OJPECMNINNA==id&&row.NGBNIPKFPOE>0&&seen.Add(row.Pointer))rows.Add((row,list));
  }return rows;
 }
 // The base public-city upgrade path pays cash/tech/strategy, not the
 // private-estate renovation book. Keep other mod material costs intact.
 static bool PrivateRenovationBook(PABIPDOFBFG b)=>b.OCJNCECECMH!=FFDDNPOHLHN.Private&&!string.IsNullOrEmpty(b.MDOIJIAONJM)&&b.JADLIKBKOHG=="XHP1000109";
 static bool HasMaterials(MKMOJNAJKJE f,PABIPDOFBFG b){
  if(b.GJPOJINPCHP<0)return false;if(PrivateRenovationBook(b)||string.IsNullOrEmpty(b.JADLIKBKOHG)||b.GJPOJINPCHP==0)return true;
  long count=0;foreach(var item in MaterialRows(f,b.JADLIKBKOHG))count+=item.Row.NGBNIPKFPOE;return count>=b.GJPOJINPCHP;
 }
 sealed class MaterialPayment:IDisposable {
  internal readonly List<(CMHNADEDJIE Row,int Count,Il2CppSystem.Collections.Generic.List<CMHNADEDJIE> Stock)> Rows=new();internal bool Committed;
  public void Dispose(){foreach(var v in Rows){if(!Committed)v.Row.NGBNIPKFPOE=v.Count;else if(v.Row.NGBNIPKFPOE==0)v.Stock.Remove(v.Row);}Rows.Clear();}
 }
 static MaterialPayment PayMaterials(MKMOJNAJKJE f,PABIPDOFBFG b){
  if(!HasMaterials(f,b))return null;var payment=new MaterialPayment();if(PrivateRenovationBook(b)||string.IsNullOrEmpty(b.JADLIKBKOHG)||b.GJPOJINPCHP<=0)return payment;
  try{int left=b.GJPOJINPCHP;foreach(var v in MaterialRows(f,b.JADLIKBKOHG)){int old=v.Row.NGBNIPKFPOE,take=Math.Min(old,left);payment.Rows.Add((v.Row,old,v.Stock));v.Row.NGBNIPKFPOE=old-take;left-=take;if(left==0)break;}if(left!=0)throw new InvalidOperationException("Material stock changed");return payment;}catch{payment.Dispose();throw;}
 }
 static bool SpendBuilding(AFAGFHJONFJ c,MKMOJNAJKJE f,PABIPDOFBFG b,Action execute,Func<bool> changed){
  // Re-read at commit: a sliced plan may outlive a settings change. This
  // matches native GHCOLMGCKKD's player-faction test and both point costs.
  if(NativeUpgradePointsBanned(f,b))return false;
  using(var payment=PayMaterials(f,b)){if(payment==null)return false;
   try{return SpendMaintenance(c,f,b.NPCHPDNFAME,b.ODKKMLDAPJL,b.ADGOBJJMEAO,execute,changed);}finally{payment.Committed=changed();}
  }
 }
 static bool NativeUpgradePointsBanned(MKMOJNAJKJE f,PABIPDOFBFG b)=>
  f!=null&&b!=null&&f.IBGCLCAIBOL==NEINKOOBJIH.OPIKIHPNCIP&&!GLIJMELMEHI.MIHOJGEPPID
  &&!string.IsNullOrEmpty(b.MDOIJIAONJM)&&(b.ODKKMLDAPJL>0||b.ADGOBJJMEAO>0);
}
