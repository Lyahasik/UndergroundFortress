using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "MoneysData", menuName = "Static data/Moneys")]
    public class MoneysStaticData : ScriptableObject
    {
        public List<MoneyData> moneysData;
    }
}