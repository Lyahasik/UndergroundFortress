using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.Gameplay.Stats
{
    public class StatView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text valueText;

        public void SetValues(string keyName, Sprite iconSprite, QualityType qualityType, in float value)
        {
            background.color = GetColorQuality(qualityType);
            icon.sprite = iconSprite;

            //TODO locale
            nameText.text = keyName;
            valueText.text = Math.Round(value, 2).ToString();
            
            gameObject.SetActive(true);
        }

        public void SetValues(string keyName, Sprite iconSprite, in float minValue, in float maxValue)
        {
            icon.sprite = iconSprite;

            //TODO locale
            nameText.text = keyName;
            valueText.text = $"{ minValue } - { maxValue }";
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            background.color = GetColorQuality(QualityType.Empty);
            icon.sprite = null;
            valueText.text = string.Empty;
        }

        private Color GetColorQuality(QualityType qualityType)
        {
            switch (qualityType)
            {
                case QualityType.Grey:
                    return Color.gray;
                case QualityType.Green:
                    return Color.green;
                case QualityType.Blue:
                    return Color.blue;
                case QualityType.Purple:
                    return Color.magenta;
                case QualityType.Yellow:
                    return Color.yellow;
                case QualityType.Red:
                    return Color.red;
                case QualityType.White:
                    return Color.white;
            }

            return Color.gray;
        }
    }
}