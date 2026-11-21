using System;
using System.Collections.Generic;
using System.Reflection;
public partial class HeroRow{public int JAAMIHKIMKP=-1,DOGMGCIADDE;}
public partial class CLAGKDECFID{
 public static bool RejectMilitary,MilitaryWorking=true; public void KFNJDLCBLDN(OEBGIDGEIIG d,bool b){if(d==OEBGIDGEIIG.Military&&RejectMilitary)return;BAPIJIJLLCA.HFBAJIGHOAF=d;MGLFFEANDOI=d!=OEBGIDGEIIG.Military||MilitaryWorking;}
 public int HPIBIDDIKNN(int n)=>GNCHNFHKBKE();public int NOHAMLJFMPF()=>50;
}
public class UI_OfficialTitleUI:Native{public static implicit operator bool(UI_OfficialTitleUI t)=>t!=null;public static UI_OfficialTitleUI IJGEDOMNOBK;public UnityEngine.GameObject gameObject=new();}
public class ILKODCJOMCI{public int AHPHHHONKPI,KOFJGOLGLAM;public bool JJOCEDGFKOC(CLAGKDECFID h)=>true;}
public static partial class AEHAGJAKNEK{public static List<ILKODCJOMCI> NHBPCOGKDML=new();}
public static class AdministrationTests{
 public static void Run(){
  var type=typeof(StrategicSupport);var instance=new StrategicSupport();instance.Load();
  object Call(string n,params object[] a)=>type.GetMethod(n,BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,a);
  void Check(bool ok,string name){if(!ok)throw new Exception(name);}
  var player=new CLAGKDECFID{OJPECMNINNA="player",LMPMNLLGOEC=new(){LMMNKIOAGLC="P"}};NEINKOOBJIH.POMNMBGDDIN=player;
  NEINKOOBJIH.OPIKIHPNCIP="player";var f=new MKMOJNAJKJE{IBGCLCAIBOL="P"};var g=new FNFJODHIJFO{LMMNKIOAGLC="P"};f.ANMMMOIKLEG["AI"]=g;NEINKOOBJIH.COCMABJFOFH["P"]=f;
  var c=new AFAGFHJONFJ{OJPECMNINNA="C",LMPMNLLGOEC=new(){LMMNKIOAGLC="P",IPJHOOJBCKI="AI",BKENJPNKMGI="player"}};
  Check((bool)Call("ExplicitDelegation",c),"AI governor / player mayor regression");
  c.LMPMNLLGOEC.IPJHOOJBCKI="player";Check(!(bool)Call("ExplicitDelegation",c),"personal governor preserved");
  c.LMPMNLLGOEC.IPJHOOJBCKI="missing";Check(!(bool)Call("ExplicitDelegation",c),"invalid governor rejected");
  c.LMPMNLLGOEC.IPJHOOJBCKI="AI";c.LMPMNLLGOEC.LMMNKIOAGLC="enemy";Check(!(bool)Call("ExplicitDelegation",c),"wrong owner rejected");
  c.LMPMNLLGOEC.LMMNKIOAGLC="P";
  var workers=new List<CLAGKDECFID>();for(int i=0;i<12;i++)workers.Add(new(){OJPECMNINNA="h"+i,LMPMNLLGOEC=new(){LMMNKIOAGLC="P",CNOMNBCPJDI="C"},MGLFFEANDOI=true,BAPIJIJLLCA=new(){HFBAJIGHOAF=OEBGIDGEIIG.Explore}});
  workers[0].OGPEJIEHDAN.HFBAJIGHOAF=GCPCLLMKIMM.Technology;workers[1].BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Military;workers[2].BAPIJIJLLCA.HFBAJIGHOAF=OEBGIDGEIIG.Security;
  c.Roster=workers;Call("AssignDomestic",c,f);
  Check(workers[0].OGPEJIEHDAN.HFBAJIGHOAF==GCPCLLMKIMM.Technology,"research protected");
  Check(workers[1].BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Military,"defense protected");
  Check(workers[2].BAPIJIJLLCA.HFBAJIGHOAF==OEBGIDGEIIG.Security,"security protected");
  int changed=0;for(int i=3;i<workers.Count;i++)if(workers[i].BAPIJIJLLCA.HFBAJIGHOAF!=OEBGIDGEIIG.Explore)changed++;
  Check(changed>0&&changed<=2,"bounded domestic changes");
  AEHAGJAKNEK.IEOHLJLICOA.Clear();foreach(var h in workers)AEHAGJAKNEK.IEOHLJLICOA[h.OJPECMNINNA]=h;
  workers[0].LMPMNLLGOEC.JAAMIHKIMKP=0;AEHAGJAKNEK.NHBPCOGKDML=new(){new(),new(),new()};Call("AssignTitles",f);
  Check(workers[0].LMPMNLLGOEC.JAAMIHKIMKP==0,"existing title protected");
  var ids=new HashSet<int>();foreach(var h in workers)if(h.LMPMNLLGOEC.JAAMIHKIMKP>=0)Check(ids.Add(h.LMPMNLLGOEC.JAAMIHKIMKP),"unique title");
  Console.WriteLine("PASS administration delegation, domestic staffing, research/defense preservation and title uniqueness");
 }
}
