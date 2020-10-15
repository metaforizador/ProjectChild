using System.Collections.Generic;
using UnityEngine;
using ProjectChild.Characters;

namespace ProjectChild.Weapons
{
    public class MeleeWeapon : Weapon
    {
        [SerializeField] private Collider collider = null;

        private List<Character> charactersHit = new List<Character>();


        private void OnTriggerEnter(Collider other)
        {
            TargetHit(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TargetHit(other);
        }

        protected virtual void TargetHit(Collider hit)
        {
            var character = hit.GetComponentInParent<Character>();

            if(!charactersHit.Contains(character))
            {
                charactersHit.Add(character);

                character.TakeDamage(data.damageBase);
            }
        }

        public virtual void Reset()
        {
            charactersHit.Clear();
        }

    }
}
