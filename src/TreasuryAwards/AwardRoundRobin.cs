using System;
using System.Linq;

public sealed partial class TreasuryAwards
{
 // Keep v1 cycle/count/slot history. A separate persisted turn marks ONE
 // grant within a round, shared by the manual UI and automatic allocation.
 static int DistributeRounds(CLAGKDECFID[] heroes,int cycle,Func<string,int> read,Action<string,int> write,
     Func<bool> hasStock,Func<CLAGKDECFID,int,bool> grant,int limit,out bool complete)
 {
  complete=false;int total=0;
  int round=read("rr-epoch")==cycle?Math.Max(1,read("rr-round")):1;
  write("rr-epoch",cycle);write("rr-round",round);
  bool Done(CLAGKDECFID h)=>read("seen/"+h.OJPECMNINNA)==cycle;
  for(int sweep=0;sweep<21;sweep++){
   // Matching growth items go to their intended role before fallback items,
   // but receiving one in either pass consumes this officer's whole turn.
   for(int pass=0;pass<2;pass++)foreach(var hero in heroes){
    string id=hero.OJPECMNINNA;
    if(Done(hero)||(read("rr-epoch/"+id)==cycle&&read("rr-turn/"+id)==round))continue;
    if(!hasStock()||total>=limit)return total;
    int before=read("epoch/"+id)==cycle?read("count/"+id):0;
    bool accepted=false;
    try{accepted=grant(hero,pass);}
    finally{
     // A native immediate effect can throw after stock/count committed.
     // Preserve its turn rather than granting a second item after retry.
     if(accepted||(read("epoch/"+id)==cycle&&read("count/"+id)>before)){
      write("rr-epoch/"+id,cycle);write("rr-turn/"+id,round);
     }
    }
    if(accepted){total++;if(total>=limit)return total;}
    else if(pass==1)write("seen/"+id,cycle); // no vacancy or suitable item
   }
   if(heroes.All(Done)){
    complete=true;if(cycle<int.MaxValue)write("cycle",cycle+1);return total;
   }
   if(!hasStock())return total;
   // All remaining recipients had an opportunity. Then start the next lap.
   if(round==int.MaxValue)return total;
   round++;write("rr-round",round);
  }
  return total;
 }
}
