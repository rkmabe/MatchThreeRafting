using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaElements
{

    public class DropCellVisibility : MonoBehaviour
    {

        [SerializeField] RectTransform _fadeInArea;

        private Vector3 _fadeInTopPosition;
        private Vector3 _fadeInBottomPosition;

        private float _fadeInAreaHeight;

        private DropCell _dropCell;
        private RectTransform _dropCellRect;

        private Vector3[] _dropCellRectCorners = new Vector3[4];

        //Image itemImage = _dropCell.ItemHandler.GetImage();

        private Image _dropCellImage;


        internal float GetOpacityForPosition()
        {
            float opacity = 0f;


            if (_dropCellRect.position.y > _fadeInTopPosition.y)
            {
                opacity = Statics.ALPHA_OFF;
            }
            else if (_dropCellRect.position.y < _fadeInTopPosition.y && _dropCellRect.position.y > _fadeInBottomPosition.y)
            {
                //Debug.Log("in between");
                //Debug.Break();

                float distanceFromTop = _fadeInTopPosition.y - _dropCellRect.position.y;

                opacity = Mathf.Lerp(Statics.ALPHA_OFF, Statics.ALPHA_ON, distanceFromTop / _fadeInAreaHeight);

            }
            else
            {
                opacity = Statics.ALPHA_ON;
            }

            return opacity;
        }

        private void Awake()
        {
            _dropCell = GetComponent<DropCell>();
            _dropCellRect = GetComponent<RectTransform>();



            Vector3[] corners = new Vector3[4];
            _fadeInArea.GetWorldCorners(corners); //Each corner provides its world space value. The returned array of 4 vertices is clockwise. It starts bottom left and rotates to top left, then top right, and finally bottom right.
            _fadeInBottomPosition = corners[0];
            _fadeInTopPosition = corners[1];

            //Debug.Log("TOP=" + _fadeInTopPosition);
            //Debug.Log("BOTTOM=" + _fadeInBottomPosition);

            _fadeInAreaHeight = _fadeInTopPosition.y - _fadeInBottomPosition.y;



        }

        // Start is called before the first frame update
        void Start()
        {
            _dropCellImage = _dropCell.ItemHandler.GetImage();
        }

        // Update is called once per frame
        void Update()
        {

            if (_dropCellImage.sprite != null)
            {

                _dropCellImage.color = new Color(_dropCellImage.color.r, _dropCellImage.color.g, _dropCellImage.color.b, GetOpacityForPosition());                
            }

        }
    }
}
