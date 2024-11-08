
using System;
using UnityEngine;

public class IdleState : IState
{
    public Enemy enemy;
    float waitTime;
    float time;

    public IdleState(Enemy e)
    { 
        enemy = e;
    }


    public void Enter()
    {
        enemy.Agent.speed = enemy.walkSpeed;
        enemy.Agent.isStopped = true;

        waitTime = UnityEngine.Random.Range(enemy.minWanderWaitTime, enemy.maxWanderDistance);
        time = Time.time;

        enemy.Animation.SetMove(false);
        enemy.Animation.SetSpeed(enemy.Agent.speed / enemy.walkSpeed);
    }

    public void Execute()
    {
        if(Time.time - time > waitTime)
        {
            enemy.StateMachine.SetState(enemy.stateMap[StateType.Wander]);
            time = Time.time;
        }
    }

}