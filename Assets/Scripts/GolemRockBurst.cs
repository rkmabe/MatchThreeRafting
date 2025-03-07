using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class GolemRockBurst : MonoBehaviour, IRockPieceReturnReceiver
    {

        [SerializeField] private RectTransform _centerRect;

        [SerializeField] private Image _explosionImage;
        private RectTransform _explosionImageRect;

        private Vector3 _startExplosionRectScale;
        private static Vector3 EXPLOSION_IMG_SCALE_MAX = new Vector3(1.5f, 1.5f, 1.5f);

        private List<RockPiece> _rockPieces = new List<RockPiece>();

        private bool _isExploding = false;

        private float _secsExploding = 0;
        private static float MAX_SECS_EXPLODING = .25f;//.15f;

        private bool _isReadyToExplode = false;

        private bool _isWaitingForPiecesToSink = false;

        //private Vector3 _startExplodeAnchoredPosition;

        private int _rockPiecesReadyToReturn;

        private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClipDemise;


        public void UpdateRockPiecesReadyToReturn(int numPieces)
        {
            _rockPiecesReadyToReturn += numPieces;
        }


        private void ScatterPiecesFromCenter()
        {
            for (int i = 0; i < _rockPieces.Count; i++)
            {
                //_rockPieces[i].ScatterFromCenter();
                _rockPieces[i].ScatterOverWater();
            }
        }

        public void SetReadyToExplode()
        {
            _isReadyToExplode = true;
        }

        private void PlayGolemDemiseAudio()
        {
            _audioSource.clip= _audioClipDemise;
            _audioSource.Play();
        }

        private void Awake()
        {
            GetComponentsInChildren(_rockPieces);

            _explosionImageRect = _explosionImage.GetComponent<RectTransform>();
            _startExplosionRectScale = _explosionImageRect.localScale;
            //_startExplodeAnchoredPosition = _explosionImageRect.anchoredPosition;

            _audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_isReadyToExplode)
            {
                PlayGolemDemiseAudio();

                ScatterPiecesFromCenter();

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
                }
                else
                {
                    _explosionImage.color = new Color(_explosionImage.color.r, _explosionImage.color.g, _explosionImage.color.b, Statics.ALPHA_OFF);

                    _isExploding = false;

                    _isWaitingForPiecesToSink = true;
                }
            }
            else if (_isWaitingForPiecesToSink)
            {              
                //if (_rockPiecesReadyToReturn >= _rockPieces.Count)
                if (_rockPiecesReadyToReturn >= _rockPieces.Count && !_audioSource.isPlaying)
                {
                    //ResetToDefault();
                    Destroy(gameObject);
                }
            }
        }
    }
}