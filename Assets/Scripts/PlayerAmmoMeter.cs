using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class PlayerAmmoMeter : MonoBehaviour
    {

        [SerializeField] private Image _fillBar;

        public Transform AmmoFlyToPosition { get => _ammoFlyToPosition; }
        [SerializeField] private Transform _ammoFlyToPosition;

        private const float CLOSE_ENOUGH = .01f;

        //private const float FILL_SPEED_UP = 1f;
        //private const float FILL_SPEED_DOWN = .25f;
        //private float _fillSpeed;

        private const float FILL_SPEED = .25f;

        private float _targetFillAmount;

        private bool _isFillInFlux;

        public void AdjustFillPercent(float percent)
        {
            _targetFillAmount = percent;

            _isFillInFlux = true;
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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
                    _fillBar.fillAmount = Mathf.MoveTowards(_fillBar.fillAmount, _targetFillAmount, FILL_SPEED * Time.deltaTime);
                }
            }
        }
    }
}
