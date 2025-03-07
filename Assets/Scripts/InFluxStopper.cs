using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype
{

    public class InFluxStopper : MonoBehaviour
    {

        public bool IsStopped { get => _isStopped; }
        private bool _isStopped = false;

        private PlayArea _playArea; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_playArea.IsInFlux)
            {
                _isStopped = true;
            }
            else
            {
                _isStopped = false;
            }
        }

        private void Awake()
        {
            _playArea = FindFirstObjectByType<PlayArea>();
        }
    }
}