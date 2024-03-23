using UnityEditor;
using UnityEngine;

namespace UndergroundFortress.Core.Services.Progress
{
    public partial class ProgressProviderService
    {
        [MenuItem("MyLogic/Progress/Reset")]
        public static void Reset()
        {
            Debug.Log($"[ProgressProviderService] Reset data.");
            PlayerPrefs.DeleteAll();
        }
    }
}