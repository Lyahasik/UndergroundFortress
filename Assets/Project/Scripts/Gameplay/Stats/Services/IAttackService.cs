using UndergroundFortress.Scripts.Core.Services;
using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public interface IAttackService : IService
    {
        public void Attack(CharacterStats characterStats, float damage);
    }
}