using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle.States
{

    public class ObstacleIdleState : IContentState
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

            if (_cell.ObstacleHandler.GetIsProcessingRemoval())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ObstacleRemoving);
                return;
            }

            if (_cell.ObstacleHandler.GetIsProcessingLanding())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ObstacleLanding);
                return;
            }

            if (_cell.ObstacleHandler.GetObstacle() == null)
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.CellEmpty);
                return;
            }
        }

        public ObstacleIdleState(PlayAreaCell cell)
        {
            _cell = cell;
        }

    }
}