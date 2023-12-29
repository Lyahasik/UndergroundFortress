using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UndergroundFortress.Scripts.Gameplay.Stats.Services;

namespace UndergroundFortress.Scripts.Gameplay.Character
{
    [RequireComponent(typeof(Image))]
    public class AttackArea : MonoBehaviour, IPointerDownHandler
    {
        private CharacterStats _playerStats;
        private CharacterStats _enemyStats;

        private ICheckerCurrentStatsService _checkerCurrentStatsService;
        private IAttackService _attackService;

        public void Construct(CharacterStats playerStats,
            CharacterStats enemyStats,
            ICheckerCurrentStatsService checkerCurrentStatsService,
            IAttackService attackService)
        {
            _playerStats = playerStats;
            _enemyStats = enemyStats;
            _checkerCurrentStatsService = checkerCurrentStatsService;
            _attackService = attackService;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Attack();
        }

        private void Attack()
        {
            if (!_checkerCurrentStatsService.IsEnoughStamina(_playerStats))
                return;
            
            _attackService.Attack(_enemyStats, _playerStats.MainStats.damage);
        }
    }
}
