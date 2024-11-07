# boot-camp-practice-4

# Q1 분석
* Equipment와 EquipTool 기능의 구조와 핵심 로직을 분석해보세요.
* Resource 기능의 구조와 핵심 로직을 분석해보세요.

# Equipment
* 플레이어의 장비 장착을 관리하는 클래스입니다.
* 현재는 플레이어의 공격키(클릭)의 입력을 받아 처리하는 기능도 같이 수행하고 있습니다.

## 주요 메서드
EquipNew(ItemData data) :
이 메서드를 통해서 UIInventory에서 선택한 장비 아이템을 착용할 수 있습니다.
매개변수로 새로운 장비에 대한 데이터(ItemData)를 받고, 이 데이터에 저장된 equipPrefab에서 장비 아이템을 인스턴스화합니다.
만들어진 equip 인스턴스는 curEquip에 저장됩니다.

이때, 만일 장비를 착용 중일 수 있으므로 Unequip 메서드로 장착을 해제해줍니다.

Unequip() :
이 메서드를 호출하면 현재 장착 중인 장비를 해제합니다.
장착하면서 만들어둔 equip 인스턴스는 파괴하고 curEquip을 null 둡니다.

OnAttackInput(InputAction.CallbackContext context) : 
플레이어의 공격 입력을 받고, 현재 공격할 수 있는 상황이라면
착용중인 장비 curEquip(Equip 클래스)의 OnAttackInput 메서드를 호출하고 있습니다.
* 공격 할 수 있는 조건
    1. 공격 키 입력을 받고 있다.
    2. 장비를 장착 중이다.
    3. UI 창을 열고 있지 않다.

# Equip
* 장비를 표현하는 클래스입니다.
* 현재 이 클래스를 그대로 쓰진 않고, 이를 상속받는 EquipTool을 장비 아이템에 컴포넌트로 붙여 사용 중입니다.
* 또한 OnAttackInput 메서드를 가상 메서드로 선언하여 모든 장비에서 공통적으로 사용할 수 있도록 보장하고 있습니다. 

# EquipTool
* 실질적으로 장비를 표현하는 클래스입니다.
* Equip을 상속받아 OnAttackInput 메서드를 오버라이드하여
외부에서 OnAttackInput 호출하면 EquipTool이 재정의한 방식을 사용할 수 있도록 합니다.
* 또한 장비의 사용에 대한 애니메이션을 보유하고 있고 장비를 사용하면 애니메이션이 같이 실행되고 있습니다.
* 애니메이션이 실행되는 특정 구간에 Animation Event를 설정하여 OnHit 메서드를 호출하고 있습니다.

## 주요 메서드
OnAttackInput() :
플레이어가 공격키를 입력하면 Equipment가 이를 받아 Equip의 OnAttackInput()를 호출합니다.
Equip에서 가상 메서드로 선언된 OnAttackInput()를 EquipTool에서 다음과 같이 오버라이드했습니다.

현재 공격중이지 않다면, 플레이어의 스태미너의 따라 공격 상태로 전환하고 애니메이션을 실행합니다.
이때 공격의 시간을 나타내는 attackRate 만큼의 시간이 지난 뒤 비공격 상태로 전환합니다. 

if (!attacking)
{
    if(CharacterManager.Instance.Player.condition.UseStamina(useStamina))
    {
        attacking = true; // 공격 상태 true
        animator.SetTrigger("Attack");
        Invoke("OnEnableAttack", attackRate);
    }
}

OnEnableAttack() :
attackRate 이후에 Invoke메서드로 호출합니다.
이 메서드가 호출되면 비공격 상태로 전환됩니다.

attacking = false;

OnHit() :
공격 애니메이션에 설정되어있는 Animation Event를 통해서 호출되는 메서드입니다.
플레이어의 화면 정중앙에 attackDistance 길이의 레이캐스트를 투사하여 접촉한 대상을 검사합니다.

접촉을 처리하는 방식은 이 클래스의 조건에 따라 달라집니다.
EquipTool이 채집용 도구일때, 접촉한 대상이 Resource 컴포넌트를 갖고 있다면 자원을 채취합니다.
EquipTool이 공격용 도구일때, IDamagable 인터페이스를 상속한 컴포넌트라면 데미지를 줍니다.

Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
RaycastHit hit;

if (Physics.Raycast(ray, out hit, attackDistance))
{
    if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
    {
        resource.Gather(hit.point, hit.normal);
    }
    else if(doesDealDamage && hit.collider.TryGetComponent(out IDamagable damagable))
    {
        damagable.TakePhysicalDamage(damage);
    }
}

# Resource
채취 가능한 자원을 표현하는 클래스입니다.
인스펙터에서 채취 시 제공하는 아이템(itemToGive)과 채취 가능량(capacy), 1회 채취 시 제공량(quantityPerHit)을 입력하여
플레이어가 얻을 수 있는 자원을 설정할 수 있습니다.

