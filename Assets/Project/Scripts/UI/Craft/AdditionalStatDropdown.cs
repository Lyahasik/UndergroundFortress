using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.UI.Craft
{
    public class AdditionalStatDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        
        private IInventoryService _inventoryService;

        private List<TMP_Dropdown.OptionData> _options;
        private List<ItemData> _crystals;

        private ItemData _currentCrystal;

        public ItemData CurrentCrystal => _currentCrystal;

        public void Construct(IInventoryService inventoryService)
        {
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
            _options = new List<TMP_Dropdown.OptionData>();
            _crystals = new List<ItemData>();

            List<ItemData> crystals = _inventoryService.GetCrystals();

            AddCrystal(null);
            foreach (ItemData crystal in crystals)
                AddCrystal(crystal);

            dropdown.options = _options;
            dropdown.value = 0;

            dropdown.onValueChanged.AddListener(CrystalSelected);
        }

        private void AddCrystal(ItemData crystal)
        {
            //TODO locale
            _options.Add(crystal != null
                ? new TMP_Dropdown.OptionData(crystal.Name, crystal.Icon)
                : new TMP_Dropdown.OptionData("@without_crystal", null));
            
            _crystals.Add(crystal);
        }
    }
}