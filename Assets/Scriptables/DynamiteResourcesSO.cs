using UnityEngine;

namespace MatchThreePrototype.Scriptables
{
    [CreateAssetMenu(menuName = "DynamiteResourceSO")]
    public class DynamiteResourcesSO : ScriptableObject
    {
        public AudioClip AudioClipFuse;
        public AudioClip AudioClipExplosion;

        public Sprite[] FuseBurningSprites;
        public Sprite ExplosionEffectSprite;


    }
}