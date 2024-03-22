using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Gameplay;

namespace UndergroundFortress.Extensions
{
    public static class QualityTypeExtensions
    {
        public static QualityType Random(this QualityType type, List<QualityData> qualities)
        {
            int totalWeight = qualities.Sum(data => data.weight);

            int accident = UnityEngine.Random.Range(0, totalWeight);
            foreach (QualityData qualityData in qualities)
            {
                accident -= qualityData.weight;
                if (accident < 0)
                {
                    type = qualityData.type;
                    return type;
                }
            }

            Debug.Log($"[QualityType] Fail get random type");
            type = QualityType.Empty;

            return type;
        }
    }
}