using UnityEngine;

namespace UndergroundFortress.Scripts.Gameplay.Characteristics.Services
{
    public class AttackService : IAttackService
    {
        public void AttackEnemy(float damage)
        {
            Debug.Log($"Damage { damage } for enemy.");
        }
    }
}