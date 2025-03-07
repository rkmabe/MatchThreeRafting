using UnityEngine;
using MatchThreePrototype.PlayAreaElements;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{

    public class ItemLandingState : IContentState
    {

        private PlayAreaCell _cell;
        private RectTransform _itemRectTransform;

        private static float SCALE_SQUASHED_X = 1.3f;
        private static float SCALE_SQUASHED_Y = .7f;

        private static float SCALE_STRETCHED_X = .85f;
        private static float SCALE_STRETCHED_Y = 1.15f;

        private static Vector3 SCALE_DEFAULT = new Vector3(1,1,1);
        private static Vector3 SCALE_SQUASHED = new Vector3(SCALE_SQUASHED_X, SCALE_SQUASHED_Y, 1);

        private static Vector3 SCALE_STRETCHED = new Vector3(SCALE_STRETCHED_X, SCALE_STRETCHED_Y);

        private static Vector2 PIVOT_DEFAULT = new Vector2(.5f, .5f);
        private static Vector2 PIVOT_LANDING = new Vector2(.5f, 0);

        private static float MAX_STATE_DURATION = .42f;//.33f;//5;//.33f; 

        private float _durationPart1;
        private float _durationPart2;
        private float _durationPart3;
        private float _durationPart4;

        private float _secsInState;

        private float _secsInPart1;
        private float _secsInPart2;
        private float _secsInPart3;
        private float _secsInPart4;

        private float _secsStartPart2;
        private float _secsStartPart3;

        private float _secsStartPart4;

        public override string ToString()
        {
            return this.GetType().Name;
        }

        public void Enter()
        {

            _secsInState = 0;

            _secsInPart1 = 0;
            _secsInPart2 = 0;
            _secsInPart3 = 0;
            _secsInPart4 = 0;

            _itemRectTransform.pivot = PIVOT_LANDING;
        }

        public void Exit()
        {
            _cell.ItemHandler.FinishLanding();

            _itemRectTransform.pivot = PIVOT_DEFAULT;
            _itemRectTransform.localScale = SCALE_DEFAULT;

        }

        public void Update()
        {

            if (_cell.ItemHandler.GetIsProcessingRemoval())
            {
                //Debug.LogWarning("THIS SHOULD NOT BE POSSIBLE");
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemRemoving);
                return;
            }

            if (Time.deltaTime == 0)
            {
                return;
            }

            _secsInState += Time.deltaTime;

            if (_secsInState < MAX_STATE_DURATION)
            {

                if (_secsInState < _secsStartPart2)
                {
                    _secsInPart1 += Time.deltaTime;

                    _itemRectTransform.localScale = Vector3.Lerp(SCALE_DEFAULT, SCALE_SQUASHED, _secsInPart1 / _durationPart1);
                }
                else if (_secsInState < _secsStartPart3)
                {
                    _secsInPart2 += Time.deltaTime;

                    _itemRectTransform.localScale = Vector3.Lerp(SCALE_SQUASHED, SCALE_STRETCHED, _secsInPart2 / _durationPart2);
                }
                else if (_secsInState < _secsStartPart4)
                {
                    _secsInPart3 += Time.deltaTime;

                    _itemRectTransform.localScale = Vector3.Lerp(SCALE_STRETCHED, SCALE_DEFAULT, _secsInPart3 / _durationPart3);
                }
                else if (_secsInState < MAX_STATE_DURATION)
                {
                    _secsInPart4 += Time.deltaTime;

                    _itemRectTransform.pivot = Vector2.Lerp(PIVOT_LANDING, PIVOT_DEFAULT, _secsInPart4 / _durationPart4);
                }
            }
            else
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemIdle);
            }

        }


        public ItemLandingState(PlayAreaCell cell )
        {
            _cell = cell;

            _itemRectTransform = _cell.ItemHandler.GetImage().GetComponent<RectTransform>();

            _durationPart1 = MAX_STATE_DURATION * .35f;   // part 1 -  scale from Default  to SQUASHED
            _durationPart2 = MAX_STATE_DURATION * .25f;   // part 2 -  scale from SQUASHED to STRETCHED
            _durationPart3 = MAX_STATE_DURATION * .20f;   // part 3 - scale from STRETCHED to Default
            _durationPart4 = MAX_STATE_DURATION * .20f;   // part 4 - pivot from 0 to .5

            _secsStartPart2 = _durationPart1;
            _secsStartPart3 = _secsStartPart2 + _durationPart2;
            _secsStartPart4 = _secsStartPart3 + _durationPart3;
        }


    }



}
