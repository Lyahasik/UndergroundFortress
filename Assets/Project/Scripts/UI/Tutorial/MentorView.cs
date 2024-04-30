using UnityEngine;

namespace UndergroundFortress
{
    public class MentorView : MonoBehaviour
    {
        [SerializeField] private MonologueView monologueView;

        public void Activate(string message, bool isSkipped = false)
        {
            monologueView.Activate(message, isSkipped);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            monologueView.Deactivate();
            gameObject.SetActive(false);
        }
    }
}