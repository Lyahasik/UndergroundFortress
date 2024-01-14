using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.UI.Craft;

namespace UndergroundFortress.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button buttonStartGame;

        private ISceneProviderService _sceneProviderService;

        private List<IWindow> _windows;

        public void Construct(ISceneProviderService sceneProviderService)
        {
            _sceneProviderService = sceneProviderService;
        }

        public void Initialize(CraftView craftView)
        {
            _windows = new List<IWindow>();
            
            _windows.Add(craftView);
            
            buttonStartGame.onClick.AddListener(LoadLevel);
        }

        public void ActivateWindow(int idWindow)
        {
            WindowType windowType = (WindowType) idWindow;

            foreach (IWindow window in _windows) 
                window.ActivationUpdate(windowType);
        }

        private void LoadLevel()
        {
            _sceneProviderService.LoadLevel(ConstantValues.SCENE_NAME_LEVEL);
        }
    }
}