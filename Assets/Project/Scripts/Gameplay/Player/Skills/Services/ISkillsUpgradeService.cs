using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Skills.Services
{
    public interface ISkillsUpgradeService : IService
    {
        void ActivationSkill(SkillsType skillsType, int skillId);
        void UpdateProgressSkill(SkillsType skillsType, StatType statType);
    }
}