﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.Gameplay.Stats
{
    public class StatView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text valueText;

        public void SetValues(Sprite iconSprite, in float value)
        {
            icon.sprite = iconSprite;
            valueText.text = value.ToString();
            
            gameObject.SetActive(true);
        }
        
        public void SetValues(Sprite iconSprite, in float minValue, in float maxValue)
        {
            icon.sprite = iconSprite;
            valueText.text = $"{ minValue } - { maxValue }";
            
            gameObject.SetActive(true);
        }
    }
}