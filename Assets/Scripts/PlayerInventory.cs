using MatchThreePrototype.MatchReaction;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using static MatchThreePrototype.PlayAreaElements.PlayArea;
using System.Collections.Generic;
using UnityEngine;


namespace MatchThreePrototype
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private PlayerInventoryDisplay _inventoryDisplay;
        [SerializeField] private PlayerAmmoMeter _ammoMeter;

        private static int MAX_CANNONBALLS = 50;
        private static float F_MAX_CANNONBALLS = (float)MAX_CANNONBALLS;

        public int NumCannonballs { get => _numCannonballs; }
        private int _numCannonballs;

        public int NumCatfish { get => _numCatfish; }
        private int _numCatfish;

        public int NumLobster { get => _numLobster; }
        private int _numLobster;

        public int NumTrout { get => _numTrout; }
        private int _numTrout;

        public int NumTurtle { get => _numTurtle; }
        private int _numTurtle;

        public int NumSnake { get => _numSnake; }
        private int _numSnake;

        public int NumLifeVest { get => _numLifeVest; }
        private int _numLifeVest;

        public int NumPaddle { get => _numPaddle; }
        private int _numPaddle;

        public int NumLifePreserver { get => _numLifePreserver; }
        private int _numLifePreserver;

        public int NumFish { get => _numFish; }
        private int _numFish;

        public int NumCrab { get => _numCrab; }
        private int _numCrab;


        public int NumCoins { get => _numCoins; }
        private int _numCoins;


        public int NumJunk { get => _numJunk; }
        private int _numJunk;

        private void OnInventoryItemAddition(ItemTypes itemType)
        {
            switch (itemType)
            {
                case ItemTypes.None:
                    //Debug.LogWarning("should not be removing NONE ItemType");
                    break;
                case ItemTypes.CannonBall:
                    AdjustNumCannonballs(1);
                    break;
                case ItemTypes.Catfish:
                    AdjustNumCatfish(1);
                    break;
                case ItemTypes.Lobster:
                    AdjustNumLobster(1);
                    break;
                case ItemTypes.Trout:
                    AdjustNumTrout(1);
                    break;
                case ItemTypes.Turtle:
                    AdjustNumTurtle(1);
                    break;
                case ItemTypes.Snake:
                    AdjustNumSnake(1);
                    break;
                case ItemTypes.LifeVest:
                    AdjustNumLifeVest(1);
                    break;
                case ItemTypes.Paddle:
                    AdjustNumPaddle(1);
                    break;
                case ItemTypes.LifePreserver:
                    AdjustNumLifePreserver(1);
                    break;
                case ItemTypes.Fish:
                    AdjustNumFish(1);
                    break;
                case ItemTypes.Crab:
                    AdjustNumCrab(1);
                    break;
                case ItemTypes.Dynamite:
                    // dynamite is not collected
                    break;
                case ItemTypes.CannonBallStack:
                    AdjustNumCannonballs(3);     //5
                    break;
            }
        }

        public void AdjustNumCannonballs(int numCannonballs)
        {
            _numCannonballs = Mathf.Clamp(_numCannonballs + numCannonballs, 0, MAX_CANNONBALLS);

            float fillPercent = (float)_numCannonballs / F_MAX_CANNONBALLS;

            if (_ammoMeter != null)
            {
                _ammoMeter.AdjustFillPercent(fillPercent);
            }
        }

        public void AdjustNumCatfish(int numCatfish)
        {
            _numCatfish += numCatfish;
            //if (_display != null)
            //{
            //    _display.UpdateNumCatfish(_numCatfish);
            //}
        }
        public void AdjustNumLobster(int numLobster)
        {
            _numLobster += numLobster;
            //if (_inventoryDisplay != null)
            //{
            //    _inventoryDisplay.UpdateNumLobster(_numLobster);
            //}
        }
        public void AdjustNumTrout(int numTrout)
        {
            _numTrout += numTrout;
            if (_inventoryDisplay != null)
            {
                _inventoryDisplay.UpdateNumTrout(_numTrout);
            }
        }
        public void AdjustNumTurtle(int numTurtle)
        {
            _numTurtle += numTurtle;

            if (_inventoryDisplay) { _inventoryDisplay.UpdateNumTurtle(_numTurtle); }
        }
        public void AdjustNumSnake(int numSnake)
        {
            _numSnake += numSnake;
            //if (_display != null)
            //{
            //    _display.updatenumsn;
            //}
        }
        public void AdjustNumLifeVest(int numLifeVest)
        {
            _numLifeVest += numLifeVest;

            if (_inventoryDisplay) { _inventoryDisplay.UpdateNumLifeVest(_numLifeVest); }
        }
        public void AdjustNumPaddle(int numPaddle)
        {
            _numPaddle += numPaddle;

            if (_inventoryDisplay) { _inventoryDisplay.UpdateNumPaddle(_numPaddle); }
        }
        public void AdjustNumLifePreserver(int numLifePreserver)
        {
            _numLifePreserver += numLifePreserver;

            if (_inventoryDisplay) { _inventoryDisplay.UpdateNumLifePreserver(_numLifePreserver); }
        }
        public void AdjustNumFish(int numFish)
        {
            _numFish += numFish;
        }
        public void AdjustNumCrab(int numCrab)
        {
            _numCrab += numCrab;
        }

        public void AdjustNumJunk(int numJunk)
        {
            _numJunk += numJunk;
        }

        public void AdjustNumCoins(int numCoins)
        {
            _numCoins += numCoins;
            if (_inventoryDisplay != null)
            {
                _inventoryDisplay.UpdateNumCoins(_numCoins);
            }
        }


        internal void SetupDisplayForItemTypes(List<ItemTypeDrawConfig> itemTypeDrawConfig)
        {

            if (_inventoryDisplay == null)
            {
                return;
            }

            for (int i = 0; i < itemTypeDrawConfig.Count; i++)
            {
                // enable the PlayerInventoryDisplayItem associated with this item type

                switch (itemTypeDrawConfig[i].ItemType)
                {
                    //case ItemTypes.Catfish:
                    //    _inventoryDisplay.ShowNumCatfish();
                    //    break;
                    case ItemTypes.Lobster:
                            _inventoryDisplay.DisplayLobsterNum(true);
                        break;
                    case ItemTypes.Trout:
                        _inventoryDisplay.DisplayTroutNum(true);
                        break;
                    case ItemTypes.Turtle:
                        _inventoryDisplay.DisplayTurtleNum(true);
                        break;
                    case ItemTypes.Snake:
                        _inventoryDisplay.DisplaySnakeNum(true);
                        break;
                    case ItemTypes.LifeVest:
                        _inventoryDisplay.DisplayLifeVestNum(true);
                        break;
                    case ItemTypes.Paddle:
                        _inventoryDisplay.DisplayPaddleNum(true);
                        break;
                    case ItemTypes.LifePreserver:
                        _inventoryDisplay.DisplayLifePreserverNum(true);
                        break;
                    //case ItemTypes.Fish:
                    //    break;
                    //case ItemTypes.Crab:
                    //    break;
                }



            }
        }


        private void OnDestroy()
        {
            ItemHandler.OnInventoryItemAddition -= OnInventoryItemAddition;
        }

        private void Awake()
        {
            ItemHandler.OnInventoryItemAddition += OnInventoryItemAddition;
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