using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using MatchThreePrototype.MatchReaction.MatchTypes;
using MatchThreePrototype.PlayAreaElements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayerTouchInput
{

    public class Player : MonoBehaviour
    {

        public PlayArea PlayArea { get => _playArea; set => _playArea = value; }
        private PlayArea _playArea;

        private PlayAreaCell _dragOriginCell = null;

        public static Action<int> OnStartNewMove;
        public int MoveNum { get => _moveNum; }
        private int _moveNum = 0;
        [SerializeField] private TMPro.TextMeshProUGUI _moveNumText;

        ITouchInfoProvider _touchInfoProvider;

        public IInventoryManager Inventory { get => _inventory; }
        private IInventoryManager _inventory;



        public void OnTouchInputDown(Vector3 position)
        {
            if (_playArea.IsPlayAreaMovePermitted())
            {
                ProcessFingerDown(position);
            }
            else
            {
                //Debug.Log("YOU CANNOT MOVE - PLAY AREA IN FLUX!!");
            }
        }

        public void OnTouchInputDrag(Touch dragTouch)
        {
            ProcessFingerDrag(dragTouch);
        }

        public void OnTouchInputUp(Vector3 position)
        {
            ProcessFingerUp(position);
        }

        private void ProcessFingerUp(Vector3 position)
        {

            if (_dragOriginCell != null)
            {
                Item dragOriginItem = _dragOriginCell.ItemHandler.GetItem();
                if (dragOriginItem != null)
                {

                    if (_playArea.IsPlayAreaMovePermitted())
                    {

                        ItemTypes dragOriginItemType = dragOriginItem.ItemType;

                        PlayAreaCell dragDestinationCell;
                        bool isDestinationWithinRange = _touchInfoProvider.IsPositionInSwapRange(position, _dragOriginCell, out dragDestinationCell);
                        if (dragDestinationCell == null)
                        {
                            //Debug.Log("DRAG ERROR: no destination cell");

                            StartReturnItemToOriginCell();

                        }
                        else
                        {
                            if (!isDestinationWithinRange)
                            {
                                //Debug.Log("DRAG ERROR: destination out of range");

                                StartReturnItemToOriginCell();

                            }
                            else
                            {
                                if ((dragDestinationCell.Number == _dragOriginCell.Number) &&
                                    (dragDestinationCell.ColumnNumber == _dragOriginCell.ColumnNumber))
                                {
                                    //Debug.Log("DRAG ERROR: drop on self");
                                }
                                else
                                {
                                    if (dragOriginItemType == ItemTypes.Dynamite)
                                    {
                                        DragOriginItemDynamite(dragDestinationCell);
                                    }
                                    else
                                    {
                                        //DragOriginItemRegular(dragDestinationCell);
                                        DragOriginItemRegular(dragDestinationCell);
                                    }

                                }
                            }
                        }


                    }
                    else
                    {

                        StartReturnItemToOriginCell();

                        //Debug.Log("Cannot Process Finger Up - Play Area is in flux.");
                    }

                }
                else
                {
                    //Debug.LogWarning("Dragging a cell without an item - this should not be possible");
                }
            }


            // CLEAR DRAG ORIGIN CELL, REVERT ANY PLAYER HOLDING STATE        
            if (_dragOriginCell != null)
            {
                if (_dragOriginCell.ItemHandler.GetIsPlayerHolding()) //this should be true  here!
                {
                    _dragOriginCell.ItemHandler.FinishPlayerHolding();
                }
                else
                {
                    //Debug.LogWarning("Player not holding item on Finger Up.  This should not happen.");
                }

                _dragOriginCell = null;
            }

            // CLEAR HELD ITEM CELL
            if (_playArea.HeldItemCell.ItemHandler.GetItem() != null)
            {
                _playArea.HeldItemCell.ItemHandler.RemoveItem();
            }
            else
            {
                // Held Item Cell empty on Finger Up. 
                // this can happen when trying to move something while play area is in flux
            }
             
            // CLEAR DRAG INDICATORS
            _playArea.CellIndicators.ClearDragIndicators();
        }

        private void DragOriginItemDynamite(PlayAreaCell dragDestinationCell)
        {

            _moveNum++;
            _moveNumText.text = "Move: " + _moveNum.ToString();
            OnStartNewMove(_moveNum);

            // what is in the destination cell: Block, Obstacle, Item, Empty.
            Block destinationBlock = dragDestinationCell.BlockHandler.GetBlock();
            if (destinationBlock)
            {
                DragDynamiteToBlock2(dragDestinationCell);
                return;
            }

            Obstacle destinationObstacle = dragDestinationCell.ObstacleHandler.GetObstacle();
            if (destinationObstacle)
            {
                DragDynamiteToObstacle(dragDestinationCell);
                return;
            }

            Item destinationItem = dragDestinationCell.ItemHandler.GetItem();
            if (destinationItem)
            {
                if (dragDestinationCell.ItemHandler.GetIsProcessingRemoval())
                {
                    //Debug.LogWarning("Drag Dynamite to Item being REMOVED - should not be possible");
                    DragDynamiteToEmpty(dragDestinationCell);
                    return;
                }
                else
                {
                    if (destinationItem.ItemType==ItemTypes.Dynamite)
                    {
                        DragDynamiteToDynamite(dragDestinationCell);
                        return;
                    }
                    else
                    {
                        DragDynamiteToItem(dragDestinationCell);
                        return;
                    }

                }
            }

            // if we get here, the cell must be empty.
            DragDynamiteToEmpty(dragDestinationCell);
        }

        private void DragDynamiteToBlock2(PlayAreaCell dragDestinationCell)
        {
            _playArea.CellSettleIntoDestination.StartSettling(_playArea.HeldItemCell.transform.position, dragDestinationCell, _dragOriginCell.ItemHandler.GetItem());

            _dragOriginCell.ItemHandler.RemoveItem();
        }

        private void DragDynamiteToObstacle(PlayAreaCell dragDestinationCell)
        {
            _playArea.CellSettleIntoDestination.StartSettling(_playArea.HeldItemCell.transform.position, dragDestinationCell, _dragOriginCell.ItemHandler.GetItem());

            _dragOriginCell.ItemHandler.RemoveItem();
        }

        private void DragDynamiteToItem(PlayAreaCell dragDestinationCell)
        {
            _playArea.CellSettleIntoDestination.StartSettling(_playArea.HeldItemCell.transform.position, dragDestinationCell, _dragOriginCell.ItemHandler.GetItem());
            _dragOriginCell.ItemHandler.RemoveItem();

            // check to see if moving the ITEM to the orgin will cause a match 3
            dragDestinationCell.StagedItemHandler.SetStagedItem(_dragOriginCell.ItemHandler.GetItem());
            _dragOriginCell.StagedItemHandler.SetStagedItem(dragDestinationCell.ItemHandler.GetItem());

            //(List<PlayAreaCell> matchesCaughtAtOrigin, List<PlayAreaCell> obstaclesCaughtAtOrigin) = _dragOriginCell.MatchDetector.CatchMatchThree(false);
            //if (matchesCaughtAtOrigin.Count > 0)
            //{
            //    //Debug.Log(matchesCaughtAtOrigin.Count + " matches caught at origin");
            //}
            (List<Match> matchObjectsCaught, List<PlayAreaCell> matchesCaughtAtOrigin, List<PlayAreaCell> obstaclesCaughtAtOrigin) = _dragOriginCell.MatchDetector.CatchMatchThree(false);
            if (matchObjectsCaught.Count > 0)
            {
                //Debug.Log(matchObjectsCaught.Count + " matches caught at origin");
            }

            // start at destination and move to origin ("drag to" position to "drag from" position)            
            _playArea.CellMoveToOrigin.transform.position = dragDestinationCell.transform.position;
            _playArea.CellMoveToOrigin.SetTargetCell(_dragOriginCell);
            _playArea.CellMoveToOrigin.ItemHandler.SetItem(dragDestinationCell.ItemHandler.GetItem(), Statics.ALPHA_ON);
            _playArea.CellMoveToOrigin.SetCellMatchesCaught(matchesCaughtAtOrigin);
            _playArea.CellMoveToOrigin.SetObstaclesCaught(obstaclesCaughtAtOrigin);
            _playArea.CellMoveToOrigin.SetMatchObjectsCaught(matchObjectsCaught);
            dragDestinationCell.ItemHandler.RemoveItem();

            // removed the items Staged for matching checking
            dragDestinationCell.StagedItemHandler.RemoveStagedItem();
            _dragOriginCell.StagedItemHandler.RemoveStagedItem();
        }

        private void DragDynamiteToEmpty(PlayAreaCell dragDestinationCell)
        {
            _playArea.CellSettleIntoDestination.StartSettling(_playArea.HeldItemCell.transform.position, dragDestinationCell, _dragOriginCell.ItemHandler.GetItem());
            _dragOriginCell.ItemHandler.RemoveItem();

            //_playArea.CellMoveToOrigin.RemoveTarget(); //superflous?

        }

        private void DragDynamiteToDynamite(PlayAreaCell dragDestinationCell)
        {
            _playArea.CellSettleIntoDestination.StartSettling(_playArea.HeldItemCell.transform.position, dragDestinationCell, _dragOriginCell.ItemHandler.GetItem());
            _dragOriginCell.ItemHandler.RemoveItem();

            _playArea.CellMoveToOrigin.transform.position = dragDestinationCell.transform.position;
            _playArea.CellMoveToOrigin.SetTargetCell(_dragOriginCell);
            _playArea.CellMoveToOrigin.ItemHandler.SetItem(dragDestinationCell.ItemHandler.GetItem(), Statics.ALPHA_ON);
            dragDestinationCell.ItemHandler.RemoveItem();
        }

        private void DragOriginItemRegular(PlayAreaCell dragDestinationCell)
        {
            if (dragDestinationCell.BlockHandler.GetBlock() != null)
            {
                //Debug.Log("DRAG ERROR: cannot drag Regular Item to Block");

                StartReturnItemToOriginCell();

                return;
            }

            if (dragDestinationCell.ObstacleHandler.GetObstacle() != null)
            {
                //Debug.Log("DRAG ERROR: cannot drag Regular Item to Obstacle");

                StartReturnItemToOriginCell();

                return;
            }

            _moveNum++;
            _moveNumText.text = "Move: " + _moveNum.ToString();
            OnStartNewMove(_moveNum);

            dragDestinationCell.StagedItemHandler.SetStagedItem(_dragOriginCell.ItemHandler.GetItem());
            _dragOriginCell.StagedItemHandler.SetStagedItem(dragDestinationCell.ItemHandler.GetItem());

            // check matches at DESTINATION - you must have matches at destination to make the move
            (List<Match> matchObjectsCaught, List<PlayAreaCell> cellsWithItemsToRemove, List<PlayAreaCell> cellsWithObstaclesToRemove) = dragDestinationCell.MatchDetector.CatchMatchThree(false);
            if (matchObjectsCaught.Count > 0)
            {

                // SETTLE IN .. 
                _playArea.CellSettleIntoDestination.StartSettling(_playArea.HeldItemCell.transform.position, dragDestinationCell, _dragOriginCell.ItemHandler.GetItem(), cellsWithItemsToRemove, cellsWithObstaclesToRemove, matchObjectsCaught);

                _dragOriginCell.ItemHandler.RemoveItem();

                if (_dragOriginCell.StagedItemHandler.GetStagedItem() != null)
                {
                    (List<Match> matchObjectsCaughtAtOrigin, List<PlayAreaCell> matchesCaughtAtOrigin, List<PlayAreaCell> obstaclesCaughtAtOrigin) = _dragOriginCell.MatchDetector.CatchMatchThree(false);
                    _playArea.CellMoveToOrigin.transform.position = dragDestinationCell.transform.position;
                    _playArea.CellMoveToOrigin.SetTargetCell(_dragOriginCell);
                    _playArea.CellMoveToOrigin.ItemHandler.SetItem(dragDestinationCell.ItemHandler.GetItem(), Statics.ALPHA_ON);
                    _playArea.CellMoveToOrigin.SetCellMatchesCaught(matchesCaughtAtOrigin);
                    _playArea.CellMoveToOrigin.SetObstaclesCaught(obstaclesCaughtAtOrigin);
                    _playArea.CellMoveToOrigin.SetMatchObjectsCaught(matchObjectsCaughtAtOrigin);

                    dragDestinationCell.ItemHandler.RemoveItem();
                }
            }
            else
            {
                //Debug.Log("No matches at destination - move not allowed");

                _moveNum--;
                _moveNumText.text = "Move: " + _moveNum.ToString();

                StartReturnItemToOriginCell();

            }

            dragDestinationCell.StagedItemHandler.RemoveStagedItem();
            _dragOriginCell.StagedItemHandler.RemoveStagedItem();

        }

        //private void DragOriginItemRegular(PlayAreaCell dragDestinationCell)
        //{
        //    if (dragDestinationCell.BlockHandler.GetBlock() != null)
        //    {
        //        //Debug.Log("DRAG ERROR: cannot drag Regular Item to Block");

        //        StartReturnItemToOriginCell();

        //        return;
        //    }

        //    if (dragDestinationCell.ObstacleHandler.GetObstacle() != null)
        //    {
        //        //Debug.Log("DRAG ERROR: cannot drag Regular Item to Obstacle");

        //        StartReturnItemToOriginCell();

        //        return;
        //    }

        //    _moveNum++;
        //    _moveNumText.text = "Move: " + _moveNum.ToString();
        //    OnStartNewMove(_moveNum);



        //    dragDestinationCell.StagedItemHandler.SetStagedItem(_dragOriginCell.ItemHandler.GetItem());
        //    _dragOriginCell.StagedItemHandler.SetStagedItem(dragDestinationCell.ItemHandler.GetItem());

        //    // check matches at DESTINATION - you must have matches at destination to make the move
        //    (List<PlayAreaCell> matchesCaughtAtDestination, List<PlayAreaCell> obstaclesCaughtAtDestination) = dragDestinationCell.MatchDetector.CatchMatchThree(false);
        //    if (matchesCaughtAtDestination.Count > 0)
        //    {

        //        // SETTLE IN .. 
        //        _playArea.CellSettleIntoDestination.StartSettling(_playArea.HeldItemCell.transform.position, dragDestinationCell, _dragOriginCell.ItemHandler.GetItem(), matchesCaughtAtDestination, obstaclesCaughtAtDestination);
                

        //        _dragOriginCell.ItemHandler.RemoveItem();

        //        if (_dragOriginCell.StagedItemHandler.GetStagedItem() != null)
        //        {
        //            (List<PlayAreaCell> matchesCaughtAtOrigin, List<PlayAreaCell> obstaclesCaughtAtOrigin) = _dragOriginCell.MatchDetector.CatchMatchThree(false);
        //            _playArea.CellMoveToOrigin.transform.position = dragDestinationCell.transform.position;
        //            _playArea.CellMoveToOrigin.SetTargetCell(_dragOriginCell);
        //            _playArea.CellMoveToOrigin.ItemHandler.SetItem(dragDestinationCell.ItemHandler.GetItem(), Statics.ALPHA_ON);
        //            _playArea.CellMoveToOrigin.SetCellMatchesCaught(matchesCaughtAtOrigin);
        //            _playArea.CellMoveToOrigin.SetObstaclesCaught(obstaclesCaughtAtOrigin);

        //            dragDestinationCell.ItemHandler.RemoveItem();
        //        }
        //    }
        //    else
        //    {
        //        //Debug.Log("No matches at destination - move not allowed");

        //        _moveNum--;
        //        _moveNumText.text = "Move: " + _moveNum.ToString();

        //        StartReturnItemToOriginCell();

        //    }

        //    dragDestinationCell.StagedItemHandler.RemoveStagedItem();
        //    _dragOriginCell.StagedItemHandler.RemoveStagedItem();

        //}

        private void ProcessFingerDown(Vector3 position)
        {
            PlayAreaCell cell = _touchInfoProvider.GetCellAtPosition(position);

            if (cell != null)
            {
                //Debug.Log("Finger down on " + cell.ColumnNumber + "," + cell.Number);

                if (cell.ItemHandler.GetItem() != null && cell.ItemHandler.GetIsProcessingRemoval() == false && (cell.BlockHandler.GetBlock() == null && cell.ObstacleHandler.GetObstacle() == null))
                {
                    _dragOriginCell = cell;

                    _playArea.CellIndicators.IndicateDragFromCell(cell);

                    _playArea.HeldItemCell.transform.position = cell.transform.position;

                    //_playArea.HeldItemCell.ItemHandler.SetItem(cell.ItemHandler.GetItem(), Statics.ALPHA_ON);
                    _playArea.HeldItemCell.ItemHandler.SetItem(cell.ItemHandler.GetItem(), Statics.HELD_ALPHA_ON);

                    _playArea.HeldItemCell.StartLiftToHeld();

                    cell.ItemHandler.StartPlayerHolding();
                }
            }
        }

        private void ProcessFingerDrag(Touch dragTouch)
        {

            if (_dragOriginCell != null && _playArea.HeldItemCell.ItemHandler.GetItem() != null)
            {
                PlayAreaCell dragOverCell;
                if (_touchInfoProvider.IsPositionInSwapRange(dragTouch.position, _dragOriginCell, out dragOverCell))
                {
                    _playArea.HeldItemCell.transform.position = dragTouch.position;
                    if (dragOverCell != null)
                    {
                        _playArea.CellIndicators.IndicateDragOverCell(dragOverCell);
                    }
                }
            }
        }

        private void StartReturnItemToOriginCell()
        {
            _playArea.CellReturnToOrigin.transform.position = PlayArea.HeldItemCell.transform.position;
            _playArea.CellReturnToOrigin.ItemHandler.SetItem(_dragOriginCell.ItemHandler.GetItem(), Statics.ALPHA_ON);
            _playArea.CellReturnToOrigin.StartReturning(_dragOriginCell);
        }

        private void OnDestroy()
        {
            TouchDetector.OnTouchInputDownDelegate -= OnTouchInputDown;
            TouchDetector.OnTouchInputUpDelegate -= OnTouchInputUp;
            TouchDetector.OnTouchInputDragDelegate -= OnTouchInputDrag;
        }

        private void Awake()
        {

            TouchDetector.OnTouchInputDownDelegate += OnTouchInputDown;
            TouchDetector.OnTouchInputUpDelegate += OnTouchInputUp;
            TouchDetector.OnTouchInputDragDelegate += OnTouchInputDrag;

            _touchInfoProvider = GetComponent<TouchInfoProvider>();

            _inventory = GetComponent<IInventoryManager>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

    }
}
