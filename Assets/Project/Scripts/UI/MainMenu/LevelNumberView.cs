using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;

namespace UndergroundFortress
{
    public class LevelNumberView : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private TMP_Text number;
        
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
            number.text = progress.LevelData.Level.ToString();
        }
    }
}
