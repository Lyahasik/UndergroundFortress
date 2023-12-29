using System;
using UnityEngine;

namespace UndergroundFortress.Scripts.Gameplay.Stats
{
    [Serializable]
    public class MainStats
    {
        public float health;
        public float healthRecoveryRate;
        
        [Space]
        public float stamina;
        public float staminaRecoveryRate;
        public float staminaCost;
        
        [Space]
        public float damage;
    }
}