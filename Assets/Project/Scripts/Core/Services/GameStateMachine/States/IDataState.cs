namespace UndergroundFortress.Core.Services.GameStateMachine.States
{
    public interface IDataState<TData> : IOutputState
    {
        void Enter(TData data);
    }
}