## 주요 메서드
Gather(Vector3 hitPoint, Vector3 hitNormal) :
EquipTool의 OnHit 메서드에서 조건에 따라 Resource 컴포넌트와 접촉했다면
접촉 지점(hitPoint), 접촉면(hitNormal)을 매개변수로 하는 Gather 메서드를 호출합니다.

인스펙터에서 지정한 1회 채취 시 제공량만큼 아이템을 제공하고 채취 가능량을 1씩 깎습니다.
아이템 제공은 itemToGive에 할당한 아이템을 인스턴스화해줍니다.
이때 접촉 지점의 위치에서 1만큼 높은 곳에서, 접촉면을 바라보는 각도로 아이템을 생성합니다.

채취 가능량이 0이 되면 더 이상 아이템은 제공하지 않습니다.

for (int i = 0; i < quantityPerHit; i++)
{
    if(capacy <= 0)
        break;
    
    capacy -= 1;

    Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
}


# Q2 분석
- AI 네비게이션 시스템에서 가장 핵심이 되는 개념에 대해 복습해보세요.
- NPC 기능의 구조와 핵심 로직을 분석해보세요.

## AI 네비게이션 시스템
유니티에서 제공하는 길 찾기(Path Finding) 시스템입니다.
내장된 A-Star 알고리즘을 이용하여 캐릭터는 지정된 위치로 가장 효과적인 경로를 탐색하여 이동할 수 있습니다.

핵심 요소
* NavMeshSurface : 
캐릭터가 이동할 수 있는 표면을 정의합니다. 
3D 공간에서 캐릭터가 움직일 수 있는 영역으로 생각할 수 있습니다.
기본적으로 MeshRenderer를 통해서 3D 오브젝트의 표면(위쪽면)으로 영역을 구합니다.

Unity 구버전 방식에서는 씬에 배치된 3D 오브젝트에 navigation static을 설정하고
지정한 표면을 bake하여 캐릭터가 움직일 표면을 구성했습니다.

* Area :
캐릭터가 움직일 표면에 대해 Layer처럼 특별한 설정할 수 있습니다.
기본적으로 Walkable, Not Walkable, Jumpable을 제공하며
플레이어가 원한다면 새로운 Area를 정의할 수 있습니다.

이때, Area에 대해 Cost를 설정하여 캐릭터가 경로 계산 시, 특정 경로를 우선순위에서 밀어내거나, 앞당기는 등 다양한 활용을 할 수 있습니다.

* NavMeshAgent : 
AI 네이게이션의 중요 요소로 실질적으로 움직이고 경로를 계산하는 역할을 수행합니다.
주로 목표 지점을 설정하고 이에 대해 경로를 계산하여 움직이도록 합니다.
경로 계산 시, 장애물이나 비용이 높은 경로가 발생한다면 검사를 통해서 새로운 경로를 탐색할 수 있습니다.

* NavMeshObstacle :
캐릭터가 지날 수 없는 영역을 설정합니다.
박스 또는 캡슐 형태로 설정할 수 있고, 설치된 장애물을 캐릭터가 만나면 이 주변을 돌아서 목표지점을 지나려할 것입니다.

이때 Carve 옵션을 활성화하면,
장애물의 영역이 기존에 만들어진 네브 메쉬 표면에 구멍을 뚫어
캐릭터가 아예 다른 경로를 탐색하도록 할 수 있습니다.


## NPC 기능의 구조와 핵심 로직을 분석해보세요.
NPC의 상태를 크게 Idle, Wandering, Attacking 3가지로 정의했고
현재 NPC의 상태에 따라 다른 행동을 취하도록 구조가 설계되어 있습니다.
SetState(AIState) 메서드를 사용하여 NPC의 상태를 변경합니다.

public void SetState(AIState state)
{
    aiState = state;

    switch(aiState)
    {
        case AIState.Idle:
            agent.speed = walkSpeed;
            agent.isStopped = true;
            break;

        case AIState.Wandering:
            agent.speed = walkSpeed;
            agent.isStopped = false;
            break;

        case AIState.Attacking:
            agent.speed = runSpeed;
            agent.isStopped = false;
            break;
    }

    animator.speed = agent.speed / walkSpeed;
}


Update 메서드에서 AIState에 따라 PassiveUpdate, AttackingUpdate를 별도로 호출하여
각각의 상태에 맞는 행동을 취하도록 구현하고 있습니다.
switch(aiState)
{
    case AIState.Idle:
    case AIState.Wandering:
        PassiveUpdate();
        break;
    case AIState.Attacking:
        AttakingUpdate();
        break;
}

