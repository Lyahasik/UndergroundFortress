using TMPro;
using UnityEngine;

namespace UndergroundFortress.Gameplay.Character
{
    public class DamageValueView : MonoBehaviour
    {
        [SerializeField] private TMP_Text valueText;

        public void SetValue(string value)
        {
            valueText.text = value;
        }

        public void End()
        {
            Destroy(gameObject);
        }
    }
}