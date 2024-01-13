using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.UI.Craft
{
    public class RecipeView : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text maxLevelText;
        [SerializeField] private StatView statView;

        public void SetValues(EquipmentStaticData equipmentData, Sprite statTypeIcon)
        {
            iconImage.sprite = equipmentData.icon;
            nameText.text = equipmentData.name;
            maxLevelText.text = equipmentData.maxLevel.ToString();
            
            QualityValue qualityValue = equipmentData.qualityValues[(int)QualityType.Gray];
            statView.SetValues(statTypeIcon, qualityValue.minValue, qualityValue.maxValue);
        }
    }
}