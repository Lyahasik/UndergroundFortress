using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress.Gameplay.Skills.Services
{
    public class SkillsUpgradeService : ISkillsUpgradeService, IWritingProgress 
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;
        
        private ProgressTutorialService _progressTutorialService;

        private SkillPointsData _skillPointsData;
        private Dictionary<SkillsType, HashSet<int>> _activeSkills;
        private Dictionary<StatType, ProgressSkillData> _progressSkills;

        private bool _isEnoughPoints;

        public bool IsEnoughPoints => _isEnoughPoints;

        public SkillsUpgradeService(IStaticDataService staticDataService, IProgressProviderService progressProviderService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _skillPointsData = progress.SkillPointsData;
            _activeSkills = progress.ActiveSkills;
            _progressSkills = progress.ProgressSkills;
            
            UpdateProgress(progress);
        }

        public void UpdateProgress(ProgressData progress)
        {
            _isEnoughPoints = progress.SkillPointsData.Received > progress.SkillPointsData.Spent;
        }

        public void WriteProgress()
        {
            _progressProviderService.SaveProgress();
        }

        public void ActivationSkill(SkillsType skillsType, int skillId)
        {
            _activeSkills[skillsType].Add(skillId);
            _skillPointsData.Spent++;
            
            CheckTutorial();
            WriteProgress();
        }

        public void UpdateProgressSkill(SkillsType skillsType, StatType statType)
        {
            var skillData = _staticDataService
                .ForSkillsByType(skillsType).skillsData
                .Find(data => data.data.statType == statType);
            var levelData = skillData.levelsData.Find(data => data.level == _progressSkills[statType].CurrentLevel);
            
            if (!_activeSkills[skillsType].Contains(skillData.id)
                || skillData.levelsData.All(data => data.level != _progressSkills[statType].CurrentLevel))
                return;
            
            _progressSkills[statType].CurrentProgress++;

            if (_progressSkills[statType].CurrentProgress >= levelData.progressTarget)
            {
                _progressSkills[statType].CurrentLevel++;
                _progressSkills[statType].CurrentProgress = 0;
            }
            
            WriteProgress();
        }

        public void ResetSkills()
        {
            _progressProviderService.ResetActiveSkills();
            _activeSkills = _progressProviderService.ProgressData.ActiveSkills;
            
            WriteProgress();
        }

        public void ActivateTutorial(ProgressTutorialService progressTutorialService)
        {
            _progressTutorialService = progressTutorialService;
        }
        
        public void DeactivateTutorial()
        {
            _progressTutorialService = null;
        }

        private void CheckTutorial()
        {
            _progressTutorialService?.SuccessStep();
        }
    }
}