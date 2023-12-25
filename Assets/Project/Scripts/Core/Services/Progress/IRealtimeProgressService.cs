namespace UndergroundFortress.Scripts.Core.Services.Progress
{
    public interface IRealtimeProgressService : IService
    {
        public PlayerProgress Progress { get; set; }
    }
}