using System;using System.Collections.Generic;using System.Diagnostics;using System.Runtime.InteropServices;
public sealed partial class StrategicSupport {
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode)]static extern IntPtr GetModuleHandleW(string name);
 [DllImport("kernel32.dll",EntryPoint="RtlCaptureStackBackTrace")]static extern ushort CaptureDispatchStack(uint skip,uint count,[Out]IntPtr[] frames,IntPtr hash);
 static void LogDismissCallsite(Wo_DeployTeamCtrl t){
  if(!t||t.m_isPrivate||t.m_warInfo!=null||t.m_deployState!=AKOPKFPKJPP.Assemble&&t.m_deployState!=AKOPKFPKJPP.Expedition)return;
  // Bounded per source/state, not per frame or per newly allocated team ID.
  if(!Gate("dismiss-callsite:"+t.m_belongPower+":"+t.m_assembleCity+":"+t.m_deployState,30))return;
  var managed=new List<string>();foreach(var frame in new StackTrace(false).GetFrames()??Array.Empty<StackFrame>()){
   var method=frame.GetMethod();if(method?.DeclaringType==typeof(StrategicSupport)&&method.Name!=nameof(LogDismissCallsite))managed.Add(method.Name);
   if(managed.Count>=8)break;
  }
  var native=new List<string>();
  if(OperatingSystem.IsWindows())try{
   long module=GetModuleHandleW("GameAssembly.dll").ToInt64();if(module!=0){var frames=new IntPtr[48];int n=CaptureDispatchStack(0,(uint)frames.Length,frames,IntPtr.Zero);
    for(int i=0;i<n&&native.Count<12;i++){long offset=frames[i].ToInt64()-module;if(offset>=0&&offset<0x10000000)native.Add("0x"+offset.ToString("X"));}}
  }catch(Exception e){native.Add("unavailable:"+e.GetType().Name);}
  Note($"army-dismiss-callsite team={t.m_id} faction={t.m_belongPower} source={t.m_assembleCity} target={t.m_targetCity} targetWar={t.m_targetWarInfo?.HIENOINGBCN} state={t.m_deployState} adjustedPower={t.GetCombatVal()} soldiers={t.GetSoldierNum(false)} movePoints={t.m_listMovePos?.Count} managed={string.Join(">",managed)} gameAssemblyReturnRvas={string.Join(",",native)}");
 }
}
