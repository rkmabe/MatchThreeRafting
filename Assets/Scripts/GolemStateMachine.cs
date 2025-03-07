using MatchThreePrototype.PlayAreaCellContent;
using System;

namespace MatchThreePrototype
{
    public class GolemStateMachine
    {

        public IContentState CurrentState { get; private set; }

        public event Action<IContentState> GolemStateChanged;

        public GolemOffScreenState GolemOffScreen;

        public GolemEntryState GolemEntry;
        public GolemIdleState GolemIdle;
        public GolemAttackState GolemAttack;
        public GolemCockyState GolemCocky;


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
            GolemStateChanged?.Invoke(state);
        }

        public void TransitionTo(IContentState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // notify other objects that state has changed
            GolemStateChanged?.Invoke(nextState);
        }

        public void CleanUpOnDestroy()
        {
            //IdleState.CleanUp..
            //ItemRemoving.CleanUpOnDestroy();
        }

        public GolemStateMachine(Golem golem)
        {
            GolemOffScreen = new GolemOffScreenState(golem);
            GolemIdle = new GolemIdleState(golem);
            GolemEntry = new GolemEntryState(golem);
            GolemAttack = new GolemAttackState(golem);
            GolemCocky = new GolemCockyState(golem);
        }
    }
}
