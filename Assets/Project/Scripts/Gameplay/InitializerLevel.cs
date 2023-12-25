using UnityEngine;

using UndergroundFortress.Scripts.Core.Services;
using UndergroundFortress.Scripts.Core.Services.Factories.Gameplay;
using UndergroundFortress.Scripts.Core.Services.Factories.UI;
using UndergroundFortress.Scripts.Core.Services.Scene;
using UndergroundFortress.Scripts.Gameplay.StaticData;
using UndergroundFortress.Scripts.UI.Hud;

namespace UndergroundFortress.Scripts.Gameplay
{
    public class InitializerLevel : MonoBehaviour
    {
        private ISceneProviderService _sceneProviderService;
        private IGameplayFactory _gameplayFactory;
        private IUIFactory _uiFactory;
        private LevelStaticData _levelStaticData;

        private ServicesContainer _gameplayServicesContainer;

        private void OnDestroy()
        {
            ClearGameplayServices();
        }

        public void Construct(ISceneProviderService sceneProviderService,
            IGameplayFactory gameplayFactory,
            IUIFactory uiFactory,
            LevelStaticData levelStaticData)
        {
            _sceneProviderService = sceneProviderService;
            _gameplayFactory = gameplayFactory;
            _uiFactory = uiFactory;
            _levelStaticData = levelStaticData;
        }

        public void Initialize()
        {
            RegisterGameplayServices();
                
            HudView hudView = CreateHUD();
        }

        private HudView CreateHUD()
        {
            HudView hudView = _uiFactory.CreateHUD();
            hudView.Construct(_sceneProviderService);
            hudView.Initialize();

            return hudView;
        }

        private void RegisterGameplayServices()
        {
            _gameplayServicesContainer = new ServicesContainer();
        }

        private void ClearGameplayServices()
        {
            _gameplayServicesContainer.Clear();
            
            _gameplayServicesContainer = null;
        }
    }
}