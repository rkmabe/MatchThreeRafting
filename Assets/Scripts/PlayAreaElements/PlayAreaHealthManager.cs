using System;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class PlayAreaHealthManager : MonoBehaviour, IPlayAreaHealthManager
    {
        //private static float DEFAULT_HIT_POINTS = 36.00f;

        [SerializeField] private float _maxHitPoints = 36;

        public static event Action<float> OnPlayAreaHealthChanged;

        public static event Action OnPlayAreaDestroyed;

        public static event Action OnPlayAreaDamaged;

        private float _hitPoints;

        private float HitPoints
        {
            get => _hitPoints;
            set
            {
                bool notifyChange = (value != _hitPoints) ? true : false;

                _hitPoints = value;

                if (notifyChange)
                {
                    //OnPlayAreaHealthChanged(GetPercentIntact());                    
                    OnPlayAreaHealthChanged?.Invoke(GetPercentIntact());
                }
            }
        }

        private float GetPercentIntact()
        {
            //return (_hitPoints / _maxHitPoints) * 100;
            return (_hitPoints / _maxHitPoints);
        }

        public void HealDamage(float points)
        {
            HitPoints = Mathf.Clamp(_hitPoints + points, 0, _maxHitPoints);
        }

        public void TakeDamage(float points)
        {

            OnPlayAreaDamaged();

            HitPoints = Mathf.Clamp(_hitPoints - points, 0, _maxHitPoints);
            if (_hitPoints == 0)
            {

                //Debug.Log("GAME OVER MAN");
                // raise raft destroyed event
                OnPlayAreaDestroyed();

            }
        }

        public void HealCompletely()
        {
            HitPoints = _maxHitPoints;
        }





        // Start is called before the first frame update
        void Start()
        {
            HealCompletely();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}