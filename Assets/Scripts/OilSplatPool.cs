using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype
{

    public class OilSplatPool : MonoBehaviour
    {

        [SerializeField] private int _maxPerType = 50;

        [SerializeField] private List<OilSplatPrefabConfig> _prefabConfig = new List<OilSplatPrefabConfig>();

        private List<OilSplat> _availableOilSplats = new List<OilSplat>();

        public bool IsInitialized { get => _isInitialized; }
        private bool _isInitialized = false;



        internal OilSplat GetNextAvailable(OilSplatTypes type)
        {
            if (_availableOilSplats.Count > 0)
            {
                for (int i = 0; i < _availableOilSplats.Count; i++)
                {
                    if (_availableOilSplats[i].OilSplattype == type)
                    {
                        OilSplat returnOilSplat = _availableOilSplats[i];
                        _availableOilSplats.RemoveAt(i);

                        return returnOilSplat;
                    }
                }
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("No more " + type.ToString() + " OILSPLATS in pool");
            return null;
        }

        internal OilSplat GetNextAvailable()
        {
            if (_availableOilSplats.Count > 0)
            {
                OilSplat returnOilSplat = _availableOilSplats[0];
                _availableOilSplats.RemoveAt(0);


                return returnOilSplat;
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("NO more OILSPLATS in pool");
            return null;
        }

        public void Shuffle()
        {
            for (int i = _availableOilSplats.Count - 1; i > 0; i--)
            {
                int k = UnityEngine.Random.Range(0, i + 1);
                OilSplat itemToSwap = _availableOilSplats[k];
                _availableOilSplats[k] = _availableOilSplats[i];
                _availableOilSplats[i] = itemToSwap;
            }
        }

        private void InitializePool()
        {
            for (int i = 0; i < _prefabConfig.Count; i++)
            {
                for (int j = 0; j < _maxPerType; j++)
                {
                    PutInPool(Instantiate(_prefabConfig[i].Prefab) as OilSplat);

                }
            }

            Shuffle();

            _isInitialized = true;
        }

        private void PutInPool(OilSplat oilSplat)
        {

            oilSplat.gameObject.SetActive(false);

            oilSplat.transform.SetParent(transform);
            oilSplat.transform.localPosition = Statics.Vector3Zero();
            //oilSplat.gameObject.SetActive(false);

            _availableOilSplats.Add(oilSplat);
        }

        internal void OnPooledOilSplatReturn(OilSplat oilSplat)
        {
            PutInPool(oilSplat);
        }

        private void OnDestroy()
        {
            OilSplat.OnPooledOilSplatReturn -= OnPooledOilSplatReturn;


        }

        private void Awake()
        {
            InitializePool();

            OilSplat.OnPooledOilSplatReturn += OnPooledOilSplatReturn;


        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        [System.Serializable]
        public struct OilSplatPrefabConfig
        {
            public OilSplatTypes Type;
            public OilSplat Prefab;
        }
    }

}