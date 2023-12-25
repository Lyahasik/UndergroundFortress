using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.Progress;

namespace UndergroundFortress.Scripts.Core.Services.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IProgressProviderService _progressProviderService;

        public LoadProgressState(IProgressProviderService progressProviderService)
        {
            _progressProviderService = progressProviderService;
        }

        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
            
            _progressProviderService.LoadProgress();
        }

        public void Exit() {}
    }
}