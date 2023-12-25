using System.Collections;
using UnityEngine;

using UndergroundFortress.Scripts.Core.Coroutines;
using UndergroundFortress.Scripts.UI.StaticData;

namespace UndergroundFortress.Scripts.UI.Loading
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup curtain;
        
        private UIStaticData _uiStaticData;

        private WaitForSeconds _dissolutionDelay;

        public void Construct(string newName, UIStaticData uiStaticData)
        {
            name = newName;
            _uiStaticData = uiStaticData;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            curtain.alpha = 1;
        }

        public void Hide(CoroutinesContainer container) =>
            container.StartCoroutine(DoFadeIn());

        private IEnumerator DoFadeIn()
        {
            _dissolutionDelay ??= new WaitForSeconds(_uiStaticData.curtainDissolutionDelay);
            
            while (curtain.alpha > 0)
            {
                curtain.alpha -= _uiStaticData.curtainDissolutionStep;
                yield return _dissolutionDelay;
            }
      
            gameObject.SetActive(false);
        }
    }
}