using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.GameStateMachine;
using UndergroundFortress.Scripts.Core.Services.GameStateMachine.States;

namespace UndergroundFortress.Scripts.Core.Services.Progress
{
    public class ProgressProviderService : IProgressProviderService
    {
        private readonly IGameStateMachine _gameStateMachine;

        public ProgressProviderService(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public PlayerProgress LoadProgress()
        {
            Debug.Log("Loaded progress.");
            
            _gameStateMachine.Enter<LoadSceneState>();
            
            return new PlayerProgress();
        }

        public void SaveProgress()
        {
            
        }
    }
}