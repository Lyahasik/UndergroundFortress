using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;

namespace UndergroundFortress.UI.Information.Services
{
    public class InformationService : IInformationService
    {
        private InformationView _informationView;

        public void Initialize(InformationView informationView)
        {
            _informationView = informationView;
        }

        public void ShowItem(ItemData itemData, bool isEquipped = false)
        {
            _informationView.ShowItem(itemData, isEquipped);
        }

        public void ShowEquipmentComparison(EquipmentData equipmentData1, EquipmentData equipmentData2)
        {
            _informationView.ShowEquipmentComparison(equipmentData1, equipmentData2);
        }

        public void ShowWarning(string message)
        {
            _informationView.ShowWarning(message);
        }
    }
}