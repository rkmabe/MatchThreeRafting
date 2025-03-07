using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public interface IObstacleHandler
    {
        public void SetObstacle(Obstacle obstacle, float imageOpacity);

        public void RemoveObstacle();

        public Obstacle GetObstacle();

        public Image GetImage();

        public bool CanDrop();

        // LANDING STATE
        public void StartLanding();

        public bool GetIsProcessingLanding();

        public void FinishLanding();


        public void StartRemoval();
        public bool GetIsProcessingRemoval();
        public void FinishRemoval();



        //public void UpdateStateMachine();

    }
}
