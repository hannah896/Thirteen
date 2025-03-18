# Zombie Forest

**Zombie Forest**는 3인칭 3D 서바이벌 생존 게임입니다.  
플레이어는 자원을 수집하고, 이를 활용하여 생존해야 합니다.  
몬스터와의 전투 및 배고픔, 목마름 게이지 관리 등 다양한 생존 요소가 포함되어 있습니다.

## 🎮 주요 기능
- **자원 수집**: 필드에서 다양한 자원을 획득 가능
- **제작 시스템**: 수집한 자원으로 아이템 및 장비 제작
- **건축 시스템**: 건물을 건설하여 거점을 형성
- **전투 시스템**: 생존을 위협하는 적들과 전투 진행
- **생존 요소**: 배고픔과 목마름 게이지 관리

## 🕹 사용법 (Controls)

### 일반 조작
| 키 | 기능 |
|----|------|
| `W, A, S, D` | 이동 |
| `Shift` | 달리기 |
| `Space Bar` | 점프 |
| `Tab` | 인벤토리 오픈 |
| `T` | 생산 메뉴 오픈 |
| `Y` | 건축 메뉴 오픈 |
| `E` | 상호작용 (아이템 획득) |

### 건축 모드
| 키 | 기능 |
|----|------|
| `마우스 휠` | 건물 좌우 회전 |
| `R` | 건축 확정 |

## 🔧 개발 정보
- **엔진**: Unity 2022.3.17f1
- **플랫폼**: PC (추후 확장 가능)

## 📽 게임 플레이 영상

[![Zombie Forest Gameplay](https://img.youtube.com/vi/9oaVI6uJz2I/maxresdefault.jpg)](https://www.youtube.com/watch?v=9oaVI6uJz2I)

---
## 🛠 트러블슈팅 (Troubleshooting)

### ✅ Resource Auto Spawner 트러블슈팅  
**개요**  
본 스크립트는 Terrain 위에 자원(바위, 나무, 덤불, 상자 등)을 자동으로 배치해주는 시스템입니다.  
`NavMesh.SamplePosition()`을 통해 **NavMesh 위에만 스폰**되도록 안전성을 확보하며, 마지막에는 자동으로 NavMesh를 빌드합니다.  

### ⚠️ 문제 및 해결 사례  

#### 🛑 문제 1. LOD 그룹이 적용된 프리팹이 Terrain의 Tree Painter에 적용되지 않음  
**원인:**  
- Unity의 `Terrain TreePrototype` 시스템은 **LODGroup 및 복잡한 컴포넌트**를 지원하지 않음.  

**해결 방법:**  
- LOD가 필요한 나무는 `Tree Painter` 대신 **일반 GameObject로 배치**하는 방식으로 변경.  
- **Resource Auto Spawner**를 활용해 Terrain 위에 직접 배치.  

---

#### 🛑 문제 2. Terrain에 이미 배치된 나무(TreeInstance)를 GameObject로 변환하고 싶음  
**현실:**  
- Unity의 `TreeInstance`는 **GameObject가 아니며**, 콜라이더, 태그, 커스텀 컴포넌트 등을 추가할 수 없음.  

**해결 방법:**  
1. `TerrainData.treeInstances`를 가져와 위치 및 회전 정보를 읽음.  
2. 이를 기반으로 새 `GameObject`를 `Instantiate()`하여 배치.  
3. 기존 `TreeInstance` 데이터는 제거.  
4. 필요하면 Editor 스크립트 또는 플러그인을 활용하여 변환 자동화.  

---

#### 🛑 문제 3. Prefab에 LOD를 적용한 후 Terrain Tree Painter에 넣으면 그려지지 않음  
**원인:**  
- `TreePainter`는 `Mesh` 또는 `SpeedTree` 전용이며, **LOD 그룹이 적용된 프리팹은 허용하지 않음**.  

**해결 방법:**  
- Terrain의 `TreePainter`를 사용하지 않고, **Resource Auto Spawner**를 활용하여 직접 배치.  
- `NavMesh.SamplePosition()`을 사용해 **경계에서 일정 거리 이상 떨어진 위치**에만 배치하도록 개선.  

---
