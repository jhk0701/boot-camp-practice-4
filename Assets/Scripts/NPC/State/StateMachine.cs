public enum StateType
{
    Idle,
    Wander,
    Attack,
}

public class StateMachine
{
    public IState CurrentState { get; private set; }

    public void SetState(IState newState)
    {
        // if(CurrentState != null)
        //     CurrentState.Exit();

        CurrentState = newState;

        CurrentState.Enter();
    }
}