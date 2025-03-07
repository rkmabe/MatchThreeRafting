using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaElements;
using System;
using UnityEngine;

namespace MatchThreePrototype
{

    public class OilSplat : MonoBehaviour
    {
        public OilSplatTypes OilSplattype { get => _oilSplatType; }
        [SerializeField] private OilSplatTypes _oilSplatType;
                                
        [SerializeField] private BlockTypes _blockType;

        public static Action<OilSplat> OnPooledOilSplatReturn;

        public RectTransform RectTransform { get => _rectTransform; }
        protected RectTransform _rectTransform;

        protected static float SPEED = 100f;

        public TouchToStop TouchToStop { get => _touchToStop; }
        protected TouchToStop _touchToStop;

        private InFluxStopper _inFluxStopper;

        //BlockPool _blockPool;

        private PlayArea _playArea;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Statics.PLAY_AREA_COLLIDER)
            {
                CollideWithPlayArea();
            }
        }
        private void CollideWithPlayArea()
        {
            // legacy - find random free item and block it immediately
            //Block block = _blockPool.GetNextAvailable(_blockType);
            //_playArea.PlayAreaPopulator.BlockRandomItem(block);

            // working 1 - queue block type for play area
            _playArea.QueueRiverBlock(_blockType,transform.position);

            OnPooledOilSplatReturn(this);
        }

        protected void Awake()
        {
            //_blockPool = FindFirstObjectByType<BlockPool>();
            _rectTransform = GetComponent<RectTransform>();
            _touchToStop = GetComponent<TouchToStop>();

            _inFluxStopper = GetComponent<InFluxStopper>();

            _playArea = FindFirstObjectByType<PlayArea>();
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

            _rectTransform.position += Vector3.down * SPEED * Time.deltaTime;
        }
    
    }

    public enum OilSplatTypes
    {
        None = 0,
        OilSplat1 = 1,
        OilSplat2 = 2,
    }
}