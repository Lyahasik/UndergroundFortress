using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Character
{
    public class CurrentStatFillView : MonoBehaviour
    {
        [SerializeField] private StatType statType;

        [SerializeField] private TMP_Text valueText;
        [SerializeField] private bool isVisibleValue;
        [SerializeField] private Image fill;

        private void Awake()
        {
            valueText.gameObject.SetActive(isVisibleValue);
        }

        public void Subscribe(CharacterStats characterStats)
        {
            characterStats.OnUpdateCurrent += UpdateValue;
            UpdateValue(characterStats);
        }

        public void Unsubscribe(CharacterStats characterStats)
        {
            characterStats.OnUpdateCurrent -= UpdateValue;
        }

        public void UpdateValue(CharacterStats characterStats)
        {
            switch (statType)
            {
                case StatType.Health:
                    valueText.text = ((int) characterStats.CurrentStats.Health).ToString();
                    fill.fillAmount = characterStats.CurrentStats.Health / characterStats.MainStats[statType];
                    break;
                case StatType.Stamina:
                    fill.fillAmount = characterStats.CurrentStats.Stamina / characterStats.MainStats[statType];
                    break;
            }
        }
    }
}