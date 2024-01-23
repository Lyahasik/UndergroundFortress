using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.StaticData;
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
        [SerializeField] private RectTransform itemWindow;
        [SerializeField] private ListRecipesView listRecipesView;

        private IStaticDataService _staticDataService;
        private IProgressProviderService _progressProviderService;
        private ICraftService _craftService;
        private InformationView _informationView;

        private int _idItem;

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

        public void Initialize()
        {
            additionalStatDropdown.Initialize(_staticDataService);
            
            listRecipesView.Construct(this, _staticDataService, _progressProviderService);
            listRecipesView.Initialize();
            
            buttonStartCraft.onClick.AddListener(CreateEquipment);
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
        }

        public void SetRecipe(Sprite icon, int idItem)
        {
            _idItem = idItem;
            iconItem.sprite = icon;

            UpdateCraftState(true);
        }

        public void UpdateCraftState(bool isReady)
        {
            buttonStartCraft.interactable = isReady;
        }

        private void CreateEquipment()
        {
            EquipmentStaticData equipmentStaticData =
                _staticDataService.ForEquipments().Find(v => v.id == _idItem);

            _craftService.CreateEquipment(
                equipmentStaticData,
                _progressProviderService.ProgressData.Level,
                additionalStatDropdown.CurrentStatType);
        }
    }
}