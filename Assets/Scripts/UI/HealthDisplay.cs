using MatchThreePrototype.PlayAreaElements;
using System.Text;
using UnityEngine;

namespace MatchThreePrototype.UI
{

    public class HealthDisplay : MonoBehaviour
    {

        [SerializeField] private TMPro.TextMeshProUGUI _healthLabel;
        [SerializeField] private TMPro.TextMeshProUGUI _healthValue;


        StringBuilder _healthValueSB = new StringBuilder();
        private void OnPlayAreaHealthChanged(float percentIntact)
        {
            //_healthValue.text = _healthValue + "%";
           
            _healthValueSB.Clear();
            _healthValueSB.Append(percentIntact);
            _healthValueSB.Append(Statics.PERCENT);

            _healthValue.text = _healthValueSB.ToString();

        }

        private void OnDestroy()
        {
            PlayAreaHealthManager.OnPlayAreaHealthChanged -= OnPlayAreaHealthChanged;
        }

        private void Awake()
        {
            PlayAreaHealthManager.OnPlayAreaHealthChanged += OnPlayAreaHealthChanged;
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
