using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public class Obstacle : MonoBehaviour
    {

        public bool CanDrop { get => _canDrop; }
        [SerializeField] private bool _canDrop = false;

        public Sprite Sprite { get => _sprite; set => _sprite = value; }
        private Sprite _sprite;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



    }
    //public enum ObstacleTypes
    //{
    //    None = 0,

    //    AerialRock = 1,

    //    //RedWall = 1,
    //    //GreyWall = 2,
    //    //BlackWall = 3,
    //    //GreenWall = 4,
    //    //PinkWall = 5,
    //    //PurpleWall = 6,
    //    //WhiteWall = 7,
    //    //BlueWall = 8,

    //    Rock1 = 9,
    //    Rock2 = 10,
    //}
}