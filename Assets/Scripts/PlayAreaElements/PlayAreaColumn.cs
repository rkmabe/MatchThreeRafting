using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.MatchReaction.MatchTypes;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MatchThreePrototype.PlayAreaElements
{
    public class PlayAreaColumn : MonoBehaviour
    {
        public int Number { get => _number; }
        [SerializeField] private int _number;  //1-N left to right

        public List<PlayAreaCell> Cells { get => _cells; }
        [SerializeField] private List<PlayAreaCell> _cells = new List<PlayAreaCell>();


        private PlayArea _playArea;

        private IRowInfoProvider _rowInfoProvider;

        private IDropCellHandler _dropCellHandler;

        private List<DropCell> _droppingCells = new List<DropCell>();

        private List<PlayAreaCell> _cellsToCatchMatches = new List<PlayAreaCell>();

        internal void PlayAreaUpdateStateMachines(out bool isPlayAreaInFlux)
        {
            isPlayAreaInFlux = false;
            for (int i = _cells.Count - 1; i >= 0; i--)
            {
                _cells[i].UpdateStateMachine();

                if (_cells[i].ObstacleHandler.GetIsProcessingRemoval() ||
                    _cells[i].BlockHandler.GetIsProcessingRemoval() ||
                    _cells[i].BlockHandler.GetIsProcessingBlockAndItemRemoval() ||
                    _cells[i].ItemHandler.GetIsProcessingRemoval())
                {
                    isPlayAreaInFlux = true;
                }

                if (_cells[i].ItemHandler.GetIsProcessingLanding())
                {
                    isPlayAreaInFlux = true;
                }

                if (_cells[i].ItemHandler.GetIsDynamiteActive())
                {
                    isPlayAreaInFlux = true;
                }

                if (_cells[i].ItemHandler.GetIsBlockObscuring() ||
                    _cells[i].ItemHandler.GetIsObstacleCrushing())
                {
                    isPlayAreaInFlux = true;
                }

            }
        }

        internal void PlayAreaUpdateStateMachines(out bool isStateInFlux,
                                                  out bool isProcessingRemoval,
                                                  out bool isProcessingLanding,
                                                  out bool isProcessingDynamiteActive,
                                                  out bool isProcessingRiverObject)
        {
            isStateInFlux = false;
            isProcessingRemoval = false;
            isProcessingLanding = false;
            isProcessingDynamiteActive= false;
            isProcessingRiverObject = false;

            for (int i = _cells.Count - 1; i >= 0; i--)
            {
                _cells[i].UpdateStateMachine();

                if (_cells[i].ObstacleHandler.GetIsProcessingRemoval() ||
                    _cells[i].BlockHandler.GetIsProcessingRemoval() ||
                    _cells[i].BlockHandler.GetIsProcessingBlockAndItemRemoval() ||
                    _cells[i].ItemHandler.GetIsProcessingRemoval())
                {
                    isStateInFlux = true;
                    isProcessingRemoval = true;
                }

                if (_cells[i].ItemHandler.GetIsProcessingLanding())
                {
                    isStateInFlux = true;
                    isProcessingLanding = true;
                }

                if (_cells[i].ItemHandler.GetIsDynamiteActive())
                {
                    isStateInFlux = true;
                    isProcessingDynamiteActive = true;
                }

                if (_cells[i].ItemHandler.GetIsBlockObscuring() ||
                    _cells[i].ItemHandler.GetIsObstacleCrushing())
                {
                    isStateInFlux = true;
                    isProcessingRiverObject = true;
                }

            }

        }

        internal void UpdateStagedDrops(out bool anyCellsStaged)
        {
            anyCellsStaged = false;
            for (int i = _cells.Count - 1; i >= 0; i--)
            {

                if (_cells[i].ItemHandler.GetItem() == null  && _cells[i].ObstacleHandler.GetObstacle() == null && _cells[i].IsWaitingForDropCell == false && _cells[i].ItemHandler.GetIsProcessingRemoval() == false)
                {
                    DropCell dropCell = _dropCellHandler.FindDropCell(_cells[i]);
                    if (dropCell != null)
                    {
                        _cells[i].IsWaitingForDropCell = true;

                        PlayAreaRowInfo dropToInfo = _rowInfoProvider.GetRowInfo(_cells[i].Number);

                        dropCell.StartDropToTarget(dropToInfo, _cells[i]);

                        _droppingCells.Add(dropCell);

                        anyCellsStaged = true;
                    }
                }
            }
        }
        internal void UpdateDroppingCells(out bool anyCellsDropping)
        {
            anyCellsDropping = false;
            for (int i = _droppingCells.Count - 1; i >= 0; i--)
            {
                anyCellsDropping = true;
                bool hasDropArrived;
                _droppingCells[i].UpdateDropPosition(out hasDropArrived);
                if (hasDropArrived)
                {
                    Item droppingItem = _droppingCells[i].ItemHandler.GetItem();
                    //ItemTypes droppingItemType = ItemTypes.None;
                    //Item droppingItem = _droppingCells[i].ItemHandler.GetItem();
                    //if (droppingItem)
                    //{
                    //    droppingItemType = droppingItem.ItemType;
                    //}

                    _droppingCells[i].TransferContentsToCell();

                    // do not check for matches if a Dynamite item is arriving
                    //_cellsToCatchMatches.Add(_droppingCells[i].TargetCell);

                    //if (droppingItem && droppingItemType != ItemTypes.Dynamite)
                    if (droppingItem)
                    {
                        _cellsToCatchMatches.Add(_droppingCells[i].TargetCell);
                    }

                    _droppingCells.RemoveAt(i);
                }
            }
        }

        //public delegate void OnMatchCaughtNEW(Match match, PlayAreaCell cell);
        //public static OnMatchCaughtNEW OnMatchCaughtDelegateNEW;

        //public delegate void OnMatchRemoved(Match match, PlayAreaCell cell);
        //public static OnMatchRemoved OnMatchRemovedDelegate;

        public static Action<Match, PlayAreaCell> OnMatchRemovedDelegate;

        internal void UpdateMatches(out bool anyMatchesCaught)
        {
            for (int i = _cellsToCatchMatches.Count-1; i >= 0 ; i--)
            {
                //(List<PlayAreaCell> matchesCaught, List<PlayAreaCell> obstaclesCaught) = _cellsToCatchMatches[i].MatchDetector.CatchMatchThree(true);
                //(List<Match> matchesCaught, List<PlayAreaCell> obstaclesCaught) = _cellsToCatchMatches[i].MatchDetector.CatchMatchThreeNEW(true);

                (List<Match> matchObjectsCaught, List<PlayAreaCell> CellsWithItemsToRemove, List<PlayAreaCell> cellsWithObstaclesToRemove) = _cellsToCatchMatches[i].MatchDetector.CatchMatchThree(true);

                //for (int j = 0; j < matchesCaught.Count; j++)
                //{
                //    matchesCaught[j].QueueBlockOrItemRemoval();
                //    anyMatchesCaught = true;
                //}
                //for (int j = 0; j < matchesCaught.Count; j++)
                //{
                //    anyMatchesCaught = true;
                //    OnMatchCaughtDelegateNEW(matchesCaught[j], _cellsToCatchMatches[i]);
                //    for (int k = 0; k < matchesCaught[j].CellsMatched.Count; k++)
                //    {
                //        matchesCaught[j].CellsMatched[k].QueueBlockOrItemRemoval();
                //    }
                //}

                for (int j = 0; j < CellsWithItemsToRemove.Count; j++)
                {
                    CellsWithItemsToRemove[j].QueueBlockOrItemRemoval();
                    anyMatchesCaught = true;
                }

                for (int k = 0; k < cellsWithObstaclesToRemove.Count; k++)
                {
                    cellsWithObstaclesToRemove[k].QueueObstacleForRemoval();
                }

                for (int m = 0; m < matchObjectsCaught.Count; m++)
                {
                    //OnMatchCaughtDelegateNEW(matchObjectsCaught[m], _cellsToCatchMatches[i]);
                    OnMatchRemovedDelegate(matchObjectsCaught[m], _cellsToCatchMatches[i]);
                }

                _cellsToCatchMatches.RemoveAt(i);
            }
            anyMatchesCaught = false;
        }

        private void Awake()
        {
            _playArea = GetComponentInParent<PlayArea>();

            _rowInfoProvider = GetComponent<IRowInfoProvider>();

            _dropCellHandler = GetComponent<IDropCellHandler>();



        }

        // Start is called before the first frame update
        void Start()
        {
            // this MUST be done in start.  PlayAreaCell caches RectTransform in Awake.
            _rowInfoProvider.SetupRowInfo(_cells);

            _dropCellHandler.Setup(_playArea, this, _rowInfoProvider);
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}
