using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.UI.Information
{
    public class CellItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image quality;

        public Sprite Icon => icon.sprite;
        public Sprite Quality => quality.sprite;

        public void SetValues(Sprite icon, Sprite quality)
        {
            this.icon.sprite = icon;
            this.quality.sprite = quality;
            
            Show();
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