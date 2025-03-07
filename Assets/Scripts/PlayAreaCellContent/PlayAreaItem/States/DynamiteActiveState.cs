using MatchThreePrototype.PlayAreaElements;
using MatchThreePrototype.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{

    public class DynamiteActiveState : IContentState
    {
        private PlayAreaCell _cell;

        // from Dynamite
        private AudioClip _fuseClip;
        private AudioClip _explosionClip;

        private Sprite[] _fuseBurningSprites;
        private Sprite _explosionEffectSprite;

        //private float _secsFuseBurning;

        //private static float MAX_SECS_BURNING = 1f;

        private Transform _effectImageTransform;
        private Image _effectImage;

        private Transform _itemImageTransform;
        //private Image _itemImage;

        private static Vector3 SCALE_MAX = new Vector3(2.5f, 2.5f, 2.5f);
        private static Vector3 SCALE_MIN = new Vector3(0, 0, 0);

        private bool _isFuseBurning = false;
        private bool _isExploding = false;

        private int _defaultItemSiblingIndex;

        // normal values
        private float _explosionEffectFadeStep = .5f;
        private float _explosionEffectScaleStep = 30;

        // slower values for debugging
        //private float _explosionEffectFadeStep = .25f;
        //private float _explosionEffectScaleStep = 15;


        public void Enter()
        {
            _isFuseBurning = true;

            //_secsFuseBurning = 0;
            _fuseSpriteIndex = 0;


            //_cell.AudioSource.clip = _fuseClip;
            //_cell.AudioSource.Play();

            _cell.AudioSourceDynamite.clip = _fuseClip;
            _cell.AudioSourceDynamite.Play();

            _effectImageTransform.localScale = SCALE_MIN;
            _effectImage.color = new Color(_effectImage.color.r, _effectImage.color.g, _effectImage.color.b, Statics.ALPHA_ON);

            // must be done in settle item cell, move item cell as well
            // to prevent "flashes" of dynamite sprite
            //_itemImageTransform.SetAsLastSibling();

        }

        public void Exit()
        {
            _cell.ItemHandler.FinishDynamiteActive();

            _cell.ProcessDynamiteIgnition();

            _effectImageTransform.localScale = SCALE_MIN;

            _itemImageTransform.SetSiblingIndex(_defaultItemSiblingIndex);
        }

        private int _fuseSpriteIndex = 0;
        private static float MAX_SECS_FUSE_SPRITES = .041f; //.05f; //.07f;//.041f;
        private float _secsFuseSprites = 0;

        public void Update()
        {

            //if (_cell.ItemHandler.GetIsProcessingRemoval())
            //{
            //    _cell.StateMachine.TransitionTo(_cell.StateMachine.ItemRemoving);
            //    return;
            //}

            if (Time.deltaTime == 0)
            {
                return;
            }


            if (_isFuseBurning)
            {
                _secsFuseSprites += Time.deltaTime;

                if (_fuseSpriteIndex < _fuseBurningSprites.Length - 1)
                {
                    if (_secsFuseSprites > MAX_SECS_FUSE_SPRITES)
                    {
                        _fuseSpriteIndex++;
                        _cell.ItemHandler.SetAnimationImage(_fuseBurningSprites[_fuseSpriteIndex]);

                        _secsFuseSprites = 0;
                    }
                }
                else
                {
                    //_cell.AudioSource.Stop();
                    _cell.AudioSourceDynamite.Stop();
                    _isFuseBurning = false;

                    _cell.EffectHandler.SetImageSprite(_explosionEffectSprite);

                    _cell.AudioSourceDynamite.clip = _explosionClip;
                    _cell.AudioSourceDynamite.Play();

                    _isExploding = true;
                }
            }

            else if (_isExploding)
            {
                if (_effectImageTransform.localScale.x < SCALE_MAX.x)
                {
                    //Vector3 newScale = Vector3.MoveTowards(_effectImageTransform.localScale, SCALE_MAX, .5f);
                    Vector3 newScale = Vector3.MoveTowards(_effectImageTransform.localScale, SCALE_MAX, _explosionEffectScaleStep);
                    _effectImageTransform.localScale = newScale;

                    //float newAlpha = Mathf.MoveTowards(_effectImage.color.a, Statics.ALPHA_OFF, .01f);
                    float newAlpha = Mathf.MoveTowards(_effectImage.color.a, Statics.ALPHA_OFF, _explosionEffectFadeStep);
                    _effectImage.color = new Color(_effectImage.color.r, _effectImage.color.g, _effectImage.color.b, newAlpha);
                }
                else
                {
                    _isExploding= false;

                    _effectImageTransform.localScale = SCALE_MAX;
                    _effectImage.color = new Color(_effectImage.color.r, _effectImage.color.g, _effectImage.color.b, Statics.ALPHA_OFF);

                    //_cell.ProcessDynamiteIgnition();
                }
            }
            else
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.CellEmpty);
            }
        }

        public DynamiteActiveState(PlayAreaCell cell, DynamiteResourcesSO dynamiteResources)
        {
            _cell = cell;
            _fuseClip = dynamiteResources.AudioClipFuse;
            _fuseBurningSprites = dynamiteResources.FuseBurningSprites;

            _explosionClip = dynamiteResources.AudioClipExplosion;
            _explosionEffectSprite = dynamiteResources.ExplosionEffectSprite;

            _effectImageTransform = cell.EffectHandler.GetImage().transform;
            _effectImage = cell.EffectHandler.GetImage();

            _itemImageTransform = cell.ItemHandler.GetImage().transform;
            _defaultItemSiblingIndex = _itemImageTransform.GetSiblingIndex();
            //_itemImage = cell.ItemHandler.GetImage();

            _explosionEffectScaleStep *= Time.deltaTime;
            _explosionEffectFadeStep*= Time.deltaTime;

            //_explosionEffectScaleStep = .5f;
            //_explosionEffectFadeStep = .01f;


        }
    }
}