using System.Collections.Generic;
using UnityEngine;
//using static MatchThreePrototype.EnemyPool;

namespace MatchThreePrototype
{


    public class RockPool : MonoBehaviour
    {

        [SerializeField] private int _maxPerType = 50;

        [SerializeField] private List<RockPrefabConfig> _rockPrefabConfig = new List<RockPrefabConfig>();

        private List<Rock> _availableRocks = new List<Rock>();

        public bool IsInitialized { get => _isInitialized; }
        private bool _isInitialized = false;





        internal Rock GetNextAvailable(RockTypes type)
        {
            if (_availableRocks.Count > 0)
            {
                for (int i = 0; i < _availableRocks.Count; i++)
                {
                    if (_availableRocks[i].RockType == type)
                    {
                        Rock returnRock = _availableRocks[i];
                        _availableRocks.RemoveAt(i);

                        return returnRock;
                    }
                }
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("No more " + type.ToString() + " ROCKS in pool");
            return null;
        }

        internal Rock GetNextAvailable()
        {
            if (_availableRocks.Count > 0)
            {
                Rock returnRock = _availableRocks[0];
                _availableRocks.RemoveAt(0);


                return returnRock;
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("NO more ROCKS in pool");
            return null;
        }

        public void Shuffle()
        {
            for (int i = _availableRocks.Count - 1; i > 0; i--)
            {
                int k = UnityEngine.Random.Range(0, i + 1);
                Rock itemToSwap = _availableRocks[k];
                _availableRocks[k] = _availableRocks[i];
                _availableRocks[i] = itemToSwap;
            }
        }

        private void InitializePool()
        {
            for (int i = 0; i < _rockPrefabConfig.Count; i++)
            {
                for (int j = 0; j < _maxPerType; j++)
                {

                    //PutInPool(Instantiate(_rockPrefabConfig[i].Prefab) as RockOne);

                    PutInPool(Instantiate(_rockPrefabConfig[i].Prefab) as Rock);


                    //switch (_rockPrefabConfig[i].Type)
                    //{
                    //    case RockTypes.None:
                    //        break;
                    //    case RockTypes.One:
                    //        PutInPool(Instantiate(_rockPrefabConfig[i].Prefab) as RockOne);
                    //        break;
                    //    case RockTypes.Two:
                    //        PutInPool(Instantiate(_rockPrefabConfig[i].Prefab) as RockTwo);
                    //        break;
                    //    default:
                    //        break;
                    //}

                }
            }

            Shuffle();

            _isInitialized = true;
        }

        private void PutInPool(Rock rock)
        {

            rock.gameObject.SetActive(false);

            rock.transform.SetParent(transform);
            rock.transform.localPosition = Statics.Vector3Zero();
            //rock.gameObject.SetActive(false);

            _availableRocks.Add(rock);
        }

        internal void OnPooledRockReturn(Rock rock)
        {
            PutInPool(rock);
        }

        private void OnDestroy()
        {
            Rock.OnPooledRockReturn -= OnPooledRockReturn;

        }

        private void Awake()
        {
            InitializePool();

            Rock.OnPooledRockReturn += OnPooledRockReturn;


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
        public struct RockPrefabConfig
        {
            public RockTypes Type;
            public Rock Prefab;
        }
    }

}
