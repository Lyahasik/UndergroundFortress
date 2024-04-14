using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Extensions
{
    public static class StatTypeExtensions
    {
        public static bool IsInteger(this StatType type)
        {
            return type is StatType.Health
                or StatType.Stamina
                or StatType.Damage
                or StatType.Defense;
        }
        
        public static bool IsEquipmentAdditional(this StatType type)
        {
            return type is StatType.Health
                or StatType.HealthRecoveryRate
                or StatType.Stamina
                or StatType.StaminaRecoveryRate
                or StatType.Dodge
                or StatType.Accuracy
                or StatType.Crit
                or StatType.CritDamage
                or StatType.Parry
                or StatType.Block
                or StatType.BreakThrough
                or StatType.Stun
                or StatType.Strength;
        }
        
        public static bool IsPassiveProgress(this StatType type)
        {
            return type is StatType.Dodge
                or StatType.Accuracy
                or StatType.Crit
                or StatType.Parry
                or StatType.Block
                or StatType.BreakThrough
                or StatType.Stun
                or StatType.Strength;
        }
        
        public static string IncreaseIndicatorToString(this StatType type, in float value)
        {
            var newString = $"+{ (int) value }";
            
            switch (type)
            {
                case StatType.StaminaCost:
                    newString = $"-{ value }%";
                    break;
                case StatType.Dodge:
                case StatType.Accuracy:
                case StatType.Crit:
                case StatType.Parry:
                case StatType.CritDamage:
                case StatType.Block:
                case StatType.BreakThrough:
                case StatType.Stun:
                case StatType.Strength:
                case StatType.StunDuration:
                    newString = $"+{ value }%";
                    break;
            }

            return newString;
        }
    }
}