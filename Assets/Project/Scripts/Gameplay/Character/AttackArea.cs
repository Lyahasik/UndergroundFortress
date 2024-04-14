using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Stats.Services;

namespace UndergroundFortress.Gameplay.Character
{
    [RequireComponent(typeof(Image))]
    public class AttackArea : MonoBehaviour, IPointerDownHandler
    {
        private PlayerData _playerData;

        private ICheckerCurrentStatsService _checkerCurrentStatsService;
        private IAttackService _attackService;
        
        private EnemyData _enemyData;
        private bool _isActive;

        public void Construct(PlayerData playerData,
            ICheckerCurrentStatsService checkerCurrentStatsService,
            IAttackService attackService)
        {
            _playerData = playerData;
            _checkerCurrentStatsService = checkerCurrentStatsService;
            _attackService = attackService;
        }

        public void Activate(EnemyData enemyData)
        {
            _enemyData = enemyData;
            _isActive = true;
        }

        public void Deactivate()
        {
            _enemyData = null;
            _isActive = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isActive)
                return;
            
            Attack();
        }

        private void Attack()
        {
            if (_playerData.Stats.IsFreeze
                || !_checkerCurrentStatsService.IsEnoughStamina(_playerData.Stats))
                return;
            
            _attackService.Attack(_playerData, _enemyData);
        }
    }
}
