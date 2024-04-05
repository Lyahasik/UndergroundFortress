using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

using UndergroundFortress.Core.Converters;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class SaleResourceView : ResourceView
    {
        [Space]
        [SerializeField] private TMP_Text priceText;
        
        [Space]
        [SerializeField] private Slider numberSlider;
        [SerializeField] private TMP_Text maxNumberText;
        [SerializeField] private TMP_Text selectedPriceText;
        [SerializeField] private Button saleButton;

        private IShoppingService _shoppingService;

        private CellSaleView _selectedCell;
        private int _currentPrice;
        
        private int _selectedPrice;
        private int _selectedNumber;

        public void Construct(IStaticDataService staticDataService,
            IShoppingService shoppingService)
        {
            base.Construct(staticDataService);

            _shoppingService = shoppingService;
        }

        public void Initialize(UnityAction onClose)
        {
            numberSlider.onValueChanged.AddListener(UpdateNumberSlider);
            
            saleButton.onClick.AddListener(onClose);
            saleButton.onClick.AddListener(SaleResource);
        }

        public void Show(CellSaleView cellSale)
        {
            _selectedCell = cellSale;
            
            var resourceStaticData = _staticDataService.GetResourceById(_selectedCell.ItemData.Id);
            _currentPrice = CalculatePrice(resourceStaticData);
            priceText.text = _currentPrice.ToString();

            maxNumberText.text = _selectedCell.Number.ToString();
            numberSlider.maxValue = _selectedCell.Number;
            numberSlider.value = numberSlider.maxValue;
            
            base.Show(_selectedCell.ItemData);
        }
        
        private int CalculatePrice(ResourceStaticData resourceStaticData)
        {
            var recipeStaticData = _staticDataService.GetRecipeById(resourceStaticData.id);

            if (recipeStaticData == null)
                return CurrencyConverter.PriceTimeToMoney1(resourceStaticData.priceTime);

            int price = recipeStaticData.money1;
            recipeStaticData.resourcesPrice.ForEach(data =>
            {
                int priceMoney =
                    CurrencyConverter.PriceTimeToMoney1(_staticDataService.GetResourceById(data.idItem).priceTime);
                price += data.required * priceMoney;
            });

            return price;
        }

        private void UpdateNumberSlider(float value)
        {
            _selectedNumber = (int) value;
            _selectedPrice = _selectedNumber * _currentPrice;
            selectedPriceText.text = _selectedPrice.ToString();
        }

        private void SaleResource()
        {
            _shoppingService.SaleResource(_selectedCell, _selectedNumber, _selectedPrice);
        }

        private void Reset()
        {
            _selectedCell = null;
            
            base.Reset();
        }
    }
}