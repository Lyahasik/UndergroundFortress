using System;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.StaticData;
using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class AttackService : IAttackService
    {
        private readonly IStatsWasteService _statsWasteService;
        private readonly IStatsRestorationService _statsRestorationService;
        
        private bool _isDoubleDamage;
        private bool _isVampireDamage;

        public bool IsDoubleDamage => _isDoubleDamage;
        public bool IsVampireDamage => _isVampireDamage;

        public AttackService(IStatsWasteService statsWasteService,
            IStatsRestorationService statsRestorationService)
        {
            _statsWasteService = statsWasteService;
            _statsRestorationService = statsRestorationService;
        }

        public void Attack(CharacterData dataAttacking, CharacterData dataDefending, bool isPlayer = false)
        {
            CharacterStats statsAttacking = dataAttacking.Stats;
            CharacterStats statsDefending = dataDefending.Stats;

            if (!TryHit(statsAttacking, statsDefending))
            {
                _statsWasteService.WasteStamina(statsAttacking, statsAttacking.MainStats[StatType.StaminaCost]);
                dataAttacking.AttackEffect(StatType.Dodge);
                dataDefending.TakeHitEffect(StatType.Dodge);
                
                return;
            }

            float damage = statsAttacking.MainStats[StatType.Damage];
            if (isPlayer && _isDoubleDamage)
            {
                damage *= 2;
                _isDoubleDamage = false;
            }
            damage = Math.Clamp(damage - statsDefending.MainStats[StatType.Defense], 0, float.MaxValue);

            if (TryBreakThrough(statsAttacking, statsDefending, ref damage))
                dataDefending.TakeHitEffect(StatType.Damage);
            else
                dataDefending.TakeHitEffect(StatType.Block);
                
            if (TryApplyCrit(statsAttacking, statsDefending, ref damage))
                dataAttacking.AttackEffect(StatType.Crit);

            _statsWasteService.WasteHealth(statsDefending, (int) damage);
            _statsWasteService.WasteStamina(statsAttacking, statsAttacking.MainStats[StatType.StaminaCost]);
            if (isPlayer && _isVampireDamage)
            {
                _statsRestorationService.RestoreHealth(statsAttacking, (int) (damage * 0.5f));
            }

            if (TryDead(dataDefending))
            {
                if (!isPlayer)
                    Reset();

                return;
            }

            if (TryStun(statsAttacking, dataDefending))
            {
                dataAttacking.AttackEffect(StatType.Stun);
                dataDefending.TakeHitEffect(StatType.Stun);
            }
            
            dataAttacking.AttackEffect(StatType.Damage);
        }

        public void ActivateConsumable(ConsumableType consumableType)
        {
            switch (consumableType)
            {
                case ConsumableType.Type2:
                    _isDoubleDamage = true;
                    break;
                case ConsumableType.Type3:
                    _isVampireDamage = true;
                    break;
            }
        }

        private bool TryHit(CharacterStats statsAttacking, CharacterStats statsDefending)
        {
            float probabilityMiss
                = statsDefending.MainStats[StatType.Dodge] - statsAttacking.MainStats[StatType.Accuracy];
            probabilityMiss = Math.Clamp(probabilityMiss, 0, statsDefending.MainStats[StatType.Dodge]);

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            return result >= probabilityMiss;
        }

        private bool TryBreakThrough(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityBlock =
                statsDefending.MainStats[StatType.Block] - statsAttacking.MainStats[StatType.BreakThrough];
            probabilityBlock = Math.Clamp(probabilityBlock, 0, statsDefending.MainStats[StatType.Block]);

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            if (result < probabilityBlock)
                damage -= damage * (statsAttacking.MainStats[StatType.BlockAttackDamage] / ConstantValues.MAX_PERCENTAGE);

            return result >= probabilityBlock;
        }

        private bool TryApplyCrit(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityCrit =
                statsAttacking.MainStats[StatType.Crit] - statsDefending.MainStats[StatType.Parry];
            probabilityCrit = Math.Clamp(probabilityCrit, 0, statsAttacking.MainStats[StatType.Crit]);

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            if (result < probabilityCrit)
                damage += damage * (statsAttacking.MainStats[StatType.CritDamage] / ConstantValues.MAX_PERCENTAGE);

            return result < probabilityCrit;
        }

        private bool TryStun(CharacterStats statsAttacking, CharacterData dataDefending)
        {
            float probabilityStun =
                statsAttacking.MainStats[StatType.Stun] - dataDefending.Stats.MainStats[StatType.Strength];
            probabilityStun = Math.Clamp(probabilityStun, 0, statsAttacking.MainStats[StatType.Stun]);

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            if (result < probabilityStun)
                dataDefending.ActivateStun(statsAttacking.MainStats[StatType.StunDuration]);

            return result < probabilityStun;
        }

        private bool TryDead(CharacterData dataDefending)
        {
            if (dataDefending.Stats.CurrentStats.Health != 0)
                return false;

            dataDefending.StartDead();
            return true;
        }

        private void Reset()
        {
            _isDoubleDamage = false;
            _isVampireDamage = false;
        }
    }
}