using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype
{
    // this Object Pool is different than the otehrs in this project
    // it must have a properly sized canvas as the parent and be placed in the UI hierarchy

    public class RockBurstPool : MonoBehaviour
    {

        [SerializeField] private int _maxInstances = 20;

        [SerializeField] private GameObject _parentedRockBurstPrefab;

        private List<GameObject> _availableParentedBursts = new List<GameObject>();

        public bool IsInitialized { get => _isInitialized; }
        private bool _isInitialized = false;

        internal void OnPooledRockBurstReturn(RockBurst burst)
        {
            burst.ResetToDefault();

            burst.gameObject.SetActive(false);

            for (int i = 0; i < _availableParentedBursts.Count; i++)
            {
                if (_availableParentedBursts[i].transform.childCount == 0)
                {
                    burst.transform.SetParent(_availableParentedBursts[i].transform);
                    burst.transform.localPosition = Statics.Vector3Zero();

                    break;
                }
            }
        }

        internal RockBurst GetNextAvailable()
        {
            RockBurst returnBurst = null;

            for (int i = 0; i < _availableParentedBursts.Count; i++)
            {
                RockBurst rb = _availableParentedBursts[i].GetComponentInChildren<RockBurst>(true);
                if (rb)
                {
                    returnBurst = rb;
                    break;
                }
            }

            if (returnBurst == null)
            {
                Debug.LogWarning("No More BURSTS in Pool!");
                _availableParentedBursts.Add(GetNewInstance());
                return GetNextAvailable();
            }

            return returnBurst;
        }

        public void Shuffle()
        {
            for (int i = _availableParentedBursts.Count - 1; i > 0; i--)
            {
                int k = UnityEngine.Random.Range(0, i + 1);
                GameObject itemToSwap = _availableParentedBursts[k];
                _availableParentedBursts[k] = _availableParentedBursts[i];
                _availableParentedBursts[i] = itemToSwap;
            }
        }

        private GameObject GetNewInstance()
        {
            GameObject rockBurstBundle = Instantiate(_parentedRockBurstPrefab) as GameObject;

            rockBurstBundle.gameObject.SetActive(false);

            rockBurstBundle.transform.SetParent(transform);
            rockBurstBundle.transform.localPosition = Statics.Vector3Zero();

            rockBurstBundle.transform.localScale = Statics.Vector3One();

            return rockBurstBundle;
        }

        private void InitializePool()
        {
            for (int i = 0; i < _maxInstances; i++)
            {
                _availableParentedBursts.Add(GetNewInstance());
            }

            Shuffle();

            _isInitialized = true;
        }

        private void OnDestroy()
        {
            RockBurst.OnPooledRockBurstReturn -= OnPooledRockBurstReturn;
        }

        private void Awake()
        {
            InitializePool();

            RockBurst.OnPooledRockBurstReturn += OnPooledRockBurstReturn;
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