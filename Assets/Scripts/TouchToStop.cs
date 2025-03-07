using MatchThreePrototype.Controllers;
using MatchThreePrototype.PlayerTouchInput;
using UnityEngine;


namespace MatchThreePrototype
{

    public class TouchToStop : MonoBehaviour
    {
        public bool IsFeatureEnabled { get => _isFeatureEnabled; }
        private bool _isFeatureEnabled  = false;

        public bool IsStopped { get => _isStopped; }
        private bool _isStopped = false;        

        internal void EnableFeature(bool isEnabled)
        {
            _isFeatureEnabled = isEnabled;
        }

        public void OnTouchInputDown(Vector3 position)
        {
            if (_isFeatureEnabled)
            {
                _isStopped = true;
            }
        }

        public void OnTouchInputUp(Vector3 position)
        {
            _isStopped = false;
        }

        public void OnChangeTouchToSwap(bool isEnabled)
        {
            //_isFeatureEnabled  = isEnabled;
            EnableFeature(isEnabled);
        }

        private void OnDestroy()
        {
            TouchDetector.OnTouchInputDownDelegate -= OnTouchInputDown;
            TouchDetector.OnTouchInputUpDelegate -= OnTouchInputUp;

            SettingsController.OnChangeTouchToStopDelegate -= OnChangeTouchToSwap;
        }

        private void Awake()
        {
            TouchDetector.OnTouchInputDownDelegate += OnTouchInputDown;
            TouchDetector.OnTouchInputUpDelegate += OnTouchInputUp;

            SettingsController.OnChangeTouchToStopDelegate += OnChangeTouchToSwap;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}