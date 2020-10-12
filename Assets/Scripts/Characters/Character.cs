using System.Collections.Generic;
using UnityEngine;
using ProjectChild.Inputs;
using ProjectChild.Weapons;
using ProjectChild.Models;
using ProjectChild.Movement;

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
        [SerializeField] protected CharacterMovement movement;

        public virtual void DealDamage() {}

        public virtual void Die() {}

        public virtual void Init() {}

        public virtual void TakeDamage() {}

        public virtual void Move(MovementInput input)
        {
            if(movement != null)
            {
                movement.Move(input);
            }
        }

        public virtual void Attack(AttackInput input)
        {
            if(movement != null)
            {
                movement.Attack(input);
            }
        }

        public virtual CharacterType GetCharacterType()
        {
            return CharacterType.Undefined;
        }
    }

    public enum CharacterType
    {
        Undefined,
        Player,
        EnemyGeneric,
        EnemyIshin
    }
}
