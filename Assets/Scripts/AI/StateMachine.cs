using UnityEngine;
using System.Collections;
using AI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectChild.AI
{
    public class StateMachine : MonoBehaviour
    {
        private Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>();
        private BaseState activeState;

        public Action<BaseState> OnStateChanged;

        private void Update()
        {
            if(activeState == null)
            {
                activeState = states.Values.First();
            }

            var nextState = activeState?.Update();
            if(nextState != null && nextState != activeState?.GetType())
            {
                ChangeState(nextState);
            }
        }

        public void SetStates(Dictionary<Type, BaseState> states)
        {
            this.states = states;
        }

        private void ChangeState(Type nextState)
        {
            activeState = states[nextState];
            OnStateChanged?.Invoke(activeState);
        }
    }
}