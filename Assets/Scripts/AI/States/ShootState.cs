using System;
using UnityEngine;
using ProjectChild.Inputs;
using ProjectChild.Characters;

namespace ProjectChild.AI
{
    public class ShootState : BaseState
    {
        private float shootDistanceThreshold = 100f;

        public ShootState(Character character, float shootDistanceThreshold) : base(character)
        {
            this.shootDistanceThreshold = shootDistanceThreshold;
        }

        public override Type Update()
        {
            var distance = (Characters.Player.Instance.transform.position - character.transform.position).magnitude;

            var movementInput = new MovementInput() { direction = Vector3.zero };
            var attackInput = new AttackInput() { melee = true };

            character.Move(movementInput);
            character.Attack(attackInput);

            if(distance > shootDistanceThreshold)
            {
                attackInput.melee = false;
                character.Attack(attackInput);

                return (typeof(FollowState));
            }



            return typeof(ShootState);
        }
    }
}
