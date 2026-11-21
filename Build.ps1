param([Parameter(Mandatory=$true)][string]$GameDir)
$ErrorActionPreference='Stop'
$GameDir=(Resolve-Path -LiteralPath $GameDir).Path
. (Join-Path $PSScriptRoot 'BuildTools.ps1')
$spec=Get-Content (Join-Path $PSScriptRoot 'source-list.json') -Raw | ConvertFrom-Json
$native=@(Get-HVGameRefs $GameDir)
foreach($component in 'StrategicSupport','TreasuryAwards','Settings'){
 $output=if($component -eq 'Settings'){'HeroesVowSettings.dll'}else{"$component.dll"}
 Push-Location (Join-Path $PSScriptRoot "src/$component")
 try {
  $options=@('-optimize+','-target:library',"-out:$output")
  if($component -ne 'Settings'){$options+='-deterministic'}
  Invoke-HVCsc ($options+$native+@($spec.$component))
 }finally{Pop-Location}
}
Push-Location (Join-Path $PSScriptRoot 'src/Updater')
try {
 Invoke-HVCsc @('-optimize+','-deterministic','-target:library','-out:GovernorAutoUpdate.dll',('-r:'+(Join-Path $GameDir 'BepInEx/core/BepInEx.Core.dll')),('-r:'+(Join-Path $GameDir 'BepInEx/core/BepInEx.Preloader.Core.dll')),('-r:'+(Join-Path $GameDir 'BepInEx/core/Mono.Cecil.dll')),'Bootstrap.cs','Engine.cs')
} finally { Pop-Location }
Write-Output 'Built three modules and startup updater. No game files were changed.'
