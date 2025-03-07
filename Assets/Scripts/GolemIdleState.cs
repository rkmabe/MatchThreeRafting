using MatchThreePrototype.PlayAreaCellContent;
using UnityEngine;

namespace MatchThreePrototype
{
    public class GolemIdleState : IContentState
    {
        public Golem _golem;
        public Animator _anim;

        private const float MIN_DURATION = 2;
        private const float MAX_DURATION = 3;

        private float _duration = MIN_DURATION;

        private float _secsInState = 0;


        public void Enter()
        {
            _secsInState = 0;

            _duration = Random.Range(MIN_DURATION, MAX_DURATION);

            _anim.SetBool(GolemAnimatorConstants.IsIdleID, true);
        }

        public void Exit()
        {
            _anim.SetBool(GolemAnimatorConstants.IsIdleID, false);
        }

        public void Update()
        {

            if (_golem.IsDestroyed)
            {
                _golem.StateMachine.TransitionTo(_golem.StateMachine.GolemOffScreen);

                return;
            }

            if (_secsInState < _duration)
            {
                _secsInState += Time.deltaTime;
            }
            else
            {
                _golem.StateMachine.TransitionTo(_golem.StateMachine.GolemAttack);
            }

        }

        public GolemIdleState(Golem golem)
        {
            _golem = golem;
            _anim = golem.Anim;
        }
    }
}