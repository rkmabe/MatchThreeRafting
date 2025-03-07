using MatchThreePrototype.Controllers;
using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle.States
{

    public class ObstacleRemovingState : IContentState
    {

        //private float _secsRemovalProcessing = 0;

        //internal static float IGNORE_SETTINGS_DURATION = .5f;

        //internal static float DEFAULT_REMOVAL_DURATION = .5f;
        //private float _removalDuration = DEFAULT_REMOVAL_DURATION;

        //private ObstacleHandler _obstacleHandler;
        private PlayAreaCell _cell;

        private RockBurstPool _burstPool;
        private RockBurst _burst;

        public override string ToString()
        {
            return this.GetType().Name;
        }

        public void Enter()
        {
            _burst = _burstPool.GetNextAvailable();
            _burst.transform.SetParent(_cell.ObstacleHandler.GetImage().GetComponent<RectTransform>());
            _burst.GetComponent<RectTransform>().anchoredPosition = Statics.Vector3Zero();
            _burst.GetComponent<RectTransform>().offsetMin = Statics.Vector2Zero();
            _burst.GetComponent<RectTransform>().offsetMax = Statics.Vector2Zero();
            _burst.gameObject.SetActive(true);
            _burst.SetupExplosion();
        }

        public void Exit()
        {
            _cell.ObstacleHandler.FinishRemoval();
        }

        public void Update()
        {
            _cell.StateMachine.TransitionTo(_cell.StateMachine.CellEmpty);
        }

        internal void OnNewRemoveDuration(float duration)
        {
            //_removalDuration = duration;
        }

        internal void CleanUpOnDestroy()
        {
            SettingsController.OnNewRemoveDurationDelegate -= OnNewRemoveDuration;
        }

        public ObstacleRemovingState(PlayAreaCell cell, RockBurstPool burstPool)
        {
            _cell = cell;

            _burstPool= burstPool;

            SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;
        }


    }
}