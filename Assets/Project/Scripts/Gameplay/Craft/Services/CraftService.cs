using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.Craft.Recipe;
using UndergroundFortress.UI.Inventory;

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
            StatType additionalStatType = StatType.Empty,
            ItemData crystal = null,
            bool isEnoughResources = true)
        {
            if (additionalStatType == StatType.Empty)
                additionalStatType = GetStatTypeByItem(crystal);
            EquipmentData equipmentData = _itemsGeneratorService.GenerateEquipment(itemId, currentLevel, additionalStatType);

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
                _inventoryService.RemoveItemsByType(InventoryCellType.Bag, crystal.Type, 1);
            
            _inventoryService.WalletOperationService.RemoveMoney1(moneyPrice);
            listPrice.PriceResources.ForEach(data => 
                _inventoryService.RemoveItemsById(InventoryCellType.Bag, data.ItemId, data.Required));
        }

        private void PayPriceWithSurcharge(int moneyPrice, ListPrice listPrice, ItemData crystal = null)
        {
            if (crystal != null)
                _inventoryService.RemoveItemsByType(InventoryCellType.Bag, crystal.Type, 1);
            
            var wallet = _inventoryService.WalletOperationService;
            if (!wallet.IsEnoughMoney(MoneyType.Money1, moneyPrice, false))
                wallet.RemoveMoney1(wallet.Money1);
            
            listPrice.PriceResources.ForEach(data =>
            {
                if (data.IsEnough)
                    return;
                
                var numberItems = _inventoryService.GetNumberItemsById(data.ItemId);
                if (numberItems > 0)
                    _inventoryService.RemoveItemsById(InventoryCellType.Bag, data.ItemId, numberItems);
            });
        }
    }
}