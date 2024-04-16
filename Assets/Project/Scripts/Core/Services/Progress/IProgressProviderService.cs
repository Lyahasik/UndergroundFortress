using UndergroundFortress.Core.Progress;

namespace UndergroundFortress.Core.Services.Progress
{
    public interface IProgressProviderService : IService
    {
        public ProgressData ProgressData { get; }
        public void LoadProgress();
        public void SaveProgress();
        public void Register(IReadingProgress progressReader);
        public void Register(IWritingProgress progressWriter);
        public void Unregister(IReadingProgress progressReader);
        public void Unregister(IWritingProgress progressWriter);
    }
}