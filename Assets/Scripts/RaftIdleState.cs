using MatchThreePrototype.PlayAreaCellContent;

namespace MatchThreePrototype
{

    public class RaftIdleState : IContentState
    {

        private Raft _raft;


        public void Enter()
        {
            //Debug.Log("Raft ENTER idle state");
        }

        public void Exit()
        {
            //Debug.Log("Raft EXIT idle state");
        }

        public void Update()
        {
            //throw new System.NotImplementedException();

            if (_raft.IsReactingToDamage)
            {
                _raft.StateMachine.TransitionTo(_raft.StateMachine.RaftShudder);
                return;
            }

        }

        public RaftIdleState(Raft raft)
        {
            _raft = raft;
        }

    }

}
