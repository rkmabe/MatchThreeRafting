using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype
{

    public class Raft : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _raftSprite;


        [SerializeField] AnimationCurve _shudderCurve;

        public RectTransform RectTransform { get => _rectTransform; set => _rectTransform = value; }
        private RectTransform _rectTransform;

        public AnimationCurve ShudderCurve { get => _shudderCurve; set => _shudderCurve = value; }

        public RaftStateMachine StateMachine { get => _stateMachine; }
        private RaftStateMachine _stateMachine;

        public bool IsReactingToDamage { get => _isReactingToDamage; set => _isReactingToDamage = value; }
        private bool _isReactingToDamage;


        public void OnPlayAreaDamaged()
        {
            //_stateMachine.TransitionTo(_stateMachine.RaftShudder);

            _isReactingToDamage = true;
        }

        private void OnDestroy()
        {
            PlayAreaHealthManager.OnPlayAreaDamaged -= OnPlayAreaDamaged;
        }

        private void Awake()
        {
            PlayAreaHealthManager.OnPlayAreaDamaged += OnPlayAreaDamaged;

            _stateMachine = new RaftStateMachine(this);
            _stateMachine.Initialize(_stateMachine.RaftIdle);

            _rectTransform = GetComponent<RectTransform>();

        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            _stateMachine.Update();
        }
    }
}
