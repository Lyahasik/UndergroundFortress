using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress.Gameplay.Skills.Services
{
    public interface ISkillsUpgradeService : IService
    {
        public bool IsEnoughPoints { get; }
        public void ActivationSkill(SkillsType skillsType, int skillId);
        public void UpdateProgressSkill(SkillsType skillsType, StatType statType);
        public void ResetSkills();
        public void ActivateTutorial(ProgressTutorialService progressTutorialService);
        public void DeactivateTutorial();
    }
}