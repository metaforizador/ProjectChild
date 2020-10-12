using System;
using ProjectChild.Characters;

namespace ProjectChild.AI
{
    public abstract class BaseState
    {
        protected Character character;

        public BaseState(Character character)
        {
            this.character = character;
        }

        public abstract Type Update();

    }
}
