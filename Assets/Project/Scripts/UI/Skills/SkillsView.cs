using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Skills
{
    public class SkillsView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private PointsNumberView pointsNumberView;
        [SerializeField] private List<ListSkills> listsSkills;

        public void Initialize(IStaticDataService staticDataService,
            IInformationService informationService,
            IProgressProviderService progressProviderService)
        {
            pointsNumberView.Initialize(progressProviderService);
            
            listsSkills.ForEach(data =>
            {
                data.gameObject.SetActive(data.SkillsType == SkillsType.Dodge);
                data.Construct(staticDataService);
                data.Initialize(informationService, progressProviderService);
            });
        }
        
        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
        }

        public void SwitchSkillsType(int idSkillsType)
        {
            listsSkills.ForEach(data => data.gameObject.SetActive(data.SkillsType == (SkillsType) idSkillsType));
        }
    }
}