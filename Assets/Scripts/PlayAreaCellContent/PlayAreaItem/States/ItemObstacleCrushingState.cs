using MatchThreePrototype.PlayAreaElements;
using MatchThreePrototype.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{

    public class ItemObstacleCrushingState : IContentState
    {
        private PlayAreaCell _cell;

        private RectTransform _itemRectTransform;

        private RectTransform _obstacleRectTransform;
        private Image _obstacleImage;

        private static float SCALE_CRUSHED_X = 1.3f;
        private static float SCALE_CRUSHED_Y = 0;//.2f;

        private static Vector3 SCALE_DEFAULT = new Vector3(1, 1, 1);
        private static Vector3 SCALE_CRUSHED = new Vector3(SCALE_CRUSHED_X, SCALE_CRUSHED_Y, 1);

        private static Vector2 PIVOT_DEFAULT = new Vector2(.5f, .5f);
        private static Vector2 PIVOT_CRUSHED = new Vector2(.5f, 0);

        private static Vector2 OBSTACLE_ANCHOR_MIN_ABOVE = new Vector2(0, 1);
        private static Vector2 OBSTACLE_ANCHOR_MAX_ABOVE = new Vector2(1, 2);

        private static Vector2 OBSTACLE_ANCHOR_MIN_DEFAULT = new Vector2(0, 0);
        private static Vector2 OBSTACLE_ANCHOR_MAX_DEFAULT = new Vector2(1, 1);

        private static float CRUSH_STEP_SYNC = 5f;//.05f;//5f;
        private float _itemCrushScaleStep = CRUSH_STEP_SYNC;     //5; //15;//25;
        private float _obstacleCrushMoveStep = CRUSH_STEP_SYNC;  //5;
        private float _obstacleAlphaStep = 7;//5;

        private AudioClip _rockHitsRaftClip;

        public void Enter()
        {
            //Debug.Log("enter Obstacle Crushing state");

            //_secsInState = 0;
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, Statics.ALPHA_OFF);
            _obstacleRectTransform.anchorMax = OBSTACLE_ANCHOR_MAX_ABOVE;
            _obstacleRectTransform.anchorMin = OBSTACLE_ANCHOR_MIN_ABOVE;

            //_cell.AudioSourceGeneral.clip= _rockHitsRaftClip;
            //_cell.AudioSourceGeneral.Play();

        }
        public void Exit()
        {
            _cell.ItemHandler.FinishObstacleCrushing();

            _itemRectTransform.pivot = PIVOT_DEFAULT;
            _itemRectTransform.localScale = SCALE_DEFAULT;

            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, Statics.ALPHA_ON);
            _obstacleRectTransform.anchorMin = OBSTACLE_ANCHOR_MIN_DEFAULT;
            _obstacleRectTransform.anchorMax = OBSTACLE_ANCHOR_MAX_DEFAULT;
        }

        public void Update()
        {

            if (Time.deltaTime==0)
            {
                return;
            }

            // covers special case if dynamite is dragged onto a cell while obstacle crushing is in progress.
            if (_cell.ItemHandler.GetIsDynamiteActive())
            {
                _cell.StateMachine.TransitionTo(_cell.StateMachine.DynamiteActive);
                return;
            }
            // alt. implementation ...           
            //if (_cell.ItemHandler.GetIsDynamiteActive())
            //{
            //    _itemRectTransform.pivot = PIVOT_DEFAULT;
            //    _itemRectTransform.localScale = SCALE_DEFAULT;
            //}
            //else
            //{
            //    _itemRectTransform.localScale = Vector3.MoveTowards(_itemRectTransform.localScale, SCALE_CRUSHED, _itemCrushScaleStep);
            //    _itemRectTransform.pivot = Vector3.MoveTowards(_itemRectTransform.pivot, PIVOT_CRUSHED, _itemCrushScaleStep);
            //}

            _itemRectTransform.localScale = Vector3.MoveTowards(_itemRectTransform.localScale, SCALE_CRUSHED, _itemCrushScaleStep);
            _itemRectTransform.pivot = Vector3.MoveTowards(_itemRectTransform.pivot, PIVOT_CRUSHED, _itemCrushScaleStep);

            _obstacleRectTransform.anchorMin = Vector2.MoveTowards(_obstacleRectTransform.anchorMin, OBSTACLE_ANCHOR_MIN_DEFAULT, _obstacleCrushMoveStep);
            _obstacleRectTransform.anchorMax = Vector2.MoveTowards(_obstacleRectTransform.anchorMax, OBSTACLE_ANCHOR_MAX_DEFAULT, _obstacleCrushMoveStep);

            // fade in the image
            float alpha = Mathf.MoveTowards(_obstacleImage.color.a, Statics.ALPHA_ON, _obstacleAlphaStep);
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, alpha);

            if (Statics.IsCloseEnough(_obstacleRectTransform.anchorMin, OBSTACLE_ANCHOR_MIN_DEFAULT, .01f))
            {
                _itemRectTransform.localScale = SCALE_CRUSHED;
                _itemRectTransform.pivot = PIVOT_CRUSHED;

                //_obstacleRectTransform.anchorMin = OBSTACLE_ANCHOR_MIN_DEFAULT;
                //_obstacleRectTransform.anchorMax = OBSTACLE_ANCHOR_MAX_DEFAULT;

                _cell.StateMachine.TransitionTo(_cell.StateMachine.ObstacleIdle);
            }


        }

        public ItemObstacleCrushingState(PlayAreaCell cell, ObstacleCrushingStateResourcesSO obstacleCrushingResources)
        {
            _cell = cell;

            _itemRectTransform = _cell.ItemHandler.GetImage().GetComponent<RectTransform>();

            _obstacleRectTransform = _cell.ObstacleHandler.GetImage().GetComponent<RectTransform>();

            _obstacleImage = _cell.ObstacleHandler.GetImage();

            _itemCrushScaleStep *= Time.deltaTime;
            _obstacleCrushMoveStep *= Time.deltaTime;
            _obstacleAlphaStep *= Time.deltaTime;

            _rockHitsRaftClip = obstacleCrushingResources.RockHitsRaftClip;
        }

    }
}