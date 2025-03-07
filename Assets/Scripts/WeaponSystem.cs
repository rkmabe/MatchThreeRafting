using UnityEngine;

namespace MatchThreePrototype
{

    public class WeaponSystem : MonoBehaviour
    {

        private Weapon _weapon;


        //private void OnMatchCaught(Match match)
        //{
        //    if (match is IProjectileMatch)
        //    {
        //        IProjectileMatch projectileMatch = (IProjectileMatch)match;

        //        _weapon.LoadWeapon(match.NumMatches, projectileMatch.GetProjectileType());
        //    }
        //}

        private void OnDestroy()
        {
            //PlayAreaCellMatchDetector.OnMatchCaughtDelegate -= OnMatchCaught;
        }

        private void Awake()
        {
            //PlayAreaCellMatchDetector.OnMatchCaughtDelegate += OnMatchCaught;

            _weapon = FindFirstObjectByType<Weapon>();
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