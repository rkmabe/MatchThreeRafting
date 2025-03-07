using MatchThreePrototype.Controllers;
using MatchThreePrototype.PlayAreaElements;
using MatchThreePrototype.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{

    public class ItemRemovingState : IContentState
    {
        private float _secsInState = 0;

        private static Vector3 SCALE_DEFAULT = new Vector3(1, 1, 1);
        private static Vector3 SCALE_REMOVED = new Vector3(0, 0, 0);

        private static Vector3 EFFECT_SCALE_MAX = new Vector3(1.3f, 1.3f, 1.3f);

        private PlayAreaCell _cell;
        private Transform _itemImageTransform;
        private Image _itemImage;


        private Transform _effectImageTransform;
        private Image _effectImage;

        private Sprite _removalEffectSprite;

        //internal static float IGNORE_SETTINGS_DURATION = .42f;//.33f;//3f;//.33f;
        //private float _removalDuration = IGNORE_SETTINGS_DURATION;

        private float _removalDuration = Statics.DEFAULT_REMOVE_DURATION;

        private float _secsInPart1;
        private float _secsInPart2;

        private float _durationPart1;
        private float _durationPart2;

        private float _secsStartPart2;

        public override string ToString()
        {
            return this.GetType().Name;
        }

        public void Enter()
        {
            //_removalDuration = IGNORE_SETTINGS_DURATION;

            _secsInState = 0;

            _secsInPart1 = 0;
            _secsInPart2 = 0;

            _durationPart1 = _removalDuration / 2; // part 1 - item shrinks to nothing
            _durationPart2 = _removalDuration / 2; // part 2 - circle grows from center

            _secsStartPart2 = _durationPart1;

            _cell.EffectHandler.SetImageSprite(_removalEffectSprite);
        }

        public void Exit()
        {
            _cell.ItemHandler.FinishRemoval();

            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_OFF);
            _itemImageTransform.localScale = SCALE_DEFAULT;

            _effectImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_OFF);
            _effectImageTransform.localScale = SCALE_REMOVED;
        }

        public void Update()
        {

            if (Time.deltaTime == 0)
            {
                return;
            }

            _secsInState += Time.deltaTime;

            if (_secsInState < _removalDuration)
            {
                if (_secsInState < _secsStartPart2)
                {
                    _secsInPart1 += Time.deltaTime;

                    _itemImageTransform.localScale = Vector3.Lerp(SCALE_DEFAULT, SCALE_REMOVED, _secsInPart1 / _durationPart1);

                }
                else if (_secsInState < _removalDuration)
                {
                    if (_secsInPart2 == 0)
                    {
                        _itemImageTransform.localScale = SCALE_REMOVED;
                    }

                    _secsInPart2 += Time.deltaTime;

                    float alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsInPart2 / _durationPart2);
                    _effectImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, alphaLerp);

                    _effectImageTransform.localScale = Vector3.Lerp(SCALE_REMOVED, EFFECT_SCALE_MAX, _secsInPart2 / _durationPart2);
                }

            }
            else
            {
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

        public ItemRemovingState(PlayAreaCell cell, ItemRemovingStateResourcesSO itemRemovingResources)
        {
            _cell = cell;
            _itemImageTransform = cell.ItemHandler.GetImage().transform;
            _itemImage = cell.ItemHandler.GetImage();

            _effectImageTransform = cell.EffectHandler.GetImage().transform;
            _effectImage = cell.EffectHandler.GetImage();

            _removalEffectSprite = itemRemovingResources.RemovalEffectSprite;

            SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;
        }


    }
}