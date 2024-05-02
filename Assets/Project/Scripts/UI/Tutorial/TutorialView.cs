using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private List<TutorialStageView> stages;

        private ILocalizationService _localizationService;
        private IProgressTutorialService _progressTutorialService;
        
        private TutorialStageView _currentStageView;

        public void Construct(ILocalizationService localizationService, IProgressTutorialService progressTutorialService)
        {
            _localizationService = localizationService;
            _progressTutorialService = progressTutorialService;
        }

        public void ActivateStage(TutorialStageType stageType)
        {
            _currentStageView = stages.Find(data => data.StageType == stageType);
            _currentStageView?.Construct(_localizationService);
            _currentStageView?.Activate();
        }

        public void SuccessStep()
        {
            if (_currentStageView == null
                || _currentStageView.CurrentStep == null)
                return;
            
            if (_currentStageView.CurrentStep.IsClosing)
                _progressTutorialService.OnClose();

            if (!_currentStageView.TryDeactivate())
                return;

            _progressTutorialService.DeactivateStage();
            
            if (_currentStageView.StageType == TutorialStageType.FirstShopping)
                _progressTutorialService.TryActivateStage(TutorialStageType.SecondCreateEquipment);
        }
    }
}
