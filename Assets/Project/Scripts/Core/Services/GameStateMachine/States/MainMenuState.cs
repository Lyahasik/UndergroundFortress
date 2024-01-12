using UnityEngine;

namespace UndergroundFortress.Core.Services.GameStateMachine.States
{
    public class MainMenuState : IState
    {
        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
        }

        public void Exit() {}
    }
}