using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private List<TutorialStageView> stages;

        private IProgressTutorialService _progressTutorialService;
        private TutorialStageView _currentStageView;
        

        public void Construct(IProgressTutorialService progressTutorialService)
        {
            _progressTutorialService = progressTutorialService;
        }

        public void ActivateStage(TutorialStageType stageType)
        {
            _currentStageView = stages.Find(data => data.StageType == stageType);
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
