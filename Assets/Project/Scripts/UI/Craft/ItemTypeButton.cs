using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.UI.Craft
{
    [RequireComponent(typeof(Button))]
    public class ItemTypeButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private ItemType itemType;

        public void Change(ItemType type)
        {
            button.interactable = type != itemType;
        }
    }
}