using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.StaticData;
using UndergroundFortress.Scripts.Gameplay.Character;
using UndergroundFortress.Scripts.Gameplay.StaticData;

namespace UndergroundFortress.Scripts.Core.Services.Factories.Gameplay
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly IStaticDataService _staticDataService;

        public GameplayFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public Canvas CreateGameplayCanvas()
        {
            LevelStaticData levelData = _staticDataService.ForLevel();

            return Object.Instantiate(levelData.gameplayCanvas);
        }

        public AttackArea CreateAttackArea(Transform parent)
        {
            LevelStaticData levelData = _staticDataService.ForLevel();

            return Object.Instantiate(levelData.attackArea, parent);
        }
    }
}