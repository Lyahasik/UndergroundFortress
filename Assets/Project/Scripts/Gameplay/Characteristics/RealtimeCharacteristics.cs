namespace UndergroundFortress.Scripts.Gameplay.Characteristics
{
    public class RealtimeCharacteristics
    {
        private float _stamina;
        private float _staminaRecoveryRate;
        private float _staminaCost;
        
        private float _damage;

        public float Stamina => _stamina;
        public float StaminaRecoveryRate => _staminaRecoveryRate;
        public float StaminaCost => _staminaCost;
        
        public float Damage => _damage;

        public RealtimeCharacteristics(float stamina,
            float staminaRecoveryRate,
            float staminaCost,
            float damage)
        {
            _stamina = stamina;
            _staminaRecoveryRate = staminaRecoveryRate;
            _staminaCost = staminaCost;
            _damage = damage;
        }
    }
}