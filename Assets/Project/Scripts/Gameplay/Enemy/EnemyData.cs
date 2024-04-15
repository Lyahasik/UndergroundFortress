using System;
using MoreMountains.Feedbacks;
using UnityEngine;

using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.Gameplay.Stats.Services;

namespace UndergroundFortress.Gameplay.Character
{
    public class EnemyData : CharacterData
    {
        [SerializeField] private Attacking attacking;

        [Space]
        [SerializeField] private CurrentStatFillView healthFillView;
        
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
        
        [Space]
        [SerializeField] private MMF_Player deadFeedback;
        
        private IStatsRestorationService _statsRestorationService;
        private IAttackService _attackService;
        private ICheckerCurrentStatsService _checkerCurrentStatsService;
        private PlayerData _playerData;

        private Action _onStartDead;
        private Action _onDead;
        private Action<EnemyData> _onReady;


        public void Construct(CharacterStats stats,
            IStatsRestorationService statsRestorationService,
            IAttackService attackService,
            ICheckerCurrentStatsService checkerCurrentStatsService,
            PlayerData playerData,
            Action onStartDead,
            Action onDead,
            Action<EnemyData> onReady)
        {
            base.Construct(stats);
            
            _statsRestorationService = statsRestorationService;
            _attackService = attackService;
            _checkerCurrentStatsService = checkerCurrentStatsService;
            _playerData = playerData;

            _onStartDead = onStartDead;
            _onDead = onDead;
            _onReady = onReady;
        }
        
        public override void Initialize()
        {
            base.Initialize();

            healthFillView.Subscribe(Stats);
            
            _statsRestorationService.AddStats(Stats);

            Stats.CurrentStats.Stamina = 0;
        }

        private void OnDestroy()
        {
            _statsRestorationService.RemoveStats(Stats);
        }

        public override void ActivateStun(float duration)
        {
            base.ActivateStun(duration);

            Stats.CurrentStats.Stamina = 0;
        }

        public override void TakeHitEffect(StatType hitType)
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
                //     parryHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Block:
                    blockHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Stun:
                    stunHitFeedback.PlayFeedbacks();
                    //     strengthHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Strength:
                    damageHitFeedback.PlayFeedbacks();
                    //     strengthHitFeedback.PlayFeedbacks();
                    break;
            }
        }

        public override void AttackEffect(StatType attackType)
        {
            attackFeedback.PlayFeedbacks();
            
            // switch (attackType)
            // {case StatType.Damage:
            //         attackFeedback.PlayFeedbacks();
            //         break;
            //     case StatType.Accuracy:
            //         accuracyFeedback.PlayFeedbacks();
            //         break;
            //     case StatType.Crit:
            //         critFeedback.PlayFeedbacks();
            //         break;
            //     case StatType.BreakThrough:
            //         breakThroughFeedback.PlayFeedbacks();
            //         break;
            //     case StatType.Stun:
            //         stunFeedback.PlayFeedbacks();
            //         break;
            // }
        }

        public override void RemoveHitEffect()
        {
            base.RemoveHitEffect();
            
            stunHitFeedback.StopFeedbacks();
        }

        public void Ready()
        {
            _onReady?.Invoke(this);
            
            attacking.Construct(_attackService, _checkerCurrentStatsService, this, _playerData);
        }

        public override void StartDead()
        {
            attacking.enabled = false;
            healthFillView.gameObject.SetActive(false);
            deadFeedback.PlayFeedbacks();
            
            _onStartDead?.Invoke();
        }

        public override void Dead()
        {
            _onDead?.Invoke();
            
            base.Dead();
        }
    }
}