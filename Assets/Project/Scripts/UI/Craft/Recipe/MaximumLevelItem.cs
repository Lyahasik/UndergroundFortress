using TMPro;
using UnityEngine;

namespace UndergroundFortress.UI.Craft.Recipe
{
    public class MaximumLevelItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text textValue;

        public void SetValue(in int value)
        {
            textValue.text = value.ToString();
            Show();
        }

        private void Show() => 
            gameObject.SetActive(true);
        
        public void Hide() => 
            gameObject.SetActive(false);
    }
}