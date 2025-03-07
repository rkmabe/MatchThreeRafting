using MatchThreePrototype.Controllers;
using MatchThreePrototype.PlayAreaElements;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{
    public class AmmoRemovingState : IContentState
    {

        private PlayAreaCell _cell;

        private Image _ammoImage;

        private Vector3 _flyToPosition;

        private static float STATE_DURATION = .33f;//3;//.33f;
        //private float _removalDuration = Statics.DEFAULT_REMOVE_DURATION;

        private float _secsInState;

        private Vector3 _startPosition;

        private Vector3 _startScale;

        //private static Vector3 FINISH_SCALE = new Vector3(.1f, .1f, .1f);
        private static Vector3 FINISH_SCALE = Statics.Vector3Zero();

        public void Enter()
        {
            _secsInState = 0;

            _startPosition = _ammoImage.transform.position;

            _startScale = _ammoImage.transform.localScale;
        }

        public void Update()
        {
            //_ammoImage.transform.position += Vector3.up * .5f;

            if (Time.deltaTime == 0)
            {
                return;
            }

            _secsInState += Time.deltaTime;

            if (_secsInState < STATE_DURATION)
            {
                Vector3 posLerp = Vector3.Lerp(_startPosition, _flyToPosition, _secsInState / STATE_DURATION);

                Vector3 scaleLerp = Vector3.Lerp(_startScale, FINISH_SCALE, _secsInState / STATE_DURATION);

                _ammoImage.transform.position = posLerp;
                _ammoImage.transform.localScale = scaleLerp;
            }
            else
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.CellEmpty);
            }




        }
        public void Exit()
        {
            _ammoImage.transform.position = _startPosition;
            _ammoImage.transform.localScale= _startScale;

            _cell.ItemHandler.FinishRemoval();
        }

        //internal void OnNewRemoveDuration(float duration)
        //{
        //    _removalDuration = duration;
        //}
        //internal void CleanUpOnDestroy()
        //{
        //    SettingsController.OnNewRemoveDurationDelegate -= OnNewRemoveDuration;
        //}

        public AmmoRemovingState(PlayAreaCell cell, Vector3 flyToPosition)
        {
            _cell = cell;

            _ammoImage = cell.ItemHandler.GetImage();

            _flyToPosition = flyToPosition;

            //SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;

        }

    }
}