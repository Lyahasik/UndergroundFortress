using UndergroundFortress.Core.Progress;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.UI.Information.Services
{
    public class InformationService : IInformationService
    {
        private InformationView _informationView;

        public void Initialize(InformationView informationView)
        {
            _informationView = informationView;
        }

        public void ShowSkill(SkillsType skillsType,
            SkillData skillData,
            bool isCanUpgrade = false, 
            ProgressSkillData progressSkillData = null)
        {
            _informationView.ShowSkill(skillsType, skillData, isCanUpgrade, progressSkillData);
        }

        public void ShowItem(ItemData itemData)
        {
            _informationView.ShowItem(itemData);
        }

        public void ShowEquipmentComparison(ItemData equipmentData1, ItemData equipmentData2)
        {
            _informationView.ShowEquipmentComparison(equipmentData1, equipmentData2);
        }

        public void ShowWarning(string message)
        {
            _informationView.ShowWarning(message);
        }
    }
}