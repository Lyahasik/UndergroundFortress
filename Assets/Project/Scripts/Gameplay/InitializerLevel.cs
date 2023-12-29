using UnityEngine;

using UndergroundFortress.Scripts.Core.Services;
using UndergroundFortress.Scripts.Core.Services.Factories.Gameplay;
using UndergroundFortress.Scripts.Core.Services.Factories.UI;
using UndergroundFortress.Scripts.Core.Services.Scene;
using UndergroundFortress.Scripts.Gameplay.Character;
using UndergroundFortress.Scripts.Gameplay.Characteristics.Services;
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
        private CharacterCharacteristics _characterCharacteristics;

        private ServicesContainer _gameplayServicesContainer;

        private void OnDestroy()
        {
            ClearGameplayServices();
        }

        public void Construct(ISceneProviderService sceneProviderService,
            IGameplayFactory gameplayFactory,
            IUIFactory uiFactory,
            LevelStaticData levelStaticData,
            CharacterCharacteristics characterCharacteristics)
        {
            _sceneProviderService = sceneProviderService;
            _gameplayFactory = gameplayFactory;
            _uiFactory = uiFactory;
            _levelStaticData = levelStaticData;
            _characterCharacteristics = characterCharacteristics;
        }

        public void Initialize()
        {
            RegisterGameplayServices();
                
            HudView hudView = CreateHUD();

            CreateGameplay();
        }

        private void CreateGameplay()
        {
            Canvas gameplayCanvas = _gameplayFactory.CreateGameplayCanvas();

            AttackArea attackArea = _gameplayFactory.CreateAttackArea(gameplayCanvas.transform);
            attackArea.Construct(
                _characterCharacteristics.RealtimeCharacteristics,
                _gameplayServicesContainer.Single<ICheckerCurrentCharacteristicsService>(),
                _gameplayServicesContainer.Single<IAttackService>());
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
            
            _gameplayServicesContainer.Register<ICheckerCurrentCharacteristicsService>(
                new CheckerCurrentCharacteristicsService());
            _gameplayServicesContainer.Register<IAttackService>(
                new AttackService());
        }

        private void ClearGameplayServices()
        {
            _gameplayServicesContainer.Clear();
            
            _gameplayServicesContainer = null;
        }
    }
}