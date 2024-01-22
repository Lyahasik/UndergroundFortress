using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private Transform bagListTransform;
        [SerializeField] private ActiveArea bagActiveArea;
        [SerializeField] private CellBagView prefabCellBagView;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private IMovingItemService _movingItemService;

        private List<CellBagView> _cellsBag;

        public ActiveArea BagActiveArea => bagActiveArea;
        
        private void OnEnable()
        {
            if (_inventoryService != null)
                FillBag();
        }

        public void Construct(IStaticDataService staticDataService,
            IInventoryService inventoryService,
            IMovingItemService movingItemService)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
            _movingItemService = movingItemService;
        }

        public void Initialize()
        {
            _cellsBag = new List<CellBagView>();
            
            bagActiveArea.Construct(_movingItemService);
            
            FillBag();
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
        }

        private void FillBag()
        {
            List<CellData> bag = _inventoryService.Bag;
            IncreaseBagSizeView(bag.Count);

            for (int i = 0; i < bag.Count; i++)
            {
                ItemData itemData = bag[i].ItemData;
                
                if (itemData != null)
                    _cellsBag[i].SetValues(_staticDataService.GetItemIcon(itemData.Id), itemData.Quality, bag[i].Number);
                else
                    _cellsBag[i].Reset();
            }
        }

        private void IncreaseBagSizeView(int newSize)
        {
            if (_cellsBag.Count >= newSize)
                return;
            
            for (int i = _cellsBag.Count; i < newSize; i++)
            {
                CellBagView cellBagView = Instantiate(prefabCellBagView, bagListTransform);
                cellBagView.Construct(i, _movingItemService);
                cellBagView.Initialize();
                cellBagView.Subscribe(bagActiveArea);
                _cellsBag.Add(cellBagView);
            }
        }
    }
}
