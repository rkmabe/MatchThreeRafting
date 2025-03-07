using MatchThreePrototype.PlayAreaElements;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class WaterFlow : MonoBehaviour
    {

        private RawImage _waterTexture;

        [SerializeField] private float _flowSpeed = .01f;

        private PlayArea _playArea;

        private bool _isFingerDownPause = false;

        private TouchToStop _touchToStop;

        private InFluxStopper _inFluxStopper;


        private void OnDestroy()
        {

        }

        private void Awake()
        {
            _waterTexture = GetComponent<RawImage>();

            _playArea = FindFirstObjectByType<PlayArea>();

            _touchToStop = GetComponent<TouchToStop>();

            _inFluxStopper = GetComponent<InFluxStopper>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Vector2 positionDiff = Vector2.up * _flowSpeed * Time.deltaTime;

            if (_touchToStop.IsStopped)
            {
                return;
            }

            if (_inFluxStopper.IsStopped)
            {
                return;
            }


            Vector2 newPosition = _waterTexture.uvRect.position + (Vector2.up * _flowSpeed * Time.deltaTime);

            //_waterTexture.uvRect = new Rect(_waterTexture.uvRect.position + new Vector2(x, y) * Time.deltaTime, _waterTexture.uvRect.size);

            _waterTexture.uvRect = new Rect(newPosition, _waterTexture.uvRect.size);

        }
    }
}
