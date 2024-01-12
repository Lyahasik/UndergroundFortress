using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.Information;

namespace UndergroundFortress.Gameplay.Items.Equipment
{
    public class EquipmentView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private List<StatView> mainStats;
        
        [Space]
        [SerializeField] private Image icon;
        [SerializeField] private List<StatView> stats;
        
        [Space]
        [SerializeField] private List<StoneView> stones;

        public void Initialize()
        {
            mainStats ??= new List<StatView>();
            
            stats ??= new List<StatView>();
            
            stones ??= new List<StoneView>();
        }

        public void Show(EquipmentData equipmentData)
        {
            titleText.text = equipmentData.Name;

            for (int i = 0; i < equipmentData.MainStats.Count; i++)
            {
                StatItemData statData = equipmentData.MainStats[i];
                mainStats[i].SetValues(statData.Icon, statData.Value);
            }

            icon.sprite = equipmentData.Icon;

            for (int i = 0; i < equipmentData.AdditionalStats.Count; i++)
            {
                StatItemData statData = equipmentData.AdditionalStats[i];
                stats[i].SetValues(statData.Icon, statData.Value);
            }

            for (int i = 0; i < equipmentData.Stones.Count; i++)
            {
                StoneItemData stoneData = equipmentData.Stones[i];
                stones[i].SetValues();
            }
            
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            Reset();
            gameObject.SetActive(false);
        }

        private void Reset()
        {
            titleText.text = "Empty";
            foreach (StatView statView in mainStats)
            {
                statView.SetValues(null, 0f);
            }
            
            icon.sprite = null;
            
            foreach (StatView statView in stats)
            {
                statView.SetValues(null, 0f);
            }

            foreach (StoneView stoneView in stones)
            {
                stoneView.SetValues();
            }
        }
    }
}
