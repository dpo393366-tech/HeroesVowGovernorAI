using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[BepInPlugin("local.heroesvow.treasuryawards","Treasury Awards","1.4.3")]
public sealed partial class TreasuryAwards : BasePlugin
{
    static TreasuryAwards plugin;
    static bool busy;
    public static ConfigEntry<int> RecipientPercent;
    public override void Load()
    {
        plugin=this;
        RecipientPercent=Config.Bind("Distribution","RecipientPercent",50,new ConfigDescription("Top percentage of own-faction officers, excluding protagonist. Rounded up. Read live on every manual or automatic distribution; F12 changes need no restart.",new AcceptableValueRange<int>(1,100)));
        var h=new Harmony("local.heroesvow.treasuryawards");
        h.Patch(AccessTools.Method(typeof(UI_RewardUI),"Start"),postfix:new HarmonyMethod(typeof(TreasuryAwards),nameof(AddButton)));
        h.Patch(AccessTools.Method(typeof(UI_RewardUI),"ShowBox"),prefix:new HarmonyMethod(typeof(TreasuryAwards),nameof(BeforeBulkShowBox)));
    }
    static void ShowResult(string message){
        try{UI_NotifyUI.OpenUI("국고 분배\n"+message,true,NEINKOOBJIH.POMNMBGDDIN,false,false,null);}catch(Exception e){plugin.Log.LogError("Treasury result notice: "+e);}
    }
    static void AddButton(UI_RewardUI __instance)
    {
        const string name="TreasuryAwardsButton";
        if (__instance.transform.Find(name)) return;
        var obj=new GameObject(name,Il2CppType.Of<RectTransform>());
        var rect=obj.GetComponent<RectTransform>();
        rect.SetParent(__instance.transform,false);
        rect.anchorMin=rect.anchorMax=rect.pivot=new Vector2(1,1);
        rect.anchoredPosition=new Vector2(-90,-12);
        rect.sizeDelta=new Vector2(288,44);
        var bg=obj.AddComponent<Image>();bg.color=new Color(.07f,.19f,.22f,.96f);
        var button=obj.AddComponent<Button>();button.targetGraphic=bg;
        var labelObj=new GameObject("Label",Il2CppType.Of<RectTransform>());
        var r=labelObj.GetComponent<RectTransform>();r.SetParent(rect,false);
        r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;
        var label=labelObj.AddComponent<Text>();label.font=__instance.m_textTitle.font;
        label.fontSize=19;label.alignment=TextAnchor.MiddleCenter;label.raycastTarget=false;
        label.color=Color.white;label.text="국고 분배";
        button.onClick.AddListener((UnityAction)(Action)(()=>{
            if(!__instance.gameObject.activeInHierarchy)return;
            if(busy){ShowResult("현재 분배 처리 중입니다.");return;}
            if(UI_RewardUI.m_isDragging){ShowResult("아이템 드래그를 끝낸 뒤 다시 눌러 주세요.");return;}
            busy=true;button.interactable=false;
            var journal=new List<string>{DateTime.Now.ToString("O")+" manual-start"};auditManual=line=>journal.Add(line);
            string result;
            try{result=Run(__instance);journal.Add("manual-result "+result);}
            catch(Exception e){plugin.Log.LogError(e);journal.Add("manual-error "+e);result="오류로 분배가 중단됐습니다. 일부 지급·예약이 반영되었을 수 있습니다.\n상벌 예약칸을 확인해 주세요.";}
            finally{
                busy=false;auditManual=null;button.interactable=true;
                try{string path=System.IO.Path.Combine(Paths.BepInExRootPath,"TreasuryAwards-actions.log");if(System.IO.File.Exists(path)&&new System.IO.FileInfo(path).Length>1048576)System.IO.File.Move(path,path+".previous",true);System.IO.File.AppendAllLines(path,journal);}catch(Exception e){plugin.Log.LogError("Treasury audit write: "+e);}
            }
            ShowResult(result);

        }));
    }

