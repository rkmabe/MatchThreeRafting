using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections.Generic;
using static MatchThreePrototype.PlayAreaElements.PlayArea;

namespace MatchThreePrototype.PlayAreaElements
{
    public interface IDrawnItemsHandler
    {
        public void Setup(List<ItemTypeDrawConfig> itemTypeDrawConfig, ItemPool itemPool);

        public void DrawItems(int numCells);

        public void ShuffleItems();

        public List<Item> GetDrawnItems();

        public Item GetRandomItem();

        public int GetDrawnItemsIndex(List<ItemTypes> excludedItemTypes);

        public void OnDrawnItemReturn(Item item);

    }
}