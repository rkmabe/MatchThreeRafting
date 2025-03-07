using MatchThreePrototype.PlayAreaElements;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock.States
{



    public class BlockAndItemRemovingState : IContentState
    {

        private PlayAreaCell _cell;


        private Image _itemImage;
        private Image _blockImage;

        internal static float IGNORE_SETTINGS_DURATION = .33f;//3;//.5f;
        private float _removalDuration = IGNORE_SETTINGS_DURATION;
        private float _secsRemovalProcessing = 0;

        public void Enter()
        {
            _secsRemovalProcessing = 0;
        }
        public void Exit() 
        {
            _cell.BlockHandler.FinishBlockAndItemRemoval();
            _cell.ItemHandler.FinishRemoval();


        }
        public void Update()
        {

            if (Time.deltaTime == 0)
            {
                return;
            }

            float alphaLerp;
            if (_secsRemovalProcessing < _removalDuration)
            {
                alphaLerp = Mathf.Lerp(Statics.BLOCK_ALPHA_ON, Statics.ALPHA_OFF, _secsRemovalProcessing / _removalDuration);

                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, alphaLerp);
                _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, alphaLerp);

                _secsRemovalProcessing += Time.deltaTime;
            }
            else
            {
                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, Statics.ALPHA_OFF);
                _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_OFF);

                _cell.StateMachine.TransitionTo(_cell.StateMachine.CellEmpty);
            }
        }

        public BlockAndItemRemovingState(PlayAreaCell cell)
        {
            _cell = cell;

            _itemImage = cell.ItemHandler.GetImage();
            _blockImage = cell.BlockHandler.GetImage();

        }

    }
}