using UndergroundFortress.Core.Progress;

namespace UndergroundFortress.Core.Services.Progress
{
    public interface IReadingProgress
    {
        public void Register(IProgressProviderService progressProviderService);
        public void ReadProgress(ProgressData progress);
    }
}