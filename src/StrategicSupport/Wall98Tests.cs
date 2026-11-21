using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;
public static class Wall98Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
 static void Check(bool ok,string message){checks++;if(!ok)throw new Exception("Wall98: "+message);}
 public static void Run(){
  typeof(CityIntegrationTests).GetMethod("Setup",F).Invoke(null,null);
  var c=(AFAGFHJONFJ)typeof(CityIntegrationTests).GetField("c",F).GetValue(null);
  var f=(MKMOJNAJKJE)typeof(CityIntegrationTests).GetField("f",F).GetValue(null);
  c.FGBLEMGGOPO.Clear();var d=new JNHOOCJJPJO{OJPECMNINNA="wall",AJJCCPCJPFC=c,PLIFMOIBPME=true,FAFNFFJAFKH=new()};c.FGBLEMGGOPO.Add(d);
  var row=new Il2CppSystem.Collections.Generic.List<FEEDONDCELP>();d.FAFNFFJAFKH.Add(row);
  var tower=new PABIPDOFBFG{OJPECMNINNA="JZ40009",HFBAJIGHOAF=MHCEFEODENB.Wall,MLAJLJKFGBD=1,PNPLBNAFMCM=1,NPCHPDNFAME=1000,CNCPIILGDEF=0};AEHAGJAKNEK.ACIGFKMDOKE[tower.OJPECMNINNA]=tower;
  for(int j=0;j<15;j++){var tile=new FEEDONDCELP{GMIKIMACDBC=d,HFBAJIGHOAF=1,PMGDGAAICCG=j%4+1,PAHKDHJPFKD=j};row.Add(tile);if(j<12)d.FPFEFLILLFN(new OGABIJECHFG{OJPECMNINNA=j+1,GEKKLMPLDHJ=tower,Cells=new[]{tile}},false);}
  d.BBJBFAFMCCB(false);Check(d.HGNFEMJOPPB.Count==0,"native wall generator returns without ordinary parcels");
  var plans=((IEnumerable)Call("RebuildPlans",c,d,tower,true)).Cast<object>().Where(x=>x!=null).ToList();Check(plans.Count==3,"all three empty sockets found despite twelve existing towers and no borders");
  var original=row.Take(12).Select(x=>x.FCOJFBEHMAG).ToArray();int cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
  for(int j=0;j<3;j++)Check((bool)Call("BuildBest",c,f),"each legal wall vacancy built through production planner");
  Check(!(bool)Call("BuildBest",c,f),"filled wall does not build overlapping duplicate");
  Check(d.AIMLPNBOIIF.Count==15&&row.All(x=>x.FCOJFBEHMAG!=-9999),"fifteen sockets occupied after three registrations");
  Check(original.SequenceEqual(row.Take(12).Select(x=>x.FCOJFBEHMAG)),"existing twelve towers unchanged");
  Check(cash-f.DNOGPFHMOMO-c.LMPMNLLGOEC.DNOGPFHMOMO==3000,"three actual native registrations charged once each");
  for(int side=1;side<=4;side++)Check((int)Call("WallSocketDirection",side)==new[]{3,1,2,0}[side-1],"side encoding converted to building orientation");
  Check((int)Call("WallSocketDirection",0)<0,"unknown wall direction rejected");
  var book=new PABIPDOFBFG{MDOIJIAONJM="JZ40001",JADLIKBKOHG="XHP1000109",GJPOJINPCHP=1};
  Check((bool)Call("HasMaterials",f,book),"public barracks upgrade does not require private estate book");
  using((IDisposable)Call("PayMaterials",f,book)){}
  book.OCJNCECECMH=FFDDNPOHLHN.Private;Check(!(bool)Call("HasMaterials",f,book),"private estate keeps book requirement");
  book.OCJNCECECMH=FFDDNPOHLHN.Public;book.JADLIKBKOHG="mod-ore";Check(!(bool)Call("HasMaterials",f,book),"other mod materials are not waived");
  tower.LJCICFHPMLM[0]=15;Check(!(bool)Call("CanBuild",c,f,tower,10000),"native building cap preserved");
  Console.WriteLine($"PASS Wall98 {checks} recorded 12+3 wall vacancies, build/cost/occupancy and public upgrade materials; native actions mocked.");
 }
}
