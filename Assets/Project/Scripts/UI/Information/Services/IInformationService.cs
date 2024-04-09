using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.Information.Services
{
    public interface IInformationService : IService
    {
        public void Initialize(InformationView informationView);
        public void ShowSkill(SkillsType skillsType, SkillData skillData, bool isCanUpgrade = false, ProgressSkillData progressSkillData = null);
        public void ShowItem(ItemData itemData);
        public void ShowSaleItem(CellSaleView cellSale);
        public void ShowPurchase(CellPurchaseView cellPurchase);
        public void ShowEquipmentComparison(ItemData equipmentData1, ItemData equipmentData2);
        public void ShowWarning(string text);
    }
}