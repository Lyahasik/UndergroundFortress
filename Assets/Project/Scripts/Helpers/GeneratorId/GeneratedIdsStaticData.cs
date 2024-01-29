using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Helpers.GeneratorId
{
    [CreateAssetMenu(fileName = "GeneratedIdsData", menuName = "Helpers/Generator id/Generator")]
    public class GeneratedIdsStaticData : ScriptableObject
    {
        public List<int> resourcesIds;
        public List<int> equipmentIds;
    }
}