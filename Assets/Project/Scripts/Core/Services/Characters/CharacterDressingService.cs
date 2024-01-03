using System.Collections.Generic;

using UndergroundFortress.Scripts.Gameplay.Character;
using UndergroundFortress.Scripts.Gameplay.StaticData;
using UndergroundFortress.Scripts.Gameplay.Stats;

namespace UndergroundFortress.Scripts.Core.Services.Characters
{
    public class CharacterDressingService : ICharacterDressingService
    {
        private delegate void ActionRef<T1, T2>(ref T1 currentValue, in T2 value);
        
        public void DressThePlayer(CharacterStats characterStats, List<ItemStaticData> items)
        {
            foreach (ItemStaticData item in items)
            {
                PutOnAnItem(characterStats, item);
            }
        }

        public void PutOnAnItem(CharacterStats characterStats, ItemStaticData itemData)
        {
            foreach (StatData statData in itemData.stats)
            {
                ChangeStat(characterStats, statData.type, statData.value, UpStat);
            }
        }

        public void RemoveAnItem(CharacterStats characterStats, ItemStaticData itemData)
        {
            foreach (StatData statData in itemData.stats)
            {
                ChangeStat(characterStats, statData.type, statData.value, DownStat);
            }
        }

        private void ChangeStat(CharacterStats characterStats,
            StatType statType,
            float statValue,
            ActionRef<float, float> changeMethod)
        {
            switch (statType)
            {
                case StatType.Health:
                    changeMethod(ref characterStats.MainStats.health, statValue);
                    break;
                case StatType.HealthRecoveryRate:
                    changeMethod(ref characterStats.MainStats.healthRecoveryRate, statValue);
                    break;
                
                case StatType.Stamina:
                    changeMethod(ref characterStats.MainStats.stamina, statValue);
                    break;
                case StatType.StaminaRecoveryRate:
                    changeMethod(ref characterStats.MainStats.staminaRecoveryRate, statValue);
                    break;
                case StatType.StaminaCost:
                    changeMethod(ref characterStats.MainStats.staminaCost, statValue);
                    break;
                
                case StatType.Damage:
                    changeMethod(ref characterStats.MainStats.damage, statValue);
                    break;
                
                case StatType.Defense:
                    changeMethod(ref characterStats.MainStats.defense, statValue);
                    break;
            }
        }

        private void UpStat(ref float currentValue, in float value)
        {
            currentValue += value;
        }

        private void DownStat(ref float currentValue, in float value)
        {
            currentValue -= value;
        }
    }
}