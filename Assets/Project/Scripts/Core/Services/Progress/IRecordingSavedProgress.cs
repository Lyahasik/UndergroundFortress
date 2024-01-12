namespace UndergroundFortress.Core.Services.Progress
{
    public interface IRecordingSavedProgress : ILoadingSavedProgress
    {
        void UpdateProgress(PlayerProgress progress);
    }
}