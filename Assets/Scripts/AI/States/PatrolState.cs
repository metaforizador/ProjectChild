using System;
using UnityEngine;
using AI;
using ProjectChild.Inputs;
using ProjectChild.Characters;

namespace ProjectChild.AI
{
    public class PatrolState : BaseState
    {
        private float followDistanceThreshold = 500f;
        private Route[] routes;
        private Route activeRoute;
        private Vector3 activeWaypoint;

        public PatrolState(Character character, Route[] routes, float followDistanceThreshold) : base(character)
        {
            this.routes = routes;
            this.followDistanceThreshold = followDistanceThreshold;
        }

        public override Type Update()
        {
            if (activeRoute == null)
            {
                if (routes == null) return typeof(PatrolState);
                else if (routes.Length <= 0) return typeof(PatrolState);
                activeRoute = routes[0];
            }

            // check if there is a closer route 
            float closestDistance = float.MaxValue;
            Route closestRoute = activeRoute;
            foreach (var route in routes)
            {
                if (route.character != null) continue;

                var closestWaypoint = route.FindClosestPoint(character.transform);
                var distance = (closestWaypoint - character.transform.position).magnitude;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRoute = route;
                }
            }

            // switch routes if the closest one is not the one the character is currently patroling
            if (closestRoute != activeRoute)
            {
                activeRoute.character = null;
                activeRoute = closestRoute;
                activeRoute.character = character;
                activeWaypoint = activeRoute.FindClosestPoint(character.transform);
            }

            var waypointReached = activeRoute.WaypointReached(character.transform, activeRoute.waypoints.IndexOf(activeWaypoint));

            // select next closest waypoint if current waypoint is reached
            if (waypointReached)
            {
                var waypointIndex = activeRoute.waypoints.IndexOf(activeWaypoint);
                waypointIndex = (waypointIndex + 1) % activeRoute.waypoints.Count;
                activeWaypoint = activeRoute.waypoints[waypointIndex];
            }

            var movementInput = new MovementInput()
            {
                direction = activeWaypoint - character.transform.position
            };

            character.Move(movementInput);

            // exit state condition
            // when player is within threshold distance switch to follow state
            var distancePlayer = (character.transform.position - Characters.Player.Instance.transform.position).magnitude;
            if(distancePlayer < followDistanceThreshold)
            {
                return typeof(FollowState);
            }


            return typeof(PatrolState);
        }
    }
}
