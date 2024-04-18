using System.Collections.Generic;

using UndergroundFortress.Constants;
using UndergroundFortress.Helpers.CreateItemWindowEditor;

namespace UndergroundFortress.Helpers.GeneratorId
{
    public static class GeneratorId
    {
        public static int GenerateUnique(GeneratedIdsStaticData generatedIdsStaticData, ItemStaticDataType itemType)
        {
            return CreateIdForType(generatedIdsStaticData, itemType);
        }

        private static int CreateIdForType(GeneratedIdsStaticData generatedIdsStaticData, ItemStaticDataType itemType)
        {
            switch (itemType)
            {
                case ItemStaticDataType.Equipment:
                    return CreateId(generatedIdsStaticData.equipmentIds, ConstantValues.EQUIPMENT_START_ID);
                case ItemStaticDataType.Resource:
                case ItemStaticDataType.Consumable:
                    return CreateId(generatedIdsStaticData.resourcesIds, ConstantValues.RESOURCES_START_ID);
            }

            return ConstantValues.ERROR_ID;
        }

        private static int CreateId(List<int> listIds, in int startId)
        {
            int newId = listIds.Count >= 0 ? startId + listIds.Count : startId;
            listIds.Add(newId);

            return newId;
        }
    }
}