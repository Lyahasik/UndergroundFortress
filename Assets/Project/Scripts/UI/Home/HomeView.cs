using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Testing;

namespace UndergroundFortress.UI.MainMenu
{
    public class HomeView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private MainStatsView statsView;

        private IProcessingPlayerStatsService _processingPlayerStatsService;

        public void Construct(IProcessingPlayerStatsService processingPlayerStatsService)
        {
            _processingPlayerStatsService = processingPlayerStatsService;
        }

        public void Initialize(IStaticDataService staticDataService, ILocalizationService localizationService)
        {
            statsView.Construct(staticDataService, localizationService);
        }
        
        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
            
            if (type == windowType)
            {
                statsView.UpdateValues(_processingPlayerStatsService.PlayerStats);
            }
        }
    }
}