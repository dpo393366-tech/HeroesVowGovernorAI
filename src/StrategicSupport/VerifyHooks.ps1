param([Parameter(Mandatory=$true)][string]$GameDir)
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot '../../BuildTools.ps1')
Push-Location $PSScriptRoot
$prior=$env:HV_BEPINEX
try {
 $env:HV_BEPINEX=Join-Path $GameDir 'BepInEx'
 $bindings=@()
 Get-ChildItem -File -Filter '*.cs' | Where-Object {$_.Name -notmatch 'Tests|QA|CompileHooks|ResolveHooks|Treasury'} | ForEach-Object {
  foreach($line in Get-Content -LiteralPath $_.FullName){
   if($line -match 'h\.Patch\(AccessTools\.Method\(typeof\((\w+)\),"(\w+)"\)'){
    $owner=$Matches[1]; $method=$Matches[2]
    $found=[regex]::Matches($line,'(prefix|postfix|finalizer):(?:H\(nameof\((\w+)\)\)|new HarmonyMethod\(typeof\(StrategicSupport\),nameof\((\w+)\)\))')
    if(!$found.Count){throw "Unrecognized patch registration: $line"}
    foreach($m in $found){
     $callback=$m.Groups[2].Value;if(!$callback){$callback=$m.Groups[3].Value}
     $bindings+="$owner $method $($m.Groups[1].Value) $callback"
    }
   }
  }
 }
 if($bindings.Count -lt 18){throw 'Hook coverage fell below 18 callbacks'}
 $bindings=$bindings | ForEach-Object {$_ -replace '^EventSystem ','UnityEngine.EventSystems.EventSystem ' -replace '^DeployConfirmation ','UI_DeployUI+GMPKDAEEJMK '}
 $bindings | Set-Content hook-bindings.txt

 Invoke-HVCsc (@('-target:exe','-out:CompileHooks.dll',('-r:'+(Join-Path $env:HV_BEPINEX 'core/0Harmony.dll')),'CompileHooks.cs'))
 Copy-Item IntegrationTests.runtimeconfig.json CompileHooks.runtimeconfig.json
 & dotnet CompileHooks.dll StrategicSupport.dll
 if($LASTEXITCODE -ne 0){throw 'Production hook compilation failed'}
 Write-Output 'PASS current Harmony contracts. Legacy binary negative control is not bundled.'
}finally{$env:HV_BEPINEX=$prior;Pop-Location}
