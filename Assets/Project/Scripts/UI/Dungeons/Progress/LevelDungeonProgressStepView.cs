using UnityEngine;

namespace UndergroundFortress
{
    public class LevelDungeonProgressStepView : MonoBehaviour
    {
        [SerializeField] private GameObject numberObject;
        [SerializeField] private GameObject successObject;
        
        public void SetSuccess(bool isSuccess)
        {
            numberObject.SetActive(!isSuccess);
            successObject.SetActive(isSuccess);
        }
    }
}
