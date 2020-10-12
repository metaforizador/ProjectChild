using System;
using ProjectChild.Inputs;
using ProjectChild.Characters;

namespace ProjectChild.AI
{
    public class FollowState : BaseState
    {
        private float followDistanceThreshold = 500f;
        private float shootDistanceThreshold = 100f;

        public FollowState(Character character, float followDistanceThreshold, float shootDistanceThreshold) : base(character)
        {
            this.followDistanceThreshold = followDistanceThreshold;
            this.shootDistanceThreshold  = shootDistanceThreshold;
        }

        public override Type Update()
        {
            var direction = Characters.Player.Instance.transform.position - character.transform.position;
            var distance = direction.magnitude;
            var movementInput = new MovementInput()
            {
                direction = direction
            };

            character.Move(movementInput);

            if(distance >= followDistanceThreshold)
            {
                return typeof(PatrolState);
            }
            else if(distance < shootDistanceThreshold)
            {
                return typeof(ShootState);
            }

            return typeof(FollowState);
        }
    }
}
