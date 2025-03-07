using MatchThreePrototype.MatchReaction.MatchTypes;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype.MatchReaction
{
    public class MatchTypeFactory : MonoBehaviour
    {
        public Match GetNewMatchBase(ItemTypes type)
        {
            switch (type)
            {
                case ItemTypes.CannonBall: return new MatchCannonBall();

                case ItemTypes.Catfish: return new MatchCatfish();
                case ItemTypes.Lobster: return new MatchLobster();
                case ItemTypes.Trout: return new MatchTrout();
                case ItemTypes.Turtle: return new MatchTurtle();
                case ItemTypes.Snake: return new MatchSnake();

                case ItemTypes.LifeVest: return new MatchLifeVest();
                case ItemTypes.Paddle: return new MatchPaddle();
                

                case ItemTypes.LifePreserver: return new MatchLifePreserver();

                case ItemTypes.Fish: return new MatchFish();
                case ItemTypes.Crab: return new MatchCrab();

                case ItemTypes.Dynamite: return new MatchDynamite();

                case ItemTypes.CannonBallStack: return new MatchCannonBallStack();

                default: return null;
            }
        }

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

        }
    }
}
