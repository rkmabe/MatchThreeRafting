using MatchThreePrototype.Controllers;
using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype
{
    public class GolemActivator : MonoBehaviour
    {

        [SerializeField] private Golem _golem;
        [SerializeField] private GolemHealthMeter _healthMeter;

        [SerializeField] private Golem _golemPrefab;

        [SerializeField] private RectTransform _golemEntryPosition;
        [SerializeField] private RectTransform _golemAttackPosition;

        private SettingsController _settingsController;

        private void OnWayointReached()
        {
            Golem newGolem = Instantiate(_golemPrefab) as Golem;
            newGolem.transform.SetParent(this.transform, false);
            newGolem.transform.position = _golemEntryPosition.position;


            newGolem.MaxHitPointsNEW = _settingsController.GetGolemHealth();
            newGolem.MaxRocksToThrowNEW = _settingsController.GetGolemRocks();

            newGolem.StartEntry(_golemEntryPosition, _golemAttackPosition);

            _healthMeter.AnimateIn();

        }

        private void OnDestroy()
        {
            DistanceTracker.OnWayointReached -= OnWayointReached;
        }
        private void Awake()
        {
            DistanceTracker.OnWayointReached += OnWayointReached;

            _settingsController = FindFirstObjectByType<SettingsController>();  

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
