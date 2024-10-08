﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Progress;

namespace UndergroundFortress.UI.Skills
{
    public class ProgressSkillButtonView : SkillButtonView
    {
        [Space]
        [SerializeField] private TMP_Text level;
        [SerializeField] private Image progressBarFill;
        [SerializeField] private TMP_Text progressValue;

        private ProgressSkillData _currentProgressData;

        public override void UpdateProgress(ProgressData progress)
        {
            base.UpdateProgress(progress);

            _currentProgressData = progress.ProgressSkills[_skillData.data.statType];
            level.text = _currentProgressData.CurrentLevel.ToString();
            
            var currentLevelData = _staticDataService
                .ForSkillsByType(_skillsType)
                .skillsData[id]
                .levelsData
                .Find(data => data.level == _currentProgressData.CurrentLevel);
            
            progressValue.gameObject.SetActive(currentLevelData != null);
            if (currentLevelData == null)
            {
                progressBarFill.fillAmount = 1f;
                return;
            }
            
            progressBarFill.fillAmount = (float) _currentProgressData.CurrentProgress / currentLevelData.progressTarget;
            progressValue.text = $"{ _currentProgressData.CurrentProgress }/{ currentLevelData.progressTarget }";
        }

        protected override void ShowInformation()
        {
            _informationService.ShowSkill(_skillsType, _skillData, frame.activeSelf, _currentProgressData);
        }
    }
}