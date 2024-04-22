using System.Collections.Generic;

using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Core.Progress
{
    public class ProgressData
    {
        public PlayerLevelData LevelData;
        public SkillPointsData SkillPointsData;
        public Dictionary<StatType, float> MainStats;
        public Dictionary<SkillsType, HashSet<int>> ActiveSkills;
        public Dictionary<StatType, ProgressSkillData> ProgressSkills;

        public WalletData Wallet;
        
        public Dictionary<ItemType, List<int>> ActiveRecipes;
        public List<CellData> Equipment;
        public List<CellData> Bag;
        public int FilledNumberBag;

        public Dictionary<BonusType, float> BonusesLifetime;

        public Dictionary<int, HashSet<int>> Dungeons;
    }
}