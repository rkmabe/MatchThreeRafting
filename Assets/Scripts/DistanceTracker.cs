using MatchThreePrototype.Controllers;
using MatchThreePrototype.UI;
using System;
using System.Text;
using UnityEngine;

namespace MatchThreePrototype
{
    public class DistanceTracker : MonoBehaviour
    {

        //[SerializeField] private TMPro.TextMeshProUGUI _distanceLabel;
        [SerializeField] private TMPro.TextMeshProUGUI _distanceValue;

        private readonly float PERCENT_WAYPONIT_NEAR = .85f;

        //private const float DEFAULT_METERS_TO_WAYPOINT = 60;
        //[SerializeField] private float _metersToWaypoint = 60;

        private float _metersToWaypointNear;

        private float _metersPerSecond = 1.3888333f;                       //1.38888333f;   // 5 km per hour 

        private float _timeElapsed = 0;

        StringBuilder _distanceValueSB = new StringBuilder();

        public static event Action OnWayointReached;

        public static event Action OnWayointNear;

        //private PlayArea _playArea;

        private bool _isTracking;

        private TouchToStop _touchToStop;

        private InFluxStopper _inFluxStopper;

        private bool _wayPointNearAlerted = false;

        private SettingsController _settingsController;

        private float _metersToWaypoint;

        private void StartTracking()
        {
            _isTracking = true;
        }
        private void StopTracking()
        {
            _isTracking = false;
        }


        internal void SetDistanceToWaypoint(float distance)
        {
            _metersToWaypoint = distance;

            _metersToWaypointNear = distance - (distance * PERCENT_WAYPONIT_NEAR);

            //_metersToWaypoint = distance;
            //_metersToWaypointNear = distance - (distance * .10f);

        }

        //internal void ResetTimeElapsed()
        //{
        //    _timeElapsed = 0;
        //}

        internal void ResetTracker()
        {
            _timeElapsed = 0;
            _wayPointNearAlerted = false;
        }


        internal void UpdateDistanceUI(float distance)
        {
            _distanceValueSB.Clear();
            _distanceValueSB.Append(distance.ToString("0.00"));
            _distanceValueSB.Append(Statics.METERS);
            _distanceValue.text = _distanceValueSB.ToString();
        }

        private void OnPlay()
        {
            StartTracking();
        }
        private void OnPause()
        {
            StopTracking();
        }

        private void OnDestroy()
        {
            PausePlayButton.OnPause -= OnPause;
            PausePlayButton.OnPlay -= OnPlay;
        }

        private void Awake()
        {
            //SetDistanceToWaypoint(DEFAULT_METERS_TO_WAYPOINT);      //DEFAULT_METERS_TO_WAYPOINT

            //SetDistanceToWaypoint(_metersToWaypoint);      //DEFAULT_METERS_TO_WAYPOINT

            //_playArea = FindFirstObjectByType<PlayArea>();

            _settingsController = FindFirstObjectByType<SettingsController>();

            PausePlayButton.OnPause += OnPause;
            PausePlayButton.OnPlay += OnPlay;

            _touchToStop = GetComponent<TouchToStop>();

            _inFluxStopper = GetComponent<InFluxStopper>();

        }


        // Start is called before the first frame update
        void Start()
        {
            SetDistanceToWaypoint(_settingsController.GetWaypointDistance());
            StartTracking();
        }

        // Update is called once per frame
        void Update()
        {

            if (_touchToStop.IsStopped)
            {
                return;
            }

            if (_inFluxStopper.IsStopped)
            {
                return;
            }
            


            if (_isTracking)
            {

                _timeElapsed += Time.deltaTime;

                float metersTracked = _timeElapsed * _metersPerSecond;

                float remainingDistance = Mathf.Clamp(_metersToWaypoint - metersTracked, 0, _metersToWaypoint);

                UpdateDistanceUI(remainingDistance);

                if (remainingDistance < _metersToWaypointNear && !_wayPointNearAlerted)
                {

                    _wayPointNearAlerted = true;
                    OnWayointNear();
                }

                if (remainingDistance == 0)
                {
                    //ResetTimeElapsed();
                    ResetTracker();

                    _isTracking = false;

                    OnWayointReached();
                }

            }
        }
    }
}