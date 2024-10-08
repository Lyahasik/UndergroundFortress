﻿using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public class ActivationRecipesService : IActivationRecipesService, IWritingProgress 
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;
        
        private Dictionary<ItemType, List<int>> _activeRecipes;
        
        public Dictionary<ItemType, List<int>> ActiveRecipes => _activeRecipes;

        public ActivationRecipesService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void Register(IProgressProviderService progressProviderService) => 
            progressProviderService.Register(this);

        public void LoadProgress(ProgressData progress)
        {
            _activeRecipes = progress.ActiveRecipes;
            TryFirstActivation();
        }

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.SaveProgress();
        }

        public void ActivateRecipe(int itemId)
        {
            ItemStaticData itemStaticData = _staticDataService.GetItemById(itemId);

            if (itemStaticData != null)
            {
                if (_activeRecipes[itemStaticData.type].Contains(itemStaticData.id))
                {
                    Debug.LogWarning($"[ActivationRecipesService] the recipe { itemStaticData.type }:{ itemStaticData.id } is already open");
                    return;
                }
                
                _activeRecipes[itemStaticData.type].Add(itemStaticData.id);
            }

            WriteProgress();
        }

        private void TryFirstActivation()
        {
            if (_activeRecipes[ItemType.Boots].Count != 0)
                return;
            
            var recipes = _staticDataService.GetDungeonById(0).unlockRecipes;
            recipes.ForEach(data => ActivateRecipe(data.itemData.id));
        }
    }
}