using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.StaticData;

namespace UndergroundFortress.UI.Inventory
{
    public class PriceMoneyView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text numberText;
        
        private IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void SetValues(PurchaseStaticData purchaseStaticData)
        {
            icon.sprite = _staticDataService.GetIconMoneyByType(purchaseStaticData.moneyType);

            numberText.text = purchaseStaticData.price.ToString();
            numberText.gameObject.SetActive(purchaseStaticData.moneyType != MoneyType.Ads);
        }
    }
}