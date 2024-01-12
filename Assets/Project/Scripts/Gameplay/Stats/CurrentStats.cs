namespace UndergroundFortress.Gameplay.Stats
{
    public class CurrentStats
    {
        public float Health { get; set; }
        public float Stamina { get; set; }

        public CurrentStats(float health,
            float stamina)
        {
            Health = health;
            Stamina = stamina;
        }
    }
}