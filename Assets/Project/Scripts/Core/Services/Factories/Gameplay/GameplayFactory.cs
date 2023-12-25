using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.StaticData;
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

        public GameObject CreateMatch3()
        {
            LevelStaticData levelData = _staticDataService.ForLevel();

            return new GameObject();
        }
    }
}