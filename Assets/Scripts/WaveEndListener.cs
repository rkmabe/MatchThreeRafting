using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype
{
    public class WaveEndListener : MonoBehaviour
    {

        [SerializeField] WaveEndScreen _waveEndScreen;

        private PlayArea _playArea;

        private float MAX_SECS_DELAY = 1;//.01f;//1;
        private bool _isWaitingToDisplayWaveEnd;
        private float _secsDelayed = 0;

        private void OnWayointReached()
        {
            _waveEndScreen.gameObject.SetActive(true);
            _waveEndScreen.Open();
        }

        private void OnGolemDestroyed()
        {
            _isWaitingToDisplayWaveEnd = true;
            _secsDelayed = 0;
        }

        private void OpenWaveEndScreen()
        {
            _waveEndScreen.gameObject.SetActive(true);
            _waveEndScreen.Open();
        }

        private void OnDestroy()
        {
            //DistanceTracker.OnWayointReached -= OnWayointReached;
            Golem.OnGolemDestroyed -= OnGolemDestroyed;
        }
        private void Awake()
        {
            //DistanceTracker.OnWayointReached += OnWayointReached;
            Golem.OnGolemDestroyed += OnGolemDestroyed;


            _playArea = FindFirstObjectByType<PlayArea>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_isWaitingToDisplayWaveEnd)
            {
                _secsDelayed += Time.deltaTime;

                if (_secsDelayed >= MAX_SECS_DELAY)
                {
                    // not until play area is out of flux
                    if (!_playArea.IsInFlux)
                    {
                        _isWaitingToDisplayWaveEnd = false;
                        OpenWaveEndScreen();
                    }
                    else
                    {
                        //Debug.Log("we observed this working");
                    }
                }
            }
        }
    }
}
