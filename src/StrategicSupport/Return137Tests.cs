using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Reflection;using UnityEngine;
public static class Return137Tests {
 const BindingFlags F=BindingFlags.Static|BindingFlags.NonPublic;static int n;static Wo_DeployTeamCtrl army;
 static object Call(string name,params object[] a)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,a);
 static void Check(bool ok,string why){n++;if(!ok)throw new Exception("Return137: "+why);}
 static void Setup(int[] counts,int[] powers){
  typeof(Campaign81Tests).GetMethod("Setup",F).Invoke(null,null);Call("ResetWork");Call("ResetDefense");
  army=(Wo_DeployTeamCtrl)typeof(Campaign81Tests).GetField("t",F).GetValue(null);army.m_listHero.Clear();army.m_dicDeployArmy.Clear();army.m_targetCity="";army.Food=9000000;army.Combat=0;Wo_DeployTeamCtrl.SplitCalls137=0;
  for(int i=0;i<counts.Length;i++){
   var h=new CLAGKDECFID{OJPECMNINNA="return137-"+i,SplitOrder137=i};army.m_listHero.Add(h);army.m_dicDeployArmy[h.OJPECMNINNA]=new();
   for(int j=0;j<counts[i];j++){army.m_dicDeployArmy[h.OJPECMNINNA].Add(new(){FGHFOLKBHDL=1000,CombatValue=powers[i]});army.Combat+=powers[i];}
  }
 }
 static int Units(Wo_DeployTeamCtrl t)=>((HashSet<IntPtr>)Call("TeamTroops",t)).Count;
 public static void Run(){
  Setup(new[]{7,6,2},new[]{10000,1000,1000000});int soldiers=army.GetSoldierNum(false),food=army.GetFood();
  long plan=(long)Call("ReturnSplitPower",army,10);var child=(Wo_DeployTeamCtrl)Call("SplitFieldArmy",army,plan);
  Check(child!=null&&Units(child)==7&&Wo_DeployTeamCtrl.SplitCalls137==1,"unequal-power groups fit ten slots on first native split, not thirteen slots followed by halving");
  Check(army.GetSoldierNum(false)+child.GetSoldierNum(false)==soldiers&&army.GetFood()+child.GetFood()==food,"troops and food conserved");
  child.MergeToTeam(army);Check(army.m_listHero.Count==3&&Units(army)==15,"rollback preserves all officers and unit references");
  Setup(new[]{11,2,2},new[]{10,10000,10000});Check((long)Call("ReturnSplitPower",army,10)==0&&Wo_DeployTeamCtrl.SplitCalls137==0,"native indivisible first group is rejected without repeated split/merge");
  Setup(new[]{2,3,4},new[]{100,10000,1000});Check((long)Call("ReturnSplitPower",army,0)==0,"zero destination capacity cannot create child");
  var random=new Random(137);
  for(int k=0;k<160;k++){
   var counts=Enumerable.Range(0,random.Next(2,12)).Select(_=>random.Next(1,9)).ToArray();var powers=counts.Select(_=>random.Next(1,100000)).ToArray();Setup(counts,powers);int room=random.Next(1,counts.Sum());
   int expect=0;for(int i=0;i<counts.Length-1;i++){if(expect+counts[i]>room)break;expect+=counts[i];}
   var original=((HashSet<IntPtr>)Call("TeamTroops",army));soldiers=army.GetSoldierNum(false);food=army.Food;plan=(long)Call("ReturnSplitPower",army,room);
   if(expect==0){Check(plan==0&&Wo_DeployTeamCtrl.SplitCalls137==0,"infeasible prefix has no native mutation "+k);continue;}
   child=(Wo_DeployTeamCtrl)Call("SplitFieldArmy",army,plan);Check(child!=null&&Units(child)==expect&&Units(child)<=room,"real whole-group count follows native order "+k);
   var left=(HashSet<IntPtr>)Call("TeamTroops",army);var moved=(HashSet<IntPtr>)Call("TeamTroops",child);Check(!left.Overlaps(moved),"no duplicate unit references "+k);left.UnionWith(moved);
   Check(left.SetEquals(original)&&army.GetSoldierNum(false)+child.GetSoldierNum(false)==soldiers&&army.Food+child.Food==food,"all resources conserved "+k);
  }
  Call("ResetWork");Console.WriteLine($"PASS Return137 {n} capacity-aware native split planning checks across 160 generated armies; native order/power boundary mocked.");
 }
}
