using UnityEngine;

namespace ProjectChild.Characters
{
    public class Halycon : Character
    {
        public override void Init()
        {
            base.Init();

            // TODO: Maybe use a compound collider instead of character controller
            // this will allow for more accurate colliders and we won't have to set isTrigger via code
            var collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }
    }
}
