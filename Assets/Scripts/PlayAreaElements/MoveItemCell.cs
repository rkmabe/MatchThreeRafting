using MatchThreePrototype.Controllers;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaElements;
using MatchThreePrototype.MatchReaction.MatchTypes;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MatchThreePrototype
{

    public class MoveItemCell : MonoBehaviour
    {

        public bool IsMoving { get => _isMoving; }
        private bool _isMoving = false;

        //public int TargetColumn { get => _targetColNum; }
        //private int _targetColNum;

        private PlayAreaCell _targetCell;

        private List<PlayAreaCell> _cellMatchesCaught = new List<PlayAreaCell>();

        private List<PlayAreaCell> _obstaclesCaught = new List<PlayAreaCell>();

        private List<Match> _matchObjectsCaught = new List<Match>();

        //private PlayArea _playArea;

        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;

        private TMPro.TextMeshProUGUI _debugText;

        private RectTransform _rectTransform;

        public static Action<Match, PlayAreaCell> OnMatchRemovedDelegate;

        internal static float DEFAULT_MOVE_SPEED = Statics.DEFAULT_MOVE_SPEED;
        private float _moveSpeed = DEFAULT_MOVE_SPEED;



        internal void SetCellMatchesCaught(List<PlayAreaCell> cellMatchesCaught)
        {
            _cellMatchesCaught = cellMatchesCaught;
        }
        internal void SetObstaclesCaught(List<PlayAreaCell> obstaclesCaught)
        {
            _obstaclesCaught = obstaclesCaught;
        }
        internal void SetMatchObjectsCaught(List<Match> matchObjectsCaught)
        {
            _matchObjectsCaught = matchObjectsCaught;
        }

        internal void SetTargetCell(PlayAreaCell cell)
        {
            _targetCell = cell;
            //_targetColNum = cell.ColumnNumber;

            _isMoving = true;
        }

        internal void RemoveTarget()
        {
            _targetCell = null;
            //_targetColNum = 0;
        }

        internal void ProcessOnArrival()
        {
            Item movingItem = _itemHandler.GetItem();
            if (movingItem == null)
            {
                //Debug.LogWarning("ITEM should NOT be null on arrival!");
            }
            else
            {
                _targetCell.ItemHandler.SetItem(movingItem, Statics.ALPHA_ON);

                if (movingItem.ItemType==ItemTypes.Dynamite)
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
                OnMatchRemovedDelegate(_matchObjectsCaught[k], _targetCell);
            }
            _matchObjectsCaught.Clear();


            _itemHandler.RemoveItem();
            RemoveTarget();
        }

        internal void UpdatePosition(out bool hasArrived)
        {
            if (Statics.IsCloseEnough(_rectTransform.position, _targetCell.RectTransform.position))
            {
                _isMoving = false;
            }
            else
            {
                _rectTransform.position = Vector2.MoveTowards(_rectTransform.position, _targetCell.RectTransform.position, _moveSpeed * Time.deltaTime);
            }

            hasArrived = !(_isMoving);
        }


        internal void OnNewMoveSpeed(float speed)
        {
            _moveSpeed = speed;
            //Debug.Log("MOVESPEED=" + _moveSpeed);
        }

        private void OnDestroy()
        {
            SettingsController.OnNewMoveSpeedDelegate -= OnNewMoveSpeed;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

            SettingsController.OnNewMoveSpeedDelegate  += OnNewMoveSpeed;

            _itemHandler = GetComponent<IItemHandler>();

            //_playArea = GetComponentInParent<PlayArea>();
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
