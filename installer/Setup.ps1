param([string]$GameDir='', [ValidateSet('install','remove')][string]$Action='install', [switch]$CheckOnly, [string]$LoaderArchive='')
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'GamePath.ps1')
$GameDir=Resolve-HeroesVowGameDir -GameDir $GameDir -NearbyPaths @($PSScriptRoot,(Get-Location).Path)
$root=[IO.Path]::GetFullPath($GameDir).TrimEnd('\')+'\'
$manifest=Get-Content -LiteralPath (Join-Path $PSScriptRoot 'manifest.json') -Raw | ConvertFrom-Json
function Digest([string]$p){(Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash}
function CheckStopped {
 if(@(Get-CimInstance Win32_Process -Filter "Name='ThreeKingdom.exe'" -ErrorAction Stop | Where-Object {$_.ThreadCount -gt 0}).Count){throw '게임을 저장하고 종료한 뒤 실행하세요. 게임을 강제로 종료하지 않습니다.'}
}
function SafePath([string]$relative){
 if([IO.Path]::IsPathRooted($relative) -or $relative -match '(^|[/\\])\.\.([/\\]|$)|:'){throw '설치 경로가 올바르지 않습니다.'}
 $path=[IO.Path]::GetFullPath((Join-Path $GameDir $relative))
 if(!$path.StartsWith($root,[StringComparison]::OrdinalIgnoreCase)){throw '게임 폴더 밖 경로입니다.'}
 $probe=$path
 while($probe -and ($probe.StartsWith($root,[StringComparison]::OrdinalIgnoreCase) -or $probe -eq $GameDir)){
  if(Test-Path -LiteralPath $probe){if((Get-Item -LiteralPath $probe -Force).Attributes -band [IO.FileAttributes]::ReparsePoint){throw '연결된 파일/폴더는 자동 변경하지 않습니다.'}}
  $probe=[IO.Path]::GetDirectoryName($probe)
 }
 return $path
}
function AddPlan([string]$relative,[string]$source,[string]$next,[string]$operation,$entry=$null){
 $target=SafePath $relative
 if(!$script:targets.Add($target)){throw "중복 작업 경로: $relative"}
 $old=$null
 if(Test-Path -LiteralPath $target){$old=Digest $target}
 if(($operation -eq 'write' -and $old -eq $next) -or ($operation -eq 'remove' -and !$old)){return}
 $script:plan.Add([PSCustomObject]@{Source=$source;Target=$target;Relative=$relative;Old=$old;New=$next;Operation=$operation;Entry=$entry;Backup=$null})
}
function PluginCopies([string]$file){
 $plugins=SafePath 'BepInEx/plugins'
 if(!(Test-Path -LiteralPath $plugins)){return}
 $todo=New-Object 'System.Collections.Generic.Stack[string]';$todo.Push($plugins)
 while($todo.Count){
  foreach($p in Get-ChildItem -LiteralPath $todo.Pop() -Force){
   if($p.Attributes -band [IO.FileAttributes]::ReparsePoint){continue}
   if($p.PSIsContainer){$todo.Push($p.FullName)}elseif($p.Name -ieq $file){$p.FullName}
  }
 }
}
if($manifest.id -notmatch '^[a-z0-9-]+$'){throw '패키지 식별자가 잘못되었습니다.'}
$lock=New-Object Threading.Mutex($false,'Local\HeroesVowConvenienceInstaller')
$held=$false;$zip=$null;$download=$null
$plan=New-Object 'System.Collections.Generic.List[object]'
$done=New-Object 'System.Collections.Generic.List[object]'
$targets=New-Object 'System.Collections.Generic.HashSet[string]' ([StringComparer]::OrdinalIgnoreCase)
try {
 try{$held=$lock.WaitOne(0)}catch [Threading.AbandonedMutexException]{$held=$true}
 if(!$held){throw '다른 모드 설치가 진행 중입니다.'}
 CheckStopped
 # No game metadata hash or old-plugin allowlist gate.
 foreach($item in $manifest.files){
  if($item.path -notmatch '^BepInEx/plugins/(?:[A-Za-z0-9_-]+/)?[A-Za-z0-9_-]+\.dll$'){throw '모드 대상 경로가 올바르지 않습니다.'}
  $dest=SafePath $item.path;$file=[IO.Path]::GetFileName($dest)
  $src=Join-Path (Join-Path $PSScriptRoot 'payload') $file
  if($Action -eq 'install' -and (Digest $src) -ne $item.sha256){throw "설치 파일이 손상되었습니다: $file"}
  foreach($duplicate in @(PluginCopies $file)){
   if($duplicate -ine $dest){AddPlan $duplicate.Substring($root.Length).Replace('\','/') '' '' 'remove'}
  }
  if($Action -eq 'install'){AddPlan $item.path $src $item.sha256 'write'}else{AddPlan $item.path '' '' 'remove'}
 }
 $needLoader=$Action -eq 'install' -and !(Test-Path -LiteralPath (SafePath 'BepInEx/core/BepInEx.Unity.IL2CPP.dll'))
 if($Action -eq 'install'){
  if(!$needLoader){Write-Host '기존 BepInEx IL2CPP를 사용합니다. 로더와 설정은 변경하지 않습니다.'}
  elseif($CheckOnly){Write-Host 'BepInEx IL2CPP 없음: 설치 실행 시 공식 로더를 자동 설치합니다.'}
  else {
   if(!$LoaderArchive){
    Write-Host 'BepInEx IL2CPP가 없어 공식 로더를 다운로드합니다.'
    $download=Join-Path ([IO.Path]::GetTempPath()) ('HVLoader-'+[guid]::NewGuid().ToString('N')+'.zip')
    $LoaderArchive=$download
    [Net.ServicePointManager]::SecurityProtocol=[Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12
    Invoke-WebRequest -UseBasicParsing -Uri $manifest.loader.source -OutFile $download -TimeoutSec 180
   }
   if((Digest $LoaderArchive) -ne $manifest.loader.sha256){throw '로더 다운로드가 손상되었습니다. 다시 실행하거나 공식 ZIP을 -LoaderArchive로 지정하세요.'}
   Add-Type -AssemblyName System.IO.Compression.FileSystem
   $zip=[IO.Compression.ZipFile]::OpenRead([IO.Path]::GetFullPath($LoaderArchive))
   foreach($entry in $zip.Entries){
    if(!$entry.Name){continue}
    $relative=$entry.FullName.Replace('\','/')
    # Bootstrap only: never replace plugins, configuration or saves from the ZIP.
    if($relative -notmatch '^(BepInEx/core/[^:]+|dotnet/[^:]+|winhttp\.dll|doorstop_config\.ini|\.doorstop_version)$'){continue}
    $stream=$entry.Open();$sha=[Security.Cryptography.SHA256]::Create()
    try{$hash=([BitConverter]::ToString($sha.ComputeHash($stream))).Replace('-','')}finally{$sha.Dispose();$stream.Dispose()}
    AddPlan $relative '' $hash 'write' $entry
   }
   if(!$targets.Contains((SafePath 'BepInEx/core/BepInEx.Unity.IL2CPP.dll'))){throw 'IL2CPP 로더 파일을 찾을 수 없습니다.'}
   Write-Host '공식 BepInEx IL2CPP를 자동 설치합니다. 기존 파일이 겹치면 백업합니다.'
  }
 }
 if($CheckOnly){Write-Output ("검사 완료: "+$manifest.title+" / 모드 변경 파일 "+$plan.Count+" / 로더 자동설치 "+$needLoader);return}
 if(!$plan.Count){Write-Output '이미 요청한 상태입니다.';return}
 $backup=SafePath ('BepInEx/independent-backups/'+(Get-Date -Format yyMMddHHmmss)+'-'+[guid]::NewGuid().ToString('N').Substring(0,8))
 [IO.Directory]::CreateDirectory($backup)|Out-Null
 $i=0
 foreach($item in $plan){
  if($item.Old){$item.Backup=Join-Path $backup ('{0:D4}.bak' -f $i);Copy-Item -LiteralPath $item.Target -Destination $item.Backup}
  $i++
 }
 $receipt=@($plan | Select-Object Relative,Old,New,Operation,Backup)
 [ordered]@{package=$manifest.id;action=$Action;state='prepared';files=$receipt;time=(Get-Date -Format o)}|ConvertTo-Json -Depth 6|Set-Content -LiteralPath (Join-Path $backup 'receipt.json') -Encoding UTF8
 try {
  foreach($item in $plan){
   CheckStopped
   [void](SafePath $item.Relative)
   if($item.Old){if((Digest $item.Target) -ne $item.Old){throw '작업 도중 대상 파일이 변경되었습니다.'}}elseif(Test-Path -LiteralPath $item.Target){throw '작업 도중 새 대상 파일이 생겼습니다.'}
   if($item.Operation -eq 'remove'){
    [IO.File]::Delete($item.Target);$done.Add($item)
   }else{
    [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($item.Target))|Out-Null
    $tmp=Join-Path ([IO.Path]::GetDirectoryName($item.Target)) ('.hv'+[guid]::NewGuid().ToString('N').Substring(0,8)+'.tmp')
    try {
     if($null -ne $item.Entry){[IO.Compression.ZipFileExtensions]::ExtractToFile($item.Entry,$tmp,$false)}else{Copy-Item -LiteralPath $item.Source -Destination $tmp}
     if((Digest $tmp) -ne $item.New){throw '복사 검증 실패'}
     if($item.Old){[IO.File]::Replace($tmp,$item.Target,[NullString]::Value)}else{[IO.File]::Move($tmp,$item.Target)}
     $done.Add($item)
     if((Digest $item.Target) -ne $item.New){throw '설치 검증 실패'}
    }finally{if(Test-Path -LiteralPath $tmp){Remove-Item -LiteralPath $tmp}}
   }
  }
 }catch {
  $cause=$_;$rollbackErrors=@()
  for($j=$done.Count-1;$j -ge 0;$j--){$item=$done[$j]
   try{if($item.Old){Copy-Item -LiteralPath $item.Backup -Destination $item.Target -Force}elseif(Test-Path -LiteralPath $item.Target){Remove-Item -LiteralPath $item.Target}}catch{$rollbackErrors+=$_.Exception.Message}
  }
  if($rollbackErrors.Count){throw ("설치 실패 및 일부 복구 실패. 백업: $backup / "+($rollbackErrors -join '; '))}
  throw $cause
 }
 [ordered]@{package=$manifest.id;action=$Action;state='complete';files=$receipt;time=(Get-Date -Format o)}|ConvertTo-Json -Depth 6|Set-Content -LiteralPath (Join-Path $backup 'receipt.json') -Encoding UTF8
 Write-Output ($manifest.title+' '+$Action+' 완료. 설정과 저장파일은 유지했습니다. 백업: '+$backup)
 if($needLoader){Write-Host '최초 게임 실행은 BepInEx 준비로 평소보다 오래 걸릴 수 있습니다.'}
}finally{
 if($zip){$zip.Dispose()}
 if($download -and (Test-Path -LiteralPath $download)){Remove-Item -LiteralPath $download -ErrorAction SilentlyContinue}
 if($held){$lock.ReleaseMutex()};$lock.Dispose()
}
