using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.StaticData;
using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Core.Services.Factories.Gameplay
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

        public CharacterData CreatePlayer(Transform parent) => 
            PrefabInstantiate(_staticDataService.ForLevel().player, parent);

        public CharacterData CreateEnemy(Transform parent) => 
            PrefabInstantiate(_staticDataService.ForLevel().enemy, parent);
    }
}