using UnityEngine;
using ProjectChild.Inputs;

namespace ProjectChild.Movement
{
    public interface ICharacterMovement
    {
        void Move(MovementInput input);
        void Attack(AttackInput input);

        bool Grounded();
    }

    public class CharacterMovement : MonoBehaviour, ICharacterMovement
    {
        [SerializeField] protected Animator animator = null;
        [SerializeField] protected CharacterController characterController = null;

        public virtual void Attack(AttackInput Input) {}

        public virtual void Move(MovementInput input) {}

        public virtual bool Grounded() { return false; }
    }
}
