using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Tutorial.Services;
using UndergroundFortress.UI.Core.Buttons;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Skills
{
    public class SkillsView : MonoBehaviour, IWindow, IReadingProgress
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private Image background;
        [SerializeField] private List<ListSkills> listsSkills;
        
        [Space]
        [SerializeField] private PointsNumberView pointsNumberView;
        [SerializeField] private ButtonAds resetButton;
        
        private ISkillsUpgradeService _skillsUpgradeService;
        private IProgressProviderService _progressProviderService;

        public void Construct(ISkillsUpgradeService skillsUpgradeService,
            IProgressProviderService progressProviderService)
        {
            _skillsUpgradeService = skillsUpgradeService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize(IStaticDataService staticDataService,
            IProcessingAdsService processingAdsService,
            IInformationService informationService)
        {
            pointsNumberView.Initialize(_progressProviderService);
            resetButton.Construct(processingAdsService);
            resetButton.Initialize(ResetActiveSkills);
            
            listsSkills.ForEach(data =>
            {
                data.gameObject.SetActive(data.SkillsType == SkillsType.Dodge);
                data.Construct(staticDataService);
                data.Initialize(informationService, _progressProviderService);
            });

            Register(_progressProviderService);
        }
        
        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            UpdateProgress(progress);
        }

        public void UpdateProgress(ProgressData progress)
        {
            resetButton.gameObject.SetActive(progress.SkillPointsData.Spent > 0);
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
        }

        public void SwitchSkillsType(int idSkillsType)
        {
            listsSkills.ForEach(data =>
            {
                if (data.SkillsType == (SkillsType)idSkillsType)
                    background.color = data.BackgroundColor;
                
                data.gameObject.SetActive(data.SkillsType == (SkillsType) idSkillsType);
            });
        }

        private void ResetActiveSkills()
        {
            _skillsUpgradeService.ResetSkills();
        }

        public void ActivateTutorial(ProgressTutorialService progressTutorialService)
        {
            listsSkills.ForEach(data => data.ActivateTutorial(progressTutorialService));
        }

        public void DeactivateTutorial()
        {
            listsSkills.ForEach(data => data.DeactivateTutorial());
        }
    }
}