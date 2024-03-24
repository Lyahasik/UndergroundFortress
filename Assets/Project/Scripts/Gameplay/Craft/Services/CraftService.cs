using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.Craft.Recipe;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public class CraftService : ICraftService
    {
        private readonly IInventoryService _inventoryService;
        private readonly IItemsGeneratorService _itemsGeneratorService;

        public CraftService(IInventoryService inventoryService,
            IItemsGeneratorService itemsGeneratorService)
        {
            _inventoryService = inventoryService;
            _itemsGeneratorService = itemsGeneratorService;
        }

        public EquipmentData TryCreateEquipment(int itemId,
            int currentLevel,
            int moneyPrice,
            ListPrice listPrice,
            ItemData crystal = null,
            bool isEnoughResources = true)
        {
            EquipmentData equipmentData = _itemsGeneratorService.GenerateEquipment(itemId, currentLevel, GetStatTypeByItem(crystal));

            if (equipmentData == null)
                return null;
            
            if (isEnoughResources)
                PayPrice(moneyPrice, listPrice, crystal);
            else
                PayPriceWithSurcharge(moneyPrice, listPrice, crystal);

            return equipmentData;
        }

        public ResourceData TryCreateResource(int itemId,
            int moneyPrice,
            ListPrice listPrice,
            bool isEnoughResources = true)
        {
            ResourceData resourceData = _itemsGeneratorService.GenerateResourceById(itemId);

            if (resourceData == null)
                return null;
            
            if (isEnoughResources)
                PayPrice(moneyPrice, listPrice);
            else
                PayPriceWithSurcharge(moneyPrice, listPrice);

            return resourceData;
        }

        private StatType GetStatTypeByItem(ItemData itemData)
        {
            if (itemData == null)
                return StatType.Empty;
            
            switch (itemData.Type)
            {
                case ItemType.ResourceDodgeSet:
                    return StatType.Dodge;
                case ItemType.ResourceCritSet:
                    return StatType.Crit;
                case ItemType.ResourceBlockSet:
                    return StatType.Block;
                case ItemType.ResourceStunSet:
                    return StatType.Stun;
            }
            
            return StatType.Empty;
        }

        private void PayPrice(int moneyPrice, ListPrice listPrice, ItemData crystal = null)
        {
            if (crystal != null)
                _inventoryService.RemoveItemsByType(crystal.Type, 1);
            
            _inventoryService.WalletOperationService.RemoveMoney1(moneyPrice);
            listPrice.PriceResources.ForEach(data => _inventoryService.RemoveItemsById(data.ItemId, data.Required));
        }

        private void PayPriceWithSurcharge(int moneyPrice, ListPrice listPrice, ItemData crystal = null)
        {
            if (crystal != null)
                _inventoryService.RemoveItemsByType(crystal.Type, 1);
            
            var wallet = _inventoryService.WalletOperationService;
            if (!wallet.IsEnoughMoney(moneyPrice))
                wallet.RemoveMoney1(wallet.Money1);
            
            listPrice.PriceResources.ForEach(data =>
            {
                if (data.IsEnough)
                    return;
                
                var numberItems = _inventoryService.GetNumberItemsById(data.ItemId);
                if (numberItems > 0)
                    _inventoryService.RemoveItemsById(data.ItemId, numberItems);
            });
        }
    }
}