using System.Runtime.InteropServices;

using UndergroundFortress.Helpers;

#if UNITY_WEBGL
namespace UndergroundFortress.Core.Publish.Web.Yandex
{
    public class YandexPurchasesModule : PurchasesModule
    {
        [DllImport("__Internal")]
        private static extern void CheckPurchasesExtern();
        [DllImport("__Internal")]
        private static extern void BuyPurchaseExtern(string idPurchase);
    
        public override void CheckPurchases()
        {
            if (OSManager.IsEditor())
                return;
            
            CheckPurchasesExtern();
        }

        public override void StartBayPurchase(int idPurchase)
        {
            BuyPurchaseExtern(idPurchase.ToString());
        }
    }
}
#endif