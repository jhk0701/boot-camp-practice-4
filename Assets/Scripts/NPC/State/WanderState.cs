public class WanderState : IState
{
    public Enemy enemy;
    public WanderState(Enemy e)
    { 
        enemy = e;
    }


    public void Enter()
    {
        enemy.Agent.SetDestination(enemy.GetWanderLocation());
        enemy.Agent.speed = enemy.walkSpeed;
        enemy.Agent.isStopped = false;

        enemy.Animation.SetMove(true);
        enemy.Animation.SetSpeed(enemy.Agent.speed / enemy.walkSpeed);
    }

    public void Execute()
    {
        if (enemy.Agent.remainingDistance < 0.1f)
        {
            // Idle로 전환
            enemy.StateMachine.SetState(enemy.stateMap[StateType.Idle]);
        }
    }

}