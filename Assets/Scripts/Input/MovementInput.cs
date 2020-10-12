using UnityEngine;

namespace ProjectChild.Inputs
{
    public struct MovementInput
    {
        public Vector3 direction;
        public bool jump;
        public bool dash;
        public bool dashing;
        public bool exitingDash;

        public MovementInput(Vector3 direction, bool jump, bool dash, bool dashing, bool exitingDash)
        {
            this.direction = direction;
            this.jump = jump;
            this.dash = dash;
            this.dashing = dashing;
            this.exitingDash = exitingDash;
        }
    }
}
