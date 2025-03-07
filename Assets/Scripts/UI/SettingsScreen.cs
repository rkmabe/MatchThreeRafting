using Lofelt.NiceVibrations;
using MatchThreePrototype.Controllers;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.UI
{

    public class SettingsScreen : MonoBehaviour
    {

        [SerializeField] private ScrollRect _contentScrollRect;

        [Header("Settings Fields - APPLE")]
        [SerializeField] private Toggle _debugTextToggleA;
        [SerializeField] private TMPro.TextMeshProUGUI _moveSpeedTextA;

        [SerializeField] private Slider _moveSpeedTextSliderA;

        [SerializeField] private TMPro.TextMeshProUGUI _dropSpeedTextA;

        [SerializeField] private Slider _dropSpeedTextSliderA;

        [SerializeField] private TMPro.TextMeshProUGUI _removeDurationTextA;

        [SerializeField] private Slider _removeDurationSliderA;

        [SerializeField] private Slider _numBlocksSliderA;
        [SerializeField] private Slider _numObstaclesSliderA;

        [SerializeField] private Slider _freqBlockSliderA;
        [SerializeField] private Slider _freqObstacleSliderA;

        [SerializeField] private Slider _waypointDistanceSliderA;

        [SerializeField] private Toggle _limitSwapRangeToggleA;

        [SerializeField] private Slider _golemHealthSliderA;
        [SerializeField] private Slider _golemRocksSliderA;


        [SerializeField] private TMPro.TMP_Dropdown _numItemTypesDropdownA;  //inactive
        [SerializeField] private Toggle _showScoreInfoToggleA;

        [SerializeField] private TMPro.TMP_Dropdown _playAreaDropdownA;     //inactive
        [SerializeField] private Toggle _touchToPauseA;


        [SerializeField] private TMPro.TextMeshProUGUI _versionTextA;
        [SerializeField] private Button _privacyPolicyLinkButtonA;
        [SerializeField] private Button _supportLinkButtonA;
        [SerializeField] private RectTransform _contentA;

        // TODO: Settings for GOOGLE is incomplete/untested!!!  must catch it up with apple changes!
        [Header("Settings Fields - GOOGLE")]
        [SerializeField] private Toggle _debugTextToggleG;
        [SerializeField] private TMPro.TextMeshProUGUI _moveSpeedTextG;

        [SerializeField] private Slider _moveSpeedTextSliderG;

        [SerializeField] private TMPro.TextMeshProUGUI _dropSpeedTextG;

        [SerializeField] private Slider _dropSpeedTextSliderG;

        [SerializeField] private TMPro.TextMeshProUGUI _removeDurationTextG;

        [SerializeField] private Slider _removeDurationSliderG;

        [SerializeField] private Slider _numBlocksSliderG;
        [SerializeField] private Slider _numObstaclesSliderG;

        [SerializeField] private Slider _freqBlocksSliderG;
        [SerializeField] private Slider _freqObstaclesSliderG;

        [SerializeField] private Slider _waypointDistanceSliderG;

        [SerializeField] private Toggle _limitSwapRangeToggleG;

        [SerializeField] private Slider _golemHealthSliderG;
        [SerializeField] private Slider _golemRocksSliderG;

        [SerializeField] private TMPro.TMP_Dropdown _numItemTypesDropdownG;    //inactive
        [SerializeField] private Toggle _showScoreInfoToggleG;


        [SerializeField] private TMPro.TMP_Dropdown _playAreaDropdownG;        //inactive
        [SerializeField] private Toggle _touchToPauseG;

        [SerializeField] private TMPro.TextMeshProUGUI _versionTextG;
        [SerializeField] private Button _privacyPolicyLinkButtonG;
        [SerializeField] private Button _supportLinkButtonG;
        [SerializeField] private RectTransform _contentG;

        // ONLY these variables should be used by the class (other than the assignment method)
        private Toggle _debugTextToggle;
        private TMPro.TextMeshProUGUI _moveSpeedText;

        private Slider _moveSpeedSlider;

        private TMPro.TextMeshProUGUI _dropSpeedText;

        private Slider _dropSpeedSlider;

        private TMPro.TextMeshProUGUI _removeDurationText;

        private Slider _removeDurationSlider;

        private Slider _numBlocksSlider;
        private Slider _numObstaclesSlider;

        private Slider _freqBlocksSlider;
        private Slider _freqObstaclesSlider;

        private Slider _waypointDistanceSlider;

        private Toggle _limitSwapRangeToggle;

        private TMPro.TMP_Dropdown _numItemTypesDropdown;       
        
        private Toggle _showScoreInfoToggle;

        private Slider _golemHealthSlider;
        private Slider _golemRocksSlider;

        private TMPro.TMP_Dropdown _playAreaDropdown;                   //inactive

        private Toggle _touchToPause;

        private TMPro.TextMeshProUGUI _versionText;
        private Button _privacyPolicyLinkButton;
        private Button _supportLinkButton;


        public delegate void OnSettingsScreenCloseComplete();
        public static OnSettingsScreenCloseComplete settingsScreenCloseCompleteDelegate;


        private SettingsController _settingsController;

        private void SettingsScreenCloseComplete()
        {
            settingsScreenCloseCompleteDelegate();
        }

        private void AssignScreenFieldsForPlatform()
        {
#if (UNITY_IOS)
            _contentA.gameObject.SetActive(true);
            _contentG.gameObject.SetActive(false);

            _contentScrollRect.content = _contentA;
            _debugTextToggle = _debugTextToggleA;
            _moveSpeedText = _moveSpeedTextA;

            _moveSpeedSlider = _moveSpeedTextSliderA;

            _dropSpeedText = _dropSpeedTextA;

            _dropSpeedSlider = _dropSpeedTextSliderA;

            _removeDurationText = _removeDurationTextA;

            _removeDurationSlider = _removeDurationSliderA;

            _numBlocksSlider = _numBlocksSliderA;
            _numObstaclesSlider = _numObstaclesSliderA;

            _freqBlocksSlider= _freqBlockSliderA;
            _freqObstaclesSlider= _freqObstacleSliderA;

            _waypointDistanceSlider= _waypointDistanceSliderA;

            _golemHealthSlider= _golemHealthSliderA;
            _golemRocksSlider= _golemRocksSliderA;

            _limitSwapRangeToggle= _limitSwapRangeToggleA;         //inactive
            _showScoreInfoToggle= _showScoreInfoToggleA;

            _numItemTypesDropdown = _numItemTypesDropdownA;

            _playAreaDropdown = _playAreaDropdownA;               //inactive
            _touchToPause = _touchToPauseA;
 

            _versionText = _versionTextA;
            _privacyPolicyLinkButton = _privacyPolicyLinkButtonA;
            _supportLinkButton = _supportLinkButtonA;
#elif (UNITY_ANDROID)
            _contentA.gameObject.SetActive(false);
            _contentG.gameObject.SetActive(true);

            _contentScrollRect.content = _contentG;
            _debugText = _debugTextToggleG;
            _moveSpeedText = _moveSpeedTextG;
            _moveSpeedSlider = _moveSpeedTextSliderG;
            _dropSpeedText = _dropSpeedTextG;
            _dropSpeedSlider = _dropSpeedTextSliderG;
            _removeDurationText = _removeDurationTextG;
            _removeDurationSlider = _removeDurationSliderG;
            _numBlocksSlider = _numBlocksSliderG;
            _numObstaclesSlider = _numObstaclesSliderG;
            _freqBlocksSlider= _freqBlocksSliderG;
            _freqObstaclesSlider= _freqObstaclesSliderG;
            _waypointDistanceSlider= _waypointDistanceSliderG;
            _golemHealthSlider= _golemHealthSliderG;
            _golemRocksSlider= _golemRocksSliderG;
            _limitSwapRangeToggle = _limitSwapRangeToggleG;           //inactive
            _showScoreInfoToggle = _showScoreInfoToggleG;
            _numItemTypesDropdown = _numItemTypesDropdownG;
            _playAreaDropdown = _playAreaDropdownG;                  //inactive
            _touchToPause = _touchToPauseG;
            _versionText= _versionTextG;
            _privacyPolicyLinkButton= _privacyPolicyLinkButtonG;
            _supportLinkButton= _supportLinkButtonG;
#endif
        }

        private static string VERSION = "Version: ";
        private StringBuilder _versionStringBuilder = new StringBuilder();
        private void SetVersionText()
        {
            _versionStringBuilder.Append(VERSION);
            _versionStringBuilder.Append(Application.version);
            _versionText.text = _versionStringBuilder.ToString();
        }

        private void OnDebugTextToggleValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetDebugTextOn(_debugTextToggle.isOn);
        }

        private void OnMoveSpeedSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetMoveSpeed(_moveSpeedSlider.value);
        }

        private void OnDropSpeedSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetDropSpeed(_dropSpeedSlider.value);
        }

        private void OnRemoveDurationSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            float removeDuration = Statics.Interpolate(_removeDurationSlider.value, 0, 1, .05f,2.5f);

            _settingsController.SetRemoveDuration(removeDuration);
        }

        private void OnNumBlocksSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetPctBlock(_numBlocksSlider.value);
        }

        private void OnNumObstaclesSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetPctObstacle(_numObstaclesSlider.value);
        }

        private void OnFreqBlocksSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetFreqBlock(_freqBlocksSlider.value);

        }

        private void OnFreqObstaclesSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetFreqObstacle(_freqObstaclesSlider.value);
        }

        private void OnWaypointDistanceSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetWaypointDistance(_waypointDistanceSlider.value);
        }

        private void OnGolemHealthSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetGolemHealth((int)_golemHealthSlider.value);
        }
        private void OnGolemRocksSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetGolemRocks((int)_golemRocksSlider.value);
        }

        private void OnLimitSwapRangeToggleValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetLimitSwapRange(_limitSwapRangeToggle.isOn);
        }

        private void OnNumItemTypesDropdownValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetNumItemTypes(_numItemTypesDropdown.value);

        }

        private void OnPlayAreaDropdownValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetPlayAreaSelection(_playAreaDropdown.value);
        }

        private void OnShowScoreInfoToggleValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetShowScoreInfo(_showScoreInfoToggle.isOn);
        }
        private void OnTouchToPauseValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetTouchToPause(_touchToPause.isOn);
        }


        private void SetDefaultNumBlocks()
        {
            _numBlocksSlider.value = _settingsController.GetPctBlock();
        }

        private void SetDefaultNumObstacles()
        {
            _numObstaclesSlider.value = _settingsController.GetPctObstacle();
        }

        private void SetDefaultFreqBlocks()
        {
            _freqBlocksSlider.value = _settingsController.GetFreqBlock();
        }
        private void SetDefaultFreqObstacles()
        {
            _freqObstaclesSlider.value = _settingsController.GetFreqObstacle();
        }

        private void SetDefaultWaypointDistance()
        {
            _waypointDistanceSlider.value = _settingsController.GetWaypointDistance();
        }

        private void SetDefaultGolemHealth()
        {
            _golemHealthSlider.value = _settingsController.GetGolemHealth();
        }

        private void SetDefaultGolemRocks()
        {
            _golemRocksSlider.value = _settingsController.GetGolemRocks();
        }

        private void SetDefaultLimitSwapRange()
        {
            _limitSwapRangeToggle.isOn = _settingsController.GetLimitSwapRange();
        }
        private void SetDefaultNumItemTypes()
        {
            _numItemTypesDropdown.value = _settingsController.GetNumItemTypes();            
        }

        private void SetDefaultShowCellIDs()
        {
            _debugTextToggle.isOn = _settingsController.GetDebugTextOn();

        }
        private void SetDefaultMoveSpeed()
        {
            _moveSpeedSlider.value = _settingsController.GetMoveSpeed();
        }
        private void SetDefaultDropSpeed()
        {
            _dropSpeedSlider.value = _settingsController.GetDropSpeed();
        }
        private void SetDefaultRemoveSpeed()
        {
            float sliderValue = Statics.Interpolate(_settingsController.GetRemoveDuration(), .05f, 2.5f, 0, 1);
            _removeDurationSlider.value = sliderValue;
        }

        private void SetDefaultPlayArea()
        {
            _playAreaDropdown.value = _settingsController.GetPlayAreaSelection();
        }

        private void SetDefaultShowScoreInfo()
        {
            _showScoreInfoToggle.isOn = _settingsController.GetShowScoreInfo();
        }

        private void SetDefaultTouchToPause()
        {
            _touchToPause.isOn = _settingsController.GetTouchToPause();
        }

        internal void SetDefaults()
        {
            SetDefaultNumBlocks();
            SetDefaultNumObstacles();

            SetDefaultFreqBlocks();
            SetDefaultFreqObstacles();

            SetDefaultWaypointDistance();

            SetDefaultGolemHealth();
            SetDefaultGolemRocks();

            SetDefaultLimitSwapRange();
            SetDefaultNumItemTypes();
            SetDefaultShowCellIDs();
            SetDefaultMoveSpeed();
            SetDefaultDropSpeed();
            SetDefaultRemoveSpeed();
            SetDefaultPlayArea();

            SetDefaultShowScoreInfo();
            SetDefaultTouchToPause();
        }

        private void Awake()
        {
            _settingsController = FindObjectOfType<SettingsController>();

            AssignScreenFieldsForPlatform();  // must come before .AddListeners for screen fields!

            SetVersionText();

            _debugTextToggle.onValueChanged.AddListener(delegate { OnDebugTextToggleValueChanged(); });

            _moveSpeedSlider.onValueChanged.AddListener(delegate { OnMoveSpeedSliderValueChanged(); });

            _dropSpeedSlider.onValueChanged.AddListener(delegate { OnDropSpeedSliderValueChanged(); });

            _removeDurationSlider.onValueChanged.AddListener(delegate { OnRemoveDurationSliderValueChanged(); });

            _numBlocksSlider.onValueChanged.AddListener(delegate { OnNumBlocksSliderValueChanged(); });

            _numObstaclesSlider.onValueChanged.AddListener(delegate { OnNumObstaclesSliderValueChanged(); });

            _freqBlocksSlider.onValueChanged.AddListener(delegate { OnFreqBlocksSliderValueChanged(); });

            _freqObstaclesSlider.onValueChanged.AddListener(delegate { OnFreqObstaclesSliderValueChanged(); });

            _waypointDistanceSlider.onValueChanged.AddListener(delegate { OnWaypointDistanceSliderValueChanged(); });

            _golemHealthSlider.onValueChanged.AddListener(delegate { OnGolemHealthSliderValueChanged(); });

            _golemRocksSlider.onValueChanged.AddListener(delegate { OnGolemRocksSliderValueChanged(); });

            _limitSwapRangeToggle.onValueChanged.AddListener(delegate { OnLimitSwapRangeToggleValueChanged(); });

            _numItemTypesDropdown.onValueChanged.AddListener(delegate { OnNumItemTypesDropdownValueChanged(); }); // inactive

            _showScoreInfoToggle.onValueChanged.AddListener(delegate { OnShowScoreInfoToggleValueChanged(); });

            _playAreaDropdown.onValueChanged.AddListener(delegate { OnPlayAreaDropdownValueChanged(); });        // inactive

            _touchToPause.onValueChanged.AddListener(delegate { OnTouchToPauseValueChanged(); });

            SetDefaults();
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
}
