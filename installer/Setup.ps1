param([string]$GameDir='', [ValidateSet('install','remove')][string]$Action='install', [switch]$CheckOnly)
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
 if($relative -notmatch '^BepInEx/plugins/(?:[A-Za-z0-9_-]+/)?[A-Za-z0-9_-]+\.dll$'){throw '설치 대상 경로가 올바르지 않습니다.'}
 $path=[IO.Path]::GetFullPath((Join-Path $GameDir $relative))
 if(!$path.StartsWith($root,[StringComparison]::OrdinalIgnoreCase)){throw '게임 폴더 밖 경로입니다.'}
 $probe=$path
 while($probe -and $probe.StartsWith($root,[StringComparison]::OrdinalIgnoreCase)){
  if(Test-Path -LiteralPath $probe){if((Get-Item -LiteralPath $probe -Force).Attributes -band [IO.FileAttributes]::ReparsePoint){throw '연결된 파일/폴더는 자동 변경하지 않습니다.'}}
  $probe=[IO.Path]::GetDirectoryName($probe)
 }
 return $path
}
if($manifest.id -notmatch '^[a-z0-9-]+$'){throw '패키지 식별자가 잘못되었습니다.'}
$lock=New-Object Threading.Mutex($false,'Local\HeroesVowConvenienceInstaller');$held=$false;$done=@();$plan=@()
try {
 try{$held=$lock.WaitOne(0)}catch [Threading.AbandonedMutexException]{$held=$true}
 if(!$held){throw '다른 모드 설치가 진행 중입니다.'}
 CheckStopped
 if($Action -eq 'install'){
  if(!(Test-Path -LiteralPath (Join-Path $GameDir 'BepInEx/core/BepInEx.Unity.IL2CPP.dll'))){throw 'BepInEx IL2CPP를 먼저 설치하세요.'}
  if((Digest (Join-Path $GameDir 'ThreeKingdom_Data/il2cpp_data/Metadata/global-metadata.dat')) -ne $manifest.metadataHash){throw '지원하는 게임 버전과 다릅니다. 설치하지 않았습니다.'}
 }
 foreach($item in $manifest.files){
  $dest=SafePath $item.path;$file=[IO.Path]::GetFileName($dest);$src=Join-Path (Join-Path $PSScriptRoot 'payload') $file
  if((Digest $src) -ne $item.sha256){throw "패키지 파일 검증 실패: $file"}
  $plugins=Join-Path $GameDir 'BepInEx/plugins'
  if(Test-Path -LiteralPath $plugins){foreach($duplicate in @(Get-ChildItem -LiteralPath $plugins -Filter $file -Recurse -File)){
   if($duplicate.FullName -ne $dest){throw "다른 위치에 같은 모드가 있습니다. 중복 설치하지 않습니다: $($duplicate.FullName)"}
  }}
  $old=$null;if(Test-Path -LiteralPath $dest){$old=Digest $dest;if($old -ne $item.sha256 -and $old -notin @($item.accepted)){throw "확인되지 않은 버전이 있어 보존했습니다: $file"}}
  if(($Action -eq 'install' -and $old -eq $item.sha256) -or ($Action -eq 'remove' -and !$old)){continue}
  $plan+=,[PSCustomObject]@{Source=$src;Target=$dest;Relative=$item.path;Old=$old;New=$item.sha256}
 }
 if($CheckOnly){Write-Output ("검사 완료: "+$manifest.title+" / 변경 파일 "+$plan.Count);return}
 if(!$plan.Count){Write-Output '이미 요청한 상태입니다.';return}
 $backup=Join-Path $GameDir ('BepInEx/independent-backups/'+(Get-Date -Format yyMMddHHmmss)+'-'+[guid]::NewGuid().ToString('N').Substring(0,8))
 [IO.Directory]::CreateDirectory($backup)|Out-Null
 foreach($item in $plan){if($item.Old){$b=Join-Path $backup ('before/'+[IO.Path]::GetFileName($item.Target));[IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($b))|Out-Null;Copy-Item -LiteralPath $item.Target -Destination $b}}
 try {
  foreach($item in $plan){
   CheckStopped
   if($item.Old){if((Digest $item.Target) -ne $item.Old){throw '작업 도중 대상 파일이 변경되었습니다.'}}elseif(Test-Path -LiteralPath $item.Target){throw '작업 도중 새 대상 파일이 생겼습니다.'}
   if($Action -eq 'remove'){
    $moved=Join-Path $backup ('removed/'+[IO.Path]::GetFileName($item.Target));[IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($moved))|Out-Null
    if(![IO.Path]::GetFullPath($moved).StartsWith($root,[StringComparison]::OrdinalIgnoreCase)){throw '백업 경로가 게임 폴더 밖입니다.'}
    [IO.File]::Move($item.Target,$moved);$done+=,$item
   }else{
    [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($item.Target))|Out-Null;$tmp=$item.Target+'.'+[guid]::NewGuid().ToString('N')+'.tmp'
    try{Copy-Item -LiteralPath $item.Source -Destination $tmp;if((Digest $tmp) -ne $item.New){throw '복사 검증 실패'}
     if($item.Old){[IO.File]::Replace($tmp,$item.Target,[NullString]::Value)}else{[IO.File]::Move($tmp,$item.Target)};$done+=,$item
     if((Digest $item.Target) -ne $item.New){throw '설치 검증 실패'}
    }finally{if(Test-Path -LiteralPath $tmp){Remove-Item -LiteralPath $tmp}}
   }
  }
 }catch {
  foreach($item in $done){if($item.Old){Copy-Item -LiteralPath (Join-Path $backup ('before/'+[IO.Path]::GetFileName($item.Target))) -Destination $item.Target -Force}elseif(Test-Path -LiteralPath $item.Target){Remove-Item -LiteralPath $item.Target}}
  throw
 }
 [ordered]@{package=$manifest.id;action=$Action;files=$plan;time=(Get-Date -Format o)}|ConvertTo-Json -Depth 6|Set-Content -LiteralPath (Join-Path $backup 'receipt.json') -Encoding UTF8
 Write-Output ($manifest.title+' '+$Action+' 완료. 설정과 저장파일은 유지했습니다. 백업: '+$backup)
}finally{if($held){$lock.ReleaseMutex()};$lock.Dispose()}
