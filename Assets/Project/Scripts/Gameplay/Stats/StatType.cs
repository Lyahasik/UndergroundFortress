﻿namespace UndergroundFortress.Gameplay.Stats
{
    public enum StatType
    {
        Empty = 0,
        
        Health = 1,
        HealthRecoveryRate,
        
        Stamina = 11,
        StaminaRecoveryRate,
        StaminaCost,
        
        Damage = 31,

        Defense = 41,

        Dodge = 51,
        Accuracy,

        Crit = 61,
        Parry,
        CritDamage,

        Block = 71,
        BreakThrough,
        BlockAttackDamage,

        Stun = 81,
        Strength,
        StunDuration
    }
}