using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Core.Converters;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.UI.Craft.Recipe
{
    public class ListPrice : MonoBehaviour
    {
        [SerializeField] private List<PriceResource> priceResources;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private RecipeStaticData _recipeStaticData;
        private int _currentLevel;

        private List<PriceResource> MissingResources => priceResources.Where(data => data.IsInit && !data.IsEnough).ToList();

        public List<PriceResource> PriceResources => priceResources;
        public bool IsEnough => priceResources.All(data => !data.IsInit || data.IsEnough);

        public void Construct(IStaticDataService staticDataService,
            IInventoryService inventoryService,
            RecipeStaticData recipeStaticData)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
            _recipeStaticData = recipeStaticData;
        }

        public void Initialize(PlayerLevelData playerLevelData)
        {
            _currentLevel = _recipeStaticData.levelsPrice.Count > 1 ? playerLevelData.Level % 5 : 0;
            
            FillPrices();
        }

        public void UpdateCurrentResources()
        {
            for (int i = 0; i < _recipeStaticData.levelsPrice[_currentLevel].resourcesPrice.Count; i++)
            {
                priceResources[i].UpdateInStock(
                    _inventoryService.GetNumberItemsById(_recipeStaticData.levelsPrice[_currentLevel].resourcesPrice[i].itemStaticData.id));
            }
        }

        public int TotalPriceTime()
        {
            int totalPrice = 0;

            if (_recipeStaticData.levelsPrice[_currentLevel].money1 > _inventoryService.WalletOperationService.Money1)
                totalPrice += CurrencyConverter.Money1ToPriceTime(_recipeStaticData.levelsPrice[_currentLevel].money1 - _inventoryService.WalletOperationService.Money1);
            
            foreach (PriceResource resource in MissingResources)
            {
                int missingNumber = resource.Required - resource.InStock;
                totalPrice += missingNumber * _staticDataService.GetResourceById(resource.ItemId).priceTime;
            }

            return totalPrice;
        }

        private void FillPrices()
        {
            priceResources.ForEach(data => data.gameObject.SetActive(false));

            for (int i = 0; i < _recipeStaticData.levelsPrice[_currentLevel].resourcesPrice.Count; i++)
            {
                PriceResourceData priceResource = _recipeStaticData.levelsPrice[_currentLevel].resourcesPrice[i];
                
                Sprite icon = _staticDataService
                    .ForResources()
                    .Find(data => data.id == priceResource.itemStaticData.id)
                    .icon;
                
                priceResources[i].Init(priceResource.itemStaticData.id, icon, priceResource.required);
                priceResources[i].gameObject.SetActive(true);
            }

            UpdateCurrentResources();
        }
    }
}
