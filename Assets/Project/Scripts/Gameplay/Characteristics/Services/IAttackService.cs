using UndergroundFortress.Scripts.Core.Services;

namespace UndergroundFortress.Scripts.Gameplay.Characteristics.Services
{
    public interface IAttackService : IService
    {
        public void AttackEnemy(float damage);
    }
}