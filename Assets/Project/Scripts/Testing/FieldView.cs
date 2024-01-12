using TMPro;
using UnityEngine;

namespace UndergroundFortress.Testing
{
    public class FieldView : MonoBehaviour
    {
        public TMP_Text title;
        public TMP_Text value;

        public void UpdateValues(string title, string value)
        {
            this.title.text = title.ToUpperInvariant();
            this.value.text = value;
        }
    }
}