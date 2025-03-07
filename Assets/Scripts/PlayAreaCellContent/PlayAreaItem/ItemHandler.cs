using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{

    public class ItemHandler : MonoBehaviour, IItemHandler
    {
        [SerializeField] private Image _itemImage;


        private Item _item;

        public static Action<Item> OnDrawnItemReturn;

        public static Action<ItemTypes> OnInventoryItemAddition;

        public void SetItem(Item item, float imageOpacity)
        {
            _item = item;
            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, imageOpacity);
            _itemImage.sprite = item.Sprite;
        }
        public Item GetItem()
        {
            return _item;
        }

        public void RemoveItem()
        {
            _item = null;
            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_OFF);
            _itemImage.sprite = null;
        }
        public void RemoveItemObject()
        {
            _item = null;
        }
        public void RemoveItemSprite()
        {
            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_OFF);
            _itemImage.sprite = null;
        }

        public Image GetImage()
        {
            return _itemImage;           
        }

        public void SetAnimationImage(Sprite sprite)
        {
            _itemImage.sprite = sprite;
        }

        // REMOVING STATE
        private bool _isProcessingRemoval;
        public void StartRemoval()
        {
            _isProcessingRemoval = true;
        }
        public bool GetIsProcessingRemoval()
        {
            return _isProcessingRemoval;
        }
        public void FinishRemoval()
        {
            _isProcessingRemoval = false;

            OnDrawnItemReturn?.Invoke(_item);

            OnInventoryItemAddition?.Invoke(_item.ItemType);

            RemoveItem();
        }

        // LANDING STATE
        private bool _isProcessingLanding;
        public void StartLanding()
        {
            //Debug.Log("ITEM just landed!");
            //_hasJustLanded = true;
            _isProcessingLanding = true;
        }
        public bool GetIsProcessingLanding()
        {
            return _isProcessingLanding;
        }
        public void FinishLanding()
        {
            _isProcessingLanding = false;
        }

        // DYNAMITE ACTIVE STATE
        private bool _isDynamiteActive;
        public void StartDynamiteActive()
        {
            _isDynamiteActive = true;
        }
        public bool GetIsDynamiteActive()
        {
            return _isDynamiteActive;
        }
        public void FinishDynamiteActive()
        {
            _isDynamiteActive = false;

            OnDrawnItemReturn?.Invoke(_item);

            RemoveItem();
        }


        // OBSTACLE CRUSHING STATE
        private bool _isObstacleCrushing;
        public void StartObstacleCrushing()
        {
            _isObstacleCrushing = true;
        }
        public bool GetIsObstacleCrushing()
        {
            return _isObstacleCrushing;
        }
        public void FinishObstacleCrushing()
        {
            _isObstacleCrushing = false;

            ////OnDrawnItemReturn?.Invoke(_item);
            ////RemoveItem();

            //RemoveItemSprite();

            // Do not remove item sprite if the type is dynamite!
            // when an obstcle crushes an item, it removes the item object immediately
            // but leaves the item sprite for animation.
            // here, we need to remove the sprite IN MOST CASES.
            // EDGE CASE: player drags dynamite to an obstacle CURRENTLY being crushed.
            // here, the item object will NOT be null and we must NOT remove the sprite.            
            if (_item == null || _item.ItemType != ItemTypes.Dynamite)
            {
                RemoveItemSprite();
            }

        }

        // BLOCK OBSCURING STATE
        private bool _isBlockObscuring;
        public void StartBlockObscuring()
        {
            _isBlockObscuring = true;
        }
        public bool GetIsBlockObscuring()
        {
            return _isBlockObscuring;
        }
        public void FinishBlockObscuring()
        {
            _isBlockObscuring = false;
        }

        // PLAYER HOLDING STATE
        private bool _isPlayerHolding = false;
        public void StartPlayerHolding()
        {
            _isPlayerHolding = true;
        }
        public bool GetIsPlayerHolding()
        {
            return _isPlayerHolding;
        }
        public void FinishPlayerHolding()
        {
            _isPlayerHolding = false;
        }

        //public void UpdateStateMachine()
        //{
        //    _stateMachine.Update();
        //}

        private void OnDestroy()
        {
            //_stateMachine.CleanUpOnDestroy();
        }

        private void Awake()
        {
            //_stateMachine = new ItemStateMachine(this);
            //_stateMachine.Initialize(_stateMachine.IdleState);
        }


    }
}