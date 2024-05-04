namespace UndergroundFortress.Core.Publish
{
    public abstract class PurchasesModule
    {
        public abstract void CheckPurchases();
        public abstract void StartBayPurchase(int idPurchase);
    }
}
