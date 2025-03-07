using MatchThreePrototype.MatchReaction.MatchTypes;
using MatchThreePrototype.PlayAreaElements;
using MatchThreePrototype.PlayerTouchInput;
using MatchThreePrototype.UI;
using UnityEngine;

namespace MatchThreePrototype.MatchReaction
{
    public class MatchScoring : MonoBehaviour
    {

        private Player _player;
        private ScoreInfo _scoreInfo;


        //private void OnMatchCaught(Match match)
        private void OnMatchCaught(Match match, PlayAreaCell cell)
        {
            MoveRecord rec;
            rec.PlayerMoveNum = _player.MoveNum;
            rec.match = match;

            _scoreInfo.UpdateMoveText(rec);

            //match.ScoreMatch(_scoreInfo);            

        }

        private void OnDestroy()
        {
            //PlayAreaCellMatchDetector.OnMatchCaughtDelegate -= OnMatchCaught;

            PlayAreaColumn.OnMatchRemovedDelegate -= OnMatchCaught;
            MoveItemCell.OnMatchRemovedDelegate -= OnMatchCaught;
            SettleItemCell.OnMatchRemovedDelegate -= OnMatchCaught;
        }

        private void Awake()
        {
            //PlayAreaCellMatchDetector.OnMatchCaughtDelegate += OnMatchCaught;

            PlayAreaColumn.OnMatchRemovedDelegate += OnMatchCaught;
            MoveItemCell.OnMatchRemovedDelegate += OnMatchCaught;
            SettleItemCell.OnMatchRemovedDelegate += OnMatchCaught;

            _player = FindFirstObjectByType<Player>();
            _scoreInfo = FindFirstObjectByType<ScoreInfo>();

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

    struct MoveRecord
    {
        public int PlayerMoveNum;
        public Match match;
    }

}
