using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.GameStateMachine;
using UndergroundFortress.Scripts.Core.Services.GameStateMachine.States;
using UndergroundFortress.Scripts.Core.Services.StaticData;
using UndergroundFortress.Scripts.Core.Services.Characters;
using UndergroundFortress.Scripts.Gameplay.Character;
using UndergroundFortress.Scripts.Gameplay.StaticData;
using UndergroundFortress.Scripts.Gameplay.Stats;

namespace UndergroundFortress.Scripts.Core.Services.Progress
{
    public class ProgressProviderService : IProgressProviderService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICharacterDressingService _characterDressingService;
        private readonly IGameStateMachine _gameStateMachine;

        private CharacterStats _playerStats;
        private List<ItemStaticData> _playerItems;

        public CharacterStats PlayerStats => _playerStats;
        public List<ItemStaticData> PlayerItems => _playerItems;

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
            _playerStats = LoadingPlayerData();

            _gameStateMachine.Enter<LoadSceneState>();
        }
        
        private CharacterStats LoadingPlayerData()
        {
            PlayerStaticData playerStaticData = _staticDataService.ForPlayer();
            CharacterStats characterStats = new CharacterStats(
                playerStaticData.mainStats,
                new CurrentStats(playerStaticData.mainStats.health, playerStaticData.mainStats.stamina));

            _playerItems = _staticDataService.ForItems();
            _characterDressingService.DressThePlayer(characterStats, _playerItems);
            
            return characterStats;
        }

        public void SaveProgress()
        {
            
        }
    }
}