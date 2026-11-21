using System;using System.Collections.Generic;using System.Linq;using System.Reflection;
public static class Button106Tests {
 const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
 static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("Button106: "+why);}
 static object Fixture(string name,params object[] args)=>typeof(RoundRobinTests).GetMethod(name,F).Invoke(null,args);
 static UI_RewardUI Setup(){Fixture("Setup",1,false);Fixture("Stock","medicine",4,1);var ui=(UI_RewardUI)typeof(RoundRobinTests).GetField("ui",F).GetValue(null);ui.FBFDDKOLPEI[0]=7;ui.NNIIMHLKFME[0]=new(){new(),new(),new(),new()};UI_RewardUI.Virtualized106=true;return ui;}
 static bool Guard(UI_RewardUI ui,int row,int group=0)=>(bool)typeof(TreasuryAwards).GetMethod("BeforeBulkShowBox",F).Invoke(null,new object[]{ui,row,group,row});
 public static void Run(){
  var ui=Setup();UI_RewardUI.BypassGuard106=true;bool reproduced=false;
  try{Fixture("Manual");}catch(TargetInvocationException e){reproduced=e.ToString().Contains("ArgumentOutOfRangeException");}
  Check(reproduced,"native missing lower-bound check reproduces scrolled inventory failure");
  Check(UI_RewardUI.m_listBagItem[0][0].NGBNIPKFPOE==3,"failure occurred AFTER stock debit; retry cannot assume nothing happened");
  ui=Setup();UI_RewardUI.BypassGuard106=false;Fixture("Manual");
  Check(UI_RewardUI.m_listBagItem[0][0].NGBNIPKFPOE==1&&UI_RewardUI.m_curHero.queue.Count(x=>x=="medicine")==3,"offscreen draw deferral preserves all three native reservations and exact debit");
  Check(typeof(TreasuryAwards).GetField("bulkRewardUI",F).GetValue(null)==null,"bulk context restored after completion");
  Check(Guard(ui,0),"normal manual dragging outside bulk scope remains native");
  typeof(TreasuryAwards).GetField("bulkRewardUI",F).SetValue(null,ui);
  try{Check(!Guard(ui,6)&&Guard(ui,7)&&Guard(ui,10)&&!Guard(ui,11),"only offscreen source rows are deferred");Check(Guard(ui,0,3),"reward destination rendering remains native");UI_RewardUI.m_viewType=1;Check(Guard(ui,0),"other view remains native");}
  finally{typeof(TreasuryAwards).GetField("bulkRewardUI",F).SetValue(null,null);UI_RewardUI.m_viewType=0;UI_RewardUI.Virtualized106=UI_RewardUI.BypassGuard106=false;}
  Console.WriteLine("PASS Button106 "+checks+" native-scroll failure reproduction, reservation conservation and scoped redraw checks; UI mocked.");
 }
}
