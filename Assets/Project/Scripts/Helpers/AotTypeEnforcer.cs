using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Helpers
{
    public class AotTypeEnforcer : MonoBehaviour
    {
        public void Awake()
        {
            AotHelper.EnsureType<float>();
            AotHelper.EnsureType<PlayerLevelData>();
            AotHelper.EnsureType<SkillPointsData>();
            AotHelper.EnsureType<WalletData>();
            AotHelper.EnsureType<RewardsData>();
            
            AotHelper.EnsureType<HashSet<int>>();
            
            AotHelper.EnsureList<int>();
            AotHelper.EnsureList<CellData>();
        
            AotHelper.EnsureDictionary<StatType, float>();
            AotHelper.EnsureDictionary<SkillsType, HashSet<int>>();
            AotHelper.EnsureDictionary<StatType, ProgressSkillData>();
            AotHelper.EnsureDictionary<ItemType, List<int>>();
            AotHelper.EnsureDictionary<BonusType, float>();
            AotHelper.EnsureDictionary<int, HashSet<int>>();
        }
    }
}
