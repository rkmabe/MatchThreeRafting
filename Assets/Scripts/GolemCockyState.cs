using MatchThreePrototype.PlayAreaCellContent;
using UnityEngine;

namespace MatchThreePrototype
{
    public class GolemCockyState : IContentState
    {
        public Golem _golem;
        public Animator _anim;

        public void Enter()
        {
            _anim.SetBool(GolemAnimatorConstants.IsCockyID, true);
        }

        public void Exit()
        {
            _anim.SetBool(GolemAnimatorConstants.IsCockyID, false);
        }

        public void Update()
        {
            if (!_golem.IsCocky)
            {
                _golem.StateMachine.TransitionTo(_golem.StateMachine.GolemIdle);
            }

        }
        public GolemCockyState(Golem golem)
        {
            _golem = golem;
            _anim = golem.Anim;
        }
    }
}