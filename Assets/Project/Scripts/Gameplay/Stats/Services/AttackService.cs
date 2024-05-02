using System;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.StaticData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class AttackService : IAttackService
    {
        private readonly IStatsWasteService _statsWasteService;
        private readonly IStatsRestorationService _statsRestorationService;
        private readonly ISkillsUpgradeService _skillsUpgradeService;
        private readonly IProcessingBonusesService _processingBonusesService;

        private bool _isDoubleDamage;
        private bool _isVampireDamage;

        public bool IsDoubleDamage => _isDoubleDamage;
        public bool IsVampireDamage => _isVampireDamage;

        public AttackService(IStatsWasteService statsWasteService,
            IStatsRestorationService statsRestorationService,
            ISkillsUpgradeService skillsUpgradeService,
            IProcessingBonusesService processingBonusesService)
        {
            _statsWasteService = statsWasteService;
            _statsRestorationService = statsRestorationService;
            _skillsUpgradeService = skillsUpgradeService;
            _processingBonusesService = processingBonusesService;
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void Attack(CharacterData dataAttacking,
            CharacterData dataDefending,
            in float staminaCost,
            bool isPlayerAttack = false)
        {
            CharacterStats statsAttacking = dataAttacking.Stats;
            CharacterStats statsDefending = dataDefending.Stats;

            if (!TryHit(statsAttacking, statsDefending, isPlayerAttack))
            {
                if (!isPlayerAttack || !_processingBonusesService.IsBuffActivate(BonusType.InfiniteStamina))
                    _statsWasteService.WasteStamina(statsAttacking, staminaCost);
                
                dataAttacking.AttackEffect(StatType.Dodge);
                dataDefending.TakeHitEffect(StatType.Dodge);
                if (!isPlayerAttack)
                    _skillsUpgradeService.UpdateProgressSkill(SkillsType.Dodge, StatType.Dodge);
                
                return;
            }
            
            if (isPlayerAttack)
                _skillsUpgradeService.UpdateProgressSkill(SkillsType.Dodge, StatType.Accuracy);

            float damage = statsAttacking.MainStats[StatType.Damage];
            if (isPlayerAttack)
                damage += _processingBonusesService.GetBuffValue(BonusType.IncreasedDamage);
            
            if (isPlayerAttack && _isDoubleDamage)
            {
                damage *= 2;
                _isDoubleDamage = false;
            }
            damage = Math.Clamp(damage - statsDefending.MainStats[StatType.Defense], ConstantValues.MIN_DAMAGE, float.MaxValue);

            if (TryApplyCrit(statsAttacking, statsDefending, ref damage, isPlayerAttack))
            {
                dataAttacking.AttackEffect(StatType.Crit);
                if (isPlayerAttack)
                    _skillsUpgradeService.UpdateProgressSkill(SkillsType.Crit, StatType.Crit);
            }
            else
            {
                if (!isPlayerAttack)
                    _skillsUpgradeService.UpdateProgressSkill(SkillsType.Crit, StatType.Parry);
            }

            if (TryBreakThrough(statsAttacking, statsDefending, ref damage, isPlayerAttack))
            {
                dataDefending.TakeHitEffect(StatType.Damage, (int) damage);
                if (isPlayerAttack)
                    _skillsUpgradeService.UpdateProgressSkill(SkillsType.Block, StatType.BreakThrough);
            }
            else
            {
                dataDefending.TakeHitEffect(StatType.Block, (int) damage);
                if (!isPlayerAttack)
                    _skillsUpgradeService.UpdateProgressSkill(SkillsType.Block, StatType.Block);
            }

            _statsWasteService.WasteHealth(statsDefending, (int) damage);
            if (!isPlayerAttack || !_processingBonusesService.IsBuffActivate(BonusType.InfiniteStamina))
                _statsWasteService.WasteStamina(statsAttacking, staminaCost);
            
            if (isPlayerAttack && _isVampireDamage)
            {
                _statsRestorationService.RestoreHealth(statsAttacking, (int) (damage * 0.5f));
            }

            if (TryDead(dataDefending))
            {
                if (!isPlayerAttack)
                    Reset();

                return;
            }

            if (TryStun(statsAttacking, dataDefending, isPlayerAttack))
            {
                dataAttacking.AttackEffect(StatType.Stun);
                if (isPlayerAttack)
                    _skillsUpgradeService.UpdateProgressSkill(SkillsType.Stun, StatType.Stun);
                
                dataDefending.TakeHitEffect(StatType.Stun);
            }
            else
            {
                if (!isPlayerAttack)
                    _skillsUpgradeService.UpdateProgressSkill(SkillsType.Stun, StatType.Strength);
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

        private bool TryHit(CharacterStats statsAttacking, CharacterStats statsDefending, bool isPlayerAttack)
        {
            float attackedProbability = statsDefending.MainStats[StatType.Dodge];
            if (!isPlayerAttack)
                attackedProbability += _processingBonusesService.GetBuffValue(BonusType.IncreasedDodge);
            
            float probabilityMiss = attackedProbability - statsAttacking.MainStats[StatType.Accuracy];
            probabilityMiss = Math.Clamp(probabilityMiss, 0, Math.Abs(attackedProbability));

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            return result >= probabilityMiss;
        }

        private bool TryBreakThrough(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage, bool isPlayerAttack)
        {
            float attackedProbability = statsDefending.MainStats[StatType.Block];
            if (!isPlayerAttack)
                attackedProbability += _processingBonusesService.GetBuffValue(BonusType.IncreasedBlock);
            
            float probabilityBlock = attackedProbability - statsAttacking.MainStats[StatType.BreakThrough];
            probabilityBlock = Math.Clamp(probabilityBlock, 0, Math.Abs(attackedProbability));

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            if (result < probabilityBlock)
                damage -= damage * (statsDefending.MainStats[StatType.BlockAttackDamage] / ConstantValues.MAX_PERCENTAGE);

            return result >= probabilityBlock;
        }

        private bool TryApplyCrit(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage, bool isPlayerAttack)
        {
            float attackedProbability = statsAttacking.MainStats[StatType.Crit];
            if (isPlayerAttack)
                attackedProbability += _processingBonusesService.GetBuffValue(BonusType.IncreasedCrit);
            
            float probabilityCrit = attackedProbability - statsDefending.MainStats[StatType.Parry];
            probabilityCrit = Math.Clamp(probabilityCrit, 0, Math.Abs(attackedProbability));

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            if (result < probabilityCrit)
                damage += damage * (statsAttacking.MainStats[StatType.CritDamage] / ConstantValues.MAX_PERCENTAGE);

            return result < probabilityCrit;
        }

        private bool TryStun(CharacterStats statsAttacking, CharacterData dataDefending, bool isPlayerAttack)
        {
            float attackedProbability = statsAttacking.MainStats[StatType.Stun];
            if (isPlayerAttack)
                attackedProbability += _processingBonusesService.GetBuffValue(BonusType.IncreasedStun);
            
            float probabilityStun = attackedProbability - dataDefending.Stats.MainStats[StatType.Strength];
            probabilityStun = Math.Clamp(probabilityStun, 0, Math.Abs(attackedProbability));

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