using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.StaticData;

namespace UndergroundFortress.UI.Craft
{
    [RequireComponent(typeof(Button))]
    public class ItemTypeButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;

        private IStaticDataService _staticDataService;
        private ListRecipesView _listRecipesView;

        private ItemType _itemType;

        private void Awake()
        {
            button.onClick.AddListener(ActivateTypeList);
        }

        public void Construct(IStaticDataService staticDataService, ListRecipesView listRecipesView)
        {
            _staticDataService = staticDataService;
            _listRecipesView = listRecipesView;
        }

        public void UpdateType(in ItemType newType)
        {
            _itemType = newType;

            CraftItemData craftItemData
                = _staticDataService.ForUI().craftItemsData.Find(data => data.itemType == _itemType);
            icon.sprite = craftItemData.icon;
            
            Show();
        }

        public void Change(ItemType type) => 
            button.interactable = type != _itemType;

        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);

        public void ActivateTypeList()
        {
            _listRecipesView.FillList(_itemType);
        }
    }
}