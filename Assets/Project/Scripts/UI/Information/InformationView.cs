using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Information.Prompts;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.Skills;

namespace UndergroundFortress.UI.Information
{
    public class InformationView : MonoBehaviour
    {
        [SerializeField] private GameObject capArea;
        [SerializeField] private GameObject closeButton;
        
        [Space]
        [SerializeField] private SkillView skillView;
        [SerializeField] private ProgressSkillView progressSkillView;
        
        [Space]
        [SerializeField] private EquipmentView equipmentView;
        [SerializeField] private SaleEquipmentView saleEquipmentView;
        [SerializeField] private EquipmentComparisonView equipmentComparisonView;
        
        [Space]
        [SerializeField] private ResourceView resourceView;
        [SerializeField] private SaleResourceView saleResourceView;

        [Space]
        [SerializeField] private CellItemView cellItemView;

        [Space]
        [SerializeField] private WarningPrompt warningPrompt;
        
        private EquipmentView _currentEquipmentView;

        public CellItemView CellItemView => cellItemView;

        public void Initialize(IStaticDataService staticDataService,
            ISkillsUpgradeService skillsUpgradeService,
            IShoppingService shoppingService)
        {
            skillView.Construct(staticDataService, skillsUpgradeService);
            skillView.Initialize(CloseView);
            
            progressSkillView.Construct(staticDataService, skillsUpgradeService);
            progressSkillView.Initialize(CloseView);
            
            equipmentView.Construct(staticDataService);
            equipmentView.Initialize();
            saleEquipmentView.Construct(staticDataService, shoppingService);
            saleEquipmentView.Initialize(CloseView);
            
            equipmentComparisonView.Initialize(staticDataService);
            
            resourceView.Construct(staticDataService);
            saleResourceView.Construct(staticDataService, shoppingService);
            saleResourceView.Initialize(CloseView);
        }

        public void ShowSkill(SkillsType skillsType, SkillData skillData, bool isCanUpgrade, ProgressSkillData progressSkillData = null)
        {
            capArea.SetActive(true);
            closeButton.SetActive(true);
            
            if (progressSkillData == null)
                skillView.Show(skillsType, skillData, isCanUpgrade);
            else
                progressSkillView.Show(skillsType, skillData, progressSkillData, isCanUpgrade);
        }

        public void ShowItem(ItemData itemData)
        {
            capArea.SetActive(true);
            closeButton.SetActive(true);
            
            if (itemData.Type.IsEquipment())
                ShowEquipment(itemData);
            else
                ShowResource(itemData);
        }

        public void ShowSaleItem(CellSaleView cellSale)
        {
            capArea.SetActive(true);
            closeButton.SetActive(true);
            
            if (cellSale.ItemData.Type.IsEquipment())
                ShowSaleEquipment(cellSale);
            else
                ShowSaleResource(cellSale);
        }

        public void ShowEquipmentComparison(ItemData equipmentData1, ItemData equipmentData2)
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
            
            progressSkillView.Hide();
            skillView.Hide();
            equipmentView.Hide();
            saleEquipmentView.Hide();
            equipmentComparisonView.Hide();
            resourceView.Hide();
            saleResourceView.Hide();
            warningPrompt.Hide();
        }

        private void ShowSaleResource(CellSaleView cellSale) => 
            saleResourceView.Show(cellSale);

        private void ShowSaleEquipment(CellSaleView cellSale) => 
            saleEquipmentView.Show(cellSale);

        private void ShowResource(ItemData resourceData) => 
            resourceView.Show(resourceData);

        private void ShowEquipment(ItemData equipmentData) => 
            equipmentView.Show(equipmentData);
    }
}