using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;

namespace UndergroundFortress.UI.Information.Services
{
    public interface IInformationService : IService
    {
        void Initialize(InformationView informationView);
        public void ShowItem(ItemData itemData, bool isEquipped = false);
        public void ShowEquipmentComparison(EquipmentData equipmentData1, EquipmentData equipmentData2);
        public void ShowWarning(string text);
    }
}