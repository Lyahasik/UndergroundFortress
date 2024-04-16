using UnityEngine;

using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Core.Services.Factories.Gameplay
{
    public interface IGameplayFactory : IService
    {
        public Canvas CreateGameplayCanvas();
        public AttackArea CreateAttackArea(Transform parent);
        public PlayerData CreatePlayer(Transform parent);
        public EnemyData CreateEnemy(EnemyData enemyData, Transform parent);
    }
}