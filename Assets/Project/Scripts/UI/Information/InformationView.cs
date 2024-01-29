using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.UI.Information.Prompts;

namespace UndergroundFortress.UI.Information
{
    public class InformationView : MonoBehaviour
    {
        [SerializeField] private GameObject closeObjects;
        
        [Space]
        [SerializeField] private List<EquipmentView> equipmentQualities;
        [SerializeField] private List<EquipmentView> equipmentSetQualities;
        
        [Space]
        [SerializeField] private ResourceView resourceView;

        [Space]
        [SerializeField] private CellItemView cellItemView;

        [Space]
        [SerializeField] private WarningPrompt warningPrompt;

        private EquipmentView _currentEquipmentView;

        public CellItemView CellItemView => cellItemView;

        public void Initialize(IStaticDataService staticDataService)
        {
            foreach (EquipmentView equipmentView in equipmentQualities) 
                equipmentView.Initialize();
            
            foreach (EquipmentView equipmentView in equipmentSetQualities) 
                equipmentView.Initialize();
            
            resourceView.Construct(staticDataService);
        }

        public void ShowItem(ItemData itemData)
        {
            closeObjects.SetActive(true);
            
            if (itemData is EquipmentData equipmentData)
                ShowEquipment(equipmentData);
            else if (itemData is ResourceData resourceData)
                ShowResource(resourceData);
        }

        public void ShowWarning(string message) => 
            warningPrompt.Show(message);

        public void CloseItems()
        {
            closeObjects.SetActive(false);
            
            _currentEquipmentView?.Hide();
            resourceView.Hide();
        }

        private void ShowResource(ResourceData resourceData) => 
            resourceView.Show(resourceData);

        private void ShowEquipment(EquipmentData equipmentData)
        {
            if (!equipmentData.IsSet)
            {
                _currentEquipmentView = equipmentQualities[(int)equipmentData.QualityType - 1];
                _currentEquipmentView.Show(equipmentData);
            }
            else
            {
                _currentEquipmentView = equipmentSetQualities[(int)equipmentData.QualityType - 1];
                _currentEquipmentView.Show(equipmentData);
            }
        }

        private void ShowEquipment(EquipmentData equipmentData, in Vector3 position)
        {
            ShowEquipment(equipmentData);

            _currentEquipmentView.transform.position = position;
        }
    }
}