﻿using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Static data/Character/Character")]
    public class CharacterStaticData : ScriptableObject
    {
        [Space]
        public float health;
        public float healthRecoveryRate;
        
        [Space]
        public float stamina;
        public float staminaRecoveryRate;
        public float staminaCost;
        
        [Space]
        public float damage;

        [Space]
        public float defense;

        [Space]
        public float dodge;
        public float accuracy;

        [Space]
        public float crit;
        public float parry;
        public float critDamage;

        [Space]
        public float block;
        public float breakThrough;
        public float blockAttackDamage;

        [Space]
        public float stun;
        public float strength;
        public float stunDuration;
    }
}