using MatchThreePrototype.PlayAreaElements;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{

    public class ItemBlockObscuringState : IContentState
    {
        private PlayAreaCell _cell;

        //private RectTransform _itemRectTransform;

        private RectTransform _blockRectTransform;
        private Image _blockImage;

        private static Vector2 BLOCK_ANCHOR_MIN_ABOVE = new Vector2(0, 1);
        private static Vector2 BLOCK_ANCHOR_MAX_ABOVE = new Vector2(1, 2);

        private static Vector2 BLOCK_ANCHOR_MIN_DEFAULT = new Vector2(0, 0);
        private static Vector2 BLOCK_ANCHOR_MAX_DEFAULT = new Vector2(1, 1);

        private float _blockObscureMoveStep = 5f; //.05f;  //.5f;
        private float _blockAlphaStep = 7;

        public void Enter()
        {
            _blockRectTransform.anchorMax = BLOCK_ANCHOR_MAX_ABOVE;
            _blockRectTransform.anchorMin = BLOCK_ANCHOR_MIN_ABOVE;
            _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, Statics.ALPHA_OFF);

        }
        public void Exit() 
        {

            _cell.ItemHandler.FinishBlockObscuring();

            _blockRectTransform.anchorMin = BLOCK_ANCHOR_MIN_DEFAULT;
            _blockRectTransform.anchorMax = BLOCK_ANCHOR_MAX_DEFAULT;

            _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, Statics.BLOCK_ALPHA_ON);

        }
        public void Update()
        {

            if (Time.deltaTime == 0)
            {
                return;
            }

            // covers special case if dynamite is dragged onto a cell while obstacle crushing is in progress.
            if (_cell.ItemHandler.GetIsDynamiteActive())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.DynamiteActive);
                return;
            }

            _blockRectTransform.anchorMin = Vector2.MoveTowards(_blockRectTransform.anchorMin, BLOCK_ANCHOR_MIN_DEFAULT, _blockObscureMoveStep);
            _blockRectTransform.anchorMax = Vector2.MoveTowards(_blockRectTransform.anchorMax, BLOCK_ANCHOR_MAX_DEFAULT, _blockObscureMoveStep);

            // fade in the image
            float alpha = Mathf.MoveTowards(_blockImage.color.a, Statics.BLOCK_ALPHA_ON, _blockAlphaStep);
            _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, alpha);

            if (Statics.IsCloseEnough(_blockRectTransform.anchorMin, BLOCK_ANCHOR_MIN_DEFAULT, .01f))
            {
                //_blockRectTransform.anchorMin = BLOCK_ANCHOR_MIN_DEFAULT;
                //_blockRectTransform.anchorMax = BLOCK_ANCHOR_MAX_DEFAULT;

                _cell.StateMachine.TransitionTo(_cell.StateMachine.BlockIdle);
            }
        }

        public ItemBlockObscuringState(PlayAreaCell cell)
        {
            _cell = cell;

            //_itemRectTransform = _cell.ItemHandler.GetImage().GetComponent<RectTransform>();

            _blockRectTransform = _cell.BlockHandler.GetImage().GetComponent<RectTransform>();

            _blockImage = _cell.BlockHandler.GetImage();

            _blockObscureMoveStep *= Time.deltaTime;
            _blockAlphaStep *= Time.deltaTime;
        }

    }
}