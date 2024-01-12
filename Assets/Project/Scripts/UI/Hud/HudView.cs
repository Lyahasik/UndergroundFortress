using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Testing;

namespace UndergroundFortress.UI.Hud
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private Button buttonMenu;

        //Testing
        public StatsView playerStatsView;
        public StatsView enemyStatsView;

        private ISceneProviderService _sceneProviderService;

        public void Construct(ISceneProviderService sceneProviderService)
        {
            _sceneProviderService = sceneProviderService;
        }

        public void Initialize()
        {
            buttonMenu.onClick.AddListener(LoadLevel);
        }
        
        private void LoadLevel()
        {
            _sceneProviderService.LoadMainScene();
        }
    }
}
