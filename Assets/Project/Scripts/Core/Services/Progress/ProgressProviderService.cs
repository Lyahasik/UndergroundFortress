using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Characters;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.Core.Services.Progress
{
    public class ProgressProviderService : IProgressProviderService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICharacterDressingService _characterDressingService;
        private readonly IGameStateMachine _gameStateMachine;

        private CharacterStats _playerStats;
        private ProgressData _progressData;

        public CharacterStats PlayerStats => _playerStats;
        public ProgressData ProgressData => _progressData;

        public ProgressProviderService(IStaticDataService staticDataService,
            ICharacterDressingService characterDressingService,
            IGameStateMachine gameStateMachine)
        {
            _staticDataService = staticDataService;
            _characterDressingService = characterDressingService;
            _gameStateMachine = gameStateMachine;
        }

        public void LoadProgress()
        {
            Debug.Log("Loaded progress.");
            _playerStats = LoadingBaseStats();
            _progressData = new ProgressData
            {
                Level = 3,
                PlayerEquipments = new List<EquipmentData>()
            };

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