using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Shop
{
    public class ShopView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;
        [SerializeField] private GameObject scrollHandleShop;
        
        [Space] 
        [SerializeField] private List<GroupPurchaseTypeButton> groupPurchasesButtons;

        [Space]
        [SerializeField] private ListPurchasesView listPurchases;

        private IInventoryService _inventoryService;
        
        private GroupPurchaseType _currentGroupType;

        private void OnEnable()
        {
            if (_inventoryService != null) 
                listPurchases.FillSales();
        }

        public void Construct(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void Initialize(IStaticDataService staticDataService,
        IShoppingService shoppingService)
        {
            scrollHandleShop.SetActive(true);
            
            foreach (GroupPurchaseTypeButton groupPurchaseTypeButton in groupPurchasesButtons) 
                groupPurchaseTypeButton.Construct(this);

            listPurchases.Construct(staticDataService, _inventoryService, shoppingService);
            listPurchases.Initialize();
            
            UpdateGroupPurchases(GroupPurchaseType.Sale);
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
            
            if (type == windowType)
                UpdateGroupPurchases();
        }

        public void UpdateGroupPurchases(GroupPurchaseType groupType = GroupPurchaseType.Empty)
        {
            if (groupType != GroupPurchaseType.Empty) 
                _currentGroupType = groupType;

            foreach (GroupPurchaseTypeButton groupPurchaseTypeButton in groupPurchasesButtons) 
                groupPurchaseTypeButton.Change(_currentGroupType);
            
            listPurchases.ActivatePurchaseGroup(_currentGroupType);
        }
    }
}