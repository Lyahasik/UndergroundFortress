using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Information.Services;

namespace UndergroundFortress.UI.Skills
{
    public class ListSkills : MonoBehaviour
    {
        [SerializeField] private SkillsType skillsType;
        [SerializeField] private List<SkillButtonView> skills;

        private IStaticDataService _staticDataService;

        public SkillsType SkillsType => skillsType;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Initialize(IInformationService informationService, IProgressProviderService progressProviderService)
        {
            skills.ForEach(data =>
            {
                data.Construct(
                    _staticDataService,
                    informationService,
                    skillsType,
                    _staticDataService.ForSkillsByType(skillsType).skillsData[data.Id]);
                data.Initialize(progressProviderService);
            });
        }
    }
}