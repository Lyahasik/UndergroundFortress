using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.UI.Inventory
{
    public class CellBagView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image quality;
        [SerializeField] private TMP_Text numberText;

        public void SetValues(Sprite icon,
            in QualityType qualityType,
            in int number)
        {
            this.icon.sprite = icon;
            quality.sprite = null;
            numberText.text = number.ToString();
        }

        public void Reset()
        {
            icon.sprite = null;
            quality.sprite = null;
            numberText.text = string.Empty;
        }
    }
}