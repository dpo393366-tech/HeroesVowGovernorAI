# 구성·빌드·검증 정보

일반 사용자는 [설치 설명](INSTALL.ko.md)과 [기능 설명](GUIDE.ko.md)을 먼저 읽으세요.

## 포함 버전

| 구성 | 버전 | 역할 |
|---|---|---|
| StrategicSupport | 1.5.43 | 도독 AI, 등용 큐, 보고 제어 |
| TreasuryAwards | 1.4.3 | 국고 물품 분배 |
| HeroesVowSettings | 1.4.0 | F12 설정 창 |
| GovernorAutoUpdate | 1.0.0 | 게임 시작 자동 업데이트 |

## 검증 범위

실제 게임 참조 빌드, 93개 Harmony 콜백 연결 검사 및 AI·국고·등용 큐·설정 창 모의 회귀검사를 수행했습니다. 야영 정체 수정은 실제 로그의 호출 순서를 재현하여 기존 코드에서 실패하고 수정 코드에서 통과하는 것을 확인했습니다.

자동 업데이트는 44개 검사, 설치기는 37개 검사를 통과했습니다. 실제 BepInEx 로더를 격리된 폴더에서 실행해 GitHub에서 파일을 내려받고 기존 1.5.42를 1.5.43으로 교체하는 것을 확인했습니다. 게임 본체를 실행한 장기 플레이 검증과는 구분됩니다.

도시 배치는 현재 후보를 비교하는 제한된 탐색입니다. 모든 모드 건물 조합에 대한 수학적 전역 최적해나 실전 무결점을 보장하지 않습니다.

## 소스 빌드

Windows PowerShell, .NET 6 SDK/런타임, .NET SDK 7 이상 및 지원 게임의 BepInEx/interop 참조가 필요합니다. 기존 배포는 SDK 7.0.203과 .NET 6.0.16 참조로 빌드했습니다. 게임/BepInEx DLL은 각자의 설치본에서 참조합니다.

```powershell
.\Build.ps1 -GameDir 'D:\SteamLibrary\steamapps\common\LegendOfHeros'
.\Test.ps1 -GameDir 'D:\SteamLibrary\steamapps\common\LegendOfHeros'
```

빌드는 `src/StrategicSupport`, `src/TreasuryAwards`, `src/Settings`, `src/Updater`에 DLL을 생성하며 게임 파일은 바꾸지 않습니다. `Test.ps1`은 모의검사와 실제 게임 메서드 서명 검사를 수행하며 게임을 실행하지 않습니다. 과거 고장 DLL을 사용하는 내부 음성 대조군 검사는 공개 소스에 그 바이너리를 넣지 않아 제외했습니다. 설치용 ZIP은 릴리스 자산을 사용하세요. `installer`는 설치기 소스이며 `payload` 바이너리는 릴리스 ZIP에만 포함합니다.
