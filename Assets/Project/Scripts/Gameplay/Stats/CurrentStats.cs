using UnityEngine;

namespace UndergroundFortress.Scripts.Gameplay.Stats
{
    public class CurrentStats
    {
        public float Health;
        
        [Space]
        public float Stamina;

        public CurrentStats(float health,
            float stamina)
        {
            Health = health;
            Stamina = stamina;
        }
    }
}