using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.UI.Shop;

namespace UndergroundFortress
{
    
    [RequireComponent(typeof(Button))]
    public class GroupPurchaseTypeButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private GroupPurchaseType groupPurchaseType;
        
        private ShopView _shopView;

        private void Awake()
        {
            button.onClick.AddListener(TurnOnGroup);
        }

        public void Construct(ShopView shopView)
        {
            _shopView = shopView;
        }

        public void Change(GroupPurchaseType groupType) => 
            button.interactable = groupPurchaseType != groupType;

        private void TurnOnGroup()
        {
            _shopView.UpdateGroupPurchases(groupPurchaseType);
        }
    }
}
