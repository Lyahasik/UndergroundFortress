using UnityEngine;

using UndergroundFortress.Scripts.Core.Coroutines;
using UndergroundFortress.Scripts.UI.Loading;

namespace UndergroundFortress.Scripts.Core.Services.GameStateMachine.States
{
    public class LoadSceneState : IState
    {
        private readonly CoroutinesContainer _coroutinesContainer;
        private readonly LoadingCurtain _curtain;

        public LoadSceneState(CoroutinesContainer coroutinesContainer,
            LoadingCurtain curtain)
        {
            _coroutinesContainer = coroutinesContainer;
            _curtain = curtain;
        }

        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
            
            _curtain.Show();
        }

        public void Exit()
        {
            _curtain.Hide(_coroutinesContainer);
        }
    }
}