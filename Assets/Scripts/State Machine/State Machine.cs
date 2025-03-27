namespace State_Machine
{
    public class StateMachine
    {
        public State State;

        public void Set(State newState, bool forceReset = false)
        {
            if (State == newState && forceReset) return;
            
            State?.Exit();
            State = newState;
            State.Initialise(this);
            State.Enter();
        }
    }
}