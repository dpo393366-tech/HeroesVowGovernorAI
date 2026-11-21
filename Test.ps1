param([Parameter(Mandatory=$true)][string]$GameDir)
$ErrorActionPreference='Stop'
$GameDir=(Resolve-Path -LiteralPath $GameDir).Path
& (Join-Path $PSScriptRoot 'Build.ps1') -GameDir $GameDir
. (Join-Path $PSScriptRoot 'BuildTools.ps1')
$spec=Get-Content (Join-Path $PSScriptRoot 'source-list.json') -Raw | ConvertFrom-Json
Push-Location (Join-Path $PSScriptRoot 'src/StrategicSupport')
try {
 Invoke-HVCsc (@('-target:exe','-main:Runner','-out:Regression.dll')+@($spec.StrategicSupport)+@($spec.StrategicSupportTests))
 Copy-Item IntegrationTests.runtimeconfig.json Regression.runtimeconfig.json
 & dotnet Regression.dll
 if($LASTEXITCODE -ne 0){throw 'AI regression failed'}
 & ./VerifyHooks.ps1 -GameDir $GameDir
}finally{Pop-Location}
Push-Location (Join-Path $PSScriptRoot 'src/TreasuryAwards')
try {
 $source=Get-Content TreasuryAwards.cs -Raw
 $core='using System;using System.Linq;using System.Collections.Generic;public sealed partial class TreasuryAwards { public static Percent RecipientPercent=new();static bool busy;static MockPlugin plugin=new();'+$source.Substring($source.IndexOf('    static UI_RewardUI bulkRewardUI;'))
 [IO.File]::WriteAllText((Join-Path (Get-Location).Path 'tests/Core.cs'),$core)
 Invoke-HVCsc @('-target:exe','-out:tests/Suite.dll','tests/Core.cs','tests/Suite.cs','tests/NewTests.cs','tests/AutomaticTests.cs','tests/RoundRobinTests.cs','tests/PerfTests.cs','tests/Button106Tests.cs','TreasuryAutomatic.cs','AwardRules.cs','AwardRoundRobin.cs')
 & dotnet tests/Suite.dll
 if($LASTEXITCODE -ne 0){throw 'Treasury regression failed'}
}finally{Pop-Location}
Push-Location (Join-Path $PSScriptRoot 'src/Settings')
$prior=$env:HV_BEPINEX
try {
 $env:HV_BEPINEX=Join-Path $GameDir 'BepInEx'
 New-Item -ItemType Directory -Force settings-test | Out-Null
 Invoke-HVCsc @('-optimize+','-target:exe','-out:settings-test/Test.dll',('-r:'+(Join-Path $env:HV_BEPINEX 'core/0Harmony.dll')),'SettingsRegistry.cs','UnifiedSettings.cs','SettingsTest.cs')
 Copy-Item ../StrategicSupport/IntegrationTests.runtimeconfig.json settings-test/Test.runtimeconfig.json
 foreach($module in 'DuelCounters','FieldEncounters','ReserveWaves','GovernorWatch','EnemyTempo','DuelAggression','FramePacing','DodgeLimit','TreasuryAwards','StrategicSupport'){
  $define=if($module -eq 'DodgeLimit'){'-define:DODGE'}else{"-define:PANEL,$module"}
  Invoke-HVCsc @('-optimize+','-target:library',"-out:settings-test/$module.dll",'-r:settings-test/Test.dll',$define,'SettingsFake.cs')
 }
 & dotnet settings-test/Test.dll
 if($LASTEXITCODE -ne 0){throw 'Settings regression failed'}
}finally{$env:HV_BEPINEX=$prior;Pop-Location}
