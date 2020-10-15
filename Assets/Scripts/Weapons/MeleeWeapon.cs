using UnityEngine;
using ProjectChild.Characters;

namespace ProjectChild.Weapons
{
    public class MeleeWeapon : Weapon
    {
        [SerializeField] private Collider collider = null;


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collider: {other.name}");

            var character = collider.GetComponentInParent<Character>();
        }

    }
}
