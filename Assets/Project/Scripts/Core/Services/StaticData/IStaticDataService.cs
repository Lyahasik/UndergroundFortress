using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.MainMenu.StaticData;
using UndergroundFortress.UI.StaticData;

namespace UndergroundFortress.Core.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        public UIStaticData ForUI();
        public MainMenuStaticData ForMainMenu();
        public LevelStaticData ForLevel();
        public CharacterStaticData ForPlayer();
        public CharacterStaticData ForEnemy();
        public List<StatStaticData> ForStats();
        public List<EquipmentStaticData> ForEquipments();
        public List<ResourceStaticData> ForResources();
        public List<RecipeStaticData> ForRecipes();
        public Sprite GetItemIcon(int itemDataId);
        public Sprite GetQualityIcon(QualityType type);
        public ResourceStaticData GetResourceById(int itemId);
    }
}