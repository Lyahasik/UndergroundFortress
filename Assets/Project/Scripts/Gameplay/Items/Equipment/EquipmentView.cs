using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.Information;

namespace UndergroundFortress.Gameplay.Items.Equipment
{
    public class EquipmentView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private CellItemView cellItemView;

        [Space]
        [SerializeField] private List<StatView> mainStats;
        
        [SerializeField] private List<StatView> stats;

        [Space]
        [SerializeField] private List<StoneView> stones;

        protected IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;

        public void Construct(IStaticDataService staticDataService, ILocalizationService localizationService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
        }

        public void Initialize()
        {
            mainStats ??= new List<StatView>();
            
            stats ??= new List<StatView>();
            
            stones ??= new List<StoneView>();
        }

        public void Show(ItemData equipmentData)
        {
            if (nameText != null)
                nameText.text = _localizationService.LocaleEquipment(equipmentData.Name);
            
            if (cellItemView != null)
                cellItemView.SetValues(
                    _staticDataService.GetItemIcon(equipmentData.Id),
                    _staticDataService.GetQualityBackground(equipmentData.QualityType),
                    equipmentData.Level);
            
            UpdateStatsView(mainStats, equipmentData.MainStats);
            UpdateStatsView(stats, equipmentData.AdditionalStats);

            // for (int i = 0; i < equipmentData.Stones.Count; i++)
            // {
            //     StoneItemData stoneData = equipmentData.Stones[i];
            //     stones[i].SetValues();
            // }
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Reset();
            gameObject.SetActive(false);
        }

        private void UpdateStatsView(List<StatView> statViews, List<StatItemData> statsData)
        {
            for (int i = 0; i < statViews.Count; i++)
            {
                if (i < statsData.Count)
                {
                    StatItemData statData = statsData[i];
                    StatStaticData statStaticData = _staticDataService.GetStatByType(statData.Type);
                    statViews[i].SetValues(
                        _localizationService.LocaleStat(statStaticData.keyName),
                        statStaticData.icon,
                        statData.QualityType,
                        statData.Value);
                }
                else
                {
                    statViews[i].Hide();
                }
            }
        }

        protected void Reset()
        {
            if (nameText != null)
                nameText.text = "Empty";
            
            foreach (StatView statView in mainStats) 
                statView.Hide();

            foreach (StatView statView in stats) 
                statView.Hide();

            foreach (StoneView stoneView in stones) 
                stoneView.Hide();
        }
    }
}
