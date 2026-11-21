using System;using System.Collections.Generic;using System.Linq;
public sealed partial class StrategicSupport {
 // Saved per faction, using stable IDs. Only the head owns the existing native
 // mission ledger; no second item is activated while old missions are pending.
 const int RecruitRunning=1,RecruitPaused=2,RecruitClosing=3,RecruitDone=4,RecruitCancelled=5;
 static string RecruitItem(string root,int id)=>root+"queue/item/"+id+"/";
 static IntPtr recruitListSave;static string recruitListRoot;static int recruitListRevision=-1;static List<int> recruitListIds=new();
 static List<int> RecruitIds(string root){
  var save=MLGOLGGHLMB.CBEGDDKANAO;if(save==null)return new();int revision=QueueRead(root+"queue/revision");
  if(recruitListSave!=save.Pointer||recruitListRoot!=root||recruitListRevision!=revision){
   string prefix=root+"queue/item/";var ids=new List<int>();
   foreach(var kv in save)if(kv.Key.StartsWith(prefix,StringComparison.Ordinal)&&kv.Key.EndsWith("/goal",StringComparison.Ordinal)&&int.TryParse(kv.Key.Substring(prefix.Length,kv.Key.Length-prefix.Length-5),out int id)&&id>0)ids.Add(id);
   ids.Sort();recruitListIds=ids;recruitListSave=save.Pointer;recruitListRoot=root;recruitListRevision=revision;
  }return recruitListIds;
 }
 static void EnsureRecruitList(string root){
  if(QueueRead(root+"queue/schema")==1)return;
  int gen=QueueRead(root+"generation"),goal=QueueRead(root+"goal");
  if(gen>0&&goal>0){string item=RecruitItem(root,gen);foreach(string field in new[]{"goal","gender","done","pending"})QueueWrite(item+field,QueueRead(root+field));
   int status=QueueRead(root+"done")>=goal&&QueueRead(root+"pending")==0?RecruitDone:QueueRead(root+"enabled")==1?RecruitRunning:RecruitPaused;
   QueueWrite(item+"status",status);QueueWrite(root+"queue/active",status==RecruitDone?0:gen);QueueWrite(root+"queue/last",gen);
  }
  QueueWrite(root+"queue/revision",QueueRead(root+"queue/revision")+1);QueueWrite(root+"queue/schema",1);
 }
 static int RecruitHead(string root)=>RecruitIds(root).FirstOrDefault(id=>{int s=QueueRead(RecruitItem(root,id)+"status");return s>=RecruitRunning&&s<=RecruitClosing;});
 static bool ActivateRecruitHead(string root){
  int id=RecruitHead(root);if(id==0)return false;if(QueueRead(root+"queue/active")==id)return true;
  // Old head can only advance after its results have been settled.
  if(QueueRead(root+"pending")>0)return false;
  string item=RecruitItem(root,id);QueueWrite(root+"queue/active",id);QueueWrite(root+"generation",id);
  foreach(string field in new[]{"goal","gender","done","pending"})QueueWrite(root+field,QueueRead(item+field));
  QueueWrite(root+"notified",0);QueueWrite(root+"enabled",QueueRead(item+"status")==RecruitRunning?1:0);return true;
 }
 static void WakeRecruitment(){
  string faction=NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC;if(string.IsNullOrEmpty(faction))return;
  decisionStamps.Remove("recruit-queue:"+faction);if(NEINKOOBJIH.COCMABJFOFH.TryGetValue(faction,out var f))ScheduleRecruitment(f);
 }
 static bool RecruitListReady(out string root){root=null;if(!Ready()||MLGOLGGHLMB.CBEGDDKANAO==null||string.IsNullOrEmpty(NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC))return false;root=PlayerRecruitKey();EnsureRecruitList(root);return true;}
 public static string StartRecruitmentQueue(int amount,int gender){
  if(!RecruitListReady(out string root))return "저장을 불러오고 소속 세력을 확인해 주세요.";
  if(amount<1||amount>1000||gender<0||gender>2)return "등용 인원은 1~1000명입니다.";
  int last=Math.Max(QueueRead(root+"queue/last"),QueueRead(root+"generation"));if(last==int.MaxValue)return "예약 번호를 더 만들 수 없습니다.";
  int id=last+1;string item=RecruitItem(root,id);QueueWrite(item+"goal",amount);QueueWrite(item+"gender",gender);QueueWrite(item+"status",RecruitRunning);
  QueueWrite(root+"queue/last",id);QueueWrite(root+"queue/revision",QueueRead(root+"queue/revision")+1);ActivateRecruitHead(root);WakeRecruitment();
  return "예약 #"+id+" 등록 · 위에서부터 순서대로 실행합니다.";
 }
 public static string SetRecruitmentQueuePaused(int id,bool pause){
  if(!RecruitListReady(out string root)||!RecruitIds(root).Contains(id))return "해당 예약이 없습니다.";
  string item=RecruitItem(root,id);int status=QueueRead(item+"status");if(status!=RecruitRunning&&status!=RecruitPaused)return "종료된 예약은 재개할 수 없습니다.";
  QueueWrite(item+"status",pause?RecruitPaused:RecruitRunning);if(QueueRead(root+"queue/active")==id)QueueWrite(root+"enabled",pause?0:1);WakeRecruitment();return pause?"예약을 중지했습니다. 뒤 예약도 순서를 기다립니다.":"예약을 재개했습니다.";
 }
 public static void PauseRecruitmentQueue(bool pause){if(RecruitListReady(out string root)){int id=RecruitHead(root);if(id>0)SetRecruitmentQueuePaused(id,pause);}}
 public static string CancelRecruitmentQueue(int id){
  if(!RecruitListReady(out string root)||!RecruitIds(root).Contains(id))return "해당 예약이 없습니다.";
  string item=RecruitItem(root,id);int status=QueueRead(item+"status");
  if(status==RecruitDone||status==RecruitCancelled){QueueWrite(item+"hidden",1);return "목록에서 제거했습니다.";}
  if(QueueRead(root+"queue/active")==id){
   // Keep a visible draining row until native orders finish. Cancel means no
   // further dispatch; never force-reset an officer's real job or reassign it.
   QueueWrite(item+"status",RecruitClosing);QueueWrite(root+"enabled",0);
   bool outstanding=false;string prefix=root+"mission/";foreach(var record in MLGOLGGHLMB.CBEGDDKANAO)if(record.Value==id&&record.Key.StartsWith(prefix,StringComparison.Ordinal)){outstanding=true;break;}
   if(QueueRead(root+"pending")==0&&!outstanding){QueueWrite(item+"status",RecruitCancelled);QueueWrite(item+"hidden",1);QueueWrite(root+"queue/active",0);ActivateRecruitHead(root);WakeRecruitment();return "예약을 종료했습니다.";}
   WakeRecruitment();return "종료 요청 · 진행 중 임무를 정리한 뒤 다음 예약을 실행합니다.";
  }
  QueueWrite(item+"status",RecruitCancelled);QueueWrite(item+"hidden",1);WakeRecruitment();return "대기 중인 예약을 종료했습니다.";
 }
 public static string[][] RecruitmentQueueRows(){
  if(!RecruitListReady(out string root))return Array.Empty<string[]>();int head=RecruitHead(root),active=QueueRead(root+"queue/active");var rows=new List<string[]>();
  foreach(int id in RecruitIds(root)){string item=RecruitItem(root,id);if(QueueRead(item+"hidden")!=0)continue;int status=QueueRead(item+"status"),pending=QueueRead(id==active?root+"pending":item+"pending"),done=QueueRead(id==active?root+"done":item+"done");
   string label=status==RecruitDone?"완료":status==RecruitCancelled?"종료":status==RecruitClosing?"종료 중 · 기존 임무 정리":status==RecruitPaused?(id==head?"중지됨 · 다음 예약 대기":"중지된 예약"):id==head?(pending>0?"실행 중 · 등용 임무 진행":"실행 중 · 후보/파견 조건 확인"):"대기 · 앞 예약 완료 후 실행";
   rows.Add(new[]{id.ToString(),QueueRead(item+"gender")==1?"남성만":QueueRead(item+"gender")==2?"여성만":"전체",done+" / "+QueueRead(item+"goal")+"명",pending.ToString(),label,status==RecruitPaused?"재개":status==RecruitRunning?"중지":"",status.ToString()});
  }return rows.ToArray();
 }
 public static string RecruitmentQueueStatus(){
  if(!RecruitListReady(out string root))return "저장을 불러온 뒤 사용할 수 있습니다.";
  int head=RecruitHead(root);return head==0?"실행할 예약이 없습니다.":"순차 실행 · 현재 예약 #"+head+" · 아래 예약은 앞 예약 종료까지 대기";
 }
 static IEnumerable<string> RecruitmentQueueSteps(MKMOJNAJKJE f){
  if(f==null||f.IBGCLCAIBOL!=NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC||MLGOLGGHLMB.CBEGDDKANAO==null)yield break;
  string root=RecruitPrefix+f.IBGCLCAIBOL+"/";EnsureRecruitList(root);var save=MLGOLGGHLMB.CBEGDDKANAO;
  bool Valid()=>CurrentFaction(f)&&f.IBGCLCAIBOL==NEINKOOBJIH.POMNMBGDDIN?.LMPMNLLGOEC?.LMMNKIOAGLC&&save.Pointer==MLGOLGGHLMB.CBEGDDKANAO?.Pointer;
  while(Valid()&&ActivateRecruitHead(root)){
   int id=QueueRead(root+"queue/active");string item=RecruitItem(root,id);
   foreach(var phase in RecruitmentActiveSteps(f)){yield return phase;if(!Valid()||QueueRead(root+"queue/active")!=id)yield break;}
   if(!Valid()||QueueRead(root+"queue/active")!=id)yield break;
   int done=QueueRead(root+"done"),pending=QueueRead(root+"pending");QueueWrite(item+"done",done);QueueWrite(item+"pending",pending);
   if(pending>0)yield break;
   if(QueueRead(item+"status")==RecruitClosing){QueueWrite(item+"status",RecruitCancelled);QueueWrite(item+"hidden",1);}
   else if(done>=QueueRead(item+"goal")){
    QueueWrite(item+"status",RecruitDone);
    // Synchronous native recruitment can complete in the dispatch phase,
    // before the legacy result-scan notification branch is reached.
    if(QueueRead(root+"notified")!=id){QueueWrite(root+"notified",id);UI_NotifyUI.OpenUI($"자동 등용 완료: 예약 #{id}, 요청한 {QueueRead(item+"goal")}명의 장수가 합류했습니다.",true,NEINKOOBJIH.POMNMBGDDIN,false,false,null);}
   }else yield break;
   QueueWrite(root+"queue/active",0);QueueWrite(root+"enabled",0);yield return "recruit-next-queue";
  }
 }
 static bool RecruitTargetBusy(MKMOJNAJKJE f,string target){foreach(var h in OfficerRegistrySnapshot())if(h?.LMPMNLLGOEC?.LMMNKIOAGLC==f.IBGCLCAIBOL&&h.OGPEJIEHDAN!=null&&RecruitJob(h.OGPEJIEHDAN.HFBAJIGHOAF)&&h.OGPEJIEHDAN.LKEPGHJDHGG==target)return true;return false;}
}
