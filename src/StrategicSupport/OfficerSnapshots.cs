using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
public sealed partial class StrategicSupport {
 // Native bulk copy avoids thousands of interop Enumerator calls. This array
 // belongs to this evaluation only; jobs/ownership are rechecked before acting.
 static Il2CppReferenceArray<CLAGKDECFID> CopyOfficerRegistry(){
  var registry=AEHAGJAKNEK.IEOHLJLICOA;
  var snapshot=new Il2CppReferenceArray<CLAGKDECFID>(registry?.Count??0);
  registry?.Values.CopyTo(snapshot,0);return snapshot;
 }
 static IEnumerable<CLAGKDECFID> OfficerRegistrySnapshot(){
  var snapshot=CopyOfficerRegistry();for(int i=0;i<snapshot.Length;i++)yield return snapshot[i];
 }
}
