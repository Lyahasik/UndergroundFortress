using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.Gameplay.Stats.Services;
using UnityEngine;

namespace UndergroundFortress.Gameplay.Character
{
    public class CurrentStaminaFillView : CurrentStatFillView
    {
        [Space]
        [SerializeField] private RelativePositionBetweenPointsView minimalCastMarker;
        [SerializeField] private Color shortageColor;

        private ICheckerCurrentStatsService _checkerCurrentStatsService;

        private bool _isInit;
        private Color _baseColor;

        protected override void Awake()
        {
            base.Awake();

            _baseColor = fill.color;
        }

        public void Construct(ICheckerCurrentStatsService checkerCurrentStatsService)
        {
            _checkerCurrentStatsService = checkerCurrentStatsService;
        }

        public void Initialize(CharacterStats characterStats)
        {
            minimalCastMarker.UpdatePosition(characterStats.MainStats[StatType.StaminaCost] / characterStats.MainStats[StatType.Stamina]);
            _isInit = true;
        }

        public override void UpdateValue(CharacterStats characterStats)
        {
            if (!_isInit)
                return;
            
            base.UpdateValue(characterStats);

            fill.color = _checkerCurrentStatsService.IsEnoughStamina(characterStats) ? _baseColor : shortageColor;
        }
    }
}