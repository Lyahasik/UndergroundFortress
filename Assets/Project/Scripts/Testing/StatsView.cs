using UnityEngine;

using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Testing
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
            UpdateValues(stats);
            stats.OnUpdateCurrent += UpdateCurrentValues;
        }

        private void UpdateValues(CharacterStats stats)
        {
            health.UpdateValues(StatType.Health.ToString(), stats.MainStats[StatType.Health].ToString());
            healthRecoveryRate.UpdateValues(StatType.HealthRecoveryRate.ToString(),
                stats.MainStats[StatType.HealthRecoveryRate].ToString());

            stamina.UpdateValues(StatType.Stamina.ToString(), stats.MainStats[StatType.Stamina].ToString());
            staminaRecoveryRate.UpdateValues(StatType.StaminaRecoveryRate.ToString(),
                stats.MainStats[StatType.StaminaRecoveryRate].ToString());
            staminaCost.UpdateValues(StatType.StaminaCost.ToString(), 
                stats.MainStats[StatType.StaminaCost].ToString());

            damage.UpdateValues(StatType.Damage.ToString(), stats.MainStats[StatType.Damage].ToString());

            UpdateCurrentValues(stats);
        }

        private void UpdateCurrentValues(CharacterStats stats)
        {
            currentHealth.UpdateValues(nameof(stats.CurrentStats.Health), stats.CurrentStats.Health.ToString());

            currentStamina.UpdateValues(nameof(stats.CurrentStats.Stamina), stats.CurrentStats.Stamina.ToString());
        }
    }
}
