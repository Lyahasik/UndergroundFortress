using UndergroundFortress.Helpers;

#if UNITY_WEBGL
namespace UndergroundFortress.Core.Publish.Web.Yandex
{
    public class YandexPurchaseModule : PurchaseModule
    {
        // [DllImport("__Internal")]
        // private static extern void CheckGoodsExtern();
        // [DllImport("__Internal")]
        // private static extern void BuyGoodsExtern(string idGoods);
    
        public override void CheckGoods()
        {
            if (OSManager.IsEditor())
                return;
            
            // CheckGoodsExtern();
        }

        public override void BayGoods(string idGoods)
        {
            
            // BuyGoodsExtern(idGoods);
        }
    }
}
#endif