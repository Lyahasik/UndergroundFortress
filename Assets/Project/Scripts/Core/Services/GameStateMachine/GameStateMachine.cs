using System;
using System.Collections.Generic;

using UndergroundFortress.Scripts.Core.Coroutines;
using UndergroundFortress.Scripts.Core.Services.GameStateMachine.States;
using UndergroundFortress.Scripts.Core.Services.Progress;
using UndergroundFortress.Scripts.UI.Loading;

namespace UndergroundFortress.Scripts.Core.Services.GameStateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IOutputState> _states;
        
        private IOutputState _activeState;

        public void Initialize(IProgressProviderService progressProviderService,
            CoroutinesContainer coroutinesContainer,
            LoadingCurtain curtain)
        {
            _states = new Dictionary<Type, IOutputState>
            {
                [typeof(LoadProgressState)] = new LoadProgressState(progressProviderService),
                [typeof(LoadSceneState)] = new LoadSceneState(coroutinesContainer, curtain),
                [typeof(MainMenuState)] = new MainMenuState(),
                [typeof(GameplayState)] = new GameplayState()
            };
        }
    
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TData>(TData data) where TState : class, IDataState<TData>
        {
            TState state = ChangeState<TState>();
            state.Enter(data);
        }

        private TState ChangeState<TState>() where TState : class, IOutputState
        {
            _activeState?.Exit();
      
            TState state = GetState<TState>();
            _activeState = state;
      
            return state;
        }

        private TState GetState<TState>() where TState : class, IOutputState => 
            _states[typeof(TState)] as TState;
    }
}