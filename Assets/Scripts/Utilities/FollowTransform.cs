using UnityEngine;
using System.Collections;

namespace ProjectChild.Utilities
{
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform transformToFollow = null;

        private Vector3 cachedPosition = Vector3.zero;
        private Quaternion cachedRotation = Quaternion.identity;

        private void Update()
        {
            if (!transformToFollow) return;

            if(cachedPosition != transformToFollow.position)
            {
                cachedPosition = transformToFollow.position;
                transform.position = cachedPosition;
            }

            if(cachedRotation != transformToFollow.rotation)
            {
                cachedRotation = transformToFollow.rotation;
                transform.rotation = cachedRotation;
            }
        }

    }
}