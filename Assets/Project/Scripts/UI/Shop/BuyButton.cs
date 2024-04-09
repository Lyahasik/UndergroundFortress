using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UndergroundFortress.Constants;

namespace UndergroundFortress.UI.Inventory
{
    [RequireComponent(typeof(Button))]
    public class BuyButton : MonoBehaviour
    {
        [Space]
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;

        public void Subscribe(UnityAction onClick)
        {
            button.onClick.AddListener(onClick);
        }
        
        public void UpdateText(bool isAds)
        {
            text.text = isAds ? ConstantValues.LOCALE_ADS : ConstantValues.LOCALE_BUY;
        }
    }
}