using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.Core.Services.Progress
{
    public class ProgressProviderService : IProgressProviderService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameStateMachine _gameStateMachine;

        private CharacterStats _playerStats;
        private ProgressData _progressData;

        public CharacterStats PlayerStats => _playerStats;
        public ProgressData ProgressData => _progressData;

        public ProgressProviderService(IStaticDataService staticDataService,
            IGameStateMachine gameStateMachine)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public void LoadProgress()
        {
            Debug.Log("Loaded progress.");
            _playerStats = LoadingBaseStats();
            _progressData = new ProgressData
            {
                Level = 3,
                Equipment = new List<CellData>
                {
                    new (null, 1),
                    new (null, 1),
                    new (null, 1),
                    new (null, 1),
                    new (null, 1),
                    new (null, 1)
                },
                ActiveRecipes = new Dictionary<ItemType, List<int>>
                {
                    { ItemType.Weapon, new List<int> { 1, 2, 2, 2} },
                    { ItemType.RightHand, new List<int> { 1 } },
                    { ItemType.Helmet, new List<int> { 1 } },
                    { ItemType.Bib, new List<int> { 1 } },
                    { ItemType.Trousers, new List<int> { 1 } },
                    { ItemType.Boots, new List<int> { 1 } }
                },
                Bag = new List<CellData>()
            };
            
            for (int i = 0; i < ConstantValues.BASE_SIZE_BAG; i++) 
                _progressData.Bag.Add(new CellData());

            _gameStateMachine.Enter<LoadSceneState>();
        }
        
        private CharacterStats LoadingBaseStats()
        {
            CharacterStaticData characterStaticData = _staticDataService.ForPlayer();
            CharacterStats characterStats = new CharacterStats();
            characterStats.Initialize(characterStaticData);
            
            return characterStats;
        }

        public void SaveProgress()
        {
            
        }
    }
}