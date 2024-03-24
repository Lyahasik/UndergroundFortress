using System.Collections.Generic;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public interface IActivationRecipesService : IService
    {
        public Dictionary<ItemType, List<int>> ActiveRecipes { get; }
        public void Initialize();
        public void ActivateRecipe(int itemId);
    }
}