using UnityEngine;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class StatsRestorationHandler : MonoBehaviour
    {
        private IStatsRestorationService _statsRestorationService;

        public void Construct(IStatsRestorationService statsRestorationService)
        {
            _statsRestorationService = statsRestorationService;
        }

        private void Update()
        {
            _statsRestorationService?.RestoreStats();
        }
    }
}