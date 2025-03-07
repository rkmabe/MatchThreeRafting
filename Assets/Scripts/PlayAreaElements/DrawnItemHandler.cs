using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections.Generic;
using UnityEngine;
using static MatchThreePrototype.PlayAreaElements.PlayArea;

namespace MatchThreePrototype.PlayAreaElements
{

    public class DrawnItemHandler : MonoBehaviour, IDrawnItemsHandler
    {
        private List<Item> _drawnItems = new List<Item>();

        private List<ItemTypes> _allowedItemTypes;
        private List<ItemTypeDrawConfig> _itemTypeDrawConfig;

        private ItemPool _itemPool;

        public void Setup(List<ItemTypeDrawConfig> itemTypeDrawConfig, ItemPool itemPool)
        {
            _itemTypeDrawConfig = itemTypeDrawConfig;
            _itemPool = itemPool;
        }

        //public void DrawItems(int numCells)
        //{
        //    // draw enough items to fully populate play area twice.
        //    // try to draw an equal amount of each type.
        //    // if not an even divide, draw a random type for each remainder.

        //    int drawCount = numCells * 2;

        //    int divTypesPerDrawCount = drawCount / _allowedItemTypes.Count;
        //    for (int i = 0; i < _allowedItemTypes.Count; i++)
        //    {
        //        for (int j = 0; j < divTypesPerDrawCount; j++)
        //        {
        //            ItemTypes itemType = _allowedItemTypes[i];

        //            Item item = _itemPool.GetNextAvailable(itemType);

        //            _drawnItems.Add(item);
        //        }
        //    }

        //    int modTypesPerDrawCount = drawCount % _allowedItemTypes.Count;
        //    for (int i = 0; i < modTypesPerDrawCount; i++)
        //    {
        //        Item item = _itemPool.GetNextAvailable();
        //        _drawnItems.Add(item);
        //    }

        //}

        // each time a DRAWN ITEM is used in the play area it should have a unique DRAW ID.
        private int _drawIDCounter = 0;
        private int GetNewDrawID()
        {
            // method must be called synchonrously!
            if (_drawIDCounter == int.MaxValue)
            {
                _drawIDCounter = 0;
            }

            _drawIDCounter++;

            return _drawIDCounter;
        }

        private List<Item> _itemsToReturnToDrawList = new List<Item>();
        public void OnDrawnItemReturn(Item item)
        {
            //_drawnItems.Add(item);
            _itemsToReturnToDrawList.Add(item);
        }
        private void ReturnItemToDrawList(Item returnItem)
        {
            // method must be called synchonrously! 
            // to ensure integrity of _drawIDCounter

            returnItem.DrawID = GetNewDrawID();

            _drawnItems.Add(returnItem);
        }


        public void DrawItems(int numCells)
        {

            // MAKE SURE that percentages configured in ITEM TYPE DRAW CONFIG are correct!

            _drawnItems.Clear();

            float drawCount = numCells * 2;

            for (int i = 0; i < _itemTypeDrawConfig.Count; i++)
            {
                ItemTypes itemType = _itemTypeDrawConfig[i].ItemType;

                int numItemsWhole = (int)(drawCount * _itemTypeDrawConfig[i].PercentOfDraw);

                for (int j = 0; j < numItemsWhole; j++)
                {
                    Item item = _itemPool.GetNextAvailable(itemType);
                    
                    item.DrawID = GetNewDrawID();

                    _drawnItems.Add(item);
                }

            }

            // add any more items necessary due to ignored fractions above.  1 random per items remaining.
            int numRemaining = (numCells * 2) - _drawnItems.Count;
            for (int k = 0; k < numRemaining; k++)
            {
                int randIndex = Random.Range(0,_itemTypeDrawConfig.Count);
                ItemTypes randItemType = _itemTypeDrawConfig[randIndex].ItemType;

                Item randItem = _itemPool.GetNextAvailable(randItemType);
                _drawnItems.Add(randItem);
            }

        }


        public int GetDrawnItemsIndex(List<ItemTypes> excludedItemTypes)
        {
            if (excludedItemTypes.Count == 0)
            {
                return UnityEngine.Random.Range(0, _drawnItems.Count);
            }
            else
            {
                for (int i = 0; i < _drawnItems.Count; i++)
                {
                    bool isExcluded = false;
                    for (int j = 0; j < excludedItemTypes.Count; j++)
                    {
                        if (_drawnItems[i].ItemType == excludedItemTypes[j])
                        {
                            isExcluded = true;
                            break;
                        }
                    }
                    if (!isExcluded)
                    {
                        return i;
                    }
                }
            }

            // you are never going to get here. .. unless you have too few allowed item types!
            //Debug.LogError("oh yeah?");
            return UnityEngine.Random.Range(0, _drawnItems.Count);
        }

        public List<Item> GetDrawnItems()
        {
            return _drawnItems;
        }
        public Item GetRandomItem()
        {
            // PRODUCTION
            if (_drawnItems.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, _drawnItems.Count);
                Item item = _drawnItems[index];
                _drawnItems.RemoveAt(index);

                return item;
            }

            // FORCE BLUE PINS
            //ONLY DRAW TROUT - stress test loops of matches
            //if (_drawnItems.Count > 0)
            //{
            //    int i = 0;
            //    bool found = false;
            //    while (!found)
            //    {
            //        int index = UnityEngine.Random.Range(0, _drawnItems.Count);
            //        Item item = _drawnItems[index];

            //        if (item.ItemType == ItemTypes.Trout)
            //        {
            //            _drawnItems.RemoveAt(index);
            //            return item;
            //        }

            //        i++;
            //        if (i > 100)
            //        {
            //            break;
            //        }
            //    }
            //}

            Debug.LogError("No more DRAWN items!");
            return null;
        }

        public void ShuffleItems()
        {
            for (int i = _drawnItems.Count - 1; i > 0; i--)
            {
                int k = UnityEngine.Random.Range(0, i + 1);
                Item itemToSwap = _drawnItems[k];
                _drawnItems[k] = _drawnItems[i];
                _drawnItems[i] = itemToSwap;
            }
        }



        private void OnDestroy()
        {
            ItemHandler.OnDrawnItemReturn -= OnDrawnItemReturn;
        }

        private void Awake()
        {
            ItemHandler.OnDrawnItemReturn += OnDrawnItemReturn;

            PlayArea.OnDrawnItemReturn += OnDrawnItemReturn;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_itemsToReturnToDrawList.Count>0)
            {
                ReturnItemToDrawList(_itemsToReturnToDrawList[0]);
                _itemsToReturnToDrawList.RemoveAt(0);
            }
        }
    }
}
