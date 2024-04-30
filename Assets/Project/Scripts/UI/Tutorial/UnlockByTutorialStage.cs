using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress
{
    public class UnlockByTutorialStage : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private TutorialStageType stageType;

        private IProgressProviderService _progressProviderService;
        private bool _isUnlock;

        public void Initialize(IProgressProviderService progressProviderService)
        {
            _progressProviderService = progressProviderService;
            
            Register(progressProviderService);
        }

        private void OnDestroy()
        {
            _progressProviderService.Unregister(this);
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
            if (_isUnlock)
                return;
            
            _isUnlock = progress.TutorialStages.Contains((int) stageType);
            gameObject.SetActive(_isUnlock);
        }
    }
}
