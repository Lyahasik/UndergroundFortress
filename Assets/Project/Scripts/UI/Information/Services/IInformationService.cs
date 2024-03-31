﻿using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.UI.Information.Services
{
    public interface IInformationService : IService
    {
        public void Initialize(InformationView informationView);
        public void ShowSkill(SkillsType skillsType, SkillData skillData, bool isCanUpgrade = false, ProgressSkillData progressSkillData = null);
        public void ShowItem(ItemData itemData);
        public void ShowEquipmentComparison(ItemData equipmentData1, ItemData equipmentData2);
        public void ShowWarning(string text);
    }
}