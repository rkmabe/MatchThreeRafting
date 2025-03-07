using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class EffectHandler : MonoBehaviour
    {

        [SerializeField] private Image _effectImage;

        public Image GetImage()
        {
            return _effectImage;
        }

        public void SetImageSprite(Sprite sprite)
        {
            _effectImage.sprite= sprite;
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