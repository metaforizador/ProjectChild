using UnityEngine;

namespace ProjectChild.Stats
{
    [CreateAssetMenu(fileName = "New Character Stats", menuName = "Project Child/Create New Character Stats")]
    public class CharacterStats : ScriptableObject
    {
        [Header("Health")]
        public float maxHP;
        public float recoveryRateHP;

        [Header("Shield")]
        public float maxShield;
        public float recoveryRateShielf;

        [Header("Stamina")]
        public float maxStamina;
        public float recoveryRateStamina;
    }
}
