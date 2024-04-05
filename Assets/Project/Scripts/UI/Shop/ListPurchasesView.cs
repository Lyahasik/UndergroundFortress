using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.Craft
{
    public class ListPurchasesView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private ActiveArea activeArea;
        
        [Space]
        [SerializeField] private CellSaleView prefabCellSaleView;
        [SerializeField] private CellSaleView prefabCellPurchaseView;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private IShoppingService _shoppingService;
        
        private List<CellSaleView> _cellsSale;

        public void Construct(IStaticDataService staticDataService,
            IInventoryService inventoryService,
            IShoppingService shoppingService)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
            _shoppingService = shoppingService;
        }

        public void Initialize()
        {
            _cellsSale = new List<CellSaleView>();
            
            FillSales();
        }
        
        public void FillSales()
        {
            List<CellData> saleBag = _inventoryService.Bag;
            IncreaseShopSizeView(saleBag.Count);

            for (int i = 0; i < saleBag.Count; i++)
            {
                if (saleBag[i].ItemData != null)
                    _cellsSale[i].SetValues(saleBag[i], InventoryCellType.Bag);
                else
                    _cellsSale[i].Reset();
            }
        }

        private void IncreaseShopSizeView(int newSize)
        {
            if (_cellsSale.Count >= newSize)
                return;
            
            gridLayoutGroup.cellSize = prefabCellSaleView.RectSize;
            
            for (int i = _cellsSale.Count; i < newSize; i++)
            {
                CellSaleView cellSaleView = Instantiate(prefabCellSaleView, transform);
                cellSaleView.Construct(i, _staticDataService, _inventoryService, _shoppingService);
                cellSaleView.Initialize();
                cellSaleView.Subscribe();
                cellSaleView.Subscribe(activeArea);
                _cellsSale.Add(cellSaleView);
            }
        }
    }
}