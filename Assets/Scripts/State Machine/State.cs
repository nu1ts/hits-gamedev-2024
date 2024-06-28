using UnityEngine;

namespace State_Machine
{
    public abstract class State : MonoBehaviour
    {
        public bool IsComplete { get; protected set; }

        private float _startTime;

        public float time => Time.time - _startTime;
        
        public Core core;
        
        protected Rigidbody2D Body => core.body;
        protected Animator Animator => core.animator;

        private StateMachine _machine;

        public StateMachine Parent;

        protected State CurrentState => _machine.State;

        protected void Set(State newState, bool forceReset = false)
        {
            _machine.Set(newState, forceReset);
        }
        public void SetCore(Core inputCore)
        {
            _machine = new StateMachine();
            core = inputCore;
        }
        
        public virtual void Enter() { }
        
        public virtual void Do() { }
        
        protected virtual void FixedDo() { }
        
        public virtual void Exit() { }

        public void DoBranch()
        {
            Do();
            CurrentState?.DoBranch();
        }
        
        public void FixedDoBranch()
        {
            FixedDo();
            CurrentState?.FixedDoBranch();
        }

        public void Initialise(StateMachine parent)
        {
            Parent = parent;
            IsComplete = false;
            _startTime = Time.time;
        }
    }
}