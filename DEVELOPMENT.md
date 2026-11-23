## r144–r146: manual assembly, joint hunt and bounded road work

Manual confirmation now captures the native auto-assembly UI coroutine scope and fully formed RefreshLeader roster. Manual protection is registered before attack cancellation guards. Delayed departure, mission completion/five-day grace, immediate disband handoff and private/player scopes remain unchanged. Twenty-two focused mock checks passed; native signature binding checked. Previously unmarked/disbanded armies are not retroactively reconstructed.

Joint hunt uses nearby cities' actual native assembled power when no individual source is sufficient. Validated road ETA ranks available sources; participants physically travel to a safe friendly field rendezvous, then merge using the native method. Ownership/world identity, home defense, manual commands, food, route safety, actual arrival, soldier conservation and final power are rechecked. A target already fighting the same faction hands existing participants to normal support; invalid/stalled plans return through ordinary recovery. 270 focused checks include 60 seeded conditions and 1000 repeated active-plan evaluations without additional route searches or generation. Native behavior is mocked, not live hunt success.

Performance review covered107 retained BepInEx logs/rotations including subfolders (474323 lines) plus the preceding Player log. Current-session16 renewal rollback exceptions reveal that same-parcel conversion lacked the completed-goal/construction-time safeguards already used by multi-parcel rebuilding. Both now share the guard. Unexpected post-registration failures still restore the original and preserve resource accounting; diagnostic context identifies city and definitions.

Route commands now search alternatives using saved fixed geometry and freshly validate only the chosen legs. Dijkstra candidates retain predecessor/leg references instead of copying every accumulated corner list. Linear minimum selection avoids sorting temporary objects. Read-only graph work coalesces at most16 nodes/0.2ms while retaining policy/native-query yield boundaries and the outer frame budget. Sparse managed-queue diagnostics report8ms+ steps at most once per5s through the existing background writer; no global/native Update profiler hooks were restored.

Five before/after managed-fixture runs on80 cities: moving-origin100 commands33900->1300 native-path calls; median managed time37.744->23.232ms; warm graph yields81->6; allocation389632->304824 bytes. Native path calls are stand-ins, so these are not live FPS measurements. Twenty seeded directed road graphs match an independent shortest-path oracle. New672 assertions and the full existing regression suite pass. Three defect-reintroduction controls fail as expected (all-alternative native search, omitted physical validation, omitted renewal preflight). 100 production callbacks compile through installed Harmony over58 managed stand-ins with actual native signatures.

Local installed StrategicSupport hash: FFE7EF263285F30F6D3AFD550C9896F5C358C747EDD0ECD53844A780B949310F. Live campaign/FPS verification remains pending. A separate model module's603.27ms ApplyEntry spike and multi-second world/menu gaps are not claimed fixed by this AI release. Model instrumentation changes are outside this repository/package.

## r143: fixed map geometry separate from passage decisions

Audit scope:17 retained/snapshotted files,261186 unique lines dated2026-11-23 00:19:35 through04:20:36, across9 startups. Reloaded saves and missing definitive result events mean this is not a complete unique win/loss ledger. Current r141 contains104 player-faction support-order records and no matching observed immediate departure cancellations. Dispatch is not arrival.

Jianye:team24295 started with130000 soldiers and adjusted729501 vs required495591; battle observed128848 vs157206. Forty-eight cold source quotes took21860ms before decision. Nearest ETA20.7 exceeded estimated hold20.5, so no support; team later reached0 soldiers. Chengdu before reload:44-source pass (23 reused/21 calculated)19375ms; nearest20.6 vs hold18.3. Reloaded battle:queue3750ms plus44 cold quotes12406ms. Wuwei30000 vs188464:43 quotes7453ms; ETA40.8 vs hold28.9. These elapsed times include scheduling/pause, not exclusive CPU time or controlled FPS measures.

