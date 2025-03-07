using MatchThreePrototype.PlayAreaCellContent;
using MatchThreePrototype.PlayAreaElements;

namespace MatchThreePrototype
{

    public class PlayAreaCellEmptyState : IContentState
    {

        private PlayAreaCell _cell;

        public void Enter()
        {
        }
        public void Update()
        {

            if (_cell.ItemHandler.GetItem() != null)
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemIdle);
            }
            else if (_cell.ObstacleHandler.GetObstacle() != null)
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ObstacleIdle);
            }
            else if (_cell.BlockHandler.GetBlock() != null)
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.BlockIdle);
            }

        }
        public void Exit()
        {


        }

        public PlayAreaCellEmptyState(PlayAreaCell cell)
        {
            _cell = cell;
        }
    }
}
