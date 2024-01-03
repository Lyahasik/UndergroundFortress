using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Core.Services.Factories.Gameplay
{
    public interface IGameplayFactory : IService
    {
        public Canvas CreateGameplayCanvas();
        public AttackArea CreateAttackArea(Transform parent);
        public CharacterData CreatePlayer(Transform parent);
        public CharacterData CreateEnemy(Transform parent);
    }
}