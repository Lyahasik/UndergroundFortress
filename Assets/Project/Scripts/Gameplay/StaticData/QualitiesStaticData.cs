using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "QualitiesData", menuName = "Static data/Qualities")]
    public class QualitiesStaticData : ScriptableObject
    {
        public List<QualityData> qualitiesData;
    }
}