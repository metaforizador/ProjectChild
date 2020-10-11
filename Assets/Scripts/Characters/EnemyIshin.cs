using UnityEngine;
using AI;
using ProjectChild.AI;
using ProjectChild.Movement;
using System.Collections.Generic;
using System;

namespace ProjectChild.Characters
{
    public class EnemyIshin : Enemy
    {
        [SerializeField] private float followDistanceThreshold = 500f;
        [SerializeField] private float shootDistanceThreshold = 100f;

        [Header("AI")]
        [SerializeField] private StateMachine stateMachine = null;

        private void Awake()
        {
            var routes = FindObjectsOfType<Route>();
            var states = new Dictionary<Type, BaseState>()
            {
                { typeof(PatrolState), new PatrolState(this, routes, followDistanceThreshold) },
                { typeof(FollowState), new FollowState(this, followDistanceThreshold, shootDistanceThreshold) },
                { typeof(ShootState), new ShootState(this, shootDistanceThreshold) }
            };

            stateMachine.SetStates(states);
        }

        private void Update()
        {

        }

        


        public override CharacterType GetCharacterType()
        {
            return CharacterType.EnemyIshin;
        }
    }
}
