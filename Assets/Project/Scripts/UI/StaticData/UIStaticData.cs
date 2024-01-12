using UnityEngine;

namespace UndergroundFortress.UI.StaticData
{
    [CreateAssetMenu(fileName = "UIData", menuName = "Static data/UI")]
    public class UIStaticData : ScriptableObject
    {
        public float curtainDissolutionStep;
        public float curtainDissolutionDelay;
    }
}