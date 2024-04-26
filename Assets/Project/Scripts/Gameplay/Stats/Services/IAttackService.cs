using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public interface IAttackService : IService
    {
        public bool IsDoubleDamage { get; }

        public bool IsVampireDamage { get; }
        
        public void Attack(CharacterData dataAttacking,
            CharacterData dataDefending,
            in float staminaCost,
            bool isPlayerAttack = false);
        public void ActivateConsumable(ConsumableType consumableType);
    }
}