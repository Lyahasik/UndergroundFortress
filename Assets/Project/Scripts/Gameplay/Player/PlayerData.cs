﻿using System;
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
        
        public event Action OnDead;

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
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        private void OnDestroy()
        {
            _healthFillView.Unsubscribe(Stats);
            _staminaFillView.Unsubscribe(Stats);
        }

        public override void TakeHitEffect(StatType hitType, int damage = 0)
        {
            switch (hitType)
            {
                case StatType.Damage:
                    damageHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Dodge:
                    dodgeHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Parry:
                    damageHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Block:
                    blockHitFeedback.PlayFeedbacks();
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
            
            stunHitFeedback.StopFeedbacks();
        }

        public override void StartDead()
        {
            OnDead?.Invoke();
        }
    }
}