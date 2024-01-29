using TMPro;
using UnityEngine;

namespace UndergroundFortress.UI.Information.Prompts
{
    public class WarningPrompt : MonoBehaviour
    {
        [SerializeField] private GameObject cap;
        [SerializeField] private TMP_Text text;

        public void Show(string message)
        {
            text.text = message;
            
            cap.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            cap.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
