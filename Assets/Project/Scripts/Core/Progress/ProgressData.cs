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
        public float TimeGame;

        public int NumberCrafting;
        public int NumberPurchases;
        public int NumberKilling;
            
        public HashSet<int> TutorialStages;

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
        public RewardsData RewardsData;

        public Dictionary<int, HashSet<int>> Dungeons;
        
        public static bool operator==(ProgressData value1, ProgressData value2)
        {
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
                return ReferenceEquals(value1, value2);
            
            return value1.TimeGame == value2.TimeGame;
        }

        public static bool operator!=(ProgressData value1, ProgressData value2)
        {
            return !(value1 == value2);
        }

        public static bool operator<(ProgressData value1, ProgressData value2)
        {
            return value1.TimeGame < value2.TimeGame;
        }

        public static bool operator>(ProgressData value1, ProgressData value2)
        {
            return !(value1 < value2);
        }

        public static bool operator<=(ProgressData value1, ProgressData value2)
        {
            return value1.TimeGame <= value2.TimeGame;
        }

        public static bool operator>=(ProgressData value1, ProgressData value2)
        {
            return !(value1 <= value2);
        }
    }
}