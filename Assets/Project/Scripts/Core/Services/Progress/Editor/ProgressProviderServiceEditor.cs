using UnityEditor;
using UnityEngine;

namespace UndergroundFortress.Core.Services.Progress
{
    public class ProgressProviderServiceEditor
    {
        [MenuItem("MyLogic/Progress/Reset")]
        public static void Reset()
        {
            Debug.Log($"[ProgressProviderService] Reset data.");
            PlayerPrefs.DeleteAll();
        }
    }
}