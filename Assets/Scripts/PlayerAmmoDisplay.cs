using UnityEngine;

namespace MatchThreePrototype.MatchReaction
{


    public class PlayerAmmoDisplay : MonoBehaviour
    {

        [SerializeField] private PlayerInventoryDisplayItem _cannonballDisplay;


        internal void UpdateNumCannonballs(int numCannonballs)
        {
            _cannonballDisplay.UpdateTargetNum(numCannonballs);
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