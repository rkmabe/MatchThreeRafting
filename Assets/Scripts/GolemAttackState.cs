using MatchThreePrototype.PlayAreaCellContent;
using UnityEngine;

namespace MatchThreePrototype
{
    public class GolemAttackState : IContentState
    {
        public Golem _golem;
        public Animator _anim;

        public void Enter()
        {
            _golem.StartAttack();
            _anim.SetBool(GolemAnimatorConstants.IsAttackingID, true);

        }

        public void Exit()
        {
            _anim.SetBool(GolemAnimatorConstants.IsAttackingID, false);
        }

        public void Update()
        {
            if (_golem.IsDestroyed)
            {
                _golem.StateMachine.TransitionTo(_golem.StateMachine.GolemOffScreen);

                return;
            }

            if (!_golem.IsAttacking)
            {
                _golem.StateMachine.TransitionTo(_golem.StateMachine.GolemIdle);
            }

        }

        public GolemAttackState(Golem golem)
        {
            _golem = golem;
            _anim = golem.Anim;
        }
    }
}