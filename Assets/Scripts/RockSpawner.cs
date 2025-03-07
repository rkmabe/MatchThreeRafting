using MatchThreePrototype.Controllers;
using UnityEngine;

namespace MatchThreePrototype
{

    public class RockSpawner : MonoBehaviour
    {
        private RockPool _rockPool;

        private RockBurstPool _burstPool;

        private float _secsBetweenSpawns;


        //[SerializeField] private float MAX_SECS_BETWEEN_SPAWNS = 5;




        // legacy for increasing rate over wave
        //[SerializeField] private float _reduceMaxSecsBy = .05f; //.10f;
        //[SerializeField] private float _currentSecsBetweenSpawns = _maxSecsBetweenSpawns;
        //private static float _minSecsBetweenSpawns = .5f;
        //private static float _maxSecsBetweenSpawns = 10.5f;

        private TouchToStop _touchToStop;

        private InFluxStopper _inFluxStopper;

        [SerializeField] private SpawnArea _spawnArea;
        [SerializeField] private RectTransform _spawnOverlapBox;

        private bool _isPausedForWaypoint = false;


        private SettingsController _settingsController;


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_spawnOverlapBox.transform.position, _spawnOverlapBox.sizeDelta);
        }

        private void PlaceRockRandom()
        {
            float randomX = UnityEngine.Random.Range(0.00f, 1.00f);
            float randomY = UnityEngine.Random.Range(0.00f, 1.00f);

            Vector2 anchorMin = new Vector2(randomX, randomY);
            Vector2 anchorMax = new Vector2(randomX, randomY);

            _spawnOverlapBox.anchorMin = anchorMin;
            _spawnOverlapBox.anchorMax = anchorMax;

            _spawnOverlapBox.anchoredPosition = Statics.Vector2Zero();
            _spawnOverlapBox.localScale = Statics.Vector3One();

            if (_spawnArea.IsSpawnPositionValid(_spawnOverlapBox))
            {
                //Debug.Log("Spawn Position Valid");
                Rock newRock = _rockPool.GetNextAvailable();
                if (newRock)
                {
                    newRock.gameObject.SetActive(true);
                    //newRock.transform.SetParent(this.transform);
                    newRock.transform.SetParent(_spawnArea.transform);

                    newRock.RectTransform.anchorMin = _spawnOverlapBox.anchorMin;
                    newRock.RectTransform.anchorMax = _spawnOverlapBox.anchorMax;

                    newRock.RectTransform.anchoredPosition = Statics.Vector2Zero();
                    newRock.RectTransform.localScale = Statics.Vector3One();

                    newRock.TouchToStop.EnableFeature(_touchToStop.IsFeatureEnabled);

                    RockBurst burst = _burstPool.GetNextAvailable();
                    if (burst)
                    {
                        burst.transform.SetParent(newRock.transform);
                        burst.transform.localPosition = Statics.Vector3Zero();

                        newRock.SetRockBurst(burst);
                    }
                }
            }
            else
            {
                //Debug.Log("Spawn Position INVALID");
            }
        }

        private void OnWaveEndScreenClose()
        {
            _isPausedForWaypoint = false;
        }

        private void OnWayointNear()
        {
            _isPausedForWaypoint = true;
        }


        private void OnDestroy()
        {
            DistanceTracker.OnWayointNear -= OnWayointNear;

            WaveEndScreen.OnWaveEndScreenClose -= OnWaveEndScreenClose;
        }

        private void Awake()
        {
            _rockPool = FindFirstObjectByType<RockPool>();

            _touchToStop = GetComponent<TouchToStop>();

            _burstPool = FindFirstObjectByType<RockBurstPool>();

            _inFluxStopper = GetComponent<InFluxStopper>();

            _settingsController = FindFirstObjectByType<SettingsController>();

            DistanceTracker.OnWayointNear += OnWayointNear;

            WaveEndScreen.OnWaveEndScreenClose += OnWaveEndScreenClose;
        }

        // Start is called before the first frame update
        void Start()
        {


        }

        //bool ROCK_PLACED = false;

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

            if (_rockPool.IsInitialized == false)
            {
                return;
            }

            if (_isPausedForWaypoint)
            {
                return;
            }

            _secsBetweenSpawns += Time.deltaTime;

            //if (_secsBetweenSpawns > _currentSecsBetweenSpawns)
            //if (_secsBetweenSpawns > MAX_SECS_BETWEEN_SPAWNS)
            if (_secsBetweenSpawns > _settingsController.GetFreqObstacle())
            
            {
                PlaceRockRandom();

                _secsBetweenSpawns = 0;

                //_maxSecsBetweenSpawns -= _reduceMaxSecsBy;
                //_currentSecsBetweenSpawns = Mathf.Clamp(_currentSecsBetweenSpawns - _reduceMaxSecsBy, _minSecsBetweenSpawns, _maxSecsBetweenSpawns);

            }
        }
    }
}