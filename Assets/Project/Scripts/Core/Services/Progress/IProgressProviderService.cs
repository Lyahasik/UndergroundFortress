using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Core.Services.Progress
{
    public interface IProgressProviderService : IService
    {
        public CharacterStats PlayerStats { get; }
        public PlayerProgress LoadProgress();
        public void SaveProgress();
    }
}