using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype
{

    public class SpawnArea : MonoBehaviour
    {

        ContactFilter2D _filter = new ContactFilter2D();

        public bool IsSpawnPositionValid(RectTransform spawnOverlapBox)
        {

            List<Collider2D> results = new List<Collider2D>();
            Physics2D.OverlapBox(spawnOverlapBox.transform.position, spawnOverlapBox.sizeDelta, 0, _filter, results);
            if (results.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Awake()
        {
            _filter.NoFilter();
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