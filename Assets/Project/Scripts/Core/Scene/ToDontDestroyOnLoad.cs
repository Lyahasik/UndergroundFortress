using UnityEngine;

namespace UndergroundFortress.Scripts.Core.Scene
{
    public class ToDontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
