using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.Gameplay.Items.Services
{
    public class ItemsGeneratorService : IItemsGeneratorService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IInventoryService _inventoryService;

        public ItemsGeneratorService(IStaticDataService staticDataService,
            IInventoryService inventoryService)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
        }

        public void GenerateResource()
        {
            List<ResourceStaticData> resources = _staticDataService.ForResources();

            int id = Random.Range(0, resources.Count);
            CreateResource(resources[id]);
        }

        public void GenerateResource(int id)
        {
            List<ResourceStaticData> resources = _staticDataService.ForResources();

            CreateResource(resources.Find(resource => resource.id == id));
        }

        private void CreateResource(ResourceStaticData resourceStaticData)
        {
            if (_inventoryService.IsBagFullForResource(resourceStaticData.type, resourceStaticData.id))
                return;

            ResourceData resourceData = new ResourceData(
            resourceStaticData.id,
            resourceStaticData.type,
            resourceStaticData.name,
            resourceStaticData.description,
            resourceStaticData.quality,
            resourceStaticData.icon,
            resourceStaticData.maxNumberForCell);
            
            _inventoryService.AddItem(resourceData);
        }
    }
}