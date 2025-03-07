using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public class ObstacleHandler : MonoBehaviour, IObstacleHandler
    {

        [SerializeField] private Image _obstacleImage;

        private Obstacle _obstacle;

        private bool _isProcessingRemoval;

        private bool _isProcessingLanding;

        public static Action<Obstacle> OnPooledObstacleReturn;

        public void RemoveObstacle()
        {
            _obstacle = null;
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, Statics.ALPHA_OFF);
            _obstacleImage.sprite = null;
        }
        public void SetObstacle(Obstacle obstacle, float imageOpacity)
        {
            _obstacle = obstacle;
            //_obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, Statics.ALPHA_ON);
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, imageOpacity);
            _obstacleImage.sprite = obstacle.Sprite;
        }

        public Obstacle GetObstacle()
        {
            return _obstacle;
        }

        public Image GetImage()
        {
            return _obstacleImage;
        }

        //public void SetImage(Sprite sprite)
        //{
        //    _obstacleImage.sprite = sprite;
        //}

        public bool CanDrop()
        {
            return _obstacle.CanDrop;
        }
        public void StartLanding()
        {
            _isProcessingLanding = true;
        }
        public bool GetIsProcessingLanding()
        {
            return _isProcessingLanding;
        }
        public void FinishLanding()
        {
            _isProcessingLanding = false;
        }

        public void StartRemoval()
        {
            _isProcessingRemoval = true;
        }
        public bool GetIsProcessingRemoval()
        {
            return _isProcessingRemoval;
        }
        public void FinishRemoval()
        {
            _isProcessingRemoval = false;

            OnPooledObstacleReturn?.Invoke(_obstacle);

            RemoveObstacle();

        }

        //public void UpdateStateMachine()
        //{
        //    _stateMachine.Update();
        //}

        private void Awake()
        {
            //_stateMachine = new ObstacleStateMachine(this);
            //_stateMachine.Initialize(_stateMachine.IdleState);
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