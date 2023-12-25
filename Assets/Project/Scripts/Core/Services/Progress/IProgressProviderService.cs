namespace UndergroundFortress.Scripts.Core.Services.Progress
{
    public interface IProgressProviderService : IService
    {
        public PlayerProgress LoadProgress();
        public void SaveProgress();
    }
}