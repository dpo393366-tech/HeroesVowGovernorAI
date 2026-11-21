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
Write-Output 'Built all three modules. No game files were changed.'
