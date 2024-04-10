using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.UI.Information
{
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image qualityBackground;
        [SerializeField] private TMP_Text numberText;

        public void Initialize(Sprite iconSprite, int number, Sprite qualitySprite = null)
        {
            icon.sprite = iconSprite;
            numberText.text = number.ToString();
            qualityBackground.sprite = qualitySprite;
            qualityBackground.gameObject.SetActive(qualitySprite != null);
        }
    }
}