Geometry:positive exact-coordinate native corner chains shared across factions within the current map registry. Bounded8192 entries/262144 corners; individual eviction rather than clearing every road. No faction owner or diplomacy in the geometry key. Successful route choices no longer expire after3s; saved battle roads no longer refresh after60s solely by time. Actual owner/relation/topology comparisons and intermediate-territory permissions still run. Final selected commands still validate native geometry; physical failure invalidates the affected leg. New world or endpoint cannot reuse old coordinates. No disk persistence across game restarts is claimed.

Cold support expands through road-adjacency rings, with validated actual ETA ranking inside each ring, then all remaining candidates. Unknown farther-ring routes are not claimed globally slower; warm plans retain global order. Actual path permission/corridor checks are mandatory. Preparation no longer scans whole-map policy on each inner yielded geometry step; route completion still validates policy before publishing. Added simulation elapsed time to existing plan-end action record distinguishes future simulation time from wall-clock pauses.

Verification:1260 new assertions, including200 identical route plans with zero extra native searches,80 sources and12 ownership changes/960 plans with zero extra native geometry searches, changing passage during yields, new world/endpoints, selected-command physical failure, and second-ring relief before100 unrelated routes. Four defect reintroduction controls must fail (geometry reuse/TTL/rings/physical guard). Existing regression suite and99 Harmony callbacks/57 stand-ins pass. Native calls are mocks in simulations; no claim of live r143 FPS or restored lost battles.

## r142: cold loaded-battle relief and hunt scheduling

Live r141 verification: target22175 (Xiongnu) was hunted by team23175 from CS1032 at 04:02:09, with five officers and100000 soldiers, adjusted918129 vs required593108. Actual battle entry04:02:33; target reached zero soldiers04:03:46;61359 survivors received a return order and reached home04:04:48. This verifies that particular r141 hunt, not every raider or r142 performance.

Remaining failure: a save loaded with Chengdu already fighting bypassed pre-march preparation. Queue wait3750ms plus44 cold quotes12406ms preceded the first support decision. Nearest ETA20.6s exceeded estimated remaining hold10.3s then. No relief was dispatched. r142 splits a cold pass into direct-road-neighbour and remaining batches, each sorted by validated road ETA; warm passes retain global ranking. Unknown remote paths are not claimed slower. Existing dispatch reservations and final route/policy/live-force checks remain mandatory.

Battle planning and other critical jobs alternate within the existing critical share; hunt gets an existing urgent lane. Unrelated cities are filtered before hunt-source and muster-destination yields. Existing20-day hunt/convoy limit is unchanged; logging now distinguishes path failure from travel limit. No additional frame time budget or troop movement acceleration.

Verification:27 focused mock assertions including100 remote battle sources,200 unrelated hunt cities and300 watchers;3 negative controls restore the whole-world cold batch, unrelated city yields or shared watcher FIFO and must fail. Existing regression suite, actual native reference build and hook checks also pass. Native actions in the mock suite are substitutes; live r142 support latency and FPS remain unverified.

## r141: 공격 전력 누락·토벌 집결·행군 중 지원 준비

Observed in the reported session: Shuofang attack 22225 was approved with requiredAdjusted=1350 and 10000 soldiers while field defenders were present. Wuwei army22629 had30000 soldiers against188464 at battle start. Its cold support pass calculated43 routes in7453ms before ranking; nearest ETA40.8 exceeded observed hold28.9. Chengdu army22740 had10000 against205890; initial routing did not produce a dispatch before that battle ended.

Changes: deduplicated local/incoming field defenders plus unreserved garrison, separate manpower floor(.75 at new autonomous attack; .5 at march reconsideration, alongside parity), safe full-pool native allocation, capability-change retry suppression for manpower failures, scheduled march reassessment and same-endpoint prewar route preparation. Native path validation/corridor handling remains mandatory. Manual/private/active-combat/support orders are preserved. No claim that numerical thresholds guarantee a battle result.

Hunt: actual insufficient assembled force now remains a deficit even if uncommanded roster power looks sufficient. Safe full-pool generation avoids stopping at the misleading native request threshold. Incoming cargo reservations and real arrival rechecks retained; blocked muster stages now log a reason.

