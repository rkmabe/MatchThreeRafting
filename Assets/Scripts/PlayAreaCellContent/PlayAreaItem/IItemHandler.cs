using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{
    public interface IItemHandler
    {
        public Item GetItem();

        public void SetItem(Item item, float imageOpacity);

        public void RemoveItem();

        public void RemoveItemObject();
        public void RemoveItemSprite();

        public Image GetImage();

        public void SetAnimationImage(Sprite sprite);

        // REMOVING STATE
        public void StartRemoval();
        public bool GetIsProcessingRemoval();
        public void FinishRemoval();

        // LANDING STATE
        public void StartLanding();

        public bool GetIsProcessingLanding();

        public void FinishLanding();

        // DYNAMITE ACTIVE STATE
        public void StartDynamiteActive();

        public bool GetIsDynamiteActive();

        public void FinishDynamiteActive();

        // OBSTACLE CRUSHING STATE
        public void StartObstacleCrushing();

        public bool GetIsObstacleCrushing();

        public void FinishObstacleCrushing();


        // BLOCK OBSCURING STATE
        public void StartBlockObscuring();

        public bool GetIsBlockObscuring();

        public void FinishBlockObscuring();

        // PLAYER HOLDING STATE
        public void StartPlayerHolding();

        public bool GetIsPlayerHolding();

        public void FinishPlayerHolding();


    }
}