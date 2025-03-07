using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaElements;
using System.Collections.Generic;
using MatchThreePrototype.MatchReaction.MatchTypes;

namespace MatchThreePrototype.MatchDetection
{
    public interface IPlayAreaCellMatchDetector
    {
        public void Setup(PlayArea playArea, PlayAreaColumn column, PlayAreaCell cell, IItemHandler itemHandler, IStagedItemHandler stagedItemHandler);

        public PlayAreaCellMatches CheckAdjacentMatches();

        //public (List<PlayAreaCell> ItemMatchesCaught, List<PlayAreaCell> ObstaclesCaught) CatchMatchThreeOLD(bool isDrop);

        public (List<Match> MatchesCaught, List<PlayAreaCell> CellsWithItemsToRemove, List<PlayAreaCell> CellsWithObstaclesToRemove) CatchMatchThree(bool isDrop);


    }
}
