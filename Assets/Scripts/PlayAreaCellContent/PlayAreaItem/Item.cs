using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{
    public class Item : MonoBehaviour
    {

        public ItemTypes ItemType { get => _type; }
        [SerializeField] private ItemTypes _type;

        public Sprite Sprite { get => _sprite; }
        [SerializeField] private Sprite _sprite;

        public int DrawID { get => _drawID; set => _drawID = value; }
        private int _drawID = 0;

        //public Sprite SpriteHeld { get => _spriteHeld; }
        //[SerializeField] private Sprite _spriteHeld;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



    }

    public enum ItemTypes
    {
        None = 0,

        CannonBall = 1,

        Catfish = 2,
        Lobster = 3,
        Trout = 4,
        Turtle = 5,
        Snake = 6,

        LifeVest = 7,
        Paddle = 8,
        LifePreserver = 9,

        Fish = 10,
        Crab = 11,

        Dynamite = 12,

        CannonBallStack = 13,

    }


}
