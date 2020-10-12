using UnityEngine;
using ProjectChild.Data;

namespace ProjectChild.Characters
{
    public class Player : Character
    {
        public static Player Instance;

        private void Start()
        {
            if(Instance == null && Instance != this)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
