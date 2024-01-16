using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Inventory
{
    public class InventoryView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private List<CellBagView> cellsBag;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;

        private void OnEnable()
        {
            if (_inventoryService != null)
                FillBag(ConstantValues.FIRST_BAG_ID);
        }

        public void Construct(IStaticDataService staticDataService,
            IInventoryService inventoryService)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
        }

        public void Initialize()
        {
            FillBag(ConstantValues.FIRST_BAG_ID);
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
        }

        private void FillBag(int bagId)
        {
            CellData[] bag = _inventoryService.Bags[bagId];

            for (int i = 0; i < bag.Length; i++)
            {
                ItemData itemData = bag[i].ItemData;
                
                if (itemData != null)
                    cellsBag[i].SetValues(_staticDataService.GetItemIcon(itemData.Id), itemData.Quality, bag[i].Number);
                else
                    cellsBag[i].Reset();
            }
        }
    }
}
