using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype
{
    public class CollapsibleScreen : MonoBehaviour
    {

        private Animator _anim;

        private AudioSource _audioSource;

        private bool _isWaitingForOpenFinish = false;
        private bool _isWaitingForCloseFinish = false;
        private bool _isExpanded = false;

        [SerializeField] private List<GameObject> _enclosededElements = new List<GameObject>();

        [SerializeField] private bool _playMenuSounds = true;
        [SerializeField] private AudioClip _menuOpenSound = null;
        [SerializeField] private AudioClip _menuCloseSound = null;

        public static Action OnCollapsibleScreenOpen;
        public static Action OnCollapsibleScreenClose;

        private void NotifyAtMinSize()
        {
            //Debug.Log(this.gameObject.name + " AT MIN SIZE");

            if (_isWaitingForCloseFinish)
            {
                _anim.SetBool(CollapsibleScreenAnimatorConstants.IsClosingID, false);
                _anim.SetBool(CollapsibleScreenAnimatorConstants.IsWaitingID, true);

                //_canvas.gameObject.SetActive(false);

                _isWaitingForCloseFinish = false;

                _isExpanded = false;

                gameObject.SetActive(false);

            }
        }

        private void NotifyAtMaxSize()
        {
            //Debug.Log(this.gameObject.name + " AT MAX SIZE");

            if (_isWaitingForOpenFinish)
            {

                SetEnclosedElementsActive(true);

                _isWaitingForOpenFinish = false;

                _isExpanded = true;

            }

        }

        internal void Open()
        {

            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }

            if (_playMenuSounds && _menuOpenSound != null)
            {
                _audioSource.clip = _menuOpenSound;
                _audioSource.Play();
            }

            _anim.SetBool(CollapsibleScreenAnimatorConstants.IsWaitingID, false);
            _anim.SetBool(CollapsibleScreenAnimatorConstants.IsOpeningID, true);
            _isWaitingForOpenFinish = true;

            OnCollapsibleScreenOpen();
        }
        internal void Close()
        {
            if (_playMenuSounds && _menuCloseSound != null)
            {
                _audioSource.clip = _menuCloseSound;
                _audioSource.Play();
            }

            SetEnclosedElementsActive(false);
            _anim.SetBool(CollapsibleScreenAnimatorConstants.IsOpeningID, false);
            _anim.SetBool(CollapsibleScreenAnimatorConstants.IsClosingID, true);
            _isWaitingForCloseFinish = true;

            OnCollapsibleScreenClose();


        }

        internal void SetEnclosedElementsActive(bool active)
        {
            for (int i = 0; i < _enclosededElements.Count; i++)
            {
                _enclosededElements[i].gameObject.SetActive(active);
            }
        }

        private void OnEnable()
        {
            //_anim = GetComponent<Animator>();
        }

        private void Awake()
        {
            //_anim = GetComponent<Animator>();
            //_canvas = GetComponentInParent<Canvas>();
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

    static class CollapsibleScreenAnimatorConstants
    {
        // provide optimized access to animator parameters

        // bools
        private static string IsOpening = "IsOpening";
        private static string IsClosing = "IsClosing";
        private static string IsWaiting = "IsWaiting";

        // identifiers for use in animator coding 
        //public static int SpeedMultiplierID = Animator.StringToHash(SpeedMultiplier);
        public static int IsOpeningID = Animator.StringToHash(IsOpening);
        public static int IsClosingID = Animator.StringToHash(IsClosing);
        public static int IsWaitingID = Animator.StringToHash(IsWaiting);

    }

}