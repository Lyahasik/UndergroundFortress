using System.Collections.Generic;

using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Items.Equipment;

namespace UndergroundFortress.Core.Services.Characters
{
    public interface ICharacterDressingService : IService
    {
        public void DressThePlayer(CharacterData characterData, List<EquipmentData> items);
        public void PutOnAnItem(CharacterData characterData, EquipmentData itemData);
        public void RemoveAnItem(CharacterData characterData, EquipmentData itemData);
    }
}