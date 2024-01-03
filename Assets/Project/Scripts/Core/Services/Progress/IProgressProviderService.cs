using System.Collections.Generic;

using UndergroundFortress.Scripts.Gameplay.Character;
using UndergroundFortress.Scripts.Gameplay.StaticData;

namespace UndergroundFortress.Scripts.Core.Services.Progress
{
    public interface IProgressProviderService : IService
    {
        public CharacterStats PlayerStats { get; }
        public List<ItemStaticData> PlayerItems { get; }
        public void LoadProgress();
        public void SaveProgress();
    }
}