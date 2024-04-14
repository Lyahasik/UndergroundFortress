using UnityEngine;

using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats.Services;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public class ProgressDungeonService : IProgressDungeonService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IAttackService _attackService;
        private readonly IStatsRestorationService _statsRestorationService;
        private readonly ICheckerCurrentStatsService _checkerCurrentStatsService;

        private Canvas _gameplayCanvas;
        private AttackArea _attackArea;
        private PlayerData _playerData;

        private EnemyData _currentEnemy;

        public ProgressDungeonService(IStaticDataService staticDataService,
            IGameplayFactory gameplayFactory,
            IAttackService attackService,
            IStatsRestorationService statsRestorationService,
            ICheckerCurrentStatsService checkerCurrentStatsService)
        {
            _staticDataService = staticDataService;
            _gameplayFactory = gameplayFactory;
            _attackService = attackService;
            _statsRestorationService = statsRestorationService;
            _checkerCurrentStatsService = checkerCurrentStatsService;
        }
        
        public void Initialize(Canvas gameplayCanvas, PlayerData playerData)
        {
            _gameplayCanvas = gameplayCanvas;
            _playerData = playerData;
            
            _attackArea = _gameplayFactory.CreateAttackArea(gameplayCanvas.transform);
            _attackArea.Construct(
                _playerData,
                _checkerCurrentStatsService,
                _attackService);
        }

        public void StartBattle()
        {
            CreateEnemy();
        }

        private void CreateEnemy()
        {
            CharacterStaticData enemyStaticData = _staticDataService.ForEnemy();
            var enemyStats = new CharacterStats();
            enemyStats.Initialize(enemyStaticData);
            
            _currentEnemy = _gameplayFactory.CreateEnemy(_gameplayCanvas.transform);
            _currentEnemy.Construct(enemyStats, _statsRestorationService);
            _currentEnemy.Initialize(_attackService, _checkerCurrentStatsService, _playerData);
            _currentEnemy.transform.SetSiblingIndex(0);
            
            _attackArea.Activate(_currentEnemy);
        }
    }
}