using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Skills.Services;

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

        private int _selectedDungeonId;

        public void Construct(ISceneProviderService sceneProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService,
            ISkillsUpgradeService skillsUpgradeService)
        {
            _sceneProviderService = sceneProviderService;
            _itemsGeneratorService = itemsGeneratorService;
            _inventoryService = inventoryService;
            _skillsUpgradeService = skillsUpgradeService;
        }

        public void Initialize(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            listDungeons.ForEach(data =>
            {
                data.Construct(staticDataService);
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
                _selectedDungeonId,
                idLevel);
            
            listDungeons.ForEach(data => data.Reset());
            gameObject.SetActive(false);
        }
    }
}