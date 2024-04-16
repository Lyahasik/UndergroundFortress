using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress
{
    public class DungeonBackground : MonoBehaviour
    {
        [SerializeField] private Image background;

        public void UpdateBackground(Sprite sprite)
        {
            background.sprite = sprite;
        }
    }
}
