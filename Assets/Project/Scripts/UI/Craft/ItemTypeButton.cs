using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.UI.Craft
{
    [RequireComponent(typeof(Button))]
    public class ItemTypeButton : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void Change(ItemType type)
        {
            _button.interactable = type != itemType;
        }
    }
}