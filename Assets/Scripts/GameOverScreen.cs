using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MatchThreePrototype
{
    public class GameOverScreen : CollapsibleScreen
    {

        [SerializeField] private Button _exitButton;

        internal void ResetScene()
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        private void OnExitButtonClick()
        {

            //HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            ResetScene();

        }

        private void OnDestroy()
        {
            _exitButton.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
            _exitButton.onClick.AddListener(delegate { OnExitButtonClick(); });
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
