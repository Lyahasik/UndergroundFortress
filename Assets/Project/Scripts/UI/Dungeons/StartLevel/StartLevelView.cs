﻿using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress.UI.MainMenu
{
    public class StartLevelView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private List<ListLevelsDungeon> listDungeons;

        private ISceneProviderService _sceneProviderService;
        private IItemsGeneratorService _itemsGeneratorService;
        private IInventoryService _inventoryService;
        private ISkillsUpgradeService _skillsUpgradeService;
        private IProcessingBonusesService _processingBonusesService;
        private IActivationRecipesService _activationRecipesService;
        private IProgressTutorialService _progressTutorialService;

        private int _selectedDungeonId;

        public void Construct(ISceneProviderService sceneProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService,
            ISkillsUpgradeService skillsUpgradeService,
            IProcessingBonusesService processingBonusesService,
            IActivationRecipesService activationRecipesService,
            IProgressTutorialService progressTutorialService)
        {
            _sceneProviderService = sceneProviderService;
            _itemsGeneratorService = itemsGeneratorService;
            _inventoryService = inventoryService;
            _skillsUpgradeService = skillsUpgradeService;
            _processingBonusesService = processingBonusesService;
            _activationRecipesService = activationRecipesService;
            _progressTutorialService = progressTutorialService;
        }

        public void Initialize(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProgressProviderService progressProviderService)
        {
            listDungeons.ForEach(data =>
            {
                data.Construct(staticDataService, localizationService);
                data.Initialize(progressProviderService, UpdateSelectDungeon, StartLevel);
            });
        }
        
        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);

            if (type != windowType)
                listDungeons.ForEach(data => { data.Reset(); });
        }

        private void UpdateSelectDungeon(int id)
        {
            _selectedDungeonId = id;
            
            listDungeons.ForEach(data => data.UpdateSelect(id));
        }

        private void StartLevel(int idLevel)
        {
            _sceneProviderService.LoadLevel(
                _itemsGeneratorService,
                _inventoryService,
                _skillsUpgradeService,
                _processingBonusesService,
                _activationRecipesService,
                _progressTutorialService,
                _selectedDungeonId,
                idLevel);
            
            listDungeons.ForEach(data => data.Reset());
            gameObject.SetActive(false);
        }
    }
}