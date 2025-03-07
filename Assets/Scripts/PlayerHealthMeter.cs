using MatchThreePrototype.PlayAreaElements;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class PlayerHealthMeter : MonoBehaviour
    {

        [SerializeField] private Image _fillBar;

        private float _targetFill;

        private static float MAX_TRANSITION_DURATION = .25f;

        private float _secsSinceLastUpdate = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_targetFill != _fillBar.fillAmount)
            {
                _secsSinceLastUpdate += Time.deltaTime;

                _fillBar.fillAmount = Mathf.Lerp(_fillBar.fillAmount, _targetFill, _secsSinceLastUpdate / MAX_TRANSITION_DURATION);
            }
        }

        private void OnPlayAreaHealthChanged(float percentIntact)
        {
            //_fillBar.fillAmount = percentIntact;

            _targetFill = percentIntact;

            _secsSinceLastUpdate = 0;

            //float diff = _fillBar.fillAmount - _targetFill;

            //_fillUpdateDelta = diff / MAX_TRANSITION_DURATION;

        }

        private void OnDestroy()
        {
            PlayAreaHealthManager.OnPlayAreaHealthChanged -= OnPlayAreaHealthChanged;
        }

        private void Awake()
        {
            PlayAreaHealthManager.OnPlayAreaHealthChanged += OnPlayAreaHealthChanged;
        }
    }
}