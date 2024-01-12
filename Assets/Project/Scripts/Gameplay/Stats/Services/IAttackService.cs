using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public interface IAttackService : IService
    {
        public void Attack(CharacterData dataAttacking, CharacterData dataDefending);
    }
}