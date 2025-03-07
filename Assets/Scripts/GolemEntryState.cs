using MatchThreePrototype.PlayAreaCellContent;
using UnityEngine;

namespace MatchThreePrototype
{

    public class GolemEntryState : IContentState
    {
        public Golem _golem;
        public Animator _anim;

        private static float STATE_DURATION = 3;

        private float _secsInState;

        public void Enter()
        {
            _secsInState = 0;

            _anim.SetBool(GolemAnimatorConstants.IsWalkingID, true);
        }

        public void Exit()
        {
            _anim.SetBool(GolemAnimatorConstants.IsWalkingID, false);
            _golem.IsEntering = false;
        }

        public void Update()
        {

            if (Time.deltaTime == 0)
            {
                return;
            }


            if (_secsInState < STATE_DURATION)
            {
                _secsInState += Time.deltaTime;

                Vector3 posLerp = Vector3.Lerp(_golem.GolemEntryPosition.transform.position, _golem.GolemAttackPosition.transform.position, _secsInState / STATE_DURATION);

                _golem.transform.position = posLerp;
            }
            else
            {
                _golem.StateMachine.TransitionTo(_golem.StateMachine.GolemIdle);
                //_golem.IsCocky = true;
                //_golem.StateMachine.TransitionTo(_golem.StateMachine.GolemCocky);
            }

        }

        public GolemEntryState(Golem golem)
        {
            _golem = golem;
            _anim = golem.Anim;
        }
    }
}