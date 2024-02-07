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
                    new (null, 1),
                    new (null, 1),
                    new (null, 1),
                    new (null, 1)
                },
                ActiveRecipes = new Dictionary<ItemType, List<int>>
                {
                    { ItemType.Sword, new List<int> { 7, 9, 10 } },
                    { ItemType.TwoHandedWeapon, new List<int> { 8 } },
                    { ItemType.Dagger, new List<int> { 2 } },
                    { ItemType.Mace, new List<int> { 4 } },
                    { ItemType.Shield, new List<int> { 6 } },
                    { ItemType.Chest, new List<int> { 1, 11 } },
                    { ItemType.Pants, new List<int> { 5 } },
                    { ItemType.Boots, new List<int> { 0 } },
                    { ItemType.Gloves, new List<int> { 3 } }
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