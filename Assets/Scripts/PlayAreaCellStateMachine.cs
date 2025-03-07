using MatchThreePrototype.PlayAreaCellContent;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock.States;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle.States;
using MatchThreePrototype.PlayAreaElements;
using MatchThreePrototype.Scriptables;
using System;
using UnityEngine;

namespace MatchThreePrototype
{

    public class PlayAreaCellStateMachine
    {
        public IContentState CurrentState { get; private set; }

        public event Action<IContentState> ItemStateChanged;

        // EMPTY states
        public PlayAreaCellEmptyState CellEmpty;

        // ITEM states
        public ItemIdleState ItemIdle;
        public ItemRemovingState ItemRemoving;
        public AmmoRemovingState AmmoRemoving;
        public ItemSizeFluctuateState ItemSizeFlucuate;
        public ItemRotationFluctuateState ItemRotationFluctuate;
        public ItemLandingState ItemLanding;

        public DynamiteActiveState DynamiteActive;

        public ItemObstacleCrushingState ObstacleCrushingItem;

        public ItemBlockObscuringState BlockObscuringItem;

        public ItemPlayerHoldingState PlayerHoldingItem;

        // OBSTACLE states
        public ObstacleIdleState ObstacleIdle;
        public ObstacleRemovingState ObstacleRemoving;
        public ObstacleLandingState ObstacleLanding;

        // BLOCK states
        public BlockIdleState BlockIdle;
        public BlockRemovingState BlockRemoving;
        public BlockAndItemRemovingState BlockAndItemRemoving;

        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }

        public void Initialize(IContentState state)
        {
            CurrentState = state;
            state.Enter();

            // notify other objects that state has changed
            ItemStateChanged?.Invoke(state);
        }

        public void TransitionTo(IContentState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // notify other objects that state has changed
            ItemStateChanged?.Invoke(nextState);
        }

        public void CleanUpOnDestroy()
        {
            //IdleState.CleanUp..
            ItemRemoving.CleanUpOnDestroy();
            //AmmoRemoving.CleanUpOnDestroy();
        }

        public PlayAreaCellStateMachine(PlayAreaCell cell, RockBurstPool _burstPool, Vector3 ammoFlyToPosition, DynamiteResourcesSO dynamiteResources, ItemRemovingStateResourcesSO itemRemovingResources, ObstacleCrushingStateResourcesSO obstacleCrushingResources)
        {
            CellEmpty = new PlayAreaCellEmptyState(cell);

            // ITEM states
            ItemIdle = new ItemIdleState(cell);
            ItemRemoving = new ItemRemovingState(cell, itemRemovingResources);
            AmmoRemoving = new AmmoRemovingState(cell, ammoFlyToPosition);
            ItemSizeFlucuate = new ItemSizeFluctuateState(cell);
            ItemRotationFluctuate = new ItemRotationFluctuateState(cell);
            ItemLanding = new ItemLandingState(cell);

            DynamiteActive = new DynamiteActiveState(cell, dynamiteResources);
            ObstacleCrushingItem = new ItemObstacleCrushingState(cell, obstacleCrushingResources);
            BlockObscuringItem = new ItemBlockObscuringState(cell);
            PlayerHoldingItem = new ItemPlayerHoldingState(cell);


            // OBSTACLE states
            ObstacleIdle = new ObstacleIdleState(cell);
            ObstacleRemoving = new ObstacleRemovingState(cell, _burstPool);
            ObstacleLanding = new ObstacleLandingState(cell);

            // BLOCK states
            BlockIdle = new BlockIdleState(cell);
            BlockRemoving = new BlockRemovingState(cell);
            BlockAndItemRemoving = new BlockAndItemRemovingState(cell);
        }

    }
}
