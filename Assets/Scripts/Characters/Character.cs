using System.Collections.Generic;
using UnityEngine;
using ProjectChild.Inputs;
using ProjectChild.Weapons;
using ProjectChild.Models;
using ProjectChild.Movement;
using ProjectChild.Stats;

namespace ProjectChild.Characters
{
    public interface ICharacter
    {
        void Init();
        void Die();
        void DealDamage();
        void TakeDamage(float damage);
        CharacterType GetCharacterType();
    }

    public class Character : MonoBehaviour, ICharacter 
    {
        [SerializeField] protected CharacterStats stats;
        public CharacterStats Stats { get => stats; }

        [SerializeField] protected CharacterModel model;
        public CharacterModel Model { get => model; }

        [SerializeField] protected CharacterMovement movement;

        private void Awake()
        {
            model = new CharacterModel();
        }

        public virtual void DealDamage() {}

        public virtual void Die() {}

        public virtual void Init()
        {
            if(stats != null)
            {
                model.Init(stats);
            }
        }

        public virtual void TakeDamage(float damage)
        {
            if(model != null)
            {
                model.UpdateHP(-damage);
            }
        }

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