그리고 모든 상태에서 Update 메서드를 통해서 플레이어(타겟)과 자신의 거리를 계산하고
Idle, Wandering 상태에서 일정 거리 안(detectDistance)으로 들어오면 Attacking 상태로 전환하여 플레이어를 공격합니다.

void PassiveUpdate()
{
    if(aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
    {
        SetState(AIState.Idle); // 잠깐 멈추는 것
        Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
    }

    if (playerDistance < detectDistance)
    {
        SetState(AIState.Attacking);
    }
}

### 주요 메서드와 상태
* Wandering 상태 : 
시작 시 NPC의 상태이며 Wandering 상태에 돌입하면 NavMeshAgent의 SetDestination 메서드로 지정된 위치로 이동하고, 목표 지점에 가까워졌다면 잠시 대기 후 Idle 상태로 전환합니다.
if(aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
{
    SetState(AIState.Idle); // 잠깐 멈추는 것
    Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
}

Wandering 상태에서는 NPC가 움직이고 있다면 Walk 애니메이션을 실행합니다.

단, 이때 플레이어가 탐지 범위 안으로 돌아온다면 Attacking으로 전환합니다.
if (playerDistance < detectDistance)
{
    SetState(AIState.Attacking);
}

* Idle 상태 :
Wandering 상태에 있던 NPC가 목표 지점에 거의 가까워졌다면 Idle 상태로 전환됩니다.
Idle 상태에서 minWanderWaitTime ~ maxWanderWaitTime 사이의 랜덤한 시간이 지나면 다시 Wandering 상태로 돌아갑니다.

Idle 상태에서는 NPC의 Idle 애니메이션을 실행합니다.

단, 이때 플레이어가 탐지 범위 안으로 돌아온다면 Attacking으로 전환합니다.

* WanderToNewLocation() 메서드:
현재상태가 Idle일 경우에 NavMeshAgent를 통해 새로운 목표지점을 설정하고 Wandering 상태로 전환합니다.

void WanderToNewLocation()
{
    if (aiState != AIState.Idle)
        return;
    
    SetState(AIState.Wandering);
    agent.SetDestination(GetWanderLocation());
}

* GetWanderLocation() 메서드
NPC가 움직일 목표 위치를 찾는 메서드입니다.
NavMesh.SamplePosition 메서드를 통해 NPC의 반경 minWanderDistance ~ maxWanderDistance만큼의 값 중 랜덤한 위치를 찾습니다.
이때, NPC가 가지 못하는 곳이 위치로 선택될 수 있기에 최대 30회까지 시도하여 값을 구합니다.
(NPC가 가지 못하는 위치는 현재 위치에서 NPC의 탐색 범위를 벗어나는 곳입니다.)

Vector3 GetWanderLocation()
{
    NavMeshHit hit;
    NavMesh.SamplePosition(transform.position + Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
    
    int i = 0;
    // TODO do-while문으로 수정해볼 것
    while (Vector3.Distance(transform.position, hit.position) < detectDistance)
    {
        NavMesh.SamplePosition(transform.position + Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
        i++;

        if(i == 30) 
            break;
    }

    return hit.position;
}

* Attacking 상태 :
플레이어가 NPC의 탐지 범위 안으로 들어왔다면 전환되는 상태입니다.
Attacking 상태에서는 Update 메서드에서 AttakingUpdate() 메서드를 호출합니다.

Update에서 구한 플레이어와 NPC의 거리가 공격범위(attackDistance) 내에 있다면
그리고 NPC의 시야각도인 120내에 있다면 플레이어에게 데미지를 가합니다.

if(playerDistance < attackDistance && IsPlayerInFOV())
{
    agent.isStopped = true;
    if (Time.time - lastAttackTime > attackRate)
    {
        lastAttackTime = Time.time;
        CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
        animator.speed = 1;
        animator.SetTrigger("Attack");
    }
}

데미지를 가할 때 구체적은 플레이어의 특정 클래스가 아닌 IDamagable 인터페이스를 찾아
TakePhysicalDamage(float damage)를 호출하고 있습니다.

* bool IsPlayerInFOV():
NPC의 시야각에 플레이어가 있는지 계산하는 메서드입니다.
플레이어에 대한 NPC의 방향 벡터를 구하고,
플레이어의 정면과 플레이어를 향한 벡터를 Vector3.Angle 메서드의 매개변수로 넣어
플레이어와 캐릭터 정면에 대한 각도를 구합니다.

이때 120도는 정면을 기준으로 좌우로 각각 60도를 나타내므로 120 / 2f 해주고 있습니다.
bool IsPlayerInFOV()
{
    Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
    float angle = Vector3.Angle(transform.forward, directionToPlayer);

    return angle < fieldOfView * 0.5f; // 120인데 좌우 방향
}


만약 위의 공격 조건에 만족하지 못한다면
NavMeshAgent를 통해서 플레이어의 위치로 가는 경로를 찾습니다.
플레이어에게 가는 경로를 찾았다면 그곳을 목적지로 설정하고 움직입니다.

만약 경로를 찾지 못했다면 제자리에서 멈추고 다시 Wandering 상태로 전환합니다.
else
{
    if (playerDistance < detectDistance)
    {
        agent.isStopped = false;
        NavMeshPath path = new NavMeshPath();

        if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
        {
            agent.SetDestination(CharacterManager.Instance.Player.transform.position);
        }
        else
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(AIState.Wandering);
        }
    }
    else
    {
        agent.SetDestination(transform.position);
        agent.isStopped = true;
        SetState(AIState.Wandering);
    }
}

# Q3 분석
- 보간에 대해 학습하고 선형보간(Lerp)과 구면선형보간(Slerp)에 대해 학습해보세요.
- 근사값(`Mathf.Approximately`)을 사용하는 이유에 대해 학습해보세요.

## 보간
보간이란 두 값의 사이에 있는 중간 값을 찾는 방법으로
Unity에서는 Lerp 메서드를 통해 선형보간을 구현하고 Slerp를 통해서 구면선형보간을 구현하고 있습니다.

### 선형 보간 Lerp (Linear Interpolate):
선형 보간이란 a, b 두 점이 있다고할 때, a, b를 잇는 직선을 긋고 직선상의 값을 구하는 방법입니다.

* Mathf.Lerp(float a, float b, float t) :
값 a와 값 b를 잇는 직선을 긋고, 이 직선의 길이를 0~1로 정규화합니다.
매개변수로 받은 t값은 0 ~ 1의 실수이며, t값에 해당하는 직선 상의 위치를 찾아 값을 반환합니다

예를 들어, a가 0f, b가 4f인 경우에 a와 b를 잇는 직선의 길이는 4f 길이입니다.
시작점인 a의 위치는 t가 0을 의미하고, 끝점인 b의 위치는 t가 1이라는 것을 의미합니다.

이때 t가 0.5f이라면
a, b 사이의 정확히 중간 지점을 가리키므로 보간 결과는 2f를 반환합니다.

* Vector3.Lerp(Vector3 a, Vector3 b, float t)
위에서 본 Lerp와 동일한 원리를 3차원으로 표현하는 메서드입니다.
3차원 좌표계에서 점 a와 점 b를 직선으로 잇는 선분의 t 위치를 구합니다.

### 구면 선형 보간 Slerp (Spherically Interpolate):
Lerp가 두 점 a, b를 잇는 직선 상에서 값을 찾는 방법이라면
Slerp는 두 점 a, b가 직선이 아니라 구의 표면에 있다고 가정하고 그 사이의 값을 찾는 방법입니다.

* Vector3 Slerp(Vector2 a, Vector2 b, float t)
3차원 좌표계에서 점 a, 점 b가 있다고 할 때,
a, b를 어떤 구체의 평면에 있다고 가정하고 이 둘을 잇는 호(포물선)를 긋습니다.
이렇게 그어진 호를 선형 보간합니다.

### 차이
* Lerp 선형 보간 : 두 점이 평평한 면 위에 있어서 그 둘을 잇는 직선을 통해 보간하는 방법
* Slerp 구면 선형 보간 : 두 점이 구의 표면에 있어서 그 둘을 잇는 호(포물선)을 통해 선형 보간하는 방법

## 근사값(`Mathf.Approximately`)을 사용하는 이유
우리가 쓰는 float의 정확한 명칭은 부동소수점(floatint point)입니다.
float이란 단어는 떠다닌다는 의미로 소수점이 고정되어 있지 않다는 의미입니다.

컴퓨터가 2진법으로 실수를 표현할 때, 소수점의 위치를 고정하지 않고 그 위치를 나타내는 수를 따로 적어둡니다.
값을 지수(소수점의 위치)와 가수(유효숫자)로 나누어 표현하는 것입니다.

만약 어떤 값을 2진법으로 변환할 때
맨 앞자리의 1의 바루 뒤로 소수점을 옮겨서 표현하고, 옮긴 만큼의 값을 2 ^ n (지수)로 저장합니다.
그리고 소수점 이하의 숫자들을 가수에 저장합니다.
다만 깔끔하게 나누어 떨어지지 않는 부분은 표현이 가능한 만큼만 가져가다보니
이를 다시 10진수로 표현했을 때 완전히 동일한 값이 나오진 않습니다. (최대한 근사치의 값이 산출됨)

이러한 문제로 인해서 0.1f + 0.2f != 0.3f인 경우가 발생하는 것입니다.
float을 사용할 때, 정확한 연산은 이러한 오차가 발생할 가능성이 매우 높기 때문에
Mathf.Approximately를 이용하여 근사값을 통한 계산을 수행하는 것입니다.