using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.MainMenu.StaticData;
using UndergroundFortress.UI.StaticData;

namespace UndergroundFortress.Core.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private UIStaticData _ui; 
            
        private MainMenuStaticData _mainMenu;
        private CharacterStaticData _player;

        private LevelStaticData _level;
        private CharacterStaticData _enemy;

        private PlayerLevelsStaticData _playerLevels;
        private List<StatStaticData> _stats;
        private List<SkillsStaticData> _skills;
        private QualitiesStaticData _qualities;
        private List<ResourceStaticData> _resources;
        private List<EquipmentStaticData> _equipments;
        private List<RecipeStaticData> _recipes;

        public void Load()
        {
            _ui = Resources
                .Load<UIStaticData>(ConstantPaths.UI_DATA_PATH);
            
            _mainMenu = Resources
                .Load<MainMenuStaticData>(ConstantPaths.MAIN_MENU_DATA_PATH);
            _player = Resources
                .Load<CharacterStaticData>(ConstantPaths.PLAYER_DATA_PATH);
            
            _level = Resources
                .Load<LevelStaticData>(ConstantPaths.LEVEL_DATA_PATH);
            _enemy = Resources
                .Load<CharacterStaticData>(ConstantPaths.ENEMY_DATA_PATH);

            _qualities = Resources
                .Load<QualitiesStaticData>(ConstantPaths.QUALITY_DATA_PATH);
            
            _playerLevels = Resources
                .Load<PlayerLevelsStaticData>(ConstantPaths.PLAYER_LEVELS_DATA_PATH);
            _stats = Resources
                .LoadAll<StatStaticData>(ConstantPaths.STATS_DATA_PATH)
                .ToList();
            _skills = Resources
                .LoadAll<SkillsStaticData>(ConstantPaths.SKILLS_DATA_PATH)
                .ToList();
            _resources = Resources
                .LoadAll<ResourceStaticData>(ConstantPaths.RESOURCES_DATA_PATH)
                .ToList();
            _equipments = Resources
                .LoadAll<EquipmentStaticData>(ConstantPaths.EQUIPMENTS_DATA_PATH)
                .ToList();
            _recipes = Resources
                .LoadAll<RecipeStaticData>(ConstantPaths.RECIPES_DATA_PATH)
                .ToList();
        }

        public UIStaticData ForUI() => 
            _ui;

        public MainMenuStaticData ForMainMenu() => 
            _mainMenu;
        public CharacterStaticData ForPlayer() => 
            _player;

        public LevelStaticData ForLevel() => 
            _level;
        public CharacterStaticData ForEnemy() => 
            _enemy;
        
        public QualitiesStaticData ForQualities() => 
            _qualities;
        public PlayerLevelStaticData GetPlayerLevelByCurrent(int currentLevel) => 
            _playerLevels.levelsData.Find(data => data.level == currentLevel);
        public List<StatStaticData> ForStats() => 
            _stats;

        public SkillsStaticData ForSkillsByType(SkillsType skillsType) => 
            _skills.Find(skills => skills.type == skillsType);
        public List<EquipmentStaticData> ForEquipments() => 
            _equipments;

        public List<ResourceStaticData> ForResources() => 
            _resources;

        public List<RecipeStaticData> ForRecipes() => 
            _recipes;

        public StatStaticData GetStatByType(StatType statType) => 
            _stats.Find(data => data.type == statType);

        public Sprite GetItemIcon(int itemDataId)
        {
            foreach (EquipmentStaticData equipmentStaticData in _equipments)
                if (equipmentStaticData.id == itemDataId)
                    return equipmentStaticData.icon;

            foreach (ResourceStaticData resourceStaticData in _resources)
                if (resourceStaticData.id == itemDataId)
                    return resourceStaticData.icon;

            Debug.LogWarning($"[StaticDataService] Not found of id for item icon");
            return null;
        }

        public Sprite GetQualityBackground(QualityType type)
        {
            if (type == QualityType.Empty)
                return null;
            
            return _qualities.qualitiesData.Find(data => data.type == type).background;
        }

        public ResourceStaticData GetResourceById(int itemId)
        {
            foreach (ResourceStaticData resourceStaticData in _resources)
                if (resourceStaticData.id == itemId)
                    return resourceStaticData;

            Debug.LogWarning($"[StaticDataService] Not found of id for resource data");
            return null;
        }

        public int GetItemMaxNumberForCellById(int itemId)
        {
            foreach (EquipmentStaticData equipmentStaticData in _equipments)
                if (equipmentStaticData.id == itemId)
                    return equipmentStaticData.maxNumberForCell;

            foreach (ResourceStaticData resourceStaticData in _resources)
                if (resourceStaticData.id == itemId)
                    return resourceStaticData.maxNumberForCell;

            Debug.LogWarning($"[StaticDataService] Not found of id for max number for cell");
            return ConstantValues.MIN_NUMBER_ITEM_FOR_CELL;
        }

        public ItemStaticData GetItemById(int itemId)
        {
            foreach (EquipmentStaticData equipmentStaticData in _equipments)
                if (equipmentStaticData.id == itemId)
                    return equipmentStaticData;

            foreach (ResourceStaticData resourceStaticData in _resources)
                if (resourceStaticData.id == itemId)
                    return resourceStaticData;

            Debug.LogWarning($"[StaticDataService] Not found of id for item");
            return null;
        }

        public string GetItemDescriptionById(int itemId)
        {
            foreach (ResourceStaticData resourceStaticData in _resources)
                if (resourceStaticData.id == itemId)
                    return resourceStaticData.description;

            Debug.LogWarning($"[StaticDataService] Not found of id for description");
            return string.Empty;
        }
    }
}