using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype
{

    public class GameOverListener : MonoBehaviour
    {

        [SerializeField] GameOverScreen _gameOverScreen;

        private void OnPlayAreaDestroyed()
        {
            _gameOverScreen.gameObject.SetActive(true);
            _gameOverScreen.Open();
        }

        private void OnDestroy()
        {
            PlayAreaHealthManager.OnPlayAreaDestroyed -= OnPlayAreaDestroyed;
        }

        private void Awake()
        {
            PlayAreaHealthManager.OnPlayAreaDestroyed += OnPlayAreaDestroyed;
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