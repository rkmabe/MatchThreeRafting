using MatchThreePrototype.Controllers;
using MatchThreePrototype.PlayAreaElements;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock.States
{

    public class BlockRemovingState : IContentState
    {

        private float _secsRemovalProcessing = 0;

        internal static float IGNORE_SETTINGS_DURATION = .33f;//3;//.5f;

        //internal static float DEFAULT_REMOVAL_DURATION = .5f;
        private float _removalDuration = IGNORE_SETTINGS_DURATION;

        private PlayAreaCell _cell;
        private Image _blockImage;

        public override string ToString()
        {
            return this.GetType().Name;
        }

        public void Enter()
        {
            _removalDuration = IGNORE_SETTINGS_DURATION;

            _secsRemovalProcessing = 0;
        }

        public void Exit()
        {
            _cell.BlockHandler.FinishRemoval();
        }

        public void Update()
        {

            if (Time.deltaTime == 0)
            {
                return;
            }

            // TODO: change to transition to next block level, if any.
            // although only one level is currently in use, this is set up for mulitple levels.

            float alphaLerp;
            if (_secsRemovalProcessing < _removalDuration)
            {

                alphaLerp = Mathf.Lerp(Statics.BLOCK_ALPHA_ON, Statics.ALPHA_OFF, _secsRemovalProcessing / _removalDuration);

                //Image image = _cell.BlockHandler.GetImage();
                //image.color = new Color(image.color.r, image.color.g, image.color.b, alphaLerp);

                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, alphaLerp);

                _secsRemovalProcessing += Time.deltaTime;
            }
            else
            {
                //Image image = _cell.BlockHandler.GetImage();
                //image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, Statics.ALPHA_OFF);

                _cell.StateMachine.TransitionTo(_cell.StateMachine.CellEmpty);


            }

        }

        internal void OnNewRemoveDuration(float duration)
        {
            _removalDuration = duration;
        }

        internal void CleanUpOnDestroy()
        {
            SettingsController.OnNewRemoveDurationDelegate -= OnNewRemoveDuration;
        }

        public BlockRemovingState(PlayAreaCell cell)
        {
            _cell = cell;
            _blockImage = _cell.BlockHandler.GetImage();

            SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;
        }


    }
}