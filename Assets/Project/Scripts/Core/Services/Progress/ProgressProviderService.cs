using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.GameStateMachine;
using UndergroundFortress.Scripts.Core.Services.GameStateMachine.States;
using UndergroundFortress.Scripts.Core.Services.StaticData;
using UndergroundFortress.Scripts.Gameplay.Character;
using UndergroundFortress.Scripts.Gameplay.StaticData;
using UndergroundFortress.Scripts.Gameplay.Stats;

namespace UndergroundFortress.Scripts.Core.Services.Progress
{
    public class ProgressProviderService : IProgressProviderService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameStateMachine _gameStateMachine;

        private CharacterStats _playerStats;

        public CharacterStats PlayerStats => _playerStats;

        public ProgressProviderService(IStaticDataService staticDataService,
            IGameStateMachine gameStateMachine)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public PlayerProgress LoadProgress()
        {
            Debug.Log("Loaded progress.");
            _playerStats = LoadingPlayerData();

            _gameStateMachine.Enter<LoadSceneState>();
            
            return new PlayerProgress();
        }
        
        private CharacterStats LoadingPlayerData()
        {
            PlayerStaticData playerStaticData = _staticDataService.ForPlayer();
            CharacterStats characterStats = new CharacterStats(
                playerStaticData.mainStats,
                new CurrentStats(100, 100));

            return characterStats;
        }

        public void SaveProgress()
        {
            
        }
    }
}