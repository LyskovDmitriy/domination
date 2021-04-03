using UnityEngine;


namespace Domination.Warfare
{
    [CreateAssetMenu]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private int damage = default;
        [SerializeField] private int range = default;

        public string Name => name;
        public int Damage => damage;
        public int Range => range;
    }
}
