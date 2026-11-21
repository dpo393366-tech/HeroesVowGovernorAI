using System;
using System.Collections.Generic;
using System.Linq;
public sealed partial class StrategicSupport
{
    static void AuditCityIncome(AFAGFHJONFJ city,MKMOJNAJKJE faction){foreach(var _ in SafePhases(AuditCityIncomeSteps(city,faction))){} }
    static IEnumerable<string> AuditCityIncomeSteps(AFAGFHJONFJ city,MKMOJNAJKJE faction)
    {
        if(!Gate("income-audit:"+city.OJPECMNINNA,120)||city.FGBLEMGGOPO==null)yield break;
        {
            bool Valid()=>CurrentFaction(faction)&&city.LMPMNLLGOEC?.LMMNKIOAGLC==faction.IBGCLCAIBOL;
            if(!Valid())yield break;
            int income=TimedAI(faction,"income-native-total",()=>city.EMPGDOLPPFI()),illegal=0;
            yield return "income-total";if(!Valid())yield break;
            if(income<1000000&&!city.AKIJOBLFLCJ(NEINKOOBJIH.OPIKIHPNCIP)&&!Gate("enemy-income-detail:"+city.OJPECMNINNA,600))yield break;long baseMoney=0,baseFood=0;var descriptions=new List<string>();
            foreach(var d in city.FGBLEMGGOPO.ToArray()){if(d?.AIMLPNBOIIF==null)continue;
                var counts=new Dictionary<string,int>();long m=0,f=0;double bonus=0;
                foreach(var x in d.AIMLPNBOIIF.ToArray()){yield return "income-building";if(!Valid())yield break;var b=x?.GEKKLMPLDHJ;if(b==null)continue;
                    counts.TryGetValue(b.OJPECMNINNA,out int n);counts[b.OJPECMNINNA]=n+1;m+=b.CNCPIILGDEF;f+=b.AJJJJNDNNME;bonus+=b.FIPONBEGNJO;
                    if(d.KFCOLLDEJNF==HNDFFIEJNHI.Public&&b.OCJNCECECMH==FFDDNPOHLHN.Private)illegal++;
                }
                baseMoney+=m;baseFood+=f;yield return "income-block";if(!Valid())yield break;
                int manager=0;if(!string.IsNullOrEmpty(d.HLGFIMOKLAK)&&AEHAGJAKNEK.KDHLLDHILCO(d.HLGFIMOKLAK,out var h)&&h!=null)manager=h.FKOIGKFHHKO(true);
                descriptions.Add($"{d.OJPECMNINNA}/{d.KFCOLLDEJNF}:native={d.EMPGDOLPPFI(faction.KCKGONGIMDA,false)},base={m},food={f},bonus={bonus:F2},commerce={d.IMMLKOLPPHF()},manager={manager},buildings=[{string.Join(",",counts.Select(k=>k.Key+"*"+k.Value))}]");
            }
            if(illegal>0||income>=1000000||city.AKIJOBLFLCJ(NEINKOOBJIH.OPIKIHPNCIP)){
                Note($"income-audit city={city.OJPECMNINNA} faction={faction.IBGCLCAIBOL} income={income} directMoney={baseMoney} directFood={baseFood} privateOnlyInPublic={illegal} factionBonus={faction.IJJDBPGOJJA((COGDLKNKJGA)15,city)} cityBonus={city.EHCCLNDFJGL((HLIMKFFGEPO)12)}");
                foreach(var line in descriptions)Note("income-block city="+city.OJPECMNINNA+" "+line);
            }
        }
    }
}
