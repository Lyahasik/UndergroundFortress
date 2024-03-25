using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.UI.Information.Services
{
    public interface IInformationService : IService
    {
        void Initialize(InformationView informationView);
        public void ShowItem(ItemData itemData);
        public void ShowEquipmentComparison(ItemData equipmentData1, ItemData equipmentData2);
        public void ShowWarning(string text);
    }
}