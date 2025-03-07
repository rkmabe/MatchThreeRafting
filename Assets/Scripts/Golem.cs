using System;
using System.Collections;
using UnityEngine;

namespace MatchThreePrototype
{

    public class Golem : MonoBehaviour, IWeaponTarget
    {
        public GolemStateMachine StateMachine { get => _stateMachine; }
        private GolemStateMachine _stateMachine;

        public bool IsEntering { get => _isEntering; set => _isEntering = value; }
        private bool _isEntering = false;

        public bool IsAttacking { get => _isAttacking; }
        private bool _isAttacking = false;

        public bool IsDestroyed { get => _isDestroyed; }
        private bool _isDestroyed = false;
        public bool IsCocky { get => _isCocky; set => _isCocky = value; }
        private bool _isCocky = false;

        public RectTransform GolemEntryPosition { get => _golemEntryPosition; }
        private RectTransform _golemEntryPosition;

        public RectTransform GolemAttackPosition { get => _golemAttackPosition; }
        private RectTransform _golemAttackPosition;

        public Animator Anim { get => _anim; }


        private Animator _anim;

        [SerializeField] private RectTransform _spawnOverlapBox;
        [SerializeField] private SpawnArea _spawnArea;

        private RockPool _rockPool;
        private RockBurstPool _burstPool;




        //private const int MAX_HIT_POINTS = 20;
        //[SerializeField] private int _maxHitPoints = 24;

        private int _hitPoints = 0;

        //private const int DEFAULT_ROCKS_PER_THROW = 1;
        //[SerializeField] private int _numRocksPerThrow = DEFAULT_ROCKS_PER_THROW;

        private int _numRocksToThrow = 0;


  


        public int MaxHitPointsNEW { get => _maxHitPointsNEW; set => _maxHitPointsNEW = value; }
        private int _maxHitPointsNEW;

        public int MaxRocksToThrowNEW { get => _maxRocksToThrowNEW; set => _maxRocksToThrowNEW = value; }
        private int _maxRocksToThrowNEW;





        private const int MAX_TRIES_PER_ROCK = 10; // max num we will look for a "clear" spot to place a spawn


        public static event Action<float> OnGolemHealthChanged;

        public static event Action OnGolemDestroyed;



        [SerializeField] private GolemRockBurst _golemRockBurst;

        private AudioSource _audioSource;
        [SerializeField] private AudioClip _attackClip;

        public void TakeDamage(int damage, ContactPoint2D contactPoint)
        {
            _hitPoints = Mathf.Clamp(_hitPoints - damage, 0, _maxHitPointsNEW);

            OnGolemHealthChanged(GetPercentIntact());

            if (_hitPoints == 0) 
            {
                _isDestroyed = true;

                _golemRockBurst.transform.SetParent(transform.parent, true);
                _golemRockBurst.gameObject.SetActive(true);

                _golemRockBurst.SetReadyToExplode();

                Destroy(gameObject);

                OnGolemDestroyed();
            }

        }

        private void HealCompletely()
        {
            _hitPoints = _maxHitPointsNEW;
            OnGolemHealthChanged(GetPercentIntact());
        }

        private float GetPercentIntact()
        {
            return ((float)_hitPoints / (float)_maxHitPointsNEW);
        }

        public WeaponTargetPriority GetTargetPriority()
        {
            return WeaponTargetPriority.Golem;
        }

        public Vector3 GetTransformPosition()
        {
            return transform.position;
        }

        public bool IsActive()
        {
            return isActiveAndEnabled;
        }

        public void StartEntry(RectTransform entryPosition, RectTransform attackPosition)
        {
            HealCompletely();

            _golemEntryPosition = entryPosition;
            _golemAttackPosition = attackPosition;

            _isEntering = true;

            //ParentRockBurst();

        }



        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_spawnOverlapBox.transform.position, _spawnOverlapBox.sizeDelta);
        }

        public void AttackAnimThrow()
        {

            //_audioSource.clip = _attackClip;
            //_audioSource.Play();    

            // THROW RANDOM - 
            // throws _numRocksToThrow over N frames
            // checking the position is clear before placing a new frame.
            // It is necessary to check this one per frame becasue any previous rock must actually be placed in scene before it can be checked.

            _numRocksToThrow = _maxRocksToThrowNEW;
            StartCoroutine(ThrowOnePerFrameRandom());

        }


        private IEnumerator ThrowOnePerFrameRandom()
        {
            while (_numRocksToThrow > 0)
            {
                int tries = 0;
                bool wasRockPlaced = false;
                while (tries < MAX_TRIES_PER_ROCK && wasRockPlaced == false)
                {
                    PlaceRockRandom(out wasRockPlaced);
                    tries++;

                    //if (!wasRockPlaced)
                    //{
                    //    Debug.Log("POSITION INVALID - tries=" + tries);
                    //}

                }
                _numRocksToThrow--;

                yield return null;
            }

            //Debug.Log("done throwing rocks");
            yield break;

        }
        private void PlaceRockRandom(out bool wasSpawnPositionValid)
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
                Rock newRock = _rockPool.GetNextAvailable();
                if (newRock)
                {
                    newRock.gameObject.SetActive(true);
                    newRock.transform.SetParent(_spawnArea.transform);

                    newRock.RectTransform.anchorMin = _spawnOverlapBox.anchorMin;
                    newRock.RectTransform.anchorMax = _spawnOverlapBox.anchorMax;

                    newRock.RectTransform.anchoredPosition = Statics.Vector2Zero();
                    newRock.RectTransform.localScale = Statics.Vector3One();

                    RockBurst burst = _burstPool.GetNextAvailable();
                    if (burst)
                    {
                        burst.transform.SetParent(newRock.transform);
                        burst.transform.localPosition = Statics.Vector3Zero();

                        newRock.SetRockBurst(burst);
                    }

                }
                wasSpawnPositionValid = true;
            }
            else
            {
                wasSpawnPositionValid = false;
            }
        }

        public void StartAttack()
        {
            _isAttacking = true;
        }
        public void AttackAnimEnded()
        {
            _isAttacking = false;
        }

        public void CockyAnimStart()
        {
            _isCocky = true;
        }
        public void CockyAnimFinish()
        {
            _isCocky = false;
        }



        private void Awake()
        {
            _anim = GetComponent<Animator>();



            _rockPool = FindFirstObjectByType<RockPool>();

            _burstPool = FindFirstObjectByType<RockBurstPool>();


            _stateMachine = new GolemStateMachine(this);
            _stateMachine.Initialize(_stateMachine.GolemOffScreen);

            _audioSource = GetComponent<AudioSource>();

        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _stateMachine.Update();
        }
    }


    static class GolemAnimatorConstants
    {
        private static string IsWalking = "IsWalking";
        private static string IsIdle = "IsIdle";
        private static string IsAttacking = "IsAttacking";
        private static string IsCocky = "IsCocky";

        public static int IsWalkingID = Animator.StringToHash(IsWalking);
        public static int IsIdleID = Animator.StringToHash(IsIdle);
        public static int IsAttackingID = Animator.StringToHash(IsAttacking);
        public static int IsCockyID = Animator.StringToHash(IsCocky);

    }


}
