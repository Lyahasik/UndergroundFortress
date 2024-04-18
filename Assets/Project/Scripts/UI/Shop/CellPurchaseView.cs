using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Shop;

namespace UndergroundFortress.UI.Inventory
{
    public class CellPurchaseView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject numberLevelView;
        [SerializeField] private TMP_Text numberLevelText;
        [SerializeField] private GameObject numberObject;
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private PriceMoneyView priceMoneyView;
        
        [Space]
        [SerializeField] private Vector2 rectSize;
        [SerializeField] private BuyButton buyButton;

        private IStaticDataService _staticDataService;
        private IShoppingService _shoppingService;

        private ActiveArea _activeArea;
        private RectTransform _rect;

        private PurchaseStaticData _purchaseStaticData;

        public Vector2 RectSize => rectSize;
        
        public PurchaseStaticData PurchaseStaticData => _purchaseStaticData;
        
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void Construct(IStaticDataService staticDataService,
            IShoppingService shoppingService)
        {
            _staticDataService = staticDataService;
            _shoppingService = shoppingService;
        }
        
        public void Initialize()
        {
            priceMoneyView.Construct(_staticDataService);
            
            buyButton.Subscribe(ShowPurchase);
        }
        
        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void SetValues(PurchaseStaticData purchaseStaticData)
        {
            _purchaseStaticData = purchaseStaticData;
            
            gameObject.name = nameof(CellPurchaseView) + _purchaseStaticData.id;

            icon.sprite = purchaseStaticData.icon;

            if (purchaseStaticData.rewardData.items.Count + purchaseStaticData.rewardData.moneys.Count == 1
                && purchaseStaticData.rewardData.items.Count > 0)
            {
                numberLevelView.SetActive(purchaseStaticData.rewardData.items[0].level > 0);
                numberLevelText.text = purchaseStaticData.rewardData.items[0].level.ToString();
            }
            else
            {
                numberLevelView.SetActive(false);
            }
            
            numberText.text = purchaseStaticData.rewardData.moneys.Count > 0 
                ? purchaseStaticData.rewardData.moneys[0].number.ToString()
                : purchaseStaticData.rewardData.items[0].number.ToString();
            numberObject.SetActive(purchaseStaticData.rewardData.items.Count + purchaseStaticData.rewardData.moneys.Count == 1);
            priceMoneyView.SetValues(purchaseStaticData);
            
            buyButton.UpdateText(_purchaseStaticData.moneyType == MoneyType.Ads);
        }

        public void Subscribe(ActiveArea activeArea)
        {
            _activeArea = activeArea;
            activeArea.OnUp += Hit;
        }

        private void Unsubscribe()
        {
            _activeArea.OnUp -= Hit;
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