    static UI_RewardUI bulkRewardUI;
    static Action<string> auditManual;
    // FinishDrag updates the global stock and then asks ShowBox to redraw its
    // original row. A row above the scroll viewport gives a NEGATIVE pooled
    // box index (row - FBFDDKOLPEI[group]); native only checks the upper bound.
    // Defer only that offscreen redraw in our bulk operation. Keep the native
    // transfer, queue, effects and final full refresh unchanged.
    static bool BeforeBulkShowBox(UI_RewardUI __instance,int __0,int __1,int __2){
        if(bulkRewardUI==null||__instance==null||bulkRewardUI.Pointer!=__instance.Pointer||UI_RewardUI.m_viewType!=0||__1!=0)return true;
        var starts=__instance.FBFDDKOLPEI;var boxes=__instance.NNIIMHLKFME;
        if(starts==null||boxes==null||starts.Length==0||boxes.Length==0||boxes[0]==null)return true;
        long local=(long)__0-starts[0];return local>=0&&local<boxes[0].Count;
    }
    static long Score(CLAGKDECFID h) => (long)h.GGOIKBIJNCJ()+h.JLAMCIKNBON()+h.GNCHNFHKBKE()
        +h.DAEKNKAKHDA()+h.DHOMPHMNCJB()+h.IHMMCAALFKG();

    static CLAGKDECFID[] Rank(MKMOJNAJKJE faction,CLAGKDECFID player)
    {
        var candidates=new Dictionary<string,CLAGKDECFID>(StringComparer.Ordinal);
        void Include(Il2CppSystem.Collections.Generic.List<CLAGKDECFID> list)
        {
            if(list==null)return;
            foreach(var h in list)
                if(h!=null && h.Pointer!=player.Pointer && h.LMPMNLLGOEC?.LMMNKIOAGLC==faction.IBGCLCAIBOL
                    && !string.IsNullOrEmpty(h.OJPECMNINNA)) candidates.TryAdd(h.OJPECMNINNA,h);
        }
        Include(UI_HeroListInfoUI.PDPLMJHLKJF); // original roster before the hero list's text filter
        Include(UI_RewardUI.ABJNKABKKJM);
        var cities=faction.PJMLPLDPEIM();
        if(cities!=null)foreach(var city in cities)
            if(city!=null)Include(city.EOMOIINOEMB(0,true,true,false,false,false,false));
        return candidates.Values.OrderByDescending(Score).ThenBy(h=>h.OJPECMNINNA,StringComparer.Ordinal)
            .Take(AwardRules.RecipientCount(candidates.Count,RecipientPercent?.Value??50)).ToArray();
    }
    sealed class Item
    {
        public string Id;
        public CMHNADEDJIE Row;
        public int Grade, Price, SlotBit, GrowthRole;
        public KKDFJCCMGHK Equipment;
    }
    static int EquipmentSlotBit(string itemId)
    {
        if(string.IsNullOrEmpty(itemId) || !AEHAGJAKNEK.GHNDPFCGKDJ.TryGetValue(itemId,out var equipment))return 0;
        var type=equipment.CIJIOJJFBEI;
        // These are the six wearing slots recognized by the native reward screen.
        return type switch {
            NGPAOHKHFPE.Weapon or NGPAOHKHFPE.Armor or NGPAOHKHFPE.Helmet or
            NGPAOHKHFPE.Treasure or NGPAOHKHFPE.Horse or NGPAOHKHFPE.Shield => 1 << (int)type,
            _ => 0
        };
    }
    static List<Item> Stock() => Stock(UI_RewardUI.m_listBagItem[0]);
    static List<Item> Stock(Il2CppSystem.Collections.Generic.List<CMHNADEDJIE> rows)
    {
        var items=new List<Item>();
        foreach(var row in rows)
        {
            if(row==null || row.NGBNIPKFPOE<=0 || string.IsNullOrEmpty(row.OJPECMNINNA))continue;
            // The treasury's sale screen uses these shared item methods across every category.
            // A factor of one gives the unit price, independent of the stockpile quantity.
            var item=new Item{Id=row.OJPECMNINNA,Row=row,SlotBit=EquipmentSlotBit(row.OJPECMNINNA),Price=row.ABLAHGCONGN(1f),Grade=(int)row.MIOJGPKDBHL()};
            if(item.SlotBit!=0&&AEHAGJAKNEK.GHNDPFCGKDJ.TryGetValue(item.Id,out var equipment))item.Equipment=equipment;
            if(AEHAGJAKNEK.EGFJHINFDCA.TryGetValue(item.Id,out var drug))item.GrowthRole=AwardRules.GrowthRole(drug.IJGILOJIOAM,drug.GDNLOKFFHDD);
            items.Add(item);
        }
        return items.OrderByDescending(x=>x.Price).ThenByDescending(x=>x.Grade)
            .ThenBy(x=>x.Id,StringComparer.Ordinal).ToList();
    }

