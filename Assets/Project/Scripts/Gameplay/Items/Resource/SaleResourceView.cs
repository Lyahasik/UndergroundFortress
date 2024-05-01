using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

using UndergroundFortress.Core.Converters;
using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class SaleResourceView : ResourceView, IReadingProgress
    {
        [Space]
        [SerializeField] private TMP_Text priceText;
        
        [Space]
        [SerializeField] private Slider numberSlider;
        [SerializeField] private TMP_Text currentNumberText;
        [SerializeField] private TMP_Text selectedPriceText;
        [SerializeField] private Button saleButton;

        private IShoppingService _shoppingService;
        private PlayerLevelData _levelData;

        private CellSaleView _selectedCell;
        private int _currentPrice;

        private int _selectedPrice;
        private int _selectedNumber;

        public void Construct(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IShoppingService shoppingService)
        {
            base.Construct(staticDataService, localizationService);

            _shoppingService = shoppingService;
        }

        public void Initialize(IProgressProviderService progressProviderService, UnityAction onClose)
        {
            numberSlider.onValueChanged.AddListener(UpdateNumberSlider);
            
            saleButton.onClick.AddListener(onClose);
            saleButton.onClick.AddListener(SaleResource);
            
            Register(progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _levelData = progress.LevelData;
        }

        public void UpdateProgress(ProgressData progress) {}

        public void Show(CellSaleView cellSale)
        {
            _selectedCell = cellSale;
            
            var resourceStaticData = _staticDataService.GetResourceById(_selectedCell.ItemData.Id);
            _currentPrice = CalculatePrice(resourceStaticData);
            priceText.text = _currentPrice.ToString();

            currentNumberText.text = _selectedCell.Number.ToString();
            numberSlider.maxValue = _selectedCell.Number;
            numberSlider.value = numberSlider.maxValue;
            
            base.Show(_selectedCell.ItemData);
        }

        private int CalculatePrice(ResourceStaticData resourceStaticData)
        {
            var recipeStaticData = _staticDataService.GetRecipeById(resourceStaticData.id);

            if (recipeStaticData == null)
                return CurrencyConverter.PriceTimeToMoney1(resourceStaticData.priceTime);

            int price = recipeStaticData.GetLevelPrice(_levelData.Level).money1;
            recipeStaticData.GetLevelPrice(_levelData.Level).resourcesPrice.ForEach(data =>
            {
                int priceMoney =
                    CurrencyConverter.PriceTimeToMoney1(_staticDataService.GetResourceById(data.itemStaticData.id).priceTime);
                price += data.required * priceMoney;
            });

            return price;
        }

        private void UpdateNumberSlider(float value)
        {
            _selectedNumber = (int) value;
            _selectedPrice = _selectedNumber * _currentPrice;
            
            currentNumberText.text = _selectedNumber.ToString();
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