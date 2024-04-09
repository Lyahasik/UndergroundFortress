using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.Shop;

namespace UndergroundFortress.UI.Craft
{
    public class ListPurchasesView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private ActiveArea activeArea;
        
        [Space]
        [SerializeField] private CellSaleView prefabCellSaleView;
        [SerializeField] private CellPurchaseView prefabCellPurchaseView;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private IShoppingService _shoppingService;
        
        private List<CellSaleView> _cellsSale;
        private List<CellPurchaseView> _cellsPurchase;

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
            _cellsPurchase = new List<CellPurchaseView>();
            
            FillSales();
        }

        public void ActivatePurchaseGroup(GroupPurchaseType groupPurchaseType)
        {
            if (groupPurchaseType == GroupPurchaseType.Sale)
            {
                FillSales();
            }
            else
            {
                switch (groupPurchaseType)
                {
                    case GroupPurchaseType.Money1:
                        FillPurchases(MoneyType.Money1);
                        break;
                    case GroupPurchaseType.Money2:
                        FillPurchases(MoneyType.Money2);
                        break;
                    case GroupPurchaseType.Money3:
                        FillPurchases(MoneyType.Money3);
                        break;
                    case GroupPurchaseType.Ads:
                        FillPurchases(MoneyType.Ads);
                        break;
                }
            }
        }
        
        public void FillSales()
        {
            ResetLists();
            
            List<CellData> saleBag = _inventoryService.Bag;
            IncreaseSaleBagSizeView(saleBag.Count);

            for (int i = 0; i < saleBag.Count; i++)
            {
                if (saleBag[i].ItemData != null)
                    _cellsSale[i].SetValues(saleBag[i], InventoryCellType.Bag);
                else
                    _cellsSale[i].Reset();
            }
        }

        public void FillPurchases(MoneyType moneyType)
        {
            ResetLists();
            
            List<PurchaseStaticData> purchaseBag = _staticDataService.ForPurchasesByMoneyType(moneyType);
            IncreasePurchaseBagSizeView(purchaseBag.Count);

            for (int i = 0; i < purchaseBag.Count; i++)
            {
                _cellsPurchase[i].SetValues(purchaseBag[i]);
            }
        }

        private void ResetLists()
        {
            _cellsSale.ForEach(data => Destroy(data.gameObject));
            _cellsSale.Clear();
            
            _cellsPurchase.ForEach(data => Destroy(data.gameObject));
            _cellsPurchase.Clear();
        }

        private void IncreaseSaleBagSizeView(int newSize)
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

        private void IncreasePurchaseBagSizeView(int newSize)
        {
            if (_cellsPurchase.Count >= newSize)
                return;
            
            gridLayoutGroup.cellSize = prefabCellPurchaseView.RectSize;
            
            for (int i = _cellsSale.Count; i < newSize; i++)
            {
                CellPurchaseView cellPurchaseView = Instantiate(prefabCellPurchaseView, transform);
                cellPurchaseView.Construct(_staticDataService, _inventoryService, _shoppingService);
                cellPurchaseView.Initialize();
                cellPurchaseView.Subscribe(activeArea);
                _cellsPurchase.Add(cellPurchaseView);
            }
        }
    }
}