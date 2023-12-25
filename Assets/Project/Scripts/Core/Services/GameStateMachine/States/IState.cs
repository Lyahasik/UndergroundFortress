namespace UndergroundFortress.Scripts.Core.Services.GameStateMachine.States
{
    public interface IState : IOutputState
    {
        void Enter();
    }
}