using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProgressProviderService _progressProviderService;
        
        private List<CellData> _bag;

        public List<CellData> Bag => _bag;

        public InventoryService(IProgressProviderService progressProviderService)
        {
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            _bag = _progressProviderService.ProgressData.Bag;
        }

        public void AddItem(ItemData itemData)
        {
            if (itemData.Type == ItemType.Resource)
                AddResource(itemData);
            else
                AddNewItem(itemData);
        }

        public void SwapItems(in int id1, in int id2) => 
            (_bag[id1], _bag[id2]) = (_bag[id2], _bag[id1]);

        private void AddResource(ItemData itemData)
        {
            int itemBagId = GetResourceId(itemData);

            if (itemBagId != ConstantValues.ERROR_ID)
                _bag[itemBagId].Number++;
            else
                AddNewItem(itemData);
        }

        private int GetResourceId(ItemData itemData)
        {
            for (int i = 0; i < _bag.Count; i++)
            {
                if (_bag[i].ItemData == null)
                    continue;
                    
                if (_bag[i].ItemData.Id == itemData.Id
                    && _bag[i].Number < itemData.MaxNumberForCell)
                {
                    return i;
                }
            }

            return ConstantValues.ERROR_ID;
        }

        private void AddNewItem(ItemData itemData)
        {
            for (int i = 0; i < _bag.Count; i++)
            {
                if (_bag[i].ItemData == null)
                {
                    _bag[i].ItemData = itemData;
                    _bag[i].Number = 1;
                    return;
                }
            }
            
            Debug.LogWarning($"Failed to add item to bag");
        }
    }
}