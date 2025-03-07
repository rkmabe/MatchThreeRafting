using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{

    public class ItemIdleState : IContentState
    {
        //private ItemHandler _itemHandler;
        private PlayAreaCell _cell;

        private static float MIN_SECS_IDLE = 2;
        private static float MAX_SECS_IDLE = 20;

        private float _secsIdle = 0;

        private float _randomDuration = MIN_SECS_IDLE;

        public override string ToString()
        {
            return this.GetType().Name;
        }

        public void Enter()
        {
            _secsIdle = 0;

            _randomDuration = UnityEngine.Random.Range(MIN_SECS_IDLE, MAX_SECS_IDLE);

        }

        public void Exit()
        {
        }



        public void Update()
        {
            //if (_itemHandler.GetItem() == null)
            //{
            //    return;
            //}

            if (Time.deltaTime == 0)
            {
                return;
            }

            if (_cell.ItemHandler.GetIsPlayerHolding())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.PlayerHoldingItem);
                return;
            }

            if (_cell.ItemHandler.GetIsObstacleCrushing())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ObstacleCrushingItem);
                return;
            }

            if (_cell.ItemHandler.GetIsBlockObscuring())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.BlockObscuringItem);
                return;
            }

            if (_cell.ObstacleHandler.GetObstacle() != null)
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ObstacleIdle);
                return;
            }

            if (_cell.BlockHandler.GetBlock() != null)
            {
                _cell.StateMachine.TransitionTo((_cell.StateMachine.BlockIdle));
                return;
            }

            if (_cell.ItemHandler.GetItem() == null)
            {
                //Debug.LogWarning("THIS SHOULD NOT BE POSSIBLE?");
                _cell.StateMachine.TransitionTo(_cell.StateMachine.CellEmpty);
                return;
            }

            //if (_itemHandler.IsProcessingRemoval)
            //{
            //    _itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.RemovingState);
            //    return;
            //}

            if (_cell.ItemHandler.GetIsProcessingRemoval())
            {
                //if (_cell.ItemHandler.GetItem().ItemType == ItemTypes.CannonBall)
                if (_cell.ItemHandler.GetItem().ItemType == ItemTypes.CannonBall || _cell.ItemHandler.GetItem().ItemType == ItemTypes.CannonBallStack)
                {
                    _cell.StateMachine.TransitionTo(_cell.StateMachine.AmmoRemoving);
                }
                else
                {
                    _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemRemoving);
                }
                return;
            }

            if (_cell.ItemHandler.GetIsProcessingLanding())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemLanding);
                return;
            }

            if (_cell.ItemHandler.GetIsDynamiteActive())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.DynamiteActive);
                return;
            }

            //if (_cell.ItemHandler.GetIsObstacleCrushing())
            //{
            //    _cell.StateMachine.TransitionTo(_cell.StateMachine.ObstacleCrushingItem);
            //    return;
            //}


            if (_secsIdle < _randomDuration)
            {
                _secsIdle += Time.deltaTime;
            }
            else
            {
                if (UnityEngine.Random.value > .5f)
                {
                    //_itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.SizeFluctuateState);
                    _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemSizeFlucuate);
                }
                else
                {
                    //_itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.RotationFluctuateState);
                    _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemRotationFluctuate);
                }
            }

        }


        //public ItemIdleState(ItemHandler itemHandler)
        //{
        //    _itemHandler = itemHandler;
        //}
        public ItemIdleState(PlayAreaCell cell)
        {
            _cell = cell;
        }
    }
}
