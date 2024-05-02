using UnityEngine;

namespace UndergroundFortress.Core.Publish
{
    public abstract class PurchaseModule : MonoBehaviour
    {
        public abstract void CheckGoods();
        public abstract void BayGoods(string idGoods);
    }
}
