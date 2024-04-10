using System;
using System.Collections.Generic;

namespace UndergroundFortress.UI.Inventory
{
    [Serializable]
    public class RewardData
    {
        public string nameReward;

        public List<MoneyNumberData> moneys;
        public List<ItemNumberData> items;
    }
}