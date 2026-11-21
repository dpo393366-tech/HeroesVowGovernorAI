using System;
using System.Collections.Generic;

public sealed partial class StrategicSupport
{
    sealed class BudgetWindow{public long Until;public int RecruitmentCap,ConstructionCap,ConstructionSpent;}
    static readonly Dictionary<string,BudgetWindow> budgets=new();
    static BudgetWindow Budget(MKMOJNAJKJE f){
        if(!budgets.TryGetValue(f.IBGCLCAIBOL,out var b)||Environment.TickCount64>=b.Until){
            long spare=Math.Max(0L,(long)f.DNOGPFHMOMO-reserveCash.Value);
            budgets[f.IBGCLCAIBOL]=b=new(){Until=Environment.TickCount64+10000,
                RecruitmentCap=(int)Math.Min(cityLimit.Value,spare*budgetShare.Value),
                ConstructionCap=(int)Math.Min(int.MaxValue,Math.Max(0L,(long)f.DNOGPFHMOMO-investmentTreasuryReserve.Value))};
        }
        return b;
    }
    static int InvestmentReserve(MKMOJNAJKJE f){var b=Budget(f);return Math.Max(SharedReserve(f),f.DNOGPFHMOMO-Math.Max(0,b.ConstructionCap-b.ConstructionSpent));}
    static bool FoodEmergency(AFAGFHJONFJ c){
        long soldiers=0;if(c.LMPMNLLGOEC.KJPPGIOCDID!=null)foreach(var a in c.LMPMNLLGOEC.KJPPGIOCDID)if(a!=null)soldiers+=Math.Max(0,a.FGHFOLKBHDL);
        return c.LMPMNLLGOEC.OEGNGFBCHFL<Math.Max(1,soldiers*5);
    }
}
