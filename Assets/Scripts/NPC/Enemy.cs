using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyCondition))]
[RequireComponent(typeof(EnemyAnimationController))]
public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; }
    public EnemyCondition Condition { get; private set; }
    public EnemyAnimationController Animation { get; private set; }

    public StateMachine StateMachine { get; private set; }
    public Dictionary<StateType, IState> stateMap = new Dictionary<StateType, IState>();

    
    [Header("Stats")]
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;
    public float detectDistance;

    [Header("Wandering")]
    public float minWanderDistance = 3f;
    public float maxWanderDistance = 10f;
    public float minWanderWaitTime = 3f;
    public float maxWanderWaitTime = 10f;

    [Header("Combat")]
    public float damage = 1;
    public float attackRate = 3f;
    public float lastAttackTime;
    public float attackDistance = 3f;

    public float PlayerDistance { get; private set; }
    [SerializeField] float playerCheckingRate = 1f;
    public float fieldOfView = 120f; // 시야각 - 전방 120도를 탐지하도록

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Condition = GetComponent<EnemyCondition>();
        Animation = GetComponent<EnemyAnimationController>();

        StateMachine = new StateMachine();
        stateMap.Add(StateType.Idle, new IdleState(this));
        stateMap.Add(StateType.Wander, new WanderState(this));
        stateMap.Add(StateType.Attack, new AttackState(this));
    }

    void Start()
    {
        StateMachine.SetState(stateMap[StateType.Wander]);
        
        // 비용 감안 - 상시 체크할거라면 1초 간격으로 해놓기
        InvokeRepeating("CheckPlayerDistance", 0f, playerCheckingRate);
    }

    void Update()
    {
        StateMachine.CurrentState.Execute();

        if (PlayerDistance < detectDistance)
        {
            // 공격 전환
            if(StateMachine.CurrentState != stateMap[StateType.Attack])
                StateMachine.SetState(stateMap[StateType.Attack]);
        }
    }

    void CheckPlayerDistance()
    {
        PlayerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
    }

    public Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        int i = 0;

        do
        {
            NavMesh.SamplePosition(transform.position + Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;

            if(i == 30) 
                break;
        }
        while(Vector3.Distance(transform.position, hit.position) < detectDistance);

        return hit.position;
    }

    public bool IsPlayerInFOV()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle < fieldOfView * 0.5f; // 120인데 좌우 방향
    }

}