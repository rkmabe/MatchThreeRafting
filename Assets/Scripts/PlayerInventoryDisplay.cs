using System;
using UnityEngine;


namespace MatchThreePrototype.MatchReaction
{

    public class PlayerInventoryDisplay : MonoBehaviour
    {
        // TODO: consider making these generic .. so that any item type can be used .. 

        //[SerializeField] private PlayerInventoryDisplayItem _cannonballDisplay;
        [SerializeField] private PlayerInventoryDisplayItem _coinsDisplay;
        [SerializeField] private PlayerInventoryDisplayItem _fishDisplay;
        [SerializeField] private PlayerInventoryDisplayItem _troutDisplay;
        //[SerializeField] private PlayerInventoryDisplayItem _catfishDisplay;
        [SerializeField] private PlayerInventoryDisplayItem _crabDisplay;
        [SerializeField] private PlayerInventoryDisplayItem _lobsterDisplay;
        [SerializeField] private PlayerInventoryDisplayItem _turtleDisplay;
        [SerializeField] private PlayerInventoryDisplayItem _junkDisplay;

        [SerializeField] private PlayerInventoryDisplayItem _paddleDisplay;
        [SerializeField] private PlayerInventoryDisplayItem _lifeVestDisplay;

        [SerializeField] private PlayerInventoryDisplayItem _lifePreserverDisplay;


        //[SerializeField] private TMPro.TextMeshProUGUI _numCannonballsText;
        //[SerializeField] private TMPro.TextMeshProUGUI _numCoinsText;
        //[SerializeField] private TMPro.TextMeshProUGUI _numFishText;
        //[SerializeField] private TMPro.TextMeshProUGUI _numCrabText;
        //[SerializeField] private TMPro.TextMeshProUGUI _numJunkText;

        //private int _targetNumCannonballs;
        //private int _targetNumCoins;
        //private int _targetNumFish;
        //private int _targetNumCrab;
        //private int _targetNumJunk;

        //private int _displayedNumCannonballs;
        //private int _displayedNumCoins;
        //private int _displayedNumFish;
        //private int _displayedNumCrab;
        //private int _displayedNumJunk;



        //internal void UpdateNumCannonballs(int numCannonballs)
        //{
        //    _cannonballDisplay.UpdateTargetNum(numCannonballs);
        //}

        internal void UpdateNumCoins(int numCoins)
        {
            _coinsDisplay.UpdateTargetNum(numCoins);
        }

        //internal void UpdateNumFish(int numFish)
        //{
        //    _fishDisplay.UpdateTargetNum(numFish);
        //}

        internal void DisplayTroutNum(bool isDisplayed)
        {
            _troutDisplay.gameObject.SetActive(isDisplayed);
        }
        internal void UpdateNumTrout(int numTrout)
        {
            _troutDisplay.UpdateTargetNum(numTrout);
        }

        //internal void UpdateNumCatfish(int numCatfish)
        //{
        //    _catfishDisplay.UpdateTargetNum(numCatfish);
        //}

        internal void UpdateNumCrab(int numCrab)
        {
            _crabDisplay.UpdateTargetNum(numCrab);
        }

        internal void DisplayLobsterNum(bool isDisplayed)
        {
            _lobsterDisplay.gameObject.SetActive(isDisplayed);
        }
        internal void UpdateNumLobster(int numLobster)
        {
            _lobsterDisplay.UpdateTargetNum(numLobster);
        }

        internal void DisplayTurtleNum(bool isDisplayed)
        {
            _turtleDisplay.gameObject.SetActive(isDisplayed);
        }
        internal void UpdateNumTurtle(int numTurtle)
        {
            _turtleDisplay.UpdateTargetNum(numTurtle);
        }

        //internal void UpdateNumJunk(int numJunk)
        //{
        //    _junkDisplay.UpdateTargetNum(numJunk);
        //}

        internal void DisplayPaddleNum(bool isDisplayed)
        {
            _paddleDisplay.gameObject.SetActive(isDisplayed);

        }
        internal void UpdateNumPaddle(int numPaddle)
        {
            _paddleDisplay.UpdateTargetNum(numPaddle);
        }

        internal void DisplayLifeVestNum(bool isDisplayed)
        {
            _lifeVestDisplay.gameObject.SetActive(isDisplayed);
        }
        internal void UpdateNumLifeVest(int numLifeVest)
        {
            _lifeVestDisplay.UpdateTargetNum(numLifeVest);
        }

        internal void DisplayLifePreserverNum(bool isDispalyed)
        {
            _lifePreserverDisplay.gameObject.SetActive(isDispalyed);
        }
        internal void UpdateNumLifePreserver(int numLifePreserver)
        {
            _lifePreserverDisplay.UpdateTargetNum(numLifePreserver);
        }

        internal void DisplaySnakeNum(bool isDisplayed)
        {
            //_snakeDisplay.gameObject.SetActive(isDisplayed);
        }
        //internal void UpdateSnakeNum()
        // nobody wants to collect freaking snakes

        //private void UpdateInventoryDisplay( int displayCount, int targetCount, TMPro.TextMeshProUGUI guiText)
        //{
        //    if (displayCount != targetCount)
        //    {
        //        if (displayCount < targetCount)
        //        {
        //            displayCount++;
        //        }
        //        else if (displayCount > targetCount)
        //        {
        //            displayCount--;
        //        }
        //        guiText.text = displayCount.ToString();
        //    }

        //}


        private void Awake()
        {


        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            //UpdateInventoryDisplay(out _displayedNumCannonballs, _targetNumCannonballs, _numCannonballsText);
            //UpdateInventoryDisplay(_displayedNumCoins, _targetNumCoins, _numCoinsText);
            //UpdateInventoryDisplay(_displayedNumFish, _targetNumFish, _numFishText);
            //UpdateInventoryDisplay(_displayedNumCrab, _targetNumCrab, _numCrabText);
            //UpdateInventoryDisplay(_displayedNumJunk, _targetNumJunk, _numJunkText);

            //if (_displayedNumCannonballs != _targetNumCannonballs)
            //{
            //    if (_displayedNumCannonballs < _targetNumCannonballs)
            //    {
            //        _displayedNumCannonballs++;
            //    }
            //    else if (_displayedNumCannonballs > _targetNumCannonballs)
            //    {
            //        _displayedNumCannonballs--;
            //    }
            //    _numCannonballsText.text = _displayedNumCannonballs.ToString();
            //}
        }


    }
}