$ErrorActionPreference='Stop'
$sdkLines=@(& dotnet --list-sdks)
if($LASTEXITCODE -ne 0 -or !$sdkLines.Count){throw '.NET SDK is required'}
$sdk=$sdkLines | Where-Object {$_ -match '^7\.0\.203 '} | Select-Object -First 1
if(!$sdk){$sdk=$sdkLines[-1]}
if($sdk -notmatch '^(\S+) \[(.+)\]$'){throw 'Cannot resolve .NET SDK'}
$script:HVCompiler=Join-Path $Matches[2] ($Matches[1]+'/Roslyn/bincore/csc.dll')
$dotnetRoot=Split-Path $Matches[2] -Parent
$refRoot=Join-Path $dotnetRoot 'packs/Microsoft.NETCore.App.Ref'
$refPack=Get-ChildItem -LiteralPath $refRoot -Directory | Where-Object {$_.Name -match '^6\.0\.\d+$'} | Sort-Object {[version]$_.Name} -Descending | Select-Object -First 1
if(!$refPack){throw '.NET 6 targeting pack is required (install .NET 6 SDK)'}
$script:HVRefs=@(Get-ChildItem (Join-Path $refPack.FullName 'ref/net6.0') -Filter '*.dll' | ForEach-Object {'-r:'+$_.FullName})
function Invoke-HVCsc([string[]]$CompilerArgs){
 & dotnet $script:HVCompiler -nologo @script:HVRefs @CompilerArgs
 if($LASTEXITCODE -ne 0){throw 'C# compilation failed'}
}
function Get-HVGameRefs([string]$GameDir){
 $g=Join-Path $GameDir 'BepInEx'
 foreach($n in 'Assembly-CSharp','UnityEngine.PhysicsModule','UnityEngine.AIModule','UnityEngine.UIModule','UnityEngine.TextRenderingModule','UnityEngine.CoreModule','UnityEngine.InputLegacyModule','UnityEngine.IMGUIModule','Il2Cppmscorlib','UnityEngine.UI'){
  $p=Join-Path $g "interop/$n.dll";if(!(Test-Path -LiteralPath $p)){throw "Missing game dependency: $p"};'-r:'+$p
 }
 foreach($n in 'BepInEx.Core','BepInEx.Unity.IL2CPP','0Harmony','Il2CppInterop.Runtime'){'-r:'+(Join-Path $g "core/$n.dll")}
}
