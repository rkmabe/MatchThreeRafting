using MatchThreePrototype.MatchDetection;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{
    public class PlayAreaCell : MonoBehaviour, IComparable<PlayAreaCell>
    {
        public int Number { get => _number; }
        public int ColumnNumber { get => _parentColumn.Number; }

        private TMPro.TextMeshProUGUI _debugText;

        [SerializeField] private int _number;  //1-N top to bottom

        private PlayArea _playArea;
        private PlayAreaColumn _parentColumn;


        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;

        public IObstacleHandler ObstacleHandler { get => _obstacleHandler; }
        private IObstacleHandler _obstacleHandler;

        public IBlockHandler BlockHandler { get => _blockHandler; }
        private IBlockHandler _blockHandler;

        public IPlayAreaCellMatchDetector MatchDetector { get => _matchDetector; }
        private IPlayAreaCellMatchDetector _matchDetector;

        public IStagedItemHandler StagedItemHandler { get => _stagedItemHandler; }
        private IStagedItemHandler _stagedItemHandler;

        public EffectHandler EffectHandler { get => _effectHandler; }
        private EffectHandler _effectHandler;

        internal bool IsWaitingForDropCell { get => _isWaitingForDropCell; set => _isWaitingForDropCell = value; }
        private bool _isWaitingForDropCell = false;

        public RectTransform RectTransform { get => _rectTransform; }
        private RectTransform _rectTransform;

        internal static float DEFAULT_REMOVAL_DURATION = .5f;

        public PlayAreaCellStateMachine StateMachine { get => _stateMachine; }
        private PlayAreaCellStateMachine _stateMachine;

        //private RockBurstPool _burstPool;

        //public AudioSource AudioSource { get => _audioSource; }
        //private AudioSource _audioSource;

        public AudioSource AudioSourceGeneral { get => _audioSourceGeneral; }
        [SerializeField] private AudioSource _audioSourceGeneral;

        public AudioSource AudioSourceDynamite { get => _audioSourceDynamite; }
        [SerializeField] private AudioSource _audioSourceDynamite;

        public void UpdateStateMachine()
        {
            _stateMachine.Update();
        }

        public override string ToString()
        {
            return ColumnNumber + "," + _number + " Item=" + _itemHandler.GetItem() + ", StagedItem=" + _stagedItemHandler.GetStagedItem();
        }

        // PlayAreaColumn sorts cells DESCENDING by CELL NUMBER (ie row number)
        public int CompareTo(PlayAreaCell compareCell)
        {
            if (compareCell == null)
            {
                return 1;
            }
            else
            {
                //return this.Number.CompareTo(compareCell.Number); // ascending
                return compareCell.Number.CompareTo(this.Number); // descending
            }
        }

        internal void QueueItemRemoval()
        {
            if (_itemHandler.GetItem() != null)
            {
                if (_itemHandler.GetIsProcessingRemoval())
                {
                    //Debug.Log("already processing ITEM removal! -" + _parentColumn.Number + ", " + _number);
                }
                else
                {
                    _itemHandler.StartRemoval();
                }
            }
        }
        internal void QueueBlockRemoval()
        {
            if (_blockHandler.GetBlock() != null)
            {
                if (_blockHandler.GetIsProcessingRemoval())
                {
                    //Debug.Log("already processing BLOCK removal! -" + _parentColumn.Number + ", " + _number);
                }
                else
                {
                    _blockHandler.StartRemoval();
                }
            }
        }
        internal void QueueBlockAndItemRemoval()
        {
            //Debug.Log("Q both Block and Item removal");


            if (_blockHandler.GetIsProcessingBlockAndItemRemoval())
            {
                //Debug.Log("already processing BLOCK and ITEM removal! -" + _parentColumn.Number + ", " + _number);
            }
            else
            {
                _blockHandler.StartBlockAndItemRemoval();
            }

            //_blockHandler.StartRemoval();
            //_itemHandler.StartRemoval();
        }

        internal void QueueBlockOrItemRemoval()
        {
            if (_blockHandler.GetBlock() != null)
            {
                if (_blockHandler.GetIsProcessingRemoval())
                {
                    //Debug.Log("already processing BLOCK removal! -" + _parentColumn.Number + ", " + _number);
                }
                else
                {
                    _blockHandler.StartRemoval();
                }
            }
            else
            {
                if (_itemHandler.GetIsProcessingRemoval())
                {
                    //Debug.Log("already processing ITEM removal! -" + _parentColumn.Number + ", " + _number);
                }
                else
                {
                    _itemHandler.StartRemoval();
                }
            }
        }

        internal void QueueObstacleForRemoval()
        {
            if (_obstacleHandler.GetObstacle() != null)
            {
                if (_obstacleHandler.GetIsProcessingRemoval())
                {
                    //Debug.Log("already processing OBSTACLE removal! -" + _parentColumn.Number + ", " + _number);
                }
                else
                {
                    _obstacleHandler.StartRemoval();
                }
            }
        }

        internal void ProcessDynamiteIgnition()
        {

            //Debug.Log("Find adjacent cells, blow them up");

            List<PlayAreaCell> cells = _playArea.GetAdjacentCells(this);

            for (int i = 0; i < cells.Count; i++)
            {
                //cells[i].ItemHandler.StartRemoval();


                if ((cells[i].BlockHandler.GetBlock() != null) && (cells[i].ItemHandler.GetItem() != null))
                {

                    //cells[i].QueueBlockAndItemRemoval();

                    if (cells[i].ItemHandler.GetItem().ItemType == ItemTypes.Dynamite)
                    {
                        cells[i].ItemHandler.StartDynamiteActive();
                    }
                    else
                    {
                        cells[i].QueueBlockAndItemRemoval();
                    }

                }
                else if (cells[i].BlockHandler.GetBlock() != null)
                {
                    // this should not happen
                    //Debug.LogWarning("ONLY a block in cell - this should not happen");
                    QueueBlockRemoval();
                }
                else if (cells[i].ItemHandler.GetItem() != null)
                {
                    if (cells[i].ItemHandler.GetItem().ItemType == ItemTypes.Dynamite)
                    {
                        cells[i].ItemHandler.StartDynamiteActive();
                    }
                    else
                    {
                        cells[i].QueueItemRemoval();
                    }


                    //cells[i].ItemHandler.StartRemoval();
                }
                else if (cells[i].ObstacleHandler.GetObstacle() != null)
                {
                    cells[i].QueueObstacleForRemoval();
                    //cells[i].ObstacleHandler.StartRemoval();

                }
            }



            // if there is an objstacle in THIS cell, animate its removal
            if (_obstacleHandler.GetObstacle())
            {
                //_obstacleHandler.StartRemoval();
                QueueObstacleForRemoval();
            }

            // if there is a block in THIS cell, animate its removal
            // TODO: if you introduce more than 1 block level, ALL must be removed here
            if (_blockHandler.GetBlock())
            {
                //_blockHandler.StartRemoval();
                QueueBlockRemoval();
            }

            // instantly remove the dynamite sprite in THIS cell
            _itemHandler.RemoveItem();

            //_stateMachine.TransitionTo(StateMachine.ItemRemoving);


            //if (_itemHandler.GetItem() != null)
            //{
            //    QueueItemForRemoval();
            //}
            //else if (_obstacleHandler.GetObstacle() != null)
            //{
            //    QueueObstacleForRemoval();
            //}

        }

        //private void SetDebugText()
        //{
        //    _debugText.text = _parentColumn.Number + ", " + _number;
        //}

        private string GetCellIDText()
        {
            return _parentColumn.Number + ", " + _number;
        }

        private void OnDestroy()
        {
            _stateMachine.CleanUpOnDestroy();
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _playArea = GetComponentInParent<PlayArea>();

            _parentColumn = GetComponentInParent<PlayAreaColumn>();

            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (_debugText != null)
            {
                _debugText.text = GetCellIDText();
            }


            _itemHandler = GetComponent<IItemHandler>();
            _obstacleHandler = GetComponent<IObstacleHandler>();
            _blockHandler = GetComponent<IBlockHandler>();

            _stagedItemHandler = GetComponent<IStagedItemHandler>();

            _effectHandler = GetComponent<EffectHandler>();

            _matchDetector = GetComponent<IPlayAreaCellMatchDetector>();
            _matchDetector.Setup(_playArea, _parentColumn, this, _itemHandler, _stagedItemHandler);

            //RockBurstPool burstPool = FindFirstObjectByType<RockBurstPool>();
            //_burstPool = FindFirstObjectByType<RockBurstPool>();

            //PlayerAmmoMeter ammoMeter = FindFirstObjectByType<PlayerAmmoMeter>();

            //DynamiteResourcesMB dynamiteResourcesMB = FindFirstObjectByType<DynamiteResourcesMB>();

            //_stateMachine = new CellStateMachine(this, _burstPool);
            _stateMachine = new PlayAreaCellStateMachine(this, _playArea.BurstPool, _playArea.PlayerAmmoMeter.AmmoFlyToPosition.position, _playArea.DynamiteResources, _playArea.ItemRemovingResources, _playArea.ObstacleCrushingResources);
            _stateMachine.Initialize(_stateMachine.CellEmpty);

            //_audioSource = GetComponent<AudioSource>();
        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //_debugText.text = _stateMachine.CurrentState.ToString();
            if (_debugText.enabled)
            {
                if (_itemHandler.GetItem() == null)
                {
                    _debugText.text = GetCellIDText();
                }
                else
                {
                    _debugText.text = GetCellIDText() + System.Environment.NewLine + _itemHandler.GetItem().DrawID.ToString();
                }
            }
        }
    }

}
