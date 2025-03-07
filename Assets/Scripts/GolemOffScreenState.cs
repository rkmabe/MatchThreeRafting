using MatchThreePrototype.PlayAreaCellContent;
using UnityEngine;

namespace MatchThreePrototype
{

    public class GolemOffScreenState : IContentState
    {
        private Golem _golem;
        private Animator _anim;

        public void Enter()
        {
            //Debug.Log("Golem ENTER OffScreen state");

            //_anim.enabled= false;
            //_golem.gameObject.SetActive(false);
        }

        public void Exit()
        {
            //Debug.Log("Golem EXIT OffScreen state");
            _golem.gameObject.SetActive(true);
            _anim.enabled = true;
        }

        public void Update()
        {
            if (_golem.IsEntering)
            {
                _golem.StateMachine.TransitionTo(_golem.StateMachine.GolemEntry);
                return;
            }

        }

        public GolemOffScreenState(Golem golem)
        {
            _golem = golem;
            _anim = golem.Anim;
        }
    }
}
