
using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock.States
{

    public class BlockIdleState : IContentState
    {
        private PlayAreaCell _cell;

        public override string ToString()
        {
            return this.GetType().Name;
        }
        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {

            if (Time.deltaTime == 0)
            {
                return;
            }


            if (_cell.ItemHandler.GetIsDynamiteActive())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.DynamiteActive);
                return;
            }

            if (_cell.BlockHandler.GetIsProcessingRemoval())
            {
                //_blockHandler.StateMachine.TransitionTo(_blockHandler.StateMachine.RemovingState);
                _cell.StateMachine.TransitionTo(_cell.StateMachine.BlockRemoving);
                return;
            }

            if (_cell.BlockHandler.GetIsProcessingBlockAndItemRemoval())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.BlockAndItemRemoving);
            }
        }

        public BlockIdleState(PlayAreaCell cell)
        {
            _cell = cell;
        }
    }
}