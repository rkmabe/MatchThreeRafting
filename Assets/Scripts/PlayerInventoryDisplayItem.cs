using UnityEngine;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;

namespace MatchThreePrototype.MatchReaction
{
    public class PlayerInventoryDisplayItem : MonoBehaviour
    {

        //[SerializeField] private ItemTypes _itemType;

        [SerializeField] private TMPro.TextMeshProUGUI _countTextMesh;

        private int _targetNum;
        private int _displayNum;

        private float SECS_BETWEEN_UPDATES = .10f;
        private static float MAX_TRANSITION_DURATION = 5;

        private float _incrementAmount;

        private float _secsSinceLastUpdate = 0;


        internal void UpdateTargetNum(int targetNum)
        {
            _targetNum = targetNum;

            float diff = Mathf.Abs(_targetNum - _displayNum);

            if (diff > 0)
            {
                if ((diff * SECS_BETWEEN_UPDATES) > MAX_TRANSITION_DURATION)
                {
                    _incrementAmount = Mathf.Clamp(diff / MAX_TRANSITION_DURATION, 1, diff);
                }
                else 
                {
                    _incrementAmount = 1;
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (_displayNum != _targetNum)
            {
                _secsSinceLastUpdate += Time.deltaTime;

                if (_secsSinceLastUpdate > SECS_BETWEEN_UPDATES)
                {
                    _secsSinceLastUpdate = 0;

                    _displayNum = (int)Mathf.Clamp(_displayNum + _incrementAmount, _displayNum, _targetNum);

                    _countTextMesh.text = _displayNum.ToString();
                }
            }
        }
    }
}
