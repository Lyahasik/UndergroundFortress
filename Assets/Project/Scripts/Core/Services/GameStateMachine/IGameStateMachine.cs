using UndergroundFortress.Core.Services.GameStateMachine.States;

namespace UndergroundFortress.Core.Services.GameStateMachine
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TData>(TData data) where TState : class, IDataState<TData>;
    }
}