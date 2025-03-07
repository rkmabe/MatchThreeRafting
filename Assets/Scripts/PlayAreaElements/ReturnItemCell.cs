using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class ReturnItemCell : MonoBehaviour
    {

        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;

        public bool IsReturning { get => _isReturning; }
        private bool _isReturning = false;

        private RectTransform _rectTransform;

        private PlayAreaCell _targetCell;

        //internal static float DEFAULT_MOVE_SPEED = 2000;//3000;
        //private float _moveSpeed = DEFAULT_MOVE_SPEED;

        internal static float DEFAULT_MOVE_SPEED = Statics.DEFAULT_MOVE_SPEED;
        private float _moveSpeed = DEFAULT_MOVE_SPEED;

        internal void StartReturning(PlayAreaCell cell)
        {
            _targetCell = cell;

            _isReturning = true;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _itemHandler = GetComponent<IItemHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_isReturning)
            {
                if (Statics.IsCloseEnough(_rectTransform.position, _targetCell.RectTransform.position))
                {
                    _isReturning = false;
                    _targetCell = null;
                    _itemHandler.RemoveItem();
                }
                else
                {
                    _rectTransform.position = Vector2.MoveTowards(_rectTransform.position, _targetCell.RectTransform.position, _moveSpeed * Time.deltaTime);
                }
            }
        }
    }
}
