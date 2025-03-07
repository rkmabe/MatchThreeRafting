using UnityEngine;

namespace MatchThreePrototype
{
    public abstract class Projectile : MonoBehaviour
    {

        public ProjectileTypes ProjectileType { get => _projectileType; }
        [SerializeField] private ProjectileTypes _projectileType;


        private Rigidbody2D _rigidBody;

        private static float FIXED_SPEED = 1500;


        // TODO: revisit if some projectiles do other things besides cause damage on impact .. 
        //public abstract void ProcessOnImpact(IWeaponTarget target, ContactPoint2D point);


        public void Send(IWeaponTarget target) 
        {
            Vector2 direction = target.GetTransformPosition() - transform.position;

            _rigidBody.AddForce(direction.normalized * FIXED_SPEED, ForceMode2D.Impulse);
        }

        private void Awake()
        {
            //_rectTransform = GetComponent<RectTransform>();
            _rigidBody = GetComponent<Rigidbody2D>();
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

    public enum ProjectileTypes
    {
        None = 0,

        Cannonball = 1,

        //White = 1,
        //Green = 2,
        //Blue = 3,
        //Red = 4,
        //Purple = 5,
        //Black = 6,
        //Pink = 7,
    }

}
