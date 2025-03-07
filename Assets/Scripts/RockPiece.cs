using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class RockPiece : MonoBehaviour
    {
        // Start is called before the first frame update

        private static Vector3 RIPPLE_SCALE_START = new Vector3(1f, 1f, 1f);
        private static Vector3 RIPPLE_SCALE_FINISH = new Vector3(3f, 3f, 3f);

        [SerializeField] private Sprite _rockSprite;
        [SerializeField] private Sprite _rippleSprite;

        [SerializeField] public GameObject center;
        [SerializeField] public GameObject bottom;


        private Rigidbody2D _body;
        private BoxCollider2D _collider;

        //private RockBurst _rockBurst;
        private IRockPieceReturnReceiver _rockBurst;


        private bool _isSinking = false;
        private bool _isScattering = false;
        private bool _isOverWater = false;


        private float _secsScattering = 0;
        private static float MAX_SECS_EXPLODING = .25f;

        private float _secsSinking = 0;
        private static float MAX_SECS_SINKING = .25f;

        private static float EXPLOSION_FORCE = 600;

        private static float DEFAULT_GRAVITY_SCALE = 500;

        private Image _image;

        private RectTransform _imageRect;

        private Vector3 _startAnchoredPosition;
        private Quaternion _startRectRotation;

        //private bool _isContactPointExplosion = false;

        internal void ResetToDefault()
        {
            _collider.enabled = true;
            _body.gravityScale = DEFAULT_GRAVITY_SCALE;

            _image.sprite = _rockSprite;

            _imageRect.localScale = RIPPLE_SCALE_START;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, Statics.ALPHA_ON);

            _imageRect.anchoredPosition = _startAnchoredPosition;

            _imageRect.rotation = _startRectRotation;

            _isSinking = false;
            _isScattering = false;

            _secsScattering = 0;
            _secsSinking = 0;
        }





        internal void Scatter()
        {
            _isOverWater = false;
            ApplyScatterForce(center.transform.position);
        }
        internal void Scatter(Vector3 contactPoint)
        {
            _isOverWater = false;
            ApplyScatterForce(contactPoint);
        }
        internal void ScatterOverWater()
        {
            _isOverWater = true;
            ApplyScatterForce(center.transform.position);
        }
        internal void ScatterOverWater(Vector3 contactPoint)
        {
            _isOverWater = true;
            ApplyScatterForce(contactPoint);
        }


        private void ApplyScatterForce(Vector3 forceOriginPosition)
        {
            _isScattering= true;
            Vector2 direction = _collider.bounds.center - forceOriginPosition;
            _body.AddForce(direction.normalized * EXPLOSION_FORCE, ForceMode2D.Impulse);
        }


        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _image = GetComponent<Image>();
            _imageRect = _image.GetComponent<RectTransform>();

            _startAnchoredPosition = _imageRect.anchoredPosition;

            _startRectRotation = _imageRect.rotation;

            _rockBurst = transform.parent.GetComponent<IRockPieceReturnReceiver>();

        }
        

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (_isScattering)
            {
                _secsScattering += Time.deltaTime;

                if (_secsScattering > MAX_SECS_EXPLODING)
                {
                    _collider.enabled = false;
                    _body.gravityScale = 0;
                    _body.velocity = Vector2.zero;
                    _body.angularVelocity = 0;
                    _image.sprite = _rippleSprite;

                    _isScattering = false;

                    if (_isOverWater)
                    {
                        _isSinking = true;
                    }
                    else
                    {
                        _rockBurst.UpdateRockPiecesReadyToReturn(1);
                    }
                }
            }

            if (_isSinking)
            {
                _secsSinking += Time.deltaTime;

                if (_secsSinking < MAX_SECS_SINKING)
                {
                    _imageRect.localScale = Vector3.Lerp(RIPPLE_SCALE_START, RIPPLE_SCALE_FINISH, _secsSinking / MAX_SECS_SINKING);

                    float alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsSinking / MAX_SECS_SINKING);
                    _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alphaLerp);
                }
                else
                {

                    _imageRect.localScale = RIPPLE_SCALE_FINISH;
                    _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, Statics.ALPHA_OFF);

                    _isSinking = false;

                    _rockBurst.UpdateRockPiecesReadyToReturn(1);
                }

            }


            //if (_isOverWater)
            //{
            //    // exploding from contact point (rock removal, golem removal)

            //    if (_isScattering)
            //    {
            //        _secsScattering += Time.deltaTime;

            //        if (_secsScattering > MAX_SECS_EXPLODING)
            //        {

            //            _collider.enabled = false;
            //            _body.gravityScale = 0;
            //            _body.velocity = Vector2.zero;
            //            _body.angularVelocity = 0;
            //            _image.sprite = _rippleSprite;

            //            _isScattering = false;
            //            _isSinking = true;

            //        }
            //    }
            //    else if (_isSinking)
            //    {
            //        _secsSinking += Time.deltaTime;
            //        if (_secsSinking < MAX_SECS_SINKING)
            //        {
            //            _imageRect.localScale = Vector3.Lerp(RIPPLE_SCALE_START, RIPPLE_SCALE_FINISH, _secsSinking / MAX_SECS_SINKING);

            //            float alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsSinking / MAX_SECS_SINKING);
            //            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alphaLerp);
            //        }
            //        else
            //        {

            //            _imageRect.localScale = RIPPLE_SCALE_FINISH;
            //            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, Statics.ALPHA_OFF);

            //            _isSinking = false;

            //            _rockBurst.UpdateRockPiecesReadyToReturn(1);
            //        }
            //    }
            //}
            //else
            //{
            //    // exploding from center (obstacle removal)
            //    if (_isScattering)
            //    {
            //        _secsScattering += Time.deltaTime;

            //        if (_secsScattering > MAX_SECS_EXPLODING)
            //        {

            //            _collider.enabled = false;
            //            _body.gravityScale = 0;
            //            _body.velocity = Vector2.zero;
            //            _body.angularVelocity = 0;
            //            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, Statics.ALPHA_OFF);

            //            _isScattering = false;

            //            _rockBurst.UpdateRockPiecesReadyToReturn(1);

            //        }
            //    }
            //}
        }
    }
}