Verification: existing full regression suite,100 focused production-code mock checks,4 negative controls (removed field census/manpower check/muster deficit/prewar cache must fail), actual native reference build and Harmony stand-ins. Not live gameplay/FPS verification. Actual march ETA uses existing complete path corners without new path searches; prewar route generation yields through existing scheduler budget.

# 구성·빌드·검증 정보

일반 사용자는 [설치 설명](INSTALL.ko.md)과 [기능 설명](GUIDE.ko.md)을 먼저 읽으세요.

## 포함 버전

| 구성 | 버전 | 역할 |
|---|---|---|
| StrategicSupport | 1.5.44 | 도독 AI, 등용 큐, 보고 제어 |
| TreasuryAwards | 1.4.3 | 국고 물품 분배 |
| HeroesVowSettings | 1.4.0 | F12 설정 창 |
| GovernorAutoUpdate | 1.0.0 | 게임 시작 자동 업데이트 |

## 검증 범위

`intercept-defender-r140`은 공격받는 도시의 실제 소유자와 전투 양측을 확인하여, 동맹이 공격자인 전투의 방어 지원군을 요격 대상으로 잘못 포함하던 조건을 수정합니다. 바닐라 요격 함수의 후보 없음 정리 중에만 기존 자동 요격대의 즉시 해산을 막고, 목적 재판단 및 실제 경로 복귀를 수행합니다. 일반 입성·수동 해산·전투 사망·다른 호출 경로의 해산은 유지합니다. 전후 모든 해산 경로가 해결됐다는 의미는 아닙니다.

분할 복귀는 실제 장수별 병력 묶음의 순서와 도시 수용량을 반영합니다. 추가 요격 검사 359개와 분할 복귀 검사 435개(생성된 편성 160개), 기존 회귀검사 및 설치 게임의 실제 서명을 사용하는 연결 검사를 통과했습니다. 검사 횟수에는 반복 단언이 포함되며 게임 API는 대역입니다. 이전의 과도한 요격 조건을 다시 넣으면 새 검사가 실패합니다.

에디터 호환성은 전역 갱신 성능 계측 제거와 관련 동작 회귀검사에 더해 사용자 환경에서 충돌 오류 해소를 확인했습니다. 유언비어 주기 보정·효과 호출 진단은 AI 소스와 배포 파일에서 완전히 분리했습니다. 해당 보정의 실전 반복 알림 해소 여부는 별도 통합모드에서 확인할 사항입니다.

`capacity-evacuation-r124` (v1.5.44)은 기존 건물의 완공·목표 진행과 부지·건물 종류·해금 변경을 구분하여 건설 계획의 반복 폐기를 줄입니다. 건설 실행 직전 자금·부지·조건 검사를 유지하며 계산을 최대 0.5ms/256단계로 묶고 실행 시 즉시 양보합니다. 단독 토벌 전력이 부족하면 실제 군량·병력 수송으로 공동 집결하고, 도착 후 지휘관 보정을 포함한 전력을 다시 검사합니다. 초과 병력 이송에는 시간 내 도착하는 다른 도시의 군량 지원을 연결합니다. 공여 도시는 후방 5일/국경 15일분과 기존 보급 예약을 보존합니다. 국경 방어 부족 시 강한 병력을 유지하고, 그 외에는 큰 부대부터 손실을 줄입니다. 수송 기록은 자원을 변경하지 않고 실제 입고와 출발지 환불을 구분합니다.

이번 변경의 추가 모의검사 54건(건설 15·공동 토벌 14·수송 기록 10·초과 병력 이송 15)과 기존 회귀검사를 통과했습니다. 103개 Harmony 콜백을 설치 게임의 57개 대상 서명으로 검사했습니다. 이전 건설 취소 조건·이송 우선순위·보급 연결 누락을 복원한 대조군은 새 검사에서 실패했습니다. 실제 저장에서 수정 후 건설·토벌 출발·중산 병력 이송 및 FPS 개선은 아직 미확인입니다. 하내/업→상당 수송의 과거 소멸 원인은 기존 로그에 종결 기록이 없어 미확정이며, 이번 입고 추적을 통해 확인합니다.

