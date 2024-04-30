using TMPro;
using UnityEngine;

namespace UndergroundFortress
{
    public class MonologueView : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private GameObject confirmButton;

        public void Activate(string message, bool isSkipped = false)
        {
            messageText.text = message;
            gameObject.SetActive(true);
            
            confirmButton.SetActive(isSkipped);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
