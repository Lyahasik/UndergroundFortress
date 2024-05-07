using System;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.Gameplay.Stats.Services;

using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Character
{
    public class EnemyData : CharacterData
    {
        [SerializeField] private Attacking attacking;

        [Space]
        [SerializeField] private CurrentStatFillView healthFillView;
        [SerializeField] private RectTransform dropItemArea;
        [SerializeField] private RectTransform damageValueSpawnArea;
        
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

        private IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;
        private IWalletOperationService _walletOperationService;
        private IItemsGeneratorService _itemsGeneratorService;
        private IStatsRestorationService _statsRestorationService;
        private IAttackService _attackService;
        private ICheckerCurrentStatsService _checkerCurrentStatsService;
        private PlayerData _playerData;

        private Action _onStartDead;
        private Action _onDead;
        private Action<EnemyData> _onReady;

        private bool _isDead;

        public bool IsDead => _isDead;

        public void Construct(CharacterStats stats,
            IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IWalletOperationService walletOperationService,
            IItemsGeneratorService itemsGeneratorService,
            IStatsRestorationService statsRestorationService,
            IAttackService attackService,
            ICheckerCurrentStatsService checkerCurrentStatsService,
            PlayerData playerData,
            Action onStartDead,
            Action onDead,
            Action<EnemyData> onReady)
        {
            base.Construct(stats);

            _staticDataService = staticDataService;
            _localizationService = localizationService;
            _walletOperationService = walletOperationService;
            _itemsGeneratorService = itemsGeneratorService;
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

        public override void TakeHitEffect(StatType hitType, int damage = 0)
        {
            if (_isDead)
                return;
            
            switch (hitType)
            {
                case StatType.Damage:
                    damageHitFeedback.PlayFeedbacks();
                    break;
                case StatType.Dodge:
                    GenerateDamageView(_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_MISSED));
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

            if (damage > 0)
                GenerateDamageView(damage.ToString());
        }

        public override void AttackEffect(StatType attackType)
        {
            if (_isDead)
                return;
            
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

        public void DroppingItem(ItemStaticData itemData)
        {
            DropItemView dropItem = Instantiate(_staticDataService.ForLevel().dropItemPrefab, dropItemArea.position, Quaternion.identity, transform.parent);
            dropItem.Construct(_walletOperationService, _itemsGeneratorService, itemData);
            float duration = dropItem.DurationDrop;
            dropItem.transform.DOJump(dropItemArea.RandomInsidePoint(), 250f, 1, duration);
            dropItem.transform.DORotate(new Vector3(0f, 0f, Random.Range(0f, 360f)), duration);
            // dropItem.Initialize();
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
            _isDead = true;
            _onDead?.Invoke();
            
            base.Dead();
        }

        private void GenerateDamageView(string value)
        {
            DamageValueView damageValueView
                = Instantiate(_staticDataService.ForLevel().damageValuePrefab, damageValueSpawnArea.RandomInsidePoint(), Quaternion.identity, transform.parent);
            damageValueView.SetValue(value);
        }
    }
}