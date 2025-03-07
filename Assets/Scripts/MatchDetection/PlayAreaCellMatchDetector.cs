using MatchThreePrototype.MatchReaction;
using MatchThreePrototype.MatchReaction.MatchTypes;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaElements;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MatchThreePrototype.MatchDetection
{

    public class PlayAreaCellMatchDetector : MonoBehaviour, IPlayAreaCellMatchDetector
    {
        [SerializeField] private DebugBorderMatcher _borderMatcher;

        private PlayArea _playArea;
        private PlayAreaColumn _column;
        private PlayAreaCell _cell;
        private IItemHandler _itemHandler;
        private IStagedItemHandler  _stagedItemHandler;

        private MatchTypeFactory _matchFactory;

        public void Setup(PlayArea playArea, PlayAreaColumn column, PlayAreaCell cell, IItemHandler itemHandler, IStagedItemHandler stagedItemHandler)
        {
            _playArea = playArea;
            _column = column;
            _cell = cell;
            _itemHandler = itemHandler;
            _stagedItemHandler = stagedItemHandler;
        }


        //public delegate void OnMatchCaught(Match match, PlayAreaCell cell);
        //public static OnMatchCaught OnMatchCaughtDelegate;


        public delegate void OnCellCheckMatchComplete(bool isMatch3);
        public static OnCellCheckMatchComplete OnCellCheckMatchCompleteDelegate;

        public PlayAreaCellMatches CheckAdjacentMatches()
        {
            PlayAreaCellMatches m = new PlayAreaCellMatches();
            m.IsMatchUp = false;
            m.CellMatchUp = null;

            m.IsMatchDown = false;
            m.CellMatchDown = null;

            m.IsMatchLeft = false;
            m.CellMatchLeft = null;

            m.IsMatchRight = false;
            m.CellMatchRight = null;

            m.IsMiddleMatchVert = false;
            m.IsMiddleMatchHorz = false;

            m.IsObstacleUp = false;
            m.CellObstacleUp = null;

            m.IsObstacleDown = false;
            m.CellObstacleDown = null;

            m.IsObstacleLeft = false;
            m.CellObstacleLeft = null;

            m.IsObstacleRight = false;
            m.CellObstacleRight = null;

            Item thisCellItem = (_stagedItemHandler.GetMatchWithStagedItem()) ? _stagedItemHandler.GetStagedItem() : _itemHandler.GetItem();

            // if there is NO item in the cell, there are NO matches.
            //if (thisCellItem != null && !_itemHandler.GetIsProcessingRemoval())
            if (thisCellItem != null)
            {

                m.ItemType = thisCellItem.ItemType;

                // do not attempt to match SPECIAL item types (currently only dynamite)
                if (m.ItemType != ItemTypes.Dynamite)
                {

                    // MATCH UP?
                    PlayAreaCell cellUp = _playArea.GetPlayAreaCell(_column, _cell.Number - 1);
                    if (cellUp != null)
                    {
                        Item itemUP = (cellUp.StagedItemHandler.GetMatchWithStagedItem()) ? cellUp.StagedItemHandler.GetStagedItem() : cellUp.ItemHandler.GetItem();
                        //if (itemUP != null && !cellUp.ItemHandler.GetIsProcessingRemoval())
                        if (itemUP != null)
                        {
                            m.IsMatchUp = (itemUP.ItemType == m.ItemType) ? true : false;
                            if (m.IsMatchUp)
                            {
                                m.CellMatchUp = cellUp;
                            }
                        }
                        //else if (cellUp.ObstacleHandler.GetObstacle() != null && !cellUp.ObstacleHandler.GetIsProcessingRemoval())
                        else if (cellUp.ObstacleHandler.GetObstacle() != null)
                        {
                            m.IsObstacleUp = true;
                            m.CellObstacleUp = cellUp;
                        }
                    }

                    // MATCH DOWN ?
                    PlayAreaCell cellDOWN = _playArea.GetPlayAreaCell(_column, _cell.Number + 1);
                    if (cellDOWN != null)
                    {
                        Item itemDOWN = (cellDOWN.StagedItemHandler.GetMatchWithStagedItem()) ? cellDOWN.StagedItemHandler.GetStagedItem() : cellDOWN.ItemHandler.GetItem();
                        //if (itemDOWN != null && !cellDOWN.ItemHandler.GetIsProcessingRemoval())
                        if (itemDOWN != null)
                        {
                            m.IsMatchDown = (itemDOWN.ItemType == m.ItemType) ? true : false;
                            if (m.IsMatchDown)
                            {
                                m.CellMatchDown = cellDOWN;
                            }
                        }
                        //else if (cellDOWN.ObstacleHandler.GetObstacle() != null && !cellDOWN.ObstacleHandler.GetIsProcessingRemoval())
                        else if (cellDOWN.ObstacleHandler.GetObstacle() != null)
                        {
                            m.IsObstacleDown = true;
                            m.CellObstacleDown = cellDOWN;
                        }
                    }

                    // MATCH LEFT ?
                    PlayAreaColumn colulmnLeft = _playArea.GetPlayAreaColumn(_column.Number - 1);
                    if (colulmnLeft != null)
                    {
                        PlayAreaCell cellLEFT = _playArea.GetPlayAreaCell(colulmnLeft, _cell.Number);
                        if (cellLEFT != null)
                        {
                            Item itemLEFT = (cellLEFT.StagedItemHandler.GetMatchWithStagedItem()) ? cellLEFT.StagedItemHandler.GetStagedItem() : cellLEFT.ItemHandler.GetItem();
                            //if (itemLEFT != null && !cellLEFT.ItemHandler.GetIsProcessingRemoval())
                            if (itemLEFT != null)
                            {
                                m.IsMatchLeft = (itemLEFT.ItemType == m.ItemType) ? true : false;
                                if (m.IsMatchLeft)
                                {
                                    m.CellMatchLeft = cellLEFT;
                                }
                            }
                            //else if (cellLEFT.ObstacleHandler.GetObstacle() != null && !cellLEFT.ObstacleHandler.GetIsProcessingRemoval())
                            else if (cellLEFT.ObstacleHandler.GetObstacle() != null)
                            {
                                m.IsObstacleLeft = true;
                                m.CellObstacleLeft = cellLEFT;
                            }
                        }
                    }

                    // MATCH RIGHT ?
                    PlayAreaColumn columnRight = _playArea.GetPlayAreaColumn(_column.Number + 1);
                    if (columnRight != null)
                    {
                        PlayAreaCell cellRIGHT = _playArea.GetPlayAreaCell(columnRight, _cell.Number);
                        if (cellRIGHT != null)
                        {
                            Item itemRIGHT = (cellRIGHT.StagedItemHandler.GetMatchWithStagedItem()) ? cellRIGHT.StagedItemHandler.GetStagedItem() : cellRIGHT.ItemHandler.GetItem();
                            //if (itemRIGHT != null && !cellRIGHT.ItemHandler.GetIsProcessingRemoval())
                            if (itemRIGHT != null)
                            {
                                m.IsMatchRight = (itemRIGHT.ItemType == m.ItemType) ? true : false;
                                if (m.IsMatchRight)
                                {
                                    m.CellMatchRight = cellRIGHT;
                                }
                            }
                            //else if (cellRIGHT.ObstacleHandler.GetObstacle() != null && !cellRIGHT.ObstacleHandler.GetIsProcessingRemoval())
                            else if (cellRIGHT.ObstacleHandler.GetObstacle() != null)
                            {
                                m.IsObstacleRight = true;
                                m.CellObstacleRight = cellRIGHT;
                            }
                        }
                    }

                    // MIDDLE match?
                    m.IsMiddleMatchVert = (m.IsMatchUp && m.IsMatchDown) ? true : false;
                    m.IsMiddleMatchHorz = (m.IsMatchLeft && m.IsMatchRight) ? true : false;
                }
            }

            // highlight borders
            if (_playArea.EnableDebugBorderMatcher && _borderMatcher != null)
            {
                _borderMatcher.HighlightMatches(m);
            }

            return m;

        }

        private void AddAdjacentObstacles(PlayAreaCellMatches m, List<PlayAreaCell> adjacentObstacleCells)
        {
            if (m.IsObstacleUp)
            {
                adjacentObstacleCells.Add(m.CellObstacleUp);
            }
            if (m.IsObstacleDown)
            {
                adjacentObstacleCells.Add(m.CellObstacleDown);
            }
            if (m.IsObstacleLeft)
            {
                adjacentObstacleCells.Add(m.CellObstacleLeft);
            }
            if (m.IsObstacleRight)
            {
                adjacentObstacleCells.Add(m.CellObstacleRight);
            }
        }

        //public (List<PlayAreaCell> ItemMatchesCaught, List<PlayAreaCell> ObstaclesCaught) CatchMatchThreeOLD(bool isDrop)
        //{
        //    // Look for matches in each cardinal direction.
        //    // if you have a match, "crawl" that direction finding additional matches until chain is broken

        //    List<PlayAreaCell> matchesCaught = new List<PlayAreaCell>();
        //    List<PlayAreaCell> obstaclesCaught = new List<PlayAreaCell>();

        //    PlayAreaCellMatches m1 = CheckAdjacentMatches();


        //    // VERTICAL matches-------------------------------------------------------------------
        //    List<PlayAreaCell> vertCellMatches = new List<PlayAreaCell>();

        //    List<PlayAreaCell> vertObstaclesAdjacent = new List<PlayAreaCell>();
        //    AddAdjacentObstacles(m1, vertObstaclesAdjacent);

        //    if (m1.IsMatchUp)
        //    {
        //        //vertCellMatches.Add(this);
        //        vertCellMatches.Add(_cell);
        //        vertCellMatches.Add(m1.CellMatchUp);

        //        bool matchUpChainBroken = false;
        //        //int cellNumUp = _number;
        //        int cellNumUp = _cell.Number;
        //        while (!matchUpChainBroken)
        //        {
        //            cellNumUp--;

        //            PlayAreaCell cellUpChain = _playArea.GetPlayAreaCell(_column, cellNumUp);
        //            PlayAreaCellMatches mUP = cellUpChain.MatchDetector.CheckAdjacentMatches();

        //            if (mUP.IsMatchUp)
        //            {
        //                vertCellMatches.Add(mUP.CellMatchUp);
        //            }
        //            else
        //            {
        //                matchUpChainBroken = true;
        //            }

        //            AddAdjacentObstacles(mUP, vertObstaclesAdjacent);

        //        }
        //    }

        //    if (m1.IsMatchDown)
        //    {
        //        // this could have already been added if it matches the item ABOVE (if this is the middle of a match)
        //        // so check before adding it here again for a match BELOW
        //        if (!vertCellMatches.Contains(_cell))
        //        {
        //            vertCellMatches.Add(_cell);
        //        }
        //        vertCellMatches.Add(m1.CellMatchDown);


        //        bool matchDownChainBroken = false;
        //        int cellNumDown = _cell.Number;
        //        while (!matchDownChainBroken)
        //        {
        //            cellNumDown++;
        //            PlayAreaCell cellDownChain = _playArea.GetPlayAreaCell(_column, cellNumDown);
        //            PlayAreaCellMatches mDOWN = cellDownChain.MatchDetector.CheckAdjacentMatches();

        //            if (mDOWN.IsMatchDown)
        //            {
        //                vertCellMatches.Add(mDOWN.CellMatchDown);
        //            }
        //            else
        //            {
        //                matchDownChainBroken = true;
        //            }

        //            AddAdjacentObstacles(mDOWN, vertObstaclesAdjacent);

        //        }
        //    }
        //    if (vertCellMatches.Count >= 3)
        //    {
        //        // vertical match 3 detected!
        //        matchesCaught.AddRange(vertCellMatches);

        //        Match match = _matchFactory.GetNewMatchBase(m1.ItemType);
        //        match.ItemType = m1.ItemType;
        //        match.NumMatches = vertCellMatches.Count;
        //        match.IsBonusCatch = isDrop;

        //        match.MatchID = CreateMatchID(vertCellMatches);

        //        //OnMatchCaughtDelegate(match);
        //        OnMatchCaughtDelegate(match, _cell);                

        //        obstaclesCaught.AddRange(vertObstaclesAdjacent);

        //        //Debug.Log("VERT MATCH - " + match.ItemType);

        //    }

        //    // HORIZTONTAL matches--------------------------------------------------------------------
        //    List<PlayAreaCell> horzCellMatches = new List<PlayAreaCell>();

        //    List<PlayAreaCell> horzObstaclesAdjacent = new List<PlayAreaCell>();
        //    AddAdjacentObstacles(m1, horzObstaclesAdjacent);

        //    if (m1.IsMatchLeft)
        //    {
        //        horzCellMatches.Add(_cell);
        //        horzCellMatches.Add(m1.CellMatchLeft);

        //        bool matchLeftChainBroken = false;
        //        int colNumLeft = _column.Number;
        //        while (!matchLeftChainBroken)
        //        {
        //            colNumLeft--;
        //            PlayAreaColumn columnLeft = _playArea.GetPlayAreaColumn(colNumLeft);
        //            if (columnLeft != null)
        //            {
        //                PlayAreaCell cellLeftChain = _playArea.GetPlayAreaCell(columnLeft, _cell.Number);
        //                if (cellLeftChain != null)
        //                {
        //                    PlayAreaCellMatches mLEFT = cellLeftChain.MatchDetector.CheckAdjacentMatches();

        //                    if (mLEFT.IsMatchLeft)
        //                    {
        //                        horzCellMatches.Add(mLEFT.CellMatchLeft);
        //                    }
        //                    else
        //                    {
        //                        matchLeftChainBroken = true;
        //                    }

        //                    AddAdjacentObstacles(mLEFT, horzObstaclesAdjacent);

        //                }
        //            }
        //        }
        //    }
        //    if (m1.IsMatchRight)
        //    {
        //        // could have already been placed if THIS is in middle horiztonally
        //        if (!horzCellMatches.Contains(_cell))
        //        {
        //            horzCellMatches.Add(_cell);
        //        }
        //        horzCellMatches.Add(m1.CellMatchRight);

        //        bool matchRightChainBroken = false;
        //        int colNumRight = _column.Number;
        //        while (!matchRightChainBroken)
        //        {
        //            colNumRight++;
        //            PlayAreaColumn columnRight = _playArea.GetPlayAreaColumn(colNumRight);
        //            if (columnRight != null)
        //            {
        //                PlayAreaCell cellRightChain = _playArea.GetPlayAreaCell(columnRight, _cell.Number);
        //                if (cellRightChain != null)
        //                {
        //                    PlayAreaCellMatches mRIGHT = cellRightChain.MatchDetector.CheckAdjacentMatches();

        //                    if (mRIGHT.IsMatchRight)
        //                    {
        //                        horzCellMatches.Add(mRIGHT.CellMatchRight);
        //                    }
        //                    else
        //                    {
        //                        matchRightChainBroken = true;
        //                    }

        //                    AddAdjacentObstacles(mRIGHT, horzObstaclesAdjacent);

        //                }
        //            }
        //        }
        //    }
        //    if (horzCellMatches.Count >= 3)
        //    {
        //        // match could already be here if the cell is part of a vertical AND horiztontal match
        //        for (int i = 0; i < horzCellMatches.Count; i++)
        //        {
        //            if (!matchesCaught.Contains(horzCellMatches[i]))
        //            {
        //                matchesCaught.Add(horzCellMatches[i]);
        //            }
        //        }

        //        Match match = _matchFactory.GetNewMatchBase(m1.ItemType);
        //        match.ItemType = m1.ItemType;
        //        match.NumMatches = horzCellMatches.Count;
        //        match.IsBonusCatch = isDrop;

        //        match.MatchID = CreateMatchID(horzCellMatches);

        //        //OnMatchCaughtDelegate(match);
        //        OnMatchCaughtDelegate(match, _cell);


        //        obstaclesCaught.AddRange(horzObstaclesAdjacent);

        //        //Debug.Log("HORZ MATCH - " + match.ItemType);

        //    }

        //    return (matchesCaught, obstaclesCaught);
        //}
        //public (List<Match> ItemMatchesCaught, List<PlayAreaCell> ObstaclesCaught) CatchMatchThreeNEW(bool isDrop)
        //{
        //    // Look for matches in each cardinal direction.
        //    // if you have a match, "crawl" that direction finding additional matches until chain is broken

        //    List<Match> matchesCaught2 = new List<Match>();

        //    List<PlayAreaCell> matchesCaught = new List<PlayAreaCell>();
        //    List<PlayAreaCell> obstaclesCaught = new List<PlayAreaCell>();

        //    PlayAreaCellMatches m1 = CheckAdjacentMatches();


        //    // VERTICAL matches-------------------------------------------------------------------
        //    List<PlayAreaCell> vertCellMatches = new List<PlayAreaCell>();

        //    List<PlayAreaCell> vertObstaclesAdjacent = new List<PlayAreaCell>();
        //    AddAdjacentObstacles(m1, vertObstaclesAdjacent);

        //    if (m1.IsMatchUp)
        //    {
        //        //vertCellMatches.Add(this);
        //        vertCellMatches.Add(_cell);
        //        vertCellMatches.Add(m1.CellMatchUp);

        //        bool matchUpChainBroken = false;
        //        //int cellNumUp = _number;
        //        int cellNumUp = _cell.Number;
        //        while (!matchUpChainBroken)
        //        {
        //            cellNumUp--;

        //            PlayAreaCell cellUpChain = _playArea.GetPlayAreaCell(_column, cellNumUp);
        //            PlayAreaCellMatches mUP = cellUpChain.MatchDetector.CheckAdjacentMatches();

        //            if (mUP.IsMatchUp)
        //            {
        //                vertCellMatches.Add(mUP.CellMatchUp);
        //            }
        //            else
        //            {
        //                matchUpChainBroken = true;
        //            }

        //            AddAdjacentObstacles(mUP, vertObstaclesAdjacent);

        //        }
        //    }

        //    if (m1.IsMatchDown)
        //    {
        //        // this could have already been added if it matches the item ABOVE (if this is the middle of a match)
        //        // so check before adding it here again for a match BELOW
        //        if (!vertCellMatches.Contains(_cell))
        //        {
        //            vertCellMatches.Add(_cell);
        //        }
        //        vertCellMatches.Add(m1.CellMatchDown);


        //        bool matchDownChainBroken = false;
        //        int cellNumDown = _cell.Number;
        //        while (!matchDownChainBroken)
        //        {
        //            cellNumDown++;
        //            PlayAreaCell cellDownChain = _playArea.GetPlayAreaCell(_column, cellNumDown);
        //            PlayAreaCellMatches mDOWN = cellDownChain.MatchDetector.CheckAdjacentMatches();

        //            if (mDOWN.IsMatchDown)
        //            {
        //                vertCellMatches.Add(mDOWN.CellMatchDown);
        //            }
        //            else
        //            {
        //                matchDownChainBroken = true;
        //            }

        //            AddAdjacentObstacles(mDOWN, vertObstaclesAdjacent);

        //        }
        //    }
        //    if (vertCellMatches.Count >= 3)
        //    {
        //        // vertical match 3 detected!
        //        matchesCaught.AddRange(vertCellMatches);

        //        Match match = _matchFactory.GetNewMatchBase(m1.ItemType);
        //        match.ItemType = m1.ItemType;
        //        match.NumMatches = vertCellMatches.Count;
        //        match.IsBonusCatch = isDrop;

        //        match.MatchID = CreateMatchID(vertCellMatches);
        //        //match.CellsMatched = vertCellMatches;

        //        //OnMatchCaughtDelegate(match);
        //        //OnMatchCaughtDelegate(match, _cell);                
        //        matchesCaught2.Add(match);

        //        obstaclesCaught.AddRange(vertObstaclesAdjacent);

        //        //Debug.Log("VERT MATCH - " + match.ItemType);

        //    }

        //    // HORIZTONTAL matches--------------------------------------------------------------------
        //    List<PlayAreaCell> horzCellMatches = new List<PlayAreaCell>();

        //    List<PlayAreaCell> horzObstaclesAdjacent = new List<PlayAreaCell>();
        //    AddAdjacentObstacles(m1, horzObstaclesAdjacent);

        //    if (m1.IsMatchLeft)
        //    {
        //        horzCellMatches.Add(_cell);
        //        horzCellMatches.Add(m1.CellMatchLeft);

        //        bool matchLeftChainBroken = false;
        //        int colNumLeft = _column.Number;
        //        while (!matchLeftChainBroken)
        //        {
        //            colNumLeft--;
        //            PlayAreaColumn columnLeft = _playArea.GetPlayAreaColumn(colNumLeft);
        //            if (columnLeft != null)
        //            {
        //                PlayAreaCell cellLeftChain = _playArea.GetPlayAreaCell(columnLeft, _cell.Number);
        //                if (cellLeftChain != null)
        //                {
        //                    PlayAreaCellMatches mLEFT = cellLeftChain.MatchDetector.CheckAdjacentMatches();

        //                    if (mLEFT.IsMatchLeft)
        //                    {
        //                        horzCellMatches.Add(mLEFT.CellMatchLeft);
        //                    }
        //                    else
        //                    {
        //                        matchLeftChainBroken = true;
        //                    }

        //                    AddAdjacentObstacles(mLEFT, horzObstaclesAdjacent);

        //                }
        //            }
        //        }
        //    }
        //    if (m1.IsMatchRight)
        //    {
        //        // could have already been placed if THIS is in middle horiztonally
        //        if (!horzCellMatches.Contains(_cell))
        //        {
        //            horzCellMatches.Add(_cell);
        //        }
        //        horzCellMatches.Add(m1.CellMatchRight);

        //        bool matchRightChainBroken = false;
        //        int colNumRight = _column.Number;
        //        while (!matchRightChainBroken)
        //        {
        //            colNumRight++;
        //            PlayAreaColumn columnRight = _playArea.GetPlayAreaColumn(colNumRight);
        //            if (columnRight != null)
        //            {
        //                PlayAreaCell cellRightChain = _playArea.GetPlayAreaCell(columnRight, _cell.Number);
        //                if (cellRightChain != null)
        //                {
        //                    PlayAreaCellMatches mRIGHT = cellRightChain.MatchDetector.CheckAdjacentMatches();

        //                    if (mRIGHT.IsMatchRight)
        //                    {
        //                        horzCellMatches.Add(mRIGHT.CellMatchRight);
        //                    }
        //                    else
        //                    {
        //                        matchRightChainBroken = true;
        //                    }

        //                    AddAdjacentObstacles(mRIGHT, horzObstaclesAdjacent);

        //                }
        //            }
        //        }
        //    }
        //    if (horzCellMatches.Count >= 3)
        //    {
        //        // match could already be here if the cell is part of a vertical AND horiztontal match
        //        for (int i = 0; i < horzCellMatches.Count; i++)
        //        {
        //            if (!matchesCaught.Contains(horzCellMatches[i]))
        //            {
        //                matchesCaught.Add(horzCellMatches[i]);
        //            }
        //        }

        //        Match match = _matchFactory.GetNewMatchBase(m1.ItemType);
        //        match.ItemType = m1.ItemType;
        //        match.NumMatches = horzCellMatches.Count;
        //        match.IsBonusCatch = isDrop;

        //        match.MatchID = CreateMatchID(horzCellMatches);
        //        //match.CellsMatched = horzCellMatches;

        //        //OnMatchCaughtDelegate(match);
        //        //OnMatchCaughtDelegate(match, _cell);
        //        matchesCaught2.Add(match);


        //        obstaclesCaught.AddRange(horzObstaclesAdjacent);

        //        //Debug.Log("HORZ MATCH - " + match.ItemType);

        //    }

        //    //return (matchesCaught, obstaclesCaught);
        //    return (matchesCaught2, obstaclesCaught);
            
        //}


        private List<Match> _matchObjectsCaught = new List<Match>();
        private List<PlayAreaCell> _cellsWithItemsToRemove = new List<PlayAreaCell>();
        private List<PlayAreaCell> _cellsWithObstaclesToRemove = new List<PlayAreaCell>();
        public (List<Match> MatchesCaught, List<PlayAreaCell> CellsWithItemsToRemove, List<PlayAreaCell> CellsWithObstaclesToRemove) CatchMatchThree(bool isDrop)
        {
            // Look for matches in each cardinal direction.
            // if you have a match, "crawl" that direction finding additional matches until chain is broken

            // return 3 lists.
            // 1) MatchesCaught - a list of Match Objects, including MatchID
            // 2) CellsWithItemsToRemove - a list of unique cells with items to remove
            // 3) CellsWithObstaclesToRemove - list of unique cells with obstacles to remove

            _matchObjectsCaught.Clear();
            _cellsWithItemsToRemove.Clear();
            _cellsWithObstaclesToRemove.Clear();

            PlayAreaCellMatches m1 = CheckAdjacentMatches();

            // VERTICAL matches-------------------------------------------------------------------
            List<PlayAreaCell> vertCellMatches = new List<PlayAreaCell>();

            List<PlayAreaCell> vertObstaclesAdjacent = new List<PlayAreaCell>();
            AddAdjacentObstacles(m1, vertObstaclesAdjacent);

            if (m1.IsMatchUp)
            {
                vertCellMatches.Add(_cell);
                vertCellMatches.Add(m1.CellMatchUp);

                bool matchUpChainBroken = false;
                int cellNumUp = _cell.Number;
                while (!matchUpChainBroken)
                {
                    cellNumUp--;

                    PlayAreaCell cellUpChain = _playArea.GetPlayAreaCell(_column, cellNumUp);
                    PlayAreaCellMatches mUP = cellUpChain.MatchDetector.CheckAdjacentMatches();

                    if (mUP.IsMatchUp)
                    {
                        vertCellMatches.Add(mUP.CellMatchUp);
                    }
                    else
                    {
                        matchUpChainBroken = true;
                    }

                    AddAdjacentObstacles(mUP, vertObstaclesAdjacent);

                }
            }

            if (m1.IsMatchDown)
            {
                // this could have already been added if it matches the item ABOVE (if this is the middle of a match)
                // so check before adding it here again for a match BELOW
                if (!vertCellMatches.Contains(_cell))
                {
                    vertCellMatches.Add(_cell);
                }
                vertCellMatches.Add(m1.CellMatchDown);


                bool matchDownChainBroken = false;
                int cellNumDown = _cell.Number;
                while (!matchDownChainBroken)
                {
                    cellNumDown++;
                    PlayAreaCell cellDownChain = _playArea.GetPlayAreaCell(_column, cellNumDown);
                    PlayAreaCellMatches mDOWN = cellDownChain.MatchDetector.CheckAdjacentMatches();

                    if (mDOWN.IsMatchDown)
                    {
                        vertCellMatches.Add(mDOWN.CellMatchDown);
                    }
                    else
                    {
                        matchDownChainBroken = true;
                    }

                    AddAdjacentObstacles(mDOWN, vertObstaclesAdjacent);

                }
            }
            if (vertCellMatches.Count >= 3)
            {
                // vertical match 3 detected!
                _cellsWithItemsToRemove.AddRange(vertCellMatches);

                Match match = _matchFactory.GetNewMatchBase(m1.ItemType);
                match.ItemType = m1.ItemType;
                match.NumMatches = vertCellMatches.Count;
                match.IsBonusCatch = isDrop;
                match.MatchID = CreateMatchID(vertCellMatches);
                _matchObjectsCaught.Add(match);


                _cellsWithObstaclesToRemove.AddRange(vertObstaclesAdjacent);

                //Debug.Log("VERT MATCH - " + match.ItemType);

            }

            // HORIZTONTAL matches--------------------------------------------------------------------
            List<PlayAreaCell> horzCellMatches = new List<PlayAreaCell>();

            List<PlayAreaCell> horzObstaclesAdjacent = new List<PlayAreaCell>();
            AddAdjacentObstacles(m1, horzObstaclesAdjacent);

            if (m1.IsMatchLeft)
            {
                horzCellMatches.Add(_cell);
                horzCellMatches.Add(m1.CellMatchLeft);

                bool matchLeftChainBroken = false;
                int colNumLeft = _column.Number;
                while (!matchLeftChainBroken)
                {
                    colNumLeft--;
                    PlayAreaColumn columnLeft = _playArea.GetPlayAreaColumn(colNumLeft);
                    if (columnLeft != null)
                    {
                        PlayAreaCell cellLeftChain = _playArea.GetPlayAreaCell(columnLeft, _cell.Number);
                        if (cellLeftChain != null)
                        {
                            PlayAreaCellMatches mLEFT = cellLeftChain.MatchDetector.CheckAdjacentMatches();

                            if (mLEFT.IsMatchLeft)
                            {
                                horzCellMatches.Add(mLEFT.CellMatchLeft);
                            }
                            else
                            {
                                matchLeftChainBroken = true;
                            }

                            AddAdjacentObstacles(mLEFT, horzObstaclesAdjacent);

                        }
                    }
                }
            }
            if (m1.IsMatchRight)
            {
                // could have already been placed if THIS is in middle horiztonally
                if (!horzCellMatches.Contains(_cell))
                {
                    horzCellMatches.Add(_cell);
                }
                horzCellMatches.Add(m1.CellMatchRight);

                bool matchRightChainBroken = false;
                int colNumRight = _column.Number;
                while (!matchRightChainBroken)
                {
                    colNumRight++;
                    PlayAreaColumn columnRight = _playArea.GetPlayAreaColumn(colNumRight);
                    if (columnRight != null)
                    {
                        PlayAreaCell cellRightChain = _playArea.GetPlayAreaCell(columnRight, _cell.Number);
                        if (cellRightChain != null)
                        {
                            PlayAreaCellMatches mRIGHT = cellRightChain.MatchDetector.CheckAdjacentMatches();

                            if (mRIGHT.IsMatchRight)
                            {
                                horzCellMatches.Add(mRIGHT.CellMatchRight);
                            }
                            else
                            {
                                matchRightChainBroken = true;
                            }

                            AddAdjacentObstacles(mRIGHT, horzObstaclesAdjacent);

                        }
                    }
                }
            }
            if (horzCellMatches.Count >= 3)
            {
                // match could already be here if the cell is part of a vertical AND horiztontal match
                for (int i = 0; i < horzCellMatches.Count; i++)
                {
                    if (!_cellsWithItemsToRemove.Contains(horzCellMatches[i]))
                    {
                        _cellsWithItemsToRemove.Add(horzCellMatches[i]);
                    }
                }

                Match match = _matchFactory.GetNewMatchBase(m1.ItemType);
                match.ItemType = m1.ItemType;
                match.NumMatches = horzCellMatches.Count;
                match.IsBonusCatch = isDrop;
                match.MatchID = CreateMatchID(horzCellMatches);
                _matchObjectsCaught.Add(match);

                _cellsWithObstaclesToRemove.AddRange(horzObstaclesAdjacent);

                //Debug.Log("HORZ MATCH - " + match.ItemType);

            }

            return (_matchObjectsCaught, _cellsWithItemsToRemove, _cellsWithObstaclesToRemove);
        }



        private string CreateMatchID(List<PlayAreaCell> matches)
        {
            SortedSet<int> sortedDrawIDs = new SortedSet<int>();
            for (int i = 0; i < matches.Count; i++)
            {
                int drawID = (matches[i].StagedItemHandler.GetMatchWithStagedItem()) ? matches[i].StagedItemHandler.GetStagedItem().DrawID 
                                                                                      : drawID = matches[i].ItemHandler.GetItem().DrawID;
                sortedDrawIDs.Add(drawID);
            }

            string matchID = string.Empty;
            foreach (var item in sortedDrawIDs)
            {
                matchID += item + " ";
            }
            matchID = matchID.TrimEnd();

            return matchID;
        }

        internal void OnCellCheckMatchRequest()
        {
            PlayAreaCellMatches m = CheckAdjacentMatches();

            bool isMatch3 = (m.IsMiddleMatchHorz || m.IsMiddleMatchVert) ? true : false;

            OnCellCheckMatchCompleteDelegate(isMatch3);
        }



        private void OnDestroy()
        {
            PlayArea.OnCellCheckMatchRequestDelegate -= OnCellCheckMatchRequest;
        }


        private void Awake()
        {
            PlayArea.OnCellCheckMatchRequestDelegate += OnCellCheckMatchRequest;

            _matchFactory = FindFirstObjectByType<MatchTypeFactory>();

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
