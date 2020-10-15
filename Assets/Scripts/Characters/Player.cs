using UnityEngine;
using ProjectChild.Data;

namespace ProjectChild.Characters
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Character character = null;
        public Character Character { get => character; }

        public static Player Instance;

        private void Awake()
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

        private void Start()
        {
            character.Init();
        }
    }
}
