using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.UI.Skills
{
    public class ProgressSkillView : SkillView
    {
        [Space]
        [SerializeField] private TMP_Text level;
        [SerializeField] private Image progressBarFill;
        [SerializeField] private TMP_Text progressValue;
        
        private IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService, ISkillsUpgradeService skillsUpgradeService)
        {
            base.Construct(skillsUpgradeService);

            _staticDataService = staticDataService;
        }
        
        public void Show(SkillsType skillsType, SkillData skillData, ProgressSkillData progressSkillData, bool isCanUpgrade)
        {
            base.Show(skillsType, skillData, isCanUpgrade);
            
            var progressData = progressSkillData;
            level.text = progressData.CurrentLevel.ToString();
            
            var currentLevelData = _staticDataService
                .ForSkillsByType(_currentSkillsType)
                .skillsData[_currentSkillData.id]
                .levelsData
                .Find(data => data.level == progressData.CurrentLevel);
            progressBarFill.fillAmount = (float) progressData.CurrentProgress / currentLevelData.progressTarget;
            progressValue.text = $"{ progressData.CurrentProgress }/{ currentLevelData.progressTarget }";
        }
    }
}