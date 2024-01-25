using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Inventory
{
    public class InventoryView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private Transform bagListTransform;
        [SerializeField] private ActiveArea bagActiveArea;
        [SerializeField] private CellInventoryView prefabCellInventoryView;
        
        [Space]
        [SerializeField] private List<CellInventoryView> cellsEquipment;
        [SerializeField] private ActiveArea equipmentActiveArea;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private IMovingItemService _movingItemService;
        private InformationView _informationView;

        private List<CellInventoryView> _cellsBag;

        public ActiveArea BagActiveArea => bagActiveArea;
        public ActiveArea EquipmentActiveArea => equipmentActiveArea;
        
        private void OnEnable()
        {
            if (_inventoryService != null) 
                FillBag();
        }

        public void Construct(IStaticDataService staticDataService,
            IInventoryService inventoryService,
            IMovingItemService movingItemService,
            InformationView informationView)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
            _movingItemService = movingItemService;
            _informationView = informationView;
        }

        public void Initialize()
        {
            _cellsBag = new List<CellInventoryView>();
            
            bagActiveArea.Construct(_movingItemService);
            equipmentActiveArea.Construct(_movingItemService);
            
            FillBag();
            FillEquipment();
        }

        public void ActivationUpdate(WindowType type) => 
            gameObject.SetActive(type == windowType);

        private void FillEquipment()
        {
            List<CellData> equipment = _inventoryService.Equipment;

            for (int i = 0; i < equipment.Count; i++)
            {
                cellsEquipment[i].Construct(i, _staticDataService, _movingItemService, _informationView);
                cellsEquipment[i].Subscribe(_inventoryService, bagActiveArea);
                cellsEquipment[i].Subscribe(_inventoryService, equipmentActiveArea);
                
                ItemData itemData = equipment[i].ItemData;
                if (itemData != null
                    && itemData.Id != ConstantValues.ERROR_ID)
                    cellsEquipment[i].SetValues(equipment[i], InventoryCellType.Equipment);
                else
                    cellsEquipment[i].Reset(InventoryCellType.Equipment);
            }
        }

        private void FillBag()
        {
            List<CellData> bag = _inventoryService.Bag;
            IncreaseBagSizeView(bag.Count);

            for (int i = 0; i < bag.Count; i++)
            {
                if (bag[i].ItemData != null)
                    _cellsBag[i].SetValues(bag[i], InventoryCellType.Bag);
                else
                    _cellsBag[i].Reset();
            }
        }

        private void IncreaseBagSizeView(int newSize)
        {
            if (_cellsBag.Count >= newSize)
                return;
            
            for (int i = _cellsBag.Count; i < newSize; i++)
            {
                CellInventoryView cellInventoryView = Instantiate(prefabCellInventoryView, bagListTransform);
                cellInventoryView.Construct(i, _staticDataService, _movingItemService, _informationView);
                cellInventoryView.Initialize();
                cellInventoryView.Subscribe(_inventoryService, bagActiveArea);
                cellInventoryView.Subscribe(_inventoryService, equipmentActiveArea);
                _cellsBag.Add(cellInventoryView);
            }
        }
    }
}
