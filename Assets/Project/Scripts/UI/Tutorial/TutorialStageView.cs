using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress
{
    public class TutorialStageView : MonoBehaviour
    {
        [SerializeField] private GameObject cap;

        [Space]
        [SerializeField] private TutorialStageType stageType;
        [SerializeField] private List<TutorialStepView> steps;

        private ILocalizationService _localizationService;
        private TutorialStepView _currentStep;
        private int _currentStepId;

        public TutorialStageType StageType => stageType;
        public TutorialStepView CurrentStep => _currentStep;

        public void Construct(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public void Activate()
        {
            _currentStep = steps[_currentStepId];
            _currentStep.Activate(_localizationService);
            cap.SetActive(_currentStep.IsCapping);

            _currentStepId++;
            gameObject.SetActive(true);
        }

        public bool TryDeactivate()
        {
            if (_currentStep == null)
                return true;
            
            cap.SetActive(false);
            gameObject.SetActive(false);
            
            _currentStep?.Deactivate();
            _currentStep = null;
            if (_currentStepId < steps.Count)
            {
                Activate();
                return false;
            }

            return true;
        }
    }
}