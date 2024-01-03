using System.Collections.Generic;

using UndergroundFortress.Scripts.Gameplay.Character;
using UndergroundFortress.Scripts.Gameplay.StaticData;

namespace UndergroundFortress.Scripts.Core.Services.Characters
{
    public interface ICharacterDressingService : IService
    {
        public void DressThePlayer(CharacterStats characterStats, List<ItemStaticData> items);
        public void PutOnAnItem(CharacterStats characterStats, ItemStaticData itemData);
        public void RemoveAnItem(CharacterStats characterStats, ItemStaticData itemData);
    }
}