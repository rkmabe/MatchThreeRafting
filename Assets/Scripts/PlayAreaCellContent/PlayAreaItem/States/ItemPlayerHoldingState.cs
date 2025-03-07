using MatchThreePrototype.PlayAreaElements;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{
    public class ItemPlayerHoldingState : IContentState
    {

        private PlayAreaCell _cell;
        private Image _itemImage;

        public void Enter()
        {
            //Debug.Log("enter Player Holding");

            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.HELD_ALPHA_ON);
        }

        public void Exit()
        {
            //Debug.Log("exit Player Holding");

            if (_cell.ItemHandler.GetItem() == null)
            {
                _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_OFF);
            }
            else
            {
                _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_ON);
            }

        }

        public void Update()
        {

            if (_cell.ItemHandler.GetIsProcessingRemoval())
            {
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

            //if (_cell.ItemHandler.GetIsDynamiteActive())
            //{
            //    _cell.StateMachine.TransitionTo(_cell.StateMachine.DynamiteActive);
            //    return;
            //}


            if (!_cell.ItemHandler.GetIsPlayerHolding())
            {
                if (_cell.ItemHandler.GetItem() == null)
                {
                    //_itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_OFF);
                    _cell.StateMachine.TransitionTo(_cell.StateMachine.CellEmpty);
                }
                else
                {
                    //_itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_ON);
                    _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemIdle);
                }
            }
        }

        public ItemPlayerHoldingState(PlayAreaCell cell)
        {
            _cell = cell;
            _itemImage = cell.ItemHandler.GetImage();
        }

    }
}