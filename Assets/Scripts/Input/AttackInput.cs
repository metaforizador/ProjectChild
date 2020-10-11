using UnityEngine;

namespace ProjectChild.Inputs
{
    public struct AttackInput
    {
        public bool shooting;
        public bool melee;

        public AttackInput(bool shooting, bool melee)
        {
            this.shooting = shooting;
            this.melee = melee;
        }
    }
}
