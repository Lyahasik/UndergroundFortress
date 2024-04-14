using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.UI.Hud
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private Button buttonMenu;

        [Space]
        [SerializeField] private LevelNumberView levelNumberView;
        [SerializeField] private ExperienceBarView experienceBarView;
        
        [Space]
        [SerializeField] private CurrentStatFillView playerHealthFill;
        [SerializeField] private CurrentStatFillView playerStaminaFill;

        private ISceneProviderService _sceneProviderService;
        
        public CurrentStatFillView PlayerHealthFill => playerHealthFill;
        public CurrentStatFillView PlayerStaminaFill => playerStaminaFill;

        public void Construct(ISceneProviderService sceneProviderService)
        {
            _sceneProviderService = sceneProviderService;
        }

        public void Initialize(IStaticDataService staticDataService, IProgressProviderService progressProviderService)
        {
            buttonMenu.onClick.AddListener(LoadMainScene);
            
            levelNumberView.Initialize(progressProviderService);
            
            experienceBarView.Construct(staticDataService);
            experienceBarView.Initialize(progressProviderService);
        }
        
        private void LoadMainScene()
        {
            _sceneProviderService.LoadMainScene();
        }
    }
}
