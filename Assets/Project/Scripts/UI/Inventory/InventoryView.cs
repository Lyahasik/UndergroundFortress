using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Tutorial.Services;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Inventory
{
    public class InventoryView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;
        [SerializeField] private GameObject scrollHandleBag;

        [Space]
        [SerializeField] private Transform bagListTransform;
        [SerializeField] private ActiveArea bagActiveArea;
        [SerializeField] private TrashBin trashBin;
        [SerializeField] private CellInventoryView prefabCellInventoryView;
        
        [Space]
        [SerializeField] private List<CellInventoryView> cellsEquipment;
        [SerializeField] private ActiveArea equipmentActiveArea;

        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private IMovingItemService _movingItemService;

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
            IMovingItemService movingItemService)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
            _movingItemService = movingItemService;
        }

        public void Initialize()
        {
            scrollHandleBag.SetActive(ConstantValues.BASE_SIZE_BAG > 20);
            
            _cellsBag = new List<CellInventoryView>();
            
            bagActiveArea.Construct(_movingItemService);
            equipmentActiveArea.Construct(_movingItemService);

            trashBin.Construct(_inventoryService, _movingItemService);
            trashBin.Subscribe(bagActiveArea);
            FillBag();
            FillEquipment();
        }

        public void ActivationUpdate(WindowType type) => 
            gameObject.SetActive(type == windowType);

        public CellInventoryView GetCellEquipmentByItemType(ItemType itemType) => 
            cellsEquipment.Find(cell => cell.ItemType == itemType);

        public void ActivateTutorial(ProgressTutorialService progressTutorialService)
        {
            cellsEquipment[4].ActivateTutorial(progressTutorialService);
            cellsEquipment[6].ActivateTutorial(progressTutorialService);
        }

        public void DeactivateTutorial()
        {
            cellsEquipment[4].DeactivateTutorial();
            cellsEquipment[6].DeactivateTutorial();
        }

        private void FillEquipment()
        {
            List<CellData> equipment = _inventoryService.Equipment;

            for (int i = 0; i < equipment.Count; i++)
            {
                cellsEquipment[i].Construct(i, _staticDataService, _movingItemService, _inventoryService);
                cellsEquipment[i].Subscribe();
                cellsEquipment[i].Subscribe(bagActiveArea, false);
                cellsEquipment[i].Subscribe(equipmentActiveArea, true);
                
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
                cellInventoryView.Construct(i, _staticDataService, _movingItemService, _inventoryService);
                cellInventoryView.Initialize();
                cellInventoryView.Subscribe();
                cellInventoryView.Subscribe(bagActiveArea, true);
                cellInventoryView.Subscribe(equipmentActiveArea, false);
                _cellsBag.Add(cellInventoryView);
            }
        }
    }
}
