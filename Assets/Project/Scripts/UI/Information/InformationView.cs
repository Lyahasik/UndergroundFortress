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
        [SerializeField] private GameObject capArea;
        [SerializeField] private GameObject closeButton;
        
        [Space]
        [SerializeField] private EquipmentView equipmentView;
        [SerializeField] private EquipmentComparisonView equipmentComparisonView;
        
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
            equipmentView.Construct(staticDataService);
            equipmentView.Initialize();
            
            equipmentComparisonView.Initialize(staticDataService);
            
            resourceView.Construct(staticDataService);
        }

        public void ShowItem(ItemData itemData, bool isEquipped)
        {
            capArea.SetActive(true);
            closeButton.SetActive(true);
            
            if (itemData is EquipmentData equipmentData)
                ShowEquipment(equipmentData, isEquipped);
            else if (itemData is ResourceData resourceData)
                ShowResource(resourceData);
        }

        public void ShowEquipmentComparison(EquipmentData equipmentData1, EquipmentData equipmentData2)
        {
            capArea.SetActive(true);
            closeButton.SetActive(true);
            
            equipmentComparisonView.Show(equipmentData1, equipmentData2);
        }

        public void ShowWarning(string message) => 
            warningPrompt.Show(message);

        public void CloseView()
        {
            capArea.SetActive(false);
            closeButton.SetActive(false);
            
            equipmentView.Hide();
            equipmentComparisonView.Hide();
            resourceView.Hide();
            warningPrompt.Hide();
        }

        private void ShowResource(ResourceData resourceData) => 
            resourceView.Show(resourceData);

        private void ShowEquipment(EquipmentData equipmentData, bool isEquipped) => 
            equipmentView.Show(equipmentData, isEquipped);
    }
}