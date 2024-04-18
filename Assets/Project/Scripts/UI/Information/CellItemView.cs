using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.UI.Information
{
    public class CellItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image quality;
        
        [Space]
        [SerializeField] private GameObject numberLevelView;
        [SerializeField] private TMP_Text numberLevelText;
        [SerializeField] private bool isNumberLevelVisible = true;

        public Sprite Icon => icon.sprite;
        public Sprite Quality => quality.sprite;

        public void SetValues(Sprite icon, Sprite quality, int level = 0)
        {
            this.icon.sprite = icon;
            this.quality.sprite = quality;

            TryShowLevel(level);
            
            Show();
        }

        private void TryShowLevel(int level)
        {
            if (!isNumberLevelVisible)
            {
                numberLevelView.SetActive(false);
                return;
            }
            
            if (level > 0)
            {
                numberLevelView.SetActive(true);
                numberLevelText.text = level.ToString();
            }
            else
            {
                numberLevelView.SetActive(false);
            }
        }

        public void Reset()
        {
            icon.sprite = null;
            quality.sprite = null;
            
            Hide();
        }

        public void Hide() => 
            gameObject.SetActive(false);

        public void Show() => 
            gameObject.SetActive(true);
    }
}