using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private WalletView walletView;
        
        [Space]
        [SerializeField] private Button buttonStartGame;

        private ISceneProviderService _sceneProviderService;
        private IItemsGeneratorService _itemsGeneratorService;

        private List<IWindow> _windows;

        public void Construct(ISceneProviderService sceneProviderService,
            IItemsGeneratorService itemsGeneratorService)
        {
            _sceneProviderService = sceneProviderService;
            _itemsGeneratorService = itemsGeneratorService;
        }

        public void Initialize(CraftView craftView,
            InventoryView inventoryView,
            IProgressProviderService progressProviderService)
        {
            _windows = new List<IWindow>();
            
            _windows.Add(craftView);
            _windows.Add(inventoryView);
            
            walletView.Initialize(progressProviderService);
            
            buttonStartGame.onClick.AddListener(LoadLevel);
        }
        
        //TODO temporary
        public void CreateResource(int id)
        {
            for (int i = 0; i < 10; i++)
            {
                _itemsGeneratorService.GenerateResourceById(id);
            }
        }

        public void ActivateWindow(int idWindow)
        {
            WindowType windowType = (WindowType) idWindow;

            foreach (IWindow window in _windows) 
                window.ActivationUpdate(windowType);
        }

        private void LoadLevel() => 
            _sceneProviderService.LoadLevel(ConstantValues.SCENE_NAME_LEVEL);
    }
}