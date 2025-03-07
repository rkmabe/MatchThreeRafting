using MatchThreePrototype.MatchReaction.MatchTypes;
using MatchThreePrototype.PlayAreaElements;
using MatchThreePrototype.PlayerTouchInput;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.MatchReaction
{
    public class MatchFeedback : MonoBehaviour
    {
        //private AudioSource _audioSource;
        //private ItemPool _itemPool;

        [SerializeField] private AudioClip _matchClipL1;
        [SerializeField] private AudioClip _matchClipL2;
        [SerializeField] private AudioClip _matchClipL3;
        [SerializeField] private AudioClip _matchClipL4;
        [SerializeField] private AudioClip _matchClipL5;
        [SerializeField] private AudioClip _matchClipL6;
        [SerializeField] private AudioClip _matchClipL7;

        private int _matchCount;

        private List<Match> _uniqueMatchesPerMove = new List<Match>();

        private bool _isDelayed = false;
        private float _secsDelayed = 0;
        private static float SECS_TO_DELAY = .20f;

        private struct QueuedEntry
        {
            public PlayAreaCell Cell;
            public int FrameQueued;
            public float TimeQueued;
        }
        private Queue<QueuedEntry> _queuedEntries = new Queue<QueuedEntry>();

        private void ProcessQueueEntry()
        {
            QueuedEntry entry = _queuedEntries.Dequeue();

            _matchCount++;

            entry.Cell.AudioSourceGeneral.clip = GetClipForMatchCount();
            entry.Cell.AudioSourceGeneral.Play();

        }
        private void OnMatchCaught(Match match, PlayAreaCell cell)
        {
            bool isMatchUnique = true;
            for (int i = 0; i < _uniqueMatchesPerMove.Count; i++)
            {
                if (_uniqueMatchesPerMove[i].MatchID == match.MatchID)
                {
                    //Debug.Log("We have already seen" + match.MatchID);
                    isMatchUnique = false;
                    break;
                }
            }

            if (isMatchUnique)
            {
                _uniqueMatchesPerMove.Add(match);

                QueuedEntry entry = new QueuedEntry();
                entry.Cell = cell;
                entry.FrameQueued = Time.frameCount;
                entry.TimeQueued= Time.time;

                _queuedEntries.Enqueue(entry);
            }
        }

        private AudioClip GetClipForMatchCount()
        {

            if (_matchCount == 1)
            {
                return _matchClipL1;
            }
            else if (_matchCount == 2)
            {
                return _matchClipL2;
            }
            else if (_matchCount == 3)
            {
                return _matchClipL3;
            }
            else if (_matchCount == 4)
            {
                return _matchClipL4;
            }
            else if (_matchCount == 5)
            {
                return _matchClipL5;
            }
            else if (_matchCount == 6)
            {
                return _matchClipL6;
            }
            else
            {
                return _matchClipL7;
            }
        }

        private void OnStartNewMove(int moveNum)
        {
            _matchCount = 0;

            _uniqueMatchesPerMove.Clear();
        }



        private void OnDestroy()
        {
            //PlayAreaCellMatchDetector.OnMatchCaughtDelegate -= OnMatchCaught;

            PlayAreaColumn.OnMatchRemovedDelegate -= OnMatchCaught;
            MoveItemCell.OnMatchRemovedDelegate -= OnMatchCaught;
            SettleItemCell.OnMatchRemovedDelegate -= OnMatchCaught;

            //TouchDetector.OnTouchInputUpDelegate -= OnTouchInputUp;
            Player.OnStartNewMove -= OnStartNewMove;
        }

        private void Awake()
        {
            //PlayAreaCellMatchDetector.OnMatchCaughtDelegate += OnMatchCaught;

            PlayAreaColumn.OnMatchRemovedDelegate += OnMatchCaught;
            MoveItemCell.OnMatchRemovedDelegate += OnMatchCaught;
            SettleItemCell.OnMatchRemovedDelegate += OnMatchCaught;

            //TouchDetector.OnTouchInputUpDelegate += OnTouchInputUp;
            Player.OnStartNewMove += OnStartNewMove;

            //_itemPool = FindFirstObjectByType<ItemPool>();

            //_audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_isDelayed)
            {
                _secsDelayed += Time.deltaTime;
                if (_secsDelayed > SECS_TO_DELAY)
                {
                    _isDelayed = false;
                    //Debug.Log("delay ENDS");
                }
            }

            if (_isDelayed)
            {
                return;
            }

            if (_queuedEntries.Count > 0)
            {
                ProcessQueueEntry();

                _isDelayed = true;
                _secsDelayed = 0;

                //Debug.Log("delay BEGINS");
            }

        }
    }
}
