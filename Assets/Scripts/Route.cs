using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectChild.Characters;

namespace AI
{
    public class Route : MonoBehaviour
    {
        public Enemy occupyingEnemy;
        public Character character;
        public List<Vector3> waypoints;

        private void Start()
        {
            foreach(Transform waypoint in transform)
            {
                waypoints.Add(waypoint.position);
            }
        }

        public Vector3 FindClosestPoint(Transform target)
        {
            float closestDistance = float.MaxValue;
            Vector3 closestPoint = new Vector3();

            foreach(var waypoint in waypoints)
            {
                float distance = (waypoint - target.position).magnitude;

                if(distance < closestDistance)
                {
                    closestPoint = waypoint;
                    closestDistance = distance;
                }
            }

            return closestPoint;
        }

        public bool WaypointReached(Transform transform, int waypointIndex)
        {
            if (waypointIndex < 0 || waypointIndex > waypoints.Count - 1) return false;

            var waypoint = waypoints[waypointIndex];


            return false;
        }
    }
}
