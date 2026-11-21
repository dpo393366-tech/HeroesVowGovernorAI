param([Parameter(Mandatory=$true)][string]$PackageDir,[string]$LoaderArchive='', [switch]$Online)
$ErrorActionPreference='Stop'
$PackageDir=(Resolve-Path -LiteralPath $PackageDir).Path
$testRoot=Join-Path $PSScriptRoot ('fixture-'+[guid]::NewGuid().ToString('N').Substring(0,8))
[IO.Directory]::CreateDirectory($testRoot)|Out-Null
$checks=0;$global:HVTestCalls=0;$global:HVTestFailAt=0
# Fixture-only process simulation; production Setup.ps1 has no bypass option.
function global:Get-CimInstance {
 param($ClassName,$Filter,$ErrorAction)
 $global:HVTestCalls++
 if($global:HVTestFailAt -gt 0 -and $global:HVTestCalls -ge $global:HVTestFailAt){[pscustomobject]@{ThreadCount=1}}
}
function Assert($ok,$why){$script:checks++;if(!$ok){throw $why}}
function WriteFile($root,$relative,$text){$p=Join-Path $root $relative;[IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($p))|Out-Null;[IO.File]::WriteAllText($p,$text)}
function Hash($p){(Get-FileHash -LiteralPath $p).Hash}
function Game($name){$p=Join-Path $testRoot $name;WriteFile $p ThreeKingdom.exe 'fixture';WriteFile $p 'BepInEx/config/user.cfg' 'preserve-config';WriteFile $p 'BepInEx/plugins/Unrelated.dll' 'preserve-plugin';return $p}
function Run($game,$action='install',$pkg=$PackageDir,$archive=$LoaderArchive){& (Join-Path $pkg Setup.ps1) -GameDir $game -Action $action -LoaderArchive $archive | Out-Null}
function Fails($operation,$why){$failed=$false;try{& $operation}catch{$failed=$true};Assert $failed $why}
$m=Get-Content (Join-Path $PackageDir manifest.json) -Raw|ConvertFrom-Json
try {
 $fresh=Game fresh
 & (Join-Path $PackageDir Setup.ps1) -GameDir $fresh -CheckOnly | Out-Null
 Assert (!(Test-Path (Join-Path $fresh 'BepInEx/independent-backups'))) 'CheckOnly wrote backups'
 Assert (!(Test-Path (Join-Path $fresh 'BepInEx/core'))) 'CheckOnly installed loader'
 if($Online){Run $fresh install $PackageDir ''}else{if(!$LoaderArchive){throw 'Provide -LoaderArchive or -Online'};Run $fresh}
 foreach($p in 'BepInEx/core/BepInEx.Unity.IL2CPP.dll','dotnet/coreclr.dll','winhttp.dll','doorstop_config.ini'){Assert (Test-Path (Join-Path $fresh $p)) "Missing runtime $p"}
 foreach($f in $m.files){Assert ((Hash (Join-Path $fresh $f.path)) -eq $f.sha256) 'fresh module missing'}
 Assert ([IO.File]::ReadAllText((Join-Path $fresh 'BepInEx/config/user.cfg')) -eq 'preserve-config') 'configuration changed'
 $runtimeHash=Hash (Join-Path $fresh 'BepInEx/core/BepInEx.Unity.IL2CPP.dll')
 $before=@(Get-ChildItem (Join-Path $fresh BepInEx/independent-backups) -Directory).Count
 Run $fresh install $PackageDir 'missing-archive-is-not-needed.zip'
 Assert (@(Get-ChildItem (Join-Path $fresh BepInEx/independent-backups) -Directory).Count -eq $before) 'reinstall unnecessarily changed files'
 Write-Output 'PASS clean automatic runtime installation and repeat install'

 $existing=Game existing
 WriteFile $existing 'BepInEx/core/BepInEx.Unity.IL2CPP.dll' 'custom-existing-loader'
 WriteFile $existing 'doorstop_config.ini' 'custom-doorstop-settings'
 WriteFile $existing 'ThreeKingdom_Data/il2cpp_data/Metadata/global-metadata.dat' 'new-game-version'
 foreach($f in $m.files){WriteFile $existing $f.path 'unknown-old-module'}
 WriteFile $existing 'BepInEx/plugins/old/StrategicSupport.dll' 'duplicate-one'
 WriteFile $existing 'BepInEx/plugins/older/StrategicSupport.dll' 'duplicate-two'
 Run $existing install $PackageDir 'missing-archive-is-not-needed.zip'
 Assert ([IO.File]::ReadAllText((Join-Path $existing 'BepInEx/core/BepInEx.Unity.IL2CPP.dll')) -eq 'custom-existing-loader') 'existing loader overwritten'
 Assert ([IO.File]::ReadAllText((Join-Path $existing doorstop_config.ini)) -eq 'custom-doorstop-settings') 'existing doorstop overwritten'
 Assert (@(Get-ChildItem (Join-Path $existing BepInEx/plugins) -Filter StrategicSupport.dll -Recurse).Count -eq 1) 'duplicate modules remain'
 foreach($f in $m.files){Assert ((Hash (Join-Path $existing $f.path)) -eq $f.sha256) 'unknown old module blocked upgrade'}
 $backups=@(Get-ChildItem (Join-Path $existing BepInEx/independent-backups) -Filter '*.bak' -Recurse)
 Assert ($backups.Count -eq 5) 'distinct duplicate backups missing'
 Assert (@($backups | Where-Object {[IO.File]::ReadAllText($_.FullName) -in @('duplicate-one','duplicate-two')}).Count -eq 2) 'duplicate contents lost'
 Run $existing remove
 foreach($f in $m.files){Assert (!(Test-Path (Join-Path $existing $f.path))) 'remove missed module'}
 Assert (Test-Path (Join-Path $existing BepInEx/core/BepInEx.Unity.IL2CPP.dll)) 'remove deleted loader'
 Assert (Test-Path (Join-Path $existing BepInEx/plugins/Unrelated.dll)) 'unrelated plugin removed'
 Write-Output 'PASS existing loader preserved, version gates removed, duplicates backed up, removal scoped'

 $rollback=Game rollback
 WriteFile $rollback 'BepInEx/core/BepInEx.Unity.IL2CPP.dll' 'existing'
 WriteFile $rollback 'BepInEx/plugins/old/StrategicSupport.dll' 'duplicate-restore'
 WriteFile $rollback $m.files[0].path 'old-restore'
 $global:HVTestCalls=0;$global:HVTestFailAt=4
 Fails {Run $rollback} 'mid-transaction failure not detected'
 $global:HVTestFailAt=0
 Assert ([IO.File]::ReadAllText((Join-Path $rollback $m.files[0].path)) -eq 'old-restore') 'original file not rolled back'
 Assert ([IO.File]::ReadAllText((Join-Path $rollback 'BepInEx/plugins/old/StrategicSupport.dll')) -eq 'duplicate-restore') 'duplicate not rolled back'
 Assert (!(Test-Path (Join-Path $rollback $m.files[1].path))) 'partial installation remained'
 $bad=Game corrupt-runtime
 WriteFile $testRoot 'bad.zip' 'corrupt'
 Fails {Run $bad install $PackageDir (Join-Path $testRoot bad.zip)} 'corrupt loader accepted'
 Assert (!(Test-Path (Join-Path $bad $m.files[0].path))) 'module changed before runtime validation'
 $global:HVTestCalls=0;$global:HVTestFailAt=1
 Fails {Run $bad} 'running game accepted'
 $global:HVTestFailAt=0
 Write-Output 'PASS transaction rollback, corrupt runtime rejection and running-game guard'
 Write-Output "PASS installer checks=$checks fixtures=$testRoot"
}finally{Remove-Item Function:\Get-CimInstance -ErrorAction SilentlyContinue}
