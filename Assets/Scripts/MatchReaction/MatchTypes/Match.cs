using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaElements;
using System.Collections.Generic;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{
    public abstract class Match
    {

        public ItemTypes ItemType;
        public int PlayerMoveNum;
        public int NumMatches;
        public bool IsBonusCatch;

        public string MatchID;

        //public List<PlayAreaCell> CellsMatched= new List<PlayAreaCell>();

        public abstract void GamePlayEvent();

        //public abstract void AddInventory(IInventoryManager inventoryManager);

        //public abstract void ScoreMatch(ScoreInfoBlock scoreInfo);

        //public abstract ProjectileTypes GetProjectileType();

    }
}