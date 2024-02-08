using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.UI.Craft
{
    [RequireComponent(typeof(Button))]
    public class ItemGroupButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private ItemGroupType itemGroupType;
        
        private CraftView _craftView;

        private void Awake()
        {
            button.onClick.AddListener(TurnOnGroup);
        }

        public void Construct(CraftView craftView)
        {
            _craftView = craftView;
        }

        public void Change(ItemGroupType groupType) => 
            button.interactable = itemGroupType != groupType;

        private void TurnOnGroup()
        {
            _craftView.UpdateGroupItems(itemGroupType);
        }
    }
}