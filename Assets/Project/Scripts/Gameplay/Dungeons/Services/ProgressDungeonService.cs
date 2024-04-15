using System.Linq;
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

        private int _currentDungeonId;
        private int _currentLevelId;
        private DungeonLevelStaticData _currentLevel;

        private int _currentStage;
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
        
        public void Initialize(Canvas gameplayCanvas, PlayerData playerData, int dungeonId, int levelId)
        {
            _gameplayCanvas = gameplayCanvas;
            _playerData = playerData;
            _currentDungeonId = dungeonId;
            _currentLevelId = levelId;
            
            _attackArea = _gameplayFactory.CreateAttackArea(gameplayCanvas.transform);
            _attackArea.Construct(
                _playerData,
                _checkerCurrentStatsService,
                _attackService);
        }

        public void StartBattle()
        {
            _currentLevel = _staticDataService.GetDungeonById(_currentDungeonId).levels.Find(data => data.id == _currentLevelId);
            
            CreateEnemy(GetEnemyData());
        }

        private void CreateEnemy(EnemyStaticData enemyStaticData)
        {
            _currentStage++;
            
            var enemyStats = new CharacterStats();
            enemyStats.Initialize(enemyStaticData);
            
            _currentEnemy = _gameplayFactory.CreateEnemy(_gameplayCanvas.transform);
            _currentEnemy.Construct(
                enemyStats,
                _statsRestorationService,
                _attackService,
                _checkerCurrentStatsService,
                _playerData,
                StartDeadEnemy,
                DeadEnemy,
                _attackArea.Activate);
            _currentEnemy.Initialize();
            _currentEnemy.transform.SetSiblingIndex(0);
        }

        private EnemyStaticData GetEnemyData()
        {
            int totalWeight = _currentLevel.enemies.Sum(data => data.probabilityWeight);
            int accident = Random.Range(0, totalWeight + 1);

            foreach (SpawnEnemyStaticData spawnEnemyStaticData in _currentLevel.enemies)
            {
                accident -= spawnEnemyStaticData.probabilityWeight;
                if (accident < 0)
                    return spawnEnemyStaticData.enemyStaticData;
            }
            
            Debug.LogWarning($"[ProgressDungeonService] Not found enemy by probability weight.");
            return _currentLevel.enemies[0].enemyStaticData;
        }

        private void StartDeadEnemy()
        {
            _attackArea.Deactivate();
        }

        private void DeadEnemy()
        {
            _currentEnemy = null;

            if (_currentStage < _currentLevel.numberStages)
            {
                CreateEnemy(GetEnemyData());
            }
            else
            {
                _currentStage = 0;
                
                Debug.Log("End level");
            }
        }
    }
}