using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.GameStateMachine;
using UndergroundFortress.Scripts.Core.Services.GameStateMachine.States;
using UndergroundFortress.Scripts.Core.Services.StaticData;
using UndergroundFortress.Scripts.Gameplay.Character;
using UndergroundFortress.Scripts.Gameplay.Characteristics;
using UndergroundFortress.Scripts.Gameplay.StaticData;

namespace UndergroundFortress.Scripts.Core.Services.Progress
{
    public class ProgressProviderService : IProgressProviderService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameStateMachine _gameStateMachine;

        private CharacterCharacteristics _characterCharacteristics;

        public CharacterCharacteristics CharacterCharacteristics => _characterCharacteristics;

        public ProgressProviderService(IStaticDataService staticDataService,
            IGameStateMachine gameStateMachine)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public PlayerProgress LoadProgress()
        {
            Debug.Log("Loaded progress.");
            _characterCharacteristics = LoadingPlayerData();

            _gameStateMachine.Enter<LoadSceneState>();
            
            return new PlayerProgress();
        }
        
        private CharacterCharacteristics LoadingPlayerData()
        {
            CharacterStaticData characterStaticData = _staticDataService.ForCharacter();
            CharacterCharacteristics characterCharacteristics = new CharacterCharacteristics(
                characterStaticData.mainCharacteristics,
                new RealtimeCharacteristics(characterStaticData.mainCharacteristics.maxStamina, 10, 10, 13));

            return characterCharacteristics;
        }

        public void SaveProgress()
        {
            
        }
    }
}