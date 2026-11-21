using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
public sealed class Localized90 {public string Text;public string INIAGDLNOAP()=>Text;}
public enum LAFAADCAHAA {DisasterReduce}
public static class MLGOLGGHLMB {public static NativeDict<string,int> CBEGDDKANAO=new();}
public enum KLMDJNIBCLD {Male,Female}
public static class UI_NotifyUI {public static int Calls;public static void OpenUI(string text,bool a,CLAGKDECFID h,bool b,bool c,object d){Calls++;}}
public partial class NJHDEHLHENN {public int[] AOKBHBMECBD={10000,20000};}
public partial class AFAGFHJONFJ {public int PopularFlow90=300,SecurityFlow90=300,PopulationFlow90=100;
 public int JKLONBNMKCL(out int a,out int b,out int c,out int d,out int e){a=b=c=d=e=0;return PopularFlow90;}
 public int MCDLJIAJMFI(out int a,out int b,out int c,out int d){a=b=c=d=0;return SecurityFlow90;}
 public int HPIFFEHIDLK(LAFAADCAHAA kind,string value)=>0;
 public int KCFNACGFBFF()=>PopulationFlow90;public Il2CppSystem.Collections.Generic.List<CLAGKDECFID> JHNBHLPJKMO=new();}
public partial class MKMOJNAJKJE {
 public void CHHLIPEMKCN(int index){var s=MEAMPLFJPNH[index].BHIBPLKOFKH;if(s.EHPDLGLPOBD>0)ADJFGBAIACI[s.OJPECMNINNA]=s.EHPDLGLPOBD;MEAMPLFJPNH.RemoveAt(index);}
 public Il2CppSystem.Collections.Generic.List<CLAGKDECFID> Captives90=new();
 public Il2CppSystem.Collections.Generic.List<CLAGKDECFID> ILBMLPCNGJI(string faction)=>Captives90;
 public bool OJFOLKOFMCE(CLAGKDECFID h,bool b)=>h!=null&&!h.GMILGFFJOIL;
}
public partial class Wo_DeployTeamCtrl { public bool m_isStatic; }
public partial class CLAGKDECFID {
 public static bool PlayerAuthority94=true;public bool BBGAPBMGDPL()=>PlayerAuthority94&&!GMILGFFJOIL&&OGPEJIEHDAN?.HFBAJIGHOAF==GCPCLLMKIMM.None;
 public KLMDJNIBCLD PMHBHENJPHA;public bool RecruitFail90;
 public static Action<CLAGKDECFID,string> RecruitCallback104;
 public void BBJBFPDCBJG(string target,bool a,bool b,bool c){if(RecruitCallback104!=null){RecruitCallback104(this,target);return;}if(RecruitFail90)return;OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.StartRecruit;OGPEJIEHDAN.LKEPGHJDHGG=target;}
}
public static class Governor90Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("R90: "+why);}
 public static void Run(){
  foreach(int size in new[]{0,1,2,4,6}){
   var cells=Enumerable.Range(0,size).Select(_=>new FEEDONDCELP()).ToArray();
   var storage=(FEEDONDCELP[])Call("ParcelStorage",(object)cells);
   Check(storage.Length>=4&&storage.Take(size).SequenceEqual(cells),"native pool capacity and all actual cells preserved");
   var plot=new PEGMABENGBB{FAFNFFJAFKH=cells};Call("RepairParcelStorage",plot);
   Check(plot.FAFNFFJAFKH.Length>=4&&plot.FAFNFFJAFKH.Take(size).SequenceEqual(cells),"short recycled plot repaired before native 2x2 write");
   for(int i=0;i<4;i++)plot.FAFNFFJAFKH[i]=new();
  }
  Check((int)Call("CapacityJourneyDays",400d,2d,10d)==20,"army route converted to days rather than personal travel cap");
  Check((int)Call("CapacityJourneyDays",2000d,2d,10d)==100,"journeys above 90 days assessed by provisions, not called missing routes");
  Check((int)Call("CapacityJourneyDays",1d,2d,10d)==1,"short valid route needs at least one day");
  foreach(var bad in new[]{0d,-1d,double.NaN,double.PositiveInfinity})Check((int)Call("CapacityJourneyDays",100d,bad,10d)==-1,"invalid army speed never creates a free journey");
  Console.WriteLine($"PASS R90 {checks} parcel-pool regression checks; native calls mocked.");
 }
}

public partial class Wo_DeployTeamCtrl {
 public static bool DirtySplit95;
 static int splitId90=1000000;
 public Wo_DeployTeamCtrl GenDivideTeam(int budget){
  if(m_listHero.Count<2)return null;
  var candidates=m_listHero.Where(h=>m_dicDeployArmy.ContainsKey(h.OJPECMNINNA)).OrderBy(h=>m_dicDeployArmy[h.OJPECMNINNA].Sum(a=>a.GNCKPEPJDGF())).ToList();
  var picked=new System.Collections.Generic.List<CLAGKDECFID>();int power=0;
  foreach(var h in candidates){int p=m_dicDeployArmy[h.OJPECMNINNA].Sum(a=>a.GNCKPEPJDGF());if(power+p>=budget*1.1)break;picked.Add(h);power+=p;}
  if(picked.Count==0||picked.Count>=m_listHero.Count)return null;
  int total=GetSoldierNum(false);var child=new Wo_DeployTeamCtrl{m_id=++splitId90,m_belongPower=m_belongPower,m_belongGroup=m_belongGroup,m_assembleCity=m_assembleCity,Position=Position,Combat=power};
  foreach(var h in picked){child.m_listHero.Add(h);child.m_dicDeployArmy[h.OJPECMNINNA]=m_dicDeployArmy[h.OJPECMNINNA];m_dicDeployArmy.Remove(h.OJPECMNINNA);m_listHero.Remove(h);}
  child.Food=(int)((long)Food*child.GetSoldierNum(false)/total);Food-=child.Food;Combat-=power;ICGAEMLACJG.HFJIECIBFKB[child.m_id]=child;
  if(DirtySplit95){child.MissingCampVisual=true;child.m_deployState=AKOPKFPKJPP.Expedition;child.m_mergeTarget=this;child.m_attackTarget=this;child.m_tmpTargetCity="pooled-old-target";child.m_targetCamp="old-camp";child.m_isStarting=true;child.m_responseToPlayer=true;}
  return child;
 }
 public void MergeToTeam(Wo_DeployTeamCtrl target){foreach(var h in m_listHero)target.m_listHero.Add(h);foreach(var kv in m_dicDeployArmy)target.m_dicDeployArmy[kv.Key]=kv.Value;target.Food+=Food;target.Combat+=Combat;Food=0;m_listHero.Clear();m_dicDeployArmy.Clear();ICGAEMLACJG.HFJIECIBFKB.Remove(m_id);}
}
