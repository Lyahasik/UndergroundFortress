using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.UI.Information
{
    public class CellItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image quality;

        public void SetValues(Sprite icon, Sprite quality)
        {
            this.icon.sprite = icon;
            this.quality.sprite = quality;
            
            gameObject.SetActive(true);
        }

        public void Reset()
        {
            gameObject.SetActive(false);
        }
    }
}