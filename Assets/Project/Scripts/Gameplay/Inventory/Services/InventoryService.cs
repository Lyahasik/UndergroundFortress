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
        
        private Dictionary<int, CellData[]> _bags;

        public Dictionary<int, CellData[]> Bags => _bags;

        public InventoryService(IProgressProviderService progressProviderService)
        {
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            _bags = _progressProviderService.ProgressData.Bags;
        }

        public void AddItem(ItemData itemData)
        {
            if (itemData.Type == ItemType.Resource)
                AddResource(itemData);
            else
                AddNewItem(itemData);
        }

        private void AddResource(ItemData itemData)
        {
            ItemBagId itemBagId = GetResourceId(itemData);

            if (itemBagId.BagId != ConstantValues.EMPTY_ID)
                _bags[itemBagId.BagId][itemBagId.CellId].Number++;
            else
                AddNewItem(itemData);
            
            Debug.LogWarning($"Failed to add resource to bag");
        }

        private ItemBagId GetResourceId(ItemData itemData)
        {
            foreach (KeyValuePair<int,CellData[]> keyValuePair in _bags)
            {
                for (int i = 0; i < keyValuePair.Value.Length; i++)
                {
                    if (keyValuePair.Value[i].ItemData == null)
                        continue;
                    
                    if (keyValuePair.Value[i].ItemData.Id == itemData.Id
                        && keyValuePair.Value[i].Number < itemData.MaxNumberForCell)
                    {
                        return new ItemBagId(keyValuePair.Key, i);
                    }
                }
            }

            return new ItemBagId(0, 0);
        }

        private void AddNewItem(ItemData itemData)
        {
            foreach (KeyValuePair<int,CellData[]> keyValuePair in _bags)
            {
                for (int i = 0; i < keyValuePair.Value.Length; i++)
                {
                    if (keyValuePair.Value[i].ItemData == null)
                    {
                        keyValuePair.Value[i].ItemData = itemData;
                        keyValuePair.Value[i].Number = 1;
                        return;
                    }
                }
            }
            
            Debug.LogWarning($"Failed to add item to bag");
        }
    }
}