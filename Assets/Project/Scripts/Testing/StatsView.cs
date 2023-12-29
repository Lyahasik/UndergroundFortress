using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Testing
{
    public class StatsView : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private FieldView health;
        [SerializeField] private FieldView healthRecoveryRate;
        
        [Space]
        [SerializeField] private FieldView stamina;
        [SerializeField] private FieldView staminaRecoveryRate;
        [SerializeField] private FieldView staminaCost;
        
        [Space]
        [SerializeField] private FieldView damage;
        
        [Header("Current")]
        [Space]
        [SerializeField] private FieldView currentHealth;
        
        [Space]
        [SerializeField] private FieldView currentStamina;

        public void Construct(CharacterStats stats)
        {
            stats.OnUpdate += UpdateValues;
            UpdateValues(stats);
        }

        private void UpdateValues(CharacterStats stats)
        {
            //Main
            health.UpdateValues(nameof(stats.MainStats.health), stats.MainStats.health.ToString());
            healthRecoveryRate.UpdateValues(nameof(stats.MainStats.healthRecoveryRate),
                stats.MainStats.healthRecoveryRate.ToString());

            stamina.UpdateValues(nameof(stats.MainStats.stamina), stats.MainStats.stamina.ToString());
            staminaRecoveryRate.UpdateValues(nameof(stats.MainStats.staminaRecoveryRate),
                stats.MainStats.staminaRecoveryRate.ToString());
            staminaCost.UpdateValues(nameof(stats.MainStats.staminaCost), stats.MainStats.staminaCost.ToString());

            damage.UpdateValues(nameof(stats.MainStats.damage), stats.MainStats.damage.ToString());

            //Current
            currentHealth.UpdateValues(nameof(stats.CurrentStats.Health), stats.CurrentStats.Health.ToString());

            currentStamina.UpdateValues(nameof(stats.CurrentStats.Stamina), stats.CurrentStats.Stamina.ToString());
        }
    }
}
