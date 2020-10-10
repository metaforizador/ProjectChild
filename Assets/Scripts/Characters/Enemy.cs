using UnityEngine;
using ProjectChild.Data;

namespace ProjectChild.Characters
{
    public class Enemy : Character
    {
        [SerializeField] EnemySO data = null;


        public override CharacterType GetCharacterType()
        {
            return CharacterType.Enemy;
        }
    }
}
