using UnityEngine;

namespace ProjectChild.Inputs
{
    public struct MovementInput
    {
        public Vector3 direction;
        public bool jump;

        public MovementInput(Vector3 direction, bool jump)
        {
            this.direction = direction;
            this.jump = jump;
        }
    }
}
