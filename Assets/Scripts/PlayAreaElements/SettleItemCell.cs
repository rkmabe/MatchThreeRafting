using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections.Generic;
using MatchThreePrototype.MatchReaction.MatchTypes;
using UnityEngine;
using System;

namespace MatchThreePrototype.PlayAreaElements
{

    public class SettleItemCell : MonoBehaviour
    {

        private static Vector2 SCALE_HELD = Statics.SCALE_HELD;
        private static Vector2 SCALE_SETTLED = Statics.Vector3One();

        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;

        private RectTransform _rectTransform;

        private PlayAreaCell _targetCell;

        private float _lowerStep = Statics.LIFT_LOWER_STEP;

        public bool IsSettling { get => _isSettling; }
        private bool _isSettling = false;

        private List<PlayAreaCell> _cellMatchesCaught = new List<PlayAreaCell>();
        private List<PlayAreaCell> _obstaclesCaught = new List<PlayAreaCell>();
        private List<Match> _matchObjectsCaught = new List<Match>();

        private float _amountToScale;

        private Vector3 _startPosition;

        public static Action<Match, PlayAreaCell> OnMatchRemovedDelegate;

        internal void StartSettling(Vector3 startPosition, PlayAreaCell destinationCell, Item item)
        {
            _startPosition = startPosition;

            transform.position = startPosition;

            _targetCell = destinationCell;

            _isSettling = true;

            _itemHandler.SetItem(item, Statics.ALPHA_ON);

            _rectTransform.localScale = SCALE_HELD;
        }

        internal void StartSettling(Vector3 startPosition, PlayAreaCell destinationCell, Item item, List<PlayAreaCell> cellMatchesCaught, List<PlayAreaCell> obstaclesCaught, List<Match> matchObjectsCaught)
        {
            //_startPosition = startPosition;

            //transform.position = startPosition;

            //_targetCell = destinationCell;

            //_isSettling = true;

            //_itemHandler.SetItem(item, Statics.ALPHA_ON);

            //_rectTransform.localScale = SCALE_HELD;

            StartSettling(startPosition, destinationCell, item);

            _cellMatchesCaught = cellMatchesCaught;

            _obstaclesCaught = obstaclesCaught;

            _matchObjectsCaught = matchObjectsCaught;

        }

        internal void FinishSettling()
        {
            //Debug.Log("FINISH SETTLE");

            Item movingItem = _itemHandler.GetItem();
            if (movingItem == null)
            {
                //Debug.LogWarning("ITEM should NOT be null on arrival!");
            }
            else
            {
                _targetCell.ItemHandler.SetItem(movingItem, Statics.ALPHA_ON);

                if (movingItem.ItemType == ItemTypes.Dynamite)
                {
                    // put the dynamite image in front of any other (obstacle, block)
                    _targetCell.ItemHandler.GetImage().rectTransform.SetAsLastSibling();
                    // transition to fuse lit state
                    _targetCell.ItemHandler.StartDynamiteActive();
                }
            }

            for (int i = _cellMatchesCaught.Count - 1; i >= 0; i--)
            {
                _cellMatchesCaught[i].QueueBlockOrItemRemoval();
                _cellMatchesCaught.RemoveAt(i);
            }

            for (int j = _obstaclesCaught.Count - 1; j >= 0; j--)
            {
                _obstaclesCaught[j].QueueObstacleForRemoval();
                _obstaclesCaught.RemoveAt(j);
            }

            for (int k = 0; k < _matchObjectsCaught.Count; k++)
            {
                OnMatchRemovedDelegate(_matchObjectsCaught[k],_targetCell);
            }
            _matchObjectsCaught.Clear();


            _itemHandler.RemoveItem();

            _targetCell = null;
        }

        internal void UpdateTransform(out bool isComplete)
        {
            if (Statics.IsCloseEnough(_rectTransform.localScale, SCALE_SETTLED, .01f))
            {
                _isSettling = false;
            }
            else
            {
                _rectTransform.localScale = Vector2.MoveTowards(_rectTransform.localScale, SCALE_SETTLED, _lowerStep);

                float amountScaled = Mathf.Abs(_rectTransform.localScale.x - SCALE_HELD.x);

                transform.position = Vector3.Lerp(_startPosition, _targetCell.transform.position, amountScaled / _amountToScale);
            }

            isComplete = !(_isSettling);
        }


        private void Awake()
        {
            _itemHandler= GetComponent<IItemHandler>();
            _rectTransform = GetComponent<RectTransform>();

            _lowerStep *= Time.deltaTime;

            //_moveStep *= Time.deltaTime;

            _amountToScale = Mathf.Abs(SCALE_SETTLED.x - SCALE_HELD.x);
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