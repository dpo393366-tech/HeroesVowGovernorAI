using System;using System.Collections.Generic;using System.Linq;
namespace Vow.Settings {
 internal sealed class SettingsRegistry {
  internal sealed class Tab {internal string key,title;internal Action draw;}
  internal readonly List<Tab> tabs=new List<Tab>();
  internal string selected="R";
  internal bool open;
  internal void Add(string key,string title,Action draw){
   if(draw==null||tabs.Any(t=>t.key==key))return;
   tabs.Add(new Tab{key=key,title=title,draw=draw});
   const string order="RWEG";tabs.Sort((a,b)=>Rank(order,a.key).CompareTo(Rank(order,b.key)));
   if(!tabs.Any(t=>t.key==selected))selected=tabs[0].key;
  }
  static int Rank(string order,string key){int i=order.IndexOf(key,StringComparison.Ordinal);return i<0?100:i;}
  internal bool Select(string key){if(!tabs.Any(t=>t.key==key))return false;selected=key;return true;}
  internal void DrawSelected(){if(open)tabs.FirstOrDefault(t=>t.key==selected)?.draw();}
 }
 internal readonly struct SettingsLayout {
  internal readonly float x,y,width,height,contentY,contentHeight,tabWidth;
  internal readonly int columns,rows;
  internal SettingsLayout(int screenWidth,int screenHeight,int count){
   width=Math.Min(720,Math.Max(180,screenWidth-24));height=Math.Min(610,Math.Max(180,screenHeight-40));
   x=(screenWidth-width)*.5f;y=(screenHeight-height)*.5f;
   columns=Math.Max(1,Math.Min(4,(int)((width-20)/145)));rows=(count+columns-1)/columns;
   tabWidth=(width-20)/columns;contentY=42+rows*34;contentHeight=height-contentY-12;
  }
 }
}
