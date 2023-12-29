using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UndergroundFortress.Scripts.Gameplay.Characteristics;
using UndergroundFortress.Scripts.Gameplay.Characteristics.Services;

namespace UndergroundFortress.Scripts.Gameplay.Character
{
    [RequireComponent(typeof(Image))]
    public class AttackArea : MonoBehaviour, IPointerDownHandler
    {
        private RealtimeCharacteristics _realtimeCharacteristicsPlayer;
        private ICheckerCurrentCharacteristicsService _checkerCurrentCharacteristicsService;
        private IAttackService _attackService;

        public void Construct(RealtimeCharacteristics realtimeCharacteristicsPlayer,
            ICheckerCurrentCharacteristicsService checkerCurrentCharacteristicsService,
            IAttackService attackService)
        {
            _realtimeCharacteristicsPlayer = realtimeCharacteristicsPlayer;
            _checkerCurrentCharacteristicsService = checkerCurrentCharacteristicsService;
            _attackService = attackService;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("UP");
            Attack();
        }

        private void Attack()
        {
            if (!_checkerCurrentCharacteristicsService.IsEnoughStamina(_realtimeCharacteristicsPlayer))
                return;
            
            _attackService.AttackEnemy(_realtimeCharacteristicsPlayer.Damage);
        }
    }
}
