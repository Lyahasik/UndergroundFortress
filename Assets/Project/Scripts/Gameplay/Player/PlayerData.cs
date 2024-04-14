using MoreMountains.Feedbacks;
using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Character
{
    public class PlayerData : CharacterData
    {
        [Space]
        [SerializeField] private MMF_Player damageHitFeedback;
        [SerializeField] private MMF_Player dodgeHitFeedback;
        [SerializeField] private MMF_Player parryHitFeedback;
        [SerializeField] private MMF_Player blockHitFeedback;
        [SerializeField] private MMF_Player stunHitFeedback;
        [SerializeField] private MMF_Player strengthHitFeedback;
        
        [Space]
        [SerializeField] private MMF_Player attackFeedback;
        [SerializeField] private MMF_Player accuracyFeedback;
        [SerializeField] private MMF_Player critFeedback;
        [SerializeField] private MMF_Player breakThroughFeedback;
        [SerializeField] private MMF_Player stunFeedback;

        private CurrentStatFillView _healthFillView;
        private CurrentStatFillView _staminaFillView;

        public void Construct(CharacterStats stats,
            CurrentStatFillView healthFillView,
            CurrentStatFillView staminaFillView)
        {
            base.Construct(stats);
            
            _healthFillView = healthFillView;
            _staminaFillView = staminaFillView;
        }
        
        public void Initialize()
        {
            base.Initialize();
            
            _healthFillView.Subscribe(Stats);
            _staminaFillView.Subscribe(Stats);
        }

        private void OnDestroy()
        {
            _healthFillView.Unsubscribe(Stats);
            _staminaFillView.Unsubscribe(Stats);
        }

        public override void TakeHitEffect(StatType hitType)
        {
            switch (hitType)
            {
                case StatType.Damage:
                    damageHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Dodge:
                    damageHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Parry:
                    damageHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Block:
                    damageHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Stun:
                    stunHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Strength:
                    damageHitFeedback.PlayFeedbacks();
                    break;
            }
        }

        public override void AttackEffect(StatType attackType)
        {
            switch (attackType)
            {
                case StatType.Damage:
                    attackFeedback.PlayFeedbacks();
                    break;
                case StatType.Accuracy:
                //     accuracyFeedback.PlayFeedbacks();
                
                    attackFeedback.PlayFeedbacks();
                    break;
                case StatType.Crit:
                    critFeedback.PlayFeedbacks();
                    break;
                case StatType.BreakThrough:
                    attackFeedback.PlayFeedbacks();
                //     breakThroughFeedback.PlayFeedbacks();
                    break;
                case StatType.Stun:
                    stunFeedback.PlayFeedbacks();
                    break;
            }
        }

        public override void RemoveHitEffect()
        {
            base.RemoveHitEffect();
            
            stunFeedback.StopFeedbacks();
        }

        private void UpdateStats(CharacterStats stats)
        {
            _healthFillView.UpdateValue(Stats);
            _staminaFillView.UpdateValue(Stats);
        }
    }
}