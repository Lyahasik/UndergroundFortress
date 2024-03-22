using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.UI.Craft.Recipe
{
    public class ListPrice : MonoBehaviour
    {
        [SerializeField] private List<PriceResource> priceResources;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private RecipeStaticData _recipeStaticData;

        private List<PriceResource> MissingResources => priceResources.Where(data => data.IsInit && !data.IsEnough).ToList();

        public List<PriceResource> PriceResources => priceResources;
        public bool IsEnough => priceResources.All(data => !data.IsInit || data.IsEnough);

        public void Init()
        {
            FillPrices();
        }

        public void Construct(IStaticDataService staticDataService,
            IInventoryService inventoryService,
            RecipeStaticData recipeStaticData)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
            _recipeStaticData = recipeStaticData;
        }

        public void UpdateCurrentResources()
        {
            for (int i = 0; i < _recipeStaticData.resourcesPrice.Count; i++)
            {
                priceResources[i].UpdateInStock(
                    _inventoryService.GetNumberItemsById(_recipeStaticData.resourcesPrice[i].idItem));
            }
        }

        public int TotalPriceTime()
        {
            int totalPrice = 0;

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
            
            for (int i = 0; i < _recipeStaticData.resourcesPrice.Count; i++)
            {
                PriceResourceData priceResource = _recipeStaticData.resourcesPrice[i];
                
                Sprite icon = _staticDataService
                    .ForResources()
                    .Find(data => data.id == priceResource.idItem)
                    .icon;
                
                priceResources[i].Init(priceResource.idItem, icon, priceResource.required);
                priceResources[i].gameObject.SetActive(true);
            }

            UpdateCurrentResources();
        }
    }
}
