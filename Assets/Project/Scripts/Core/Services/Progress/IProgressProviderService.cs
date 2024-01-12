using UndergroundFortress.Core.Progress;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Core.Services.Progress
{
    public interface IProgressProviderService : IService
    {
        public CharacterStats PlayerStats { get; }
        public ProgressData ProgressData { get; }
        public void LoadProgress();
        public void SaveProgress();
    }
}