    // Use the current, mod-aware item DB, never the JSON base price/rarity as performance.
    static AwardRules.Gear Gear(KKDFJCCMGHK e){
        var a=e.MEICJBFJGHD;var v=new List<double>();
        for(int i=0;i<6;i++)v.Add(a!=null&&a.Length>i?a[i]:0);
        v.AddRange(new double[]{e.GBGEPLKBHJB,e.LDDKGCHHEKI,e.BGAENDAPBPM,e.KBABLAPAIKM,e.MKEIPLCMANH,e.BCAIBCEODLJ,
            e.EAEEGEBAEAB,e.FJDBEOJCFFM,e.EAEJLOCCOED,e.OGCFNICFNPG,e.AGPOCEIHAJL,e.JKFDDGBCJCK,
            e.NLPMMLAOPEC,e.PDMFOJGPKDM,e.JEDLCIKNNHM,e.FIGHLFPNIPF,e.CEAINOFHOBH,e.IPDLJPNILCE,e.JOKONPKJEKN,e.LIDKLOELDOG,e.PCCCMFNNAAF});
        return new AwardRules.Gear{kind=e.EPKCKLBHPGG,values=v.ToArray(),souls=new[]{e.HDECJIENHCK,e.IGNLOFFCDEL,e.JJDNNGAOMBC}};
    }
    static bool Improves(CLAGKDECFID hero,Item item){
        if(item.Equipment==null)return true;
        var wearing=hero.LMPMNLLGOEC?.ONOGINEKIJH;
        if(wearing==null)return false; // Unavailable equipment state is not an empty slot.
        var proposed=Gear(item.Equipment);bool weapon=item.Equipment.CIJIOJJFBEI==NGPAOHKHFPE.Weapon;
        foreach(string id in wearing){
            if(string.IsNullOrEmpty(id))continue;
            if(!AEHAGJAKNEK.GHNDPFCGKDJ.TryGetValue(id,out var current))return false;
            if(current.CIJIOJJFBEI!=item.Equipment.CIJIOJJFBEI)continue;
            if(id==item.Id||!AwardRules.Upgrade(proposed,Gear(current),weapon))return false;
        }
        return true;
    }

