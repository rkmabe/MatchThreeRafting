using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock
{

    public interface IBlockHandler
    {
        public void SetBlock(Block block, float imageOpacity);

        public void RemoveBlockLevel();

        public Block GetBlock();

        public Image GetImage();

        public void StartRemoval();
        public bool GetIsProcessingRemoval();
        public void FinishRemoval();


        public void StartBlockAndItemRemoval();
        public bool GetIsProcessingBlockAndItemRemoval();
        public void FinishBlockAndItemRemoval();

        //public void UpdateStateMachine();

    }
}
