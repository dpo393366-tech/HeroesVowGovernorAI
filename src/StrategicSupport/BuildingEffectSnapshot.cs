using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 static BuildingEffectsPolicy.Building BuildingProfile(PABIPDOFBFG b,string district,IEnumerable<FEEDONDCELP> cells=null,long instance=0,IEnumerable<OGABIJECHFG> neighbours=null){
  var result=new BuildingEffectsPolicy.Building{Id=b.OJPECMNINNA,District=district,Instance=instance,Kind=(int)b.JCMOAFOEPDP,Special=(int)b.OFHFGEBGMBD,
   Money=b.CNCPIILGDEF,Food=b.AJJJJNDNNME,BlockMoney=b.FIPONBEGNJO,BlockFood=b.NIKFGJLNNCA,AdjMoney=b.LGCANBOEGGH,AdjFood=b.AIFPACNFKDB,StackMoney=b.OEKNLKCBAEH,StackFood=b.ACNIHJHNIEP,
   Tech=b.IIBKLLKPNEH,Strategy=b.PEGLDBENJMF,Explore=b.DJNPCHEJGNM,Recruit=b.EOJNDEDNEKE,Coming=b.LAIEKKPFOKF,Scheme=b.EMHGLEDIOHK,Population=b.DHLNIKKLNKJ,Commerce=b.CGBGIDCEBGD,Agriculture=b.EGBEEDFCIGH};
  if(cells!=null)foreach(var q in cells){if(q==null)continue;if(!result.Geometry){result.X0=result.X1=q.PAHKDHJPFKD;result.Y0=result.Y1=q.GHMMBOIKHNC;result.Geometry=true;}else{result.X0=Math.Min(result.X0,q.PAHKDHJPFKD);result.X1=Math.Max(result.X1,q.PAHKDHJPFKD);result.Y0=Math.Min(result.Y0,q.GHMMBOIKHNC);result.Y1=Math.Max(result.Y1,q.GHMMBOIKHNC);}}
  if(neighbours!=null)foreach(var n in neighbours)if(n!=null)result.Neighbours.Add(n.Pointer.ToInt64());return result;
 }
 static IEnumerable<FEEDONDCELP> BuildingCells(OGABIJECHFG x){var cells=x?.JNMLGGLLANN();if(cells!=null)foreach(var q in cells)yield return q;}
 static IEnumerable<OGABIJECHFG> BuildingNeighbours(OGABIJECHFG x){if(x?.GJLBKJOONCF!=null)foreach(var n in x.GJLBKJOONCF)yield return n;}
 static List<BuildingEffectsPolicy.Building> DistrictEffects(JNHOOCJJPJO d,IEnumerable<OGABIJECHFG> omit=null){
  var excluded=omit==null?new HashSet<IntPtr>():new HashSet<IntPtr>(omit.Select(x=>x.Pointer));var result=new List<BuildingEffectsPolicy.Building>();
  if(d?.AIMLPNBOIIF!=null)foreach(var x in d.AIMLPNBOIIF)if(x?.GEKKLMPLDHJ!=null&&!x.MGLFFEANDOI&&!excluded.Contains(x.Pointer))result.Add(BuildingProfile(x.GEKKLMPLDHJ,d.OJPECMNINNA,BuildingCells(x),x.Pointer.ToInt64(),BuildingNeighbours(x)));
  return result;
 }
 static List<BuildingEffectsPolicy.Building> CityEffects(AFAGFHJONFJ c){var result=new List<BuildingEffectsPolicy.Building>();if(c.FGBLEMGGOPO!=null)foreach(var d in c.FGBLEMGGOPO)if(d?.KFCOLLDEJNF==HNDFFIEJNHI.Public)result.AddRange(DistrictEffects(d));return result;}
 static string EffectSignature(PABIPDOFBFG b)=>$"{b.JCMOAFOEPDP}/{b.CNCPIILGDEF}/{b.AJJJJNDNNME}/{b.CGBGIDCEBGD}/{b.EGBEEDFCIGH}/{b.HKBPGJLMFIG}/{b.DPFIFIOKHEI}/{b.FIPONBEGNJO:R}/{b.NIKFGJLNNCA:R}/{b.LGCANBOEGGH:R}/{b.AIFPACNFKDB:R}/{b.OEKNLKCBAEH}/{b.ACNIHJHNIEP}/{b.IIBKLLKPNEH}/{b.PEGLDBENJMF}/{b.DJNPCHEJGNM}/{b.EOJNDEDNEKE}/{b.LAIEKKPFOKF}/{b.EMHGLEDIOHK}/{b.DHLNIKKLNKJ}/{b.OFHFGEBGMBD}";
}
