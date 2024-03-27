using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private WalletView walletView;
        [SerializeField] private AmountSpaceBag amountSpaceBag;
        
        [Space]
        [SerializeField] private Button buttonStartGame;

        private ISceneProviderService _sceneProviderService;
        private IItemsGeneratorService _itemsGeneratorService;
        private IActivationRecipesService _activationRecipesService;

        private List<IWindow> _windows;

        public void Construct(ISceneProviderService sceneProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IActivationRecipesService activationRecipesService)
        {
            _sceneProviderService = sceneProviderService;
            _itemsGeneratorService = itemsGeneratorService;
            _activationRecipesService = activationRecipesService;
        }

        public void Initialize(
            HomeView homeView, 
            CraftView craftView,
            InventoryView inventoryView,
            IProgressProviderService progressProviderService)
        {
            _windows = new List<IWindow>();
            
            _windows.Add(homeView);
            _windows.Add(craftView);
            _windows.Add(inventoryView);
            
            walletView.Initialize(progressProviderService);
            
            amountSpaceBag.Register(progressProviderService);
            
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
        
        //TODO temporary
        private static int _idRecipeEquip;
        public void CreateRecipeEquip()
        {
            _activationRecipesService.ActivateRecipe(_idRecipeEquip++);
        }
        
        //TODO temporary
        private static int _idRecipeResource = 1000;
        public void CreateRecipeResource()
        {
            _activationRecipesService.ActivateRecipe(_idRecipeResource++);
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