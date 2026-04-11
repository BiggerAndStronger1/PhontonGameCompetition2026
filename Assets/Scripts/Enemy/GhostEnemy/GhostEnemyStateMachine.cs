public class GhostEnemyStateMachine
{
    public GhostEnemyState currentState { get; private set; }
    public void Initialize(GhostEnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(GhostEnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
