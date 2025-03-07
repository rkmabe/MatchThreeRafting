using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{
    public class WaveEndScreen : CollapsibleScreen
    {

        [SerializeField] private Button _continueButton;

        //private IInventoryManager _playerInventory;

        [SerializeField] private PlayerInventory _playerInventory;

        public static event Action OnWaveEndScreenClose;

        private void OnContinueButtonClick()
        {

            //HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            //ResetScene();

            OnWaveEndScreenClose();

            Close();

        }

        private void CashInventory()
        {
            int numCatfish = _playerInventory.NumCatfish;
            int numLobster = _playerInventory.NumLobster;
            int numTrout = _playerInventory.NumTrout;
            int numTurtle = _playerInventory.NumTurtle;
            int numSnake = _playerInventory.NumSnake;
            int numLifeVest = _playerInventory.NumLifeVest;
            int numPaddle = _playerInventory.NumPaddle;
            int numLifePreserver = _playerInventory.NumLifePreserver;            

            //int numFish = _playerInventory.NumFish;
            //int numCrab = _playerInventory.NumCrab;
            //int numJunk = _playerInventory.NumJunk;

            _playerInventory.AdjustNumCatfish(numCatfish * -1);
            _playerInventory.AdjustNumLobster(numLobster * -1);
            _playerInventory.AdjustNumTrout(numTrout * -1);
            _playerInventory.AdjustNumTurtle(numTurtle * -1);
            _playerInventory.AdjustNumSnake(numSnake * -1);
            _playerInventory.AdjustNumLifeVest(numLifeVest * -1);
            _playerInventory.AdjustNumPaddle(numPaddle * -1);
            _playerInventory.AdjustNumLifePreserver(numLifePreserver * -1);

            int numCoins = _playerInventory.NumCoins;
            int coinsFromCreatures = 10 * (numCatfish + numLobster + numTrout + numTurtle + numSnake);
            int coinsFromJunk = 5 * (numLifeVest + numPaddle + numLifePreserver);
            int waveCoins = coinsFromCreatures + coinsFromJunk;
            _playerInventory.AdjustNumCoins(numCoins + waveCoins);

        }

        //private void CashInventory()
        //{
        //    //int numnFish = _playerInventory.GetNumFish();
        //    //int numCrab = _playerInventory.GetNumCrab();
        //    //int numJunk = _playerInventory.GetNumJunk();
        //    //int numCoins = _playerInventory.GetNumCoins();

        //    int coinsFromFish = numnFish * 10;
        //    int coinsFromCrab = numCrab * 5;
        //    int coinsFromJunk = numJunk * 1;

        //    int waveCoins = coinsFromFish + coinsFromCrab + coinsFromJunk;

        //    _playerInventory.AdjustNumFish(numnFish * -1);
        //    _playerInventory.AdjustNumCrab(numCrab * -1);
        //    _playerInventory.AdjustNumJunk(numJunk * -1);

        //    _playerInventory.AdjustNumCoins(numCoins + waveCoins);
        //}

        private void OnEnable()
        {
            CashInventory();
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
            _continueButton.onClick.AddListener(delegate { OnContinueButtonClick(); });

            //_playerInventory = FindFirstObjectByType<PlayerInventoryOLD>();
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