`family-protection-r121`은 가문 사병 육성 보호(기본 켜짐)와 가문 전체 전근 제외(기본 꺼짐)를 F12 장수 배치 탭에 추가합니다. 실제 사병의 모집·훈련 잔여 여부로 완료 후 보호를 해제하며, 가문 전체 제외가 켜진 경우에는 유지합니다. 자동 배치·맞교환의 후보 및 실행 직전 검사, 바닐라 동일 세력 도시 이동 명령을 보호하고 직접 UI 명령은 허용합니다. 추가 모의검사 23건은 5명 육성 유지·다른 장수 배치, 완료 해제, 업무 표시 지연, 옵션 변경 중 재검사, 임관 상태, 가문 탈퇴, 직접 명령 등을 검사합니다. F12의 독립 토글·재발견 보존도 검사했습니다. 96개 Harmony 콜백·53개 대체 메서드를 설치된 게임 서명으로 검사했으며 실제 플레이 화면 및 장기 저장에서의 동작은 미확인입니다.

`attack-readiness-r120`은 전력 부족 공격의 날짜 만료 재편성을 제거하고, 마지막 실제 편성 전력과 가용 병력·지휘 여력을 비교해 재검토합니다. 전력 부족 외의 취소에는 기존 재시도 규칙을 유지합니다. 추가 162개 단언에는 73일 경과를 확인하는 두 반복 검사(146개 단언)가 포함됩니다. 동일 조건에서 실제 편성 함수를 호출하는 모의검사는 최초 실패 1회 뒤 추가 편성을 만들지 않았으며, 적 약화·병력 증가·지휘 여력 개선·소유권 및 저장 전환을 별도로 검사했습니다. 실제 r119 재시도 코드를 복원하면 새 검사가 실패합니다. 제보자가 제공한 52,080 대 122,999 기록은 전력 부족 판정을 뒷받침하지만, 이어지는 해산 줄은 다른 목표여서 같은 부대의 연속 기록으로 확정하지 않았습니다. 제보자의 73일·114회 수치는 독립 검증하지 못했으며 수정 후 실제 플레이 결과는 미확인입니다.

`dispatch-lifecycle-r119`는 실제 도시 이탈 직후 원점 근처 해산의 재시도 누락, 공격 편성의 비전력 취소 사유 누락, 적 전력 추정치의 미세 변화·중복 참조 정리에 따른 불필요한 재편성을 보완합니다. 추가 27개 모의검사에서 출발·경로 설치·즉시 해산 순서와 정상 도착·합류·병합·입성·재지시·전투 종료·부대 재사용의 제외 조건을 확인했습니다. 실제 r118 지원 관찰 코드와 공격 코드를 각각 복원한 두 대조군은 새 검사에서 실패합니다. 해산 직전 판정 사유와 제한된 호출 경로 기록을 추가했습니다. 제보의 발췌 세 줄만으로 최초 해산 사유나 다른 모드와의 충돌 여부를 확정하지는 않았습니다.

전투 지원 반복 편성 보완은 같은 실패 조건에서 143회 재판단, 이후 일일 재판단, 상황 변경 시 조기 재시도, 출발 전 취소와 정상 해산 구분, 전투·저장 전환, 군량 환급을 포함한 165개 추가 검사를 통과했습니다. 이전 실제 편성 코드를 사용한 대조군은 반복 생성 검사에 실패합니다. 제보자의 홍농 전투 원본 로그·저장은 제공되지 않았으므로 그 사례의 최초 취소 원인과 수정 후 실제 동작은 아직 확인하지 못했습니다.

실제 게임 참조 빌드, 93개 Harmony 콜백 연결 검사 및 AI·국고·등용 큐·설정 창 모의 회귀검사를 수행했습니다. 야영 정체 수정은 실제 로그의 호출 순서를 재현하여 기존 코드에서 실패하고 수정 코드에서 통과하는 것을 확인했습니다.

