using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.Information;

namespace UndergroundFortress.UI.Craft
{
    public class CraftView : MonoBehaviour
    {
        [SerializeField] private Button buttonStartCraft;
        [SerializeField] private RectTransform itemWindow;
        [SerializeField] private ListRecipesView listRecipesView;

        private IStaticDataService _staticDataService;
        private IProgressProviderService _progressProviderService;
        private ICraftService _craftService;
        private InformationView _informationView;

        private int _idItem = 1;
        private StatType _statType = StatType.Empty;

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
            listRecipesView.Construct(_staticDataService, _progressProviderService);
            listRecipesView.Initialize();
            
            buttonStartCraft.onClick.AddListener(CreateEquipment);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        private void CreateEquipment()
        {
            EquipmentStaticData equipmentStaticData =
                _staticDataService.ForEquipments().Find(v => v.id == _idItem);
            
            EquipmentData equipment = _craftService.CreateEquipment(
                equipmentStaticData,
                _progressProviderService.ProgressData.Level,
                _statType);
            
            _informationView.ShowEquipment(equipment, itemWindow.position);
        }
    }
}