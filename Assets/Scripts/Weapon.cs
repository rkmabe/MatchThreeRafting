using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype
{

    public class Weapon : MonoBehaviour
    {
        //private IInventoryManager _playerInventory;
        [SerializeField] private PlayerInventory _playerInventory;

        [SerializeField] private RectTransform _recoilRect;

        [SerializeField] private Transform _projectileParent;

        [SerializeField] private GameObject _rangeMarker;
        private float _rangeMarkerDistance;
        private Vector2 _range;
        private Vector2 _rangeBoxOrigin;



        private ProjectilePool _projectilePool;

        private ContactFilter2D _filter = new ContactFilter2D();

        private List<Collider2D> _results = new List<Collider2D>();


        //private IWeaponTarget _target;


        private static float AIM_DEGREES_CLOSE_ENOUGH = 5;


        private TouchToStop _touchToStop;


        private static float RECOIL_MAGNITUDE = 30;


        private bool _isRecoiling = false;


        private Vector3 _recoilReturnPosition;
        private Vector3 _recoiledPosition;       
        private Vector3 _recoilDireciton;


        [SerializeField] private GameObject _muzzleFlashImage;
        private WaitForSeconds _muzzleFlashForSeconds = new WaitForSeconds(.05f);

        [SerializeField] private AudioClip _fireClip;
        private AudioSource _audioSource;


        private bool _isRecoilingBackward = false;
        private bool _isRecoilingForward = false;

        private float _recoilBackwardStep = 500;    
        private float _recoilForwardStep = 100;

        //private bool _isDelayingAfterRecoil = false;
        //        private float MAX_SECS_AFTER_RECOIL = .5f;
                private float MAX_SECS_DELAY = .25f;
        private bool _isShotDelayed = false;
        private float _secsDelayed = 0;

        private float _rotateStep = 10; //5 //10 //15

        struct WeaponTargetResults
        {
            public WeaponTargetPriority Priority;
            public float Distance;

            public IWeaponTarget Target;
        }
        private List<WeaponTargetResults> _potentialTargets = new List<WeaponTargetResults>();

        private IWeaponTarget GetBestTarget(List<Collider2D> results)
        {
            // best target is the closest with highest priority

            IWeaponTarget bestTarget = null;

            WeaponTargetPriority highestPriority = WeaponTargetPriority.None;

            _potentialTargets.Clear();

            for (int i = 0; i < results.Count; i++)
            {

                IWeaponTarget target = results[i].GetComponent<IWeaponTarget>();

                Vector3 direction = target.GetTransformPosition() - transform.position;
                float distance = direction.sqrMagnitude;

                WeaponTargetResults result = new WeaponTargetResults();
                result.Priority = target.GetTargetPriority();
                result.Distance= distance;
                result.Target = target;

                _potentialTargets.Add(result);

                if (result.Priority > highestPriority)
                {
                    highestPriority = result.Priority;
                }
            }

            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < _potentialTargets.Count; i++)
            {
                if (_potentialTargets[i].Priority >= highestPriority)
                {
                    if (_potentialTargets[i].Distance < closestDistance)
                    {
                        closestDistance = _potentialTargets[i].Distance;
                        bestTarget = _potentialTargets[i].Target;
                    }
                }
            }

            return bestTarget;
        }

        //private IWeaponTarget GetClosestTarget(List<Collider2D> results)
        //{
        //    IWeaponTarget bestTarget = null;
        //    float closestDistanceSqr = Mathf.Infinity;

        //    for (int i = 0; i < results.Count; i++)
        //    {
        //        Vector3 directionToTarget = results[i].transform.position - transform.position;
        //        float dSqrToTarget = directionToTarget.sqrMagnitude;
        //        if (dSqrToTarget < closestDistanceSqr)
        //        {
        //            closestDistanceSqr = dSqrToTarget;
        //            bestTarget = results[i].GetComponent<IWeaponTarget>();
        //        }
        //    }
        //    return bestTarget;
        //}

        private void Aim(IWeaponTarget target, out bool isAligned, out Vector3 recoilDirection)
        {
            Vector2 direction = target.GetTransformPosition() - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotateStep);

            if (Statics.IsCloseEnough(targetRotation.eulerAngles, transform.rotation.eulerAngles, AIM_DEGREES_CLOSE_ENOUGH))
            {
                transform.rotation = targetRotation;
                isAligned = true;
                recoilDirection = direction.normalized * -1;
            }
            else
            {
                isAligned = false;
                recoilDirection = Statics.Vector2Zero();
            }
        }



        private void Fire(IWeaponTarget target)
        {
            _playerInventory.AdjustNumCannonballs(-1);

            Projectile newProjectile = _projectilePool.GetNextAvailable(ProjectileTypes.Cannonball);

            newProjectile.gameObject.SetActive(true);
            newProjectile.transform.SetParent(_projectileParent);
            //newProjectile.transform.SetParent(this.transform)
            newProjectile.transform.localScale = new Vector3(1, 1, 1);
            newProjectile.transform.localPosition = Statics.Vector3Zero();
            newProjectile.Send(target);

            StartCoroutine(MuzzleFlash());

            PlayFireSound();
        }

        private IEnumerator MuzzleFlash()
        {
            _muzzleFlashImage.SetActive(true);

            yield return _muzzleFlashForSeconds;

            _muzzleFlashImage.SetActive(false);
        }

        private void PlayFireSound()
        {
            _audioSource.clip = _fireClip;
            _audioSource.Play();

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }


        private void SetRange()
        {
            _rangeMarkerDistance = _rangeMarker.transform.position.y - transform.position.y;
            _range = new Vector2(Screen.width, _rangeMarkerDistance);
            _rangeBoxOrigin = new Vector2(transform.position.x, transform.position.y + _range.y / 2);

            //Debug.Log("RangeDistance=" + _rangeMarkerDistance);
        }
        private void SetLayerMaskFilter()
        {
            LayerMask mask = LayerMask.GetMask(Statics.LAYER_GUN_TARGET);
            _filter.useLayerMask = true;
            _filter.layerMask = mask;
        }

        private void Awake()
        {
            SetRange();
            SetLayerMaskFilter();

            _projectilePool = FindFirstObjectByType<ProjectilePool>();

            //_playerInventory = FindFirstObjectByType<PlayerInventoryOLD>();

            _touchToStop = GetComponent<TouchToStop>();

            _audioSource = GetComponent<AudioSource>();

            _recoilBackwardStep *= Time.deltaTime;
            _recoilForwardStep *= Time.deltaTime;

            _rotateStep *= Time.deltaTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (_touchToStop.IsStopped)
            {
                return;
            }

            if (Time.deltaTime==0)
            {
                return;
            }


            if (_isRecoiling)
            {
                if (_isRecoilingBackward)
                {
                    // move back, then move forward.

                    _recoilRect.position = Vector3.MoveTowards(_recoilRect.position, _recoiledPosition, _recoilBackwardStep);

                    if (Statics.IsCloseEnough(_recoilRect.position, _recoiledPosition, .01f))
                    {
                        _recoilRect.position = _recoiledPosition;

                        _isRecoilingBackward = false;
                        _isRecoilingForward = true;
                    }
                }
                else if (_isRecoilingForward)
                {
                    _recoilRect.position = Vector3.MoveTowards(_recoilRect.position, _recoilReturnPosition, _recoilForwardStep);

                    if (Statics.IsCloseEnough(_recoilRect.position, _recoilReturnPosition, .01f))
                    {
                        _recoilRect.position = _recoilReturnPosition;

                        _isRecoilingForward = false;
                        _isRecoiling = false;
                    }
                }

                return;
            }

            if (_isShotDelayed)
            {
                _secsDelayed += Time.deltaTime;
                if (_secsDelayed > MAX_SECS_DELAY)
                {
                    _isShotDelayed = false;
                }
            }

            Physics2D.OverlapBox(_rangeBoxOrigin, _range, 0, _filter, _results);
            if (_results.Count > 0)
            {
                IWeaponTarget target  = GetBestTarget(_results);

                if (target != null)
                {
                    if (target.IsActive())
                    {
                        bool isAligned;
                        Vector3 recoilDirection;
                        Aim(target, out isAligned, out recoilDirection);
                        if (isAligned)
                        {
                            if (!_isShotDelayed)
                            {
                                if (_playerInventory.NumCannonballs > 0)
                                {
                                    Fire(target);

                                    _isRecoiling = true;
                                    _isRecoilingBackward = true;

                                    _recoilDireciton = recoilDirection;

                                    _recoiledPosition = _recoilRect.transform.position + (_recoilDireciton * RECOIL_MAGNITUDE);
                                    _recoilReturnPosition = _recoilRect.transform.position;

                                    _isShotDelayed = true;
                                    _secsDelayed = 0;
                                }
                            }
                        }
                    }
                }

            }
        }


    

        //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_rangeBoxOrigin, _range);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, _rangeMarker.transform.position);
        }


    }

}

