using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;

namespace UndergroundFortress
{
    public class PointsNumberView : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private TMP_Text pointsNumberText;
        
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
            int number = progress.SkillPointsData.Received - progress.SkillPointsData.Spent;
            
            pointsNumberText.text = number.ToString();
        }
    }
}
