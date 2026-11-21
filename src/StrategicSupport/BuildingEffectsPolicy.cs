using System;using System.Collections.Generic;using System.Linq;
// Public-building structural outputs. Faction/tax/manager multipliers remain
// native; Utility is an explicit planning preference, NOT monetary income.
public static class BuildingEffectsPolicy {
 public sealed class Building {
  public string Id,District;public long Instance;public HashSet<long> Neighbours=new();
  public int X0,X1,Y0,Y1;public bool Geometry;public int Kind,Special;
  public double Money,Food,BlockMoney,BlockFood,AdjMoney,AdjFood,Tech,Strategy,Explore,Recruit,Coming,Scheme,Population,Commerce,Agriculture;
  public bool StackMoney,StackFood;
 }
 public readonly struct Output {public readonly double Money,Food,Utility;public double Value=>Money+Food/4+Utility;public Output(double m,double f,double u){Money=m;Food=f;Utility=u;}}
 public static bool Adjacent(Building a,Building b){
  if(a.District!=b.District||ReferenceEquals(a,b))return false;
  if(a.Instance!=0&&b.Instance!=0)return a.Neighbours.Contains(b.Instance)||b.Neighbours.Contains(a.Instance);
  if(!a.Geometry||!b.Geometry)return false;
  return ((a.X1+1==b.X0||b.X1+1==a.X0)&&a.Y0<=b.Y1&&b.Y0<=a.Y1)||((a.Y1+1==b.Y0||b.Y1+1==a.Y0)&&a.X0<=b.X1&&b.X0<=a.X1);
 }
 static double Trunc(double value)=>double.IsFinite(value)?Math.Truncate(value):0;
 public static double Utility(Building b)=>b.Tech*100+b.Strategy*50+b.Explore*20+b.Recruit*20+b.Coming*20+b.Scheme*20;
 public static Output Evaluate(IReadOnlyList<Building> buildings,bool includeSpecial=true){
  double money=0,food=0,utility=0;var districts=new Dictionary<string,List<Building>>();var special=new HashSet<int>();
  foreach(var b in buildings){if(!districts.TryGetValue(b.District??"",out var list))districts[b.District??""]=list=new();list.Add(b);utility+=Utility(b);if(includeSpecial&&b.Special>0&&special.Add(b.Special))utility+=100;}
  // One reusable set per evaluation instead of one allocation per recipient
  // and a LINQ source array per district, across thousands of layout probes.
  var seen=new HashSet<string>();var sources=new List<Building>();
  foreach(var list in districts.Values){sources.Clear();foreach(var source in list)if(source.AdjMoney>=.01f||source.AdjFood>=.01f)sources.Add(source);
   double dm=0,df=0;foreach(var recipient in list){dm+=recipient.Money;df+=recipient.Food;seen.Clear();
   foreach(var source in sources){if(!Adjacent(source,recipient))continue;
    // Native IDJHFHGJHDD: positive money adjacency applies only to Market(6)
    // and BigMarket(61); a dual-effect source skips other recipient kinds.
    if(source.AdjMoney>=.01f){if(recipient.Kind!=6&&recipient.Kind!=61)continue;if(source.StackMoney||seen.Add(source.Id??""))dm+=Trunc((float)recipient.Money*(float)source.AdjMoney);}
    // Native shares the per-recipient definition-ID set across both channels.
    if(source.AdjFood>=.01f&&(source.StackFood||seen.Add(source.Id??"")))df+=Trunc((float)recipient.Food*(float)source.AdjFood);
   }
  }
  // Native applies each district multiplier to the running total, truncating
  // each increment. Two +50% facilities produce 2.25x, not 2x.
  foreach(var b in list){if(b.BlockMoney>=.01f)dm+=Trunc((float)dm*(float)b.BlockMoney);if(b.BlockFood>=.01f)df+=Trunc((float)df*(float)b.BlockFood);}
  money+=dm;food+=df;
  }
  return new(money,food,utility);
 }
 public static bool PreservesUtility(Building a,Building b)=>b.Tech>=a.Tech&&b.Strategy>=a.Strategy&&b.Explore>=a.Explore&&b.Recruit>=a.Recruit&&b.Coming>=a.Coming&&b.Scheme>=a.Scheme&&b.Population>=a.Population&&(a.Special==0||b.Special==a.Special)&&(!a.StackMoney||b.StackMoney||a.AdjMoney==0)&&(!a.StackFood||b.StackFood||a.AdjFood==0);
}
