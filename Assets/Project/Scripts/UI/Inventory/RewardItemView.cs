using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.UI.Information
{
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image qualityBackground;
        
        [Space]
        [SerializeField] private GameObject numberLevelView;
        [SerializeField] private TMP_Text numberLevelText;
        [SerializeField] private TMP_Text numberText;

        public void Initialize(Sprite iconSprite, int number, int level = 0, Sprite qualitySprite = null)
        {
            icon.sprite = iconSprite;
            numberText.text = number.ToString();
            qualityBackground.sprite = qualitySprite;
            qualityBackground.gameObject.SetActive(qualitySprite != null);
            
            numberLevelView.SetActive(level != 0);
            numberLevelText.text = level.ToString();
        }
    }
}