    static void Select(UI_RewardUI ui,CLAGKDECFID hero,string pending)
    {
        UI_RewardUI.m_curHero=hero;
        UI_RewardUI.m_rewards[0]=pending;
        ui.NLIIBFOLDBD(); // Rebuild this hero's monthly slots and instant availability.
    }
    static int FirstVacancy(UI_RewardUI ui)
    {
        for(int i=ui.m_instantReward.activeSelf?0:1;i<4;i++)
            if(string.IsNullOrEmpty(UI_RewardUI.m_rewards[i]))return i;
        return -1;
    }
    static string Run(UI_RewardUI ui)
    {
        var p=NEINKOOBJIH.POMNMBGDDIN;
        var power=UI_RewardUI.m_playerPower;
        if(p==null || power==null || p.LMPMNLLGOEC?.LMMNKIOAGLC!=power.IBGCLCAIBOL)return "소속 세력 상벌에서 사용";
        var heroes=Rank(power,p);
        if(heroes.Length==0)return "분배할 소속 장수 없음";
        var flags=MLGOLGGHLMB.CBEGDDKANAO;
        if(flags==null || UI_RewardUI.m_curHero==null || UI_RewardUI.m_rewards?.Length!=4)return "상벌 정보 준비 중";
        string prefix="local.heroesvow.treasuryawards/v1/"+power.IBGCLCAIBOL+"/";
        int Read(string k)=>flags.TryGetValue(prefix+k,out int value)?value:0;
        void Write(string k,int n)=>flags[prefix+k]=n;
        int cycle=Math.Max(1,Read("cycle"));
        var initial=UI_RewardUI.m_curHero;
        string pending=UI_RewardUI.m_rewards[0];
        int page=UI_RewardUI.m_curPage,view=UI_RewardUI.m_viewType,granted=0,recipients=0;
        var previousBulk=bulkRewardUI;bulkRewardUI=ui;
        try
        {
            if(view!=0)ui.ButtonSwitchViewPress(0);
            if(page!=0)ui.PagePress(0);
            var stock=Stock();
            var recipientsGiven=new HashSet<string>();
            bool GiveOne(CLAGKDECFID hero,int pass){
                string id=hero.OJPECMNINNA;
                if(hero.LMPMNLLGOEC?.LMMNKIOAGLC!=power.IBGCLCAIBOL)return false;
                bool wasSelected=hero.Pointer==initial.Pointer;
                Select(ui,hero,wasSelected?pending:"");
                bool sameCycle=Read("epoch/"+id)==cycle;
                int count=sameCycle?Read("count/"+id):0,awarded=sameCycle?Read("slots/"+id):0,occupied=awarded;
                if(count>=20)return false;
                foreach(string reward in UI_RewardUI.m_rewards)occupied|=EquipmentSlotBit(reward);
                bool commitInstant=false;
                try{
                    foreach(var item in stock.ToArray()){
                        if(pass==0&&!AwardRules.PreferredGrowth(item.GrowthRole,hero.JLAMCIKNBON(),hero.GNCHNFHKBKE()))continue;
                        if((occupied&item.SlotBit)!=0||!Improves(hero,item))continue;
                        int slot=FirstVacancy(ui);if(slot<0)return false;
                        var rows=UI_RewardUI.m_listBagItem[0];int row=rows.IndexOf(item.Row);
                        if(row<0||item.Row.NGBNIPKFPOE<=0){stock.Remove(item);continue;}
                        int before=item.Row.NGBNIPKFPOE;
                        bool accepted=ui.FinishDrag(0,row,3,slot);
                        if(!accepted&&string.IsNullOrEmpty(UI_RewardUI.m_rewards[slot])&&rows.IndexOf(item.Row)>=0&&before==item.Row.NGBNIPKFPOE)continue;
                        if(!accepted||UI_RewardUI.m_rewards[slot]!=item.Id)throw new InvalidOperationException("Native award slot did not accept "+item.Id);
                        commitInstant=slot==0;
                        Write("slots/"+id,awarded|item.SlotBit);Write("epoch/"+id,cycle);Write("count/"+id,count+1);
                        if(rows.IndexOf(item.Row)<0||item.Row.NGBNIPKFPOE<=0)stock.Remove(item);
                        auditManual?.Invoke($"manual-award hero={id} item={item.Id} slot={slot} stockBefore={before} remaining={item.Row.NGBNIPKFPOE}");
                        recipientsGiven.Add(id);return true;
                    }
                    return false;
                }finally{if(commitInstant){ui.PIAEJBLCJGP();UI_RewardUI.m_rewards[0]="";}}
            }
            granted=DistributeRounds(heroes,cycle,Read,Write,()=>stock.Count>0,GiveOne,int.MaxValue,out bool complete);
            recipients=recipientsGiven.Count;
            return granted>0?$"{recipients}명 · 지급/예약 {granted}건":complete?"한 순회 완료 · 지급할 빈칸/물품 없음":"국고에 지급 가능한 물품 없음";
        }
        finally
        {
            bulkRewardUI=previousBulk;
            Select(ui,initial,pending);
            if(UI_RewardUI.m_curPage!=page)ui.PagePress(page);
            if(UI_RewardUI.m_viewType!=view)ui.ButtonSwitchViewPress(view);
            ui.IBPPAOHIDKL();ui.KNIEFDGIBHF();ui.HAKFBKBCENB(true);
        }
    }
}
