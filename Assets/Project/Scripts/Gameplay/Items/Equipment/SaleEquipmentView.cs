using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UndergroundFortress.Core.Converters;
using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Items.Equipment
{
    public class SaleEquipmentView : EquipmentView, IReadingProgress
    {
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button saleButton;
        
        private IShoppingService _shoppingService;
        private PlayerLevelData _levelData;

        private CellSaleView _selectedCell;
        private int _currentPrice;

        public void Construct(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IShoppingService shoppingService)
        {
            base.Construct(staticDataService, localizationService);

            _shoppingService = shoppingService;
        }

        public void Initialize(IProgressProviderService progressProviderService, UnityAction onClose)
        {
            saleButton.onClick.AddListener(onClose);
            saleButton.onClick.AddListener(SaleEquipment);
            
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
            
            var equipmentStaticData = _staticDataService.GetEquipmentById(_selectedCell.ItemData.Id);
            _currentPrice = CalculatePrice(equipmentStaticData);
            priceText.text = _currentPrice.ToString();
            
            base.Show(_selectedCell.ItemData);
        }

        private int CalculatePrice(EquipmentStaticData equipmentStaticData)
        {
            var recipeStaticData = _staticDataService.GetRecipeById(equipmentStaticData.id);

            int price = recipeStaticData.GetLevelPrice(_levelData.Level).money1;
            recipeStaticData.GetLevelPrice(_levelData.Level).resourcesPrice.ForEach(data =>
            {
                int priceMoney =
                    CurrencyConverter.PriceTimeToMoney1(_staticDataService.GetResourceById(data.itemStaticData.id).priceTime);
                price += data.required * priceMoney;
            });

            ApplyPricePercentage(ref price);

            return price;
        }

        private void ApplyPricePercentage(ref int price)
        {
            var pricePercentage = 0f;

            if (_selectedCell.ItemData.MainStats.Count == 2)
                pricePercentage += _staticDataService
                    .GetQualityByType(_selectedCell.ItemData.MainStats[1].QualityType).pricePercentage;

            foreach (StatItemData statItemData in _selectedCell.ItemData.AdditionalStats)
                pricePercentage += _staticDataService.GetQualityByType(statItemData.QualityType).pricePercentage;

            price += (int) (price * pricePercentage);
        }

        private void SaleEquipment()
        {
            _shoppingService.SaleEquipment(_selectedCell, _currentPrice);
        }

        private void Reset()
        {
            _selectedCell = null;
            
            base.Reset();
        }
    }
}