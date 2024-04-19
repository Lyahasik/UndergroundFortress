using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.UI.Skills
{
    public class ProgressSkillView : SkillView
    {
        [Space]
        [SerializeField] private TMP_Text level;
        [SerializeField] private Image progressBarFill;
        [SerializeField] private TMP_Text progressValue;
        
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
            
            progressValue.gameObject.SetActive(currentLevelData != null);
            if (currentLevelData == null)
            {
                progressBarFill.fillAmount = 1f;
                return;
            }
            
            progressBarFill.fillAmount = (float) progressData.CurrentProgress / currentLevelData.progressTarget;
            progressValue.text = $"{ progressData.CurrentProgress }/{ currentLevelData.progressTarget }";
        }
    }
}