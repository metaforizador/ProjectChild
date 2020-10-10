using UnityEngine;

namespace ProjectChild.Movement
{
    public interface ICharacterMovement
    {
        void Move(Vector3 direction);
        void Fire(Vector3 direction);

        bool Grounded();
    }

    public class CharacterMovement : MonoBehaviour, ICharacterMovement
    {
        [SerializeField] protected Animator animator = null;
        [SerializeField] protected CharacterController characterController = null;

        public virtual void Fire(Vector3 direction) {}

        public virtual void Move(Vector3 direction) {}

        public virtual bool Grounded() { return false; }
    }
}
