using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Shop;

namespace UndergroundFortress.UI.Inventory
{
    public class CellPurchaseView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject numberObject;
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private PriceMoneyView priceMoneyView;
        
        [Space]
        [SerializeField] private Vector2 rectSize;
        [SerializeField] private BuyButton buyButton;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private IShoppingService _shoppingService;
        
        private RectTransform _rect;

        private int _purchaseId;
        private MoneyType _moneyType;

        public Vector2 RectSize => rectSize;
        
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

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
            gameObject.name = nameof(CellPurchaseView) + _purchaseId;

            priceMoneyView.Construct(_staticDataService);
            
            buyButton.Subscribe(ShowPurchase);
        }
        
        public void SetValues(PurchaseStaticData purchaseStaticData)
        {
            _purchaseId = purchaseStaticData.id;
            _moneyType = purchaseStaticData.moneyType;

            icon.sprite = purchaseStaticData.icon;
            numberText.text = purchaseStaticData.items[0].number.ToString();
            numberObject.SetActive(purchaseStaticData.items.Count == 1);
            priceMoneyView.SetValues(purchaseStaticData);
            
            buyButton.UpdateText(purchaseStaticData.moneyType == MoneyType.Ads);
        }

        public void Subscribe(ActiveArea activeArea)
        {
            activeArea.OnUp += Hit;
        }

        private void Hit(Vector3 position)
        {
            if (!_rect.IsDotInside(position))
                return;

            ShowPurchase();
        }

        private void ShowPurchase()
        {
            _shoppingService.ShowPurchase(this);
        }
    }
}