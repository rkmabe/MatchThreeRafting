using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MatchThreePrototype
{
    public class RockBurst : MonoBehaviour, IRockPieceReturnReceiver
    {

        //[SerializeField] private List<RockPiece> _rockPieces;

        [SerializeField] private Image _contactPointImage;
        private RectTransform _contactPointRect;

        [SerializeField] private Image _explosionImage;
        private RectTransform _explosionImageRect;


        private Vector3 _startExplosionRectScale;
        private static Vector3 EXPLOSION_IMG_SCALE_MAX = new Vector3(1.5f, 1.5f, 1.5f);

        private Vector3 _startContactPointRectScale;
        //private static Vector3 CONTACT_IMG_SCALE_MAX = new Vector3(.5F, .5f, .5f);
        private static Vector3 CONTACT_IMG_SCALE_MAX = new Vector3(.75F, .75f, .75f);

        private List<RockPiece> _rockPieces = new List<RockPiece>();



        private bool _isExploding = false;

        private float _secsExploding = 0;
        private static float MAX_SECS_EXPLODING = .15f;

        //public bool IsReadyToExplode { get => _isReadyToExplode; set => _isReadyToExplode = value; }
        private bool _isReadyToExplode = false;

        //public ContactPoint2D ContactPoint;
        private ContactPoint2D _contactPoint;

        public static Action<RockBurst> OnPooledRockBurstReturn;

        //private int _rockPiecesSunk;
        private int _rockPiecesReadyToReturn;

        private bool _isWaitingForPiecesToSink = false;

        private Vector3 _startContactAnchoredPosition;
        private Vector3 _startExplodeAnchoredPosition;

        private bool _isContactPointExplosion = false;

        //[SerializeField] private GameObject _center;
        [SerializeField] private RectTransform _centerRect;

        private Vector3 _contactPointRectDestination;

        //private AudioSource _audioSource;
        //[SerializeField] private AudioClip _rockImpactAudioClip;

        public void UpdateRockPiecesReadyToReturn(int numPieces)
        {
            _rockPiecesReadyToReturn += numPieces;
        }

        internal void ResetToDefault()
        {

            _explosionImage.color = new Color(_explosionImage.color.r, _explosionImage.color.g, _explosionImage.color.b, Statics.ALPHA_ON );
            _contactPointImage.color = new Color(_contactPointImage.color.r, _contactPointImage.color.g, _contactPointImage.color.b, Statics.ALPHA_ON);

            _explosionImageRect.localScale = _startExplosionRectScale;
            _contactPointRect.localScale = _startContactPointRectScale;

            _contactPointRect.anchoredPosition = _startContactAnchoredPosition;
            _explosionImageRect.anchoredPosition = _startExplodeAnchoredPosition;

            _isContactPointExplosion = false;
            _isExploding = false;
            _isReadyToExplode = false;
            _secsExploding= 0;
            _rockPiecesReadyToReturn = 0;
            _isWaitingForPiecesToSink = false;
            _contactPoint = default(ContactPoint2D );

            for (int i = 0; i < _rockPieces.Count; i++)
            {
                _rockPieces[i].ResetToDefault();
            }
            
        }

        public  void SetupExplosion()
        {

            _isContactPointExplosion = false;

            _contactPointImage.color = new Color(_contactPointImage.color.r, _contactPointImage.color.g, _contactPointImage.color.b, Statics.ALPHA_OFF);

            _isReadyToExplode = true;
        }
        private void ScatterPiecesFromCenter()
        {
            for (int i = 0; i < _rockPieces.Count; i++)
            {
                //_rockPieces[i].ScatterFromCenter();
                _rockPieces[i].Scatter();
            }
        }

        public void SetupExplosion(ContactPoint2D contactPoint)
        {

            _isContactPointExplosion = true;
            _contactPoint = contactPoint;
            _contactPointRect.position = contactPoint.point;


            Vector3 direction = _centerRect.position - _contactPointRect.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _contactPointRect.rotation = rotation;

            _contactPointRectDestination = _contactPointRect.position + direction * 2;


            _isReadyToExplode = true;

        }
        private void ScatterPiecesFromContactPoint(Vector3 point)
        {
            for (int i = 0; i < _rockPieces.Count; i++)
            {
                //_rockPieces[i].ScatterFromContactPoint(point);
                _rockPieces[i].ScatterOverWater(point);
            }
        }

        //private void PlayRockImpactAudio()
        //{
        //    _audioSource.clip = _rockImpactAudioClip;
        //    _audioSource.Play();

        //}

        private void Awake()
        {
            GetComponentsInChildren(_rockPieces);

            _explosionImageRect = _explosionImage.GetComponent<RectTransform>();
            _startExplosionRectScale = _explosionImageRect.localScale;
            _startExplodeAnchoredPosition = _explosionImageRect.anchoredPosition;

            _contactPointRect = _contactPointImage.GetComponent<RectTransform>();
            _startContactPointRectScale = _contactPointRect.localScale;
            _startContactAnchoredPosition = _contactPointRect.anchoredPosition;

            //_audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            //Explode();
        }

        // Update is called once per frame
        void Update()
        {

            if (_isReadyToExplode)
            {
                //PlayRockImpactAudio(); //PlayScatterAudio()?

                if (_isContactPointExplosion)
                {
                    ScatterPiecesFromContactPoint(_contactPoint.point);
                }
                else
                {

                    ScatterPiecesFromCenter();
                }

                _isExploding = true;
                _isReadyToExplode = false;
            }


            if (_isExploding)
            {
                _secsExploding += Time.deltaTime;
                if (_secsExploding < MAX_SECS_EXPLODING)
                {
                    float alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsExploding / MAX_SECS_EXPLODING);

                    _explosionImage.color = new Color(_explosionImage.color.r, _explosionImage.color.g, _explosionImage.color.b, alphaLerp);
                    _explosionImageRect.localScale = Vector3.Lerp(_startExplosionRectScale, EXPLOSION_IMG_SCALE_MAX, _secsExploding / MAX_SECS_EXPLODING);

                    if (_isContactPointExplosion)
                    {
                        _contactPointImage.color = new Color(_contactPointImage.color.r, _contactPointImage.color.g, _contactPointImage.color.b, alphaLerp);
                        _contactPointRect.localScale = Vector3.Lerp(_startContactPointRectScale, CONTACT_IMG_SCALE_MAX, _secsExploding / MAX_SECS_EXPLODING);

                        _contactPointRect.position = Vector3.Lerp(_contactPointRect.transform.position, _contactPointRectDestination, _secsExploding / MAX_SECS_EXPLODING);
                    }

                }
                else
                {
                    _explosionImage.color = new Color(_explosionImage.color.r, _explosionImage.color.g, _explosionImage.color.b, 0);
                    _contactPointImage.color = new Color(_contactPointImage.color.r, _contactPointImage.color.g, _contactPointImage.color.b, 0);
 
                    _isExploding = false;

                    _isWaitingForPiecesToSink = true;

                }
            }
            else if (_isWaitingForPiecesToSink)
            {
                if (_rockPiecesReadyToReturn >= _rockPieces.Count)
                {
                    //Debug.Log("RETURNED rock to pool!");

                    OnPooledRockBurstReturn(this);
                }
                else
                {
                    //Debug.Log("WAITING for " + _rockPieces.Count + " pieces to SINK : " + _rockPiecesSunk);
                }
            }

        }
    }
}
