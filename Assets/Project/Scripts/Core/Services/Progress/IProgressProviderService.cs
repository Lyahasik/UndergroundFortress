using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Scene;

namespace UndergroundFortress.Core.Services.Progress
{
    public interface IProgressProviderService : IService
    {
        public ProgressData ProgressData { get; }
        public ISceneProviderService SceneProviderService { set; }
        public void StartLoadData();
        public void LoadProgress(string json);
        public void SaveProgress();
        public void Register(IReadingProgress progressReader);
        public void Register(IWritingProgress progressWriter);
        public void Unregister(IReadingProgress progressReader);
        public void Unregister(IWritingProgress progressWriter);
        public void ResetActiveSkills();
        public void WasChange();
        public void IncreaseCrafting();
        public void IncreasePurchases();
        public void IncreaseKilling();
    }
}