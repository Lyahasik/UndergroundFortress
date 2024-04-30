using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Dungeons.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
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
        [SerializeField] private ConsumableItemView consumableItemView;

        [Space]
        [SerializeField] private TMP_Text nameLevelText;
        [SerializeField] private LevelDungeonProgressBar levelDungeonProgressBar;

        [Space]
        [SerializeField] private RestWindow restWindow;

        [Space]
        [SerializeField] private UnlockByTutorialStage unlockByTutorialStage;
        
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
            IInventoryService inventoryService,
            IProgressDungeonService progressDungeonService,
            IProcessingAdsService processingAdsService,
            IProgressProviderService progressProviderService,
            IStatsRestorationService statsRestorationService,
            IAttackService attackService,
            PlayerData playerData)
        {
            levelNumberView.Initialize(progressProviderService);
            
            experienceBarView.Construct(staticDataService, progressProviderService);
            experienceBarView.Initialize();
            
            consumableItemView.Construct(staticDataService, inventoryService, statsRestorationService, attackService, playerData);
            consumableItemView.Initialize();
            
            restWindow.Construct(statsRestorationService, playerData);
            restWindow.Initialize(
                progressDungeonService,
                processingAdsService,
                progressDungeonService.StartBattle,
                progressDungeonService.NextLevel, 
                LoadMainScene);
            
            unlockByTutorialStage.Initialize(progressProviderService);
        }
        
        public void LoadMainScene()
        {
            _sceneProviderService.LoadMainScene();
        }
    }
}
