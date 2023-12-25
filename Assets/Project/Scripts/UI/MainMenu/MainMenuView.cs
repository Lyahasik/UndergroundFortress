using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Scripts.Constants;
using UndergroundFortress.Scripts.Core.Services.Scene;

namespace UndergroundFortress.Scripts.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        private ISceneProviderService _sceneProviderService;
        
        [SerializeField] private Button buttonStartGame;

        public void Construct(ISceneProviderService sceneProviderService)
        {
            _sceneProviderService = sceneProviderService;
        }

        public void Initialize()
        {
            buttonStartGame.onClick.AddListener(LoadLevel);
        }

        private void LoadLevel()
        {
            _sceneProviderService.LoadLevel(ConstantValues.SCENE_NAME_LEVEL);
        }
    }
}