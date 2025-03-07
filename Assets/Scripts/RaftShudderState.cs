using MatchThreePrototype.PlayAreaCellContent;
using UnityEngine;

namespace MatchThreePrototype
{

    public class RaftShudderState : IContentState
    {

        private static float STATE_DURATION = .25f;   //.25f;
        private float _secsInState = 0;

        private Raft _raft;

        private Vector3 _startRectTransformPosition;

        private static float MIN_MAGNITUDE = 0;
        private static float MAX_MAGNITUDE = 20;

        public void Enter()
        {
            _secsInState = 0;

            _startRectTransformPosition = _raft.RectTransform.position;
        }

        public void Exit()
        {
            _raft.RectTransform.position = _startRectTransformPosition;
        }

        public void Update()
        {

            if (Time.deltaTime == 0)
            {
                return;
            }

            _secsInState += Time.unscaledDeltaTime;  // unscaled time because game over pauses time.  this is a damaging state which will lead to game over.

            float curveMagnitude = _raft.ShudderCurve.Evaluate(_secsInState / STATE_DURATION);

            float actualMagnitude = Mathf.Lerp(MIN_MAGNITUDE,MAX_MAGNITUDE, curveMagnitude);
                       
            //float testMagnitude = Mathf.Lerp(MIN_MAGNITUDE, MAX_MAGNITUDE, .1f);
            //Assert.AreEqual(testMagnitude, 10);

            _raft.RectTransform.position = _startRectTransformPosition + Random.insideUnitSphere * actualMagnitude;

            if (_secsInState >= STATE_DURATION) // if shudder animation is complete
            {
                _raft.IsReactingToDamage = false;
                _raft.StateMachine.TransitionTo(_raft.StateMachine.RaftIdle);
            }
        }

        public RaftShudderState(Raft raft)
        {
            _raft = raft;
        }

    }
}

