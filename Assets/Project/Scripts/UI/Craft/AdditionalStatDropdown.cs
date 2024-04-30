using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.UI.Craft
{
    public class AdditionalStatDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;

        private List<TMP_Dropdown.OptionData> _options;
        private List<ItemData> _crystals;

        private ItemData _currentCrystal;
        private int _newId;

        public ItemData CurrentCrystal => _currentCrystal;

        public void Construct(IStaticDataService staticDataService, IInventoryService inventoryService)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
        }

        public void Initialise()
        {
            Subscribe();
            UpdateValues();
        }

        private void Subscribe()
        {
            _inventoryService.OnUpdateResources += UpdateValues;
        }

        public void CrystalSelected(int id)
        {
            _currentCrystal = _crystals[id];
        }

        public void UpdateValues()
        {
            _newId = 0;
            
            _options = new List<TMP_Dropdown.OptionData>();
            _crystals = new List<ItemData>();

            List<ItemData> crystalsByType = _inventoryService.GetCrystals().Distinct().ToList();

            AddCrystal(null);
            foreach (ItemData crystal in crystalsByType)
                AddCrystal(crystal);

            dropdown.options = _options;
            dropdown.value = _newId;

            dropdown.onValueChanged.AddListener(CrystalSelected);
        }

        private void AddCrystal(ItemData crystal)
        {
            if (crystal == _currentCrystal)
                _newId = _options.Count;
            
            //TODO locale
            _options.Add(crystal != null
                ? new TMP_Dropdown.OptionData(crystal.Name, _staticDataService.GetItemIcon(crystal.Id))
                : new TMP_Dropdown.OptionData("@WithoutCrystal", null));
            
            _crystals.Add(crystal);
        }
    }
}