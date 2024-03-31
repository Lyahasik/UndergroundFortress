using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;

namespace UndergroundFortress
{
    public class ExperienceBarView : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private Image fill;
        
        private IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void Initialize(IProgressProviderService progressProviderService)
        {
            Register(progressProviderService);
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
            var levelStaticData = _staticDataService.GetPlayerLevelByCurrent(progress.LevelData.Level);

            if (levelStaticData != null)
                fill.fillAmount = (float)progress.LevelData.CurrentExperience / levelStaticData.targetExperience;
            else
                fill.fillAmount = 1f;
        }
    }
}
