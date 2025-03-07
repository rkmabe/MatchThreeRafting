using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class HeldItemCell : MonoBehaviour 
    {
        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;

        private RectTransform _itemRectTransform;

        private static Vector3 SCALE_DEFUALT = Statics.Vector3One();
        private static Vector3 SCALE_HELD = Statics.SCALE_HELD; //  new Vector3(1.45f, 1.45f, 1.45f);

        private bool _isLiftingToHeld;

        private float _liftStep = Statics.LIFT_LOWER_STEP;

        internal void StartLiftToHeld()
        {
            _itemRectTransform.localScale = SCALE_DEFUALT;
            _isLiftingToHeld = true;
        }

        private void Awake()
        {
            _itemHandler = GetComponent<IItemHandler>();

            _itemRectTransform = GetComponent<RectTransform>();

            _liftStep *= Time.deltaTime;
        }

        void Start()
        {

        }

        void Update()
        {
            if (_isLiftingToHeld)
            {
                _itemRectTransform.localScale = Vector2.MoveTowards(_itemRectTransform.localScale, SCALE_HELD, _liftStep);

                if (Statics.IsCloseEnough(_itemRectTransform.localScale, SCALE_HELD, .01f))
                {
                    _itemRectTransform.localScale = SCALE_HELD;

                    _isLiftingToHeld = false;
                }
            }
        }


    }
}
