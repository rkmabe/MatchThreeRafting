using MatchThreePrototype.PlayAreaElements;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace MatchThreePrototype
{

    public class Rock : MonoBehaviour, IWeaponTarget
    {
        public RockTypes RockType { get => _rockType; }
        [SerializeField] private RockTypes _rockType;

        //[SerializeField] private ObstacleTypes _obstacleType;

        //[SerializeField] private RockBurst _rockBurst;
        private RockBurst _rockBurst;



        public static Action<Rock> OnPooledRockReturn;

        public RectTransform RectTransform { get => _rectTransform; }
        protected RectTransform _rectTransform;

        private int _hitPoints = 1;

        private int _playAreaDamagePoints = 1;//10;

        protected static float SPEED = 100f;

        private InFluxStopper _inFluxStopper;

        public TouchToStop TouchToStop { get => _touchToStop; }
        protected TouchToStop _touchToStop;

        //ObstaclePool _obstaclePool;

        private PlayArea _playArea;

        private Image _image;

        internal void SetRockBurst(RockBurst burst)
        {
            _rockBurst = null;

            _rockBurst = burst;                      

        }

        public void TakeDamage(int damage, ContactPoint2D contactPoint)
        {
            _hitPoints = Mathf.Clamp(_hitPoints - damage, 0, _hitPoints);
            if (_hitPoints == 0)
            {
                if (_rockBurst != null)
                {
                    _rockBurst.transform.SetParent(transform.parent, true);
                    _rockBurst.gameObject.SetActive(true);

                    _rockBurst.SetupExplosion(contactPoint);

                }
                OnPooledRockReturn(this);
            }
        }

        public WeaponTargetPriority GetTargetPriority()
        {
            return WeaponTargetPriority.Rocks;
        }

        public Vector3 GetTransformPosition()
        {
            return transform.position;
        }

        public bool IsActive()
        {
            return isActiveAndEnabled;
        }


        private void CollideWithPlayArea2()
        {
            // find closest column 

            // get topmost cell, consider contents
            // if item, squash it with this rock.

            // if rock, look for the next .. until you find an item .. 
        }

        private void CollideWithPlayArea()
        {

            // legacy - repalces random item with rock
            //_playArea.PlayAreaPopulator.ReplaceRandomItemWithRock(_image.sprite);

            // working 1 - replace first cell in column with rock
            //_playArea.ReplaceRandomItemWithRock(_image.sprite,transform.position);

            // working 2 - replace first cell in column with rock phase 2
            //_playArea.QueueRiverRock(_image.sprite, transform.position);

            // working 3 - queue rock for synchronized entry .. 
            _playArea.QueueRiverObstacle(_image.sprite, transform.position);

            _playArea.HealthManager.TakeDamage(_playAreaDamagePoints);

            OnPooledRockReturn(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Statics.PLAY_AREA_COLLIDER)
            {
                CollideWithPlayArea();
            }
        }


        protected void Awake()
        {
            //_obstaclePool = FindFirstObjectByType<ObstaclePool>();
            _rectTransform = GetComponent<RectTransform>();
            _touchToStop = GetComponent<TouchToStop>();

            _inFluxStopper = GetComponent<InFluxStopper>();

            _playArea = FindFirstObjectByType<PlayArea>();

            _image = GetComponent<Image>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        protected void Update()
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
    public enum RockTypes
    {
        None = 0,
        //Rock1 = 1,
        //Rock2 = 2,

        Rock34 = 1,
        Aerial = 2,
    }

}

