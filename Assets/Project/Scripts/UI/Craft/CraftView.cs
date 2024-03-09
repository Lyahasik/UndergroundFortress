using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Craft.Recipe;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Craft
{
    public class CraftView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;
        
        [Space]
        [SerializeField] private Image iconItem;

        [Space]
        [SerializeField] private AdditionalStatDropdown additionalStatDropdown;
        [SerializeField] private Button buttonStartCraft;

        [Space]
        [SerializeField] private List<ItemGroupButton> itemGroupButtons;
        [SerializeField] private List<ItemTypeButton> itemTypeButtons;
        [SerializeField] private ListRecipesView listRecipesView;

        private IStaticDataService _staticDataService;
        private IProgressProviderService _progressProviderService;
        private ICraftService _craftService;
        private InformationView _informationView;

        private int _idItem;
        private ItemGroupType _currentGroupType;
        private ItemType _itemType;
        private int _moneyPrice;
        private ListPrice _listPrice;

        public void Construct(IStaticDataService staticDataService, 
            IProgressProviderService progressProviderService,
            ICraftService craftService,
            InformationView informationView)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _craftService = craftService;
            _informationView = informationView;
        }

        public void Initialize(IInventoryService inventoryService)
        {
            additionalStatDropdown.Initialize(_staticDataService);
            
            listRecipesView.Construct(this, _staticDataService, inventoryService, _progressProviderService);
            listRecipesView.Initialize();

            foreach (ItemGroupButton itemGroupButton in itemGroupButtons) 
                itemGroupButton.Construct(this);

            foreach (ItemTypeButton itemTypeButton in itemTypeButtons) 
                itemTypeButton.Construct(_staticDataService, listRecipesView);

            UpdateGroupItems(ItemGroupType.Alchemy);

            buttonStartCraft.onClick.AddListener(CreateItem);
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
            
            if (type == windowType)
                UpdateGroupItems();
        }

        public void UpdateGroupItems(ItemGroupType groupType = ItemGroupType.Empty)
        {
            if (groupType != ItemGroupType.Empty)
                _currentGroupType = groupType;
            
            foreach (ItemGroupButton itemGroupButton in itemGroupButtons) 
                itemGroupButton.Change(_currentGroupType);
            
            foreach (ItemTypeButton itemTypeButton in itemTypeButtons)
                itemTypeButton.Hide();

            switch (_currentGroupType)
            {
                case ItemGroupType.Alchemy:
                    PrepareAlchemyLists();
                    break;
                case ItemGroupType.Weapons:
                    PrepareWeaponsLists();
                    break;
                case ItemGroupType.Armors:
                    PrepareArmorsLists();
                    break;
            }
        }

        public void SetRecipe(Sprite icon, int idItem, ItemType itemType, int moneyPrice, ListPrice listPrice)
        {
            _idItem = idItem;
            _itemType = itemType;
            iconItem.sprite = icon;
            _moneyPrice = moneyPrice;
            _listPrice = listPrice;

            UpdateCraftState(true);
        }

        public void UpdateCraftState(bool isReady) => 
            buttonStartCraft.interactable = isReady;

        private void CreateItem()
        {
            if (_itemType.IsEquipment()) 
                CreateEquipment();
            else if (_itemType.IsResource())
                CreateResource();
        }

        private void CreateEquipment()
        {
            EquipmentStaticData equipmentStaticData =
                _staticDataService.ForEquipments().Find(v => v.id == _idItem);

            _craftService.TryCreateEquipment(
                equipmentStaticData,
                _progressProviderService.ProgressData.Level,
                _moneyPrice,
                _listPrice,
                additionalStatDropdown.CurrentStatType);
        }

        private void CreateResource()
        {
            ResourceStaticData resourceStaticData =
                _staticDataService.ForResources().Find(v => v.id == _idItem);

            _craftService.TryCreateResource(resourceStaticData, _moneyPrice, _listPrice);
        }

        private void PrepareAlchemyLists()
        {
            itemTypeButtons[0].UpdateType(ItemType.Consumable);

            itemTypeButtons[0].ActivateTypeList();
        }

        private void PrepareWeaponsLists()
        {
            itemTypeButtons[0].UpdateType(ItemType.Sword);
            itemTypeButtons[1].UpdateType(ItemType.Shield);
            itemTypeButtons[2].UpdateType(ItemType.TwoHandedWeapon);
            itemTypeButtons[3].UpdateType(ItemType.Dagger);
            itemTypeButtons[4].UpdateType(ItemType.Mace);
            
            itemTypeButtons[0].ActivateTypeList();
        }

        private void PrepareArmorsLists()
        {
            itemTypeButtons[0].UpdateType(ItemType.Chest);
            itemTypeButtons[1].UpdateType(ItemType.Gloves);
            itemTypeButtons[2].UpdateType(ItemType.Pants);
            itemTypeButtons[3].UpdateType(ItemType.Boots);
            
            itemTypeButtons[0].ActivateTypeList();
        }
    }
}