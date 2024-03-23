namespace UndergroundFortress.Core.Services.Progress
{
    public interface IWritingProgress : IReadingProgress
    {
        public void WriteProgress();
    }
}