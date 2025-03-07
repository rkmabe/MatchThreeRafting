using MatchThreePrototype.PlayAreaCellContent;
using System;

namespace MatchThreePrototype
{

    public class RaftStateMachine
    {
        public IContentState CurrentState { get; private set; }

        public event Action<IContentState> RaftStateChanged;

        public RaftIdleState RaftIdle;
        public RaftShudderState RaftShudder;

        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }

        public void Initialize(IContentState state)
        {
            CurrentState = state;
            state.Enter();

            // notify other objects that state has changed
            RaftStateChanged?.Invoke(state);
        }

        public void TransitionTo(IContentState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // notify other objects that state has changed
            RaftStateChanged?.Invoke(nextState);
        }

        public void CleanUpOnDestroy()
        {
            //IdleState.CleanUp..
            //ItemRemoving.CleanUpOnDestroy();
        }

        public RaftStateMachine(Raft raft)
        {

            RaftIdle = new RaftIdleState(raft);
            RaftShudder= new RaftShudderState(raft);

        }


    }
}