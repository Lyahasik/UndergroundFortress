using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Stats.Services;

namespace UndergroundFortress.Gameplay.Character
{
    [RequireComponent(typeof(Image))]
    public class AttackArea : MonoBehaviour, IPointerDownHandler
    {
        private CharacterData _playerData;
        private CharacterData _enemyData;

        private ICheckerCurrentStatsService _checkerCurrentStatsService;
        private IAttackService _attackService;

        public void Construct(CharacterData playerData,
            CharacterData enemyData,
            ICheckerCurrentStatsService checkerCurrentStatsService,
            IAttackService attackService)
        {
            _playerData = playerData;
            _enemyData = enemyData;
            _checkerCurrentStatsService = checkerCurrentStatsService;
            _attackService = attackService;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Attack();
        }

        private void Attack()
        {
            if (!_checkerCurrentStatsService.IsEnoughStamina(_playerData.Stats))
                return;
            
            _attackService.Attack(_playerData, _enemyData);
        }
    }
}
