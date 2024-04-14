using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Core.Services.Factories.Gameplay
{
    public class GameplayFactory : Factory, IGameplayFactory
    {
        private readonly IStaticDataService _staticDataService;

        public GameplayFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public Canvas CreateGameplayCanvas() => 
            PrefabInstantiate(_staticDataService.ForLevel().gameplayCanvas);

        public AttackArea CreateAttackArea(Transform parent) => 
            PrefabInstantiate(_staticDataService.ForLevel().attackArea, parent);

        public PlayerData CreatePlayer(Transform parent) => 
            PrefabInstantiate(_staticDataService.ForLevel().player, parent);

        public EnemyData CreateEnemy(Transform parent) => 
            PrefabInstantiate(_staticDataService.ForLevel().enemy, parent);
    }
}