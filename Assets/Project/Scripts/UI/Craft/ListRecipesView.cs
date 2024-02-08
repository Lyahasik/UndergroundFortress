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
        }

        public void FillList(ItemType itemType)
        {
            ResetList();

            ChangeItemTypeButtons(itemType);
            
            Dictionary<ItemType, List<int>> activeRecipes
                = _progressProviderService.ProgressData.ActiveRecipes;

            List<RecipeStaticData> recipesStaticData = _staticDataService.ForRecipes();
            List<EquipmentStaticData> equipmentsStaticData = _staticDataService.ForEquipments();
            List<ResourceStaticData> resourcesStaticData = _staticDataService.ForResources();
            
            foreach (int itemId in activeRecipes[itemType])
            {
                RecipeStaticData recipeData = recipesStaticData.Find(v => v.idItem == itemId);
                
                RecipeView recipeView = Instantiate(prefabRecipeView, gameObject.transform);
                recipeView.Construct(_craftView, this);
                
                if (IsEquipment(itemType))
                {
                    EquipmentStaticData equipmentData
                        = equipmentsStaticData.Find(v => v.id == recipeData.idItem);
                    recipeView.Initialize(equipmentData, _staticDataService.GetStatIcon(equipmentData.typeStat));
                }
                else
                {
                    ResourceStaticData resourceData
                        = resourcesStaticData.Find(v => v.id == recipeData.idItem);
                    recipeView.Initialize(resourceData);
                }
                
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

        private bool IsEquipment(ItemType type)
        {
            return type is >= ItemType.Sword and < ItemType.Resource;
        }
    }
}
