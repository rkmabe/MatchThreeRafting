using MatchThreePrototype.Controllers;
using MatchThreePrototype.MatchDetection;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using MatchThreePrototype.Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class PlayArea : MonoBehaviour
    {
        //[SerializeField] private RectTransform _playAreaRect;

        //public RectTransform CollisionArea { get => _collisionArea;}
        [SerializeField] private RectTransform _collisionArea;
        [SerializeField] private BoxCollider2D _collisionAreaCollider;

        [SerializeField] private RectTransform _collisionAreaMiddleMount;
        [SerializeField] private BoxCollider2D _collisionAreaMiddleMountCollider;

        [SerializeField] private List<PlayAreaColumn> _columns;

        public float CellAnchorsHeight { get => _cellAnchorsHeight; }
        [Header("Must match in Editor!")]
        [SerializeField] private float _cellAnchorsHeight = 0.11111f;



        //[Header("Must be UNIQUE!")]
        //[SerializeField] private List<ItemTypes> _allowedItemTypes;
        //private List<ItemTypes> _allowedItemTypes;

        //[SerializeField] private List<ItemTypeDrawConfig> _itemTypeDrawConfig;
        private List<ItemTypeDrawConfig> _itemTypeDrawConfig;

        //[SerializeField] private int _chosenNumItemTypes;

        [SerializeField] private List<ItemTypeDrawConfig> DRAW_CONFIG_ITEMTYPE_3;
        [SerializeField] private List<ItemTypeDrawConfig> DRAW_CONFIG_ITEMTYPE_4;
        [SerializeField] private List<ItemTypeDrawConfig> DRAW_CONFIG_ITEMTYPE_5;
        [SerializeField] private List<ItemTypeDrawConfig> DRAW_CONFIG_ITEMTYPE_6;
        [SerializeField] private List<ItemTypeDrawConfig> DRAW_CONFIG_ITEMTYPE_7;

        public MoveItemCell CellMoveToOrigin { get => _cellMoveToOrigin; }
        [SerializeField] private MoveItemCell _cellMoveToOrigin;

        //public MoveItemCell CellMoveToDestination { get => _cellMoveToDestination; }
        //[SerializeField] private MoveItemCell _cellMoveToDestination;

        public SettleItemCell CellSettleIntoDestination { get => _cellSettleIntoDestination; }
        [SerializeField] private SettleItemCell _cellSettleIntoDestination;

        public ReturnItemCell CellReturnToOrigin { get => _cellReturnToOrigin; }
        [SerializeField] private ReturnItemCell _cellReturnToOrigin;

        public HeldItemCell HeldItemCell { get => _heldItemCell; }
        [SerializeField] private HeldItemCell _heldItemCell;

        [SerializeField] bool DebugCheckMatches = false;

        public bool EnableDebugBorderMatcher { get => _enableDebugBorderMatcher; }
        [SerializeField] bool _enableDebugBorderMatcher = false;


        private float _percentCellsToBlock;
        private float _percentCellsToObstruct;

        private int _numItemTypes;

        private ItemPool _itemPool;
        private BlockPool _blockPool;
        private ObstaclePool _obstaclePool;
        private ProjectilePool _projectilePool;
        private RockBurstPool _rockBurstPool;

        private int _numCells;

        public IDrawnItemsHandler DrawnItemsHandler { get => _drawnItemsHandler; }
        private IDrawnItemsHandler _drawnItemsHandler;

        public IPlayAreaPopulator PlayAreaPopulator { get => _playAreaPopulator; }
        private IPlayAreaPopulator _playAreaPopulator;

        public ICellIndicators CellIndicators { get => _cellIndicators; }
        private ICellIndicators _cellIndicators;

        private bool _isPopulated = false;

        private int _numCellsChecked = 0;
        private int _numMatch3s = 0;

        public bool IsInFlux { get => _isInFlux; }
        private bool _isInFlux = false;

        bool _anyMoveActions = false;
        bool _anyColumnDropping = false;

        bool _anyRiverObjectQueued = false;

        bool _anyColumnStateInFlux = false;
        bool _anyColumnProcessingRemoval = false;
        bool _anyColumnProcessingLanding = false;
        bool _anyColumnDynamiteActive = false;
        bool _anyColumnProcessingRiverObject = false;

        bool _anyMatchCaught = false;
        bool _anyColumnsStaged = false;



        public delegate void OnCellCheckMatchRequest();
        public static OnCellCheckMatchRequest OnCellCheckMatchRequestDelegate;

        private SettingsController _settingsController;


        private BoxCollider2D _collider;

        public IPlayAreaHealthManager HealthManager { get => _healthManager; }


        private IPlayAreaHealthManager _healthManager;

        public DynamiteResourcesSO DynamiteResources { get => _dynamiteResourcesMB.DynamiteResources; }
        [SerializeField] private DynamiteResourcesMB _dynamiteResourcesMB;

        public ItemRemovingStateResourcesSO ItemRemovingResources { get => _itemRemovingStateResourcesMB.ItemRemovingStateResources; }
        [SerializeField] private ItemRemovingStateResourcesMB _itemRemovingStateResourcesMB;

        public ObstacleCrushingStateResourcesSO ObstacleCrushingResources { get => _obstacleCrushingStateResourcesMB.ObstacleCrushingStateResources; }
        [SerializeField] private ObstacleCrushingStateResourcesMB _obstacleCrushingStateResourcesMB;        

        public RockBurstPool BurstPool { get => _burstPool; }
        [SerializeField] private RockBurstPool _burstPool;

        public PlayerAmmoMeter PlayerAmmoMeter { get => _playerAmmoMeter; }
        [SerializeField] private PlayerAmmoMeter _playerAmmoMeter;

        public static Action<Item> OnDrawnItemReturn;

        [SerializeField] private PlayerInventory _playerInventory;

        private void CheckAllCellMatches()
        {
            // currently only used in editor tests
            // can be used to cause all cells to check mathes, but
            // usually only cells in new positions must be checked

            _numCellsChecked = 0;
            PlayAreaCellMatchDetector.OnCellCheckMatchCompleteDelegate += OnCellCheckMatchComplete;

            // advertise that you want cells to check
            OnCellCheckMatchRequestDelegate();

        }
        private void OnCellCheckMatchComplete(bool isMatch3)
        {
            // currently only used in editor tests

            _numCellsChecked++;
            if (isMatch3)
            {
                _numMatch3s++;
            }
            if (_numCellsChecked == _numCells)
            {
                //Debug.Log("All cells MATHCED");

                PlayAreaCellMatchDetector.OnCellCheckMatchCompleteDelegate -= OnCellCheckMatchComplete;
            }
        }
        private void Populate()
        {

            //_drawnItemsHandler.Setup(_itemTypeDrawConfig, _itemPool);  

            int numItemTypes = _settingsController.GetNumItemTypesActualValue();

            //switch (_chosenNumItemTypes)
            switch (numItemTypes)
            {
                case 3:
                    _itemTypeDrawConfig = DRAW_CONFIG_ITEMTYPE_3;
                    break;
                case 4:
                    _itemTypeDrawConfig = DRAW_CONFIG_ITEMTYPE_4;
                    break;
                case 5:
                    _itemTypeDrawConfig = DRAW_CONFIG_ITEMTYPE_5;
                    break;
                case 6:
                    _itemTypeDrawConfig = DRAW_CONFIG_ITEMTYPE_6;
                    break;
                case 7:
                    _itemTypeDrawConfig = DRAW_CONFIG_ITEMTYPE_7;
                    break;
                default:
                    _itemTypeDrawConfig = DRAW_CONFIG_ITEMTYPE_5;
                    break;
            }
            _drawnItemsHandler.Setup(_itemTypeDrawConfig, _itemPool);  

            _drawnItemsHandler.DrawItems(_numCells);

            _drawnItemsHandler.ShuffleItems();

            _playerInventory.SetupDisplayForItemTypes(_itemTypeDrawConfig);

            _playAreaPopulator.Setup(_drawnItemsHandler, _obstaclePool, _blockPool);

            _playAreaPopulator.PlaceItems(_columns);

            int numCellsToObstruct = Mathf.RoundToInt(_numCells * _percentCellsToObstruct);
            _playAreaPopulator.PlaceObstacles(numCellsToObstruct);
            
            int numCellsToBlock = Mathf.RoundToInt(_numCells * _percentCellsToBlock);
            _playAreaPopulator.PlaceBlocks(numCellsToBlock);

            _isPopulated = true;
        }


        internal void ReplaceRandomItemWithRock(Sprite rockSprite, Vector3 rockPosition)
        {
            //Debug.Log("rockPosition=" + rockPosition);

            PlayAreaColumn closestColumn = null;
            PlayAreaCell targetCell = null;

            float lowDistance = float.MaxValue;
            for (int i = 0; i < _columns.Count; i++)
            {
                float iDistance = Mathf.Abs(rockPosition.x - _columns[i].transform.position.x);
                if (iDistance < lowDistance)
                {
                    lowDistance = iDistance;
                    closestColumn = _columns[i];
                }
            }

            if (closestColumn)
            {
                //Debug.Log("closestColumn=" + closestColumn.Number);

                for (int i = 0; i < closestColumn.Cells.Count; i++)
                {
                    if (closestColumn.Cells[i].ItemHandler.GetItem())
                    {
                        targetCell = closestColumn.Cells[i];
                        break;
                    }
                }

                if (targetCell)
                {
                    //Debug.Log("ROCK TO CELL" + targetCell.Number);

                    targetCell.ItemHandler.RemoveItem();

                    Obstacle obstacle = _obstaclePool.GetNextAvailable();

                    obstacle.Sprite = rockSprite;
                    targetCell.ObstacleHandler.SetObstacle(obstacle, Statics.ALPHA_ON);

                }
                else
                {
                    //Debug.Log("NO CELL TO DROP ROCK!");
                }
            }

        }


        internal struct QueuedRiverBlockInfo
        {
            public BlockTypes BlockType;
            public Vector3 Position;
        }
        private List<QueuedRiverBlockInfo> _queuedRiverBlocks = new List<QueuedRiverBlockInfo>();
        internal void QueueRiverBlock(BlockTypes blockType, Vector3 blockPosition)
        {
            QueuedRiverBlockInfo queuedRiverBlock = new QueuedRiverBlockInfo();
            queuedRiverBlock.BlockType = blockType;
            queuedRiverBlock.Position = blockPosition;
            _queuedRiverBlocks.Add(queuedRiverBlock);
        }


        List<int> _possibleCellIndices = new List<int>();
        private void ProcessRiverBlock(BlockTypes blockType, Vector3 blockPosition)
        {
            PlayAreaColumn column = GetClosestColumn(blockPosition);


            _possibleCellIndices.Clear();
            PlayAreaCell targetCell = null;
            for (int i = 0; i < column.Cells.Count; i++)
            {
                //if (column.Cells[i].ItemHandler.GetItem() && !column.Cells[i].ItemHandler.GetIsProcessingRemoval()
                //                                          && !column.Cells[i].ItemHandler.GetIsObstacleCrushing())

                //if (!column.Cells[i].BlockHandler.GetBlock() &&
                //    column.Cells[i].ItemHandler.GetItem() &&
                //    !column.Cells[i].ItemHandler.GetIsProcessingRemoval() &&
                //    !column.Cells[i].ItemHandler.GetIsObstacleCrushing())

                if (!column.Cells[i].BlockHandler.GetBlock() &&
                    !column.Cells[i].ObstacleHandler.GetObstacle() &&
                    !column.Cells[i].ItemHandler.GetIsProcessingRemoval() &&
                    !column.Cells[i].ItemHandler.GetIsPlayerHolding() &&
                    column.Cells[i].ItemHandler.GetItem())
                {
                    //targetCell = column.Cells[i];
                    //break;
                    _possibleCellIndices.Add(i);
                }
            }

            if (_possibleCellIndices.Count> 0) 
            {
                if (_possibleCellIndices.Count==1)
                {
                    targetCell = column.Cells[_possibleCellIndices[0]];
                }
                else
                {
                    int index = UnityEngine.Random.Range(0,_possibleCellIndices.Count);
                    targetCell = column.Cells[_possibleCellIndices[index]];
                }
            }

            if (targetCell)
            {
                Block block = _blockPool.GetNextAvailable(blockType);

                targetCell.BlockHandler.SetBlock(block,Statics.ALPHA_OFF);

                targetCell.ItemHandler.StartBlockObscuring();
            }
        }

        internal struct QueuedRiverObstacleInfo
        {
            public Sprite ObstacleSprite;
            public Vector3 Position;
        }
        private List<QueuedRiverObstacleInfo> _queuedRiverObstacles = new List<QueuedRiverObstacleInfo>();
        internal void QueueRiverObstacle(Sprite obstacleSprite, Vector3 obstaclePosition)
        {
            QueuedRiverObstacleInfo queuedRiverObstacle= new QueuedRiverObstacleInfo();
            queuedRiverObstacle.ObstacleSprite = obstacleSprite;
            queuedRiverObstacle.Position = obstaclePosition;
            _queuedRiverObstacles.Add(queuedRiverObstacle);
        }
        private void ProcessRiverObstacle(Sprite obstacleSprite, Vector3 obstaclePosition)
        {
            PlayAreaColumn column = GetClosestColumn(obstaclePosition);

            PlayAreaCell targetCell = null;
            for (int i = 0; i < column.Cells.Count; i++)
            {
                //if (column.Cells[i].ItemHandler.GetItem() && !column.Cells[i].ItemHandler.GetIsProcessingRemoval()
                //                                          && !column.Cells[i].ItemHandler.GetIsObstacleCrushing())

                //if (!column.Cells[i].BlockHandler.GetBlock() &&
                //    column.Cells[i].ItemHandler.GetItem() &&
                //    !column.Cells[i].ItemHandler.GetIsProcessingRemoval() &&
                //    !column.Cells[i].ItemHandler.GetIsObstacleCrushing())

                if (!column.Cells[i].BlockHandler.GetBlock() &&
                    !column.Cells[i].ObstacleHandler.GetObstacle() &&
                    !column.Cells[i].ItemHandler.GetIsProcessingRemoval() &&
                    !column.Cells[i].ItemHandler.GetIsPlayerHolding() &&
                    column.Cells[i].ItemHandler.GetItem())
                {
                    targetCell = column.Cells[i];
                    break;
                }
            }
            if (targetCell)
            {

                Obstacle obstacle = _obstaclePool.GetNextAvailable();
                obstacle.Sprite = obstacleSprite;
                targetCell.ObstacleHandler.SetObstacle(obstacle,Statics.ALPHA_OFF);

                OnDrawnItemReturn?.Invoke(targetCell.ItemHandler.GetItem());

                targetCell.ItemHandler.RemoveItemObject();

                targetCell.ItemHandler.StartObstacleCrushing();
            }
        }

        //internal void QueueRiverRockOLD(Sprite rockSprite, Vector3 rockPosition)
        //{
        //    PlayAreaColumn column = GetClosestColumn(rockPosition);

        //    //column.QueueRiverRock(rockSprite);

        //    PlayAreaCell targetCell = null;
        //    for (int i = 0; i < column.Cells.Count; i++)
        //    {
        //        if (column.Cells[i].ItemHandler.GetItem() && !column.Cells[i].ItemHandler.GetIsProcessingRemoval()
        //                                                  && !column.Cells[i].ItemHandler.GetIsObstacleCrushing())
        //        {
        //            targetCell = column.Cells[i];
        //            break;
        //        }
        //    }
        //    if (targetCell)
        //    {

        //        Obstacle obstacle = _obstaclePool.GetNextAvailable();
        //        obstacle.Sprite = rockSprite;
        //        targetCell.ObstacleHandler.SetObstacle(obstacle,Statics.ALPHA_OFF);

        //        //targetCell.ObstacleHandler.SetImage(rockSprite);
        //        targetCell.ItemHandler.StartObstacleCrushing();
        //    }
        //}

        private PlayAreaColumn GetClosestColumn(Vector3 position)
        {
            PlayAreaColumn closestColumn = null;

            float lowDistance = float.MaxValue;
            for (int i = 0; i < _columns.Count; i++)
            {
                float iDistance = Mathf.Abs(position.x - _columns[i].transform.position.x);
                if (iDistance < lowDistance)
                {
                    lowDistance = iDistance;
                    closestColumn = _columns[i];
                }
            }

            return closestColumn;
        }
        internal List<PlayAreaCell> GetAdjacentCells(PlayAreaCell cell)
        {
            List<PlayAreaCell> cells = new List<PlayAreaCell>();

            PlayAreaColumn thisColumn = GetPlayAreaColumn(cell.ColumnNumber);
            PlayAreaColumn columnLeft = GetPlayAreaColumn(cell.ColumnNumber - 1);
            PlayAreaColumn columnRight = GetPlayAreaColumn(cell.ColumnNumber + 1);

            // cell up
            PlayAreaCell cellUp = GetPlayAreaCell(thisColumn, cell.Number - 1);
            if (cellUp)
            {
                cells.Add(cellUp);
            }

            // cell down
            PlayAreaCell cellDown = GetPlayAreaCell(thisColumn, cell.Number + 1);
            if (cellDown)
            {
                cells.Add(cellDown);
            }

            if (columnRight)
            {
                // cellRight 
                PlayAreaCell cellRight = GetPlayAreaCell(columnRight, cell.Number);
                if (cellRight)
                {
                    cells.Add(cellRight);
                }
                //cellUpRight
                PlayAreaCell cellUpRight = GetPlayAreaCell(columnRight, cell.Number - 1);
                if (cellUpRight)
                {
                    cells.Add(cellUpRight);
                }

                // cellDownRight
                PlayAreaCell cellDownRight = GetPlayAreaCell(columnRight, cell.Number + 1);
                if (cellDownRight)
                {
                    cells.Add(cellDownRight);
                }
            }

            if (columnLeft)
            {
                //cellLeft
                PlayAreaCell cellLeft = GetPlayAreaCell(columnLeft, cell.Number);
                if (cellLeft)
                {
                    cells.Add(cellLeft);
                }

                //cellUpLeft
                PlayAreaCell cellUpLeft = GetPlayAreaCell(columnLeft, cell.Number - 1);
                if (cellUpLeft)
                {
                    cells.Add(cellUpLeft);
                }

                //cellDownLeft
                PlayAreaCell cellDownLeft = GetPlayAreaCell(columnLeft, cell.Number + 1);
                if (cellDownLeft)
                {
                    cells.Add(cellDownLeft);
                }
            }



            return cells;
        }
        internal PlayAreaCell GetPlayAreaCell(PlayAreaColumn column, int cellNum)
        {
            for (int i = 0; i < column.Cells.Count; i++)
            {
                if (column.Cells[i].Number == cellNum)
                {
                    return column.Cells[i];
                }
            }

            return null;
        }
        internal PlayAreaColumn GetPlayAreaColumn(int columnNum)
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                if (_columns[i].Number == columnNum)
                {
                    return _columns[i];
                }
            }

            return null;
        }
        private int GetCellCount()
        {
            int cellCount = 0;
            for (int i = 0; i < _columns.Count; i++)
            {
                cellCount += _columns[i].Cells.Count;
            }

            return cellCount;
        }
        private void SetColliderSize()
        {




            //RectTransform rectTransform = GetComponent<RectTransform>();
            //_collider.size = rectTransform.rect.size;



            //RectTransform rectTransform = GetComponent<RectTransform>();
            //var worldCorners = new Vector3[4];
            //rectTransform.GetWorldCorners(worldCorners);
            //var result = new Rect(
            //              worldCorners[0].x,
            //              worldCorners[0].y,
            //              worldCorners[2].x - worldCorners[0].x,
            //              worldCorners[2].y - worldCorners[0].y);

            //Debug.Log("BREAK");





            _collisionAreaCollider.size = _collisionArea.rect.size;

            _collisionAreaMiddleMountCollider.size = _collisionAreaMiddleMount.rect.size;


        }


        internal bool IsPlayAreaMovePermitted()
        {
            if (_isInFlux == false)
            {
                return true;
            }
            else
            {
                if (_anyColumnProcessingRiverObject &&
                    !_anyColumnProcessingRemoval &&
                    !_anyColumnProcessingLanding &&
                    !_anyColumnDynamiteActive )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void Awake()
        {
            _itemPool = FindFirstObjectByType<ItemPool>();
            _blockPool = FindFirstObjectByType<BlockPool>();
            _obstaclePool = FindFirstObjectByType<ObstaclePool>();
            _projectilePool = FindFirstObjectByType<ProjectilePool>();
            _rockBurstPool = FindFirstObjectByType<RockBurstPool>();

            _settingsController = FindFirstObjectByType<SettingsController>();

            _numCells = GetCellCount();


            _drawnItemsHandler = GetComponent<IDrawnItemsHandler>();
            _playAreaPopulator = GetComponent<IPlayAreaPopulator>();

            _cellIndicators = GetComponent<ICellIndicators>();

            _collider = GetComponent<BoxCollider2D>();

            _healthManager = GetComponent<IPlayAreaHealthManager>();

            //_burstPool = FindFirstObjectByType<RockBurstPool>();
            //_playerAmmoMeter = FindFirstObjectByType<PlayerAmmoMeter>();


        }
        private void OnDestroy()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            SetColliderSize();
        }






        private void Update()
        {

            if (DebugCheckMatches)
            {
                CheckAllCellMatches();
                DebugCheckMatches = false;
            }

            // do not process anything until populated
            // POPULATION PROTOTYPE-------------------------------------------------------
            // In Production, this will be triggered when a level is loaded.
            if (!_isPopulated)
            {
                if (_itemPool.IsInitialized && _blockPool.IsInitialized && _obstaclePool.IsInitialized && _projectilePool.IsInitialized && _rockBurstPool.IsInitialized)
                {
                    _percentCellsToBlock = _settingsController.GetPctBlock();
                    _percentCellsToObstruct = _settingsController.GetPctObstacle();

                    Populate();
                }
                return;
            }

            //// QUEUE ANY RIVER OBJECTS----------------------------------------------
            //// river objects are QUEUED to be animated into the play area.
            //// this does not stop the update.
            //for (int i = _queuedRiverObstacles.Count - 1; i >= 0; i--)
            //{
            //    ProcessRiverObstacle(_queuedRiverObstacles[i].ObstacleSprite, _queuedRiverObstacles[i].Position);
            //    _queuedRiverObstacles.RemoveAt(i);
            //}
            //for (int j = _queuedRiverBlocks.Count - 1; j >= 0; j--)
            //{
            //    ProcessRiverBlock(_queuedRiverBlocks[j].BlockType, _queuedRiverBlocks[j].Position);
            //    _queuedRiverBlocks.RemoveAt(j);
            //}

            // PROCESS  MOVE ACTIONS-------------------------------------------------------
            _anyMoveActions = false;
            if (_cellMoveToOrigin.ItemHandler.GetItem() != null)
            {
                _anyMoveActions = true;
                bool hasSwapArrived;
                _cellMoveToOrigin.UpdatePosition(out hasSwapArrived);
                if (hasSwapArrived)
                {
                    _cellMoveToOrigin.ProcessOnArrival();
                }
            }
            if (_cellSettleIntoDestination.ItemHandler.GetItem() != null)
            {
                _anyMoveActions = true;
                bool isItemSettleComplete;
                _cellSettleIntoDestination.UpdateTransform(out isItemSettleComplete);
                if (isItemSettleComplete)
                {
                    _cellSettleIntoDestination.FinishSettling();
                }
            }
            if (_anyMoveActions)
            {
                _isInFlux = true;
                return;
            }



            // PROCESS DROPS-------------------------------------------------------
            _anyColumnDropping = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnDropping;
                _columns[i].UpdateDroppingCells(out thisColumnDropping);
                if (thisColumnDropping)
                {
                    _anyColumnDropping = true;
                }
            }
            if (_anyColumnDropping)
            {
                _isInFlux = true;
                return;
            }

            // PROCESS STATE MACHINES--------------------------NEW
            _anyColumnStateInFlux = false;
            _anyColumnProcessingRemoval = false;
            _anyColumnProcessingLanding = false;
            _anyColumnDynamiteActive = false;
            _anyColumnProcessingRiverObject = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnStateInFlux = false;
                bool thisColumnProcessingRemoval = false;
                bool thisColumnProcessingLanding = false;
                bool thisColumnDynamiteActive = false;
                bool thisColumnProcessingRiverObject = false;

                _columns[i].PlayAreaUpdateStateMachines(out thisColumnStateInFlux,
                                                        out thisColumnProcessingRemoval,
                                                        out thisColumnProcessingLanding,
                                                        out thisColumnDynamiteActive,
                                                        out thisColumnProcessingRiverObject);
                if (thisColumnStateInFlux)
                {
                    _anyColumnStateInFlux = true;
                }
                if (thisColumnProcessingRemoval)
                {
                    _anyColumnProcessingRemoval = true;
                }
                if (thisColumnProcessingLanding)
                {
                    _anyColumnProcessingLanding = true;
                }
                if (thisColumnDynamiteActive)
                {
                    _anyColumnDynamiteActive = true;
                }
                if (thisColumnProcessingRiverObject)
                {
                    _anyColumnProcessingRiverObject = true;
                }
            }

            if (_anyColumnStateInFlux)
            {
                _isInFlux = true;
                return;
            }

            // QUEUE ANY RIVER OBJECTS----------------------------------------------
            // river objects are QUEUED to be animated into the play area.
            _anyRiverObjectQueued = false;
            for (int i = _queuedRiverObstacles.Count - 1; i >= 0; i--)
            {
                ProcessRiverObstacle(_queuedRiverObstacles[i].ObstacleSprite, _queuedRiverObstacles[i].Position);
                _queuedRiverObstacles.RemoveAt(i);
                _anyRiverObjectQueued = true;
            }
            for (int j = _queuedRiverBlocks.Count - 1; j >= 0; j--)
            {
                ProcessRiverBlock(_queuedRiverBlocks[j].BlockType, _queuedRiverBlocks[j].Position);
                _queuedRiverBlocks.RemoveAt(j);
                _anyRiverObjectQueued = true;
            }
            if (_anyRiverObjectQueued)
            {
                _isInFlux = true;
                return;
            }

            // PROCESS MATCHES------------------------------------------------------
            _anyMatchCaught = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnMatchesCaught;
                _columns[i].UpdateMatches(out thisColumnMatchesCaught);
                if (thisColumnMatchesCaught)
                {
                    _anyMatchCaught = true;
                }
            }
            if (_anyMatchCaught)
            {
                _isInFlux = true;
                return;
            }

            // STAGE NEW DROPS-------------------------------------------------------
            _anyColumnsStaged = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnStaged;
                _columns[i].UpdateStagedDrops(out thisColumnStaged);
                if (thisColumnStaged)
                {
                    _anyColumnsStaged = true;
                }
            }
            if (_anyColumnsStaged)
            {
                _isInFlux = true;
                return;
            }

            // if we make it here, the play area is NOT in flux
            _isInFlux = false;
        }

        //void UpdateOLD()
        //{

        //    if (DebugCheckMatches)
        //    {
        //        CheckAllCellMatches();
        //        DebugCheckMatches = false;
        //    }

        //    // do not process anything until populated
        //    // PROTOTYPE of play area population process.
        //    // In Production, this will be triggered when a level is loaded.
        //    if (!_isPopulated)
        //    {
        //        //if (_itemPool.IsInitialized && _blockPool.IsInitialized && _obstaclePool.IsInitialized && _projectilePool.IsInitialized)
        //            if (_itemPool.IsInitialized && _blockPool.IsInitialized && _obstaclePool.IsInitialized && _projectilePool.IsInitialized && _rockBurstPool.IsInitialized)
        //            {
        //            _percentCellsToBlock = _settingsController.GetPctBlock();
        //            _percentCellsToObstruct = _settingsController.GetPctObstacle();

        //            //_allowedItemTypes = PrototypeBuildItemTypes();
        //            //_isSwapRangeLimited = _settingsController.GetLimitSwapRange();                    

        //            Populate();
        //        }
        //        return;
        //    }

        //    //return;

        //    // PROCESS  MOVE ACTIONS-------------------------------------------------------
        //    bool anyMoveActions = false;
        //    if (_cellMoveToOrigin.ItemHandler.GetItem() != null)
        //    {
        //        anyMoveActions = true;
        //        bool hasSwapArrived;
        //        _cellMoveToOrigin.UpdatePosition(out hasSwapArrived);
        //        if (hasSwapArrived)
        //        {
        //            _cellMoveToOrigin.ProcessOnArrival();
        //        }
        //    }

        //    //if (_cellMoveToDestination.ItemHandler.GetItem() != null)
        //    //{
        //    //    anyMoveActions = true;
        //    //    bool hasSwapArrived;
        //    //    _cellMoveToDestination.UpdatePosition(out hasSwapArrived);
        //    //    if (hasSwapArrived)
        //    //    {
        //    //        _cellMoveToDestination.ProcessOnArrival();
        //    //    }
        //    //}

        //    if (_cellSettleIntoDestination.ItemHandler.GetItem() != null)
        //    {
        //        anyMoveActions = true;
        //        bool isItemSettleComplete;
        //        _cellSettleIntoDestination.UpdateTransform(out isItemSettleComplete);
        //        if (isItemSettleComplete)
        //        {
        //            _cellSettleIntoDestination.FinishSettling();                    
        //        }
        //    }
        //    if (anyMoveActions)
        //    {
        //        _isInFlux = true;
        //        return;
        //    }

        //    // PROCESS RIVER OBSTACLES/BLOCKS----------------------------------------------------
        //    bool anyRiverObjects = false;
        //    for (int i = _queuedRiverObstacles.Count - 1; i >= 0; i--)
        //    {
        //        ProcessRiverObstacle(_queuedRiverObstacles[i].ObstacleSprite, _queuedRiverObstacles[i].Position);
        //        _queuedRiverObstacles.RemoveAt(i);
        //        anyRiverObjects = true;
        //    }
        //    for (int j = _queuedRiverBlocks.Count - 1; j >= 0; j--)
        //    {
        //        ProcessRiverBlock(_queuedRiverBlocks[j].BlockType, _queuedRiverBlocks[j].Position);
        //        _queuedRiverBlocks.RemoveAt(j);
        //        anyRiverObjects= true;
        //    }
        //    if (anyRiverObjects)
        //    {
        //        _isInFlux = true;
        //        return;
        //    }

        //    // PROCESS DROPS-------------------------------------------------------
        //    bool anyColumnDropping = false;
        //    for (int i = 0; i < _columns.Count; i++)
        //    {
        //        bool thisColumnDropping;
        //        _columns[i].UpdateDroppingCells(out thisColumnDropping);
        //        if (thisColumnDropping)
        //        {
        //            anyColumnDropping = true;
        //        }
        //    }
        //    if (anyColumnDropping)
        //    {
        //        _isInFlux = true;
        //        return;
        //    }

        //    // UPDATE STATE MACHINES----------CHECK FOR IN PROCESS REMOVALS--------
        //    //bool anyColumnRemoving = false;
        //    //bool anyColumnLanding = false;
        //    //for (int i = 0; i < _columns.Count; i++)
        //    //{
        //    //    bool thisColumnRemoving;
        //    //    bool thisColumnLanding;
        //    //    _columns[i].UpdateStateMachines(out thisColumnRemoving, out thisColumnLanding);
        //    //    if (thisColumnRemoving)
        //    //    {
        //    //        anyColumnRemoving = true;
        //    //    }
        //    //    if (thisColumnLanding)
        //    //    {
        //    //        anyColumnLanding = true;
        //    //    }
        //    //}
        //    //if (anyColumnRemoving || anyColumnLanding)
        //    //{
        //    //    _isInFlux = true;
        //    //    return;
        //    //}
        //    bool anyColumnInFlux = false;
        //    for (int i = 0; i < _columns.Count; i++)
        //    {
        //        bool isColumnInFlux = false;
        //        _columns[i].PlayAreaUpdateStateMachines(out isColumnInFlux);
        //        if (isColumnInFlux)
        //        {
        //            anyColumnInFlux = true;
        //        }
        //    }
        //    if (anyColumnInFlux)
        //    {
        //        _isInFlux = true;
        //        return;
        //    }

        //    // PROCESS MATCHES------------------------------------------------------
        //    bool anyMatchCaught = false;
        //    for (int i = 0; i < _columns.Count; i++)
        //    {
        //        bool thisColumnMatchesCaught;
        //        _columns[i].UpdateMatches(out thisColumnMatchesCaught);
        //        if (thisColumnMatchesCaught)
        //        {
        //            anyMatchCaught = true;
        //        }
        //    }
        //    if (anyMatchCaught)
        //    {
        //        _isInFlux = true;
        //        return;
        //    }

        //    //// UPDATE STATE MACHINES----------CHECK FOR IN PROCESS REMOVALS--------
        //    //bool anyColumnRemoving = false;
        //    //bool anyColumnLanding = false;
        //    //for (int i = 0; i < _columns.Count; i++)
        //    //{
        //    //    bool thisColumnRemoving;
        //    //    bool thisColumnLanding;
        //    //    _columns[i].UpdateStateMachines(out thisColumnRemoving, out thisColumnLanding);
        //    //    if (thisColumnRemoving)
        //    //    {
        //    //        anyColumnRemoving = true;
        //    //    }
        //    //    if (thisColumnLanding)
        //    //    {
        //    //        anyColumnLanding = true;
        //    //    }
        //    //}
        //    //if (anyColumnRemoving || anyColumnLanding )
        //    //{
        //    //    _isInFlux = true;
        //    //    return;
        //    //}

        //    // STAGE NEW DROPS-------------------------------------------------------
        //    bool anyColumnsStaged = false;
        //    for (int i = 0; i < _columns.Count; i++)
        //    {
        //        bool thisColumnStaged;
        //        _columns[i].UpdateStagedDrops(out thisColumnStaged);
        //        if (thisColumnStaged)
        //        {
        //            anyColumnsStaged = true;
        //        }
        //    }
        //    if (anyColumnsStaged)
        //    {
        //        _isInFlux = true;
        //        return;
        //    }

        //    // if we make it here, the play area is NOT in flux
        //    _isInFlux = false;
        //}

        [Serializable]
        public struct ItemTypeDrawConfig
        {
            public ItemTypes ItemType;
            public float PercentOfDraw;
        }

        struct CellInPlay
        {
            public int index;
            public int column;
            public int row;
        }
    }
}