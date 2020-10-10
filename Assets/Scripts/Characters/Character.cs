using System.Collections.Generic;
using UnityEngine;
using ProjectChild.Weapons;
using ProjectChild.Models;

namespace ProjectChild.Characters
{
    public interface ICharacter
    {
        void Init();
        void Die();
        void DealDamage();
        void TakeDamage();
        CharacterType GetCharacterType();
    }

    public class Character : MonoBehaviour, ICharacter 
    {
        [SerializeField] protected CharacterModel model;

        public virtual void DealDamage() {}

        public virtual void Die() {}

        public virtual void Init() {}

        public virtual void TakeDamage() {}

        public virtual CharacterType GetCharacterType()
        {
            return CharacterType.Undefined;
        }
    }

    public enum CharacterType
    {
        Undefined,
        Player,
        Enemy
    }
}
