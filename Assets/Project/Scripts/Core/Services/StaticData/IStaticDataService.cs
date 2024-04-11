using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.Inventory;
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
        public PlayerLevelStaticData GetPlayerLevelByCurrent(int currentLevel);
        public List<StatStaticData> ForStats();
        public SkillsStaticData ForSkillsByType(SkillsType skillsType);
        public List<EquipmentStaticData> ForEquipments();
        public List<ResourceStaticData> ForResources();
        public List<RecipeStaticData> ForRecipes();
        public List<PurchaseStaticData> ForPurchasesByMoneyType(MoneyType moneyType);
        public QualitiesStaticData ForQualities();
        public StatStaticData GetStatByType(StatType statType);
        public Sprite GetItemIcon(int itemDataId);
        public Sprite GetQualityBackground(QualityType type);
        public EquipmentStaticData GetEquipmentById(int itemId);
        public ResourceStaticData GetResourceById(int itemId);
        public RecipeStaticData GetRecipeById(int itemId);
        public QualityData GetQualityByType(QualityType type);
        public int GetItemMaxNumberForCellById(int itemId);
        public ItemStaticData GetItemById(int itemId);
        public string GetItemDescriptionById(int itemId);
        public Sprite GetIconMoneyByType(MoneyType moneyType);
        public DungeonStaticData GetDungeonById(int id);
    }
}