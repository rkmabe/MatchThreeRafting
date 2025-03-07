using MatchThreePrototype.MatchReaction;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaElements
{
    public class GolemHealthMeter : MonoBehaviour
    {

        [SerializeField] private TMPro.TextMeshProUGUI _textLabel;

        [SerializeField] private Image _backGround;

        [SerializeField] private Image _fillBar;

        [SerializeField] private float _imageAlphaOn = 0.6509804f;

        [SerializeField] private PlayerInventoryDisplay _inventoryDisplay;

        private RectTransform _rectTransform;

        private const float CLOSE_ENOUGH = .01f;

        private const float FILL_SPEED_UP = 1f;
        private const float FILL_SPEED_DOWN = 1f;

        private float _fillSpeed;

        private float _targetFillAmount;

        private bool _isFillInFlux;

        private const float ANIMATE_INOUT_SPEED = 5f;

        private bool _isAnimatingIn = false;
        private bool _isAnimatingOut = false;

        private float _imageAlphaInFlux = 0;
        private float _textAlphaInFlux = 0;

        private const float MAX_SECS_ANIMATE_OUT_DELAY = .5f;
        private float _secsAnimateOutDelay = 0;

        private void OnGolemHealthChanged(float percentIntact)
        {
            _targetFillAmount = percentIntact;

            _isFillInFlux = true;
        }

        private void OnGolemDestroyed()
        {
            //_isAnimatingOut = true;
            AnimateOut();
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnEnable()
        {

        }

        public void AnimateIn()
        {
            _isAnimatingIn = true;
        }

        private void AnimateOut()
        {
            _secsAnimateOutDelay = 0;
            _isAnimatingOut = true;
        }



        // Update is called once per frame
        void Update()
        {
            
            if (_isAnimatingIn)
            {
                if (_inventoryDisplay.gameObject.activeSelf)
                {
                    _inventoryDisplay.gameObject.SetActive(false);
                }

                if (Mathf.Abs(_imageAlphaOn - _imageAlphaInFlux) <= CLOSE_ENOUGH)
                {
                    _imageAlphaInFlux = _imageAlphaOn;
                    _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, _imageAlphaOn);
                    _fillBar.color = new Color(_fillBar.color.r, _fillBar.color.g, _fillBar.color.b, _imageAlphaOn);

                    _textAlphaInFlux = Statics.ALPHA_ON;
                    _textLabel.alpha = Statics.ALPHA_ON;

                    _isAnimatingIn = false;
                }
                else
                {
                    _imageAlphaInFlux = Mathf.MoveTowards(_imageAlphaInFlux, _imageAlphaOn, ANIMATE_INOUT_SPEED * Time.deltaTime);
                    _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, _imageAlphaInFlux);
                    _fillBar.color = new Color(_fillBar.color.r, _fillBar.color.g, _fillBar.color.b, _imageAlphaInFlux);

                    _textAlphaInFlux = Mathf.MoveTowards(_textAlphaInFlux, Statics.ALPHA_ON, ANIMATE_INOUT_SPEED * Time.deltaTime);
                    _textLabel.alpha = _textAlphaInFlux;
                }

                return;
            }

            if (_isFillInFlux)
            {
                float diff = (_targetFillAmount - _fillBar.fillAmount);

                if (Math.Abs(diff) <= CLOSE_ENOUGH)
                {
                    _fillBar.fillAmount = _targetFillAmount;
                    _isFillInFlux = false;
                }
                else
                {
                    _fillSpeed = diff > 0 ? FILL_SPEED_UP : FILL_SPEED_DOWN;
                    _fillBar.fillAmount = Mathf.MoveTowards(_fillBar.fillAmount, _targetFillAmount, _fillSpeed * Time.deltaTime);

                }
            }

            if (_isAnimatingOut)
            {
                if (_secsAnimateOutDelay < MAX_SECS_ANIMATE_OUT_DELAY)
                {
                    _secsAnimateOutDelay += Time.deltaTime;
                }
                else
                {

                    //if (Mathf.Abs(_imageAlphaOn - _imageAlphaInFlux) <= CLOSE_ENOUGH)
                    if (Mathf.Abs(Statics.ALPHA_OFF - _backGround.color.a) <= CLOSE_ENOUGH)
                    {
                        _imageAlphaInFlux = Statics.ALPHA_OFF;
                        _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, Statics.ALPHA_OFF);
                        _fillBar.color = new Color(_fillBar.color.r, _fillBar.color.g, _fillBar.color.b, Statics.ALPHA_OFF);

                        _textAlphaInFlux = Statics.ALPHA_OFF;
                        _textLabel.alpha = Statics.ALPHA_OFF;

                        _isAnimatingOut = false;
                    }
                    else
                    {
                        _imageAlphaInFlux = Mathf.MoveTowards(_imageAlphaInFlux, Statics.ALPHA_OFF, ANIMATE_INOUT_SPEED * Time.deltaTime);
                        _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, _imageAlphaInFlux);
                        _fillBar.color = new Color(_fillBar.color.r, _fillBar.color.g, _fillBar.color.b, _imageAlphaInFlux);

                        _textAlphaInFlux = Mathf.MoveTowards(_textAlphaInFlux, Statics.ALPHA_OFF, ANIMATE_INOUT_SPEED * Time.deltaTime);
                        _textLabel.alpha = _textAlphaInFlux;

                        _inventoryDisplay.gameObject.SetActive(true);
                    }
                }

                //return;
            }



        }

        private void OnDestroy()
        {
            Golem.OnGolemHealthChanged -= OnGolemHealthChanged;
            Golem.OnGolemDestroyed -= OnGolemDestroyed;
        }

        private void Awake()
        {
            Golem.OnGolemHealthChanged += OnGolemHealthChanged;
            Golem.OnGolemDestroyed += OnGolemDestroyed;

            _rectTransform = GetComponent<RectTransform>();
        }
    }
}
