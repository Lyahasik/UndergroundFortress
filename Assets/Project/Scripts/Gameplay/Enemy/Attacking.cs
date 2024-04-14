using UnityEngine;

using UndergroundFortress.Gameplay.Stats.Services;

namespace UndergroundFortress.Gameplay.Character
{
    public class Attacking : MonoBehaviour
    {
        private IAttackService _attackService;
        private ICheckerCurrentStatsService _checkerCurrentStatsService;
        private CharacterData _characterData;
        private CharacterData _targetCharacterData;

        public void Construct(IAttackService attackService,
            ICheckerCurrentStatsService checkerCurrentStatsService,
            CharacterData characterData,
            CharacterData targetCharacterData)
        {
            _checkerCurrentStatsService = checkerCurrentStatsService;
            _attackService = attackService;
            _characterData = characterData;
            _targetCharacterData = targetCharacterData;
        }

        private void Update()
        {
            TryAttack();
        }

        private void TryAttack()
        {
            if (_characterData == null
                || _characterData.Stats.IsFreeze
                || !_checkerCurrentStatsService.IsEnoughStamina(_characterData.Stats))
                return;

            _attackService.Attack(_characterData, _targetCharacterData);
        }
    }
}