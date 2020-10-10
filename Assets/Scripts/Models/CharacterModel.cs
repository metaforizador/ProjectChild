using System;
using UnityEngine;

namespace ProjectChild.Models
{
    public class CharacterModel : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] protected float hp;
        public float HP
        {
            get { return hp; }
            protected set 
            {
                hp = value;
                OnHPUpdated.Invoke(value);
            }
        }

        [SerializeField] private float shield;
        public float Shield
        {
            get { return shield; }
            protected set 
            { 
                shield = value;
                OnShieldUpdated.Invoke(value);
            }
        }

        [SerializeField] private float stamina;
        public float Stamina
        {
            get { return stamina; }
            protected set 
            { 
                stamina = value;
                OnStaminaUpdated.Invoke(value);
            }
        }

        public Action<float> OnHPUpdated;
        public Action<float> OnShieldUpdated;
        public Action<float> OnStaminaUpdated;
    }
}
