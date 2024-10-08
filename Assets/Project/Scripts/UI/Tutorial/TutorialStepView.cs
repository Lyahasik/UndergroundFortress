using UnityEngine;

using UndergroundFortress.Core.Localization;

namespace UndergroundFortress
{
    public class TutorialStepView : MonoBehaviour
    {
        [SerializeField] private string keyMessageLocate; 
            
        [Space]
        [SerializeField] private MentorView mentorView;
        [SerializeField] private bool isSkipped;
        [SerializeField] private bool isCapping;
        [SerializeField] private bool isClosing;

        public bool IsCapping => isCapping;
        public bool IsClosing => isClosing;

        public void Activate(ILocalizationService localizationService)
        {
            mentorView?.Activate(localizationService.LocaleTutorial(keyMessageLocate), isSkipped);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            mentorView?.Deactivate();
        }
    }
}