1.5.43 야영 지연 추가 수정은 실제 로그의 58~106초 귀환 판단 지연을 조사하여 아군 도시 전체 경로 선조사와 작업 대기열 경쟁을 보완합니다. 10·46·80·140개 도시, 도로 우회, 수용량·소유권 변경, 전투 진입, 정지 행군, 수백 개 작업의 동시 대기를 포함한 48개 추가 모의검사를 통과했습니다. 실제 도로 길이로 귀환지를 선택하며, 기하학적 하한은 이미 확인한 경로보다 짧을 수 없는 후보의 계산 생략에만 사용합니다. 게임 적용 후의 실제 지연·FPS 개선은 별도 로그 검증 대상입니다.

자동 업데이트는 44개 검사, 설치기는 37개 검사를 통과했습니다. 실제 BepInEx 로더를 격리된 폴더에서 실행해 GitHub에서 파일을 내려받고 기존 1.5.42를 1.5.43으로 교체하는 것을 확인했습니다. 게임 본체를 실행한 장기 플레이 검증과는 구분됩니다.

도시 배치는 현재 후보를 비교하는 제한된 탐색입니다. 모든 모드 건물 조합에 대한 수학적 전역 최적해나 실전 무결점을 보장하지 않습니다.

## 소스 빌드

Windows PowerShell, .NET 6 SDK/런타임, .NET SDK 7 이상 및 지원 게임의 BepInEx/interop 참조가 필요합니다. 기존 배포는 SDK 7.0.203과 .NET 6.0.16 참조로 빌드했습니다. 게임/BepInEx DLL은 각자의 설치본에서 참조합니다.

```powershell
.\Build.ps1 -GameDir 'D:\SteamLibrary\steamapps\common\LegendOfHeros'
.\Test.ps1 -GameDir 'D:\SteamLibrary\steamapps\common\LegendOfHeros'
```

빌드는 `src/StrategicSupport`, `src/TreasuryAwards`, `src/Settings`, `src/Updater`에 DLL을 생성하며 게임 파일은 바꾸지 않습니다. `Test.ps1`은 모의검사와 실제 게임 메서드 서명 검사를 수행하며 게임을 실행하지 않습니다. 과거 고장 DLL을 사용하는 내부 음성 대조군 검사는 공개 소스에 그 바이너리를 넣지 않아 제외했습니다. 설치용 ZIP은 릴리스 자산을 사용하세요. `installer`는 설치기 소스이며 `payload` 바이너리는 릴리스 ZIP에만 포함합니다.

## 2026-11-23 교체 배포 검증

1.5.44 교체본은 wall-construction-r133까지의 수정입니다. 건설 대기열·성벽 등록 및 복구·중간 전투 지원 경로에 대한 회귀검사를 포함합니다. 모의 검사의 게임 API는 대역을 사용하며 실제 플레이 전체의 성공을 뜻하지 않습니다. 실제 낙양 로그에서는 배치 계획 확정과 일반 건물 및 성벽 건물 착공 기록을 확인했으나, 모든 공사의 완공과 모든 전투 경로는 검증하지 않았습니다. 무도는 도시 규모 조건 때문에 학원 목표가 제외되는 상태를 확인했으며 이번 배포에서 해당 정책을 변경하지 않았습니다.

## 2026-11-23 방어·요격 수정 검증 (defense-intercept-r135)

지원 합류 점검 분리, 위협받는 현지 방어대 유지, 동맹 전투 지원대 차단의 목적 판정을 보완했습니다. 전체 회귀검사와 실제 게임 참조를 사용하는 빌드·Harmony 연결 검사를 통과했습니다. 추가 모의검사는 방어 16건·요격 18건이며, 원인 코드를 다시 넣는 음성 대조 4건도 각각 의도한 검사에서 실패했습니다. 모의검사의 이동·전투 API는 대역을 사용합니다. 수정본의 실제 게임 행동과 전후 프레임 비교는 아직 검증하지 않았습니다.
