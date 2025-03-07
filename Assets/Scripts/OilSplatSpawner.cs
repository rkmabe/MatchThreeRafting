using MatchThreePrototype.Controllers;
using UnityEngine;

namespace MatchThreePrototype
{

    public class OilSplatSpawner : MonoBehaviour
    {

        private OilSplatPool _oilSplatPool;

         private float _secsBetweenSpawns;

        //[SerializeField] private float MAX_SECS_BETWEEN_SPAWNS = 5;

        //private static float MAX_SECS_BETWEEN_SPAWNS = 5;//5.5f;
        //private float _secsBetweenSpawns = 1.5f;       //1.5f;//5.5f;

        private TouchToStop _touchToStop;

        private InFluxStopper _inFluxStopper;


        [SerializeField] private SpawnArea _spawnArea;
        [SerializeField] private RectTransform _spawnOverlapBox;


        private bool _isPausedForWaypoint = false;

        private SettingsController _settingsController;

        private void PlaceOilSplatRandom()
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

                OilSplat newOilSplat = _oilSplatPool.GetNextAvailable();

                if (newOilSplat)
                {
                    newOilSplat.gameObject.SetActive(true);
                    newOilSplat.transform.SetParent(_spawnArea.transform);

                    newOilSplat.RectTransform.anchorMin = _spawnOverlapBox.anchorMin;
                    newOilSplat.RectTransform.anchorMax = _spawnOverlapBox.anchorMax;

                    newOilSplat.RectTransform.anchoredPosition = Statics.Vector2Zero();
                    newOilSplat.RectTransform.localScale = Statics.Vector3One();

                    newOilSplat.TouchToStop.EnableFeature(_touchToStop.IsFeatureEnabled);
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
            _oilSplatPool = FindFirstObjectByType<OilSplatPool>();

            _touchToStop = GetComponent<TouchToStop>();

            _inFluxStopper = GetComponent<InFluxStopper>();

            _settingsController = FindFirstObjectByType<SettingsController>();

            DistanceTracker.OnWayointNear += OnWayointNear;

            WaveEndScreen.OnWaveEndScreenClose += OnWaveEndScreenClose;
        }


        // Start is called before the first frame update
        void Start()
        {

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

            if (_oilSplatPool.IsInitialized == false)
            {
                return;
            }

            if (_isPausedForWaypoint)
            {
                return;
            }

            _secsBetweenSpawns += Time.deltaTime;

            //if (_secsBetweenSpawns > MAX_SECS_BETWEEN_SPAWNS)
            if (_secsBetweenSpawns > _settingsController.GetFreqBlock())
            {
                PlaceOilSplatRandom();

                _secsBetweenSpawns = 0;
            }
        }
    }
}
