using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Dungeons.Services;
using UndergroundFortress.Gameplay.Stats.Services;

namespace UndergroundFortress.UI.Hud
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private LevelNumberView levelNumberView;
        [SerializeField] private ExperienceBarView experienceBarView;
        
        [Space]
        [SerializeField] private CurrentStatFillView playerHealthFill;
        [SerializeField] private CurrentStatFillView playerStaminaFill;

        [Space]
        [SerializeField] private TMP_Text nameLevelText;
        [SerializeField] private LevelDungeonProgressBar levelDungeonProgressBar;

        [FormerlySerializedAs("successWindow")]
        [Space]
        [SerializeField] private RestWindow restWindow;

        private ISceneProviderService _sceneProviderService;
        
        public CurrentStatFillView PlayerHealthFill => playerHealthFill;
        public CurrentStatFillView PlayerStaminaFill => playerStaminaFill;

        public TMP_Text NameLevelText => nameLevelText;
        public LevelDungeonProgressBar LevelDungeonProgressBar => levelDungeonProgressBar;

        public void Construct(ISceneProviderService sceneProviderService)
        {
            _sceneProviderService = sceneProviderService;
        }

        public void Initialize(IStaticDataService staticDataService,
            IProgressDungeonService progressDungeonService,
            IProcessingAdsService processingAdsService,
            IProgressProviderService progressProviderService,
            IStatsRestorationService statsRestorationService,
            PlayerData playerData)
        {
            levelNumberView.Initialize(progressProviderService);
            
            experienceBarView.Construct(staticDataService, progressProviderService);
            experienceBarView.Initialize();
            
            restWindow.Construct(statsRestorationService, playerData);
            restWindow.Initialize(
                progressDungeonService,
                processingAdsService,
                progressDungeonService.StartBattle,
                progressDungeonService.NextLevel, 
                LoadMainScene);
        }
        
        public void LoadMainScene()
        {
            _sceneProviderService.LoadMainScene();
        }
    }
}
