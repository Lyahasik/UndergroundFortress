using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.UI.Craft
{
    public class ListRecipesView : MonoBehaviour
    {
        [SerializeField] private List<ItemTypeButton> itemTypeButtons;
        [SerializeField] private RecipeView prefabRecipeView;

        private CraftView _craftView;
        private IStaticDataService _staticDataService;
        private IProgressProviderService _progressProviderService;

        private List<RecipeView> _recipes;

        public event Action<int> OnActivateRecipe;

        public void Construct(CraftView craftView,
            IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            _craftView = craftView;
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            _recipes = new List<RecipeView>();
            
            FillList((int) ItemType.Sword);
        }

        public void FillList(int idItemType)
        {
            ItemType itemType = (ItemType) idItemType;
            
            ResetList();

            ChangeItemTypeButtons(itemType);
            
            Dictionary<ItemType, List<int>> activeRecipes
                = _progressProviderService.ProgressData.ActiveRecipes;

            List<RecipeStaticData> recipesStaticData = _staticDataService.ForRecipes();
            List<EquipmentStaticData> equipmentsStaticData = _staticDataService.ForEquipments();
            foreach (int itemId in activeRecipes[itemType])
            {
                RecipeStaticData recipeData = recipesStaticData.Find(v => v.id == itemId);
                EquipmentStaticData equipmentData
                    = equipmentsStaticData.Find(v => v.id == recipeData.idItem);
                
                RecipeView recipeView = Instantiate(prefabRecipeView, gameObject.transform);
                recipeView.Construct(_craftView, this);
                recipeView.Initialize(equipmentData, equipmentData.icon);
                recipeView.Subscribe();
                
                _recipes.Add(recipeView);
            }
        }

        public void ActivateRecipe(int idItem)
        {
            OnActivateRecipe?.Invoke(idItem);
        }

        private void ResetList()
        {
            if (_recipes.Count == 0)
                return;

            _craftView.UpdateCraftState(false);
            
            foreach (RecipeView recipeView in _recipes)
                Destroy(recipeView.gameObject);
            
            _recipes.Clear();
        }

        private void ChangeItemTypeButtons(ItemType itemType)
        {
            foreach (ItemTypeButton itemTypeButton in itemTypeButtons)
                itemTypeButton.Change(itemType);
        }
    }
}
