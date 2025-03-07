using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype
{

    public class ProjectilePool : MonoBehaviour
    {

        [SerializeField] private int _maxPerType = 30;
        public bool IsInitialized { get => _isInitialized; }
        private bool _isInitialized = false;

        private List<Projectile> _availableProjectiles = new List<Projectile>();

        [SerializeField] private List<ProjectileTypeConfig> _projectileTypeConfig = new List<ProjectileTypeConfig>();


        private void InitializePool()
        {
            for (int i = 0; i < _projectileTypeConfig.Count; i++)
            {
                for (int j = 0; j < _maxPerType; j++)
                {
                    PutInPool(Instantiate(_projectileTypeConfig[i].Prefab) as Projectile);
                }
            }

            _isInitialized = true;
        }

        private void PutInPool(Projectile projectile)
        {
            projectile.transform.SetParent(transform);
            projectile.transform.localPosition = Statics.Vector3Zero();
            projectile.gameObject.SetActive(false);

            _availableProjectiles.Add(projectile);
        }

        internal Projectile GetNextAvailable(ProjectileTypes type)
        {
            if (_availableProjectiles.Count > 0)
            {
                for (int i = 0; i < _availableProjectiles.Count; i++)
                {
                    if (_availableProjectiles[i].ProjectileType == type)
                    {
                        Projectile returnProjectile = _availableProjectiles[i];
                        _availableProjectiles.RemoveAt(i);
                        return returnProjectile;
                    }
                }
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("No more " + type.ToString() + " PROJECTILES in pool");
            return null;

        }

        internal Projectile GetNextAvailable()
        {
            if (_availableProjectiles.Count > 0)
            {
                Projectile returnProjectile = _availableProjectiles[0];
                _availableProjectiles.RemoveAt(0);
                return returnProjectile;
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("NO more PROJECTILES in pool");
            return null;
        }

        internal void Return(Projectile projectile)
        {
            PutInPool(projectile);
        }

        private void OnDestroy()
        {
            Cannonball.OnProjectileReturn -= Return;
        }

        private void Awake()
        {
            Cannonball.OnProjectileReturn += Return;

            InitializePool();
        }

        // Start is called before the first frame update
        void Start()
        {
            _isInitialized = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        [System.Serializable]
        public struct ProjectileTypeConfig
        {
            public ProjectileTypes Type;
            public Projectile Prefab;
            //public List<AudioClip> FireAudioClips;
        }

    }
}
