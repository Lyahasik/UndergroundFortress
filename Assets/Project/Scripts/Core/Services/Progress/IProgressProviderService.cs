using UndergroundFortress.Core.Progress;

namespace UndergroundFortress.Core.Services.Progress
{
    public interface IProgressProviderService : IService
    {
        public ProgressData ProgressData { get; }
        public void LoadProgress();
        public void SaveProgress();
        void Register(IReadingProgress progressReader);
        void Register(IWritingProgress progressWriter);
    }
}